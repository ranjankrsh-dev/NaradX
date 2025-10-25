using System.Text.Json.Serialization;

namespace NaradX.Shared.Models;

public class CreateTemplateResponse
{
    public class TemplateResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

}
