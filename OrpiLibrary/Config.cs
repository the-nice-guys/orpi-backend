#define localhost

namespace OrpiLibrary;

public static class Config {
    // Tokens lifetime
    public const double AccessTokenLifetime = 15;
    public const double RefreshTokenLifetime = 120;
    // Signatures
    public const string AccessTokenSignature = "AccessSecretSignature791";
    public const string RefreshTokenSignature = "RefreshSecretSignature274";
    // Docker module
    #if localhost
        public const string KafkaBootstrapServerHost = "localhost";
    #else
        public const string KafkaBootstrapServerHost = "broker";
    #endif
    
    public const int KafkaBootstrapServerPort = 9092;
    public const string KafkaRequestGroupId = "request-test-group";
    public const string KafkaRequestTopic = "docker-requests";
    public const string KafkaResponseGroupId = "response-test-group";
    public const string KafkaResponseTopic = "docker-responses";
}