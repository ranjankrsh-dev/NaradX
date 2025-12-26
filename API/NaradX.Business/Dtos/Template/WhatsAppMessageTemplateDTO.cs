// <copyright file="WhatsAppMessageTemplateDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Template;

using System.Text.Json.Serialization;

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
