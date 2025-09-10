namespace BuildOpsPlatform.ServicesCommon.Constants
{
    public class RabbitMqConstants
    {
        // RabbitMQ configuration
        // RabbitMQ exchange names
        public const string RABBITMQ_EXCHANGE_USER_CREATED = "user.created.exchange";

        // RabbitMQ queue names
        public const string RABBITMQ_QUEUE_USER_CREATED = "user.created.queue";

        // RabbitMQ credentials
        public const string RABBITMQ_HOST = "RabbitMQ:Host";
        public const string RABBITMQ_USER = "RabbitMQ:User";
        public const string RABBITMQ_PASSWORD = "RabbitMQ:Password";
    }
}
