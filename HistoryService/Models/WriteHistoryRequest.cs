namespace HistoryService.Models;

public class WriteHistoryRequest
{
    public long InfrastructureId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}