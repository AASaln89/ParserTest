using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class MaterialParameterSharedDto
    {
        public int MaterialId { get; set; }
        public Guid SharedParameterId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
