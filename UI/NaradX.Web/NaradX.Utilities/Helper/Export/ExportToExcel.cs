using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Utilities.Helper.Export
{
    public static class ExportToExcel
    {
        public static MemoryStream DataTableToExcel(DataTable data)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                var worksheet = wb.Worksheets.Add(data, "OverallData");
                worksheet.Table(0).Theme = XLTableTheme.TableStyleLight14;
                worksheet.Row(1).Style.Font.Bold = true;
                worksheet.SheetView.FreezeRows(1);
                worksheet.Tables.First().ShowAutoFilter = false;
                worksheet.AutoFilter.IsEnabled = false;
                worksheet.Columns().AdjustToContents(10.0, 50.0);

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream;
                    //return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}
