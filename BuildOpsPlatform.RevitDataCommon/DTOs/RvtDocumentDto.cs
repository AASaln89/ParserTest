using System;

namespace BuildOpsPlatform.RevitDataCommon.DTOs
{
    public class RvtDocumentDto
    {
        public string Id { get; set; } = string.Empty;
        public string? AppVersion { get; set; }
        public string? ProjectCode { get; set; }
        public string? ProjectName { get; set; }
        public string Author { get; set; } = string.Empty;
        public string? Stage { get; set; }
        public string? Building { get; set; }
        public string? Discipline { get; set; }
        public string? Description { get; set; }

        public Guid ProjectId { get; set; }
    }
}
