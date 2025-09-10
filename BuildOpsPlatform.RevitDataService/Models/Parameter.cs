namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Parameter
    {
        public Guid Id { get; set; }

        public int? ParameterId { get; set; }
        public Guid? ParameterGUID { get; set; }
        public string? Name { get; set; }

        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public virtual ICollection<ElementParameterValue> ElementLinks { get; set; } = new List<ElementParameterValue>();
    }
}
