// <copyright file="WhatsAppTemplate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Template;

using System.Text.Json.Serialization;
using NaradX.Domain.Entities.Base;

public class WhatsAppTemplate : BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public List<Component> Components { get; set; } = [];
}