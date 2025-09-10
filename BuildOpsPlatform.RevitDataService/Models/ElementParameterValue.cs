using BuildOpsPlatform.RevitDataService.Models.Enums;

namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ElementParameterValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int ElementId { get; set; }
        public virtual Element Element { get; set; } = null!;

        // Parameter meta info main
        public int? ParameterId { get; set; }
        public Guid? ParameterGUID { get; set; }

        public Guid ParameterDbId { get; set; }
        public virtual Parameter Parameter { get; set; } = null!;

        public bool IsTypeParameter { get; set; } = false;
        public StorageType? StorageType { get; set; }
        public string? UnitType { get; set; } = null!;

        // Parameter type
        public bool IsShared { get; set; } = false;
        public bool IsSystem { get; set; } = false;
        public bool IsProject { get; set; } = false;

        // Values
        public bool HasValue { get; set; } = false;
        public string? Value { get; set; }

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;
    }
}
