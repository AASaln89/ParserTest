using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs.JoinedDTOs
{
    public class SheetParameterSharedDto
    {
        public int SheetId { get; set; }
        public Guid SharedParameterId { get; set; }
        public int ParameterValueId { get; set; }
    }
}
