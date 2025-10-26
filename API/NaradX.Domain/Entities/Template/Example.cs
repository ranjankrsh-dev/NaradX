using NaradX.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Entities.Template
{
    public class Example : BaseEntity<int>
    {
        public List<BodyTextNamedParam> BodyTextNamedParams { get; set; } = [];
    }
}
