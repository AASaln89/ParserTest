using Autodesk.Revit.DB;
using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs;
using BuildOpsPlatform.RevitDataPlugin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildOpsPlatform.RevitDataPlugin.Services
{
    public class DataCollectionService
    {
        private readonly HttpRevitDataService _service;
        private List<BuiltInCategory> _builtInCategory = new List<BuiltInCategory>();

        public DataCollectionService()
        {
            _service = new HttpRevitDataService();
            _builtInCategory = new List<BuiltInCategory>();
        }

        public List<CategoryDto> Categories = new List<CategoryDto>();
        public List<ParameterDto> Parameters = new List<ParameterDto>();

        public List<ElementDto> Elements = new List<ElementDto>();
        public Dictionary<int, string> ElementIds = new Dictionary<int, string>();
        public List<ElementParameterValueDto> ElementsValues = new List<ElementParameterValueDto>();

        public List<ErrorDto> Errors = new List<ErrorDto>();
        public List<ElementErrorDto> ElementErrors = new List<ElementErrorDto>();

        public List<ViewDto> Views = new List<ViewDto>();
        public List<ElementViewDto> ElementViews = new List<ElementViewDto>();

        private string GetParameterValueAsString(Parameter param)
        {
            return param.StorageType switch
            {
                StorageType.String => param.AsString(),
                StorageType.Double => param.AsDouble().ToString(),
                StorageType.Integer => param.AsInteger().ToString(),
                StorageType.ElementId => param.AsValueString(),
                _ => "NULL"
            };
        }

        public void ExtractCategories(Document doc)
        {
            //var hasCategories = _service.Get();
            //if (hasCategories)
            //{
            //    var result = _service.GetProjectCategories();
            //    _builtInCategory.AddRange(result.Select(x => (BuiltInCategory)x.CategoryId));
            //    return result;
            //}

            var categories = doc.Settings.Categories
                .Cast<Category>()
                
                //.Where(x => x.CategoryType == CategoryType.Model)
                .Where(x => x.Id.IntegerValue < 0)
                .Where(x => !x.Name.Contains("<"));

            foreach (Category cat in categories)
            {
                var subCat = cat.SubCategories
                    .Cast<Category>()
                    .Where(x => x.CategoryType == CategoryType.Model)
                    .Where(x => x.Id.IntegerValue < 0)
                    .Where(x => !x.Name.Contains("<"))
                    .ToList();

                if (subCat.Count > 0)
                {
                    foreach (Category sub in subCat)
                    {
                        if (_builtInCategory.Contains((BuiltInCategory)sub.Id.IntegerValue))
                        {
                            continue;
                        }
                        Categories.Add(new CategoryDto
                        {
                            CategoryId = sub.Id.IntegerValue,
                            CategoryType = sub.CategoryType.ToString(),
                            Name = sub.Name
                        });
                        _builtInCategory.Add((BuiltInCategory)sub.Id.IntegerValue);
                    }
                }
                _builtInCategory.Add((BuiltInCategory)cat.Id.IntegerValue);

                Categories.Add(new CategoryDto
                {
                    CategoryId = cat.Id.IntegerValue,
                    CategoryType = cat.CategoryType.ToString(),
                    Name = cat.Name
                });
            }
            Categories.Add(new CategoryDto
            {
                CategoryId = -1,
                CategoryType = "INVALID",
                Name = "INVALID"
            });
        }

        public void ExtractElements(Document doc)
        {
            // Кеши на весь проход (один снапшот / один документ)
            var parametersByKey = new Dictionary<Guid, ParameterDto>(); // ParamKey -> ParameterDto
            var guidToParamKey = new Dictionary<Guid, Guid>();         // ParameterGUID -> ParamKey
            var intToParamKey = new Dictionary<int, Guid>();          // ParameterId(int) -> ParamKey
            var seenTypes = new HashSet<int>();                    // чтобы не дублировать type-параметры

            foreach (var cat in _builtInCategory)
            {
                var collector = new FilteredElementCollector(doc)
                    .OfCategory(cat)
                    .WhereElementIsNotElementType()
                    .OfType<Element>();

                foreach (var element in collector)
                {
                    var elementDto = new ElementDto
                    {
                        ElementId = element.Id?.IntegerValue ?? -1,
                        ElementUniqueId = element.UniqueId,
                        TypeName = element.Name,
                        CategoryId = element.Category?.Id.IntegerValue ?? -1,
                        LevelId = RevitLevelHelper.TryGetLevelIntId(element),
                        WorksetId = element.WorksetId?.IntegerValue ?? -1,
                        PhaseId = element.get_Parameter(BuiltInParameter.PHASE_CREATED)?.AsElementId().IntegerValue ?? -1,
                        DesignOptionId = element.DesignOption?.Id.IntegerValue ?? -1,
                    };
                    Elements.Add(elementDto);

                    // Inst-параметры
                    foreach (Parameter p in element.Parameters)
                        TryAddParameter(p, elementDto.ElementId, isTypeParam: false);

                    // Type-параметры (снимаем по одному разу на тип)
                    var typeId = element.GetTypeId();
                    if (typeId != ElementId.InvalidElementId && seenTypes.Add(typeId.IntegerValue))
                    {
                        var type = doc.GetElement(typeId);
                        if (type != null)
                            foreach (Parameter p in type.Parameters)
                                TryAddParameter(p, elementDto.ElementId, isTypeParam: true);
                    }
                }
            }

            // Итоговые уникальные параметры
            Parameters.AddRange(parametersByKey.Values);

            // Локальные функции
            void TryAddParameter(Parameter param, int elementId, bool isTypeParam)
            {
                if (param?.Definition == null)
                    return;

                var name = param.Definition.Name ?? string.Empty;
                var storageType = param.StorageType.ToString();
                var valueStr = GetParameterValueAsString(param);

                bool isShared = param.IsShared;
                int? idInt = param.Id?.IntegerValue ?? null;
                Guid guid = isShared ? param.GUID : Guid.Empty;

                // Определяем устойчивый ParamKey (FK на Parameters)
                if (!TryResolveParamKey(isShared, idInt, guid, name, out Guid paramKey))
                    return;

                // Добавляем значение с правильным FK
                ElementsValues.Add(new ElementParameterValueDto
                {
                    ElementId = elementId,
                    Value = valueStr,
                    StorageType = storageType,
                    IsTypeParameter = isTypeParam,
                    IsProject = (param.Id?.IntegerValue ?? 0) > 0,
                    IsShared = isShared,
                    IsSystem = (param.Id?.IntegerValue ?? 0) < 0,
                    HasValue = param.HasValue,

                    ParameterGUID = isShared ? guid : (Guid?)null,
                    ParameterId = !isShared ? idInt : null,
                    ParameterDbId = paramKey
                });
            }

            bool TryResolveParamKey(bool isShared, int? idInt, Guid guid, string name, out Guid paramKey)
            {
                if (isShared)
                {
                    if (guid == Guid.Empty)
                    {
                        paramKey = default;
                        return false;
                    }

                    if (!guidToParamKey.TryGetValue(guid, out paramKey))
                    {
                        paramKey = Guid.NewGuid();
                        guidToParamKey[guid] = paramKey;

                        parametersByKey[paramKey] = new ParameterDto
                        {
                            Id = paramKey,
                            Name = name,
                            ParameterGUID = guid,
                            ParameterId = null
                        };
                    }
                    return true;
                }
                else
                {
                    if (!idInt.HasValue)
                    {
                        paramKey = default;
                        return false;
                    }

                    var key = idInt.Value;
                    if (!intToParamKey.TryGetValue(key, out paramKey))
                    {
                        paramKey = Guid.NewGuid();
                        intToParamKey[key] = paramKey;

                        parametersByKey[paramKey] = new ParameterDto
                        {
                            Id = paramKey,
                            Name = name,
                            ParameterGUID = null,
                            ParameterId = key
                        };
                    }
                    return true;
                }
            }
        }

        public List<WorksetDto> ExtractWorksets(Document doc)
        {
            var result = new List<WorksetDto>();
            var worksets = new FilteredWorksetCollector(doc)
                .OfKind(WorksetKind.UserWorkset)
                .ToList();

            foreach (var workset in worksets)
            {
                result.Add(new WorksetDto
                {
                    WorksetId = workset.Id.IntegerValue,
                    WorksetUniqueId = workset.UniqueId.ToString(),
                    Name = workset.Name
                });
            }
            return result;
        }

        //public List<LevelDto> ExtractLevels(Document doc, out LevelParameterProjectDto levelParameterProjectDto, out LevelParameterSharedDto levelParameterSharedDto, out LevelParameterSystemDto levelParameterSystemDto)
        //{
        //    var result = new List<LevelDto>();
        //    var levels = new FilteredElementCollector(doc)
        //        .OfClass(typeof(Level))
        //        .OfType<Level>()
        //        .ToList();

        //    foreach (var level in levels)
        //    {
        //        var levelDto = new LevelDto
        //        {
        //            LevelId = level.Id.IntegerValue,
        //            LevelUniqueId = level.UniqueId,
        //            Name = level.Name,
        //            ElevationValue = level.Elevation.ToString()
        //        };
        //        result.Add(levelDto);

        //        foreach (Parameter parameter in level.Parameters)
        //        {
        //            if (parameter.IsShared)
        //            {
        //                var sharedParameter = new ParameterSharedDto
        //                {
        //                    ParameterId = parameter.GUID,
        //                    UnitType = parameter?.Definition?.ParameterType.ToString(),
        //                    Name = parameter?.Definition?.Name,
        //                    ConnectionType = "Instance"
        //                };
        //                if (parameter.StorageType == StorageType.Integer)
        //                {
        //                    sharedParameter.StorageType = StorageType.Integer.ToString();
        //                } 
        //                else if (parameter.StorageType == StorageType.ElementId)
        //                {
        //                    sharedParameter.StorageType = StorageType.ElementId.ToString();
        //                }
        //                    var levelParameterDto = new LevelParameterSharedDto
        //                    {
        //                        LevelId = level.Id.IntegerValue,
        //                        SharedParameterId = parameter.GUID,
        //                    };

        //            }
        //        }
        //    }
        //}

        public RvtDocumentDto ExtractDocument(Document doc)
        {
            return new RvtDocumentDto
            {
                Id = doc.Title,
                AppVersion = doc.Application.VersionName,
            };
        }

        public List<SiteDto> ExtractSites(Document doc)
        {
            var siteLocations = new FilteredElementCollector(doc)
                .OfClass(typeof(BasePoint))
                .OfType<BasePoint>()
                .ToList();

            return siteLocations
                .Select(bp => new SiteDto
                {
                    SiteId = bp.Id.IntegerValue,
                    SiteUniqueId = bp.UniqueId,
                    Name = bp.Name,
                    Latitude = doc.SiteLocation.Latitude.ToString(),
                    Longitude = doc.SiteLocation.Longitude.ToString(),
                    BasePointElevetionValue = bp.get_Parameter(BuiltInParameter.BASEPOINT_ELEVATION_PARAM)?.AsValueString(),
                    BasePointEastWest = bp.get_Parameter(BuiltInParameter.BASEPOINT_EASTWEST_PARAM)?.AsValueString(),
                    BasePointNorthSouth = bp.get_Parameter(BuiltInParameter.BASEPOINT_NORTHSOUTH_PARAM)?.AsValueString(),
                    BasePointAngle = bp.get_Parameter(BuiltInParameter.BASEPOINT_ANGLETON_PARAM)?.AsValueString()
                })
                .ToList();
        }
        public List<StageDto> ExtractPhases(Document doc)
        {
            var phases = new FilteredElementCollector(doc)
                .OfClass(typeof(Phase))
                .OfType<Phase>()
                .ToList();

            return phases
                .Select(p => new StageDto
                {
                    StageId = p.Id.IntegerValue,
                    StageUniqueId = p.UniqueId,
                    Name = p.Name
                })
                .ToList();
        }

        public List<DesignOptionDto> ExtractDesignOptions(Document doc)
        {
            var options = new FilteredElementCollector(doc)
                .OfClass(typeof(DesignOption))
                .OfType<DesignOption>();

            return options
                .Select(o => new DesignOptionDto
                {
                    DesignOptionId = o.Id.IntegerValue,
                    DesignOptionUniqueId = o.UniqueId,
                    Name = o.Name
                })
                .ToList();
        }

        public List<GridDto> ExtractGrids(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Grid))
                .OfType<Grid>()
                .Select(grid => new GridDto
                {
                    GridId = grid.Id.IntegerValue,
                    GridUniqueId = grid.UniqueId,
                    Name = grid.Name
                })
                .ToList();
        }

        public List<MaterialDto> ExtractMaterials(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Material))
                .OfType<Material>()
                .Select(m => new MaterialDto
                {
                    MaterialId = m.Id.IntegerValue,
                    MaterialUniqueId = m.UniqueId,
                    Name = m.Name
                })
                .ToList();
        }

        public void ExtractViews(Document doc)
        {
            var views = new FilteredElementCollector(doc)
               .OfClass(typeof(View))
               .OfType<View>()
               .Where(v => !v.IsTemplate);

            var viewDto = views
               .Select(v => new ViewDto
               {
                   ViewId = v.Id.IntegerValue,
                   ViewUniqueId = v.UniqueId,
                   Name = v.Name
               })
               .ToList();

            Views.AddRange(viewDto);

            foreach(var view in views.Where(x=>x.Name == "Navisworks"))
            {
                var elements = new FilteredElementCollector(doc, view.Id)
                   .WhereElementIsNotElementType()
                   .OfType<Element>();

                foreach (var elem in elements)
                {
                    ElementViews.Add(new ElementViewDto
                    {
                        ElementId = elem.Id.IntegerValue,
                        ViewId = view.Id.IntegerValue
                    });
                }
            }
        }

        public void ExtractErrors(Document doc)
        {
            var failures = doc.GetWarnings();

            foreach (var f in failures)
            {
                var error = new ErrorDto
                {
                    ErrorId = f.GetFailureDefinitionId().Guid,
                    Message = f.GetDescriptionText(),
                };

                foreach(var elemId in f.GetFailingElements())
                {
                    if (!ElementIds.ContainsKey(elemId.IntegerValue))
                    {
                        ElementIds.Add(elemId.IntegerValue, doc.GetElement(elemId)?.UniqueId ?? string.Empty);
                        var element =  doc.GetElement(elemId);

                        var elementDto = new ElementDto
                        {
                            ElementId = element.Id?.IntegerValue ?? -1,
                            ElementUniqueId = element.UniqueId,
                            TypeName = element.Name,
                            CategoryId = element.Category?.Id.IntegerValue ?? -1,
                            LevelId = RevitLevelHelper.TryGetLevelIntId(element),
                            WorksetId = element.WorksetId?.IntegerValue ?? -1,
                            PhaseId = element.get_Parameter(BuiltInParameter.PHASE_CREATED)?.AsElementId().IntegerValue ?? -1,
                            DesignOptionId = element.DesignOption?.Id.IntegerValue ?? -1,
                        };

                        if (!ElementIds.ContainsKey(element.Id.IntegerValue))
                        {
                            ElementIds.Add(element.Id.IntegerValue, element.UniqueId);
                            Elements.Add(elementDto);
                        }

                        var errorElement = new ElementErrorDto
                        {
                            ElementId = elemId.IntegerValue,
                            ErrorId = f.GetFailureDefinitionId().Guid
                        };
                        ElementErrors.Add(errorElement);
                    }
                }
                Errors.Add(error);
            }
        }

        private List<ElementFilter> BuildCategoryFilters(Document document)
        {
            var filters = new List<ElementFilter>();
            var categories = document.Settings.Categories;

            foreach (Category category in categories)
            {
                //_targetCategories.Add
                var builtInCategory = (BuiltInCategory)category.Id.IntegerValue;
                var collector = new FilteredElementCollector(document)
                .OfCategory(builtInCategory)
                .WhereElementIsNotElementType()
                .OfType<Element>()
                .ToList();

                if (collector.Any())
                {
                    filters.Add(new ElementCategoryFilter(category.Id));
                }
            }
            return filters;
        }
    }
}
