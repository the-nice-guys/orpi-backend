using System.Text.Json.Serialization;

namespace OrpiLibrary.Models;

public class Infrastructure
{
    [JsonPropertyName("infrastructure_id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
    [JsonPropertyName("hosts")]
    public List<Host>? Hosts { get; set; }
    //public dynamic Hosts { get; set; }
}