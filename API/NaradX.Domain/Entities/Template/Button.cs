using System.Text.Json.Serialization;

namespace NaradX.Domain.Entities.Template;

public class Button
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; }

    [JsonPropertyName("phone_number")]
    public string PhoneNumber { get; set; }
}