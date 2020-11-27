using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCCMWeb.Utility.Monitoring;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize,TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBMonitoringDirectPlusController : BaseController
    {
        // GET: DNBMonitoringDirectPlus
        public ActionResult Index()
        {
            return View();
        }
        #region "Monitoring Direct Plus"
        [DPMLicenseEnabled]
        [Route("DNB/MonitoringDirectPlus")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult indexMonitoringDirectPlus()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.DPMGetRegistration();
            //set Viewbag and viewbag bind in dropdown
            List<SelectListItem> lstMonitoringRegistrations = new List<SelectListItem>();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstMonitoringRegistrations.Add(new SelectListItem { Value = Convert.ToString(dt.Rows[0]["MonitoringRegistrationName"]), Text = Convert.ToString(dt.Rows[0]["MonitoringRegistrationName"]) });
                }
            }
            ViewBag.dtMonitoringRegistrations = dt;
            MonitoringRegistrationDetailResponse monitoringRegistrationDetailResponse = new MonitoringRegistrationDetailResponse();
            if (lstMonitoringRegistrations != null && lstMonitoringRegistrations.Count > 0)
            {
                monitoringRegistrationDetailResponse = GetMonitoringRegistrationDetailResponseFromDB(lstMonitoringRegistrations.FirstOrDefault().Text.ToString());
            }
            if (monitoringRegistrationDetailResponse.messages == null)
            {
                monitoringRegistrationDetailResponse.messages = new Messages();
                monitoringRegistrationDetailResponse.messages.registration = new Registration();
                monitoringRegistrationDetailResponse.messages.references = new List<string>();
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
            {
                return PartialView(monitoringRegistrationDetailResponse);
            }
            else
            {
                ViewBag.SelectedTab = "Monitoring Direct Plus";
                return View("~/Views/DandB/Index.cshtml", monitoringRegistrationDetailResponse);
            }
        }

        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, DPMLicenseEnabled]
        public ActionResult UpdateMonitoringRegistration(MonitoringRegistrationDetailResponse model)
        {

            model.messages.registration.Tags = model.messages.registration.Tags == "0" ? "" : model.messages.registration.Tags;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DPMUpdateRegistration(model.messages.registration.reference, model.messages.registration.Tags);
            ViewBag.MonitoringRegistrationMessage = CommonMessagesLang.msgCommanUpdateMessage;
            return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
        }

        [RequestFromAjax, RequestFromSameDomain, HttpPost, DPMLicenseEnabled]
        public ActionResult GetMonitoringPlusRegistrationDetail(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            MonitoringRegistrationDetailResponse monitoringRegistrationDetailResponse = GetMonitoringRegistrationDetailResponseFromDB(Parameters);
            if (monitoringRegistrationDetailResponse.messages == null)
            {
                monitoringRegistrationDetailResponse.messages = new Messages();
                monitoringRegistrationDetailResponse.messages.registration = new Registration();
            }
            ViewBag.GetDPMDunsRegistrationByRegistrationName = GetDPMDunsRegistrationByRegistrationName(Parameters);
            return PartialView("MonitoringPlusRegistrationDetail", monitoringRegistrationDetailResponse);
        }

        [DPMLicenseEnabled]
        public MonitoringRegistrationDetailResponse GetMonitoringRegistrationDetailResponseFromDB(string MonitoringRegistrationName)
        {
            MonitoringRegistrationDetailResponse monitoringRegistrationDetailResponse = new MonitoringRegistrationDetailResponse();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetDPMRegistrationByName(MonitoringRegistrationName);

            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                monitoringRegistrationDetailResponse.messages = new Messages();
                monitoringRegistrationDetailResponse.messages.registration = new Registration();
                monitoringRegistrationDetailResponse.messages.registration.reference = Convert.ToString(dt.Rows[0]["MonitoringRegistrationName"]);
                monitoringRegistrationDetailResponse.messages.registration.productId = Convert.ToString(dt.Rows[0]["productId"]);
                monitoringRegistrationDetailResponse.messages.registration.versionId = Convert.ToString(dt.Rows[0]["versionId"]);
                monitoringRegistrationDetailResponse.messages.registration.email = Convert.ToString(dt.Rows[0]["email"]);
                monitoringRegistrationDetailResponse.messages.registration.fileTransferProfile = Convert.ToString(dt.Rows[0]["fileTransferProfile"]);
                monitoringRegistrationDetailResponse.messages.registration.description = Convert.ToString(dt.Rows[0]["description"]);
                monitoringRegistrationDetailResponse.messages.registration.deliveryTrigger = Convert.ToString(dt.Rows[0]["deliveryTrigger"]);
                monitoringRegistrationDetailResponse.messages.registration.deliveryFrequency = Convert.ToString(dt.Rows[0]["deliveryFrequency"]);
                monitoringRegistrationDetailResponse.messages.registration.seedData = Convert.ToBoolean(dt.Rows[0]["seedData"]);
                monitoringRegistrationDetailResponse.messages.registration.blockIds = Convert.ToString(dt.Rows[0]["blockIds"]);
                monitoringRegistrationDetailResponse.messages.dunsCount = dt.Rows[0]["dunsCount"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["dunsCount"]) : 0;
                monitoringRegistrationDetailResponse.messages.registration.Tags = Convert.ToString(dt.Rows[0]["Tags"]);
                monitoringRegistrationDetailResponse.messages.registration.CredentialId = dt.Rows[0]["CredentialId"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["CredentialId"]) : 0;
                monitoringRegistrationDetailResponse.messages.registration.CredentialName = dt.Rows[0]["CredentialName"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["CredentialName"]) : string.Empty;
                monitoringRegistrationDetailResponse.messages.registration.AuthToken = dt.Rows[0]["AuthToken"] != DBNull.Value ? Convert.ToString(dt.Rows[0]["AuthToken"]) : string.Empty;
            }
            return monitoringRegistrationDetailResponse;
        }
        [DPMLicenseEnabled]
        public DataTable GetDPMDunsRegistrationByRegistrationName(string RegistrationName)
        {
            DataTable dt = new DataTable();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            dt = fac.GetDPMDunsRegistrationByRegistrationName(RegistrationName);
            return dt;
        }
        //Get List of Monitoring Registration
        [DPMLicenseEnabled]
        public ListMonitoringRegistrationResponse GetListMonitoringRegistrationResponse(string AuthToken)
        {
            SBISCCMWeb.Utility.Utility utility = new SBISCCMWeb.Utility.Utility();
            ListMonitoringRegistrationResponse objtMonitoringRegistrationResponse = utility.ListMonitoringRegistrations(AuthToken);
            return objtMonitoringRegistrationResponse;
        }

        // Get List of Monitoring Registration Detail 
        [DPMLicenseEnabled]
        public MonitoringRegistrationDetailResponse GetMonitoringRegistrationDetailResponse(string referenceName, string AuthToken)
        {
            SBISCCMWeb.Utility.Utility utility = new SBISCCMWeb.Utility.Utility();
            MonitoringRegistrationDetailResponse monitoringRegistrationDetailResponse = utility.MonitoringRegistrationDetailResponse(referenceName, AuthToken);
            return monitoringRegistrationDetailResponse;
        }

        #region "Add DUNS"
        [DPMLicenseEnabled]
        public ActionResult AddDUNSMonitoringPlus(string Parameters)
        {
            string RegistrationsName = string.Empty;
            string AuthToken = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                RegistrationsName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                AuthToken = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            ViewBag.RegistrationsName = RegistrationsName;
            ViewBag.AuthToken = AuthToken;
            return View();
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, DPMLicenseEnabled]
        public ActionResult AddDUNSMonitoringPlus(string RegistrationsName, HttpPostedFileBase file, string AuthToken)
        {
            if (file != null && CommonMethod.CheckFileType(".txt", file.FileName.ToLower()))
            {
                if (file.ContentLength > 0)
                {
                    string path = string.Empty;
                    string url = Request.Url.Authority;
                    string tempDirectory = "~/Upload/MonitoringDirectPlusAddDuns/" + url;
                    string directory = Server.MapPath(tempDirectory.Replace(":", ""));
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string FileName = System.DateTime.Now.Ticks + "_" + file.FileName;
                    path = Path.Combine(directory, Path.GetFileName(FileName));
                    file.SaveAs(path);
                    path = CheckNumberOfLinesINDUNSRegistration(path);
                    Utility.Utility utility = new Utility.Utility();
                    AddRemoveDUNSToMonitoringResponse addDUNsToMonitoringResponse = utility.AddDunsToMonitoring(RegistrationsName, path, AuthToken);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    if (addDUNsToMonitoringResponse.information != null && !string.IsNullOrEmpty(addDUNsToMonitoringResponse.information.message))
                    {
                        return Json(new { result = false, message = addDUNsToMonitoringResponse.information.message }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = addDUNsToMonitoringResponse.error.errorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanFileEmpty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgInvalidFile }, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region  "Remove DUNS Monitoring Plus"
        public ActionResult RemoveDUNSMonitoringPlus(string Parameters)
        {
            string RegistrationsName = string.Empty;
            string AuthToken = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                RegistrationsName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                AuthToken = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            ViewBag.RegistrationsName = RegistrationsName;
            ViewBag.AuthToken = AuthToken;
            return View();
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, DPMLicenseEnabled]
        public ActionResult RemoveDUNSMonitoringPlus(string RegistrationsName, HttpPostedFileBase file, string AuthToken)
        {
            if (file != null && CommonMethod.CheckFileType(".txt", file.FileName.ToLower()))
            {
                if (file.ContentLength > 0)
                {
                    string path = string.Empty;
                    string url = Request.Url.Authority;
                    string tempDirectory = "~/Upload/MonitoringDirectPlusAddDuns/" + url;
                    string directory = Server.MapPath(tempDirectory.Replace(":", ""));
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    string FileName = System.DateTime.Now.Ticks + "_" + file.FileName;
                    path = Path.Combine(directory, Path.GetFileName(FileName));
                    file.SaveAs(path);
                    path = CheckNumberOfLinesINDUNSRegistration(path);
                    Utility.Utility utility = new Utility.Utility();
                    AddRemoveDUNSToMonitoringResponse addDUNsToMonitoringResponse = utility.RemoveDunsToMonitoring(RegistrationsName, path, AuthToken);
                    if (addDUNsToMonitoringResponse.information != null && !string.IsNullOrEmpty(addDUNsToMonitoringResponse.information.message))
                    {
                        ViewBag.SuccessMessage = addDUNsToMonitoringResponse.information.message;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = addDUNsToMonitoringResponse.error.errorMessage;
                    }
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanFileEmpty }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgInvalidFile }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true, message = ImportDataLang.msgDataImportSuccessfully }, JsonRequestBehavior.AllowGet);
        }

        public string CheckNumberOfLinesINDUNSRegistration(string path)
        {
            int lines = System.IO.File.ReadAllLines(path).Count();
            if (lines > 100000)
            {
                try
                {
                    var first10Lines = System.IO.File.ReadLines(path).Take(10000).ToList();
                    string newFile = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "_New" + Path.GetExtension(path);
                    var file = new FileStream(newFile, FileMode.Create);
                    var writer = new StreamWriter(file);
                    writer.Write(string.Join("\r\n", first10Lines.ToArray()));
                    writer.Flush();
                    writer.Dispose();
                    writer.Close();
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    path = newFile;
                }
                catch (Exception)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    throw;
                }
            }
            return path;
        }
        #endregion

        #region "DUNS Sample File"
        //Download Sample File of DUNS
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain, DPMLicenseEnabled]
        public ActionResult DownloadSampleFile()
        {
            //Download Sample File of DUNS
            string FilePath = string.Empty;
            FilePath = Server.MapPath("~/DownloadApps/DownloadMonitoringSampleFile/SampleFileDUNsMonitoring.txt");
            if (!System.IO.File.Exists(FilePath))
            {
                return null;
            }
            return File(FilePath, "text/plain", Path.GetFileName(FilePath));
        }
        #endregion

        #region "Suprressed and Unsuprressed DUNS"
        //Suprressed and Unsuprressed DUNS
        [RequestFromSameDomain, RequestFromAjax, DPMLicenseEnabled]
        public JsonResult SuppUnsuppDUNS(string Parameters)
        {
            string referenceName = string.Empty;
            string AuthToken = string.Empty;
            bool isSuprressed = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                referenceName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                isSuprressed = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                AuthToken = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
            }
            Utility.Utility utility = new Utility.Utility();
            utility.SuppressUnSupressRegistration(referenceName, !isSuprressed, AuthToken);
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Suprressed and Unsuprressed DUNS
            fac.SuppressUnSupressNotifications(referenceName, !isSuprressed);
            return Json(new { result = true, message = CommonMessagesLang.msgSuccess }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [RequestFromSameDomain, RequestFromAjax, DPMLicenseEnabled]
        public JsonResult SyncDUNS()
        {
            SyncRegistration();
            return Json(new { result = true, message = DandBSettingLang.msgMonitoringRegistration }, JsonRequestBehavior.AllowGet);
        }
        [DPMLicenseEnabled]
        public List<SelectListItem> SyncRegistration()
        {
            List<SelectListItem> lstMonitoringRegistrations = new List<SelectListItem>();
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);

            //Get DirectPlus Credentials 
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            if (lstAuth != null && lstAuth.Any())
            {
                List<ThirdPartyAPICredentialsEntity> lstDirectPlusAuth = lstAuth.Where(x => x.APIType.ToLower() == ApiLayerType.Directplus.ToString().ToLower()).ToList();
                if (lstDirectPlusAuth != null && lstDirectPlusAuth.Any())
                {
                    foreach (var authItem in lstDirectPlusAuth)
                    {
                        ListMonitoringRegistrationResponse objtMonitoringRegistrationResponse = GetListMonitoringRegistrationResponse(authItem.AuthToken);
                        if (objtMonitoringRegistrationResponse != null && objtMonitoringRegistrationResponse.messages != null && objtMonitoringRegistrationResponse.messages.references != null)
                        {
                            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                            string NotDeleteRegistration = string.Join(",", objtMonitoringRegistrationResponse.messages.references.ToArray());
                            //sync DPMRegistration Name
                            fac.DeleteAllDPMRegistration(NotDeleteRegistration, authItem.CredentialId);
                            foreach (var item in objtMonitoringRegistrationResponse.messages.references)
                            {
                                MonitoringRegistrationDetailResponse monitoringRegistrationDetailResponse = new MonitoringRegistrationDetailResponse();
                                monitoringRegistrationDetailResponse = GetMonitoringRegistrationDetailResponse(item.ToString(), authItem.AuthToken);
                                //insert Registration name in database
                                fac.DPMInsertRegistration(monitoringRegistrationDetailResponse.messages.registration.reference, monitoringRegistrationDetailResponse.messages.registration.Tags, monitoringRegistrationDetailResponse.messages.notificationsSuppressed, monitoringRegistrationDetailResponse.messages.registration.productId, monitoringRegistrationDetailResponse.messages.registration.versionId, monitoringRegistrationDetailResponse.messages.registration.email, monitoringRegistrationDetailResponse.messages.registration.fileTransferProfile, monitoringRegistrationDetailResponse.messages.registration.description, monitoringRegistrationDetailResponse.messages.registration.deliveryTrigger, monitoringRegistrationDetailResponse.messages.registration.deliveryFrequency, monitoringRegistrationDetailResponse.messages.dunsCount, monitoringRegistrationDetailResponse.messages.registration.seedData, authItem.CredentialId, monitoringRegistrationDetailResponse.messages.registration.blockIds);
                                //insert Registration name in List
                                lstMonitoringRegistrations.Add(new SelectListItem { Value = item.ToString(), Text = item.ToString() });
                            }
                        }
                    }
                }
            }
            return lstMonitoringRegistrations;
        }

        [HttpPost, ValidateAntiForgeryToken, DPMLicenseEnabled]
        public ActionResult ExportDUNS(string RegistrationName)
        {
            DataTable dt = GetDPMDunsRegistrationByRegistrationName(RegistrationName);
            dt.Columns.Remove("DunsRegistrationId");
            dt.Columns.Remove("MonitoringRegistrationName");
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));
            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }
            return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", RegistrationName + "_" + DateTime.Now.Ticks + ".csv");
        }
        [DPMLicenseEnabled]
        public ActionResult MonitoringPlusRegistrationDUNSDetails(string Parameters, int? DUNSDetailsPage, int? DUNSDetailsSortby, int? DUNSDetailsSortorder, int? DUNSDetailsPagevalue, string RegistrationName = "", string DnBDUNSNumber = "", string AuthToken = "")
        {

            #region  pagination
            if (!(DUNSDetailsSortby.HasValue && DUNSDetailsSortby.Value > 0))
                DUNSDetailsSortby = 1;

            if (!(DUNSDetailsSortorder.HasValue && DUNSDetailsSortorder.Value > 0))
                DUNSDetailsSortorder = 1;

            int sortParam = int.Parse(DUNSDetailsSortby.ToString() + DUNSDetailsSortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = DUNSDetailsPage.HasValue ? DUNSDetailsPage.Value : 1;
            int pageSize = DUNSDetailsPagevalue.HasValue ? DUNSDetailsPagevalue.Value : 10;
            #endregion
            bool isFromMainPage = false;

            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                RegistrationName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                AuthToken = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                isFromMainPage = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
            }
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);

            DataTable dt = fac.GetDPMDunsRegistrationList(RegistrationName, DnBDUNSNumber, sortParam, currentPageIndex, pageSize, out totalCount);

            #region Set Viewbag
            ViewBag.DUNSDetailsSortby = DUNSDetailsSortby;
            ViewBag.DUNSDetailsSortorder = DUNSDetailsSortorder;
            ViewBag.DUNSDetailsPageno = currentPageIndex;
            ViewBag.DUNSDetailsPagevalue = pageSize;
            SessionHelper.DUNSDetailsPagevalue = Convert.ToString(pageSize);
            ViewBag.GetDPMDunsRegistrationByRegistrationName = dt;
            ViewBag.RegistrationName = RegistrationName;
            ViewBag.AuthToken = AuthToken;
            #endregion
            IPagedList<dynamic> pagedMonitorProfile = new StaticPagedList<dynamic>(dt.AsEnumerable().ToList(), currentPageIndex, pageSize, totalCount);

            if (isFromMainPage)
                return View("MonitoringPlusRegistrationDUNSDetails", pagedMonitorProfile);
            else
                return PartialView("_MonitoringPlusRegistrationDUNSDetails", pagedMonitorProfile);
        }
        #endregion
    }
}