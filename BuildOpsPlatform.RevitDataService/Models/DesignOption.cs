namespace BuildOpsPlatform.RevitDataService.Models
{
    public class DesignOption
    {
        public int? DesignOptionId { get; set; }
        public string? DesignOptionUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
    }
}
