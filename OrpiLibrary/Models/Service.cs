using System.Text.Json.Serialization;

namespace OrpiLibrary.Models;

public class Service {
    [JsonPropertyName("service_id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = null!;
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("ip")] public string Ip { get; set; } = null!;
    [JsonPropertyName("uptime")] public TimeSpan Uptime { get; set; }
    [JsonPropertyName("status")] public Status Status { get; set; }
    [JsonPropertyName("last_updated")] public DateTime? LastUpdated { get; set; }
    [JsonPropertyName("load_data")] public LoadData? LoadData { get; set; }
    [JsonPropertyName("options")] public List<Option> Options { get; set; } = null!;
    [JsonPropertyName("dependencies")] public string[]? Dependencies { get; set; }
}