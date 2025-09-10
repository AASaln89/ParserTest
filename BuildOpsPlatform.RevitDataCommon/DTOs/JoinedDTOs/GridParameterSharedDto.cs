using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class GridParameterSharedDto
    {
        public int GridId { get; set; }
        public Guid SharedParameterId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
