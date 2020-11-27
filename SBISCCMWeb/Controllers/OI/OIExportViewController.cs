using Ionic.Zip;
using Microsoft.AspNet.Identity;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchFacade.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers.OI
{
    [Authorize, OrbLicenseEnabled, TwoStepVerification, AllowLicense]
    public class OIExportViewController : BaseController
    {
        // GET: OIExportView
        [HttpGet, Route("OI/ExportView")]
        public async Task<ActionResult> Index(string FileType = null)
        {
            FileType = string.IsNullOrEmpty(FileType) ? "My Files" : FileType;
            ViewBag.FileType = FileType;

            string ApplicationId = Convert.ToString(this.CurrentClient.ApplicationId);

            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            int? UserId;
            if (FileType == "My Files")
            {
                UserId = Helper.oUser.UserId;
            }
            else
            {
                UserId = null;
            }
            OIExportJobSettingsFacade efac = new OIExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            efac.UpdateExportOIJobSettingNotificationsStatus(this.CurrentClient.ApplicationId, Helper.oUser.UserId, ProviderType.OI.ToString());
            List<OIExportJobSettingsEntity> lstExportJobs = efac.GetExportJobSettingsByUserId(ProviderType.OI.ToString(), UserId, this.CurrentClient.ApplicationId);

            //set helper for display un-flag export button
            DataTable dtActiveDataStatistics = fac.GetDashboardGetDataQueueStatistics(Helper.oUser != null ? Helper.oUser.LOBTag : null, Helper.oUser != null ? Helper.oUser.Tags : null, Helper.oUser.UserId);
            if (dtActiveDataStatistics.Rows.Count != 0)
            {
                Helper.ArchivalQueueCount = dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"] is DBNull ? 0 : Convert.ToInt32(dtActiveDataStatistics.Rows[0]["ArchivalQueueCount"]);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/OI/OIExportView/_index.cshtml", lstExportJobs);
            }
            return View("~/Views/OI/OIExportView/Index.cshtml", lstExportJobs);
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain, Route("OI/ExportView")]
        public ActionResult Index(OIExportJobSettingsEntity objExport)
        {
            try
            {
                string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
                foreach (char c in invalid)
                {
                    objExport.FilePath = objExport.FilePath.Replace(c.ToString(), "");
                }
                objExport.FilePath = objExport.FilePath + ".zip";

                objExport.UserId = Helper.oUser.UserId;
                objExport.ApplicationId = this.CurrentClient.ApplicationId;
                objExport.ExportType = ProviderType.OI.ToString();// Set provider type OI as it is from Orb Intelligence

                if (Helper.oUser.UserRole == UserRole.LOB.ToString())
                {
                    objExport.LOBTag = Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "";
                }
                OIExportJobSettingsFacade efac = new OIExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                efac.InserExportJobSettings(objExport);

                return Json(new { result = true, message = ExportLang.msgExportDataSave }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        // Deletes the record from the Export Status List
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateInput(true)]
        public JsonResult Delete(int Id)
        {
            try
            {
                OIExportJobSettingsFacade fac = new OIExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                OIExportJobSettingsEntity objOIExport = new OIExportJobSettingsEntity();
                objOIExport = fac.GetExportJobSettingsById(Id);
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string domain = hostParts[0];
                ImageHelper.DeleteBlobFile(domain, objOIExport.FilePath);
                fac.DeleteOIExportJobSettings(objOIExport);
                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        //Cancel Export process
        public JsonResult CancelExportProcess(int Id)
        {
            try
            {
                OIExportJobSettingsFacade fac = new OIExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
                OIExportJobSettingsEntity objOIExport = new OIExportJobSettingsEntity();
                objOIExport = fac.GetExportJobSettingsById(Id);

                if (objOIExport.ProcessStartDate == null)
                {
                    fac.CancelExportJobSettings(objOIExport);
                    return Json(new { result = true, message = ExportLang.msgJobCancel }, JsonRequestBehavior.AllowGet);
                }
                else if (objOIExport.ProcessStartDate != null && !objOIExport.IsProcessComplete)
                {
                    return Json(new { result = true, message = ExportLang.msgJobInProcess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        // Selecting the Custom Delimiter selection popup  
        [HttpGet]
        public ActionResult Delimiter(string Parameters)
        {
            //check file name is exists or not 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = Server.UrlDecode((StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase)));
            }
            ViewBag.Delimiter = Parameters;
            return View("~/Views/OI/OIExportView/Delimiter.cshtml");
        }
        //// Selecting a particular Custom Delimiter and then submitting
        //[HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        //public JsonResult SetDelimiter(string Delimiter)
        //{
        //    return new JsonResult { Data = Delimiter };
        //}

        // Export file name details(excel,tsv of text file)
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
            return View("~/Views/OI/OIExportView/OIExportFileName.cshtml");
        }

        // Submitting the file for Export(excel,tsv of text file)
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
                    return Json(new { result = true, message = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = OIExportDataLang.msgExistsFileName }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = OIExportDataLang.msgRequireFileName }, JsonRequestBehavior.AllowGet);
            }
        }

        // For downloading the file from the Export Status List
        [RequestFromSameDomain]
        public ActionResult GetBlobFile(string imagepath)
        {
            if (!System.IO.Directory.Exists(Server.MapPath("/ExportData/")))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("/ExportData/"));
            }
            string path = HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath));
            Stream filepath = GetZipFile(path);
            FileInfo oFileInfo = new FileInfo(imagepath);
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
        [HttpPost]
        public JsonResult SetDownloadValue(int Id)
        {
            ExportJobSettingsFacade efac = new ExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            efac.UpdateExportJobSettingsForDownload(Id, Helper.oUser.UserId);
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        // If the file or image came through the storage than we need to download and save the specific file
        public Stream GetZipFile(string filePath)
        {
            string testFileName;
            try
            {
                byte[] buffer = new byte[4097];

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response.GetResponseStream();
            }
            catch
            {
                return null;
            }
        }

        #region  Notification
        // Shows the notification when file process is ready for download 
        public JsonResult ExportOIJobNotification()
        {
            // Notification for which file are ready to download
            string strNotification = string.Empty;
            OIExportJobSettingsFacade fac = new OIExportJobSettingsFacade(StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase));
            strNotification = fac.ExportOIJobSettingNotifications(this.CurrentClient.ApplicationId, Helper.oUser.UserId, ProviderType.OI.ToString());
            if (!string.IsNullOrEmpty(strNotification))
            {
                strNotification = CommonMessagesLang.msgFiles + " " + strNotification + " " + CommonMessagesLang.msgFilesReadyToDownload;
            }
            return new JsonResult { Data = strNotification };
        }
        #endregion
    }
}