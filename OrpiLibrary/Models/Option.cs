using System.Text.Json.Serialization;

namespace OrpiLibrary.Models {
    public class Option {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("type")] public Type Type { get; set; } = null!;
        [JsonPropertyName("name")] public string Name { get; set; } = null!;
        [JsonPropertyName("value")] public string Value { get; set; } = null!;
    }
}