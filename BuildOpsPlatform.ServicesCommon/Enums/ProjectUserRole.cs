using System.Text.Json.Serialization;

namespace BuildOpsPlatform.ServicesCommon.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProjectUserRole
    {
        Owner,
        Admin,
        Guest,
    }
}
