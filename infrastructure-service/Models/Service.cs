namespace infrastructure_service.Models;

public class Service
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TimeSpan Uptime { get; set; }
    public Status Status { get; set; }
    public DateTime LastUpdated { get; set; }
    public double AverageCpuUsage { get; set; }
    public double AverageMemoryUsage { get; set; }
    public double AverageDiskUsage { get; set; }
    public double AverageNetworkUsage { get; set; }
    public double PeakCpuUsage { get; set; }
    public double PeakMemoryUsage { get; set; }
    public double PeakDiskUsage { get; set; }
    public double PeakNetworkUsage { get; set; }
    public Load? CpuLoad { get; set; }
    public Load? MemoryLoad { get; set; }
    public Load? DiskLoad { get; set; }
    public Load? NetworkLoad { get; set; }
    public List<Option>? Options { get; set; }
}