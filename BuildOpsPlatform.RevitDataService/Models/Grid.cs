namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Grid
    {
        public int GridId { get; set; }
        public string? GridUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }
    }
}
