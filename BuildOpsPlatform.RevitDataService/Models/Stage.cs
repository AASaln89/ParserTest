namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Stage
    {
        public int? StageId { get; set; }
        public string? StageUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
    }
}
