using OrpiLibrary.Models;

namespace coordinator_service.Models;

public class DeploymentRequest
{
    public string? Uuid { get; set; }
    public string? Type { get; set; }
    public Service? Service { get; set; }
}