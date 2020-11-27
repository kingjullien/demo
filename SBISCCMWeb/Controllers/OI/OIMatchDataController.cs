using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.OI.CleanseMatch;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using SBISCompanyCleanseMatchFacade.Objects.OI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers.OI
{
    [Authorize, OrbLicenseEnabled, ValidateAccount, TwoStepVerification]
    public class OIMatchDataController : BaseController
    {
        // GET: OIMatchData
        [Route("OI/MatchData")]
        public ActionResult Index(int? OIMatchPage, int? OIMatchSortby, int? OIMatchsortorder, int? OIMatchsortPagevalue, bool IncludeWithCandidates = true, bool IncludeWithoutCandidates = true)
        {
            // Clear Work Queue for Data
            StewUserActivityCloseWindowOI();
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            #region  pagination
            int pageNumber = (OIMatchPage ?? 1);
            if (!(OIMatchSortby.HasValue && OIMatchSortby.Value > 0))
                OIMatchSortby = 1;

            if (!(OIMatchsortorder.HasValue && OIMatchsortorder.Value > 0))
                OIMatchsortorder = 2;

            int sortParam = int.Parse(OIMatchSortby.ToString() + OIMatchsortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = OIMatchPage.HasValue ? OIMatchPage.Value : 1;
            int pageSize = OIMatchsortPagevalue.HasValue ? OIMatchsortPagevalue.Value : 10;
            #endregion
            LstOIMatchCompany lstcompany = fac.GetOICompanyList(Helper.oUser.UserId, IncludeWithCandidates, IncludeWithoutCandidates, currentPageIndex, pageSize, out totalCount);

            #region Set Viewbag
            ViewBag.OIMatchSortby = OIMatchSortby;
            ViewBag.OIMatchsortorder = OIMatchsortorder;
            ViewBag.OIMatchPageno = currentPageIndex;
            ViewBag.OIMatchsortPagevalue = pageSize;

            //MP-575 OI - Add Filters for Match vs No candidate
            ViewBag.IncludeWithCandidates = IncludeWithCandidates;
            ViewBag.IncludeWithoutCandidates = IncludeWithoutCandidates;

            OICompanyFacade companyfac = new OICompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            // DB Changes (MP-716)
            var sessionData = companyfac.OIGetUserSessionFilterText(Helper.oUser.UserId);
            if (sessionData != null)
            {
                ViewBag.UserSesionText = sessionData.FilterText;
                ViewBag.UserFilterExists = sessionData.FilterExists;
            }
            IPagedList<OICompanyEntity> pagedCompany = new StaticPagedList<OICompanyEntity>(lstcompany.lstOICompany.ToList(), currentPageIndex, pageSize, totalCount);
            ViewBag.QueueMessage = lstcompany.Message;

            SessionHelper.lstOIMatchCompanies = JsonConvert.SerializeObject(lstcompany);
            SessionHelper.lstOIMatchCompaniesTotalcount = totalCount;

            #endregion
            if (Request.IsAjaxRequest())
            {
                return View("~/Views/OI/OIMatchData/_Index.cshtml", pagedCompany);
            }
            else
            {
                return View("~/Views/OI/OIMatchData/Index.cshtml", pagedCompany);
            }
        }

        [RequestFromSameDomain]
        [Route("OIMatchData/GetFilteredCompanyList/{OIMatchSortby?}/{OIMatchsortorder?}/{OIMatchsortPagevalue?}/{IncludeWithCandidates?}/{IncludeWithoutCandidates?}")]
        public ActionResult GetFilteredCompanyList(int? OIMatchPage, int? OIMatchSortby, int? OIMatchsortorder, int? OIMatchsortPagevalue, bool IncludeWithCandidates = true, bool IncludeWithoutCandidates = true, string FilterdMatchValue = "both")
        {
            #region  pagination
            int pageNumber = (OIMatchPage ?? 1);
            if (!(OIMatchSortby.HasValue && OIMatchSortby.Value > 0))
                OIMatchSortby = 1;

            if (!(OIMatchsortorder.HasValue && OIMatchsortorder.Value > 0))
                OIMatchsortorder = 2;

            int sortParam = int.Parse(OIMatchSortby.ToString() + OIMatchsortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = OIMatchPage.HasValue ? OIMatchPage.Value : 1;
            int pageSize = OIMatchsortPagevalue.HasValue ? OIMatchsortPagevalue.Value : 10;
            #endregion
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            LstOIMatchCompany lstcompany = new LstOIMatchCompany();
            // Check and filter data from the session Filter.
            if (SessionHelper.lstOIMatchCompanies != null)
            {
                lstcompany = JsonConvert.DeserializeObject<LstOIMatchCompany>(SessionHelper.lstOIMatchCompanies);
                ViewBag.QueueMessage = lstcompany.Message;
                totalCount = Convert.ToInt32(SessionHelper.lstOIMatchCompaniesTotalcount);
            }
            else
            {
                lstcompany = fac.GetOICompanyList(Helper.oUser.UserId, IncludeWithCandidates, IncludeWithoutCandidates, currentPageIndex, pageSize, out totalCount);
                ViewBag.QueueMessage = lstcompany.Message;
                SessionHelper.lstOIMatchCompanies = JsonConvert.SerializeObject(lstcompany);
            }

            if (OIMatchsortorder != null && OIMatchSortby != null)
            {
                lstcompany.lstOICompany = SortData(lstcompany.lstOICompany, Convert.ToInt32(OIMatchsortorder), Convert.ToInt32(OIMatchSortby));
            }

            IPagedList<OICompanyEntity> pagedCompany = new StaticPagedList<OICompanyEntity>(lstcompany.lstOICompany.ToList(), currentPageIndex, pageSize, totalCount);

            #region Set Viewbag
            ViewBag.OIMatchSortby = OIMatchSortby;
            ViewBag.OIMatchsortorder = OIMatchsortorder;
            ViewBag.OIMatchPageno = currentPageIndex;
            ViewBag.OIMatchsortPagevalue = pageSize;

            //MP-575 OI - Add Filters for Match vs No candidate
            ViewBag.IncludeWithCandidates = IncludeWithCandidates;
            ViewBag.IncludeWithoutCandidates = IncludeWithoutCandidates;
            ViewBag.FilterdMatchValue = FilterdMatchValue;
            #endregion
            return View("~/Views/OI/OIMatchData/_Index.cshtml", pagedCompany);
        }
        // Data Sort by sort order and sort by
        public List<OICompanyEntity> SortData(List<OICompanyEntity> lstcompany, int sortorder, int sortby)
        {
            // sort order 1 for ascending order and 2 for descending order.
            switch (sortby)
            {
                case 1:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.SrcRecordId).ToList() : lstcompany.OrderByDescending(x => x.SrcRecordId).ToList();
                    break;
                case 2:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpCompanyName).ToList() : lstcompany.OrderByDescending(x => x.InpCompanyName).ToList();
                    break;
                case 3:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpAddress1).ToList() : lstcompany.OrderByDescending(x => x.InpAddress1).ToList();
                    break;
                case 4:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpCity).ToList() : lstcompany.OrderByDescending(x => x.InpCity).ToList();
                    break;
                case 5:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpState).ToList() : lstcompany.OrderByDescending(x => x.InpState).ToList();
                    break;
                case 6:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpPostalCode).ToList() : lstcompany.OrderByDescending(x => x.InpPostalCode).ToList();
                    break;
                case 7:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpCountryISOAlpha2Code).ToList() : lstcompany.OrderByDescending(x => x.InpCountryISOAlpha2Code).ToList();
                    break;
                case 8:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.InpPhoneNbr).ToList() : lstcompany.OrderByDescending(x => x.InpPhoneNbr).ToList();
                    break;
                case 9:
                    lstcompany = sortorder == 1 ? lstcompany.OrderBy(x => x.CandidateCount).ToList() : lstcompany.OrderByDescending(x => x.CandidateCount).ToList();
                    break;
            }
            return lstcompany;
        }

        #region "Match Details Popup"
        [HttpGet]
        public ActionResult MatchDataDetail(string Parameters)
        {
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            int inputId;
            if (!string.IsNullOrEmpty(Parameters))
            {
                inputId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
                SessionHelper.oIlstMatchDetails = JsonConvert.SerializeObject(oIlstMatch);
            }
            return View("~/Views/OI/OIMatchData/_OIMatchDetails.cshtml", oIlstMatch);
        }

        [HttpPost, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain]
        public ActionResult DeleteStweMatchSearch(string Parameters)
        {
            int inputId = 0, MatchId = 0;
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                MatchId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));

                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                string message = fac.StewDeleteOIMatch(inputId, MatchId);
                if (string.IsNullOrEmpty(message))
                {
                    oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
                }
                Helper.DeletedMatchId = MatchId;
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult UndoStweMatchSearch(string Parameters)
        {
            int inputId = 0, MatchId = 0;
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                MatchId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));

                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                string message = fac.StewUndoOIMatch(inputId, MatchId);
                if (string.IsNullOrEmpty(message))
                {
                    oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
                }
                Helper.DeletedMatchId = 0;
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }

        [HttpPost, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain]
        public JsonResult ResertUndoMatchId()
        {
            Helper.DeletedMatchId = 0;
            return Json(CommonMessagesLang.msgResetSuccessfully);
        }

        [HttpPost, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain]
        public ActionResult RefreshMatchSearch(string Parameters)
        {
            int inputId = 0;
            string MatchId = "";
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                MatchId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter, MatchId);
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult OIMatchNewSearch(OIlstMatchDetails model, string searchBtn)
        {
            OICleanseMatchViewModel oICleanseMatchViewModel = new OICleanseMatchViewModel();
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(searchBtn) && searchBtn == "Search" && (string.IsNullOrEmpty(model.lstOICompanyInput.InpCompanyName) || string.IsNullOrEmpty(model.lstOICompanyInput.InpCountryISOAlpha2Code)))
            {
                return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
            }
            string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;

            string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
            string SubDomain = hostParts[0];

            oICleanseMatchViewModel = APIUtility.GetOICleanseMatchResult(model.lstOICompanyInput.InpCompanyName, model.lstOICompanyInput.InpAddress1, model.lstOICompanyInput.InpAddress2, model.lstOICompanyInput.InpCity, model.lstOICompanyInput.InpState, model.lstOICompanyInput.InpCountryISOAlpha2Code, model.lstOICompanyInput.InpPostalCode, model.lstOICompanyInput.InpPhoneNbr, ConnectionString, SubDomain, model.lstOICompanyInput.SrcRecordId, model.lstOICompanyInput.InputId, model.lstOICompanyInput.InpOrbNum, model.lstOICompanyInput.InpEIN, model.lstOICompanyInput.InpWebsite, model.lstOICompanyInput.InpEmail);
            if (oICleanseMatchViewModel != null)
            {
                string Message = fac.GetNewSearch(Convert.ToInt32(model.lstOICompanyInput.InputId), oICleanseMatchViewModel.MatchUrl, oICleanseMatchViewModel.ResponseJson);
                if (!string.IsNullOrEmpty(Message))
                {
                    string[] splitStr = Message.Split('.');
                    string splitMessage = splitStr[0] + '.';
                    ViewBag.Message = Message.Replace("\r", "").Replace("\n", "");
                }
                else if (!string.IsNullOrEmpty(oICleanseMatchViewModel.Error))
                {
                    ViewBag.Message = oICleanseMatchViewModel.Error.Replace("\r", "").Replace("\n", "");
                }

                oIlstMatch = fac.GetCompanyMatchDetails(Convert.ToInt32(model.lstOICompanyInput.InputId), Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
                SessionHelper.oIlstMatchDetails = JsonConvert.SerializeObject(oIlstMatch);
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }
        [RequestFromAjax, RequestFromSameDomain]
        public ActionResult AssignORBnum(string Parameters)
        {
            int inputId = 0;
            string OrbNum = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                OrbNum = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                string message = fac.AssignStewMatchRecord(inputId, OrbNum, Helper.oUser.UserId);
                if (string.IsNullOrEmpty(message))
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        #endregion


        #region "Match Meta data"
        [HttpGet]
        public ActionResult MatchMetadata(string Parameters)
        {
            int inputId = 0;
            string OrbNum = string.Empty, dataNext = string.Empty, dataPrev = string.Empty;
            bool IsPartial = false;
            OIlstMatchMetaDetails oIlstMatchMeta = new OIlstMatchMetaDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                OrbNum = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                dataNext = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                dataPrev = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                IsPartial = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));

                ViewBag.dataNext = dataNext;
                ViewBag.dataPrev = dataPrev;
                ViewBag.OrbNum = OrbNum;
                OIlstMatchDetails objoIlstMatch = JsonConvert.DeserializeObject<OIlstMatchDetails>(SessionHelper.oIlstMatchDetails);
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatchMeta = fac.GetStewOIMatchMetadata(inputId, OrbNum);
                try
                {
                    ViewBag.NextToNextDUNS = dataNext != "" ? objoIlstMatch.lstOIMatchDetail.SkipWhile(p => p.orb_num != dataNext).ElementAt(1).orb_num : objoIlstMatch.lstOIMatchDetail.SkipWhile(p => p.orb_num != OrbNum).ElementAt(1).orb_num;
                }
                catch
                {
                    ViewBag.NextToNextDUNS = "";
                }
                try
                {
                    ViewBag.PrevToPrevDUNS = dataPrev != "" ? objoIlstMatch.lstOIMatchDetail.TakeWhile(p => p.orb_num != dataPrev).LastOrDefault().orb_num : objoIlstMatch.lstOIMatchDetail.TakeWhile(p => p.orb_num != OrbNum).LastOrDefault().orb_num;
                }
                catch
                {
                    ViewBag.PrevToPrevDUNS = "";

                }
            }
            if (!IsPartial)
            {
                return PartialView("~/Views/OI/OIMatchData/_MatchMetadataDetail.cshtml", oIlstMatchMeta);
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchMetadata.cshtml", oIlstMatchMeta);
        }
        #endregion

        #region "View Resolution Map"
        // For viewing Resolution Map
        [HttpGet]
        public ActionResult ViewresolutionMap(string Parameters)
        {
            int inputId = 0;
            string MatchId = "";
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            bool isPartial = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                MatchId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                isPartial = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));

                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter, MatchId);
            }
            if (!isPartial)
            {
                return PartialView("~/Views/OI/OIMatchData/_ViewresolutionMap.cshtml", oIlstMatch);
            }
            return PartialView("~/Views/OI/OIMatchData/ViewresolutionMap.cshtml", oIlstMatch);
        }
        #endregion

        #region Accept From File
        // Accepting From File
        public ActionResult AcceptFromFile()
        {
            // Accepting from file
            DataTable dt = new DataTable();
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAcceptData = new DataTable();

            // Get the columnname from the table 
            dtAcceptData = sfac.GetOIImportDataForAcceptColumnsName();
            Session["OIMatchdata_dtAcceptData"] = dtAcceptData;
            List<string> columnName = new List<string>();
            if (dtAcceptData.Rows.Count > 0)
            {
                for (int k = 0; k < dtAcceptData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtAcceptData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return PartialView("~/Views/OI/OIMatchData/_acceptImportFile.cshtml", pagedProducts);
        }

        // Binding the file data
        [HttpPost]
        public ActionResult BindAcceptFileMapping(HttpPostedFileBase file)
        {
            // Gets the column names from file to fill in the dropdown
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            DataTable dt = new DataTable();


            //Get Import File Column Name to fill in the dropdown
            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAcceptData = new DataTable();
            if (Session["OIMatchdata_dtAcceptData"] == null)
            {
                dtAcceptData = sfac.GetOIImportDataForAcceptColumnsName();
            }
            else
            {
                dtAcceptData = Session["OIMatchdata_dtAcceptData"] as DataTable;
            }

            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                string path = string.Empty;
                string directory = Server.MapPath("~/Upload/UploadAcceptFile");
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
                try
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
                    Session["OIMatchdata_data"] = dt;
                    stream.Close();
                }
                catch (Exception)
                {
                    //Empty catch block to stop from breaking
                }
                if (dt != null && dt.Columns != null && dt.Columns.Count > 0)
                {
                    lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                    int i = 0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;
                    }
                }
                System.IO.File.Delete(path);

            }
            List<string> columnName = new List<string>();
            if (dtAcceptData.Rows.Count > 0)
            {
                for (int k = 0; k < dtAcceptData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtAcceptData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;

            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);

            return PartialView("~/Views/OI/OIMatchData/_bindAcceptMapping.cshtml", pagedProducts);
        }

        // On submitting the file
        public JsonResult AcceptDataFile(string[] OrgColumnName, string[] ExcelColumnName)
        {
            // On submitting the file 
            string Message = string.Empty;
            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();
            if (Session["OIMatchdata_data"] != null)
            {
                dt = Session["OIMatchdata_data"] as DataTable;
                // Get Country groups Columns Name
                if (Session["OIMatchdata_dtAcceptData"]  == null)
                {
                    dtOrgColumns = sfac.GetOIImportDataForAcceptColumnsName();
                }
                else
                {
                    dtOrgColumns = Session["OIMatchdata_dtAcceptData"] as DataTable;
                }
                dtColumns.Columns.Add("Tablecolumn");
                dtColumns.Columns.Add("Excelcolumn");
                DataRow dr = dtColumns.NewRow();
                for (int i = 0; i < OrgColumnName.Length; i++)
                {
                    if (Convert.ToString(OrgColumnName[i]) != "-Select-")
                    {
                        dr = dtColumns.NewRow();
                        dr["Tablecolumn"] = Convert.ToString(ExcelColumnName[i]);
                        dr["Excelcolumn"] = Convert.ToString(OrgColumnName[i]);
                        dtColumns.Rows.Add(dr);
                    }
                }
                try
                {
                    //bulk insert new records
                    bool IsDataInsert = AcceptFileBulkInsert(dt, dtColumns, out Message);
                }
                catch (Exception ex)
                {
                    Message = CommonMessagesLang.msgCommanEnableFileImport;
                }
                Session["OIMatchdata_dtAcceptData"] = null;
                Session["OIMatchdata_data"] = null;
                return new JsonResult { Data = Message };
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSomethingWentWrong };
            }
        }

        // Bulk insert of the new records from the file
        private bool AcceptFileBulkInsert(DataTable dt, DataTable dtColumns, out string Message)
        {
            //Bulk insert of new records
            bool DataInsert = false;
            using (SqlConnection connection = new SqlConnection(this.CurrentClient.ApplicationDBConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //Creating Transaction so that it can rollback if got any error while uploading
                SqlTransaction trans = connection.BeginTransaction();
                //Start bulkCopy
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, trans))
                {
                    bulkCopy.BulkCopyTimeout = 0;
                    foreach (DataRow drCol in dtColumns.Rows)
                    {
                        bulkCopy.ColumnMappings.Add(drCol["Excelcolumn"].ToString(), drCol["Tablecolumn"].ToString());
                    }
                    bulkCopy.DestinationTableName = "ext.OIStgInputDataForAccept";// Table name from which the insertion is done
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        Message = sfac.AcceptOIMatchDataFromImport(Helper.oUser.UserId);
                    }
                    catch (Exception ex)
                    {
                        //Exception message if bulkinsertion is not done
                        Message = ex.Message.ToString();
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }
        #endregion

        #region "Bing search"
        //view bing search form to search data 
        [HttpGet]
        public async Task<ActionResult> OIBingSearch(string Parameters)
        {
            string SearchValue = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                SearchValue = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = HttpUtility.UrlDecode(SearchValue);
                }
            }
            ViewBag.SearchValue = SearchValue;
            List<webSearch> SearchResults = new List<webSearch>();
            try
            {
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    var res = await CommonMethod.MakeRequest(SearchValue);

                    for (int i = 0; i < res.Count(); i++)
                    {
                        BingSearchModel bingser = res.ElementAt(i);
                        SearchResults.Add(new webSearch { WSer = bingser });
                    }
                }
            }
            catch (Exception ex)
            {
                return View("~/Views/OI/OIMatchData/OIBingSearch.cshtml", SearchResults);
            }
            return View("~/Views/OI/OIMatchData/OIBingSearch.cshtml", SearchResults);
        }

        // When search is made in the bing search popup
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true)]
        public async Task<ActionResult> OIBingSearch(string SearchValue, string btnSearch)
        {
            //bing search 
            List<webSearch> SearchResults = new List<webSearch>();
            try
            {
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    //send request for bing search 
                    var res = await CommonMethod.MakeRequest(SearchValue);

                    for (int i = 0; i < res.Count(); i++)
                    {
                        BingSearchModel bingser = res.ElementAt(i);
                        SearchResults.Add(new webSearch { WSer = bingser });
                    }
                }
                ViewBag.SearchValue = SearchValue;
            }
            catch (Exception ex)
            {
                return View("~/Views/OI/OIMatchData/_bingSearch.cshtml", SearchResults);
            }
            return View("~/Views/OI/OIMatchData/_bingSearch.cshtml", SearchResults);
        }

        #endregion
        [HttpGet]
        public ActionResult InsertUpdateUserMatchFilter(string Parameters)
        {
            int FilterId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                FilterId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }
            OIUserMatchFilterEntity model = new OIUserMatchFilterEntity();
            model.Enabled = true;
            if (FilterId > 0)
            {
                OIUserMatchFilterFacade fac = new OIUserMatchFilterFacade(this.CurrentClient.ApplicationDBConnectionString);
                model = fac.GetUserMatchFilterById(Helper.oUser.UserId, FilterId);
            }
            return View("~/Views/OI/OIMatchData/InsertUpdateUserMatchFilter.cshtml", model);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult InsertUpdateUserMatchFilter(OIUserMatchFilterEntity model)
        {
            OIUserMatchFilterFacade fac = new OIUserMatchFilterFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.UserId = Helper.oUser.UserId;
            try
            {
                int filterId = fac.InsertUpdateUserMatchFilter(model);
                if (model.FilterId > 0)
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
            }
        }


        // Gets the user filter
        public ActionResult GetUserMatchFilter()
        {
            OIUserMatchFilterFacade fac = new OIUserMatchFilterFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<OIUserMatchFilterEntity> lst = fac.GetUserMatchFilterList(Helper.oUser.UserId);
            return View("~/Views/OI/OIMatchData/GetUserMatchFilter.cshtml", lst);
        }
        // Deletes the filter
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteUserMatchFilter(string Parameters)
        {
            int FilterId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                FilterId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            OIUserMatchFilterFacade fac = new OIUserMatchFilterFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteUserMatchFilter(FilterId, Helper.oUser.UserId);
            return Json(CommonMessagesLang.msgCommanDeleteMessage);
        }
        // Enable-Disable the filter
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult EnableDisabledUserMatchFilter(string Parameters)
        {
            int FilterId = 0;
            bool IsEnable = false;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                FilterId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                IsEnable = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            OIUserMatchFilterFacade fac = new OIUserMatchFilterFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.EnableDisableUserMatchFilter(FilterId, Helper.oUser.UserId, IsEnable);
            if (IsEnable)
            {
                return Json(OIMatchDataLang.msgEnableRecord);
            }
            else
            {
                return Json(OIMatchDataLang.msgdisableRecord);
            }
        }

        //Set Apply Match filter 
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts, HttpPost]
        public ActionResult SetApplyFilter(string Parameters)
        {
            bool IsApply = false;
            int inputId = 0;
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                IsApply = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));

                Helper.EnableApplyMatchFilter = IsApply;
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
            }

            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }

        #region "Add Company By ORB" 
        // Add company name on the search data records
        public ActionResult OIAddCompanyMatch(string Parameters)
        {
            string OriginalSrcRecId = "", orb_num = "", InputId = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                OriginalSrcRecId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1)/* + ":" + Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1)*/;
                orb_num = "ORB-" + OriginalSrcRecId;
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            // Open Popup for the add company from the matches

            ViewBag.OriginalSrcRecordId = OriginalSrcRecId;
            ViewBag.orb_num = orb_num;
            ViewBag.InputId = InputId;
            return View("~/Views/OI/OIMatchData/AddOICompanyMatch.cshtml");
        }


        //Add company match
        [HttpPost, ValidateInput(true), RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult OIAddCompanyMatch(string SrcId, string InputId, string orb_num, string Tag)
        {
            // On selecting Adding Company
            OICleanseMatchViewModel OIMatch = new OICleanseMatchViewModel();
            OICompanyMatchFacade company = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                OICompanyEntity Company = new OICompanyEntity();
                if (!string.IsNullOrWhiteSpace(orb_num))
                {
                    // Add Match Record as Company record
                    Company.Tags = Tag;
                    Company.InputId = InputId;
                    ViewBag.matchRecord = Company;
                    company.OIAddRecordAsNewCompanyFromMatch(InputId, orb_num, SrcId == null ? "123" : SrcId, Tag, Helper.oUser.UserId);
                }
            }
            catch (SqlException ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            return new JsonResult { Data = "success" };
        }
        #endregion


        #region OI Delete From Files
        //Deleting records from the file 
        public ActionResult DeleteFromFile()
        {
            DataTable dt = new DataTable();
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtRejectData = new DataTable();
            dtRejectData = sfac.GetStgInputDataForPurgeColumnName();
            List<string> columnName = new List<string>();
            if (dtRejectData.Rows.Count > 0)
            {
                for (int k = 0; k < dtRejectData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtRejectData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return PartialView("~/Views/OI/OIMatchData/_deleteImportFile.cshtml", pagedProducts);
        }

        // Binding the data of the file selected
        [HttpPost]
        public ActionResult BindDeleteMapping(HttpPostedFileBase file)
        {
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            DataTable dt = new DataTable();

            //Get Import File Column Name to fill in dropdown

            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtRejectData = new DataTable();
            dtRejectData = sfac.GetStgInputDataForPurgeColumnName();

            //checks the file format,only excel files are allowed
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                string path = string.Empty;
                string directory = Server.MapPath("~/Upload/UploadCommandFile");
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
                try
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
                    Session["OIMatchdata_DeleteMappingData"] = dt;
                    stream.Close();
                }
                catch (Exception)
                {
                    //Empty catch block to stop from breaking
                }
                if (dt != null && dt.Columns != null && dt.Columns.Count > 0)
                {
                    lstAllFilter.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                    int i = 0;
                    foreach (DataColumn c in dt.Columns)
                    {
                        lstAllFilter.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                        i++;

                    }
                }
                System.IO.File.Delete(path);

            }
            List<string> columnName = new List<string>();
            if (dtRejectData.Rows.Count > 0)
            {
                for (int k = 0; k < dtRejectData.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtRejectData.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;

            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(new List<dynamic>(), 1, 10000, 0);

            return PartialView("~/Views/OI/OIMatchData/_bindDataMapping.cshtml", pagedProducts);
        }
        // Deletes the data
        [HttpPost]
        public JsonResult DeleteData(string[] OrgColumnName, string[] ExcelColumnName)
        {
            string Message = string.Empty;
            OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();

            if (Session["OIMatchdata_DeleteMappingData"] != null)
            {
                dt = Session["OIMatchdata_DeleteMappingData"] as DataTable;
                dtOrgColumns = sfac.GetStgInputDataForPurgeColumnName();
                dtColumns.Columns.Add("Tablecolumn");
                dtColumns.Columns.Add("Excelcolumn");
                DataRow dr = dtColumns.NewRow();
                for (int i = 0; i < OrgColumnName.Length; i++)
                {
                    if (Convert.ToString(OrgColumnName[i]) != "-Select-")
                    {
                        dr = dtColumns.NewRow();
                        dr["Tablecolumn"] = Convert.ToString(ExcelColumnName[i]);
                        dr["Excelcolumn"] = Convert.ToString(OrgColumnName[i]);
                        dtColumns.Rows.Add(dr);
                    }
                }
                try
                {
                    //bulk insert new records
                    bool IsDataInsert = BulkInsert(dt, dtColumns, out Message);
                    Session["OIMatchdata_DeleteMappingData"] = null;
                }
                catch (Exception ex)
                {
                    Message = CommonMessagesLang.msgCommanEnableFileImport;
                }
                return new JsonResult { Data = Message };
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSomethingWentWrong };
            }

        }
        private bool BulkInsert(DataTable dt, DataTable dtColumns, out string Message)
        {
            bool DataInsert = false;
            using (SqlConnection connection = new SqlConnection(this.CurrentClient.ApplicationDBConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //Creating Transaction so that it can rollback if got any error while uploading
                SqlTransaction trans = connection.BeginTransaction();
                //Start bulkCopy
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers, trans))
                {
                    //Setting timeout to 0 means no time out for this command will not timeout until upload complete.
                    //Change as per you
                    bulkCopy.BulkCopyTimeout = 0;
                    foreach (DataRow drCol in dtColumns.Rows)
                    {
                        bulkCopy.ColumnMappings.Add(drCol["Excelcolumn"].ToString(), drCol["Tablecolumn"].ToString());
                    }
                    bulkCopy.DestinationTableName = "ext.OIStgInputDataForPurge";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        OISettingFacade sfac = new OISettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        Message = sfac.DeleteCompanyDataFromImport(Helper.oUser.UserId);
                        if (string.IsNullOrEmpty(Message))
                        {
                            Message = DandBSettingLang.msgCommanRejectMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Displays message regarding the exception details
                        Message = ex.Message.ToString();
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }
        #endregion

        #region  Delete Company Data
        // Deletes/Removes a single record from the UI (right - click UI option)
        public JsonResult DeleteCompanyRecord(string Parameters)
        {
            string InputId = string.Empty, SrcRecordId = string.Empty, City = string.Empty, State = string.Empty, CountryCode = string.Empty, Tag = string.Empty, ImportProcess = string.Empty;
            bool DeleteWithCandidates = true;
            bool DeleteWithoutCandidates = true; //give record count before deleting data(MP-660)
            bool GetCountOnly = false;
            int CountryGroupId = 0;

            if (!string.IsNullOrEmpty(Parameters)) ;
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                SrcRecordId = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                City = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                State = Convert.ToString(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
            }
            OICompanyFacade OIfac = new OICompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            OIfac.DeleteCompanyData(Helper.oUser.UserId, DeleteWithCandidates, DeleteWithoutCandidates, InputId, SrcRecordId, City, State, CountryCode, Tag, CountryGroupId, ImportProcess, GetCountOnly);
            return new JsonResult { Data = DandBSettingLang.msgCommonDeleteMessage };
        }
        #endregion


        //"OI -Clean Match Data UI Send to ORB - Company Detail in new window and store to database.(MP-576)"
        #region  "Send To Orb"
        [HttpGet]
        public ActionResult SendToOrb(string Parameters)
        {
            OICompanyInformationEntity Model = new OICompanyInformationEntity();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Model.SrcRecordId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Model.InputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                Model.CompanyName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                Model.Address1 = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Model.Address2 = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                Model.City = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                Model.State = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                Model.PostalCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
                Model.CountryISOAlpha2Code = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1);
                Model.PhoneNbr = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1);
                Model.Website = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 10, 1);
                Model.Email = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 11, 1);
                Model.EIN = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 12, 1);
                Model.OrbNum = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 13, 1);
                Model.Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 14, 1);
            }
            return View("~/Views/OI/OIMatchData/SendToOrb.cshtml", Model);
        }
        //"OI -Clean Match Data UI Send to ORB - Company Detail in new window and store to database.(MP-576)"
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult SendToOrbCompanyInformation(OICompanyInformationEntity Model)
        {
            try
            {
                //Get Domain
                string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
                string[] hostParts = new System.Uri(url).Host.Split('.');
                string domain = hostParts[0];

                Model.Subdomain = domain;
                Model.UserName = Helper.oUser.UserName;
                Model.UserEmail = Helper.oUser.EmailAddress;
                CompanyInformationFacade fac = new CompanyInformationFacade(this.CurrentClient.ApplicationDBConnectionString);
                //Insert Company Information
                fac.InsertCompanyInformation(Model);
                return Json(new { result = true, Message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, Message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region Exporting to Excel
        // Export to Excel from the list of Match Data
        public ActionResult ExportToExcel(string InputId)
        {
            // Export data to Excel Sheet .
            OIExportJobSettingsFacade fac = new OIExportJobSettingsFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtActiveRecords = fac.ExportActiveRecordsToExcel(InputId);
            string fileName = "ActiveRecords_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "Active Records";
            // Remove Tags/Tag column from excel if License Enable Tags is unchecked
            if (Helper.LicenseEnableTags == false && dtActiveRecords.Columns.Contains("Tags"))
            {
                dtActiveRecords.Columns.Remove("Tags");
            }
            if (Helper.LicenseEnableTags == false && dtActiveRecords.Columns.Contains("Tag"))
            {
                dtActiveRecords.Columns.Remove("Tag");
            }
            byte[] response = CommonExportMethods.ExportExcelFile(dtActiveRecords, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion
        #region Export to Excel
        [HttpGet]
        // Added dropdown Export To Excel in additional actions
        public ActionResult ExportRecordToExcel(OIExportToExcel Model)
        {
            return View("~/Views/OI/OIMatchData/ExportRecordToExcel.cshtml", Model);
        }
        // On clicking Export To Excel after giving the values
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult ExportRecordToExcelFile(OIExportToExcel Model)
        {
            //CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            int count = 0;
            OIExportJobSettingsFacade fac = new OIExportJobSettingsFacade(this.CurrentClient.ApplicationDBConnectionString);
            Model.UserId = Helper.oUser.UserId;
            DataTable dtCountry = fac.ExportActiveRecordToExcel(Model);
            string fileName = "ActiveRecords_" + DateTime.Now.Ticks.ToString() + ".xlsx";// file is saved with this filename
            string SheetName = "Active Records";

            // Remove Tags/Tag column from excel if License Enable Tags is unchecked
            if (Helper.LicenseEnableTags == false && dtCountry.Columns.Contains("Tags"))
            {
                dtCountry.Columns.Remove("Tags");
            }
            if (Helper.LicenseEnableTags == false && dtCountry.Columns.Contains("Tag"))
            {
                dtCountry.Columns.Remove("Tag");
            }

            byte[] response = CommonExportMethods.ExportExcelFile(dtCountry, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            //return new JsonResult { Data = DandBSettingLang.msgCommanRejectMessage };
        }
        #endregion


        #region "Window Close Event"
        public void StewUserActivityCloseWindowOI()
        {
            // Set window close event and manage changes discard for page.
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.StewUserActivityCloseWindowOI(Helper.oUser.UserId);
        }
        #endregion

        #region "OI Delete Data"
        // OI Delete Data from Additional Action dropdown
        [HttpGet]
        public ActionResult OIDeleteCompanyData()
        {
            OIExportToExcel Model = new OIExportToExcel();
            Model.GetCountOnly = true;
            return View("~/Views/OI/OIMatchData/OIDeleteCompanyData.cshtml", Model);
        }
        // OI Delete Data from Additional Action dropdown
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain, ValidateInput(false)]
        public ActionResult OIDeleteCompanyData(OIExportToExcel Model)
        {
            Model.UserId = Helper.oUser.UserId;
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            var response = string.Empty;
            bool IsRecordDeleted = false;
            string Message = fac.DeleteCompanyData(Model);
            //give record count before deleting data(MP-660)
            if (Model.GetCountOnly)
            {
                if (!Message.Contains("0"))
                {
                    Message = CommonMessagesLang.msgTotal + " " + Message + CommonMessagesLang.msgWantToContiune;
                    return Json(new { result = true, message = Message, IsRecordDeleted = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CleanDataLang.msgNoRecordsAffected, IsRecordDeleted = false }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = true, message = Message, IsRecordDeleted = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false, message = CleanDataLang.msgNoRecordsAffected, IsRecordDeleted = false }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Refresh Stewardship Queue"
        [HttpPost, ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain]
        public JsonResult RefreshStewardshipQueue()
        {
            //updates the user stewardship list
            OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.StewRefreshUserStewardshipList(Helper.oUser.UserId);
            return Json(CommonMessagesLang.msgSuccess);
        }
        #endregion

        // DB Changes (MP-716)
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteSessionFilter()
        {
            // Delete Session filter for user.
            OICompanyFacade fac = new OICompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            fac.OIDeleteUserSessionFilter(Helper.oUser.UserId);
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        #region  Delete Company Data - Additional Actions
        // Deletes/Removes a single record from the from Addtional Actions Dropdown in candidates popup
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult OIDeleteCompanyRecord(string Parameters)
        {
            string InputId = string.Empty, SrcRecordId = string.Empty, City = string.Empty, State = string.Empty, CountryCode = string.Empty, Tag = string.Empty, ImportProcess = string.Empty;
            bool DeleteWithCandidates = true;
            bool DeleteWithoutCandidates = true;
            bool GetCountOnly = false;
            int CountryGroupId = 0;

            if (!string.IsNullOrEmpty(Parameters)) ;
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0);
            }
            OICompanyFacade OIfac = new OICompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            OIfac.DeleteCompanyData(Helper.oUser.UserId, DeleteWithCandidates, DeleteWithoutCandidates, InputId, SrcRecordId, City, State, CountryCode, Tag, CountryGroupId, ImportProcess, GetCountOnly);
            return new JsonResult { Data = DandBSettingLang.msgCommonDeleteMessage };
        }
        #endregion

        #region MP-770 Add reject data in Match data (ORB)
        // Rejects a candidate
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult OIRejectCandidateRecord(string Parameters)
        {
            string orb_num = string.Empty, Message = string.Empty;
            int InputId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                orb_num = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            OICompanyFacade OIfac = new OICompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            Message = OIfac.RejectCandidate(InputId, orb_num);
            if (Message == "")
            {
                return Json(new { result = true, message = DandBSettingLang.msgCommanRejectMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion




        public ActionResult ReloadMatchSearchDetailList(string Parameters)
        {
            int inputId = 0;
            OIlstMatchDetails oIlstMatch = new OIlstMatchDetails();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                inputId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                OICompanyMatchFacade fac = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
                oIlstMatch = fac.GetCompanyMatchDetails(inputId, Helper.oUser.UserId, Helper.EnableApplyMatchFilter);
            }
            return PartialView("~/Views/OI/OIMatchData/_MatchSearchDetails.cshtml", oIlstMatch);
        }
    }
}