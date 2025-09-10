using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BuildOpsPlatform.ServicesCommon.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProjectAction
    {
        [Description("Создание проекта")]
        CreateProject = 1,
        UpdateProject = 2,
        DeleteProject = 3,
        GetProjectDetails = 4,
        ListProjects = 5,
        AddMemberToProject = 6,
        RemoveMemberFromProject = 7,
        UpdateProjectSettings = 8,
        ArchiveProject = 9,
        RestoreProject = 10
    }
}
