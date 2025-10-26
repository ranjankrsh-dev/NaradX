using System.Text.Json.Serialization;

namespace NaradX.Shared.Dto.Template;

public class WhatsAppMessageTemplateDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("language")]
    public string Language { get; set; }
    
    [JsonPropertyName("category")]
    public string Category { get; set; }
    
    [JsonPropertyName("components")]
    public List<ComponentDTO>? Components { get; set; }
}
