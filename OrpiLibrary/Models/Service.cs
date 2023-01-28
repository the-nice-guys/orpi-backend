using System.Text.Json.Serialization;

namespace OrpiLibrary.Models {
    public class Service {
        [JsonPropertyName("service_id")] public long Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; } = null!;
        [JsonPropertyName("description")] public string? Description { get; set; }
        [JsonPropertyName("ip")] public string Ip { get; set; } = null!;
        [JsonPropertyName("uptime")] public TimeSpan Uptime { get; set; }
        [JsonPropertyName("status")] public Status Status { get; set; }
        [JsonPropertyName("last_updated")] public DateTime? LastUpdated { get; set; }
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
        [JsonPropertyName("options")] public List<Option> Options { get; set; } = null!;
        [JsonPropertyName("dependencies")] public List<string>? Dependencies { get; set; }
    }
}