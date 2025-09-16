using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Domain.Enums
{
    public enum ContactSource
    {
        Manual,      // Manually added via UI
        ExcelImport, // Imported from Excel
        API,         // Added via API integration
        CSVImport,   // Imported from CSV
        Migration    // Migrated from old system
    }

    public enum Source
    {
        Manual,      // Manually added via UI
        ExcelImport, // Imported from Excel
        API,         // Added via API integration
        CSVImport,   // Imported from CSV
    }
}
