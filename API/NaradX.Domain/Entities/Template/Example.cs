// <copyright file="Example.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Template
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;

    public class Example : BaseEntity<int>
    {
        public List<BodyTextNamedParam> BodyTextNamedParams { get; set; } = [];
    }
}
