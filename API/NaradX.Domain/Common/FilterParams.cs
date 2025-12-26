// <copyright file="FilterParams.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FilterParams
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 5;

    public string? SearchTerm { get; set; }

    public string? SortColumn { get; set; }

    public string SortDirection { get; set; } = "asc";

    public FilterParams()
    {
        SearchTerm = string.Empty;
        SortColumn = string.Empty;
        SortDirection = "asc";
    }

    public FilterParams(int pageNumber, int pageSize, string? searchTerm = null, string? sortColumn = null, string? sortDirection = "asc")
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchTerm = searchTerm;
        SortColumn = sortColumn;
        SortDirection = sortDirection;
    }
}
