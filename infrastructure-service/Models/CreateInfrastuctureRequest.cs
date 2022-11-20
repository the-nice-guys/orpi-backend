namespace infrastructure_service.Models;

public class CreateInfrastuctureRequest
{
    public long UserId { get; set; }
    public Infrastructure Infrastructure { get; set; }
}