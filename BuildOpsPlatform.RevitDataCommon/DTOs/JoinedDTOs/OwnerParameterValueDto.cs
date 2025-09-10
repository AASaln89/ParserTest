namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class OwnerParameterValueDto
    {
        public string OwnerType { get; set; } // "Element", "Material", "View", "Sheet", "Specification"
        public int OwnerId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
