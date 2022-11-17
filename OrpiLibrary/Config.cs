// #define localhost

namespace OrpiLibrary {
    public static class Config {
        // Tokens lifetime
        public const double AccessTokenLifetime = 15;
        public const double RefreshTokenLifetime = 120;
        // Signatures
        public const string AccessTokenSignature = "AccessSecretSignature791";
        public const string RefreshTokenSignature = "RefreshSecretSignature274";
        // Database
        #if localhost
            public const string AuthenticationServiceDatabaseHost = "localhost";
        #else
            public const string AuthenticationServiceDatabaseHost = "auth-service-database";
        #endif
        public const int AuthenticationServiceDatabasePort = 5432;
        public const string AuthenticationServiceDatabaseName = "orpi";
        public const string AuthenticationServiceDatabaseUser = "auth_service";
        public const string AuthenticationServiceDatabasePassword = "authpass";
    }
}