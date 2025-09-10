using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class ViewParameterSharedDto
    {
        public int ViewId { get; set; }
        public Guid SharedParameterId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
