namespace BuildOpsPlatform.ServicesCommon.Constants
{
    public class CorsPolicyConstants
    {
        // Frontend
        // Frontend CORS policy name
        public const string FRONTEND_CORS_POLICY = "AllowFrontend";
        // Frontend CORS policy origins
        public static readonly string[] FRONTEND_CORS_ORIGINS = new[]
        {
            "http://localhost:5173"
        };
    }
}
