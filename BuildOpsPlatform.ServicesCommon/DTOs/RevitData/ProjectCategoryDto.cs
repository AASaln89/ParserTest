using BuildOpsPlatform.RevitDataCommon.DTOs;

namespace BuildOpsPlatform.ServicesCommon.DTOs.RevitData
{
    public class ProjectCategoryDto
    {
        public Guid ProjectId { get; set; }

        public int CategoryId { get; set; }
        public CategoryDto? Category { get; set; } = null!;
    }
}
