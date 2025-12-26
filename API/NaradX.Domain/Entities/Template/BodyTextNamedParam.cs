// <copyright file="BodyTextNamedParam.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Template;

using NaradX.Domain.Entities.Base;

public class BodyTextNamedParam : BaseEntity<int>
{
    public string ParamName { get; set; } = string.Empty;

    public string Example { get; set; } = string.Empty;

}