using OfficeOpenXml;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification]
    public class AutoAcceptanceController : BaseController
    {
        public const string AnyGrade = "#", AnyCode = "##";
        // GET: AutoAcceptance
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue, int? ConfidenceCode = null, string MatchGrade = null, int? CountyGroupId = null, string Tags = null)
        {
            if (MatchGrade == "Select Match Grade" || MatchGrade == "0" || string.IsNullOrEmpty(MatchGrade) || MatchGrade == "undefined")
            {
                MatchGrade = null;
            }
            #region Pagination code
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 22;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 30;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            ViewBag.ConfidenceCode = ConfidenceCode;
            ViewBag.MatchGrade = MatchGrade;
            ViewBag.TagList = Tags;
            ViewBag.CountyGroupId = CountyGroupId;
            #endregion
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            List<AutoAdditionalAcceptanceCriteriaEntity> model = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            Tags = Tags == "undefined" ? null : Tags;
            model = fac.GetAutoAcceptanceDetailsPagedSorted(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount, ConfidenceCode == null ? 0 : Convert.ToInt32(ConfidenceCode), MatchGrade, CountyGroupId == null ? 0 : Convert.ToInt32(CountyGroupId), Tags);
            IPagedList<AutoAdditionalAcceptanceCriteriaEntity> pagedAcceptance = new StaticPagedList<AutoAdditionalAcceptanceCriteriaEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            ViewBag.Message = TempData["MessageAAC"];
            TempData["MessageAAC"] = "";
            TempData.Keep();
            if (Request.IsAjaxRequest())
                return PartialView("_index", pagedAcceptance);
            else
                return View(pagedAcceptance);

        }

        #region "Additional Acceptance Criteria"
        //Open Additional Acceptance Popup
        public ActionResult popupWindowAAC(int? CriteriaGroupId)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            if (CriteriaGroupId.HasValue && CriteriaGroupId > 0)
            {
                //set properties of AutoAdditionalAcceptanceCriteria
                objAutoSetting = fac.GetAutoAcceptanceDetailByID(CriteriaGroupId.Value);

                //objAutoSetting.CompanyGrade = objAutoSetting.MatchGrade.Substring(0, 1);
                //objAutoSetting.CompanyCode = objAutoSetting.MDPCode.Substring(0, 2);

                //objAutoSetting.StreetGrade = objAutoSetting.MatchGrade.Substring(1, 1);
                //objAutoSetting.StreetCode = objAutoSetting.MDPCode.Substring(2, 2);

                //objAutoSetting.StreetNameGrade = objAutoSetting.MatchGrade.Substring(2, 1);
                //objAutoSetting.StreetNameCode = objAutoSetting.MDPCode.Substring(4, 2);

                //objAutoSetting.CityGrade = objAutoSetting.MatchGrade.Substring(3, 1);
                //objAutoSetting.CityCode = objAutoSetting.MDPCode.Substring(6, 2);

                //objAutoSetting.StateGrade = objAutoSetting.MatchGrade.Substring(4, 1);
                //objAutoSetting.StateCode = objAutoSetting.MDPCode.Substring(8, 2);

                //objAutoSetting.AddressGrade = objAutoSetting.MatchGrade.Substring(5, 1);
                //objAutoSetting.AddressCode = objAutoSetting.MDPCode.Substring(10, 2);

                //objAutoSetting.PhoneGrade = objAutoSetting.MatchGrade.Substring(6, 1);
                //objAutoSetting.PhoneCode = objAutoSetting.MDPCode.Substring(12, 2);

                //objAutoSetting.ZipGrade = objAutoSetting.MatchGrade.Substring(7, 1);
                //objAutoSetting.Density = objAutoSetting.MatchGrade.Substring(8, 1);

                //objAutoSetting.Uniqueness = objAutoSetting.MatchGrade.Substring(9, 1);
                //objAutoSetting.SIC = objAutoSetting.MatchGrade.Substring(10, 1);
            }
            else
            {
                SetDefaultValue(objAutoSetting);
            }
            TempData.Keep();
            ViewBag.IsReview = false;
            return PartialView("_popupWindowAAC", objAutoSetting);
        }
        // Save Additional Acceptance Popup
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult popupWindowAAC(string ConfidenceCode, string CompanyGrade, string CompanyCode, string StreetGrade, string StreetCode, string StreetNameGrade, string StreetNameCode, string CityGrade,
            string CityCode, string StateGrade, string StateCode, string AddressGrade, string AddressCode, string PhoneGrade, string PhoneCode,
            int GroupId, string ZipGrade, string Density, string Uniqueness, string SIC, string ExcludeFromAutoAccept, string Tags, bool IsReview, int CriteriaGroupId = 0)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            AutoAdditionalAcceptanceCriteriaEntity objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
            //set properties of AutoAdditionalAcceptanceCriteria
            string MatchGradeValue = "A,B,F,Z";
            objAutoSetting.CriteriaGroupId = CriteriaGroupId;
            objAutoSetting.ConfidenceCode = ConfidenceCode;
            objAutoSetting.CompanyGrade = CompanyGrade.Replace(" ", "") == MatchGradeValue ? "#" : (CompanyGrade.Contains("#") ? "#" : CompanyGrade);
            objAutoSetting.CompanyCode = CompanyCode.Contains("##") ? "##" : CompanyCode;
            objAutoSetting.StreetGrade = StreetGrade.Replace(" ", "") == MatchGradeValue ? "#" : (StreetGrade.Contains("#") ? "#" : StreetGrade);
            objAutoSetting.StreetCode = StreetCode.Contains("##") ? "##" : StreetCode;
            objAutoSetting.StreetNameGrade = StreetNameGrade.Replace(" ", "") == MatchGradeValue ? "#" : (StreetNameGrade.Contains("#") ? "#" : StreetNameGrade);
            objAutoSetting.StreetNameCode = StreetNameCode.Contains("##") ? "##" : StreetNameCode;
            objAutoSetting.CityGrade = CityGrade.Replace(" ", "") == MatchGradeValue ? "#" : (CityGrade.Contains("#") ? "#" : CityGrade);
            objAutoSetting.CityCode = CityCode.Contains("##") ? "##" : CityCode;
            objAutoSetting.StateGrade = StateGrade.Replace(" ", "") == MatchGradeValue ? "#" : (StateGrade.Contains("#") ? "#" : StateGrade);
            objAutoSetting.StateCode = StateCode.Contains("##") ? "##" : StateCode;
            objAutoSetting.AddressGrade = AddressGrade.Replace(" ", "") == MatchGradeValue ? "#" : (AddressGrade.Contains("#") ? "#" : AddressGrade);
            objAutoSetting.AddressCode = AddressCode.Contains("##") ? "##" : AddressCode;
            objAutoSetting.PhoneGrade = PhoneGrade.Replace(" ", "") == MatchGradeValue ? "#" : (PhoneGrade.Contains("#") ? "#" : PhoneGrade);
            objAutoSetting.PhoneCode = PhoneCode.Contains("##") ? "##" : PhoneCode;
            objAutoSetting.GroupId = GroupId;
            objAutoSetting.ZipGrade = ZipGrade.Replace(" ", "") == MatchGradeValue ? "#" : (ZipGrade.Contains("#") ? "#" : ZipGrade);
            objAutoSetting.Density = Density.Replace(" ", "") == MatchGradeValue ? "#" : (Density.Contains("#") ? "#" : Density);
            objAutoSetting.Uniqueness = Uniqueness.Replace(" ", "") == MatchGradeValue ? "#" : (Uniqueness.Contains("#") ? "#" : Uniqueness);
            objAutoSetting.SIC = SIC.Replace(" ", "") == MatchGradeValue ? "#" : (SIC.Contains("#") ? "#" : SIC);
            //objAutoSetting.CriteriaId = CriteriaId;
            objAutoSetting.ExcludeFromAutoAccept = Convert.ToBoolean(ExcludeFromAutoAccept != null ? true : false);
            objAutoSetting.GroupName = LoadCountryGroupEntity(this.CurrentClient.ApplicationDBConnectionString).Where(a => a.GroupId.Equals(GroupId)).Select(a => a.GroupName).FirstOrDefault();
            objAutoSetting.Tags = Tags == "0" ? "" : Tags;
            objAutoSetting.UserId = Helper.oUser.UserId;
            ViewBag.IsReview = IsReview;
            ViewBag.IsReviewConfirm = IsReview == true ? true : false;
            try
            {
                if (this.Validate(objAutoSetting))
                {
                    string MatchGrade = objAutoSetting.CompanyGrade + objAutoSetting.StreetGrade + objAutoSetting.StreetNameGrade + objAutoSetting.CityGrade + objAutoSetting.StateGrade + objAutoSetting.AddressGrade + objAutoSetting.PhoneGrade + objAutoSetting.ZipGrade + objAutoSetting.Density + objAutoSetting.Uniqueness + objAutoSetting.SIC;
                    TempData["MatchGrade"] = MatchGrade;
                    //Insert Or Update Data of AutoAdditionalAcceptanceCriteria
                    fac.InsertOrUpdateAcceptanceSettings(objAutoSetting);
                    if (CriteriaGroupId > 0)
                    {
                        ViewBag.Message = MessageCollection.UpdateAutoAcceptance;
                    }
                    else
                    {
                        ViewBag.Message = MessageCollection.InsertAutoAcceptance;
                    }
                    if (CriteriaGroupId > 0)
                        objAutoSetting = fac.GetAutoAcceptanceDetailByID(CriteriaGroupId);

                    PartialView("_popupWindowAAC", objAutoSetting);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            if (CriteriaGroupId > 0)
                objAutoSetting = fac.GetAutoAcceptanceDetailByID(CriteriaGroupId);

            return PartialView("_popupWindowAAC", objAutoSetting);
        }

        public void Clear(CleanseMatchSettingsModel model)
        {
            //assignee model as empty 
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }

        private void SetDefaultValue(AutoAdditionalAcceptanceCriteriaEntity model)
        {
            //set Default value of AutoAdditionalAcceptanceCriteria
            model.CompanyGrade = AnyGrade;
            model.CompanyCode = AnyCode;
            model.StreetGrade = AnyGrade;
            model.StreetCode = AnyCode;
            model.StreetNameGrade = AnyGrade;
            model.StreetNameCode = AnyCode;
            model.CityGrade = AnyGrade;
            model.CityCode = AnyCode;
            model.StateGrade = AnyGrade;
            model.StateCode = AnyCode;
            model.AddressGrade = AnyGrade;
            model.AddressCode = AnyCode;
            model.PhoneGrade = AnyGrade;
            model.PhoneCode = AnyCode;
            model.ZipGrade = AnyGrade;
            model.Density = AnyGrade;
            model.Uniqueness = AnyGrade;
            model.SIC = AnyGrade;
        }

        public bool Validate(AutoAdditionalAcceptanceCriteriaEntity model)
        {
            return model.IsValidSave;
        }

        public List<CountryGroupEntity> LoadCountryGroupEntity(string ConnectionString)
        {
            // Load Country Group Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetCountryGroup();
        }

        public List<MatchGradeEntity> LoadMatchGrades(string ConnectionString)
        {
            // Load Match Entity
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetMatchGrades();
        }

        public List<MatchEntity> LoadTopMatchGrades(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dtResult = fac.GetTopMatchGradeSettings(true);
            List<MatchEntity> lstMatches = new List<MatchEntity>();
            if (dtResult.Rows.Count > 0)
            {
                // Get Match Grade and Confidence Code
                MatchEntity objMatch = new MatchEntity();
                objMatch.DnBConfidenceCode = Convert.ToInt32(0);
                objMatch.DnBMatchGradeText = Convert.ToString("Select Match Grade");
                lstMatches.Add(objMatch);
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    objMatch = new MatchEntity();
                    objMatch.DnBConfidenceCode = Convert.ToInt32(dtResult.Rows[i]["DnBConfidenceCode"]);
                    objMatch.DnBMatchGradeText = Convert.ToString(dtResult.Rows[i]["DnBMatchGradeText"]);
                    lstMatches.Add(objMatch);
                }
            }
            return lstMatches;
        }

        public void SetMatchGradeContent(CleanseMatchSettingsModel model)
        {
            string p1 = "#"; string p2 = "#"; string p3 = "#"; string p4 = "#"; string p5 = "#"; string p7 = "#"; string p8 = "#";

            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_NAME_THRESHOLD].SettingValue))
                p1 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NO_THRESHOLD].SettingValue))
                p2 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STREET_NAME_THRESHOLD].SettingValue))
                p3 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_CITY_THRESHOLD].SettingValue))
                p4 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_STATE_THRESHOLD].SettingValue))
                p5 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue))
                p7 = "A";
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_ZIPCODE_THRESHOLD].SettingValue))
                p8 = "A";

            string matchGrade = p1 + p2 + p3 + p4 + p5 + "#" + p7 + p8 + "###";
            model.MatchGrade = matchGrade;

        }

        public void LoadAdditionalAcceptanceDeatils(CleanseMatchSettingsModel model)
        {
            //Load AutoAcceptance Criteria.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.AutoAcceptanceCriteria = fac.GetAutoAcceptanceDetails();
            ObservableCollection<AutoAdditionalAcceptanceCriteriaEntity> myCollection = new ObservableCollection<AutoAdditionalAcceptanceCriteriaEntity>(model.AutoAcceptanceCriteria);
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }

        public List<MatchCodeEntity> LoadMatchGradesEntities(string ddlMatchGrade, string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            switch (ddlMatchGrade)
            {
                case "Company":
                    return fac.GetMatchMDPCodes("Company");
                case "StreetName":
                    return fac.GetMatchMDPCodes("StreetName");
                case "StreetNo":
                    return fac.GetMatchMDPCodes("StreetNo");
                case "City":
                    return fac.GetMatchMDPCodes("City");
                case "State":
                    return fac.GetMatchMDPCodes("State"); ;
                case "MailingAddress":
                    return fac.GetMatchMDPCodes("MailingAddress");
                case "Phone":
                    return fac.GetMatchMDPCodes("Phone");


                default:
                    return fac.GetMatchMDPCodes("");
            }
        }
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult RunAutoAcceptanceRule()
        {
            //set Run Auto Acceptance Rule
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.RunAutoAcceptanceRule();
            return new JsonResult { Data = MessageCollection.RunRules };
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteAcceptance(string CriteriaGroupId, int CommentId)
        {
            // Delete Acceptance
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteAcceptance(CriteriaGroupId, Helper.oUser.UserId, CommentId);
            return Json(MessageCollection.CommanDeleteMessage);

        }
        #endregion
        #region "Import Data"
        public ActionResult ExportToExcel(int? ConfidenceCode = null, string MatchGrade = null, int? CountyGroupId = null, string Tags = null)
        {
            // Export data to Excel Sheet .
            string url = Request.Url.Scheme + "://" + Request.Url.Authority;
            string[] hostParts = new System.Uri(url).Host.Split('.');
            string domain = hostParts[0];
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtAutoAcceptRules = new DataTable();
            int totalCount = 0;
            List<AutoAdditionalAcceptanceCriteriaEntity> lstAutoAccpetance = new List<AutoAdditionalAcceptanceCriteriaEntity>();

            lstAutoAccpetance = fac.GetAutoAcceptanceDetailsPagedSorted(11, 1, 100000, out totalCount, ConfidenceCode == null ? 0 : Convert.ToInt32(ConfidenceCode), string.IsNullOrEmpty(MatchGrade) ? null : MatchGrade, CountyGroupId == null ? 0 : Convert.ToInt32(CountyGroupId), Tags);

            dtAutoAcceptRules = CommonMethod.ToDataTable(lstAutoAccpetance);
            //dtAutoAcceptRules = fac.GetAutoAcceptanceDetailsExportToExcel(Helper.LicenseEnableTags, Convert.ToInt32(ConfidenceCode), MatchGrade, CountyGroupId == null ? 0 : Convert.ToInt32(CountyGroupId), Tags);
            dtAutoAcceptRules.Columns.Remove("Error");
            dtAutoAcceptRules.Columns.Remove("lstAutoAcceptanceCriteriaDetail");
            dtAutoAcceptRules.Columns.Remove("item");
            dtAutoAcceptRules.Columns.Remove("CriteriaGroupId");
            dtAutoAcceptRules.Columns.Remove("groupId");
            dtAutoAcceptRules.Columns.Remove("UserId");
            dtAutoAcceptRules.Columns.Remove("UserName");
            dtAutoAcceptRules.Columns.Remove("IsValidSave");
            dtAutoAcceptRules.Columns.Remove("GroupId");
            dtAutoAcceptRules.Columns.Remove("MatchGrade");
            dtAutoAcceptRules.Columns.Remove("MDPCode");
            if (dtAutoAcceptRules.Columns.Contains("CountryGroupId"))
                dtAutoAcceptRules.Columns.Remove("CountryGroupId");
            string fileName = domain + "_AutoAcceptRules_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            // Make Excel sheet and download file 
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Auto Accept Rules");
                worksheet.Cells.LoadFromDataTable(dtAutoAcceptRules, true);

                package.Workbook.Properties.Title = "Auto Accept Rules";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        public ActionResult ImportData()
        {
            return View();
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ImportData(HttpPostedFileBase file, bool header)
        {
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx,", file.FileName))
            {
                if (file.ContentLength > 0)
                {
                    DataTable dt = new DataTable();
                    string path = string.Empty;
                    try
                    {
                        string directory = Server.MapPath("~/Content/UploadDataFile");
                        if (!Directory.Exists(directory))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(directory);
                        }
                        //change file name with current date & time
                        string[] strfileName = file.FileName.Split('.');
                        string extension = strfileName[strfileName.Count() - 1];
                        string fileName = System.DateTime.Now.Ticks + "." + extension;
                        TempData["fileName"] = fileName;

                        path = Path.Combine(directory, Path.GetFileName(fileName));
                        file.SaveAs(path);
                        //convert Excel file to data table
                        dt = DataController.ExcelToDataTable(path, header);
                        TempData["Data"] = dt;
                        System.IO.File.Delete(path);
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
                    return new JsonResult { Data = MessageCollection.CommanFileEmpty };
                }
            }
            else
            {
                return new JsonResult { Data = MessageCollection.CommanChechExcelFile };
            }
            TempData.Keep();
            return new JsonResult { Data = "success" };
        }
        [HttpGet]
        public ActionResult CleanseMatchDataMatch()
        {
            DataTable dt = new DataTable();
            if (TempData["Data"] != null)
            {
                dt = (TempData["Data"] as DataTable).Copy();
            }
            bool IsTag = false;
            //Get Import File Column Name to fill in dropdown
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            if (dt.Rows.Count > 0)
            {
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
                }
            }
            // Get InpCompany Data Column Names
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtSecondaryAutoAcceptanceCriteria = new DataTable();
            //Get Secondary Auto-Acceptance Criteria Columns Name
            dtSecondaryAutoAcceptanceCriteria = sfac.GetSecondaryAutoAcceptanceCriteriaColumnsName();
            List<string> columnName = new List<string>();
            if (dtSecondaryAutoAcceptanceCriteria.Rows.Count > 0)
            {
                for (int k = 0; k < dtSecondaryAutoAcceptanceCriteria.Rows.Count; k++)  //loop through the columns. 
                {
                    if (Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "ImportRowId" && Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "ImportProcessId" && Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]) != "CountryGroupId")
                        columnName.Add(Convert.ToString(dtSecondaryAutoAcceptanceCriteria.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;
            ViewBag.IsContainsTags = IsTag;
            TempData["IsTag"] = IsTag;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            TempData.Keep();
            return View(pagedProducts);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CleanseMatchDataMatch(string OrgColumnName, string ExcelColumnName, string Tags = null, bool IsOverWrite = false, int? CommentId = null)
        {
            bool IsTag = false;
            if (TempData["IsTag"] != null)
            {
                IsTag = Convert.ToBoolean(TempData["IsTag"]);
            }
            string[] OrgColumnNameArray = OrgColumnName.Split(',');
            string[] ExcelColumnNameArray = ExcelColumnName.Split(',');

            string Message = string.Empty;
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();
            if (TempData["Data"] != null)
            {
                dt = (TempData["Data"] as DataTable).Copy();
            }
            //Get Secondary Auto-Acceptance Criteria Columns Name
            dtOrgColumns = sfac.GetSecondaryAutoAcceptanceCriteriaColumnsName();
            if (!IsTag)
            {
                DataColumn Col = dt.Columns.Add("Tags", typeof(System.String));
                foreach (DataRow d in dt.Rows)
                {
                    if (!string.IsNullOrEmpty(Tags))
                    {
                        d["Tags"] = Convert.ToString(Tags);
                    }
                }
            }
            if (!Helper.LicenseEnableTags)
            {
                // if the tag is disable and file contain tags field in table. We just remove tags field from data table 
                var list = new List<string>(OrgColumnNameArray);
                string tag = list.Where(x => x.ToUpperInvariant() == "Tags".ToUpperInvariant()).Select(x => x.ToUpperInvariant()).FirstOrDefault();
                if (!string.IsNullOrEmpty(tag))
                {
                    list.Remove("Tags");
                    OrgColumnNameArray = list.ToArray();
                }
            }
            dtColumns.Columns.Add("Tablecolumn");
            dtColumns.Columns.Add("Excelcolumn");
            DataRow dr = dtColumns.NewRow();
            // Merge Original and Excel column in single table and Manage Dynamic Column matching 
            for (int i = 0; i < OrgColumnNameArray.Length; i++)
            {
                if (Convert.ToString(OrgColumnNameArray[i]) != "-Select-")
                {
                    if (i == OrgColumnNameArray.Length - 1)
                    {
                        dr = dtColumns.NewRow();
                        dr["Tablecolumn"] = "CountryGroupId";
                        dr["Excelcolumn"] = "CountryGroupId";
                        dtColumns.Rows.Add(dr);
                    }
                    dr = dtColumns.NewRow();
                    dr["Tablecolumn"] = Convert.ToString(ExcelColumnNameArray[i]);
                    dr["Excelcolumn"] = Convert.ToString(OrgColumnNameArray[i]);
                    dtColumns.Rows.Add(dr);
                }
            }
            TempData.Keep();
            try
            {
                //remove the blank rows in data table
                string strRowFilter = string.Empty;
                for (int m = 0; m < dtOrgColumns.Rows.Count; m++)
                {
                    if (Convert.ToString(dtOrgColumns.Rows[m][1]).ToLower() == "no")
                    {
                        for (int tt = 0; tt < dtColumns.Rows.Count; tt++)
                        {
                            if (Convert.ToString(dtColumns.Rows[tt][0]) == Convert.ToString(dtOrgColumns.Rows[m][0]))
                            {
                                if (Convert.ToString(dtOrgColumns.Rows[m][0]) != "CountryGroupId")
                                {
                                    strRowFilter += "([" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> '' OR [" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> 'NULL') AND";
                                }
                            }
                        }
                    }
                }
                strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3);
                DataTable dtTempDataNew = new DataTable();
                DataColumn Col = dt.Columns.Add("CountryGroupId", typeof(System.Int32));
                foreach (DataRow d in dt.Rows)
                {
                    d["CountryGroupId"] = 0;
                }
                dtTempDataNew = dt.Select(strRowFilter).CopyToDataTable();
                dt = dtTempDataNew;

                //bulk insert new records
                bool IsDataInsert = BulkInsert(dt, dtColumns, IsOverWrite, CommentId);
                Message = Convert.ToString(TempData["BulkMessage"]);

            }
            catch (Exception ex)
            {
                Message = MessageCollection.CommanEnableFileImport;
            }
            TempData.Keep();
            return new JsonResult { Data = Message };
        }

        // Get Current Column value for display in Example Field.
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult UpdateExamples(string CurrentColumn)
        {
            string strValue = string.Empty;
            DataTable dt = new DataTable();
            if (TempData["Data"] != null)
            {
                dt = (TempData["Data"] as DataTable).AsEnumerable().Take(1).CopyToDataTable();
            }
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(CurrentColumn) > 0)
                    {
                        strValue = Convert.ToString(dt.Rows[0][Convert.ToInt32(CurrentColumn) - 1]);
                    }
                }
            }
            TempData.Keep();
            return new JsonResult { Data = strValue };
        }

        //bulk insert for Secondary Auto-Acceptance Criteria
        public bool BulkInsert(DataTable dt, DataTable dtColumns, bool IsOverWrite = false, int? CommentId = null)
        {
            bool DataInsert = false;
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
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
                    bulkCopy.DestinationTableName = "ext.SecondaryAutoAcceptanceCriteriaGroup";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string Message = sfac.MergeSecondaryAutoAcceptCriteria(IsOverWrite, Helper.oUser.UserId, Convert.ToInt32(CommentId));
                        TempData["BulkMessage"] = "Data Inserted Successfully.";
                        if (!string.IsNullOrEmpty(Message))
                        {
                            TempData["BulkMessage"] = Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        TempData["BulkMessage"] = ex.Message.ToString();
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }
        #endregion

        //Get Auto-Acceptance Match Grade and fill dropdown list
        public static List<string> GetAutoAcceptanceMatchGrade(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            DataTable dt = fac.GetAutoAcceptanceMatchGrade();
            List<string> lstMatchGrade = new List<string>();

            foreach (DataRow d in dt.Rows)
            {
                lstMatchGrade.Add(Convert.ToString(d["MatchGrade"]));
            }
            return lstMatchGrade;
        }
        [HttpGet]
        public ActionResult DeleteComment(string CriteriaGroupId, string ToCall, string OrgColumnName = null, string ExcelColumnName = null, string Tags = null, bool IsOverWrite = false)
        {
            // Open Delete Comment popup for save the information for comment
            TempData["OrgColumnName"] = !string.IsNullOrEmpty(OrgColumnName) ? OrgColumnName : null;
            TempData["ExcelColumnName"] = !string.IsNullOrEmpty(ExcelColumnName) ? ExcelColumnName : null;
            TempData["Tags"] = Tags;
            TempData["IsOverWrite"] = IsOverWrite;
            ViewBag.CriteriaGroupId = CriteriaGroupId;
            ViewBag.ToCall = ToCall;
            TempData.Keep();
            return View();
        }

        //get  User Comment and fill dropdown list
        public List<UserCommentsEntity> LoadUserComment(string ConnectionString)
        {
            // Load User Comment
            UserCommentsFacade fac = new UserCommentsFacade(ConnectionString);
            return fac.GetUserCommentsByType("AUTOACCEPT_DELETE");
        }

        public ActionResult GetAutoAcceptanceCriteriaDetailByGroupId(int CriteriaGroupId)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<AutoAcceptanceCriteriaDetail> lstAutoAcceptanceCriteriaDetail = new List<AutoAcceptanceCriteriaDetail>();
            lstAutoAcceptanceCriteriaDetail = fac.GetAutoAcceptanceCriteriaDetailByGroupId(CriteriaGroupId);
            return PartialView("_AutoAcceptanceDetailView", lstAutoAcceptanceCriteriaDetail);
            //return View("_AutoAcceptanceDetailView", lstAutoAcceptanceCriteriaDetail);
        }
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetSecondaryAutoAcceptanceCriteriaGroupCount()
        {
            //set Run Auto Acceptance Rule
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            int count = Convert.ToInt32(fac.GetSecondaryAutoAcceptanceCriteriaGroupCount());
            return new JsonResult { Data = count };
        }
    }
}