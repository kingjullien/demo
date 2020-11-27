using Microsoft.AspNet.Identity;
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
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, DandBLicenseEnabled, AllowLicense]
    public class ExportViewController : BaseController
    {
        // GET: ExportView
        [Route("Export/CompanyData")]
        [OutputCache(NoStore = true, Duration = 0)]
        [HttpGet]
        public ActionResult Index(string FileType = null, string ImportProcess = null)
        {
            FileType = string.IsNullOrEmpty(FileType) ? CommonMessagesLang.lblMyFiles : FileType;
            ViewBag.FileType = FileType;
            if (!string.IsNullOrEmpty(ImportProcess))
                ImportProcess = StringCipher.Decrypt(ImportProcess, General.passPhrase);

            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            int? UserId;
            if (FileType == CommonMessagesLang.lblMyFiles)
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }
            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            efac.UpdateExportJobSettingNotificationsStatus(this.CurrentClient.ApplicationId, Helper.oUser.UserId, ProviderType.DandB.ToString());
            List<ExportJobSettingsEntity> lstExportJobs = efac.GetExportJobSettingsByUserId(ProviderType.DandB.ToString(), UserId, this.CurrentClient.ApplicationId);

            //set helper for display un-flag export button
            DataTable dtActiveDataStatistics = fac.GetDashboardGetDataQueueStatistics(Helper.oUser != null ? Helper.oUser.LOBTag : null, Helper.oUser != null ? Helper.oUser.Tags : null, Helper.oUser.UserId);
            if (dtActiveDataStatistics.Rows.Count != 0)
            {
                Helper.ArchivalQueueCount = dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"] is DBNull ? 0 : Convert.ToInt32(dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"]);
            }
            ViewBag.ImportProcess = ImportProcess;
            ViewBag.SelectedTab = "CompanyData";
            if (Request.IsAjaxRequest())
            {
                return PartialView("CompanyData", lstExportJobs);
            }
            return View(lstExportJobs);
        }
        [HttpGet]
        public ActionResult LoadExportStatus(string Parameters)
        {
            string FileType = null;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                FileType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            FileType = string.IsNullOrEmpty(FileType) ? CommonMessagesLang.lblMyFiles : FileType;
            ViewBag.FileType = FileType;
            int? UserId;
            if (FileType == CommonMessagesLang.lblMyFiles)
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }
            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            List<ExportJobSettingsEntity> lstExportJobs = efac.GetExportJobSettingsByUserId(ProviderType.DandB.ToString(), UserId, this.CurrentClient.ApplicationId);
            return PartialView("_index", lstExportJobs);
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult ExportToCompanyData(ExportJobSettingsEntity objExport)
        {
            try
            {
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                foreach (char c in invalid)
                {
                    objExport.FilePath = objExport.FilePath.Replace(c.ToString(), "");
                }
                objExport.ExportType = ProviderType.DandB.ToString();// Set provider type DandB as it is from D & B
                objExport.ApplicationId = this.CurrentClient.ApplicationId;
                objExport.UserId = Helper.oUser.UserId;
                objExport.FilePath = objExport.FilePath + ".zip";
                if (Helper.oUser.UserRole == UserRole.LOB.ToString())
                {
                    objExport.LOBTag = Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "";
                }

                ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                efac.InserExportJobSettings(objExport);
                return Json(new { result = true, message = ExportLang.msgExportDataSave }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateInput(true)]
        public JsonResult Delete(int Id)
        {
            try
            {
                ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                ExportJobSettingsEntity objExport = efac.GetExportJobSettingsByIdByClient(Id);   // MP-846 Admin database cleanup and code cleanup.-CLIENT
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string domain = hostParts[0];
                ImageHelper.DeleteBlobFile(domain, objExport.FilePath);
                efac.DeleteExportJobSettingsForClients(objExport);   // MP-846 Admin database cleanup and code cleanup.-CLIENT
                SessionHelper.ExportView_Message = CommonMessagesLang.msgCommanDeleteMessage;
                return new JsonResult { Data = CommonMessagesLang.msgSuccess };
            }
            catch (Exception ex)
            {
                SessionHelper.ExportView_Message = ex.Message.ToString();
                return new JsonResult { Data = CommonMessagesLang.msgFail };
            }
        }

        [HttpGet]
        public ActionResult Delimiter(string Parameters)
        {
            //check file name is exists or not 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = Server.UrlDecode((StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase)));
            }
            ViewBag.Delimiter = Parameters;
            return View();
        }
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult SetDelimiter(string Delimiter)
        {
            return new JsonResult { Data = Delimiter };
        }
        [HttpPost]
        public JsonResult SetDownloadValue(int Id)
        {
            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            efac.UpdateExportJobSettingsForDownload(Id, Helper.oUser.UserId);
            return new JsonResult { Data = "Success" };
        }
        #region "Export Data To Excel"  
        public void DeteleFiles()
        {
            //Delete Old file and Directory for this path only
            string strpath = Server.MapPath("/ExportData/" + Convert.ToString(this.CurrentClient.ApplicationId) + "/" + Convert.ToString(User.Identity.GetUserId()) + "/");
            System.IO.DirectoryInfo di = new DirectoryInfo(strpath);
            try
            {
                foreach (FileInfo file in di.GetFiles())
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            catch (Exception) {
                //Empty catch block to stop from breaking
            }
        }

        #endregion

        #region "Re Export"
        [HttpGet]
        public ActionResult ReExportFile()
        {
            return View();
        }
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public ActionResult ReExportFile(ReExportDataEntity objReExport)
        {
            //mark as Unflag data for ReExport data
            int count = 0;
            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(this.CurrentClient.ApplicationDBConnectionString);
            objReExport.UserId = Helper.oUser.UserId;
            count = efac.UnflagExportedRecords(objReExport);
            if (objReExport.GetCountsOnly)
            {
                return new JsonResult { Data = CommonMessagesLang.msgTotal + " " + count + " " + CommonMessagesLang.msgRecordsAffected + " " };
            }
            else
            {
                //set helper for display un-flag export button
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                DataTable dtActiveDataStatistics = fac.GetDashboardGetDataQueueStatistics(Helper.oUser != null ? Helper.oUser.LOBTag : null, Helper.oUser != null ? Helper.oUser.Tags : null, Helper.oUser.UserId);
                if (dtActiveDataStatistics.Rows.Count != 0)
                {
                    Helper.ArchivalQueueCount = dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"] is DBNull ? 0 : Convert.ToInt32(dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"]);
                }
                return new JsonResult { Data = ExportLang.msgUnFlagData };
            }
        }

        //UnFlag Export Data - Tags dropdown. [Task] MP-360
        public static List<TagsEntity> GetExportedDataTags(string ConnectionString)
        {
            // Get Exported tags from the database for un-flag as exported 
            TagFacade fac = new TagFacade(ConnectionString);
            List<TagsEntity> lstTags = fac.GetExportedDataTags(Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Helper.oUser != null ? (Helper.oUser.Tags != null ? Helper.oUser.Tags : "") : "", Helper.oUser.UserId);

            return lstTags;

        }
        //UnFlag Export Data - Import Process dropdown. [Task] MP-360
        public static SelectList GetExportedDataImportProcess(string ConnectionString)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetExportedDataImportProcess();
            List<SelectListItem> lstImportProcess = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new SelectListItem { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return new SelectList(lstImportProcess, "Value", "Text");
        }
        #endregion


        #region "Data Monitoring Notifications Data"

        [Route("Export/MonitoringData")]
        [OutputCache(NoStore = true, Duration = 0)]
        [HttpGet]
        public ActionResult MonitoringExportindex(string MonitoringFileType = null)
        {
            MonitoringFileType = string.IsNullOrEmpty(MonitoringFileType) ? "My Files" : MonitoringFileType;
            ViewBag.MonitoringFileType = MonitoringFileType;

            int? UserId;
            if (MonitoringFileType == "My Files")
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }

            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            List<MonitoringNotificationJobSettingsEntity> lstExportJobs = efac.GetMonitoringExportJobSettingsByUserId(UserId, this.CurrentClient.ApplicationId);

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("MonitoringExportindex", lstExportJobs);
            else
            {
                ViewBag.SelectedTab = "MonitoringData";
                return View("Index", lstExportJobs);
            }
        }
        [HttpGet]
        public ActionResult LoadMonitoringExportStatus(string Parameters = null)
        {
            string MonitoringFileType = null;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                MonitoringFileType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            MonitoringFileType = string.IsNullOrEmpty(MonitoringFileType) ? "My Files" : MonitoringFileType;
            ViewBag.MonitoringFileType = MonitoringFileType;

            int? UserId;
            if (MonitoringFileType == "My Files")
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }

            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            List<MonitoringNotificationJobSettingsEntity> lstExportJobs = efac.GetMonitoringExportJobSettingsByUserId(UserId, this.CurrentClient.ApplicationId);
            return PartialView("_MonitoringExport", lstExportJobs);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax]
        public ActionResult ExportToMonitoring(string Parameters)
        {

            string FileName = "", FileType = "", APIName = "", Delimiter = "";
            bool MarkAsExported = false;
            bool HasHeader = false;
            bool MonitoringHasTextQualifierToAll = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                MarkAsExported = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                FileType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                APIName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                Delimiter = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
                HasHeader = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
                MonitoringHasTextQualifierToAll = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));
            }

            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
            string[] hostParts = new System.Uri(url).Host.Split('.');
            string domain = hostParts[0];
            FileName = domain + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".zip";

            MonitoringNotificationJobSettingsEntity objExport = new MonitoringNotificationJobSettingsEntity();
            objExport.UserId = Helper.oUser.UserId;
            objExport.Format = FileType;
            objExport.FilePath = FileName;
            objExport.ApplicationId = this.CurrentClient.ApplicationId;
            objExport.APILayer = APIName;
            objExport.IsMonitoringNotifications = true;
            objExport.Delimiter = Delimiter;
            objExport.MarkAsExported = MarkAsExported;
            objExport.ExportType = ProviderType.DandB.ToString();
            objExport.HasHeader = HasHeader;
            objExport.HasTextQualifierToAll = MonitoringHasTextQualifierToAll;

            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            string message = efac.InserMonitoringNotificationsJobSettings(objExport);
            if (string.IsNullOrEmpty(message))
            {
                return new JsonResult { Data = ExportLang.msgExportDataSave };
            }
            else
            {
                return new JsonResult { Data = message };
            }
        }
        #endregion

        public static SelectList GetExportDataImportProcess(string ConnectionString, string Queue, bool IsMatchData)
        {
            CompanyFacade fac = new CompanyFacade(ConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetImportProcessesByQueue(Queue, IsMatchData);
            List<SelectListItem> lstImportProcess = new List<SelectListItem>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new SelectListItem { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return new SelectList(lstImportProcess, "Value", "Text");
        }

        [RequestFromSameDomain]
        public ActionResult GetBlobFile(string imagepath)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("/ExportData/")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/ExportData/"));
            }
            string path = HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath));
            Stream filepath = GetZipFile(path);
            if (filepath != null)
            {
                FileStreamResult objFileStreamResult = new FileStreamResult(filepath, "application/x-zip-compressed")
                {
                    FileDownloadName = path.Substring(path.LastIndexOf("/") + 1)
                };
                return objFileStreamResult;
            }
            else
            {
                return RedirectToAction("index");
            }
        }
        // If the file or image came through the storage than we need to download and save the specific file
        public Stream GetZipFile(string filePath)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }

        public JsonResult ExportJobNotification()
        {
            //Notification for which file are ready to download
            string strNotification = string.Empty;
            ExportJobSettingsFacade fac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            strNotification = fac.ExportJobSettingNotifications(this.CurrentClient.ApplicationId, Helper.oUser.UserId, ProviderType.DandB.ToString());
            if (!string.IsNullOrEmpty(strNotification))
            {
                strNotification = " Your file(s) " + strNotification + " are ready for the download.";
            }
            return new JsonResult { Data = strNotification };
        }

        public ActionResult ExportFileName(string Parameters)
        {
            string SrcRecID = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                SrcRecID = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }

            // Calling function for removing special characters
            SrcRecID = Utility.CommonMethod.RemoveSpecialChars(SrcRecID);

            //set the Export file name with domain ,srcid and datetime 
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
            string[] hostParts = new System.Uri(url).Host.Split('.');
            string domain = hostParts[0];
            string filename = "";
            filename = domain + "_" + (string.IsNullOrEmpty(SrcRecID) ? "" : SrcRecID + "_") + DateTime.Now.ToString("yyyyMMddhhmmss");
            filename = filename.TrimStart('_').TrimEnd('_');
            ViewBag.filename = filename;
            ViewBag.domain = domain;
            ViewBag.SrcRecID = SrcRecID;
            ViewBag.SpanTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            return PartialView();
        }

        [RequestFromAjax, RequestFromSameDomain, HttpPost, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult IsExistExportFileName(string Parameters)
        {
            //check file name is exists or not 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = Server.UrlDecode((StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase)));

                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                foreach (char c in invalid)
                {
                    Parameters = Parameters.Replace(c.ToString(), "");
                }
                if (String.IsNullOrEmpty(Parameters))
                {
                    return new JsonResult { Data = CommonMessagesLang.msgEnterValidName };
                }
                Parameters = Parameters + ".zip";
                int Applicationid = this.CurrentClient.ApplicationId;
                ExportJobSettingsFacade fac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                string exportFileName = fac.IsExistExportFileName(Parameters, Applicationid);
                if (string.IsNullOrEmpty(exportFileName))
                {
                    return new JsonResult { Data = CommonMessagesLang.msgNotExist };
                }
                else
                {
                    return new JsonResult { Data = CommonMessagesLang.msgExist };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgRequiredFileName };
            }

        }


        //Cancel Export -MonitoringExport request process
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CancelExportProcess(int Id)
        {
            try
            {
                ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                ExportJobSettingsEntity objExport = efac.GetExportJobSettingsByIdByClient(Id);   // MP-846 Admin database cleanup and code cleanup.-CLIENT

                if (objExport.ProcessStartDate == null)
                {
                    efac.CancelExportJobSettings(objExport);
                    SessionHelper.ExportView_Message = ExportLang.msgJobCancel;
                }
                else if (objExport.ProcessStartDate != null && !objExport.IsProcessComplete)
                {
                    SessionHelper.ExportView_Message = ExportLang.msgJobInProcess;
                }

                return new JsonResult { Data = CommonMessagesLang.msgSuccess };
            }
            catch (Exception ex)
            {
                SessionHelper.ExportView_Message = ex.Message.ToString();
                return new JsonResult { Data = CommonMessagesLang.msgFail };
            }
        }
    }
}