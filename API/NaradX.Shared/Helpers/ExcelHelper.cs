using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaradX.Shared.Helpers
{
    public static class ExcelHelper
    {
        public static List<T> ReadExcelFile<T>(Stream fileStream, Func<IXLRangeRow, int, T> mapFunction, int headerRowCount = 1) where T : class
        {
            var items = new List<T>();

            using (var workbook = new XLWorkbook(fileStream))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed().Skip(headerRowCount);

                int rowNumber = headerRowCount + 1;

                foreach (var row in rows)
                {
                    var item = mapFunction(row, rowNumber);
                    if (item != null)
                    {
                        items.Add(item);
                    }
                    rowNumber++;
                }
            }

            return items;
        }

        public static bool IsValidExcelFile(IFormFile file, string[] allowedExtensions = null)
        {
            allowedExtensions ??= new[] { ".xlsx", ".xls", ".csv" };
            var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        public static byte[] CreateExcelTemplate<T>(List<ExcelColumnDefinition<T>> columns, string sheetName = "Template")
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Add headers
            for (int i = 0; i < columns.Count; i++)
            {
                worksheet.Cell(1, i + 1).Value = columns[i].Header;
                if (!string.IsNullOrEmpty(columns[i].Description))
                {
                    // You can add description as comment or in second row
                    worksheet.Cell(2, i + 1).Value = columns[i].Description;
                }
            }

            // Style header
            var headerRange = worksheet.Range(1, 1, 1, columns.Count);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public static bool HasDataRows(Stream fileStream, int headerRowCount = 1)
        {
            try
            {
                using (var workbook = new XLWorkbook(fileStream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var dataRows = worksheet.RangeUsed()?.RowsUsed()?.Skip(headerRowCount);
                    return dataRows != null && dataRows.Any();
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public class ExcelColumnDefinition<T>
    {
        public string Header { get; set; }
        public string Description { get; set; }
        public Func<T, object> ValueSelector { get; set; }
    }
}
