using System.Text.Json.Serialization;

namespace NaradX.Domain.Entities.Template;

public class WhatsAppTemplate
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("components")]
    public List<Component> Components { get; set; } = new List<Component>();
}