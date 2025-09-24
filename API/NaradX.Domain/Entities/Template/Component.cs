using System.Text.Json.Serialization;

namespace NaradX.Domain.Entities.Template;

public class Component
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("format")]
    public string Format { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("buttons")]
    public List<Button> Buttons { get; set; }
}


