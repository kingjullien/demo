using Newtonsoft.Json;
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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [AllowLicense]
    public class ResearchInvestigationController : BaseController
    {
        #region Mini iResearch Investigation
        // GET: ResearchInvestigation
        public ActionResult Index()
        {
            SessionHelper.InvestigationRecordList = null;
            SessionHelper.InvestigationStats = null;
            return View();
        }

        [HttpGet]
        public ActionResult iResearchInvestigationRecords(string Parameters)
        {
            string InputId = ""; string SrcId = ""; string Company = ""; string StreetName = ""; string Address = ""; string City = ""; string PostalCode = ""; string CountryCode = ""; string Tags = ""; string Phone = ""; string Website = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 2) != "")
                {
                    SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) + ":" + Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 2);
                }
                Company = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                StreetName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Address = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                City = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                PostalCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                CountryCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1);
                Phone = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1);
                Website = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 10, 1);
            }
            iResearchEntity objiResearchInvestigation = new iResearchEntity();
            objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
            objiResearchInvestigation.CustomerRequestorEmail = Helper.oUser.EmailAddress;
            objiResearchInvestigation.InputId = !string.IsNullOrEmpty(InputId) ? InputId : "";
            objiResearchInvestigation.SrcRecordId = !string.IsNullOrEmpty(SrcId) ? SrcId : "";
            objiResearchInvestigation.BusinessName = !string.IsNullOrEmpty(Company) ? Company : "";
            objiResearchInvestigation.StreetAddress = !string.IsNullOrEmpty(Address) ? Address : "";
            objiResearchInvestigation.AddressLocality = !string.IsNullOrEmpty(City) ? City : "";
            objiResearchInvestigation.AddressRegion = !string.IsNullOrEmpty(StreetName) ? StreetName : "";
            objiResearchInvestigation.PostalCode = !string.IsNullOrEmpty(PostalCode) ? PostalCode : "";
            objiResearchInvestigation.CountryISOAlpha2Code = !string.IsNullOrEmpty(CountryCode) ? CountryCode : "";
            objiResearchInvestigation.Tags = !string.IsNullOrEmpty(Tags) ? Tags : "";
            objiResearchInvestigation.Phone = !string.IsNullOrEmpty(Phone) ? Phone : "";
            objiResearchInvestigation.Website = !string.IsNullOrEmpty(Website) ? Website : "";
            objiResearchInvestigation.ResearchRequestType = ResearchRequestType.Mini.ToString();

            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            IResearchInvestigationEntity IsInvestigated = fac.iResearchInvestigationIsExists(InputId, SrcId, "");
            ViewBag.IsInvestigated = IsInvestigated;
            return View("iResearchInvestigation", objiResearchInvestigation);
        }
        //Save Research Investigation Record
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult iResearchInvestigation(iResearchEntity objiResearchInvestigation)
        {
            string resMessage = string.Empty;
            bool result = false;
            bool validEmail = CommonMethod.isValidEmail(objiResearchInvestigation.CustomerRequestorEmail);
            if (ModelState.IsValid && validEmail)
            {
                iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
                ResearchInvestigationResponseEntity modelSubmit = new ResearchInvestigationResponseEntity();

                Utility.Utility api = new Utility.Utility();
                objiResearchInvestigation.roleType = 19117;
                objiResearchInvestigation.typeDnBCode = 33588;
                modelSubmit = api.iResearchInvestigate(objiResearchInvestigation);
                string Message = string.Empty;

                if (modelSubmit.researchRequestID > 0)
                {
                    objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
                    objiResearchInvestigation.ResearchRequestId = modelSubmit.researchRequestID;
                    objiResearchInvestigation.ResearchRequestType = objiResearchInvestigation.ResearchRequestType;
                    objiResearchInvestigation.InputId = objiResearchInvestigation.InputId;
                    objiResearchInvestigation.SrcRecordId = objiResearchInvestigation.SrcRecordId;
                    objiResearchInvestigation.Tags = objiResearchInvestigation.Tags;
                    objiResearchInvestigation.RequestResponseJSON = modelSubmit.responseJSON;
                    objiResearchInvestigation.RequestBody = modelSubmit.requestJSON;
                    Message = fac.InsertResearchInvestigation(objiResearchInvestigation);
                    if (Message == "")
                    {
                        result = true;
                        resMessage = iResearchInvestigationLang.msgInsertInvestigation;
                    }
                    else
                    {
                        result = false;
                        resMessage = iResearchInvestigationLang.msgInsertFailInvestigation;
                    }
                }
                else
                {
                    result = false;
                    StringBuilder sb = new StringBuilder();
                    ResearchInvestigationResponseEntity data = new ResearchInvestigationResponseEntity();
                    data = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(modelSubmit.responseJSON);
                    if (data.error != null)
                    {
                        if (data.error.errorDetails != null)
                        {
                            foreach (var err in data.error.errorDetails)
                            {
                                sb.AppendLine(err.parameter + " : " + err.description);
                            }

                        }
                        else
                        {
                            sb.AppendLine(data.error.errorMessage);
                        }

                    }
                    objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
                    objiResearchInvestigation.RequestResponseJSON = modelSubmit.responseJSON;
                    objiResearchInvestigation.RequestBody = modelSubmit.requestJSON;
                    fac.InsertiResearchInvestigationFailedCalls(objiResearchInvestigation);
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        resMessage = sb.ToString();
                    }
                    else
                    {
                        resMessage = iResearchInvestigationLang.msgInsertFailInvestigation;
                    }
                }
            }
            else
            {
                if (!validEmail)
                {
                    result = false;
                    resMessage = CommonMessagesLang.msgInvalidEmail;
                }
            }
            return Json(new { result = result, message = resMessage });
        }
        #endregion



        #region Targeted iResearch Investigation
        [HttpGet, RequestFromSameDomain]
        public ActionResult iResearchInvestigationRecordsTargeted(string Parameters)
        {
            string InputId = ""; string SrcId = ""; string Duns = ""; string Company = ""; string StreetName = ""; string City = ""; string PostalCode = ""; string CountryCode = ""; string Tags = "";
            string TradeStyle = ""; string Status = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                InputId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                if (Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 2) != "")
                {
                    SrcId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) + ":" + Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 2);
                }
                Duns = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Company = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                StreetName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                City = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                PostalCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
                CountryCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1);
                TradeStyle = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1);
                Status = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 10, 1);
            }
            iResearchEntityTargetedEntity objiResearchInvestigation = new iResearchEntityTargetedEntity();
            objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
            objiResearchInvestigation.CustomerRequestorEmail = Helper.oUser.EmailAddress;
            objiResearchInvestigation.InputId = !string.IsNullOrEmpty(InputId) ? InputId : "";
            objiResearchInvestigation.SrcRecordId = !string.IsNullOrEmpty(SrcId) ? SrcId.Trim() : "";
            objiResearchInvestigation.Duns = !string.IsNullOrEmpty(Duns) ? Duns : "";
            objiResearchInvestigation.Tags = !string.IsNullOrEmpty(Tags) ? Tags : "";
            objiResearchInvestigation.ResearchRequestType = ResearchRequestType.Targeted.ToString();
            objiResearchInvestigation.BusinessName = Company;
            objiResearchInvestigation.StreetAddress = StreetName;
            objiResearchInvestigation.AddressLocality = City;
            objiResearchInvestigation.PostalCode = PostalCode;
            objiResearchInvestigation.CountryISOAlpha2Code = CountryCode;
            objiResearchInvestigation.TradeStyle = TradeStyle;
            objiResearchInvestigation.Status = !string.IsNullOrEmpty(Status) && Status.ToLower() == "active";

            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            IResearchInvestigationEntity IsInvestigated = fac.iResearchInvestigationIsExists(InputId, SrcId, Duns);
            ViewBag.IsInvestigated = IsInvestigated;
            ViewBag.Country = CountryCode;
            return View("iResearchInvestigationRecordsTargeted", objiResearchInvestigation);
        }
        //Save Research Investigation Record
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult iResearchInvestigationRecordsTargeted(iResearchEntityTargetedEntity objiResearchInvestigation)
        {
            bool result = false;
            string resMessage = string.Empty;
            bool validEmail = CommonMethod.isValidEmail(objiResearchInvestigation.CustomerRequestorEmail);
            ViewBag.Country = objiResearchInvestigation.CountryISOAlpha2Code;
            if (ModelState.IsValid && validEmail)
            {
                iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
                ResearchInvestigationTargetedResponseEntity modelSubmit = new ResearchInvestigationTargetedResponseEntity();
                objiResearchInvestigation.roleType = 19117;
                Utility.Utility api = new Utility.Utility();
                modelSubmit = api.iResearchInvestigateTargeted(objiResearchInvestigation);
                string Message = string.Empty;

                if (modelSubmit.researchRequestID > 0)
                {
                    objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
                    objiResearchInvestigation.ResearchRequestId = modelSubmit.researchRequestID;
                    objiResearchInvestigation.ResearchRequestType = objiResearchInvestigation.ResearchRequestType;
                    objiResearchInvestigation.InputId = !string.IsNullOrEmpty(objiResearchInvestigation.InputId) ? objiResearchInvestigation.InputId : "0";
                    objiResearchInvestigation.SrcRecordId = string.IsNullOrEmpty(objiResearchInvestigation.SrcRecordId) ? "" : objiResearchInvestigation.SrcRecordId.Trim();
                    objiResearchInvestigation.Tags = objiResearchInvestigation.Tags;
                    objiResearchInvestigation.RequestResponseJSON = modelSubmit.responseJSON;
                    objiResearchInvestigation.RequestBody = modelSubmit.requestJSON;
                    Message = fac.InsertResearchInvestigation(objiResearchInvestigation);
                    if (Message == "")
                    {
                        result = true;
                        resMessage = iResearchInvestigationLang.msgInsertInvestigation;
                    }
                    else
                    {
                        result = false;
                        resMessage = iResearchInvestigationLang.msgInsertFailInvestigation;
                    }
                }
                else
                {
                    result = false;
                    StringBuilder sb = new StringBuilder();
                    ResearchInvestigationResponseEntity data = new ResearchInvestigationResponseEntity();
                    data = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(modelSubmit.responseJSON);
                    if (data.error != null)
                    {
                        if (data.error.errorDetails != null)
                        {
                            foreach (var err in data.error.errorDetails)
                            {
                                sb.AppendLine(err.parameter + " : " + err.description);
                            }

                        }
                        else
                        {
                            sb.AppendLine(data.error.errorMessage);
                        }

                    }
                    objiResearchInvestigation.UserId = Helper.oUser.UserId.ToString();
                    objiResearchInvestigation.RequestResponseJSON = modelSubmit.responseJSON;
                    objiResearchInvestigation.RequestBody = modelSubmit.requestJSON;
                    fac.InsertiResearchInvestigationFailedCalls(objiResearchInvestigation);
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        resMessage = sb.ToString();
                    }
                    else
                    {
                        resMessage = iResearchInvestigationLang.msgInsertFailInvestigation;
                    }
                }
            }
            else
            {
                if (!validEmail)
                {
                    result = false;
                    resMessage = CommonMessagesLang.msgInvalidEmail;
                }
            }
            return Json(new { result = result, message = resMessage });
        }
        #endregion



        #region "Investigation Status"
        public ActionResult GetInvestigationStatus(string parameters)
        {
            int researchRequestId = 0;
            try
            {
                if (!string.IsNullOrEmpty(parameters))
                {

                    researchRequestId = Convert.ToInt32(StringCipher.Decrypt(parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));

                    Utility.Utility api = new Utility.Utility();
                    string result = api.GetResearchInvestigationStatus(researchRequestId);
                    if (!string.IsNullOrEmpty(result))
                    {
                        StringBuilder sb = new StringBuilder();
                        ResearchInvestigationResponseEntity data = new ResearchInvestigationResponseEntity();
                        data = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(result);
                        if (data.error != null)
                        {
                            if (data.error.errorDetails != null)
                            {
                                sb.AppendLine(data.error.errorMessage);
                                foreach (var err in data.error.errorDetails)
                                {
                                    sb.AppendLine(err.parameter + " : " + err.description);
                                }
                            }
                            else
                            {
                                sb.AppendLine(data.error.errorMessage);
                            }
                            return Json(new { result = false, message = sb.ToString() }, JsonRequestBehavior.AllowGet);
                        }
                        iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
                        bool resultStatus = fac.iResearchUpdateRequestStatus(researchRequestId, result);
                    }
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetInvestigationStatusForAll()
        {
            List<IResearchInvestigationEntity> lst = new List<IResearchInvestigationEntity>();
            try
            {
                lst = JsonConvert.DeserializeObject<List<IResearchInvestigationEntity>>(SessionHelper.InvestigationRecordList);
                lst = lst.Where(x => !string.IsNullOrEmpty(x.CaseStatus) && x.CaseStatus.ToLower() != "closed" && x.CaseStatus.ToLower() != "failed").ToList();
                Utility.Utility api = new Utility.Utility();
                string token = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
                Task.Run(() => api.SubmitMultipleGetResearchInvestigationStatus(lst, this.CurrentClient.ApplicationDBConnectionString, token));
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgSomethingWrong }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region "Filter"
        public ActionResult FilterIResearchInvestigation(string Parameters)
        {
            string SrcRecordId = string.Empty, Status = string.Empty, RequestStartDateTime = string.Empty, RequestendDateTime = string.Empty, Keyword = string.Empty;

            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                SrcRecordId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Status = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                RequestStartDateTime = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                RequestendDateTime = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                Keyword = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
            }
            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<IResearchInvestigationEntity> lst = fac.GetFilterIResearchInvestigation(SrcRecordId, Status, RequestStartDateTime, RequestendDateTime, Keyword);
            return PartialView("_index", lst);
        }

        public ActionResult InvestigationDetails(IResearchInvestigationEntity investigationEntity)
        {
            return View(investigationEntity);
        }
        #endregion

        #region "Multiple MINI Requests"
        public ActionResult iResearchInvestigationRecordsMultiple(List<iResearchEntity> iResearches)
        {
            iResearches.ForEach(x =>
            {
                x.ResearchRequestType = ResearchRequestType.Mini.ToString();
                x.UserId = Helper.oUser.UserId.ToString();
                x.CustomerRequestorEmail = Helper.oUser.EmailAddress;
                x.BusinessName = string.IsNullOrEmpty(x.BusinessName) ? x.BusinessName : x.BusinessName.Trim();
                x.SrcRecordId = string.IsNullOrEmpty(x.SrcRecordId) ? x.SrcRecordId : x.SrcRecordId.Trim();
                x.HasError = (string.IsNullOrEmpty(x.BusinessName) || string.IsNullOrEmpty(x.SrcRecordId) || string.IsNullOrEmpty(x.BusinessName) || string.IsNullOrEmpty(x.StreetAddress) || string.IsNullOrEmpty(x.AddressLocality) || string.IsNullOrEmpty(x.AddressRegion) || string.IsNullOrEmpty(x.PostalCode) || string.IsNullOrEmpty(x.CountryISOAlpha2Code));
            });
            return View("_MiniRequestRecordList", iResearches);
        }

        [HttpPost]
        public ActionResult SubmitMultipleMiniRequests(List<iResearchEntity> iResearches)
        {
            Utility.Utility api = new Utility.Utility();
            string token = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
            Task.Run(() => api.SubmitMultipleMiniRequest(iResearches, this.CurrentClient.ApplicationDBConnectionString, token));
            return Json(new { message = CommonMessagesLang.msgMultipleInvestigationSubmit });
        }
        #endregion

        #region "MINI Investigation using File upload"
        public ActionResult FileUploadIndex()
        {
            Session["InvestigationFileData"] = null;
            return PartialView();
        }

        [HttpPost]
        public ActionResult FileUploadIndex(HttpPostedFileBase file, string Type)
        {
            ViewBag.ReSearchType = Type;
            if (file != null && CommonMethod.CheckFileType(".xls,.xlsx", file.FileName.ToLower()))
            {
                if (file.ContentLength > 0)
                {
                    InvestigationColumnMappingViewModel viewModel = new InvestigationColumnMappingViewModel();
                    viewModel.columns = new List<string>();
                    viewModel.fileColumns = new List<SelectListItem>();
                    DataTable dt = new DataTable();
                    try
                    {
                        dt = CommonMethod.ExcelToDataTable(file);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("already belongs to this DataTable"))
                        {
                            return new JsonResult { Data = ex.Message.Replace("DataTable", "file") };
                        }
                        else
                        {
                            return new JsonResult { Data = "Error:" + ex.Message };
                        }
                    }
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

                    Session["InvestigationFileData"] = dt;
                    string fields = "SourceRecordId,CompanyName,StreetAddress,City,State,PostalCode,Country";
                    if (Type == ResearchRequestType.Targeted.ToString())
                        fields = "DUNS Number,Research Subtype,CompanyName,StreetAddress,City,State,PostalCode,Country,TradeStyle,Customer Transaction Id,Customer Reference,IsActiveBusiness,Research Comment(Financial Figures),Research Comment(Financial Ratios),DUNS1,DUNS2,DUNS3,DUNS4,DUNS5";
                    viewModel.columns = fields.Split(',').ToList();
                    if (dt.Rows.Count > 0)
                    {
                        viewModel.fileColumns.Add(new SelectListItem { Value = "0", Text = "-Select-" });
                        int i = 0;
                        foreach (DataColumn c in dt.Columns)
                        {
                            viewModel.fileColumns.Add(new SelectListItem { Value = (i + 1).ToString(), Text = Convert.ToString(c.ColumnName) });
                            i++;
                        }
                    }
                    return PartialView("InvestigationColumnMapping", viewModel);
                }
                else
                {
                    return new JsonResult { Data = ImportDataLang.msgUnableBlankFileImport };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgAllowedOnlyExcel };
            }
        }

        [HttpPost]
        public ActionResult SubmitColumnMapping(string[] OrgColumnName, string[] ExcelColumnName, string Email, string Comment)
        {
            List<iResearchEntity> lstIResearches = new List<iResearchEntity>();
            DataTable dt = new DataTable();
            if (Session["InvestigationFileData"] != null)
            {
                dt = Session["InvestigationFileData"] as DataTable;
            }
            if (dt != null && OrgColumnName.Length > 5)
            {

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    iResearchEntity entity = new iResearchEntity();
                    entity.ResearchRequestType = ResearchRequestType.Mini.ToString();
                    entity.SrcRecordId = dt.Rows[j][OrgColumnName[0]]?.ToString();
                    entity.BusinessName = dt.Rows[j][OrgColumnName[1]]?.ToString();
                    entity.StreetAddress = dt.Rows[j][OrgColumnName[2]]?.ToString();
                    entity.AddressLocality = dt.Rows[j][OrgColumnName[3]]?.ToString();
                    entity.AddressRegion = OrgColumnName[4] != "-Select-" ? dt.Rows[j][OrgColumnName[4]]?.ToString() : "";
                    entity.PostalCode = dt.Rows[j][OrgColumnName[5]]?.ToString();
                    entity.CountryISOAlpha2Code = dt.Rows[j][OrgColumnName[6]]?.ToString();
                    entity.CustomerRequestorEmail = !string.IsNullOrEmpty(Email) ? Email.Trim() : string.Empty;
                    entity.ResearchComments = Comment;
                    entity.UserId = Helper.oUser.UserId.ToString();
                    lstIResearches.Add(entity);
                }
            }
            if (lstIResearches.Any())
            {
                Utility.Utility api = new Utility.Utility();
                string token = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
                Task.Run(() => api.SubmitMultipleMiniRequest(lstIResearches, this.CurrentClient.ApplicationDBConnectionString, token));
                return new JsonResult { Data = CommonMessagesLang.msgRequestSubmitted };
            }
            return new JsonResult { Data = CommonMessagesLang.msgCommanErrorMessage };
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DownloadSampleFile(string Type)
        {
            string FilePath = string.Empty;
            if (Type == ResearchRequestType.Mini.ToString())
            {
                FilePath = Server.MapPath("~/DownloadApps/DownloadInvestigationSampleFile/SampleFileResearchInvestigation.xlsx");
            }
            else
            {
                FilePath = Server.MapPath("~/DownloadApps/DownloadInvestigationSampleFile/SampleFileTargetedResearchInvestigation.xlsx");
            }

            if (!System.IO.File.Exists(FilePath))
            {
                return null;
            }
            return File(FilePath, "application/vnd.ms-excel", Path.GetFileName(FilePath));
        }

        public ActionResult DownloadReSearchSubType()
        {
            string FilePath = string.Empty;
            FilePath = Server.MapPath("~/DownloadApps/DownloadInvestigationSampleFile/ResearchSubType Information.xlsx");
            if (!System.IO.File.Exists(FilePath))
            {
                return null;
            }
            return File(FilePath, "application/vnd.ms-excel", Path.GetFileName(FilePath));
        }


        public ActionResult UpdateExamples(string CurrentColumn)
        {
            string strValue = string.Empty;
            DataTable dt = new DataTable();
            if (Session["InvestigationFileData"] != null)
            {
                dt = Session["InvestigationFileData"] as DataTable;
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

        #endregion

        #region "Targeted Investigation using File upload"
        public ActionResult SubmitColumnMappingTargeted(string[] OrgColumnName, string[] ExcelColumnName, string Email)
        {
            List<iResearchEntityTargetedEntity> lstIResearches = new List<iResearchEntityTargetedEntity>();
            DataTable dt = new DataTable();
            if (Session["InvestigationFileData"] != null)
            {
                dt = Session["InvestigationFileData"] as DataTable;
            }
            if (dt != null && OrgColumnName.Length > 17)
            {

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    try
                    {
                        iResearchEntityTargetedEntity entity = new iResearchEntityTargetedEntity();
                        entity.ResearchRequestType = ResearchRequestType.Targeted.ToString();
                        entity.UserId = Helper.oUser.UserId.ToString();
                        entity.Duns = OrgColumnName[0] != "-Select-" ? dt.Rows[j][OrgColumnName[0]]?.ToString() : "";
                        entity.ResearchSubTypes = OrgColumnName[1] != "-Select-" ? dt.Rows[j][OrgColumnName[1]]?.ToString() : "";
                        entity.BusinessName = OrgColumnName[2] != "-Select-" ? dt.Rows[j][OrgColumnName[2]]?.ToString() : "";
                        entity.StreetAddress = OrgColumnName[3] != "-Select-" ? dt.Rows[j][OrgColumnName[3]]?.ToString() : "";
                        entity.AddressLocality = OrgColumnName[4] != "-Select-" ? dt.Rows[j][OrgColumnName[4]]?.ToString() : "";
                        entity.AddressRegion = OrgColumnName[5] != "-Select-" ? dt.Rows[j][OrgColumnName[5]]?.ToString() : "";
                        entity.PostalCode = OrgColumnName[6] != "-Select-" ? dt.Rows[j][OrgColumnName[6]]?.ToString() : "";
                        entity.CountryISOAlpha2Code = OrgColumnName[7] != "-Select-" ? dt.Rows[j][OrgColumnName[7]]?.ToString() : "";
                        entity.TradeStyle = OrgColumnName[8] != "-Select-" ? dt.Rows[j][OrgColumnName[8]]?.ToString() : "";
                        entity.SrcRecordId = OrgColumnName[9] != "-Select-" ? dt.Rows[j][OrgColumnName[9]]?.ToString() : "";
                        entity.CustomerTransactionReference = OrgColumnName[10] != "-Select-" ? dt.Rows[j][OrgColumnName[10]]?.ToString() : "";
                        entity.Status = OrgColumnName[11] != "-Select-" ? (!string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[11]]?.ToString()) ? Convert.ToBoolean(dt.Rows[j][OrgColumnName[11]]) : false) : false;
                        entity.ResearchComments1 = OrgColumnName[12] != "-Select-" ? dt.Rows[j][OrgColumnName[12]]?.ToString() : "";
                        entity.ResearchComments2 = OrgColumnName[13] != "-Select-" ? dt.Rows[j][OrgColumnName[13]]?.ToString() : "";

                        List<string> duns = new List<string>();
                        if (OrgColumnName[14] != "-Select-" && dt.Rows[j][OrgColumnName[14]] != null && !string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[14]].ToString()))
                            duns.Add(dt.Rows[j][OrgColumnName[14]].ToString());
                        if (OrgColumnName[15] != "-Select-" && dt.Rows[j][OrgColumnName[15]] != null && !string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[15]].ToString()))
                            duns.Add(dt.Rows[j][OrgColumnName[15]].ToString());
                        if (OrgColumnName[16] != "-Select-" && dt.Rows[j][OrgColumnName[16]] != null && !string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[16]].ToString()))
                            duns.Add(dt.Rows[j][OrgColumnName[16]].ToString());
                        if (OrgColumnName[17] != "-Select-" && dt.Rows[j][OrgColumnName[17]] != null && !string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[17]].ToString()))
                            duns.Add(dt.Rows[j][OrgColumnName[17]].ToString());
                        if (OrgColumnName[18] != "-Select-" && dt.Rows[j][OrgColumnName[18]] != null && !string.IsNullOrEmpty(dt.Rows[j][OrgColumnName[18]].ToString()))
                            duns.Add(dt.Rows[j][OrgColumnName[18]].ToString());

                        entity.DuplicateDuns = string.Join(",", duns);
                        entity.CustomerRequestorEmail = !string.IsNullOrEmpty(Email) ? Email.Trim() : string.Empty;

                        lstIResearches.Add(entity);
                    }
                    catch (Exception)
                    {
                        //Empty catch block to stop from breaking
                    }
                }
                if (lstIResearches.Any())
                {
                    Utility.Utility api = new Utility.Utility();
                    string token = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_INVESTIGATIONS.ToString(), ThirdPartyProperties.AuthToken.ToString());
                    Task.Run(() => api.SubmitMultipleTargetedRequest(lstIResearches, this.CurrentClient.ApplicationDBConnectionString, token));
                    return new JsonResult { Data = CommonMessagesLang.msgRequestSubmitted };
                }
            }
            return new JsonResult { Data = CommonMessagesLang.msgCommanErrorMessage };
        }

        #endregion

        #region "Reasearch investigation filter"
        public JsonResult GetStatusDD()
        {
            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dataTable = fac.GetiResearchInvestigationCaseLookup();
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                lstAllFilter.Add(new DropDownReturn { Value = dataTable.Rows[i]["CaseStatus"].ToString(), Text = dataTable.Rows[i]["CaseStatus"].ToString() });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
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

        public JsonResult GetRequestTypeDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "Mini", Text = "Mini" });
            lstType.Add(new DropDownReturn { Value = "Targeted", Text = "Targeted" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FilterInvestigationRecords(List<FilterData> filters)
        {
            IResearchInvestigationViewModel researchInvestigationView = new IResearchInvestigationViewModel();
            if (string.IsNullOrEmpty(SessionHelper.InvestigationRecordList))
            {
                iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
                researchInvestigationView.lstResearchInvestigation = fac.GetFilterIResearchInvestigation("", "", "", "", "");
                researchInvestigationView.InvestigationStats = fac.GetDashboardV2GetInvestigationStatistics();
                SessionHelper.InvestigationRecordList = JsonConvert.SerializeObject(researchInvestigationView.lstResearchInvestigation);
                SessionHelper.InvestigationStats = JsonConvert.SerializeObject(researchInvestigationView.InvestigationStats);
            }
            else
            {
                researchInvestigationView.lstResearchInvestigation = JsonConvert.DeserializeObject<List<IResearchInvestigationEntity>>(SessionHelper.InvestigationRecordList);
                researchInvestigationView.InvestigationStats = JsonConvert.DeserializeObject<List<DashboardV2GetInvestigationStatistics>>(SessionHelper.InvestigationStats);
            }
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "Status")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => valLst.Contains(x.CaseStatus.ToString())).ToList();
                        }
                        else if (item.FieldName == "SrcRecordId")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.SrcRecordId == item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "Keyword")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestBody.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "RequestType")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => valLst.Contains(x.RequestType.ToString())).ToList();
                            researchInvestigationView.InvestigationStats = researchInvestigationView.InvestigationStats.Where(x => valLst.Contains(x.RequestType.ToString())).ToList();
                        }
                        else if (item.FieldName == "DUNSNumber")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !string.IsNullOrEmpty(x.ResolutionDUNS) && x.ResolutionDUNS == item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestDateTime >= Convert.ToDateTime(startDate) && x.RequestDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay)).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue))).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "notEqualTo")
                    {
                        if (item.FieldName == "Status")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !valLst.Contains(x.CaseStatus.ToString())).ToList();
                        }
                        else if (item.FieldName == "SrcRecordId")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.SrcRecordId != item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "Keyword")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !x.RequestBody.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "RequestType")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !valLst.Contains(x.RequestType.ToString())).ToList();
                            researchInvestigationView.InvestigationStats = researchInvestigationView.InvestigationStats.Where(x => !valLst.Contains(x.RequestType.ToString())).ToList();
                        }
                        else if (item.FieldName == "DUNSNumber")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.ResolutionDUNS != item.FilterValue).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !(x.RequestDateTime >= Convert.ToDateTime(startDate) && x.RequestDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay))).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !(x.RequestDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue)))).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "contains")
                    {
                        if (item.FieldName == "Status")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.CaseStatus.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "SrcRecordId")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !string.IsNullOrEmpty(x.SrcRecordId) && x.SrcRecordId.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "Keyword")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestBody.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "RequestType")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestType.Contains(item.FilterValue)).ToList();
                            researchInvestigationView.InvestigationStats = researchInvestigationView.InvestigationStats.Where(x => x.RequestType.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "DUNSNumber")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !string.IsNullOrEmpty(x.ResolutionDUNS) && x.ResolutionDUNS.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestDateTime >= Convert.ToDateTime(startDate) && x.RequestDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay)).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => x.RequestDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue))).ToList();
                            }
                        }
                    }
                    else if (item.Operator == "notContains")
                    {
                        if (item.FieldName == "Status")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !x.CaseStatus.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "SrcRecordId")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => string.IsNullOrEmpty(x.SrcRecordId) || (!string.IsNullOrEmpty(x.SrcRecordId) && !x.SrcRecordId.Contains(item.FilterValue))).ToList();
                        }
                        else if (item.FieldName == "Keyword")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !x.RequestBody.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "RequestType")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !x.RequestType.Contains(item.FilterValue)).ToList();
                            researchInvestigationView.InvestigationStats = researchInvestigationView.InvestigationStats.Where(x => !x.RequestType.Contains(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "DUNSNumber")
                        {
                            researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => string.IsNullOrEmpty(x.ResolutionDUNS) || (!string.IsNullOrEmpty(x.ResolutionDUNS) && !x.ResolutionDUNS.Contains(item.FilterValue))).ToList();
                        }
                        else if (item.FieldName == "RequestedDate")
                        {
                            if (item.FilterValue.Contains("-"))
                            {
                                string startDate = string.Empty, endDate = string.Empty;
                                string[] sliptedDate = item.FilterValue.Split('-');
                                startDate = sliptedDate[0].Trim();
                                endDate = sliptedDate[1].Trim();
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !(x.RequestDateTime >= Convert.ToDateTime(startDate) && x.RequestDateTime <= Convert.ToDateTime(endDate).Add(DateTime.MaxValue.TimeOfDay))).ToList();
                            }
                            else
                            {
                                item.FilterValue = item.FilterValue.Replace("D", "");
                                researchInvestigationView.lstResearchInvestigation = researchInvestigationView.lstResearchInvestigation.Where(x => !(x.RequestDateTime >= DateTime.Today.Date.AddDays(Convert.ToInt32(item.FilterValue)))).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            return PartialView("_index", researchInvestigationView);
        }


        #endregion

        #region Download CSV
        [HttpGet]
        public ActionResult DownloadCSV()
        {
            IResearchInvestigationViewModel researchInvestigationView = new IResearchInvestigationViewModel();
            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            researchInvestigationView.lstResearchInvestigation = fac.GetFilterIResearchInvestigation("", "", "", "", "");
            List<IResearchInvestigationEntity> dashboardDataQueue = new List<IResearchInvestigationEntity>();
            dashboardDataQueue = researchInvestigationView.lstResearchInvestigation;
            var lstData = dashboardDataQueue;
            string fileName = "iResearchReport" + DateTime.Now.Ticks.ToString() + ".csv";

            StringBuilder sb = new StringBuilder();
            List<string> Columnname = new List<string>();
            Columnname.Add("SrcRecordId");
            Columnname.Add("Request Type");
            Columnname.Add("Research Subtype");
            Columnname.Add("Primary Name");
            Columnname.Add("Primary Address");
            Columnname.Add("DUNS");
            Columnname.Add("IsActive");
            Columnname.Add("TradeStyle Name");
            Columnname.Add("Duplicate Duns");
            Columnname.Add("Research Comments");
            Columnname.Add("Requestor Email");
            Columnname.Add("Submitted Date");
            Columnname.Add("Research RequestID");
            Columnname.Add("Research Case Id");
            Columnname.Add("Case Status");
            Columnname.Add("Case Resolution");
            Columnname.Add("Resolution DUNS");
            Columnname.Add("Resolution Date");
            Columnname.Add("Error Message");

            string columnnames = string.Join(",", Columnname);
            sb.AppendLine(columnnames);

            foreach (var data in lstData)
            {
                try
                {
                    sb.AppendLine(
                        (data.SrcRecordId) + "," +
                        (data.RequestType) + "," +
                        (data.RequestResponseJSONlst.caseDetails == null ? "" : "\"" + data.RequestResponseJSONlst.caseDetails[0].subjectResearchTypes[0].researchSubType.dnbCode + " - " + data.RequestResponseJSONlst.caseDetails[0].subjectResearchTypes[0].researchSubType.description + "\"") + "," +
                        (data.RequestBodylst != null && data.RequestBodylst.organization != null && !string.IsNullOrEmpty(data.RequestBodylst.organization.primaryName) ? data.RequestBodylst.organization.primaryName : "") + "," +
                        (data.RequestBodylst.organization.primaryAddress != null ? ((data.RequestBodylst.organization.primaryAddress.streetAddress != null ? data.RequestBodylst.organization.primaryAddress.streetAddress.line1 : "") + " " + (data.RequestBodylst.organization.primaryAddress.addressLocality != null ? data.RequestBodylst.organization.primaryAddress.addressLocality.name : "") + " " + (data.RequestBodylst.organization.primaryAddress.addressRegion != null ? data.RequestBodylst.organization.primaryAddress.addressRegion.name : "") + " " + (data.RequestBodylst.organization.primaryAddress.postalCode != null ? data.RequestBodylst.organization.primaryAddress.postalCode : "") + " " + (data.RequestBodylst.organization.primaryAddress.addressCountry != null ? data.RequestBodylst.organization.primaryAddress.addressCountry.isoAlpha2Code : "")) : "") + "," +
                        (data.RequestBodylst.organization != null ? data.RequestBodylst.organization.duns : "") + "," +
                        (data.RequestBodylst.organization != null ? data.RequestBodylst.organization.isActiveBusiness : false) + "," +
                        (data.RequestBodylst.organization != null && !string.IsNullOrEmpty(data.RequestBodylst.organization.tradeStyleName) ? data.RequestBodylst.organization.tradeStyleName : "") + "," +
                        (data.RequestBodylst.organization != null && data.RequestBodylst.organization.duplicateDUNS != null ? string.Join(",", data.RequestBodylst.organization.duplicateDUNS) : "") + "," +
                        (data.RequestBodylst.researchComments != null && data.RequestBodylst.researchComments.Any() ? data.RequestBodylst.researchComments[0].comment : "") + "," +
                        (data.RequestBodylst.requestorEmails != null && data.RequestBodylst.requestorEmails.Any() ? data.RequestBodylst.requestorEmails[0].email : "") + "," +
                        (data.RequestDateTime.ToDatetimeShort()) + "," +
                        (data.ResearchRequestId) + "," +
                        (data.CaseId) + "," +
                        (data.CaseStatus) + "," +
                        (!string.IsNullOrEmpty(data.CaseResolution) ? "\"" + data.CaseResolution + "\"" + (!string.IsNullOrEmpty(data.CaseSubResolution) ? " - " + data.CaseSubResolution : "") : data.CaseSubResolution) + "," +
                        (data.ResolutionDUNS) + "," +
                        (data.LastestStatusDateTime.ToDatetimeShort()) + "," +
                        (data.RequestResponseJSONlst.error != null && data.RequestResponseJSONlst.error.errorDetails != null && data.RequestResponseJSONlst.error.errorDetails.Count > 0 ? data.RequestResponseJSONlst.error.errorDetails[0].parameter + " : " + data.RequestResponseJSONlst.error.errorDetails[0].description : "")
                        );
                }
                catch (Exception)
                {
                    //Empty catch block to stop from breaking
                }
            }
            return File(new System.Text.UTF8Encoding().GetBytes(sb.ToString()), "text/csv", fileName);
        }

        #endregion

        #region "Challenge Investigation"
        public ActionResult ChalleangeInvestigation(IResearchInvestigationEntity investigationEntity)
        {
            iResearchChallengeEntity iResearchChallenge = new iResearchChallengeEntity();
            iResearchChallenge.caseID = investigationEntity.CaseId;
            iResearchChallenge.customerTransactionID = investigationEntity.SrcRecordId;
            iResearchChallenge.researchRequestID = investigationEntity.ResearchRequestId;
            iResearchChallenge.customerReference = investigationEntity?.RequestBodylst?.customerReference;
            iResearchChallenge.typeDnBCode = 33594;
            return View(iResearchChallenge);
        }

        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult SubmitChallengeInvestigation(iResearchChallengeEntity iResearchChallenge)
        {
            bool result = false;
            iResearchFacade fac = new iResearchFacade(this.CurrentClient.ApplicationDBConnectionString);
            ResearchInvestigationResponseEntity modelSubmit = new ResearchInvestigationResponseEntity();
            Utility.Utility api = new Utility.Utility();
            modelSubmit = api.ChallengeInvestigation(iResearchChallenge);
            if (modelSubmit.researchRequestID > 0)
            {
                result = true;
                return Json(new { result = result, message = iResearchInvestigationLang.msgInvestigationChallengedSuccess });
            }
            else
            {
                result = false;
                StringBuilder sb = new StringBuilder();
                ResearchInvestigationResponseEntity data = new ResearchInvestigationResponseEntity();
                data = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(modelSubmit.responseJSON);
                if (data.error != null)
                {
                    if (data.error.errorDetails != null)
                    {
                        foreach (var err in data.error.errorDetails)
                        {
                            sb.AppendLine(err.parameter + " : " + err.description);
                        }

                    }
                    else
                    {
                        sb.AppendLine(data.error.errorMessage);
                    }
                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        return Json(new { result = result, message = sb.ToString() });
                    }
                    else
                    {
                        return Json(new { result = result, message = iResearchInvestigationLang.msgInvestigationChallengedFailed});
                    }
                }
                else
                {
                    return Json(new { result = result, message = iResearchInvestigationLang.msgInvestigationChallengedFailed });
                }
            }
        }
        #endregion
    }
}