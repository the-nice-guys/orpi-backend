namespace OrpiLibrary.Models {
    public class DeploymentRequest {
        public string? Uuid { get; set; }
        public string? Type { get; set; }
        public Service? Service { get; set; }
    }
}