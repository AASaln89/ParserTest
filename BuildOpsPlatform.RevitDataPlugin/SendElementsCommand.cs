using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataCommon.Messaging;
using BuildOpsPlatform.RevitDataPlugin.Publishers;
using BuildOpsPlatform.RevitDataPlugin.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildOpsPlatform.RevitDataPlugin
{
    [Transaction(TransactionMode.Manual)]
    public class SendElementsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var doc = commandData.Application.ActiveUIDocument.Document;

            var dataService = new DataCollectionService();

            //Готовим сообщение
            var snapshot = new RvtSnapshotDto
            {
                UploadDate = DateTime.UtcNow,
                DocumentId = doc.Title,
                Id = Guid.NewGuid()
            };

            dataService.ExtractCategories(doc);
            dataService.ExtractViews(doc);

            var baseInfo = new RevitProjectDataMessage
            {
                Snapshot = snapshot,
                Categories = dataService.Categories,

                //Levels = dataService.ExtractLevels(doc),
                Worksets = dataService.ExtractWorksets(doc),
                Stages = dataService.ExtractPhases(doc),
                DesignOptions = dataService.ExtractDesignOptions(doc),
                Grids = dataService.ExtractGrids(doc),
                //Materials = materials,
                Views = dataService.Views,
                //Sites = dataService.ExtractSites(doc),
                //Parameters = dataService.Parameters,
            };

            // Отправляем в RabbitMQ
            using (var publisher = new RabbitPublisher("localhost"))
            {
                publisher.Publish(baseInfo);
            }

            dataService.ExtractElements(doc);
            dataService.ExtractErrors(doc);

            var parameters = new RevitProjectDataMessage
            {
                Snapshot = snapshot,
                Parameters = dataService.Parameters,
            };

            using (var publisher = new RabbitPublisher("localhost"))
            {
                publisher.Publish(parameters);
            }

            foreach (var chunk in ChunkBy(dataService.Elements, 40000))
            {
                var revitElements = new RevitProjectDataMessage
                {
                    Snapshot = snapshot,
                    Elements = chunk
                };

                using (var publisher = new RabbitPublisher("localhost"))
                {
                    publisher.Publish(revitElements);
                }

                Task.Delay(100).Wait();
            }

            //Task.Delay(10000).Wait();

            //var elementViews = new RevitProjectDataMessage
            //{
            //    Snapshot = snapshot,
            //    ElementViews = dataService.ElementViews
            //};

            //using (var publisher = new RabbitPublisher("localhost"))
            //{
            //    publisher.Publish(elementViews);
            //}

            Task.Delay(10000).Wait();
            // Значения элементов
            foreach (var chunk in ChunkBy(dataService.ElementsValues, 40000))
            {
                var elementsValues = new RevitProjectDataMessage
                {
                    Snapshot = snapshot,
                    ElementValues = chunk
                };

                using (var publisher = new RabbitPublisher("localhost"))
                {
                    publisher.Publish(elementsValues);
                }

                Task.Delay(100).Wait();
            }

            Task.Delay(10000).Wait();
            var errors = new RevitProjectDataMessage
            {
                Snapshot = snapshot,
                Errors = dataService.Errors,
                ElementErrors = dataService.ElementErrors,
            };

            using (var publisher = new RabbitPublisher("localhost"))
            {
                publisher.Publish(errors);
            }

            return Result.Succeeded;
        }

        private static IEnumerable<List<T>> ChunkBy<T>(IEnumerable<T> source, int chunkSize)
        {
            var chunk = new List<T>(chunkSize);
            foreach (var item in source)
            {
                chunk.Add(item);
                if (chunk.Count == chunkSize)
                {
                    yield return chunk;
                    chunk = new List<T>(chunkSize);
                }
            }
            if (chunk.Count > 0)
                yield return chunk;
        }
    }
}
