using System.Text.Json.Serialization;

namespace OrpiLibrary.Models; 

public class LoadData {
    [JsonPropertyName("cpu_usage")] public double CpuUsage { get; set; }
    [JsonPropertyName("memory_usage")] public double MemoryUsage { get; set; }
    [JsonPropertyName("disk_usage")] public double DiskUsage { get; set; }
    [JsonPropertyName("total_input_network_usage")] public double TotalInputNetworkUsage { get; set; }
    [JsonPropertyName("total_output_network_usage")] public double TotalOutputNetworkUsage { get; set; }
}