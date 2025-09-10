namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ElementMaterial
    {
        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public int ElementId { get; set; }
        public virtual Element Element { get; set; } = null!;

        public int MaterialId { get; set; }
        public virtual Material Material { get; set; } = null!;
    }
}
