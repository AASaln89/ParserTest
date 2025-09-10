using BuildOpsPlatform.RevitDataCommon.DTOs;
using BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs;
using System;
using System.Collections.Generic;

namespace BuildOpsPlatform.RevitDataCommon.Messaging
{
    [Serializable]
    public class RevitProjectDataMessage
    {
        public RvtSnapshotDto Snapshot { get; set; } = null!;

        // Основные сущности (по-сути, содержимое Snapshot)
        public List<ElementDto>? Elements { get; set; }
        public List<CategoryDto>? Categories { get; set; }
        public List<StageDto>? Stages { get; set; }
        public List<SiteDto>? Sites { get; set; }
        public List<LevelDto>? Levels { get; set; }
        public List<GridDto>? Grids { get; set; }
        public List<MaterialDto>? Materials { get; set; }
        public List<WorksetDto>? Worksets { get; set; }
        public List<ViewDto>? Views { get; set; }
        public List<DesignOptionDto>? DesignOptions { get; set; }
        public List<ErrorDto>? Errors { get; set; }

        // Параметры
        public List<ParameterDto>? Parameters { get; set; }

        // Промежуточные связи (по элементам и параметрам)
        public List<ElementParameterValueDto>? ElementValues { get; set; }
        public List<ElementParameterDto>? ElementParameters { get; set; }

        //public List<MaterialParameterSystemDto> MaterialSystemParameters { get; set; } = new();
        //public List<MaterialParameterSharedDto> MaterialSharedParameters { get; set; } = new();
        //public List<MaterialParameterProjectDto> MaterialProjectParameters { get; set; } = new();

        // Связи многие-ко-многим
        public List<ElementMaterialDto>? ElementMaterials { get; set; }
        public List<ElementViewDto>? ElementViews { get; set; }
        public List<ElementErrorDto>? ElementErrors { get; set; }
    }
}
