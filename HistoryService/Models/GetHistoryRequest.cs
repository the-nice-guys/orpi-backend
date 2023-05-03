namespace HistoryService.Models;

public class GetHistoryRequest
{
    public long InfrastructureId { get; set; }
    public long Take { get; set; }
}