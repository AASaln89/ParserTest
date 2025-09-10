using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class LevelParameterSharedDto
    {
        public int LevelId { get; set; }
        public Guid SharedParameterId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
