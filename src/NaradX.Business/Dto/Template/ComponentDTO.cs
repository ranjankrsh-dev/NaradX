using System.Text.Json.Serialization;

namespace NaradX.Business.Dto.Template;

public class ComponentDTO
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
