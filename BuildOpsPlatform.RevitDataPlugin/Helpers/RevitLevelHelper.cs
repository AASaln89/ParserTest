using Autodesk.Revit.DB;

namespace BuildOpsPlatform.RevitDataPlugin.Helpers
{
    public static class RevitLevelHelper
    {
        private static readonly BuiltInParameter[] LevelParameters =
        {
        BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM,
        BuiltInParameter.LEVEL_PARAM,
        BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM,
        BuiltInParameter.WALL_BASE_CONSTRAINT,
        BuiltInParameter.FAMILY_LEVEL_PARAM,
        BuiltInParameter.FAMILY_BASE_LEVEL_PARAM,
        BuiltInParameter.STAIRS_BASE_LEVEL,
        BuiltInParameter.STAIRS_BASE_LEVEL_PARAM,
        BuiltInParameter.RVT_HOST_LEVEL,
        BuiltInParameter.ROOF_BASE_LEVEL_PARAM,
    };

        public static int TryGetLevelIntId(Element element)
        {
            int levelIntId;
            foreach (var bip in LevelParameters)
            {
                try
                {
                    var param = element.get_Parameter(bip);
                    if (param != null && param.HasValue)
                    {
                        var candidateId = param.AsElementId();
                        if (candidateId != ElementId.InvalidElementId)
                        {
                            levelIntId = candidateId.IntegerValue;
                            return levelIntId;
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }

            levelIntId = -1;
            return levelIntId;
        }
    }
}
