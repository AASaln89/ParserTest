namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ElementError
    {
        public long Id { get; set; }

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public int? ElementId { get; set; }
        public virtual Element? Element { get; set; }

        public Guid? ErrorId { get; set; }
        public virtual Error? Error { get; set; }
    }
}
