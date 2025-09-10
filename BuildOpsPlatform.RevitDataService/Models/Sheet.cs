namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Sheet
    {
        public int? SheetId { get; set; }
        public string? SheetUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<ElementView> ElementViews { get; set; } = new List<ElementView>();
    }
}
