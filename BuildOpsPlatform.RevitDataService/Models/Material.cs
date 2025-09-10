namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Material
    {
        public int? MaterialId { get; set; }
        public string? MaterialUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<ElementMaterial>? ElementMaterials { get; set; } = new List<ElementMaterial>();
    }
}
