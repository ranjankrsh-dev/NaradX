// <copyright file="WhatsAppMessageTemplateDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Template;

using System.Text.Json.Serialization;

public class WhatsAppMessageTemplateDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("language")]
    public string Language { get; set; } = string.Empty;
    
    [JsonPropertyName("category")]
    public string Category { get; set; } = string.Empty;
    
    [JsonPropertyName("components")]
    public List<ComponentDTO>? Components { get; set; }
}
