// <copyright file="ComponentDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Template;

using System.Text.Json.Serialization;

public class ComponentDTO
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
