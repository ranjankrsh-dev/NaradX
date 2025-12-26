// <copyright file="BulkUploadBatchDto.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Business.Dtos.BulkUpload;

using System;
using System.Collections.Generic;
using NaradX.Business.Dtos.Contact;

public class BulkUploadBatch
{
    public string BatchId { get; set; }

    public List<ContactDto> ValidContacts { get; set; } = new();

    public DateTime CreatedAt { get; set; }
}
