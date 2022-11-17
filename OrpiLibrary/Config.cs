namespace OrpiLibrary {
    public static class Config {
        // Tokens lifetime
        public const double AccessTokenLifetime = 15;
        public const double RefreshTokenLifetime = 120;
        // Signatures
        public const string AccessTokenSignature = "AccessSecretSignature791";
        public const string RefreshTokenSignature = "RefreshSecretSignature274";
        // Database
        public const string Host = "localhost";
        public const int Port = 5432;
        public const string Database = "orpi";
        public const string User = "auth_service";
        public const string Password = "authpass";
    }
}