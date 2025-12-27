// <copyright file="BulkUploadValidateResponseDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.BulkUpload;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BulkUploadValidateResponse
{
    public string BatchId { get; set; } = string.Empty;

    public int TotalRows { get; set; }

    public int ValidRowsCount { get; set; }

    public int InvalidRowsCount { get; set; }

    public List<InvalidRowDto> InvalidRows { get; set; } = new();

    public string Message { get; set; } = string.Empty;
}
