namespace OrpiLibrary.Models;

public class HistoryLog
{
    public long? Id { get; set; } = null!;
    public DateTime? Timestamp { get; set; } = null!;
    public string? Title { get; set; } = null!;
    public string? Message { get; set; } = null!;
}