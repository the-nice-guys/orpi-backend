namespace coordinator_service.Models;

public class OutboxMessage
{
    public long Id { get; set; }
    public string Topic { get; set; }
    public string Data { get; set; }
    public int State { get; set; }
}