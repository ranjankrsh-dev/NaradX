using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum ImportSource
    {
        Manual = 1,      // Manually added via UI
        ExcelImport = 2, // Imported from Excel
        API = 3         // Added via API integration
    }
}
