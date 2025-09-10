namespace BuildOpsPlatform.ServicesCommon.DTOs.Companies
{
    public class UpdateFeaturesRequest
    {
        public List<string> EnableFeatures { get; set; } = new();
        public List<string> DisableFeatures { get; set; } = new();
    }
}
