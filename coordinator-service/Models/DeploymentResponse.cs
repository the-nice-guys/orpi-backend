namespace coordinator_service.Models;

public class DeploymentResponse
{
    public long ServiceId { get; set; }
    public string? Result { get; set; }
    public string? Message { get; set; }
}