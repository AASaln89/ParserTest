namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ElementView
    {
        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public int ElementId { get; set; }
        public virtual Element Element { get; set; } = null!;

        public int ViewId { get; set; }
        public virtual View View { get; set; } = null!;
    }
}
