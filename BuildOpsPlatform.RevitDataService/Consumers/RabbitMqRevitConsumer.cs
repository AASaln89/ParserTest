using BuildOpsPlatform.RevitDataCommon.Messaging;
using BuildOpsPlatform.RevitDataService.DbContexts;
using BuildOpsPlatform.RevitDataService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

namespace BuildOpsPlatform.RevitDataService.Consumers
{
    public class RabbitMqRevitConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "revit.project.data";

        public RabbitMqRevitConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<RevitProjectDataMessage>(json);
                if (message == null) return;
                if (message.Snapshot == null) return;

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<RevitDataDbContext>();

                var documentId = message.Snapshot.DocumentId;
                var snapshotId = message.Snapshot.Id;


                if (db.RvtDocuments.FirstOrDefault(x => x.Id == documentId) == null)
                {
                    var document = new RvtDocument
                    {
                        Id = message.Snapshot.DocumentId,
                    };

                    db.RvtDocuments.Add(document);
                    db.SaveChanges();
                }

                if (db.Snapshots.FirstOrDefault(x => x.Id == snapshotId) == null)
                {
                    var snapshot = new RvtSnapshot
                    {
                        Id = message.Snapshot.Id,
                        DocumentId = message.Snapshot.DocumentId,
                        UploadDate = message.Snapshot.UploadDate,
                    };

                    db.Snapshots.Add(snapshot);
                    db.SaveChanges();
                }

                if (message.Categories != null)
                {
                    foreach (var category in message.Categories)
                    {
                        db.Categories.Add(new Category
                        {
                            CategoryId = category.CategoryId,
                            CategoryType = category.CategoryType,
                            Name = category.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }

                    db.SaveChanges();
                }

                if (message.Levels != null)
                {
                    foreach (var level in message.Levels)
                    {
                        db.Levels.Add(new Level
                        {
                            LevelId = level.LevelId,
                            LevelUniqueId = level.LevelUniqueId,
                            Name = level.Name,
                            ElevationValue = level.ElevationValue,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Worksets != null)
                {
                    foreach (var workset in message.Worksets)
                    {
                        db.Worksets.Add(new Workset
                        {
                            WorksetId = workset.WorksetId,
                            WorksetUniqueId = workset.WorksetUniqueId,
                            Name = workset.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Grids != null)
                {
                    foreach (var grid in message.Grids)
                    {
                        db.Grids.Add(new Grid
                        {
                            GridId = grid.GridId,
                            GridUniqueId = grid.GridUniqueId,
                            Name = grid.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Sites != null)
                {
                    foreach (var site in message.Sites)
                    {
                        db.Sites.Add(new Site
                        {
                            SiteId = site.SiteId,
                            SiteUniqueId = site.SiteUniqueId,
                            Name = site.Name,
                            Latitude = site.Latitude,
                            Longitude = site.Longitude,
                            City = site.City,
                            BasePointElevetionValue = site.BasePointElevetionValue,
                            BasePointEastWest = site.BasePointEastWest,
                            BasePointNorthSouth = site.BasePointNorthSouth,
                            BasePointAngle = site.BasePointAngle,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Stages != null)
                {
                    foreach (var phase in message.Stages)
                    {
                        db.Stages.Add(new Stage
                        {
                            StageId = phase.StageId,
                            StageUniqueId = phase.StageUniqueId,
                            Name = phase.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.DesignOptions != null)
                {
                    foreach (var design in message.DesignOptions)
                    {
                        db.DesignOptions.Add(new DesignOption
                        {
                            DesignOptionId = design.DesignOptionId,
                            DesignOptionUniqueId = design.DesignOptionUniqueId,
                            Name = design.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Materials != null)
                {
                    foreach (var material in message.Materials)
                    {
                        db.Materials.Add(new Material
                        {
                            MaterialId = material.MaterialId,
                            MaterialUniqueId = material.MaterialUniqueId,
                            Name = material.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Views != null)
                {
                    foreach (var view in message.Views)
                    {
                        db.Views.Add(new View
                        {
                            ViewId = view.ViewId,
                            ViewUniqueId = view.ViewUniqueId,
                            Name = view.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Parameters != null)
                {
                    foreach (var parameter in message.Parameters)
                    {
                        db.Parameters.Add(new Parameter
                        {
                            Id = parameter.Id,
                            ParameterId = parameter.ParameterId,
                            ParameterGUID = parameter.ParameterGUID,
                            Name = parameter.Name,
                            RvtSnapshotId = snapshotId
                        });
                    }
                    db.SaveChanges();
                }

                if (message.Elements != null)
                {
                    var cats = new List<int>();
                    foreach (var element in message.Elements)
                    {
                        var strangeCat = db.Categories
                        .FirstOrDefault(x => x.RvtSnapshotId == snapshotId
                           && x.CategoryId == element.CategoryId);

                        if (strangeCat == null)
                        {
                            cats.Add(element.CategoryId);
                            continue;
                        }

                        db.Elements.Add(new Element
                        {
                            ElementId = element.ElementId,
                            ElementUniqueId = element.ElementUniqueId,
                            TypeName = element.TypeName,
                            RvtSnapshotId = snapshotId,
                            CategoryId = element.CategoryId
                        });
                    }
                    var numb = cats.Count;
                    db.SaveChanges();
                }

                if (message.ElementValues != null)
                {
                    var elemsIds = new HashSet<int>();
                    var elems = db.Elements.Select(x => x.ElementId).ToList();
                    var length = elems.Count;
                    foreach (var value in message.ElementValues)
                    {
                        if (!elems.Contains(value.ElementId))
                        {
                            elemsIds.Add(value.ElementId);
                            continue;
                        }
                        try
                        {
                            var param = new ElementParameterValue
                            {
                                ParameterDbId = value.ParameterDbId,
                                Value = value.Value,
                                //StorageType = value.StorageType,
                                ElementId = value.ElementId,
                                IsTypeParameter = value.IsTypeParameter,
                                IsProject = value.IsProject,
                                IsShared = value.IsShared,
                                IsSystem = value.IsSystem,
                                HasValue = value.HasValue,
                                //UnitType = param.Definition.ParameterType.ToString(),
                                ParameterGUID = value.ParameterGUID,
                                ParameterId = value.ParameterId,
                                RvtSnapshotId = snapshotId
                            };
                            db.ParameterValues.Add(param);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[WARN] Missing Elements for snapshot {ex}");
                            continue;
                        }
                        var exl = elemsIds.Count;
                    }
                    db.SaveChanges();
                }

                if (message.ElementViews != null)
                {
                    var ids = db.Elements
                    .Select(x => x.ElementId)
                    .ToList();

                    var missed = new List<int>();
                    foreach (var elementView in message.ElementViews)
                    {
                        if (!ids.Contains(elementView.ElementId))
                        {
                            missed.Add(elementView.ElementId);
                            continue;
                        }
                        db.ElementViews.Add(new ElementView
                        {
                            ViewId = elementView.ViewId,
                            ElementId = elementView.ElementId,
                            RvtSnapshotId = snapshotId
                        });
                    }

                    db.SaveChanges();
                }

                if (message.Errors != null)
                {
                    foreach (var error in message.Errors)
                    {
                        db.Errors.Add(new Error
                        {
                            ErrorId = error.ErrorId,
                            Message = error.Message,
                            RvtSnapshotId = snapshotId
                        });
                    }

                    db.SaveChanges();
                }

                if (message.ElementErrors != null)
                {
                    try
                    {
                        var snapId = snapshotId;

                        // Все ElementId из сообщения (уникальные)
                        var requestedIds = message.ElementErrors
                            .Select(e => e.ElementId)
                            .Distinct()
                            .ToArray();

                        // Какие из них реально есть в Elements для этого снапшота
                        var existingIds = db.Elements
                            .AsNoTracking()
                            .Where(e => e.RvtSnapshotId == snapId && requestedIds.Contains(e.ElementId))
                            .Select(e => e.ElementId)
                            .ToHashSet();

                        // Разделяем на валидные и «пустышки»
                        var toInsert = new List<ElementError>();
                        var missing = new HashSet<int>();

                        foreach (var err in message.ElementErrors)
                        {
                            if (existingIds.Contains(err.ElementId))
                            {
                                toInsert.Add(new ElementError
                                {
                                    RvtSnapshotId = snapId,
                                    ElementId = err.ElementId,
                                    ErrorId = err.ErrorId
                                });
                            }
                            else
                            {
                                missing.Add(err.ElementId);
                            }
                        }
                        // Записываем только валидные
                        if (toInsert.Count > 0)
                        {
                            db.ElementErrors.AddRange(toInsert);
                            db.SaveChanges();
                        }
                        // Лог по «пустышкам»
                        if (missing.Count > 0)
                        {
                            Console.WriteLine($"[WARN] Missing Elements for snapshot {snapId}: {string.Join(",", missing)}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[ERROR] Failed to process message: {ex.Message}");
                    }
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
