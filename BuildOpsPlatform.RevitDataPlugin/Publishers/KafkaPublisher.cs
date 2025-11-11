using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Google.Protobuf;
using Newtonsoft.Json;
using System;
using System.Text;

namespace BuildOpsPlatform.RevitDataPlugin.Publishers
{
    using Confluent.Kafka;
    using Confluent.SchemaRegistry;
    using Confluent.SchemaRegistry.Serdes;
    using Google.Protobuf;
    using System.Collections.Generic;
    using System.Text;

    public sealed class KafkaPublisher<TProto> : Publisher, IDisposable
        where TProto : class, IMessage<TProto>, new()
    {
        private readonly IProducer<string?, TProto> _producer;
        private readonly CachedSchemaRegistryClient _schemaRegistryClient;

        public KafkaPublisher(
            string topic,
            string bootstrapServers = "localhost:9092",
            string schemaRegistryUrl = "http://localhost:8081",
            Action<ProducerConfig>? configure = null,
            SubjectNameStrategy subjectNameStrategy = SubjectNameStrategy.TopicRecord)
            : base(topic, bootstrapServers, 9092)
        {
            // Базовый конфиг продьюсера (надёжность + throughput)
            var cfg = new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All,
                EnableIdempotence = true,
                MaxInFlight = 5,
                MessageSendMaxRetries = 10,
                MessageTimeoutMs = 120_000,
                LingerMs = 30,
                BatchSize = 512 * 1024,
                CompressionType = CompressionType.Zstd
            };
            configure?.Invoke(cfg);

            // Клиент Schema Registry
            var srCfg = new SchemaRegistryConfig { Url = schemaRegistryUrl };
            _schemaRegistryClient = new CachedSchemaRegistryClient(srCfg);

            // Прото-сериалайзер, пишет/читает схему в Registry
            var serializer = new ProtobufSerializer<TProto>(
                _schemaRegistryClient,
                new ProtobufSerializerConfig { SubjectNameStrategy = subjectNameStrategy });

            _producer = new ProducerBuilder<string?, TProto>(cfg)
                .SetValueSerializer(serializer)
                .SetErrorHandler((_, e) =>
                {
                    // сюда свой логгер/метрики
                    if (e.IsFatal) Console.Error.WriteLine($"[Kafka FATAL] {e.Reason}");
                })
                .Build();
        }

        /// Базовый контракт Publisher — принимает T, но мы ожидаем именно TProto.
        public override void Publish<T>(T data)
        {
            if (data is not TProto msg)
                throw new InvalidOperationException(
                    $"KafkaPublisher<{typeof(TProto).Name}> принимает только {typeof(TProto).Name}, а не {typeof(T).Name}");

            Publish(msg, key: null, headers: null);
        }

        /// Удобный оверлоад: можно передать ключ и заголовки.
        public void Publish(TProto message, string? key, IDictionary<string, string>? headers = null)
        {
            var kafkaHeaders = new Headers();
            if (headers != null)
            {
                foreach (var kv in headers)
                {
                    kafkaHeaders.Add(kv.Key, Encoding.UTF8.GetBytes(kv.Value));
                }
            }

            var msg = new Message<string?, TProto>
            {
                Key = key,       // ключ влияет на выбор партиции и порядок по сущности
                Value = message,
                Headers = kafkaHeaders
            };

            // неблокирующая отправка — клиент сам батчит по LingerMs/BatchSize
            _producer.Produce(Topic, msg, dr =>
            {
                if (dr.Error.IsError)
                    Console.Error.WriteLine($"[Kafka DeliveryError] {dr.Error.Reason}");
                // else: при желании логируй dr.TopicPartitionOffset
            });
        }

        /// Дожать буферы вручную (опционально)
        public void Flush(TimeSpan? timeout = null) =>
            _producer.Flush(timeout ?? TimeSpan.FromSeconds(30));

        public void Dispose()
        {
            try { Flush(); } catch { /* ignore */ }
            _producer.Dispose();
            _schemaRegistryClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
