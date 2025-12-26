// <copyright file="Component.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Template;

using System.Text.Json.Serialization;
using NaradX.Domain.Entities.Base;

public class Component : BaseEntity<Guid>
{
    public string Type { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public Example? Example { get; set; }

    public List<Button>? Buttons { get; set; }
}


