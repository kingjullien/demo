using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class CommonExportMethods
    {
        public static byte[] ExportExcelFile(DataTable dt, string FileName, string SheetName)
        {
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(SheetName);
                if (dt != null && dt.Columns.Count > 0)
                {
                    worksheet.Cells.LoadFromDataTable(dt, true);
                    //Code to handle date format after the excel file gets downloaded
                    var dateColumns = from DataColumn d in dt.Columns
                                      where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                      select d.Ordinal + 1;

                    foreach (var dc in dateColumns)
                    {
                        worksheet.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                        worksheet.Cells[2, dc, dt.Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                    }

                    package.Workbook.Properties.Title = SheetName;
                }
                return package.GetAsByteArray();
            }
        }
    }
}