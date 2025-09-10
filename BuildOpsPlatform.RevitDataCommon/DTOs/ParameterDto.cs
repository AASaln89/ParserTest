using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class ParameterDto
    {
        public Guid Id { get; set; }

        public int? ParameterId { get; set; }
        public Guid? ParameterGUID { get; set; }
        public string? Name { get; set; }

        public Guid? DocumentSnapshotId { get; set; }
        public int CategoryId { get; set; }
    }
}
