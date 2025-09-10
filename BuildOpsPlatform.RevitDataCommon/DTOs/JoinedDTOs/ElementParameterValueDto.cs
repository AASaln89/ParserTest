using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class ElementParameterValueDto
    {
        public int ElementId { get; set; }
        
        // Parameter meta info main
        public Guid ParameterDbId { get; set; }
        public int? ParameterId { get; set; }
        public Guid? ParameterGUID { get; set; }
        public bool IsTypeParameter { get; set; }
        public string? StorageType { get; set; }
        public string? UnitType { get; set; }

        // Parameter type
        public bool IsShared { get; set; } = false;
        public bool IsSystem { get; set; } = false;
        public bool IsProject { get; set; } = false;

        // Values
        public bool HasValue { get; set; } = false;
        public string? Value { get; set; }

        public Guid RvtSnapshotId { get; set; }
    }
}
