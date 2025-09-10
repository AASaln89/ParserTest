namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<RvtDocument>? Documents { get; set; } = new List<RvtDocument>();
    }
}
