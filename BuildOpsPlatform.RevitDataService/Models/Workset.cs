namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Workset
    {
        public int? WorksetId { get; set; }
        public string? WorksetUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
    }
}
