// <copyright file="CreateTemplateResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Template;

using System.Text.Json.Serialization;

/// <summary>
/// Response model for create template operations.
/// </summary>
public class CreateTemplateResponse
{
    /// <summary>
    /// Inner template response model containing template details.
    /// </summary>
    public class TemplateResponse
    {
        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the template name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the template status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
