using ExcelDataReader;
using Microsoft.VisualBasic.FileIO;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification, AllowLicense]
    public class CommandMappingController : BaseController
    {

        #region CommandUpload 
        // GET: CommandMapping
        [Route("Portal/UploadConfiguration")]
        public ActionResult IndexCommand()
        {
            ColumnMappingFacade tac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CommandUploadMappingEntity> model = new List<CommandUploadMappingEntity>();

            //Get Command Upload List
            model = tac.GetCommandMapping();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_IndexCommand", model);
            else
            {
                ViewBag.SelectedTab = "Data Governance";
                ViewBag.SelectedMainTab = "Integration Gateway";
                ViewBag.SelectedIndividualTab = "Upload Configuration";
                return View("~/Views/Portal/Index.cshtml", model);
            }
        }

        //GET : Insert Update Command Upload Mapping
        public ActionResult CreateCommandMapping(string Parameters)
        {
            CommandUploadMappingEntity objCommandUploadMappingEntity = new CommandUploadMappingEntity();

            string ImportType = "data import";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                int id = Convert.ToInt32(Parameters);
                if (id > 0)
                {
                    bool IsTag = false;
                    bool IsInLanguage = false;
                    ColumnMappingFacade tac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    objCommandUploadMappingEntity = tac.GetCommandMappingById(id);
                    // Check if file contains tag field or not
                    if (Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "tags") || Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "tag"))
                    {
                        IsTag = true;
                    }
                    // Check if file contains language field or not
                    if (Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "language") ||
                        Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "language values") ||
                        Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "languagevalues") ||
                        Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "language code") ||
                        Array.Exists(objCommandUploadMappingEntity.OriginalColumns.Split(','), element => element.ToLower() == "languagecode"))
                    {
                        IsInLanguage = true;
                    }

                    List<SelectListItem> lstAllFilter = new List<SelectListItem>();
                    List<string> LstOriginalcolumnName = new List<string>();
                    LstOriginalcolumnName.AddRange(objCommandUploadMappingEntity.OriginalColumns.Split(','));
                    int i = 0;
                    foreach (var item in LstOriginalcolumnName)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i).ToString(), Text = Convert.ToString(item) });
                        i++;
                    }
                    ViewBag.IsContainsTags = IsTag;
                    SessionHelper.CommandMapping_IsInLanguage = IsInLanguage;
                    ViewBag.ExternalColumn = lstAllFilter;
                    ViewBag.ColumnMapping = objCommandUploadMappingEntity.ColumnMapping;
                    ImportType = objCommandUploadMappingEntity.ImportType;
                    objCommandUploadMappingEntity.FileFormatCommandLine = objCommandUploadMappingEntity.FileFormat;

                }
            }
            //get column Name
            List<string> columnName = GetcolumnName(ImportType, this.CurrentClient.ApplicationDBConnectionString);
            ViewBag.ImportMode = ImportType;
            ViewBag.ColumnList = columnName;

            return PartialView("_createCommandMapping", objCommandUploadMappingEntity);
        }

        //Get columns names for display in mapping
        public List<string> GetcolumnName(string ImportType, string ConnectionString)
        {
            SettingFacade sfac = new SettingFacade(ConnectionString);
            DataTable dtInpCompany = new DataTable();
            List<string> columnName = new List<string>();
            if (ImportType.ToLower().Trim() == "match refresh")
            {
                dtInpCompany = sfac.GetImportDataRefreshColumnsName();
            }
            else if (ImportType.ToLower().Trim() == "data import")
            {
                dtInpCompany = sfac.GetInpCompanyColumnsName();
            }
            else if (ImportType.ToLower().Trim() == "orb data import")
            {
                OIImportDataFacade orbfac = new OIImportDataFacade(this.CurrentClient.ApplicationDBConnectionString);
                dtInpCompany = orbfac.GetOIStgInputCompanyColumnsName();
            }
            //added orb enrichment only dropdown in upload configuration
            else if (ImportType.ToLower().Trim() == "orb match refresh")
            {
                OISettingFacade oisfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                dtInpCompany = oisfac.GetOIImportDataColumnsName();
            }
            if (dtInpCompany.Rows.Count > 0)
            {
                for (int k = 0; k < dtInpCompany.Rows.Count; k++)  //loop through the columns. 
                {
                    if (Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtInpCompany.Rows[k][0]) != "ImportProcessId")
                    {
                        if (ImportType.ToLower().Trim() == "orb data import" || ImportType.ToLower().Trim() == "orb match refresh")
                        {
                            if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address1")
                            {
                                columnName.Add("Street Line Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "Address2")
                            {
                                columnName.Add("Street Line Address2");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress1")
                            {
                                columnName.Add("Street Line Alt. Address1");
                            }
                            else if (Convert.ToString(dtInpCompany.Rows[k][0]) == "AltAddress2")
                            {
                                columnName.Add("Street Line Alt. Address2");
                            }
                            else
                            {
                                columnName.Add(Convert.ToString(dtInpCompany.Rows[k][0]));
                            }
                        }
                        else
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
            return columnName;
        }

        // POST: Set mapping columns
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult BindColumnMapping(HttpPostedFileBase file, string ImportType, string CustmDelimtr)
        {
            string fileType = "";
            DataTable dt = new DataTable();
            bool IsTag = false;
            bool IsInLanguage = false;
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();

            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,.csv,.tsv,.txt", file.FileName.ToLower()))
            {
                string path = string.Empty;
                string url = Request.Url.Authority;
                string UserId = Helper.oUser.UserId.ToString();
                string tempDirectory = "~/Upload/UploadCommandFile/" + url + "/" + UserId;
                string directory = Server.MapPath(tempDirectory.Replace(":", ""));
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileInfo oFileInfo = new FileInfo(file.FileName);
                string fileExtension = oFileInfo.Extension;
                string fileName = System.DateTime.Now.Ticks + fileExtension;
                path = Path.Combine(directory, Path.GetFileName(fileName));
                file.SaveAs(path);
                string extension = Path.GetExtension(file.FileName);
                // Read excel file & set column header in datatable
                if (extension.ToLower() == ".xls" || extension.ToLower() == ".xlsx")
                {
                    IExcelDataReader reader = null;
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    if (extension.Equals(".xls"))
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else if (extension.Equals(".xlsx"))
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    if (reader != null)
                    {
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                        };
                        //Fill DataSet
                        DataSet content = reader.AsDataSet(conf);
                        dt = content.Tables[0];
                    }
                    stream.Close();

                }
                // Read csv,tsv & delimiter file & set column header in datatable
                else if (extension.ToLower() == ".csv" || extension.ToLower() == ".txt")
                {

                    string Delimiters = string.Empty;
                    if (extension.ToLower() == ".csv")
                    {
                        Delimiters = ",";
                    }
                    else if (extension.ToLower() == ".txt")
                    {
                        if (string.IsNullOrEmpty(CustmDelimtr))
                        {
                            Delimiters = "\t";
                        }
                        else
                        {
                            Delimiters = CustmDelimtr;
                        }
                    }
                    using (TextFieldParser csvReader = new TextFieldParser(path))
                    {
                        try
                        {
                            csvReader.SetDelimiters(Delimiters);
                            csvReader.HasFieldsEnclosedInQuotes = true;
                            //read column names
                            string[] colFields = null;
                            colFields = csvReader.ReadFields();
                            foreach (string column in colFields)
                            {
                                DataColumn datecolumn = new DataColumn(column);
                                dt.Columns.Add(datecolumn);
                            }
                        }
                        catch (Exception)
                        {
                            dt = new DataTable();
                            System.IO.File.Delete(path);
                            IPagedList<dynamic> pagedProducts1 = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);
                            return PartialView("_bindColumnMapping", pagedProducts1);
                        }

                    }

                }
                if (dt != null && dt.Columns != null && dt.Columns.Count > 0)
                {
                    Helper.InitLookups();
                    foreach (var item in dt.Columns)
                    {
                        if (Helper.IsSpecialCharactersExist(item.ToString()))
                        {
                            return Json(new { result = false, message = ImportDataLang.msgError + ":" + ImportDataLang.msgSpecialCharError });
                        }
                    }

                    lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                    int i = 0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;
                        if (c.ColumnName.ToLower() == "tags" || c.ColumnName.ToLower() == "tag")
                        {
                            IsTag = true;
                        }
                        if (c.ColumnName.ToLower() == "language" || c.ColumnName.ToLower() == "language values" || c.ColumnName.ToLower() == "languagevalues" || c.ColumnName.ToLower() == "language code" || c.ColumnName.ToLower() == "languagecode")
                        {
                            IsInLanguage = true;
                        }
                    }
                }
                System.IO.File.Delete(path);
            }
            List<string> columnName = GetcolumnName(ImportType, this.CurrentClient.ApplicationDBConnectionString);
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;
            ViewBag.IsContainsTags = IsTag;
            SessionHelper.CommandMapping_IsInLanguage = IsInLanguage;



            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);
            ViewBag.fileType = fileType;
            ViewBag.ImportMode = ImportType;
            if (ImportType.ToLower().Trim() == "orb data import" || ImportType.ToLower().Trim() == "orb match refresh")
            {
                return PartialView("_bindOrbColumnMapping", pagedProducts);
            }
            return PartialView("_bindColumnMapping", pagedProducts);
        }

        // POST: Insert Update Command Upload Mapping
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult InsertUpdate(CommandUploadMappingEntity obj)
        {
            if (ModelState.IsValid)
            {
                ColumnMappingFacade fac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
                obj.UserId = Helper.oUser.UserId;
                obj.Tags = string.IsNullOrEmpty(obj.Tags) ? "" : obj.Tags == "0" ? "" : obj.Tags;
                obj.Formatvalue = obj.FileFormat == "Delimiter" ? obj.Formatvalue : "";
                obj.FileFormat = obj.FileFormatCommandLine;
                string msg = fac.InsertUpdateCommandUploadMapping(obj);
                string ImportType = obj == null ? "data import" : string.IsNullOrEmpty(obj.ImportType) ? "data import" : obj.ImportType.ToLower();
                if (string.IsNullOrEmpty(msg))
                {
                    if (obj.Id > 0)
                    {
                        ViewBag.Message = CommonMessagesLang.msgCommanUpdateMessage;
                        ImportType = obj.ImportType;



                        List<SelectListItem> lstAllFilter = new List<SelectListItem>();
                        List<string> LstOriginalcolumnName = new List<string>();
                        LstOriginalcolumnName.AddRange(obj.OriginalColumns.Split(','));
                        int i = 0;
                        foreach (var item in LstOriginalcolumnName)
                        {
                            lstAllFilter.Add(new SelectListItem { Value = (i).ToString(), Text = Convert.ToString(item) });
                            i++;
                        }
                        ViewBag.ExternalColumn = lstAllFilter;
                        ViewBag.ColumnMapping = obj.ColumnMapping;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessagesLang.msgCommanInsertMessage;
                        obj = new CommandUploadMappingEntity();
                    }
                }
                else
                {
                    ViewBag.Message = msg;
                }

                List<string> columnName = GetcolumnName(ImportType, this.CurrentClient.ApplicationDBConnectionString);
                ViewBag.ColumnList = columnName;
                ViewBag.ImportMode = ImportType;
            }
            else
            {
                ViewBag.Message = DandBSettingLang.msgInvadilState;
            }
            return PartialView("_createCommandMapping", obj);
        }


        // Delete Command Upload Mapping
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCommanduploadMapping(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // delete Command upload Mapping
            ColumnMappingFacade fac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteCommandMapping(Convert.ToInt32(Parameters));
            return Json(CommonMessagesLang.msgCommanDeleteMessage);
        }
        #endregion
        #region CommandDownload
        //GET : Insert Update Command Download Mapping
        public ActionResult InsertUpdateCommandDownload(string Parameters)
        {
            CommandDownloadMappingEntity obj = new CommandDownloadMappingEntity();
            obj.IsAppendDateTime = true;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                int id = Convert.ToInt32(Parameters);
                if (id > 0)
                {
                    ColumnMappingFacade tac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    obj = tac.GetCommandDownloadMappingById(id);
                }
            }
            return PartialView("_insertUpdateCommandDownload", obj);
        }

        //POST : Insert Update Command Download Mapping
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult InsertUpdateCommandDownload(CommandDownloadMappingEntity obj)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(obj.ProviderType))
                {
                    if (Helper.LicenseEnabledDNB && !Helper.LicenseEnabledOrb)
                    {
                        obj.ProviderType = "DandB";
                    }
                    else if (!Helper.LicenseEnabledDNB && Helper.LicenseEnabledOrb)
                    {
                        obj.ProviderType = "Orb";
                    }
                }
                ColumnMappingFacade fac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
                obj.Formatvalue = obj.DownloadFormat == "Delimiter" ? obj.Formatvalue : "";
                obj.UserId = 0;
                string msg = fac.InsertUpdateCommandDownloadMapping(obj);
                // set message for insert & update
                if (string.IsNullOrEmpty(msg))
                {
                    if (obj.Id > 0)
                    {
                        ViewBag.Message = CommonMessagesLang.msgCommanUpdateMessage;
                    }
                    else
                    {
                        ViewBag.Message = CommonMessagesLang.msgCommanInsertMessage;
                    }
                }
                else
                {
                    ViewBag.Message = msg;
                }
            }
            else
            {
                ViewBag.Message = DandBSettingLang.msgInvadilState;
            }
            return View("_insertUpdateCommandDownload", obj);
        }

        //GET: List for Command download mapping
        [Route("Portal/DownloadConfiguration")]
        public ActionResult IndexCommandDownload()
        {
            ColumnMappingFacade tac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<CommandDownloadMappingEntity> model = new List<CommandDownloadMappingEntity>();
            //Get Command Download Mapping List
            model = tac.GetCommandDownloadMapping();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexCommandDownload", model);
            else
            {

                ViewBag.SelectedTab = "Data Governance";
                ViewBag.SelectedMainTab = "Integration Gateway";
                ViewBag.SelectedIndividualTab = "Download Configuration";
                return View("~/Views/Portal/Index.cshtml", model);
            }
        }

        // POST: Delete Command Download Mapping
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult DeleteCommandDownload(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }

            ColumnMappingFacade fac = new ColumnMappingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteCommandDownloadMapping(Convert.ToInt32(Parameters));
            return Json(CommonMessagesLang.msgCommanDeleteMessage);
        }



        #endregion
        #region CommandDownloadSetup
        [Route("Portal/EXESetup")]
        public ActionResult IndexCommandDownloadSetup()
        {
            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexCommandDownloadSetup");
            else
            {
                ViewBag.SelectedTab = "Data Governance";
                ViewBag.SelectedMainTab = "Integration Gateway";
                ViewBag.SelectedIndividualTab = "EXE Setup";
                return View("~/Views/Portal/Index.cshtml");
            }
        }


        public ActionResult DownloadExeSetup(string Parameters)
        { // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            string FilePath = string.Empty;
            if (Parameters.ToLower() == "redhat6")
            {
                FilePath = Server.MapPath("~/CommandLineEXESetup/linux/redHat6.zip");
            }
            else if (Parameters.ToLower() == "redhat7")
            {
                FilePath = Server.MapPath("~/CommandLineEXESetup/linux/redHat7.zip");
            }
            else if (Parameters.ToLower() == "win32")
            {
                FilePath = Server.MapPath("~/CommandLineEXESetup/windows/windows10/win32.zip");
            }
            else if (Parameters.ToLower() == "win64")
            {
                FilePath = Server.MapPath("~/CommandLineEXESetup/windows/windows10/win64.zip");
            }
            return File(FilePath, "application/zip", Path.GetFileName(FilePath));
        }

        #endregion

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult GetDateTimeFormat(string Parameters)
        {
            bool IsAppendDateTime = false, IsFilePrefix = false;
            string DatetimeFormat = string.Empty, FilePrefix = string.Empty, fileExtension = string.Empty;
            DateTime NowDateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                IsAppendDateTime = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                DatetimeFormat = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                IsFilePrefix = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                FilePrefix = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                fileExtension = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
            }
            string fileName = "MatchOutPut";
            if (IsFilePrefix)
            {
                fileName = "e.g. " + (string.IsNullOrEmpty(FilePrefix) ? "" : FilePrefix + "_") + (IsAppendDateTime ? NowDateTime.ToString(DatetimeFormat) + "_" : "") + fileName + fileExtension;
            }
            else
            {
                fileName = "e.g. " + (string.IsNullOrEmpty(FilePrefix) ? "" : FilePrefix + "_") + fileName + (IsAppendDateTime ? "_" + NowDateTime.ToString(DatetimeFormat) : "") + fileExtension;
            }
            return Json(fileName);
        }
    }
}
