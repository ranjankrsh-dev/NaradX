// <copyright file="ConfigValueDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigValueDto
{
    public string Value { get; set; }

    public string Text { get; set; }

    public ConfigValueDto(string value, string text)
    {
        Value = value;
        Text = text;
    }
}
