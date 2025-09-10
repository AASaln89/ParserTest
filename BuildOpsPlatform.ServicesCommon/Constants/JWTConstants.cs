namespace BuildOpsPlatform.ServicesCommon.Constants
{
    public class JWTConstants
    {
        // JWT configuration
        // Authentication constants
        public const int ACCESS_TOKEN_TIME_MINUTES = 60;
        public const int REFRESH_TOKEN_TIME_MINUTES = 600;
        // Security definitions
        public const string SECURITY_DEFINITION_BEARER = "Bearer";
        public const string SECURITY_DEFINITION_BEARER_DESCRIPTION = "JWT Authorization header using the Bearer scheme. Example: \" Authorization Bearer {token}\"";
        public const string SECURITY_DEFINITION_BEARER_NAME = "Authorization";
        // Cookie names
        public const string REQUEST_HEADERS_COOKIE = "Cookie";
        public const string ACCESS_TOKEN_COOKIE_NAME = "access_token";
        public const string REFRESH_TOKEN_COOKIE_NAME = "refresh_token";
        // JSON configuration keys
        public const string CONFIG_KEY_JWT_SECRET = "Jwt:Secret";
        public const string CONFIG_KEY_JWT_ISSUER = "Jwt:Issuer";
        public const string CONFIG_KEY_JWT_AUDIENCE = "Jwt:Audience";

    }
}
