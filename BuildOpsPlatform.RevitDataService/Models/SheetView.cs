namespace BuildOpsPlatform.RevitDataService.Models
{
    public class SheetView
    {
        public int? SheetId { get; set; }
        public Sheet Sheet { get; set; } = null!;

        public int ViewId { get; set; }
        public View View { get; set; } = null!;

        public int DocumentSnapshoId { get; set; }
        public RvtSnapshot RvtSnapshot { get; set; } = null!;
    }
}
