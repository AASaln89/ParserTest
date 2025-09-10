namespace BuildOpsPlatform.RevitDataService.Models
{
    public class View
    {
        public int? ViewId { get; set; }
        public string? ViewUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<ElementView> ElementViews { get; set; } = new List<ElementView>();
    }
}
