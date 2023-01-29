using System.Text.Json.Serialization;

namespace OrpiLibrary.Models;

public class Host {
    [JsonPropertyName("host_id")] public long Id { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("icon")] public string? Icon { get; set; }
    [JsonPropertyName("ip")] public string? Ip { get; set; }
    [JsonPropertyName("status")] public Status Status { get; set; }
    [JsonPropertyName("services")] public List<Service>? Services { get; set; }
}