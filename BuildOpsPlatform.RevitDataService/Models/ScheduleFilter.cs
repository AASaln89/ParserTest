namespace BuildOpsPlatform.RevitDataService.Models
{
    public class ScheduleFilter
    {
        public int ScheduleFilterId { get; set; }
        public string ScheduleUniqueId { get; set; } = null!;
        public string Name { get; set; } = null!;

        public int ScheduleId { get; set; } 
        public virtual Schedule Schedule { get; set; } = null!;

        public int? ParameterRefId { get; set; }
        public string? ParameterRefGuid { get; set; } = null!;
        public virtual Parameter SystemParameter { get; set; } = null!;

        public string FilterType { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}