// <copyright file="ResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ResponseDto
{
    public bool IsSuccess { get; set; }

    public string Message { get; set; } = string.Empty;
}
