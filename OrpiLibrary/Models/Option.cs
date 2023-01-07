using System.Text.Json.Serialization;

namespace OrpiLibrary.Models;

public class Option
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    [JsonPropertyName("value")]
    public dynamic? Value { get; set; }
}