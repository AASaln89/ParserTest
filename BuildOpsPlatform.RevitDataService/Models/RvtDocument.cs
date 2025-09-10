namespace BuildOpsPlatform.RevitDataService.Models
{
    public class RvtDocument
    {
        public string Id { get; set; } = null!;
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public string? Stage { get; set; }
        public string? Building { get; set; }
        public string? Discipline { get; set; }
        public string? AppVersion { get; set; }
        public string? Description { get; set; }

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        public Guid? ProjectId { get; set; }

        public virtual ICollection<RvtSnapshot>? Snapshots { get; set; } = new List<RvtSnapshot>();
    }
}
