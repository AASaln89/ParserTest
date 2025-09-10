namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ElementSchedule
    {
        public Guid RvtSnapshotId { get; set; }
        public virtual RvtSnapshot RvtSnapshot { get; set; } = null!;

        public int ElementId { get; set; }
        public virtual Element Element { get; set; } = null!;

        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; } = null!;
    }
}
