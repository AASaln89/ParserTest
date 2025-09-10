namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class ElementParameterDto
    {
        public int ElementId { get; set; }
        public int? ParameterId { get; set; }
        public string? ParameterGUID { get; set; } = null!;
    }
}
