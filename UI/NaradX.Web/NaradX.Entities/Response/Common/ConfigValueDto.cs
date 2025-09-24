using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Entities.Response.Common
{
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
}
