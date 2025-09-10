using System.Reflection.Metadata;

namespace BuildOpsPlatform.RevitDataService.Models
{
    public class RvtSnapshot
    {
        public Guid Id { get; set; }
        public DateTime UploadDate { get; set; }

        public string DocumentId { get; set; } = null!;
        public virtual RvtDocument Document { get; set; } = null!;

        public virtual ICollection<Category>? Categories { get; set; } = new List<Category>();
        public virtual ICollection<Element>? Elements { get; set; } = new List<Element>();
        public virtual ICollection<Material>? Materials { get; set; } = new List<Material>();
        public virtual ICollection<Level>? Levels { get; set; } = new List<Level>();
        public virtual ICollection<Grid>? Grids { get; set; } = new List<Grid>();
        public virtual ICollection<DesignOption>? DesignOptions { get; set; } = new List<DesignOption>();
        public virtual ICollection<Workset>? Worksets { get; set; } = new List<Workset>();
        public virtual ICollection<View>? Views { get; set; } = new List<View>();
        public virtual ICollection<Sheet>? Sheets { get; set; } = new List<Sheet>();
        public virtual ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<ScheduleFilter>? ScheduleFilters { get; set; } = new List<ScheduleFilter>();
        public virtual ICollection<Parameter>? Parameters { get; set; } = new List<Parameter>();
        public virtual ICollection<Error>? Errors { get; set; } = new List<Error>();
        public virtual ICollection<Site>? Sites { get; set; } = new List<Site>();
        public virtual ICollection<Stage>? Stages { get; set; } = new List<Stage>();
    }
}
