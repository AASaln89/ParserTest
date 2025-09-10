namespace BuildOpsPlatform.RevitDataService.Models
{
    public class SheetSchedule
    {
        public int? SheetId { get; set; }
        public Sheet Sheet { get; set; } = null!;

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; } = null!;

        public int DocumentSnapshoId { get; set; }
        public RvtSnapshot RvtSnapshot { get; set; } = null!;
    }
}
