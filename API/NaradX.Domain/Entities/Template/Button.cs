// <copyright file="Button.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Template;

using NaradX.Domain.Entities.Base;

public class Button : BaseEntity<Guid>
{
    public string Type { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
}