// <copyright file="Country.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace NaradX.Domain.Entities.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NaradX.Domain.Entities.Base;

    public class Country : BaseEntity
    {
        public string Name { get; set; }=null!;

        public string Code { get; set; }=null!;

        public string PhoneCode { get; set; }=null!;

        public string CurrencyCode { get; set; }=null!;

        public string CurrencySymbol { get; set; } = null!;

        public string Timezone { get; set; }=null!;

        public ICollection<Language> Languages { get; set; } = new List<Language>();
    }
}
