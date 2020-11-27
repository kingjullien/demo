using ExcelDataReader;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Utility
{
    public class CommonImportMethods
    {
        //Excel-csv To data table  
        public static DataTable ExcelToDataTable(string filePath, bool header)
        {
            // Get Data from the uploaded file in data table
            DataTable dt = new DataTable();
            string extension = Path.GetExtension(filePath);
            if (extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
            {
                IExcelDataReader reader = null;
                using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(filePath).Equals(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (Path.GetExtension(filePath).Equals(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    if (reader != null)
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = header, EmptyColumnNamePrefix = "Column" }
                        };
                        //Fill DataSet
                        DataSet content = reader.AsDataSet(conf);
                        //dt = content.Tables[0];
                        dt = content.Tables[0].AsEnumerable().CopyToDataTable();
                    }
                }
            }
            else if (extension.ToLower() == ".csv")
            {
                #region csv file

                using (TextFieldParser csvReader = new TextFieldParser(filePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = null;
                    if (header)
                    {
                        colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            dt.Columns.Add(datecolumn);
                        }
                    }
                    bool istrue = false;
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = null;
                        try
                        {
                            fieldData = csvReader.ReadFields();
                        }
                        catch (Exception ex)
                        {
                            if (csvReader.ErrorLine.StartsWith("\""))
                            {
                                var line = csvReader.ErrorLine.Substring(1, csvReader.ErrorLine.Length - 2);
                                fieldData = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                            }
                            else
                            {
                                throw;
                            }
                        }
                        if (!header && istrue == false)
                        {
                            int collength = fieldData.Length;
                            colFields = new string[collength];
                            for (int i = 1; i <= colFields.Length; i++)
                            {
                                DataColumn datecolumn = new DataColumn("column" + i);
                                datecolumn.AllowDBNull = true;
                                dt.Columns.Add(datecolumn);
                            }
                            istrue = true;
                        }
                        int cnt = fieldData.Length - colFields.Length;
                        fieldData = fieldData.Take(fieldData.Length - cnt).ToArray();
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        dt.Rows.Add(fieldData);
                    }
                }

                #endregion
            }
            return dt;
        }

        // Convert Excel file to data-table
        public static DataTable ConvertExcelToDataTable(string filePath, bool header, int sheetIndex = 0)
        {
            DataTable dt = new DataTable();
            string extension = System.IO.Path.GetExtension(filePath);
            if (extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
            {
                IExcelDataReader reader = null;
                using (FileStream stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(filePath).Equals(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (Path.GetExtension(filePath).Equals(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    if (reader != null)
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = header, EmptyColumnNamePrefix = "cl" }
                        };
                        //Fill DataSet
                        DataSet content = reader.AsDataSet(conf);
                        //dt = content.Tables[0];
                        dt = content.Tables[sheetIndex].AsEnumerable().Skip(0).Take(1).CopyToDataTable();
                    }
                    if (!header)
                    {
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            dt.Columns[x].ColumnName = "Column" + (x + Convert.ToInt32(1));
                        }
                    }
                }
            }
            // Convert csv file to data-table
            else if (extension.ToLower() == ".csv")
            {
                using (TextFieldParser csvReader = new TextFieldParser(filePath))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    //read column names
                    string[] colFields = null;
                    if (header)
                    {
                        colFields = csvReader.ReadFields();
                        foreach (string column in colFields)
                        {
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            dt.Columns.Add(datecolumn);
                        }
                    }
                    bool istrue = false;
                    string[] fieldData = null;
                    try
                    {
                        fieldData = csvReader.ReadFields();
                    }
                    catch (Exception ex)
                    {
                        if (csvReader.ErrorLine.StartsWith("\""))
                        {
                            var line = csvReader.ErrorLine.Substring(1, csvReader.ErrorLine.Length - 2);
                            fieldData = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (!header && istrue == false)
                    {
                        int collength = fieldData.Length;
                        colFields = new string[collength];
                        for (int i = 1; i <= colFields.Length; i++)
                        {
                            DataColumn datecolumn = new DataColumn("Column " + (i - 1));
                            datecolumn.AllowDBNull = true;
                            dt.Columns.Add(datecolumn);
                        }
                        istrue = true;
                    }
                    int cnt = fieldData.Length - colFields.Length;
                    fieldData = fieldData.Take(fieldData.Length - cnt).ToArray();
                    if (csvReader.LineNumber != 2)
                    {
                        dt.Rows.Add(fieldData);
                    }
                    dt.Rows.Add(fieldData);
                }
            }
            return dt;
        }

        public static DataTable ConvertTSVToDataTable(string filePath, bool header)
        {
            // Text File Convert into Data table specific tab separated file
            DataTable dt = new DataTable();
            dt = ConvertTextToDataTable(filePath, "\t", header);
            return dt;
        }

        public static DataTable ConvertTextToDataTable(string File, string delimiter, bool header)
        {
            #region tsv file
            // Text File Convert into Data table specific Delimiter separated file
            DataTable dt = new DataTable();
            using (TextFieldParser txtReader = new TextFieldParser(File))
            {
                txtReader.SetDelimiters(delimiter);
                txtReader.HasFieldsEnclosedInQuotes = true;
                //read column names
                string[] colFields = null;
                if (header)
                {
                    colFields = txtReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        dt.Columns.Add(datecolumn);
                    }
                }

                bool istrue = false;
                //while (!txtReader.EndOfData)
                //{
                string[] fieldData = null;
                try
                {

                    fieldData = txtReader.ReadFields();

                }
                catch (Exception)
                {
                    if (txtReader.ErrorLine.StartsWith("\""))
                    {
                        var line = txtReader.ErrorLine.Substring(1, txtReader.ErrorLine.Length - 2);
                        fieldData = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                    }
                    else
                    {
                        throw;
                    }
                }
                if (!header && istrue == false)
                {
                    int collength = fieldData.Length;
                    colFields = new string[collength];
                    for (int i = 1; i <= colFields.Length; i++)
                    {
                        DataColumn datecolumn = new DataColumn("Column " + (i - 1));
                        datecolumn.AllowDBNull = true;
                        dt.Columns.Add(datecolumn);
                    }
                    istrue = true;
                }
                int cnt = fieldData.Length - colFields.Length;
                fieldData = fieldData.Take(fieldData.Length - cnt).ToArray();
                for (int i = 0; i < fieldData.Length; i++)
                {
                    if (fieldData[i] == "")
                    {
                        fieldData[i] = null;
                    }
                }
                if (txtReader.LineNumber != (header ? 2 : 1))
                {
                    dt.Rows.Add(fieldData);
                }
                //}
            }

            #endregion
            return dt;
        }

        public static DataTable TSVToDataTable(string filePath, bool header)
        {
            // Text File Convert into Data table specific tab separated file
            DataTable dt = new DataTable();
            dt = TextToDataTable(filePath, "\t", header);
            return dt;
        }

        public static DataTable ConvertFixedWidthToDataTable(string File, string ColumnMetaData, bool header, out string line)
        {
            DataTable dt = new DataTable();
            dt = FixedWidthTODataTable(File, ColumnMetaData, header, out line);
            return dt;
        }

        public static DataTable TextToDataTable(string File, string delimiter, bool header)
        {
            #region tsv file
            // Text File Convert into Data table specific Delimiter separated file
            DataTable dt = new DataTable();
            using (TextFieldParser txtReader = new TextFieldParser(File))
            {
                txtReader.SetDelimiters(delimiter);
                txtReader.HasFieldsEnclosedInQuotes = true;
                //read column names
                string[] colFields = null;
                if (header)
                {
                    colFields = txtReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        dt.Columns.Add(datecolumn);
                    }
                }

                bool istrue = false;
                while (!txtReader.EndOfData)
                {
                    string[] fieldData = null;
                    try
                    {
                        fieldData = txtReader.ReadFields();
                    }
                    catch (Exception ex)
                    {
                        if (txtReader.ErrorLine.StartsWith("\""))
                        {
                            var line = txtReader.ErrorLine.Substring(1, txtReader.ErrorLine.Length - 2);
                            fieldData = line.Split(new string[] { "\",\"" }, StringSplitOptions.None);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    if (!header && istrue == false)
                    {
                        int collength = fieldData.Length;
                        colFields = new string[collength];
                        for (int i = 1; i <= colFields.Length; i++)
                        {
                            DataColumn datecolumn = new DataColumn("Column " + (i - 1));
                            datecolumn.AllowDBNull = true;
                            dt.Columns.Add(datecolumn);
                        }
                        istrue = true;
                    }
                    int cnt = fieldData.Length - colFields.Length;
                    fieldData = fieldData.Take(fieldData.Length - cnt).ToArray();
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    dt.Rows.Add(fieldData);
                }
            }

            #endregion
            return dt;
        }

        public static DataTable FixedWidthTODataTable(string File, string ColumnMetaData, bool header, out string selectedline)
        {
            DataTable dt = new DataTable();
            List<string> lstColMetadata = new List<string>();
            lstColMetadata = ColumnMetaData.Split(',').ToList();
            int i = 1;
            List<string> fieldValues = new List<string>();
            using (TextFieldParser txtReader = new TextFieldParser(File))
            {
                string line = txtReader.ReadLine();
                string secondLine = txtReader.ReadLine();
                if (header)
                    selectedline = secondLine;
                else
                    selectedline = line;
                foreach (var item in lstColMetadata)
                {
                    var temp = item.Split('(')[1].Replace(")", "").Split('|');
                    string column = string.Empty;
                    try
                    {
                        column = line.Substring(int.Parse(temp[0]) - 1, int.Parse(temp[1])).Trim();
                    }
                    catch
                    {
                        selectedline = "Error occurred";
                    }

                    if (!header)
                        column = item.Split('(')[0];
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    dt.Columns.Add(datecolumn);
                    try
                    {
                        fieldValues.Add(secondLine.Substring(int.Parse(temp[0]) - 1, int.Parse(temp[1])).Trim());
                    }
                    catch (Exception)
                    {
                        selectedline = "Error occurred";
                    }

                }
                dt.Rows.Add(fieldValues.ToArray());
            }
            return dt;
        }
    }
}