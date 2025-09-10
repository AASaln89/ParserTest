namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Level
    {
        public int? LevelId { get; set; }
        public string? LevelUniqueId { get; set; }
        public string? Name { get; set; }
        public string? ElevationValue { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
    }
}
