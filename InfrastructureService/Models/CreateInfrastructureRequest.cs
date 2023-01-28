namespace OrpiLibrary.Models;

public class CreateInfrastructureRequest
{
    public long UserId { get; set; }
    public Infrastructure Infrastructure { get; set; }
}