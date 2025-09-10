namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Error
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ErrorId { get; set; }
        public string? Message { get; set; }

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public virtual ICollection<ElementError>? Errors { get; set; } = new List<ElementError>();
    }
}
