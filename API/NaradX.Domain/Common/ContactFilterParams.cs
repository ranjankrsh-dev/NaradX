// <copyright file="ContactFilterParams.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ContactFilterParams : FilterParams
{
    public int TenantId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Status { get; set; }

    public ContactFilterParams() : base()
    {
        Name = string.Empty;
        Phone = string.Empty;
        Status = string.Empty;
    }

    public ContactFilterParams(
        int tenantId,
        int pageNumber = 1,
        int pageSize = 10,
        string? name = null,
        string? phone = null,
        string? status = null,
        string? searchTerm = null,
        string? sortColumn = null,
        string? sortDirection = "asc")
        : base(pageNumber, pageSize, searchTerm, sortColumn, sortDirection)
    {
        TenantId = tenantId;
        Name = name;
        Phone = phone;
        Status = status;
    }
}
