using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, ValidateInput(true)]
    public class ImportDataController : BaseController
    {
        // GET: ImportData
        public ActionResult Index()
        {
            ImportJobDataFacade fac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DashboardBackgroundProcessStatsEntity> lstStats = fac.DashboardV2GetBackgroundProcessStats();
            DashboardBackgroundProcessStatsEntity obj = lstStats.FirstOrDefault(x => x.ETLType == "IMPORT");
            ViewBag.Message = obj?.Message2;
            ViewBag.ShowErrorSymbol = obj == null ? false : obj.ShowErrorSymbol;
            SessionHelper.ImportJobTableList = null;
            SessionHelper.lstImportFileTemplates = null;
            return View();

        }
        public ActionResult FileTypeIndex()
        {
            SessionHelper.objimportJobData = null;
            SessionHelper.ImportFilePath = string.Empty;
            Session["ImportData_Data"] = null;
            SessionHelper.ImportData_IsHeader = false;
            SessionHelper.ImportData_IsInLanguage = false;
            SessionHelper.ImportData_IsTag = false;
            SessionHelper.ImportData_HeaderLine = string.Empty;
            SessionHelper.ImportData_LineData = string.Empty;
            return View();
        }
        public ActionResult GetFileImportRequest(int? page, int? sortby, int? sortorder, int? pagevalue, string FileType = null)
        {
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 2;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            FileType = string.IsNullOrEmpty(FileType) ? "My Files" : FileType;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            ViewBag.FileType = FileType;

            int? UserId;
            if (FileType == "My Files")
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }

            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ImportJobDataEntity> lstImportJobs = efac.GetNewFileImportRequestByUserID(UserId, Helper.CurrentProvider);
            IPagedList<ImportJobDataEntity> pagedImportJobSettings = new StaticPagedList<ImportJobDataEntity>(lstImportJobs, currentPageIndex, pageSize, totalCount);
            return PartialView("FileImportRequestList", pagedImportJobSettings);
        }
        [Authorize, HttpGet, RequestFromSameDomain]
        public ActionResult ImportFileIndex(bool IsFromPrev = false, bool IsTemplateSelected = true)
        {
            ViewBag.IsTemplateSelected = IsTemplateSelected;
            if (IsFromPrev)
                return PartialView("_FileTypeSelection");
            else
                return PartialView();
        }
        public ActionResult GetSheetNames(HttpPostedFileBase file)
        {
            List<string> lstSheetnames = new List<string>();
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx", file.FileName.ToLower()))
            {
                try
                {
                    int sheetIndex = 0;
                    DocumentFormat.OpenXml.Packaging.SpreadsheetDocument document = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(file.InputStream, false);
                    Workbook workbook = document.WorkbookPart.Workbook;
                    foreach (DocumentFormat.OpenXml.Packaging.WorksheetPart worksheetpart in workbook.WorkbookPart.WorksheetParts)
                    {
                        string sheetName = document.WorkbookPart.Workbook.Descendants<Sheet>().ElementAt(sheetIndex).Name;
                        lstSheetnames.Add(sheetName + "::" + sheetIndex);
                        sheetIndex++;
                    }
                    return new JsonResult { Data = lstSheetnames };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = ImportDataLang.msgError + ":" + ex.Message };
                }
                
            }
            return new JsonResult { Data = ImportDataLang.msgFileFormateSupported };
        }
        [Authorize, HttpGet]
        public ActionResult UploadFileIndex(string FileFormat, string ImportMode, bool IsTemplateSelected = true)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            importJobData.SourceType = FileFormat;
            string DBImportType = string.Empty;
            if (ImportMode == "Data Import")
            {
                if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    importJobData.ImportType = "DNB_MATCH";
                    DBImportType = "D&B Cleanse Match & Enrich";
                }
                else if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    importJobData.ImportType = "OI_MATCH";
                    DBImportType = "Orb Intelligence Cleanse Match & Enrich";
                }
            }
            else if (ImportMode == "Match Refresh")
            {
                if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    importJobData.ImportType = "DNB_REFRESH";
                    DBImportType = "D&B Enrichment Only";
                }
                else if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    importJobData.ImportType = "OI_REFRESH";
                    DBImportType = "Orb Intelligence Enrichment Only";
                }
            }


            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            importJobData.lstTemplates = new List<ImportFileTemplates>();
            importJobData.lstTemplates = efac.GetAllImportFileTemplates().Where(x => x.FileFormat.ToLower() == FileFormat.ToLower() && x.ImportType.ToLower() == DBImportType.ToLower()).ToList();
            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
            ViewBag.fileType = FileFormat;
            ViewBag.ImportMode = ImportMode;
            ViewBag.IsTemplateSelected = IsTemplateSelected;
            return PartialView(importJobData);
        }

        [Authorize, HttpPost]
        public ActionResult UploadFileIndex(HttpPostedFileBase file, bool header, bool isTSV, string delimiter, string ImportMode, int templateId, string sheetName, bool isFinish, bool IsTemplateSelected = true, string templateName = null)
        {
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,.csv,.tsv,.txt", file.FileName.ToLower()))
            {
                ImportJobDataEntity importJobData = new ImportJobDataEntity();
                importJobData.selectedTemplated = null;
                importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
                if (file.ContentLength > 0)
                {
                    DataTable dt = new DataTable();
                    string path = string.Empty;
                    try
                    {
                        string directory = Server.MapPath("~/ImportDataFile/" + this.CurrentClient.ApplicationId);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        FileInfo oFileInfo = new FileInfo(file.FileName);
                        string fileExtension = oFileInfo.Extension;

                        string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + System.DateTime.Now.Ticks + fileExtension;
                        //Remove Special characters from file name 
                        string invalid = "!,@#$%^&*+" + new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                        foreach (char c in invalid)
                        {
                            fileName = fileName.Replace(c.ToString(), "");
                        }

                        string hostName = Helper.hostName.Split('.')[0];
                        string containerName = "importdatafile";
                        Task.Run(() => CommonMethod.CreateAndUploadBlobFile(hostName, file, fileName, containerName));

                        importJobData.ClientFileName = file.FileName;
                        importJobData.SourcePath = ConfigurationManager.AppSettings["azurestoragepath"].ToString() + "importdatafile/" + hostName;
                        importJobData.SourceFileName = fileName;
                        importJobData.Delimiter = delimiter;
                        importJobData.HasHeader = header;
                        importJobData.ExcelWorksheetName = !string.IsNullOrEmpty(sheetName) ? sheetName.Split(new[] { "::" }, StringSplitOptions.None)[0] : "";
                        path = Path.Combine(directory, Path.GetFileName(fileName.Replace("#", "")));
                        file.SaveAs(path);
                        if (fileExtension.ToLower() == ".xls" || fileExtension.ToLower() == ".xlsx" || fileExtension.ToLower() == ".csv")
                        {
                            dt = CommonImportMethods.ConvertExcelToDataTable(path, header, !string.IsNullOrEmpty(sheetName) ? Convert.ToInt32(sheetName.Split(new[] { "::" }, StringSplitOptions.None)[1]) : 0);
                        }
                        else if (isTSV && (fileExtension.ToLower() == ".tsv" || fileExtension.ToLower() == ".txt"))
                        {
                            dt = CommonImportMethods.ConvertTSVToDataTable(path, header);
                        }
                        else if (!isTSV && !string.IsNullOrEmpty(delimiter) && fileExtension.ToLower() == ".txt")
                        {
                            if (ValidateFilewithDelimiter(path, delimiter))
                            {
                                dt = CommonImportMethods.ConvertTextToDataTable(path, delimiter, header);
                            }
                            else
                            {
                                System.IO.File.Delete(path);
                                return new JsonResult { Data = ImportDataLang.msgInsertDelimiter };
                            }
                        }

                        //Get File Encoding
                        if (importJobData.SourceType != "EXCEL" && importJobData.SourceType != "CSV")
                            importJobData.IsUnicode = GetEncoding(path);

                        if (header)
                        {
                            Helper.InitLookups();
                            System.Collections.ArrayList columnsToRemove = new System.Collections.ArrayList();
                            foreach (var item in dt.Columns)
                            {
                                if (item.ToString().StartsWith("Column") && dt.AsEnumerable().All(dr => dr.IsNull(item.ToString())))
                                {
                                    columnsToRemove.Add(item.ToString());
                                }
                            }
                            //remove all columns that are empty
                            foreach (string columnNamea in columnsToRemove)
                            {
                                dt.Columns.Remove(columnNamea);
                            }

                            foreach (var item in dt.Columns)
                            {
                                if(Helper.IsSpecialCharactersExist(item.ToString()))
                                {
                                    return new JsonResult { Data = ImportDataLang.msgError + ":" + ImportDataLang.msgSpecialCharError };
                                }
                            }
                        }
                        Session["ImportData_Data"] = dt;
                        SessionHelper.ImportFilePath = path;
                        SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                    }
                    catch (Exception ex)
                    {
                        SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                        System.IO.File.Delete(path);
                        if (ex.Message.Contains("already belongs to this DataTable"))
                        {
                            return new JsonResult { Data = ex.Message.Replace("DataTable", "file") };
                        }
                        else
                        {
                            return new JsonResult { Data = ImportDataLang.msgError + ":" + ex.Message };
                        }

                    }

                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    OISettingFacade OIsfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    OIImportDataFacade orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                    bool IsTag = false;
                    bool IsInLanguage = false;
                    bool IsHeader = false;
                    //Get Import File Column Name to fill in dropdown
                    List<SelectListItem> lstAllFilter = new List<SelectListItem>();
                    List<string> dtColumns = new List<string>();
                    ImportFileTemplates fileTemplates = new ImportFileTemplates();
                    if (dt.Rows.Count > 0)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                        int i = 0;
                        foreach (DataColumn c in dt.Columns)
                        {
                            dtColumns.Add(c.ColumnName);
                            lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                            i++;
                            if (!c.ToString().StartsWith("Column") && !c.ToString().StartsWith("column"))
                            {
                                IsHeader = true;
                            }
                            if (c.ColumnName.ToLower() == "tags" || c.ColumnName.ToLower() == "tag")
                            {
                                IsTag = true;
                            }
                            if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                            {
                                if (c.ColumnName.ToLower() == "language" || c.ColumnName.ToLower() == "language values" || c.ColumnName.ToLower() == "languagevalues" || c.ColumnName.ToLower() == "language code" || c.ColumnName.ToLower() == "languagecode")
                                {
                                    IsInLanguage = true;
                                }
                            }
                        }
                    }

                    importJobData.AllUploadedFileColumn = lstAllFilter;
                    if (templateId > 0)
                    {
                        fileTemplates = importJobData.lstTemplates.FirstOrDefault(x => x.TemplateId == templateId);
                        importJobData.selectedTemplated = fileTemplates;
                        var TempColumns = fileTemplates.ColumnMappings.Split(',').ToList();
                        TempColumns.RemoveAll(x => (x) == "");

                        TempColumns = TempColumns.ConvertAll(x => x.ToLower());
                        var tempDtColumns = dtColumns.ConvertAll(x => x.ToLower());
                        bool count = TempColumns.Count == TempColumns.Intersect(tempDtColumns).Count();
                        sheetName = !string.IsNullOrEmpty(sheetName) ? sheetName.Split(new[] { "::" }, StringSplitOptions.None)[0] : "";
                        if (fileTemplates.ExcelWorksheetName.ToLower() != sheetName.ToLower())
                        {
                            count = false;
                        }
                        if (!string.IsNullOrEmpty(delimiter) && fileTemplates.Delimiter != delimiter)
                        {
                            count = false;
                        }
                        if (!count)
                        {
                            return new JsonResult { Data = DandBSettingLang.msgInvalidTemplate };
                        }
                        if (fileTemplates.IsUnicode != importJobData.IsUnicode)
                        {
                            return new JsonResult { Data = DandBSettingLang.msgUnicodeFormat };
                        }

                    }

                    // Get InpCompany Data Column Name
                    DataTable dtInpCompany = new DataTable();

                    if (ImportMode == "Match Refresh")
                    {
                        if (Helper.CurrentProvider == ProviderType.OI.ToString())
                        {
                            dtInpCompany = OIsfac.GetOIImportDataColumnsName();
                        }
                        else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                        {
                            dtInpCompany = sfac.GetImportDataRefreshColumnsName();
                        }
                    }
                    if (ImportMode == "Data Import")
                    {
                        if (Helper.CurrentProvider == ProviderType.OI.ToString())
                        {
                            dtInpCompany = orbfac.GetOIStgInputCompanyColumnsName();
                        }
                        else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                        {
                            dtInpCompany = sfac.GetInpCompanyColumnsName();
                        }
                    }

                    List<string> columnName = new List<string>();
                    if (dtInpCompany.Rows.Count > 0)
                    {
                        for (int k = 0; k < dtInpCompany.Rows.Count; k++)  //loop through the columns. 
                        {
                            if (Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportProcessId")
                            {
                                if (Helper.CurrentProvider == ProviderType.OI.ToString())
                                {
                                    if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                                    {
                                        columnName.Add("Street Line Address1");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                                    {
                                        columnName.Add("Street Line Address1");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address2")
                                    {
                                        columnName.Add("Street Line Address2");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                                    {
                                        columnName.Add("Street Line Alt. Address1");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                                    {
                                        columnName.Add("Street Line Alt. Address2");
                                    }
                                    else
                                    {
                                        columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                                    }
                                }
                                else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                                {
                                    if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                                    {
                                        columnName.Add("Street Line Address1");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                                    {
                                        columnName.Add("Street Line Address2");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                                    {
                                        columnName.Add("Street Line Alt. Address1");
                                    }
                                    else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                                    {
                                        columnName.Add("Street Line Alt. Address2");
                                    }
                                    else
                                    {
                                        columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                                    }
                                }
                            }
                        }
                    }

                    if (isFinish)
                    {
                        importJobData.MainColumnMapping = string.Join(",", columnName);
                        importJobData.ColumnMappings = fileTemplates.ColumnMappings;
                        importJobData.Tags = IsTag ? "" : fileTemplates.Tags;
                        importJobData.InLanguage = IsInLanguage ? "" : fileTemplates.InLanguage;
                        ViewBag.IsFromFinish = true;
                        SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                        return PartialView("ConfirmImportDetails", importJobData);
                    }

                    if (templateId > 0)
                    {
                        List<string> selectedValue = new List<string>();
                        List<string> selectedText = new List<string>();
                        foreach (var item in fileTemplates.ColumnMappings.Split(',').ToList())
                        {
                            if (string.IsNullOrEmpty(item))
                            {
                                selectedValue.Add("0");
                                selectedText.Add("");
                            }
                            else
                            {
                                var temp = lstAllFilter.FirstOrDefault(x => x.Text.ToLower() == item.ToLower());
                                if (temp != null)
                                {
                                    selectedValue.Add(temp.Value);
                                    selectedText.Add(dt.Rows[0][temp.Text].ToString());
                                }
                                else
                                {
                                    selectedValue.Add("0");
                                    selectedText.Add("");
                                }
                            }
                        }
                        ViewBag.SelectedVal = selectedValue;
                        ViewBag.SelectedText = selectedText;
                        ViewBag.TagValue = fileTemplates.Tags;
                        ViewBag.LanguageValue = fileTemplates.InLanguage;
                    }
                    SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                    ViewBag.ColumnList = columnName;
                    ViewBag.ExternalColumn = lstAllFilter;
                    ViewBag.IsContainsTags = IsTag;
                    SessionHelper.ImportData_IsTag = IsTag;
                    SessionHelper.ImportData_IsInLanguage = IsInLanguage;
                    SessionHelper.ImportData_IsHeader = IsHeader;
                    ViewBag.IsTemplate = templateId > 0;
                    IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 10000, 0);
                    ViewBag.fileType = importJobData.SourceType;
                    ViewBag.ImportMode = ImportMode;
                    ViewBag.IsTemplateSelected = IsTemplateSelected;
                    ViewBag.templateName = templateName;
                    return PartialView("ColumnMappingIndex", pagedProducts);
                }
                return new JsonResult { Data = ImportDataLang.msgUnableBlankFileImport };
            }
            else
            {
                return new JsonResult { Data = ImportDataLang.msgFileFormateSupported };
            }
        }
        [Authorize]
        public ActionResult ConfirmImportDetails(string[] OrgColumnName, string[] ExcelColumnName, string Tags = null, string InLanguage = null, string fileType = null)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            importJobData.MainColumnMapping = string.Join(",", ExcelColumnName);
            importJobData.ColumnMappings = string.Join(",", OrgColumnName).Replace("-Select-", "");
            importJobData.Tags = Tags;
            importJobData.InLanguage = InLanguage;
            bool IsTag = false;
            bool IsInLanguage = false;
            IsTag = SessionHelper.ImportData_IsTag;
            IsInLanguage = SessionHelper.ImportData_IsInLanguage;
            if (!IsTag)
                importJobData.ColumnMappings = importJobData.ColumnMappings.Replace("Tags", "");
            if (!IsInLanguage)
                importJobData.ColumnMappings = importJobData.ColumnMappings.Replace("InLanguage", "");

            List<string> clmMapping = importJobData.ColumnMappings.Split(',').AsEnumerable().Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> fileColumns = importJobData.AllUploadedFileColumn.Select(x => x.Text).ToList();
            fileColumns.Remove("-Select-");
            importJobData.UnMappedColumns = string.Join(",", fileColumns.Except(clmMapping)).Replace("-Select-,", "");
            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
            return PartialView(importJobData);
        }
        [Authorize]
        public ActionResult SaveTemplate(string[] OrgColumnName, string[] ExcelColumnName, string Tags = null, string InLanguage = null, string fileType = null, string TemplateName = null)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            ImportFileTemplates fileTemplates = new ImportFileTemplates();
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            if (string.IsNullOrEmpty(TemplateName))
                fileTemplates = importJobData.selectedTemplated;
            else
            {
                fileTemplates.TemplateId = 0;
                fileTemplates.TemplateName = TemplateName;
                fileTemplates.FileFormat = fileType;
                fileTemplates.ImportType = importJobData.ImportType;
                fileTemplates.HasHeader = importJobData.HasHeader;
                fileTemplates.Delimiter = importJobData.Delimiter;
                fileTemplates.IsUnicode = importJobData.IsUnicode;
            }

            fileTemplates.ImportType = importJobData.ImportType;
            fileTemplates.FileColumnMetadata = importJobData.FileColumnMetadata;
            fileTemplates.ExcelWorksheetName = importJobData.ExcelWorksheetName;
            fileTemplates.ColumnMappings = string.Join(",", OrgColumnName).Replace("-Select-", "");
            fileTemplates.Tags = Tags;
            fileTemplates.InLanguage = InLanguage;
            fileTemplates.UserId = Helper.oUser.UserId;

            bool IsTag = false;
            bool IsInLanguage = false;
            IsTag = SessionHelper.ImportData_IsTag;
            IsInLanguage = SessionHelper.ImportData_IsInLanguage;
            if (!IsTag)
                fileTemplates.ColumnMappings = fileTemplates.ColumnMappings.Replace("Tags", "");
            if (!IsInLanguage)
                fileTemplates.ColumnMappings = fileTemplates.ColumnMappings.Replace("InLanguage", "");

            List<string> clmMapping = fileTemplates.ColumnMappings.Split(',').AsEnumerable().Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> fileColumns = importJobData.AllUploadedFileColumn.Select(x => x.Text).ToList();
            fileColumns.Remove("-Select-");
            fileTemplates.UnMappedColumns = string.Join(",", fileColumns.Except(clmMapping)).Replace("-Select-,", "");


            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            string Message = efac.UpsertImportFileTemplates(fileTemplates);

            if (string.IsNullOrEmpty(Message) || !Message.StartsWith("Error"))
            {
                if (fileTemplates.TemplateId > 0)
                    Message = CommonMessagesLang.msgTemplateUpdated;
                else
                {
                    fileTemplates.TemplateId = Convert.ToInt32(Message);
                    Message = CommonMessagesLang.msgTemplateSaved;
                    importJobData.selectedTemplated = new ImportFileTemplates();
                    importJobData.selectedTemplated = fileTemplates;
                    SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                }
            }
            else if (!Message.Contains("please try with again with a different name."))
            {
                Message = CommonMessagesLang.msgSomethingWrong;
            }
            return new JsonResult { Data = Message + "::" + fileTemplates.TemplateId };
            //return Json(new { Data = Message, TemplateId = fileTemplates.TemplateId }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult SaveFileImportRequest()
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            importJobData.UserId = Helper.oUser.UserId;
            importJobData.ProcessStatusId = 0;
            bool IsTag = false;
            bool InLanguage = false;
            IsTag = SessionHelper.ImportData_IsTag;
            InLanguage = SessionHelper.ImportData_IsInLanguage;
            if (!IsTag)
                importJobData.ColumnMappings = importJobData.ColumnMappings.Replace("Tags", "");
            if (!InLanguage)
                importJobData.ColumnMappings = importJobData.ColumnMappings.Replace("InLanguage", "");


            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            bool result = efac.InsertNewFileImportRequest(importJobData);

            string directory = Server.MapPath("~/ImportDataFile/" + this.CurrentClient.ApplicationId + "/");
            string FilePath = directory + importJobData.SourceFileName;
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }

            SessionHelper.objimportJobData = null;
            SessionHelper.ImportFilePath = string.Empty;
            Session["ImportData_Data"] = null;
            SessionHelper.ImportData_IsHeader = false;
            SessionHelper.ImportData_IsInLanguage = false;
            SessionHelper.ImportData_IsTag = false;
            SessionHelper.ImportData_HeaderLine = string.Empty;
            SessionHelper.ImportData_LineData = string.Empty;
            if (result)
                return new JsonResult { Data = ImportDataLang.msgImportDataSuccessfully };
            else
                return new JsonResult { Data = ImportDataLang.msgImportDataError };
        }
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult UpdateExamples(string CurrentColumn)
        {
            string strValue = string.Empty;

            DataTable dt = new DataTable();
            if (Session["ImportData_Data"] != null)
            {
                dt = (Session["ImportData_Data"] as DataTable).AsEnumerable().Take(1).CopyToDataTable();
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                try
                {
                    if (Convert.ToInt32(CurrentColumn) > 0)
                    {
                        strValue = Convert.ToString(dt.Rows[0][Convert.ToInt32(CurrentColumn) - 1]);
                    }
                }
                catch
                {
                    //Empty catch block to stop from breaking
                }
            }
            return new JsonResult { Data = strValue };
        }

        public bool ValidateFilewithDelimiter(string File, string delimiter)
        {
            bool isFileValide = false;
            using (TextFieldParser txtReader = new TextFieldParser(File))
            {
                txtReader.SetDelimiters(delimiter);
                txtReader.HasFieldsEnclosedInQuotes = true;
                //read column names
                string[] colFields = null;

                colFields = txtReader.ReadFields();
                if (colFields.Count() > 2)
                {
                    isFileValide = true;
                }
            }
            return isFileValide;
        }

        #region "Fixed Width"
        [Authorize, HttpPost]
        public ActionResult UploadFixedFile(HttpPostedFileBase file, bool header, int templateId, string ImportMode, bool isFinish)
        {
            if (file != null && CommonMethod.CheckFileType(".txt", file.FileName.ToLower()))
            {
                ImportJobDataEntity importJobData = new ImportJobDataEntity();
                ImportFileTemplates fileTemplates = new ImportFileTemplates();
                string line = string.Empty;
                importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
                if (file.ContentLength > 0)
                {
                    DataTable dt = new DataTable();
                    string path = string.Empty;
                    try
                    {
                        string directory = Server.MapPath("~/ImportDataFile/" + this.CurrentClient.ApplicationId);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        FileInfo oFileInfo = new FileInfo(file.FileName);
                        string fileExtension = oFileInfo.Extension;
                        string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + System.DateTime.Now.Ticks + fileExtension;

                        string invalid = "+" + new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                        foreach (char c in invalid)
                        {
                            fileName = fileName.Replace(c.ToString(), "");
                        }

                        string hostName = Helper.hostName.Split('.')[0];
                        string containerName = "importdatafile";
                        Task.Run(() => CommonMethod.CreateAndUploadBlobFile(hostName, file, fileName, containerName));

                        importJobData.ClientFileName = file.FileName;
                        importJobData.SourcePath = ConfigurationManager.AppSettings["azurestoragepath"].ToString() + "importdatafile/" + hostName;
                        importJobData.SourceFileName = fileName;
                        importJobData.HasHeader = header;
                        path = Path.Combine(directory, Path.GetFileName(fileName.Replace("#", "")));
                        file.SaveAs(path);
                        SessionHelper.ImportFilePath = path;
                        if (templateId > 0)
                        {
                            fileTemplates = importJobData.lstTemplates.FirstOrDefault(x => x.TemplateId == templateId);
                            importJobData.selectedTemplated = fileTemplates;
                            dt = CommonImportMethods.ConvertFixedWidthToDataTable(path, fileTemplates.FileColumnMetadata, header, out line);
                            if (line == "Error occurred")
                            {
                                return new JsonResult { Data = CommonMessagesLang.msgErrorOccurred };
                            }

                            //Get Encoding of file
                            importJobData.IsUnicode = GetEncoding(path);


                            if (header)
                            {
                                Helper.InitLookups();
                                System.Collections.ArrayList columnsToRemove = new System.Collections.ArrayList();
                                foreach (var item in dt.Columns)
                                {
                                    if (item.ToString().StartsWith("Column") && dt.AsEnumerable().All(dr => dr.IsNull(item.ToString())))
                                    {
                                        columnsToRemove.Add(item.ToString());
                                    }
                                }
                                //remove all columns that are empty
                                foreach (string columnNamea in columnsToRemove)
                                {
                                    dt.Columns.Remove(columnNamea);
                                }

                                foreach (var item in dt.Columns)
                                {
                                    if (Helper.IsSpecialCharactersExist(item.ToString()))
                                    {
                                        return new JsonResult { Data = ImportDataLang.msgError + ":" + ImportDataLang.msgSpecialCharError };
                                    }
                                }
                            }
                            Session["ImportData_Data"] = dt;
                            SessionHelper.ImportData_LineData = line;
                            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);

                            bool IsTag = false;
                            bool IsInLanguage = false;
                            bool IsHeader = false;
                            //Get Import File Column Name to fill in dropdown
                            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
                            List<string> dtColumns = new List<string>();


                            if (dt.Rows.Count > 0)
                            {
                                lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                                int i = 0;
                                foreach (DataColumn c in dt.Columns)
                                {
                                    dtColumns.Add(c.ColumnName);
                                    lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                                    i++;
                                    if (!c.ToString().StartsWith("Column") && !c.ToString().StartsWith("column"))
                                    {
                                        IsHeader = true;
                                    }
                                    if (c.ColumnName.ToLower() == "tags" || c.ColumnName.ToLower() == "tag")
                                    {
                                        IsTag = true;
                                    }
                                    if (Helper.CurrentProvider != ProviderType.OI.ToString())
                                    {
                                        if (c.ColumnName.ToLower() == "language" || c.ColumnName.ToLower() == "language values" || c.ColumnName.ToLower() == "languagevalues" || c.ColumnName.ToLower() == "language code" || c.ColumnName.ToLower() == "languagecode")
                                        {
                                            IsInLanguage = true;
                                        }
                                    }

                                }
                            }
                            importJobData.AllUploadedFileColumn = lstAllFilter;
                            var TempColumns = fileTemplates.ColumnMappings.Split(',').ToList();
                            TempColumns.RemoveAll(x => (x) == "");
                            TempColumns = TempColumns.ConvertAll(x => x.ToLower());
                            var tempDtColumns = dtColumns.ConvertAll(x => x.ToLower());
                            bool count = TempColumns.Count == TempColumns.Intersect(tempDtColumns).Count();
                            if (!count)
                            {
                                return new JsonResult { Data = CommonMessagesLang.msgInvalidTemplate };
                            }
                            if (fileTemplates.IsUnicode != importJobData.IsUnicode)
                            {
                                return new JsonResult { Data = CommonMessagesLang.msgInvalidUnicodeFormat };
                            }

                            if (isFinish)
                            {
                                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                                OISettingFacade OIsfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                                OIImportDataFacade orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                                // Get InpCompany Data Column Name
                                DataTable dtInpCompany = new DataTable();
                                if (ImportMode == "Match Refresh")
                                {
                                    if (Helper.CurrentProvider == ProviderType.OI.ToString())
                                    {
                                        dtInpCompany = OIsfac.GetOIImportDataColumnsName();
                                    }
                                    else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                                    {
                                        dtInpCompany = sfac.GetImportDataRefreshColumnsName();
                                    }
                                }
                                if (ImportMode == "Data Import")
                                {
                                    if (Helper.CurrentProvider == ProviderType.OI.ToString())
                                    {
                                        dtInpCompany = orbfac.GetOIStgInputCompanyColumnsName();
                                    }
                                    else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                                    {
                                        dtInpCompany = sfac.GetInpCompanyColumnsName();
                                    }
                                }

                                List<string> columnName = new List<string>();
                                if (dtInpCompany.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dtInpCompany.Rows.Count; k++)  //loop through the columns. 
                                    {
                                        if (Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportProcessId")
                                        {
                                            if (Helper.CurrentProvider == ProviderType.OI.ToString())
                                            {
                                                if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                                                {
                                                    columnName.Add("Street Line Address1");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                                                {
                                                    columnName.Add("Street Line Address1");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address2")
                                                {
                                                    columnName.Add("Street Line Address2");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                                                {
                                                    columnName.Add("Street Line Alt. Address1");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                                                {
                                                    columnName.Add("Street Line Alt. Address2");
                                                }
                                                else
                                                {
                                                    columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                                                }
                                            }
                                            else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                                            {
                                                if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                                                {
                                                    columnName.Add("Street Line Address1");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                                                {
                                                    columnName.Add("Street Line Address2");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                                                {
                                                    columnName.Add("Street Line Alt. Address1");
                                                }
                                                else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                                                {
                                                    columnName.Add("Street Line Alt. Address2");
                                                }
                                                else
                                                {
                                                    columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                                                }
                                            }
                                        }
                                    }
                                }

                                importJobData.MainColumnMapping = string.Join(",", columnName);
                                importJobData.ColumnMappings = fileTemplates.ColumnMappings;
                                importJobData.FileColumnMetadata = fileTemplates.FileColumnMetadata;
                                importJobData.Tags = IsTag ? "" : fileTemplates.Tags;
                                importJobData.InLanguage = IsInLanguage ? "" : fileTemplates.InLanguage;
                                SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                                ViewBag.IsFromFinish = true;
                                return PartialView("ConfirmImportDetails", importJobData);
                            }

                            List<int> lstStartIndex = new List<int>();
                            List<int> lstFieldLength = new List<int>();
                            List<string> lstFieldValue = new List<string>();
                            foreach (var item in fileTemplates.FileColumnMetadata.Split(',').ToList())
                            {
                                var temp = item.Split('(')[1].Replace(")", "").Split('|');
                                lstStartIndex.Add(int.Parse(temp[0]));
                                lstFieldLength.Add(int.Parse(temp[1]));
                                if (!string.IsNullOrEmpty(line))
                                {
                                    lstFieldValue.Add(line.Substring(int.Parse(temp[0]) - 1, int.Parse(temp[1])));
                                }
                            }
                            ViewBag.IsTemplate = true;
                            ViewBag.IsFromPrev = false;
                            ViewBag.TagValue = fileTemplates.Tags;
                            ViewBag.LanguageValue = fileTemplates.InLanguage;
                            ViewBag.IsContainsTags = IsTag;
                            ViewBag.TemplateId = templateId;
                            ViewBag.lstStartIndex = lstStartIndex;
                            ViewBag.lstFieldLength = lstFieldLength;
                            ViewBag.lstFieldValue = lstFieldValue;
                            ViewBag.lstColumnNames = dtColumns;
                            ViewBag.fileType = importJobData.SourceType;
                            ViewBag.ImportMode = ImportMode;
                            SessionHelper.ImportData_IsTag = IsTag;
                            SessionHelper.ImportData_IsInLanguage = IsInLanguage;
                            SessionHelper.ImportData_IsHeader = IsHeader;
                            return PartialView("FixedFileMetaColumn");
                        }
                        else
                        {
                            SessionHelper.ImportData_IsHeader = header;
                            SessionHelper.ImportData_LineData = line;
                            ViewBag.IsTemplate = false;
                            ViewBag.IsFromPrev = false;
                            ViewBag.TemplateId = templateId;
                            ViewBag.fileType = importJobData.SourceType;
                            ViewBag.ImportMode = ImportMode;
                            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
                            return PartialView("FixedFileMetaColumn");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.IO.File.Delete(path);
                        if (ex.Message.Contains("already belongs to this DataTable"))
                        {
                            return new JsonResult { Data = ex.Message.Replace("DataTable", "file") };
                        }
                        else
                        {
                            return new JsonResult { Data = "Error:" + ex.Message };
                        }
                    }

                }
                else
                {
                    return new JsonResult { Data = ImportDataLang.msgUnableBlankFileImport };
                }
            }
            else
            {
                return new JsonResult
                {
                    Data = ImportDataLang.msgFileFormateSupported
                };
            }
        }

        [Authorize, HttpPost]
        public ActionResult FixedFileColumnMapping(string[] startIndex, string[] fieldLength, string[] fieldName, bool IsTemplate, string ImportMode, bool IsTemplateSelected = true, string templateName = null)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            ImportFileTemplates fileTemplates = new ImportFileTemplates();
            string line = string.Empty;
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            DataTable dt = new DataTable();
            string columnMetaData = string.Empty;
            bool header = importJobData.HasHeader;
            string path = Server.MapPath("~/ImportDataFile/" + this.CurrentClient.ApplicationId) + "\\" + importJobData.SourceFileName;
            if (IsTemplate)
            {
                fileTemplates = importJobData.selectedTemplated;
            }
            for (int i = 0; i < fieldName.Length; i++)
            {
                columnMetaData = (string.IsNullOrEmpty(columnMetaData) ? columnMetaData : columnMetaData + ",") + fieldName[i] + "(" + startIndex[i] + "|" + fieldLength[i] + ")";
            }
            importJobData.FileColumnMetadata = columnMetaData;
            dt = CommonImportMethods.ConvertFixedWidthToDataTable(path, columnMetaData, header, out line);
            if (line == "Error occurred")
            {
                return new JsonResult { Data = CommonMessagesLang.msgErrorOccurred };
            }

            importJobData.IsUnicode = GetEncoding(path);

            if (header)
            {
                System.Collections.ArrayList columnsToRemove = new System.Collections.ArrayList();
                foreach (var item in dt.Columns)
                {
                    if (item.ToString().StartsWith("Column") && dt.AsEnumerable().All(dr => dr.IsNull(item.ToString())))
                    {
                        columnsToRemove.Add(item.ToString());
                    }
                }
                //remove all columns that are empty
                foreach (string columnNamea in columnsToRemove)
                {
                    dt.Columns.Remove(columnNamea);
                }
            }
            Session["ImportData_Data"] = dt;
            SessionHelper.ImportData_LineData = line;
            SessionHelper.ImportFilePath = path;

            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            OISettingFacade OIsfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            OIImportDataFacade orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            bool IsTag = false;
            bool IsInLanguage = false;
            bool IsHeader = false;
            //Get Import File Column Name to fill in dropdown
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            List<string> dtColumns = new List<string>();
            if (dt.Rows.Count > 0)
            {
                lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                int i = 0;
                List<string> fieldnm = new List<string>();
                fieldnm = fieldName.ToList().ConvertAll(x => x.ToLower());
                foreach (DataColumn c in dt.Columns)
                {
                    if (fieldnm.Any(x => x == c.ColumnName.ToLower()))
                    {
                        dtColumns.Add(c.ColumnName);
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;
                        if (!c.ToString().StartsWith("Column") && !c.ToString().StartsWith("column"))
                        {
                            IsHeader = true;
                        }
                        if (c.ColumnName.ToLower() == "tags" || c.ColumnName.ToLower() == "tag")
                        {
                            IsTag = true;
                        }
                        if (Helper.CurrentProvider != ProviderType.OI.ToString())
                        {
                            if (c.ColumnName.ToLower() == "language" || c.ColumnName.ToLower() == "language values" || c.ColumnName.ToLower() == "languagevalues" || c.ColumnName.ToLower() == "language code" || c.ColumnName.ToLower() == "languagecode")
                            {
                                IsInLanguage = true;
                            }
                        }

                    }
                }
            }
            importJobData.AllUploadedFileColumn = lstAllFilter;
            SessionHelper.objimportJobData = Newtonsoft.Json.JsonConvert.SerializeObject(importJobData);
            var TempColumns = fieldName.ToList();
            TempColumns.RemoveAll(x => (x) == "");

            TempColumns = TempColumns.ConvertAll(x => x.ToLower());
            var tempDtColumns = dtColumns.ConvertAll(x => x.ToLower());
            bool count = TempColumns.Count == TempColumns.Intersect(tempDtColumns).Count();
            if (!count)
            {
                return new JsonResult { Data = ImportDataLang.msgInvalidFieldMapping };
            }

            DataTable dtInpCompany = new DataTable();
            if (ImportMode == "Match Refresh")
            {
                if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    dtInpCompany = OIsfac.GetOIImportDataColumnsName();
                }
                else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    dtInpCompany = sfac.GetImportDataRefreshColumnsName();
                }
            }
            if (ImportMode == "Data Import")
            {
                if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    dtInpCompany = orbfac.GetOIStgInputCompanyColumnsName();
                }
                else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    dtInpCompany = sfac.GetInpCompanyColumnsName();
                }
            }

            List<string> columnName = new List<string>();
            if (dtInpCompany.Rows.Count > 0)
            {
                for (int k = 0; k < dtInpCompany.Rows.Count; k++)  //loop through the columns. 
                {
                    if (Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportProcessId")
                    {
                        if (Helper.CurrentProvider == ProviderType.OI.ToString())
                        {
                            if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                            {
                                columnName.Add("Street Line Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                            {
                                columnName.Add("Street Line Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address2")
                            {
                                columnName.Add("Street Line Address2");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                            {
                                columnName.Add("Street Line Alt. Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                            {
                                columnName.Add("Street Line Alt. Address2");
                            }
                            else
                            {
                                columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                            }
                        }
                        else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                        {
                            if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                            {
                                columnName.Add("Street Line Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                            {
                                columnName.Add("Street Line Address2");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                            {
                                columnName.Add("Street Line Alt. Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                            {
                                columnName.Add("Street Line Alt. Address2");
                            }
                            else
                            {
                                columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                            }
                        }

                    }
                }
            }

            if (IsTemplate)
            {
                List<string> selectedValue = new List<string>();
                List<string> selectedText = new List<string>();
                foreach (var item in fileTemplates.ColumnMappings.Split(',').ToList())
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        selectedValue.Add("0");
                        selectedText.Add("");
                    }
                    else
                    {
                        var temp = lstAllFilter.FirstOrDefault(x => x.Text.ToLower() == item.ToLower());
                        if (temp != null)
                        {
                            selectedValue.Add(temp.Value);
                            selectedText.Add(dt.Rows[0][temp.Text].ToString());
                        }
                        else
                        {
                            selectedValue.Add("0");
                            selectedText.Add("");
                        }
                    }
                }
                ViewBag.SelectedVal = selectedValue;
                ViewBag.SelectedText = selectedText;
                ViewBag.TagValue = fileTemplates.Tags;
                ViewBag.LanguageValue = fileTemplates.InLanguage;
            }

            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;
            ViewBag.IsContainsTags = IsTag;
            SessionHelper.ImportData_IsTag = IsTag;
            SessionHelper.ImportData_IsInLanguage = IsInLanguage;
            SessionHelper.ImportData_IsHeader = IsHeader;
            ViewBag.IsTemplate = IsTemplate;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 10000, 0);
            ViewBag.fileType = importJobData.SourceType;
            ViewBag.ImportMode = ImportMode;
            ViewBag.IsTemplateSelected = IsTemplateSelected;
            ViewBag.templateName = templateName;
            return PartialView("ColumnMappingIndex", pagedProducts);
        }

        public JsonResult UpdateFixedExamples(int startIndex, int fieldLength, string fieldName)
        {
            string strValue = string.Empty;
            string lineData = string.Empty;
            if (!string.IsNullOrEmpty(SessionHelper.ImportData_LineData))
            {
                lineData = SessionHelper.ImportData_LineData;
            }
            if (!string.IsNullOrEmpty(lineData))
            {
                try
                {
                    strValue = lineData.Substring(startIndex - 1, fieldLength);
                }
                catch
                {
                    strValue = "";
                }

            }
            else if (!string.IsNullOrEmpty(SessionHelper.ImportFilePath))
            {
                using (TextFieldParser txtReader = new TextFieldParser(SessionHelper.ImportFilePath))
                {
                    bool header = SessionHelper.ImportData_IsHeader;
                    string line = txtReader.ReadLine();
                    string secondLine = txtReader.ReadLine();
                    if (header)
                        lineData = secondLine;
                    else
                        lineData = line;
                    SessionHelper.ImportData_LineData = lineData;
                    strValue = lineData.Substring(startIndex - 1, fieldLength);
                }
            }
            return new JsonResult { Data = strValue };
        }
        public JsonResult UpdateFixedColumnNames(int startIndex, int fieldLength, string fieldName)
        {
            string strValue = string.Empty;
            string lineData = string.Empty;
            if (!string.IsNullOrEmpty(SessionHelper.ImportData_HeaderLine))
            {
                lineData = SessionHelper.ImportData_HeaderLine;
            }
            if (!string.IsNullOrEmpty(lineData))
            {
                try
                {
                    strValue = lineData.Substring(startIndex - 1, fieldLength).Trim();
                }
                catch
                {
                    strValue = "";
                }

            }
            else if (!string.IsNullOrEmpty(SessionHelper.ImportFilePath))
            {
                using (TextFieldParser txtReader = new TextFieldParser(SessionHelper.ImportFilePath))
                {
                    lineData = txtReader.ReadLine();
                    SessionHelper.ImportData_HeaderLine = lineData;
                    strValue = lineData.Substring(startIndex - 1, fieldLength).Trim();
                }
            }
            return new JsonResult { Data = strValue };
        }
        #endregion

        #region Tags
        #region Add Tags and Validate Tags
        [HttpGet]
        public ActionResult AddTags(bool isAllowLOBTag = true)
        {
            if (!Helper.LicenseEnableTags)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.isAllowLOBTag = isAllowLOBTag;
            return View();
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult AddTags(string Parameters)
        {
            // Decrypted Value and fill the parameter value 
            string Tags = "", Tag = "", TagTypeCode = "", LOBTags = "";
            int TagId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Tag = "[" + Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) + "::" + Tags + "]";
                TagTypeCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                LOBTags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1).Replace("&#58&#58", "::");
                TagId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
            }
            string strOption = "";
            if (!string.IsNullOrEmpty(Tags.Trim()) && !string.IsNullOrEmpty(TagTypeCode.Trim()))
            {
                if (CommonMethod.isValidTagName(Tags))
                {

                    TagsEntity objTags = new TagsEntity();
                    objTags.TagId = TagId;
                    objTags.TagValue = Tags.Replace("[", "").Replace("]", "");
                    objTags.Tag = Tag;
                    objTags.TagTypeCode = TagTypeCode;
                    objTags.CreatedUserId = Convert.ToInt32(User.Identity.GetUserId());
                    objTags.LOBTag = LOBTags;
                    TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
                    if (TagId > 0)
                    {
                        fac.UpdateTags(objTags);
                        strOption = CommonMessagesLang.msgCommanUpdateMessage;
                    }
                    else
                    {
                        if (!IsTasExists(Tag))// Validate tag is already exists or not
                        {
                            // Insert Tag into database.
                            fac.InsertTags(objTags, Helper.oUser.UserId);
                            strOption = Tag;
                        }
                        else
                        {
                            strOption = "ExistsFail";
                        }
                    }
                }
                else
                {
                    strOption = CommonMessagesLang.msgValidCharacters;
                }
            }
            return new JsonResult { Data = strOption };
        }
        public bool IsTasExists(string tagValue)
        {
            // Validate tag is already exists or not
            bool IsExists = false;
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            int TagId = fac.GetAllTags("").Where(x => x.Tag.ToLower() == tagValue.ToLower()).Select(x => x.TagId).FirstOrDefault();
            if (TagId > 0)
            {
                IsExists = true;
            }
            return IsExists;
        }
        #endregion
        #endregion

        #region "Prev button methods"

        public ActionResult BackToColumnMapping(bool IsTemplateSelected = true)
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            List<string> columnName = new List<string>();
            columnName = importJobData.MainColumnMapping.Split(',').ToList();
            if (importJobData.selectedTemplated != null && importJobData.selectedTemplated.TemplateId > 0)
            {
                ViewBag.IsTemplate = true;
            }
            else
            {
                ViewBag.IsTemplate = false;
            }
            DataTable dt = Session["ImportData_Data"] as DataTable;

            List<string> selectedValue = new List<string>();
            List<string> selectedText = new List<string>();
            foreach (var item in importJobData.ColumnMappings.Split(',').ToList())
            {
                if (string.IsNullOrEmpty(item))
                {
                    selectedValue.Add("0");
                    selectedText.Add("");
                }
                else
                {
                    var temp = importJobData.AllUploadedFileColumn.FirstOrDefault(x => x.Text.ToLower() == item.ToLower());
                    if (temp != null)
                    {
                        selectedValue.Add(temp.Value);
                        selectedText.Add(dt.Rows[0][temp.Text].ToString());
                    }
                    else
                    {
                        selectedValue.Add("0");
                        selectedText.Add("");
                    }
                }
            }

            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = importJobData.AllUploadedFileColumn;
            ViewBag.SelectedVal = selectedValue;
            ViewBag.SelectedText = selectedText;
            ViewBag.TagValue = importJobData.Tags;
            ViewBag.LanguageValue = importJobData.InLanguage;
            ViewBag.IsContainsTags = importJobData.ColumnMappings.Contains("Tags");
            SessionHelper.ImportData_IsTag = importJobData.ColumnMappings.Contains("Tags");
            SessionHelper.ImportData_IsInLanguage = importJobData.ColumnMappings.Contains("InLanguage");
            SessionHelper.ImportData_IsHeader = importJobData.HasHeader;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 10000, 0);
            ViewBag.fileType = importJobData.SourceType;
            ViewBag.ImportMode = importJobData.ImportType.Contains("MATCH") ? "Data Import" : "Match Refresh";
            ViewBag.IsFromPrev = true;
            ViewBag.IsTemplateSelected = IsTemplateSelected;
            return PartialView("ColumnMappingIndex", pagedProducts);
        }

        public ActionResult BackToFixedFileMetaColumn()
        {
            ImportJobDataEntity importJobData = new ImportJobDataEntity();
            ImportFileTemplates fileTemplates = new ImportFileTemplates();
            string line = string.Empty;
            line = SessionHelper.ImportData_LineData;
            bool IsTemplate = false;
            importJobData = !string.IsNullOrEmpty(SessionHelper.objimportJobData) ? Newtonsoft.Json.JsonConvert.DeserializeObject<ImportJobDataEntity>(SessionHelper.objimportJobData) : new ImportJobDataEntity();
            DataTable dt = Session["ImportData_Data"] as DataTable;
            if (importJobData.selectedTemplated != null && importJobData.selectedTemplated.TemplateId > 0)
            {
                IsTemplate = true;
            }
            else
            {
                IsTemplate = false;
            }
            List<string> dtColumns = new List<string>();
            List<int> lstStartIndex = new List<int>();
            List<int> lstFieldLength = new List<int>();
            List<string> lstFieldValue = new List<string>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn c in dt.Columns)
                {
                    dtColumns.Add(c.ColumnName);
                }
            }

            foreach (var item in importJobData.FileColumnMetadata.Split(',').ToList())
            {
                var temp = item.Split('(')[1].Replace(")", "").Split('|');
                lstStartIndex.Add(int.Parse(temp[0]));
                lstFieldLength.Add(int.Parse(temp[1]));
                if (!string.IsNullOrEmpty(line))
                {
                    lstFieldValue.Add(line.Substring(int.Parse(temp[0]) - 1, int.Parse(temp[1])));
                }
            }

            ViewBag.IsTemplate = IsTemplate;
            ViewBag.TagValue = importJobData.Tags;
            ViewBag.LanguageValue = importJobData.InLanguage;
            ViewBag.IsContainsTags = importJobData?.ColumnMappings?.Contains("Tags");
            ViewBag.lstStartIndex = lstStartIndex;
            ViewBag.lstFieldLength = lstFieldLength;
            ViewBag.lstFieldValue = lstFieldValue;
            ViewBag.lstColumnNames = dtColumns;
            ViewBag.fileType = importJobData.SourceType;
            ViewBag.IsFromPrev = true;
            ViewBag.ImportMode = importJobData.ImportType.Contains("MATCH") ? "Data Import" : "Match Refresh";
            if (importJobData.ColumnMappings != null)
            {
                SessionHelper.ImportData_IsTag = (bool)importJobData?.ColumnMappings?.Contains("Tags");
                SessionHelper.ImportData_IsInLanguage = (bool)importJobData?.ColumnMappings?.Contains("InLanguage");
            }
            else
            {
                SessionHelper.ImportData_IsTag = false;
                SessionHelper.ImportData_IsInLanguage = false;
            }
            SessionHelper.ImportData_IsHeader = importJobData.HasHeader;
            return PartialView("FixedFileMetaColumn");

        }

        #endregion

        #region "Single form entry Data Import"
        [Authorize]
        public ActionResult SingleFormEntryMain()
        {
            return View();
        }

        [Authorize, HttpGet]
        public ActionResult SingleFormEntry(bool IsPartial = false)
        {
            InpCompanyEntity model = new InpCompanyEntity();
            // Open Single Form Entry Form
            ViewBag.Message = "";
            if (IsPartial)
                return PartialView("_SingleFormEntry", model);
            else
                return PartialView(model);
        }

        [Authorize, HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult SingleFormEntry(InpCompanyEntity model)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            if (ModelState.IsValid)
            {
                try
                {
                    //fill the data in DataImportProcess  Table 
                    DataImportProcessEntity objDataImport = new DataImportProcessEntity();
                    objDataImport.UserId = Convert.ToInt32(User.Identity.GetUserId());
                    objDataImport.SourceType = "D&B";
                    objDataImport.Source = "Manual Entry";
                    objDataImport.ImportedRowCount = 1;
                    int ImportProcessId = fac.InsertDataImportProcess(objDataImport);

                    model.ImportProcessId = ImportProcessId;
                    int result = fac.InsertStgInputCompany(model);
                    string Message = fac.ProcessDataImport(ImportProcessId);
                    if (result > 0)
                    {
                        return Json(new
                        {
                            result = true,
                            message = string.IsNullOrEmpty(Message) ? ImportDataLang.msgDataImportSuccessfully : Message
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
            }
        }



        #endregion

        #region "Single Form Entry Form for Data Refresh"
        [Authorize, HttpGet]
        // Open Single Form Entry Form for Data Refresh
        public ActionResult SingleMatchRefreshFormEntry()
        {
            InpCompanyDataRefershEntity model = new InpCompanyDataRefershEntity();

            ViewBag.Message = "";

            return View("_SingleMatchRefreshFormEntry", model);
        }
        [Authorize, HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult SingleMatchRefreshFormEntry(InpCompanyDataRefershEntity model)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            if (ModelState.IsValid)
            {
                try
                {
                    DataImportProcessEntity objDataImport = new DataImportProcessEntity();
                    objDataImport.UserId = Convert.ToInt32(User.Identity.GetUserId());
                    objDataImport.SourceType = "D&B";
                    objDataImport.Source = "Manual Entry";
                    objDataImport.ImportedRowCount = 1;
                    //save data of single entry form of match refresh
                    int ImportProcessId = fac.InsertDataImportProcess(objDataImport);
                    model.ImportProcessId = ImportProcessId;
                    int result = fac.InsertStgImportDataRefresh(model);
                    string Message = fac.ImportDataRefresh(ImportProcessId);

                    if (result > 0)
                    {
                        return Json(new { result = true, message = string.IsNullOrEmpty(Message) ? ImportDataLang.msgDataImportSuccessfully : Message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Orb Single Entry Form"
        #region "Cleanse, Match & Enrichment"
        //ORB Single entry form Cleanse,Match & Enrichment
        [Authorize]
        public ActionResult OISingleEntryForm(bool IsPartial = false)
        {
            OIInpCompanyEntity model = new OIInpCompanyEntity();
            if (IsPartial)
                return View("~/Views/OI/OIData/_OISingleEntryForm.cshtml", model);
            else
                return View("~/Views/OI/OIData/OISingleEntryForm.cshtml", model);

        }
        [Authorize, HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult OISingleEntryForm(OIInpCompanyEntity model)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            OIImportDataFacade Orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (ModelState.IsValid)
            {
                try
                {
                    // fill the data in DataImportProcess  Table 
                    DataImportProcessEntity objDataImport = new DataImportProcessEntity();
                    objDataImport.UserId = Convert.ToInt32(User.Identity.GetUserId());
                    objDataImport.SourceType = "OI";
                    objDataImport.Source = "Manual Entry";
                    objDataImport.ImportedRowCount = 1;

                    // insert ImportProcessId
                    int ImportProcessId = fac.InsertDataImportProcess(objDataImport);

                    // if ImportProcessId value is greater than 0 
                    model.ImportProcessId = ImportProcessId;
                    int result = Orbfac.InsertOIStgInputCompany(model);
                    string Message = Orbfac.OIProcessDataImport(ImportProcessId);
                    if (result > 0)
                    {
                        // Message showing data import done successfully
                        return Json(new
                        {
                            result = true,
                            message = string.IsNullOrEmpty(Message) ? ImportDataLang.msgDataImportSuccessfully : Message
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        // Error message showing data not imported in database
                        return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ViewBag.Message = DandBSettingLang.msgInvadilState;
                return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);

            }

        }
        #endregion
        #region "Match Refresh"
        //ORB Single entry form import for Match Refresh
        [Authorize, HttpGet]
        public ActionResult OISingleEntryMatchRefresh()
        {
            OIInpCompanyEntityMatchRefresh model = new OIInpCompanyEntityMatchRefresh();
            return View("~/Views/OI/OIData/OISingleEntryFormMatchRefresh.cshtml", model);
        }
        [Authorize, HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult OISingleEntryMatchRefresh(OIInpCompanyEntityMatchRefresh model)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            OIImportDataFacade Orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (ModelState.IsValid)
            {       //fill the data in DataImportProcess  Table 
                DataImportProcessEntity objDataImport = new DataImportProcessEntity();
                objDataImport.UserId = Convert.ToInt32(User.Identity.GetUserId());
                objDataImport.SourceType = "OI";
                objDataImport.Source = "Manual Entry";
                objDataImport.ImportedRowCount = 1;
                int ImportProcessId = fac.InsertDataImportProcess(objDataImport);
                model.ImportProcessId = ImportProcessId;
                int result = Orbfac.InsertOIStgInputCompanyMatchRefresh(model);
                string Message = Orbfac.OIProcessDataForEnrichment(ImportProcessId);
                if (result > 0)
                {
                    // Message showing DataImport done Successfully
                    return Json(new { result = true, message = string.IsNullOrEmpty(Message) ? ImportDataLang.msgDataImportSuccessfully : Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Error message if import fails
                    return Json(new { result = false, message = ImportDataLang.msgImportDataError }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #endregion

        private static bool? GetEncoding(string filename)
        {
            using (var reader = new StreamReader(filename, Encoding.Default, true))
            {
                if (reader.Peek() >= 0)
                    reader.Read();
                var encoding = reader.CurrentEncoding;
                if (encoding.EncodingName.ToLower().Contains("unicode"))
                    return true;
                else
                    return false;
            }
        }

        public JsonResult GetUserList()
        {
            List<DropDownReturn> lstGetAllUser = new List<DropDownReturn>();
            List<UsersEntity> lstUsers = new List<UsersEntity>();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstUsers = fac.GetUsersList();
            foreach (var item in lstUsers)
            {
                lstGetAllUser.Add(new DropDownReturn { Value = item.UserId.ToString(), Text = item.UserName });
            }
            return Json(new { Data = lstGetAllUser }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStatus()
        {
            List<DropDownReturn> lstStatus = new List<DropDownReturn>();
            lstStatus.Add(new DropDownReturn { Value = "Processed", Text = "Processed" });
            lstStatus.Add(new DropDownReturn { Value = "Import Failed", Text = "Import Failed" });
            return Json(new { Data = lstStatus }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImportTypes()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            if (Helper.CurrentProvider == ProviderType.DandB.ToString())
            {
                lstType.Add(new DropDownReturn { Value = "D&B Cleanse Match & Enrich", Text = "D&B Cleanse Match & Enrich" });
                lstType.Add(new DropDownReturn { Value = "D&B Enrichment Only", Text = "D&B Enrichment Only" });
            }
            else if (Helper.CurrentProvider == ProviderType.OI.ToString())
            {
                lstType.Add(new DropDownReturn { Value = "Orb Intelligence Cleanse Match & Enrich", Text = "Orb Intelligence Cleanse Match & Enrich" });
                lstType.Add(new DropDownReturn { Value = "Orb Intelligence Enrichment Only", Text = "Orb Intelligence Enrichment Only" });
            }

            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFilesDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            if (Helper.oUser.EnableImportData)
            {
                lstType.Add(new DropDownReturn { Value = "My Files", Text = "My Files" });
            }
            lstType.Add(new DropDownReturn { Value = "All Files", Text = "All Files" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRequestedDate()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "1D", Text = "Last 1 Day" });
            lstType.Add(new DropDownReturn { Value = "3D", Text = "Last 3 Days" });
            lstType.Add(new DropDownReturn { Value = "7D", Text = "Last 7 Days" });
            lstType.Add(new DropDownReturn { Value = "customdate", Text = "Custom" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterImportData(List<FilterData> filters)
        {
            List<ImportJobDataEntity> lstImportjobs = new List<ImportJobDataEntity>();
            if (string.IsNullOrEmpty(SessionHelper.ImportJobTableList))
            {
                ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                lstImportjobs = efac.GetNewFileImportRequestByUserID(0, Helper.CurrentProvider);
                SessionHelper.ImportJobTableList = Newtonsoft.Json.JsonConvert.SerializeObject(lstImportjobs);
            }
            else
            {
                lstImportjobs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImportJobDataEntity>>(SessionHelper.ImportJobTableList);
            }
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "User")
                        {
                            lstImportjobs = lstImportjobs.Where(x => valLst.Contains(x.UserId.ToString())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportjobs = lstImportjobs.Where(x => valLst.Contains(x.ImportType.ToString())).ToList();
                        }
                        else if (item.FieldName == "Status")
                        {
                            lstImportjobs = lstImportjobs.Where(x => valLst.Contains(x.ProcessStatus.ToString())).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.Message) && x.Message.ToLower() == item.FilterValue.ToLower()).ToList();
                        }
                        else if (item.FieldName == "ErrorMessage")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.ErrorMessage) && x.ErrorMessage.ToLower() == item.FilterValue.ToLower()).ToList();
                        }
                        else if (item.FieldName == "Files")
                        {
                            if (item.FilterValue == "My Files")
                            {
                                lstImportjobs = lstImportjobs.Where(x => x.UserId == Helper.oUser.UserId).ToList();
                            }
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                lstImportjobs = lstImportjobs.Where(x => x.RequestedDateTime >= Convert.ToDateTime(startDate) && x.RequestedDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay)).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                lstImportjobs = lstImportjobs.Where(x => x.RequestedDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue) * -1)).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "notEqualTo")
                    {
                        if (item.FieldName == "User")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !valLst.Contains(x.UserId.ToString())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !valLst.Contains(x.ImportType.ToString())).ToList();
                        }
                        else if (item.FieldName == "Status")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !valLst.Contains(x.ProcessStatus.ToString())).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.Message) && x.Message.ToLower() != item.FilterValue.ToLower()).ToList();
                        }
                        else if (item.FieldName == "ErrorMessage")
                        {
                            lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.ErrorMessage) && x.ErrorMessage.ToLower() != item.FilterValue.ToLower()).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                lstImportjobs = lstImportjobs.Where(x => !(x.RequestedDateTime >= Convert.ToDateTime(startDate) && x.RequestedDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay))).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                lstImportjobs = lstImportjobs.Where(x => !(x.RequestedDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue) * -1))).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "contains")
                    {
                        if (item.FieldName == "User")
                        {
                            lstImportjobs = lstImportjobs.Where(x => x.UserId.ToString() == item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => x.ImportType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "Status")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => x.ProcessStatus.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.Message) && x.Message.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "ErrorMessage")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => !string.IsNullOrEmpty(x.ErrorMessage) && x.ErrorMessage.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                lstImportjobs = lstImportjobs.Where(x => x.RequestedDateTime >= Convert.ToDateTime(startDate) && x.RequestedDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay)).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                lstImportjobs = lstImportjobs.Where(x => x.RequestedDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue))).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "notContains")
                    {
                        if (item.FieldName == "User")
                        {
                            lstImportjobs = lstImportjobs.Where(x => x.UserId.ToString() != item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => !x.ImportType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "Status")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => !x.ProcessStatus.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => string.IsNullOrEmpty(x.Message) || (!string.IsNullOrEmpty(x.Message) && !x.Message.ToLower().Contains(item.FilterValue.ToLower()))).ToList();
                        }
                        else if (item.FieldName == "ErrorMessage")
                        {
                            if (!string.IsNullOrEmpty(item.FilterValue))
                                lstImportjobs = lstImportjobs.Where(x => string.IsNullOrEmpty(x.ErrorMessage) || (!string.IsNullOrEmpty(x.ErrorMessage) && !x.ErrorMessage.ToLower().Contains(item.FilterValue.ToLower()))).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                lstImportjobs = lstImportjobs.Where(x => !(x.RequestedDateTime >= Convert.ToDateTime(startDate) && x.RequestedDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay))).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                lstImportjobs = lstImportjobs.Where(x => !(x.RequestedDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue)))).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Empty catch block to stop from breaking
            }

            IPagedList<ImportJobDataEntity> pagedImportJobSettings = new StaticPagedList<ImportJobDataEntity>(lstImportjobs, 1, 50, lstImportjobs.Count);
            return PartialView("FileImportRequestList", pagedImportJobSettings);
        }

        public ActionResult GetFileDetails(string Parameters)
        {
            int FileId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                FileId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DashboardImportProcessDataQueueStatisticsEntity> dashboardDataQueue = fac.DashboardV2GetDataQueueStatisticsByImportProcess(FileId);
            DashboardImportProcessDataQueueStatisticsEntity dataQueueStatisticsEntity = new DashboardImportProcessDataQueueStatisticsEntity();
            if (dashboardDataQueue != null && dashboardDataQueue.Any())
            {
                dataQueueStatisticsEntity = dashboardDataQueue[0];
                string viewString = RenderRazorViewToString("_ImportFileStats", dataQueueStatisticsEntity);
                return Json(new { result = true, message = viewString }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgNoDataInSystem }, JsonRequestBehavior.AllowGet);
            }
        }

        #region "TemplateDetails"
        public ActionResult ImportFileTemplatesList()
        {
            SessionHelper.lstImportFileTemplates = null;
            return View();
        }

        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult RemoveTemplateData(string Parameters)
        {
            int TemplateId = 0;
            string TemplateName = string.Empty;
            int UserId = Helper.oUser.UserId;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                TemplateId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                TemplateName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                efac.DeleteImportFileTemplateByTemplateId(TemplateId,TemplateName, UserId) ;
                SessionHelper.lstImportFileTemplates = null;
                return Json(CommonMessagesLang.msgCommanDeleteMessage);
            }
            return Json("");
        }


        public ActionResult GetTemplateDetails(string Parameters)
        {
            int templateId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                templateId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }
            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            ImportFileTemplates templates = efac.GetImportFileTemplateByTemplateId(templateId);
            templates.IsUnicode = templates.IsUnicode == null ? false : templates.IsUnicode;

            if (templates.ImportType.Contains("Enrichment Only"))
            {
                templates.ImportType = "Match Refresh";
            }
            else
            {
                templates.ImportType = "Data Import";
            }
            // Get InpCompany Data Column Name
            DataTable dtInpCompany = new DataTable();

            OISettingFacade OIsfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            OIImportDataFacade orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (templates.ImportType == "Match Refresh")
            {
                if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    dtInpCompany = OIsfac.GetOIImportDataColumnsName();
                }
                else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    dtInpCompany = sfac.GetImportDataRefreshColumnsName();
                }
            }
            if (templates.ImportType == "Data Import")
            {
                if (Helper.CurrentProvider == ProviderType.OI.ToString())
                {
                    dtInpCompany = orbfac.GetOIStgInputCompanyColumnsName();
                }
                else if (Helper.CurrentProvider == ProviderType.DandB.ToString())
                {
                    dtInpCompany = sfac.GetInpCompanyColumnsName();
                }
            }

            string[] arrColumn = templates.ColumnMappings.Split(',');
            List<string> lstMappingColumn = arrColumn.ToList();
            lstMappingColumn.RemoveAll(x => (x) == "");
            templates.lstMappingColumn = new List<string>();
            templates.lstMappingColumn.AddRange(lstMappingColumn);
            List<ColumnClass> columnName = new List<ColumnClass>();
            if (dtInpCompany.Rows.Count > 0)
            {
                for (int k = 0; k < dtInpCompany.Rows.Count; k++)  //loop through the columns. 
                {
                    if (Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportProcessId")
                    {
                        if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address")
                        {
                            columnName.Add(new ColumnClass { DBColumn = "Street Line Address1" });

                        }
                        else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                        {
                            columnName.Add(new ColumnClass { DBColumn = "Street Line Address2" });
                        }
                        else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress")
                        {
                            columnName.Add(new ColumnClass { DBColumn = "Street Line Alt. Address1" });
                        }
                        else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                        {
                            columnName.Add(new ColumnClass { DBColumn = "Street Line Alt. Address2" });
                        }
                        else
                        {
                            columnName.Add(new ColumnClass { DBColumn = Convert.ToString(dtInpCompany.Rows[k][0]) });
                        }
                    }
                }
            }
            for (int cnt = 0; cnt < arrColumn.Length; cnt++)
            {
                columnName[cnt].ExcelColumn = arrColumn[cnt];
            }
            templates.lstcolumnName = new List<ColumnClass>();
            templates.lstcolumnName.AddRange(columnName);
            return PartialView("_TemplateDetail", templates);
        }


        public ActionResult FilterImportDataTemplateList(List<FilterData> filters)
        {
            List<ImportFileTemplates> lstImportFileTemplates = new List<ImportFileTemplates>();
            if (string.IsNullOrEmpty(SessionHelper.lstImportFileTemplates))
            {
                ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                lstImportFileTemplates = efac.GetAllImportFileTemplates();
                SessionHelper.lstImportFileTemplates = Newtonsoft.Json.JsonConvert.SerializeObject(lstImportFileTemplates);
            }
            else
            {
                lstImportFileTemplates = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImportFileTemplates>>(SessionHelper.lstImportFileTemplates);
            }
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "Format")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => valLst.Contains(x.FileFormat.ToString())).ToList();
                        }
                        else if (item.FieldName == "TemplateName")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => valLst.Contains(x.TemplateName.ToString())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => valLst.Contains(x.ImportType.ToString())).ToList();
                        }
                    }
                    else if (item.Operator == "notEqualTo")
                    {
                        if (item.FieldName == "FileFormat")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !valLst.Contains(x.FileFormat.ToString())).ToList();
                        }
                        else if (item.FieldName == "TemplateName")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !valLst.Contains(x.TemplateName.ToString())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !valLst.Contains(x.ImportType.ToString())).ToList();
                        }
                    }
                    else if (item.Operator == "contains")
                    {
                        if (item.FieldName == "FileFormat")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => x.FileFormat.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "TemplateName")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => x.TemplateName.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => x.ImportType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                    }
                    else if (item.Operator == "notContains")
                    {
                        if (item.FieldName == "FileFormat")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !x.FileFormat.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "TemplateName")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !x.TemplateName.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "ImportType")
                        {
                            lstImportFileTemplates = lstImportFileTemplates.Where(x => !x.ImportType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Empty catch block to stop from breaking
            }
            return PartialView("_FileTemplateList", lstImportFileTemplates);
        }

        public JsonResult GetFormat()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "EXCEL", Text = "EXCEL" });
            lstType.Add(new DropDownReturn { Value = "CSV", Text = "CSV" });
            lstType.Add(new DropDownReturn { Value = "TSV", Text = "TSV" });
            lstType.Add(new DropDownReturn { Value = "TXT", Text = "Delimiter" });
            lstType.Add(new DropDownReturn { Value = "FIXED", Text = "FIXED" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTemplateName()
        {
            List<ImportFileTemplates> lstTemplates = new List<ImportFileTemplates>();
            ImportJobDataFacade efac = new ImportJobDataFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstTemplates = efac.GetAllImportFileTemplates();
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            foreach (var item in lstTemplates)
            {
                lstType.Add(new DropDownReturn { Value = item.TemplateName, Text = item.TemplateName });
            }
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}