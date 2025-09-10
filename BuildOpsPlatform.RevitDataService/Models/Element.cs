
namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Element
    {
        public int ElementId { get; set; }
        public string? ElementUniqueId { get; set; }
        public string? TypeName { get; set; }

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public int? LevelId { get; set; }
        public virtual Level? Level { get; set; }

        public int? WorksetId { get; set; }
        public virtual Workset? Workset { get; set; }

        public int? StageId { get; set; }
        public virtual Stage? Stage { get; set; }

        public int? DesignOptionId { get; set; }
        public virtual DesignOption? DesignOption { get; set; }

        public virtual ICollection<ElementMaterial>? ElementMaterials { get; set; } = new List<ElementMaterial>();
        public virtual ICollection<ElementParameterValue>? Parameters { get; set; } = new List<ElementParameterValue>();
        public virtual ICollection<ElementView> ElementViews { get; set; } = new List<ElementView>();
        public virtual ICollection<ElementError> Errors { get; set; } = new List<ElementError>();
    }
}
