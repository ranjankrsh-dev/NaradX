// <copyright file="ValidationResultDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.BulkUpload;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ValidationResultDto
{
    public bool IsValid { get; set; }

    public List<string> Errors { get; set; } = new();
}
