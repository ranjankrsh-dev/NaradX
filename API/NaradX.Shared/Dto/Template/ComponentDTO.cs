using System.Text.Json.Serialization;

namespace NaradX.Shared.Dto.Template;

public class ComponentDTO
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
