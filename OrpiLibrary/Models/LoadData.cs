using System.Text.Json.Serialization;

namespace OrpiLibrary.Models; 

public class LoadData {
    [JsonPropertyName("current_cpu_usage")] public double CurrentCpuUsage { get; set; }
    [JsonPropertyName("current_memory_usage")] public double CurrentMemoryUsage { get; set; }
    [JsonPropertyName("current_disk_usage")] public double CurrentDiskUsage { get; set; }
    [JsonPropertyName("current_network_usage")] public double CurrentNetworkUsage { get; set; }
    [JsonPropertyName("average_cpu_usage")] public double AverageCpuUsage { get; set; }
    [JsonPropertyName("average_memory_usage")] public double AverageMemoryUsage { get; set; }
    [JsonPropertyName("average_disk_usage")] public double AverageDiskUsage { get; set; }
    [JsonPropertyName("average_network_usage")] public double AverageNetworkUsage { get; set; }
    [JsonPropertyName("peak_cpu_usage")] public double PeakCpuUsage { get; set; }
    [JsonPropertyName("peak_memory_usage")] public double PeakMemoryUsage { get; set; }
    [JsonPropertyName("peak_disk_usage")] public double PeakDiskUsage { get; set; }
    [JsonPropertyName("peak_network_usage")] public double PeakNetworkUsage { get; set; }
}