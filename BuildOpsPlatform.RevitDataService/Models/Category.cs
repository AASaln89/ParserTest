namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryType { get; set; }
        public string Name { get; set; } = null!;

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
        }
}
