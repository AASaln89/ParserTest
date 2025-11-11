using BuildOpsPlatform.RevitDataCommon.Protos;
using BuildOpsPlatform.RevitDataService.DbContexts;
using BuildOpsPlatform.RevitDataService.Models;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.EntityFrameworkCore;

namespace BuildOpsPlatform.RevitDataService.Consumers
{
    public class KafkaRevitConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<KafkaRevitConsumer> _log;
        private readonly string _bootstrap;
        private readonly string _schemaRegistryUrl;
        private readonly string _topic;
        private readonly string _groupId;

        private IConsumer<string?, RevitProjectDataMessage> _consumer = default!;
        private CachedSchemaRegistryClient _schemaRegistry = default!;

        public KafkaRevitConsumer(
            IServiceScopeFactory scopeFactory,
            ILogger<KafkaRevitConsumer> log,
            IConfiguration cfg)
        {
            _scopeFactory = scopeFactory;
            _log = log;

            _bootstrap = cfg["Kafka:BootstrapServers"] ?? "localhost:9092";
            _schemaRegistryUrl = cfg["Kafka:SchemaRegistryUrl"] ?? "http://localhost:8081";
            _topic = cfg["Kafka:Topic"] ?? "revit.project.data.v1";
            _groupId = cfg["Kafka:GroupId"] ?? "revit.consumer.v1";
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var cCfg = new ConsumerConfig
            {
                BootstrapServers = _bootstrap,
                GroupId = _groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false,       // коммитим вручную после успешной записи в БД
                EnableAutoOffsetStore = false,
                // при больших сообщениях можно подстроить max fetch:
                // MaxPartitionFetchBytes = 5 * 1024 * 1024,
                // FetchMaxBytes = 20 * 1024 * 1024
            };

            _schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            {
                Url = _schemaRegistryUrl
            });

            // Прото-десериалайзер. Важно: тип должен быть IMessage<T>
            var deserializer = new ProtobufDeserializer<RevitProjectDataMessage>().AsSyncOverAsync();

            _consumer = new ConsumerBuilder<string?, RevitProjectDataMessage>(cCfg)
                .SetValueDeserializer(deserializer)
                .SetErrorHandler((_, e) => _log.LogError("Kafka error: {Err}", e))
                .Build();

            _consumer.Subscribe(_topic);

            _ = Task.Run(() => RunLoop(stoppingToken), stoppingToken);
            _log.LogInformation("KafkaRevitConsumer started. Topic={Topic}, GroupId={Group}", _topic, _groupId);

            return Task.CompletedTask;
        }

        private async Task RunLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                ConsumeResult<string?, RevitProjectDataMessage>? cr = null;
                try
                {
                    cr = _consumer.Consume(ct);
                    if (cr == null || cr.Message?.Value == null) continue;

                    var msg = cr.Message.Value;

                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<RevitDataDbContext>();

                    // ----------------------
                    // Ниже — адаптация твоей логики записи
                    // ----------------------
                    var snapshot = msg.Snapshot;
                    if (snapshot == null)
                    {
                        _log.LogWarning("Skip message without Snapshot. TPO={TPO}", cr.TopicPartitionOffset);
                        _consumer.StoreOffset(cr);
                        _consumer.Commit();
                        continue;
                    }

                    var documentId = snapshot.DocumentId;
                    var snapshotId = snapshot.Id;

                    // Документ
                    if (await db.RvtDocuments.FirstOrDefaultAsync(x => x.Id == documentId, ct) == null)
                    {
                        db.RvtDocuments.Add(new RvtDocument { Id = documentId });
                        await db.SaveChangesAsync(ct);
                    }

                    // Снапшот
                    if (await db.Snapshots.FirstOrDefaultAsync(x => x.Id == Guid.Parse(snapshotId), ct) == null)
                    {
                        db.Snapshots.Add(new RvtSnapshot
                        {
                            Id = Guid.Parse(snapshotId),
                            DocumentId = documentId,
                            UploadDate = snapshot.UploadDateUnixMs.ToDateTime(),
                        });
                        await db.SaveChangesAsync(ct);
                    }

                    // Categories
                    if (msg.Categories != null && msg.Categories.Count > 0)
                    {
                        foreach (var c in msg.Categories)
                        {
                            db.Categories.Add(new Category
                            {
                                CategoryId = c.CategoryId,
                                CategoryType = c.CategoryType,
                                Name = c.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Levels
                    if (msg.Levels != null && msg.Levels.Count > 0)
                    {
                        foreach (var l in msg.Levels)
                        {
                            db.Levels.Add(new Level
                            {
                                LevelId = l.LevelId,
                                LevelUniqueId = l.LevelUniqueId,
                                Name = l.Name,
                                ElevationValue = l.ElevationValue,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Worksets
                    if (msg.Worksets != null && msg.Worksets.Count > 0)
                    {
                        foreach (var w in msg.Worksets)
                        {
                            db.Worksets.Add(new Workset
                            {
                                WorksetId = w.WorksetId,
                                WorksetUniqueId = w.WorksetUniqueId,
                                Name = w.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Grids
                    if (msg.Grids != null && msg.Grids.Count > 0)
                    {
                        foreach (var g in msg.Grids)
                        {
                            db.Grids.Add(new Grid
                            {
                                GridId = g.GridId,
                                GridUniqueId = g.GridUniqueId,
                                Name = g.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Sites
                    if (msg.Sites != null && msg.Sites.Count > 0)
                    {
                        foreach (var s in msg.Sites)
                        {
                            db.Sites.Add(new Site
                            {
                                SiteId = s.SiteId,
                                SiteUniqueId = s.SiteUniqueId,
                                Name = s.Name,
                                Latitude = s.Latitude,
                                Longitude = s.Longitude,
                                City = s.City,
                                BasePointElevetionValue = s.BasePointElevetionValue,
                                BasePointEastWest = s.BasePointEastWest,
                                BasePointNorthSouth = s.BasePointNorthSouth,
                                BasePointAngle = s.BasePointAngle,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Stages
                    if (msg.Stages != null && msg.Stages.Count > 0)
                    {
                        foreach (var ph in msg.Stages)
                        {
                            db.Stages.Add(new Stage
                            {
                                StageId = ph.StageId,
                                StageUniqueId = ph.StageUniqueId,
                                Name = ph.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // DesignOptions
                    if (msg.DesignOptions != null && msg.DesignOptions.Count > 0)
                    {
                        foreach (var d in msg.DesignOptions)
                        {
                            db.DesignOptions.Add(new DesignOption
                            {
                                DesignOptionId = d.DesignOptionId,
                                DesignOptionUniqueId = d.DesignOptionUniqueId,
                                Name = d.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Materials
                    if (msg.Materials != null && msg.Materials.Count > 0)
                    {
                        foreach (var m in msg.Materials)
                        {
                            db.Materials.Add(new Material
                            {
                                MaterialId = m.MaterialId,
                                MaterialUniqueId = m.MaterialUniqueId,
                                Name = m.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Views
                    if (msg.Views != null && msg.Views.Count > 0)
                    {
                        foreach (var v in msg.Views)
                        {
                            db.Views.Add(new View
                            {
                                ViewId = v.ViewId,
                                ViewUniqueId = v.ViewUniqueId,
                                Name = v.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Parameters
                    if (msg.Parameters != null && msg.Parameters.Count > 0)
                    {
                        foreach (var p in msg.Parameters)
                        {
                            db.Parameters.Add(new Parameter
                            {
                                Id = Guid.Parse(p.Id),
                                ParameterId = p.ParameterId,
                                ParameterGUID = Guid.Parse(p.ParameterGuid),
                                Name = p.Name,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // Elements
                    if (msg.Elements != null && msg.Elements.Count > 0)
                    {
                        var skippedCats = new List<int>();

                        // Предзагрузка категорий для снапшота, чтобы не бить БД каждым элементом
                        var knownCats = await db.Categories
                            .AsNoTracking()
                            .Where(x => x.RvtSnapshotId == Guid.Parse(snapshotId))
                            .Select(x => x.CategoryId)
                            .ToListAsync(ct);

                        foreach (var e in msg.Elements)
                        {
                            if (!knownCats.Contains(e.CategoryId))
                            {
                                skippedCats.Add(e.CategoryId);
                                continue;
                            }

                            db.Elements.Add(new Element
                            {
                                ElementId = e.ElementId,
                                ElementUniqueId = e.ElementUniqueId,
                                TypeName = e.TypeName,
                                RvtSnapshotId = Guid.Parse(snapshotId),
                                CategoryId = e.CategoryId
                            });
                        }
                        if (skippedCats.Count > 0)
                            _log.LogWarning("Missing categories for snapshot {SnapshotId}: {List}", snapshotId, string.Join(",", skippedCats.Distinct()));

                        await db.SaveChangesAsync(ct);
                    }

                    // ElementValues
                    if (msg.ElementValues != null && msg.ElementValues.Count > 0)
                    {
                        var existingElementIds = await db.Elements
                            .AsNoTracking()
                            .Where(x => x.RvtSnapshotId == Guid.Parse(snapshotId))
                            .Select(x => x.ElementId)
                            .ToListAsync(ct);

                        var missingElems = new HashSet<int>();
                        foreach (var ev in msg.ElementValues)
                        {
                            if (!existingElementIds.Contains(ev.ElementId))
                            {
                                missingElems.Add(ev.ElementId);
                                continue;
                            }

                            db.ParameterValues.Add(new ElementParameterValue
                            {
                                ParameterDbId = Guid.Parse(ev.ParameterDbId),
                                Value = ev.Value,
                                ElementId = ev.ElementId,
                                IsTypeParameter = ev.IsTypeParameter,
                                IsProject = ev.IsProject,
                                IsShared = ev.IsShared,
                                IsSystem = ev.IsSystem,
                                HasValue = ev.HasValue,
                                ParameterGUID = Guid.Parse(ev.ParameterGuid),
                                ParameterId = ev.ParameterId,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        if (missingElems.Count > 0)
                            _log.LogWarning("Missing Elements for snapshot {SnapshotId}: {List}", snapshotId, string.Join(",", missingElems));

                        await db.SaveChangesAsync(ct);
                    }

                    // ElementViews
                    if (msg.ElementViews != null && msg.ElementViews.Count > 0)
                    {
                        var ids = await db.Elements
                            .AsNoTracking()
                            .Where(x => x.RvtSnapshotId == Guid.Parse(snapshotId))
                            .Select(x => x.ElementId)
                            .ToListAsync(ct);

                        var missed = new List<int>();
                        foreach (var ev in msg.ElementViews)
                        {
                            if (!ids.Contains(ev.ElementId))
                            {
                                missed.Add(ev.ElementId);
                                continue;
                            }
                            db.ElementViews.Add(new ElementView
                            {
                                ViewId = ev.ViewId,
                                ElementId = ev.ElementId,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        if (missed.Count > 0)
                            _log.LogWarning("Missing Elements for ElementViews snapshot {SnapshotId}: {List}", snapshotId, string.Join(",", missed));

                        await db.SaveChangesAsync(ct);
                    }

                    // Errors
                    if (msg.Errors != null && msg.Errors.Count > 0)
                    {
                        foreach (var er in msg.Errors)
                        {
                            db.Errors.Add(new BuildOpsPlatform.RevitDataService.Models.Error
                            {
                                ErrorId = Guid.Parse(er.ErrorId),
                                Message = er.Message,
                                RvtSnapshotId = Guid.Parse(snapshotId)
                            });
                        }
                        await db.SaveChangesAsync(ct);
                    }

                    // ElementErrors
                    if (msg.ElementErrors != null && msg.ElementErrors.Count > 0)
                    {
                        var requestedIds = msg.ElementErrors.Select(e => e.ElementId).Distinct().ToArray();
                        var existingIds = await db.Elements
                            .AsNoTracking()
                            .Where(e => e.RvtSnapshotId == Guid.Parse(snapshotId) && requestedIds.Contains(e.ElementId))
                            .Select(e => e.ElementId)
                            .ToListAsync(ct);

                        var toInsert = new List<ElementError>();
                        var missing = new HashSet<int>();

                        foreach (var err in msg.ElementErrors)
                        {
                            if (existingIds.Contains(err.ElementId))
                            {
                                toInsert.Add(new ElementError
                                {
                                    RvtSnapshotId = Guid.Parse(snapshotId),
                                    ElementId = err.ElementId,
                                    ErrorId = Guid.Parse(err.ErrorId) 
                                });
                            }
                            else
                            {
                                missing.Add(err.ElementId);
                            }
                        }

                        if (toInsert.Count > 0)
                        {
                            db.ElementErrors.AddRange(toInsert);
                            await db.SaveChangesAsync(ct);
                        }

                        if (missing.Count > 0)
                            _log.LogWarning("Missing Elements for snapshot {SnapshotId}: {List}", snapshotId, string.Join(",", missing));
                    }

                    // Успех → сохраняем и коммитим оффсет
                    _consumer.StoreOffset(cr);
                    _consumer.Commit();
                }
                catch (OperationCanceledException)
                {
                    // остановка
                    break;
                }
                catch (ConsumeException ex)
                {
                    _log.LogError(ex, "ConsumeException at TPO={TPO}", cr?.TopicPartitionOffset);
                    // при consume error оффсет обычно не коммитим, пусть ретраится
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, "Processing error at TPO={TPO}", cr?.TopicPartitionOffset);

                    // ⚠️ продакшен-вариант: отправить в DLT-топик и СКОММИТИТЬ оффсет,
                    // чтобы не зациклиться на «ядовитом» сообщении.
                    // Пример:
                    // await SendToDltAsync(cr, ex, ct);
                    // _consumer.StoreOffset(cr); _consumer.Commit();
                }
            }

            try { _consumer.Close(); } catch { /* ignore */ }
            _consumer.Dispose();
            _schemaRegistry.Dispose();
        }
    }
}
