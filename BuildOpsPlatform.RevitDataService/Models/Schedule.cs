namespace BuildOpsPlatform.RevitDataService.Models
{
    public class Schedule
    {
        public int? ScheduleId { get; set; }
        public string? ScheduleUniqueId { get; set; }
        public string? Name { get; set; }

        public Guid? RvtSnapshotId { get; set; }
        public virtual RvtSnapshot? RvtSnapshot { get; set; }

        public virtual ICollection<ElementView> ElementViews { get; set; } = new List<ElementView>();
    }
}
