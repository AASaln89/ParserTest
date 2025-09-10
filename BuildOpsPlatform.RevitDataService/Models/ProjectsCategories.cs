namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ProjectsCategories
    {
        public Guid ProjectId { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;
    }
}
