namespace OrpiLibrary.Models;

public class AddHostRequest
{
    public long InfrastructureId { get; set; }
    public Host Host { get; set; }
}