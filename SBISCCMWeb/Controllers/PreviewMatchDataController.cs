using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.PreviewMatchData.Main;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using SBISCCMWeb.LanguageResources;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, Authorize, AllowLicense, DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class PreviewMatchDataController : BaseController
    {
        // GET: PreviewMatchData
        public ActionResult Index(PreviewMatchDataModel model, int? page, int? sortby, int? sortorder, int? pagevalue, string Parameters)
        {
            bool isDownload = false;
            Helper.oPreviewMatchDataModel = null;
            Response response = new Response();
            if (!string.IsNullOrEmpty(Parameters))
            {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    isDownload = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0));
            }
            else
            {
                SessionHelper.Preview_NodataMessage = "";
            }
            if (Request.IsAjaxRequest())
            {
                model.UserId = Helper.oUser.UserId;
                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 2;

                int totalCount = 0;
                int currentPageIndex = page.HasValue ? page.Value : 1;

                pagevalue = pagevalue < 5 ? 5 : pagevalue;
                int pageNumber = (page ?? 1);
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pagevalue = pagevalue;
                ViewBag.pageNumber = pageNumber;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 15;
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;

                ViewBag.Tag = model.Tag;
                ViewBag.ImportProcess = model.ImportProcess;
                ViewBag.LobTag = model.LobTag;
                ViewBag.SrcRecordId = model.SrcRecordId;
                ViewBag.IsExactMatch = model.IsExactMatch;
                ViewBag.ConfidenceCode = model.ConfidenceCode;
                ViewBag.AcceptedBy = model.AcceptedBy;
                ViewBag.UserId = model.UserId;
                model.AcceptedBy = !string.IsNullOrEmpty(model.AcceptedBy) ? model.AcceptedBy : SessionHelper.AcceptedBy;
                PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);

                model.PageNumber = currentPageIndex;
                model.PageSize = pageSize;
                DataSet ds = new DataSet();
                Models.PreviewMatchData.PreviewMatchDataModel MatchdataModel = new Models.PreviewMatchData.PreviewMatchDataModel();
                MatchdataModel.lstMatchOutPutData = new List<Models.PreviewMatchData.MatchOutPutModel>();
                MatchdataModel.lstApiModel = new List<Models.PreviewMatchData.DCP.APIModel>();
                MatchdataModel.DCP = new Models.PreviewMatchData.DCP.DCP_MainModel();
                MatchdataModel.DCP.lstBase = new List<Models.PreviewMatchData.DCP.DCP_Base>();
                MatchdataModel.DCP.lstCompetitors = new List<Models.PreviewMatchData.DCP.DCP_Competitors>();
                MatchdataModel.DCP.lstConsolidatedEmployees = new List<Models.PreviewMatchData.DCP.DCP_ConsolidatedEmployees>();
                MatchdataModel.DCP.lstCurrentPrincipals = new List<Models.PreviewMatchData.DCP.DCP_CurrentPrincipals>();
                MatchdataModel.DCP.lstFamilyTreeMembers = new List<Models.PreviewMatchData.DCP.DCP_FamilyTreeMembers>();
                MatchdataModel.DCP.lstFinancialStatements = new List<Models.PreviewMatchData.DCP.DCP_FinancialStatements>();
                MatchdataModel.DCP.lstIndustryCodes = new List<Models.PreviewMatchData.DCP.DCP_IndustryCodes>();
                MatchdataModel.DCP.lstKeyFinancialFigures = new List<Models.PreviewMatchData.DCP.DCP_KeyFinancialFigures>();
                MatchdataModel.DCP.lstNonMarketableReasons = new List<Models.PreviewMatchData.DCP.DCP_NonMarketableReasons>();
                MatchdataModel.DCP.lstOrganizationIDNumbers = new List<Models.PreviewMatchData.DCP.DCP_OrganizationIDNumbers>();
                MatchdataModel.DCP.lstSocialMedia = new List<Models.PreviewMatchData.DCP.DCP_SocialMedia>();
                MatchdataModel.DCP.lstStockExchange = new List<Models.PreviewMatchData.DCP.DCP_StockExchange>();

                ds = fac.SearchPreviewMatchData(model.Tag, model.ImportProcess, model.LobTag, model.SrcRecordId, model.IsExactMatch, model.ConfidenceCode, model.AcceptedBy, model.UserId, model.PageNumber, model.PageSize, ref totalCount);
                DataTable dt0 = ds.Tables[0];
                if (ds != null && dt0 != null)
                {
                    #region OutPutData

                    foreach (DataRow d in dt0.Rows)
                    {
                        MatchdataModel.lstMatchOutPutData.Add(new Models.PreviewMatchData.MatchOutPutModel()
                        {

                            InputdId = Convert.ToString(d[0]),
                            SrcRecordId = Convert.ToString(d[1]),
                            CompanyName = Convert.ToString(d[2]),
                            Address = Convert.ToString(d[3]),
                            City = Convert.ToString(d[4]),
                            State = Convert.ToString(d[5]),
                            PostalCode = Convert.ToString(d[6]),
                            CountryISOAlpha2Code = Convert.ToString(d[7]),
                            PhoneNbr = Convert.ToString(d[8]),
                            DUNSNumber = Convert.ToString(d[9]),
                            CEOName = Convert.ToString(d[10]),
                            WebSite = Convert.ToString(d[11]),
                            AltCompanyName = Convert.ToString(d[12]),
                            AltAddress = Convert.ToString(d[13]),
                            AltCity = Convert.ToString(d[14]),
                            AltState = Convert.ToString(d[15]),
                            AltPostalCode = Convert.ToString(d[16]),
                            AltCountry = Convert.ToString(d[17]),
                            Email = Convert.ToString(d[18]),
                            RegistrationNbr = Convert.ToString(d[19]),
                            RegistrationType = Convert.ToString(d[20]),
                            Tag = Convert.ToString(d[21]),
                            Source = Convert.ToString(d[22]),
                            UpdatedCompanyName = Convert.ToString(d[23]),
                            UpdatedAddress = Convert.ToString(d[24]),
                            UpdatedCity = Convert.ToString(d[25]),
                            UpdatedState = Convert.ToString(d[26]),
                            UpdatedPostalCode = Convert.ToString(d[27]),
                            UpdatedCountryISOAlpha2Code = Convert.ToString(d[28]),
                            UpdatedPhoneNbr = Convert.ToString(d[29]),
                            StandardizedOrganizationName = Convert.ToString(d[30]),
                            StandardizedAddressLine = Convert.ToString(d[31]),
                            StandardizedPrimaryTownName = Convert.ToString(d[32]),
                            StandardizedCountyName = Convert.ToString(d[33]),
                            StandardizedTerritoryAbbreviatedName = Convert.ToString(d[34]),
                            StandardizedTerritoryName = Convert.ToString(d[35]),
                            StandardizedPostalCode = Convert.ToString(d[36]),
                            StandardizedPostalCodeExtensionCode = Convert.ToString(d[37]),
                            StandardizedCountryISOAlpha2Code = Convert.ToString(d[38]),
                            StandardizedCountryName = Convert.ToString(d[39]),
                            StandardizedDeliveryPointValidationStatusValue = Convert.ToString(d[40]),
                            StandardizedDeliveryPointValidationCMRAValue = Convert.ToString(d[41]),
                            StandardizedAddressTypeValue = Convert.ToString(d[42]),
                            StandardizedInexactAddressIndicator = Convert.ToString(d[43]),
                            ServiceTransactionID = Convert.ToString(d[44]),
                            TransactionTimestamp = Convert.ToString(d[45]),
                            CandidateMatchedQty = Convert.ToString(d[46]),
                            MatchDataCriteriaText = Convert.ToString(d[47]),
                            DnBDUNSNumber = Convert.ToString(d[48]),
                            DnBOrganizationName = Convert.ToString(d[49]),
                            DnBTradeStyleName = Convert.ToString(d[50]),
                            DnBSeniorPrincipalName = Convert.ToString(d[51]),
                            DnBStreetAddressLine = Convert.ToString(d[52]),
                            DnBPrimaryTownName = Convert.ToString(d[53]),
                            DnBCountryISOAlpha2Code = Convert.ToString(d[54]),
                            DnBPostalCode = Convert.ToString(d[55]),
                            DnBPostalCodeExtensionCode = Convert.ToString(d[56]),
                            DnBTerritoryAbbreviatedName = Convert.ToString(d[57]),
                            DnBAddressUndeliverable = Convert.ToString(d[58]),
                            DnBMailingStreetAddressLine = Convert.ToString(d[59]),
                            DnBMailingPrimaryTownName = Convert.ToString(d[60]),
                            DnBMailingCountryISOAlpha2Code = Convert.ToString(d[61]),
                            DnBMailingPostalCode = Convert.ToString(d[62]),
                            DnBMailingPostalCodeExtensionCode = Convert.ToString(d[63]),
                            DnBMailingTerritoryAbbreviatedName = Convert.ToString(d[64]),
                            DnBMailingAddressUndeliverable = Convert.ToString(d[65]),
                            DnBTelephoneNumber = Convert.ToString(d[66]),
                            DnBTelephoneNumberUnreachableIndicator = Convert.ToString(d[67]),
                            DnBOperatingStatus = Convert.ToString(d[68]),
                            DnBFamilyTreeMemberRole = Convert.ToString(d[69]),
                            DnBStandaloneOrganization = Convert.ToString(d[70]),
                            DnBConfidenceCode = Convert.ToString(d[71]),
                            DnBMatchGradeText = Convert.ToString(d[72]),
                            DnBMatchGradeComponentCount = Convert.ToString(d[73]),
                            DnBMatchDataProfileText = Convert.ToString(d[74]),
                            DnBMatchDataProfileComponentCount = Convert.ToString(d[75]),
                            DnBDisplaySequence = Convert.ToString(d[76]),
                            DnBMarketabilityIndicator = Convert.ToString(d[77]),
                            MGCompany = Convert.ToString(d[78]),
                            MGStreetNo = Convert.ToString(d[79]),
                            MGStreetName = Convert.ToString(d[80]),
                            MGCity = Convert.ToString(d[81]),
                            MGState = Convert.ToString(d[82]),
                            MGPOBox = Convert.ToString(d[83]),
                            MGPhone = Convert.ToString(d[84]),
                            MGPostalCode = Convert.ToString(d[85]),
                            MGDensity = Convert.ToString(d[86]),
                            MGUniqueness = Convert.ToString(d[87]),
                            MGSIC = Convert.ToString(d[88]),
                            MDPCompany = Convert.ToString(d[89]),
                            MDPStreetNo = Convert.ToString(d[90]),
                            MDPStreetName = Convert.ToString(d[91]),
                            MDPCity = Convert.ToString(d[92]),
                            MDPState = Convert.ToString(d[93]),
                            MDPPOBox = Convert.ToString(d[94]),
                            MDPPhone = Convert.ToString(d[95]),
                            MDPPostalCode = Convert.ToString(d[96]),
                            MDPDUNS = Convert.ToString(d[97]),
                            MDPSIC = Convert.ToString(d[98]),
                            MDPDensity = Convert.ToString(d[99]),
                            MDPUniqueness = Convert.ToString(d[100]),
                            MDPNationalID = Convert.ToString(d[101]),
                            MDPURL = Convert.ToString(d[102]),
                            AcceptedBy = Convert.ToString(d[103]),
                            TotalRecordCount = totalCount
                        });
                    }
                    #endregion
                }

                SessionHelper.BuildList_Data = JsonConvert.SerializeObject(MatchdataModel.lstMatchOutPutData);
                var skip = pageSize * (currentPageIndex - 1);
                IPagedList<Models.PreviewMatchData.MatchOutPutModel> pglst = new StaticPagedList<Models.PreviewMatchData.MatchOutPutModel>(MatchdataModel.lstMatchOutPutData.Skip(skip).Take(pageSize).ToList(), currentPageIndex, pageSize, totalCount);
                return View(Tuple.Create(pglst, MatchdataModel));
            }
            return View();
        }

        #region "Preview Match Data Filters"
        public JsonResult GetIsExactMatchDD()
        {
            List<DropDownReturn> lstGetIsExactMatchDD = new List<DropDownReturn>();
            lstGetIsExactMatchDD.Add(new DropDownReturn { Value = "true", Text = "true" });
            lstGetIsExactMatchDD.Add(new DropDownReturn { Value = "false", Text = "false" });
            return Json(new { Data = lstGetIsExactMatchDD }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetImportProcessDD()
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            DataTable dt = fac.GetImportProcessesByQueue(ImportProcess.ExportData.ToString(), false);
            List<DropDownReturn> lstImportProcess = new List<DropDownReturn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstImportProcess.Add(new DropDownReturn { Value = dt.Rows[i]["ImportProcess"].ToString(), Text = dt.Rows[i]["ImportProcess"].ToString() });
            }
            return Json(new { Data = lstImportProcess }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetConfidenceCodeDD()
        {
            List<DropDownReturn> lstConfidenceCode = new List<DropDownReturn>();
            lstConfidenceCode.Add(new DropDownReturn { Value = "3", Text = "3" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "4", Text = "4" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "5", Text = "5" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "6", Text = "6" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "7", Text = "7" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "8", Text = "8" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "9", Text = "9" });
            lstConfidenceCode.Add(new DropDownReturn { Value = "10", Text = "10" });
            return Json(new { Data = lstConfidenceCode }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAutoAcceptDD()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetAcceptedBy();
            List<DropDownReturn> lstAcceptedBy = new List<DropDownReturn>();
            lstAcceptedBy.Add(new DropDownReturn { Value = "All", Text = "All" });
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAcceptedBy.Add(new DropDownReturn { Value = dt.Rows[i]["AcceptedBy"].ToString(), Text = dt.Rows[i]["AcceptedBy"].ToString() });
            }
            return Json(new { Data = lstAcceptedBy }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTagsDD()
        {
            // Get All tags from the database and fill the dropdown 
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetExportDataTag(Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "", Helper.oUser != null ? (Helper.oUser.Tags != null ? Helper.oUser.Tags : "") : "", Helper.oUser.UserId);
            List<DropDownReturn> lstTag = new List<DropDownReturn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTag.Add(new DropDownReturn { Value = dt.Rows[i]["Tag"].ToString(), Text = dt.Rows[i]["TagName"].ToString() });
            }
            return Json(new { Data = lstTag }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLOBTagsDD()
        {
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetTagsByTypeCode(ConstantValues.LOBTagCode);
            List<DropDownReturn> lstTagTypeCode = new List<DropDownReturn>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstTagTypeCode.Add(new DropDownReturn { Value = dt.Rows[i]["Tag"].ToString(), Text = dt.Rows[i]["Tag"].ToString() });
            }
            return Json(new { Data = lstTagTypeCode }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FilterPreviewMatchData(List<FilterData> filters)
        {
            UserSessionFilterEntity filtermodel = new UserSessionFilterEntity();
            foreach (var item in filters)
            {
                if (item.FieldName == "SrcRecordId")
                    filtermodel.SrcRecordId = item.FilterValue;
                else if (item.FieldName == "SourceRecordIdExactMatch")
                    filtermodel.IsExactMatch = Convert.ToBoolean(item.FilterValue);
                else if (item.FieldName == "ImportProcess")
                    filtermodel.ImportProcess = item.FilterValue;
                else if (item.FieldName == "ConfidenceCode")
                    filtermodel.ConfidenceCode = item.FilterValue;
                else if (item.FieldName == "AcceptedBy")
                    filtermodel.AcceptedBy = item.FilterValue;
                else if (item.FieldName == "Tag")
                    filtermodel.Tag = item.FilterValue;
                else if (item.FieldName == "LOBTag")
                    filtermodel.LOBTag = item.FilterValue;
            }
            filtermodel.UserId = Helper.oUser.UserId;
            if (filtermodel.AcceptedBy == "All")
            {
                filtermodel.AcceptedBy = "";
            }
            int pagenumber = 1;
            int totalCount = 0;
            int pagevalue = 10;

            Response response = new Response();
            PreviewMatchDataFacade pfac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataSet ds = new DataSet();
            Models.PreviewMatchData.PreviewMatchDataModel MatchdataModel = new Models.PreviewMatchData.PreviewMatchDataModel();
            MatchdataModel.lstMatchOutPutData = new List<Models.PreviewMatchData.MatchOutPutModel>();
            MatchdataModel.lstApiModel = new List<Models.PreviewMatchData.DCP.APIModel>();
            MatchdataModel.DCP = new Models.PreviewMatchData.DCP.DCP_MainModel();
            MatchdataModel.DCP.lstBase = new List<Models.PreviewMatchData.DCP.DCP_Base>();
            MatchdataModel.DCP.lstCompetitors = new List<Models.PreviewMatchData.DCP.DCP_Competitors>();
            MatchdataModel.DCP.lstConsolidatedEmployees = new List<Models.PreviewMatchData.DCP.DCP_ConsolidatedEmployees>();
            MatchdataModel.DCP.lstCurrentPrincipals = new List<Models.PreviewMatchData.DCP.DCP_CurrentPrincipals>();
            MatchdataModel.DCP.lstFamilyTreeMembers = new List<Models.PreviewMatchData.DCP.DCP_FamilyTreeMembers>();
            MatchdataModel.DCP.lstFinancialStatements = new List<Models.PreviewMatchData.DCP.DCP_FinancialStatements>();
            MatchdataModel.DCP.lstIndustryCodes = new List<Models.PreviewMatchData.DCP.DCP_IndustryCodes>();
            MatchdataModel.DCP.lstKeyFinancialFigures = new List<Models.PreviewMatchData.DCP.DCP_KeyFinancialFigures>();
            MatchdataModel.DCP.lstNonMarketableReasons = new List<Models.PreviewMatchData.DCP.DCP_NonMarketableReasons>();
            MatchdataModel.DCP.lstOrganizationIDNumbers = new List<Models.PreviewMatchData.DCP.DCP_OrganizationIDNumbers>();
            MatchdataModel.DCP.lstSocialMedia = new List<Models.PreviewMatchData.DCP.DCP_SocialMedia>();
            MatchdataModel.DCP.lstStockExchange = new List<Models.PreviewMatchData.DCP.DCP_StockExchange>();

            ds = pfac.SearchPreviewMatchData(filtermodel.Tag, filtermodel.ImportProcess, filtermodel.LOBTag, filtermodel.SrcRecordId, filtermodel.IsExactMatch, filtermodel.ConfidenceCode, filtermodel.AcceptedBy, filtermodel.UserId, pagenumber, pagevalue, ref totalCount);
            DataTable dt0 = ds.Tables[0];
            if (ds != null && dt0 != null)
            {
                #region OutPutData

                foreach (DataRow d in dt0.Rows)
                {
                    MatchdataModel.lstMatchOutPutData.Add(new Models.PreviewMatchData.MatchOutPutModel()
                    {

                        InputdId = Convert.ToString(d[0]),
                        SrcRecordId = Convert.ToString(d[1]),
                        CompanyName = Convert.ToString(d[2]),
                        Address = Convert.ToString(d[3]),
                        City = Convert.ToString(d[4]),
                        State = Convert.ToString(d[5]),
                        PostalCode = Convert.ToString(d[6]),
                        CountryISOAlpha2Code = Convert.ToString(d[7]),
                        PhoneNbr = Convert.ToString(d[8]),
                        DUNSNumber = Convert.ToString(d[9]),
                        CEOName = Convert.ToString(d[10]),
                        WebSite = Convert.ToString(d[11]),
                        AltCompanyName = Convert.ToString(d[12]),
                        AltAddress = Convert.ToString(d[13]),
                        AltCity = Convert.ToString(d[14]),
                        AltState = Convert.ToString(d[15]),
                        AltPostalCode = Convert.ToString(d[16]),
                        AltCountry = Convert.ToString(d[17]),
                        Email = Convert.ToString(d[18]),
                        RegistrationNbr = Convert.ToString(d[19]),
                        RegistrationType = Convert.ToString(d[20]),
                        Tag = Convert.ToString(d[21]),
                        Source = Convert.ToString(d[22]),
                        UpdatedCompanyName = Convert.ToString(d[23]),
                        UpdatedAddress = Convert.ToString(d[24]),
                        UpdatedCity = Convert.ToString(d[25]),
                        UpdatedState = Convert.ToString(d[26]),
                        UpdatedPostalCode = Convert.ToString(d[27]),
                        UpdatedCountryISOAlpha2Code = Convert.ToString(d[28]),
                        UpdatedPhoneNbr = Convert.ToString(d[29]),
                        StandardizedOrganizationName = Convert.ToString(d[30]),
                        StandardizedAddressLine = Convert.ToString(d[31]),
                        StandardizedPrimaryTownName = Convert.ToString(d[32]),
                        StandardizedCountyName = Convert.ToString(d[33]),
                        StandardizedTerritoryAbbreviatedName = Convert.ToString(d[34]),
                        StandardizedTerritoryName = Convert.ToString(d[35]),
                        StandardizedPostalCode = Convert.ToString(d[36]),
                        StandardizedPostalCodeExtensionCode = Convert.ToString(d[37]),
                        StandardizedCountryISOAlpha2Code = Convert.ToString(d[38]),
                        StandardizedCountryName = Convert.ToString(d[39]),
                        StandardizedDeliveryPointValidationStatusValue = Convert.ToString(d[40]),
                        StandardizedDeliveryPointValidationCMRAValue = Convert.ToString(d[41]),
                        StandardizedAddressTypeValue = Convert.ToString(d[42]),
                        StandardizedInexactAddressIndicator = Convert.ToString(d[43]),
                        ServiceTransactionID = Convert.ToString(d[44]),
                        TransactionTimestamp = Convert.ToString(d[45]),
                        CandidateMatchedQty = Convert.ToString(d[46]),
                        MatchDataCriteriaText = Convert.ToString(d[47]),
                        DnBDUNSNumber = Convert.ToString(d[48]),
                        DnBOrganizationName = Convert.ToString(d[49]),
                        DnBTradeStyleName = Convert.ToString(d[50]),
                        DnBSeniorPrincipalName = Convert.ToString(d[51]),
                        DnBStreetAddressLine = Convert.ToString(d[52]),
                        DnBPrimaryTownName = Convert.ToString(d[53]),
                        DnBCountryISOAlpha2Code = Convert.ToString(d[54]),
                        DnBPostalCode = Convert.ToString(d[55]),
                        DnBPostalCodeExtensionCode = Convert.ToString(d[56]),
                        DnBTerritoryAbbreviatedName = Convert.ToString(d[57]),
                        DnBAddressUndeliverable = Convert.ToString(d[58]),
                        DnBMailingStreetAddressLine = Convert.ToString(d[59]),
                        DnBMailingPrimaryTownName = Convert.ToString(d[60]),
                        DnBMailingCountryISOAlpha2Code = Convert.ToString(d[61]),
                        DnBMailingPostalCode = Convert.ToString(d[62]),
                        DnBMailingPostalCodeExtensionCode = Convert.ToString(d[63]),
                        DnBMailingTerritoryAbbreviatedName = Convert.ToString(d[64]),
                        DnBMailingAddressUndeliverable = Convert.ToString(d[65]),
                        DnBTelephoneNumber = Convert.ToString(d[66]),
                        DnBTelephoneNumberUnreachableIndicator = Convert.ToString(d[67]),
                        DnBOperatingStatus = Convert.ToString(d[68]),
                        DnBFamilyTreeMemberRole = Convert.ToString(d[69]),
                        DnBStandaloneOrganization = Convert.ToString(d[70]),
                        DnBConfidenceCode = Convert.ToString(d[71]),
                        DnBMatchGradeText = Convert.ToString(d[72]),
                        DnBMatchGradeComponentCount = Convert.ToString(d[73]),
                        DnBMatchDataProfileText = Convert.ToString(d[74]),
                        DnBMatchDataProfileComponentCount = Convert.ToString(d[75]),
                        DnBDisplaySequence = Convert.ToString(d[76]),
                        DnBMarketabilityIndicator = Convert.ToString(d[77]),
                        MGCompany = Convert.ToString(d[78]),
                        MGStreetNo = Convert.ToString(d[79]),
                        MGStreetName = Convert.ToString(d[80]),
                        MGCity = Convert.ToString(d[81]),
                        MGState = Convert.ToString(d[82]),
                        MGPOBox = Convert.ToString(d[83]),
                        MGPhone = Convert.ToString(d[84]),
                        MGPostalCode = Convert.ToString(d[85]),
                        MGDensity = Convert.ToString(d[86]),
                        MGUniqueness = Convert.ToString(d[87]),
                        MGSIC = Convert.ToString(d[88]),
                        MDPCompany = Convert.ToString(d[89]),
                        MDPStreetNo = Convert.ToString(d[90]),
                        MDPStreetName = Convert.ToString(d[91]),
                        MDPCity = Convert.ToString(d[92]),
                        MDPState = Convert.ToString(d[93]),
                        MDPPOBox = Convert.ToString(d[94]),
                        MDPPhone = Convert.ToString(d[95]),
                        MDPPostalCode = Convert.ToString(d[96]),
                        MDPDUNS = Convert.ToString(d[97]),
                        MDPSIC = Convert.ToString(d[98]),
                        MDPDensity = Convert.ToString(d[99]),
                        MDPUniqueness = Convert.ToString(d[100]),
                        MDPNationalID = Convert.ToString(d[101]),
                        MDPURL = Convert.ToString(d[102]),
                        AcceptedBy = Convert.ToString(d[103]),
                        TotalRecordCount = totalCount
                    });
                }
                #endregion
            }
            SessionHelper.BuildList_Data = JsonConvert.SerializeObject(MatchdataModel.lstMatchOutPutData);
            ViewBag.SrcRecordId = filtermodel.SrcRecordId;
            ViewBag.IsExactMatch = filtermodel.IsExactMatch;
            ViewBag.ImportProcess = filtermodel.ImportProcess;
            ViewBag.ConfidenceCode = filtermodel.ConfidenceCode;
            ViewBag.AcceptedBy = filtermodel.AcceptedBy;
            SessionHelper.AcceptedBy = filtermodel.AcceptedBy;
            ViewBag.LobTag = filtermodel.LOBTag;
            ViewBag.Tag = filtermodel.Tag;
            var skip = pagevalue * (1 - 1);
            IPagedList<Models.PreviewMatchData.MatchOutPutModel> pglst = new StaticPagedList<Models.PreviewMatchData.MatchOutPutModel>(MatchdataModel.lstMatchOutPutData.Skip(skip).Take(pagevalue).ToList(), pagenumber, pagevalue, totalCount);
            response.Success = true;
            return PartialView("~/Views/PreviewMatchData/SearchGrid.cshtml", Tuple.Create(pglst, MatchdataModel));
        }
        #endregion

        public ActionResult Pagging(int? page, int? sortby, int? sortorder, int? pagevalue, string Tag, string ImportProcess, string LobTag, string SrcRecordId, bool IsExactMatch = false, string ConfidenceCode = null, string AcceptedBy = null, int UserId = 0)
        {
            try
            {
                // Set page no and Page size for the pagination.

                if (pagevalue == null || Convert.ToInt32(pagevalue) == 0)
                {
                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    pagevalue = Convert.ToInt32(sfac.GetDefaultPageSize(Helper.oUser.UserId, "MatchData"));
                    pagevalue = pagevalue == 0 ? 10 : pagevalue;
                }

                if (!(sortby.HasValue && sortby.Value > 0))
                    sortby = 1;

                if (!(sortorder.HasValue && sortorder.Value > 0))
                    sortorder = 2;

                int totalCount = 0;

                int currentPageIndex = page.HasValue ? page.Value : 1;
                int pageSize = pagevalue.HasValue ? pagevalue.Value : 2;
                ViewBag.SortBy = sortby;
                ViewBag.SortOrder = sortorder;
                ViewBag.pageno = currentPageIndex;
                ViewBag.pagevalue = pageSize;

                ViewBag.Tag = Tag;
                ViewBag.ImportProcess = ImportProcess;
                ViewBag.LobTag = LobTag;
                ViewBag.SrcRecordId = SrcRecordId;
                ViewBag.IsExactMatch = IsExactMatch;
                ViewBag.ConfidenceCode = ConfidenceCode;
                ViewBag.AcceptedBy = AcceptedBy;

                PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);

                DataSet ds = new DataSet();
                Models.PreviewMatchData.PreviewMatchDataModel MatchdataModel = new Models.PreviewMatchData.PreviewMatchDataModel();
                MatchdataModel.lstMatchOutPutData = new List<Models.PreviewMatchData.MatchOutPutModel>();
                MatchdataModel.lstApiModel = new List<Models.PreviewMatchData.DCP.APIModel>();
                MatchdataModel.DCP = new Models.PreviewMatchData.DCP.DCP_MainModel();
                MatchdataModel.DCP.lstBase = new List<Models.PreviewMatchData.DCP.DCP_Base>();
                MatchdataModel.DCP.lstCompetitors = new List<Models.PreviewMatchData.DCP.DCP_Competitors>();
                MatchdataModel.DCP.lstConsolidatedEmployees = new List<Models.PreviewMatchData.DCP.DCP_ConsolidatedEmployees>();
                MatchdataModel.DCP.lstCurrentPrincipals = new List<Models.PreviewMatchData.DCP.DCP_CurrentPrincipals>();
                MatchdataModel.DCP.lstFamilyTreeMembers = new List<Models.PreviewMatchData.DCP.DCP_FamilyTreeMembers>();
                MatchdataModel.DCP.lstFinancialStatements = new List<Models.PreviewMatchData.DCP.DCP_FinancialStatements>();
                MatchdataModel.DCP.lstIndustryCodes = new List<Models.PreviewMatchData.DCP.DCP_IndustryCodes>();
                MatchdataModel.DCP.lstKeyFinancialFigures = new List<Models.PreviewMatchData.DCP.DCP_KeyFinancialFigures>();
                MatchdataModel.DCP.lstNonMarketableReasons = new List<Models.PreviewMatchData.DCP.DCP_NonMarketableReasons>();
                MatchdataModel.DCP.lstOrganizationIDNumbers = new List<Models.PreviewMatchData.DCP.DCP_OrganizationIDNumbers>();
                MatchdataModel.DCP.lstSocialMedia = new List<Models.PreviewMatchData.DCP.DCP_SocialMedia>();
                MatchdataModel.DCP.lstStockExchange = new List<Models.PreviewMatchData.DCP.DCP_StockExchange>();

                ds = fac.SearchPreviewMatchData(Tag, ImportProcess, LobTag, SrcRecordId, IsExactMatch, ConfidenceCode, AcceptedBy, UserId, currentPageIndex, pageSize, ref totalCount);
                DataTable dt0 = ds.Tables[0];
                if (ds != null && dt0 != null)
                {

                    #region OutPutData

                    foreach (DataRow d in dt0.Rows)
                    {
                        MatchdataModel.lstMatchOutPutData.Add(new Models.PreviewMatchData.MatchOutPutModel()
                        {

                            InputdId = Convert.ToString(d[0]),
                            SrcRecordId = Convert.ToString(d[1]),
                            CompanyName = Convert.ToString(d[2]),
                            Address = Convert.ToString(d[3]),
                            City = Convert.ToString(d[4]),
                            State = Convert.ToString(d[5]),
                            PostalCode = Convert.ToString(d[6]),
                            CountryISOAlpha2Code = Convert.ToString(d[7]),
                            PhoneNbr = Convert.ToString(d[8]),
                            DUNSNumber = Convert.ToString(d[9]),
                            CEOName = Convert.ToString(d[10]),
                            WebSite = Convert.ToString(d[11]),
                            AltCompanyName = Convert.ToString(d[12]),
                            AltAddress = Convert.ToString(d[13]),
                            AltCity = Convert.ToString(d[14]),
                            AltState = Convert.ToString(d[15]),
                            AltPostalCode = Convert.ToString(d[16]),
                            AltCountry = Convert.ToString(d[17]),
                            Email = Convert.ToString(d[18]),
                            RegistrationNbr = Convert.ToString(d[19]),
                            RegistrationType = Convert.ToString(d[20]),
                            Tag = Convert.ToString(d[21]),
                            Source = Convert.ToString(d[22]),
                            UpdatedCompanyName = Convert.ToString(d[23]),
                            UpdatedAddress = Convert.ToString(d[24]),
                            UpdatedCity = Convert.ToString(d[25]),
                            UpdatedState = Convert.ToString(d[26]),
                            UpdatedPostalCode = Convert.ToString(d[27]),
                            UpdatedCountryISOAlpha2Code = Convert.ToString(d[28]),
                            UpdatedPhoneNbr = Convert.ToString(d[29]),
                            StandardizedOrganizationName = Convert.ToString(d[30]),
                            StandardizedAddressLine = Convert.ToString(d[31]),
                            StandardizedPrimaryTownName = Convert.ToString(d[32]),
                            StandardizedCountyName = Convert.ToString(d[33]),
                            StandardizedTerritoryAbbreviatedName = Convert.ToString(d[34]),
                            StandardizedTerritoryName = Convert.ToString(d[35]),
                            StandardizedPostalCode = Convert.ToString(d[36]),
                            StandardizedPostalCodeExtensionCode = Convert.ToString(d[37]),
                            StandardizedCountryISOAlpha2Code = Convert.ToString(d[38]),
                            StandardizedCountryName = Convert.ToString(d[39]),
                            StandardizedDeliveryPointValidationStatusValue = Convert.ToString(d[40]),
                            StandardizedDeliveryPointValidationCMRAValue = Convert.ToString(d[41]),
                            StandardizedAddressTypeValue = Convert.ToString(d[42]),
                            StandardizedInexactAddressIndicator = Convert.ToString(d[43]),
                            ServiceTransactionID = Convert.ToString(d[44]),
                            TransactionTimestamp = Convert.ToString(d[45]),
                            CandidateMatchedQty = Convert.ToString(d[46]),
                            MatchDataCriteriaText = Convert.ToString(d[47]),
                            DnBDUNSNumber = Convert.ToString(d[48]),
                            DnBOrganizationName = Convert.ToString(d[49]),
                            DnBTradeStyleName = Convert.ToString(d[50]),
                            DnBSeniorPrincipalName = Convert.ToString(d[51]),
                            DnBStreetAddressLine = Convert.ToString(d[52]),
                            DnBPrimaryTownName = Convert.ToString(d[53]),
                            DnBCountryISOAlpha2Code = Convert.ToString(d[54]),
                            DnBPostalCode = Convert.ToString(d[55]),
                            DnBPostalCodeExtensionCode = Convert.ToString(d[56]),
                            DnBTerritoryAbbreviatedName = Convert.ToString(d[57]),
                            DnBAddressUndeliverable = Convert.ToString(d[58]),
                            DnBMailingStreetAddressLine = Convert.ToString(d[59]),
                            DnBMailingPrimaryTownName = Convert.ToString(d[60]),
                            DnBMailingCountryISOAlpha2Code = Convert.ToString(d[61]),
                            DnBMailingPostalCode = Convert.ToString(d[62]),
                            DnBMailingPostalCodeExtensionCode = Convert.ToString(d[63]),
                            DnBMailingTerritoryAbbreviatedName = Convert.ToString(d[64]),
                            DnBMailingAddressUndeliverable = Convert.ToString(d[65]),
                            DnBTelephoneNumber = Convert.ToString(d[66]),
                            DnBTelephoneNumberUnreachableIndicator = Convert.ToString(d[67]),
                            DnBOperatingStatus = Convert.ToString(d[68]),
                            DnBFamilyTreeMemberRole = Convert.ToString(d[69]),
                            DnBStandaloneOrganization = Convert.ToString(d[70]),
                            DnBConfidenceCode = Convert.ToString(d[71]),
                            DnBMatchGradeText = Convert.ToString(d[72]),
                            DnBMatchGradeComponentCount = Convert.ToString(d[73]),
                            DnBMatchDataProfileText = Convert.ToString(d[74]),
                            DnBMatchDataProfileComponentCount = Convert.ToString(d[75]),
                            DnBDisplaySequence = Convert.ToString(d[76]),
                            DnBMarketabilityIndicator = Convert.ToString(d[77]),
                            MGCompany = Convert.ToString(d[78]),
                            MGStreetNo = Convert.ToString(d[79]),
                            MGStreetName = Convert.ToString(d[80]),
                            MGCity = Convert.ToString(d[81]),
                            MGState = Convert.ToString(d[82]),
                            MGPOBox = Convert.ToString(d[83]),
                            MGPhone = Convert.ToString(d[84]),
                            MGPostalCode = Convert.ToString(d[85]),
                            MGDensity = Convert.ToString(d[86]),
                            MGUniqueness = Convert.ToString(d[87]),
                            MGSIC = Convert.ToString(d[88]),
                            MDPCompany = Convert.ToString(d[89]),
                            MDPStreetNo = Convert.ToString(d[90]),
                            MDPStreetName = Convert.ToString(d[91]),
                            MDPCity = Convert.ToString(d[92]),
                            MDPState = Convert.ToString(d[93]),
                            MDPPOBox = Convert.ToString(d[94]),
                            MDPPhone = Convert.ToString(d[95]),
                            MDPPostalCode = Convert.ToString(d[96]),
                            MDPDUNS = Convert.ToString(d[97]),
                            MDPSIC = Convert.ToString(d[98]),
                            MDPDensity = Convert.ToString(d[99]),
                            MDPUniqueness = Convert.ToString(d[100]),
                            MDPNationalID = Convert.ToString(d[101]),
                            MDPURL = Convert.ToString(d[102]),
                            AcceptedBy = Convert.ToString(d[103]),
                            TotalRecordCount = totalCount
                        });
                    }
                    #endregion
                }

                SessionHelper.BuildList_Data = JsonConvert.SerializeObject(MatchdataModel.lstMatchOutPutData);
                IPagedList<Models.PreviewMatchData.MatchOutPutModel> pglst = new StaticPagedList<Models.PreviewMatchData.MatchOutPutModel>(MatchdataModel.lstMatchOutPutData.ToList(), currentPageIndex, pageSize, totalCount);
                return PartialView("~/Views/PreviewMatchData/SearchGrid.cshtml", Tuple.Create(pglst, MatchdataModel));
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult ViewDetails(string Parameters, bool isDownload = false)
        {
            string DunsNumber = "";
            if (isDownload)
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    DunsNumber = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 0);
                }
            }
            else if (!isDownload)
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    DunsNumber = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                }
            }
            PreviewMatchDataFacade fac = new PreviewMatchDataFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataSet ds = fac.PreviewEnrichmentData(DunsNumber);
            DunsInfo model = new DunsInfo();
            CommonMethod.EnrichmentDataPreview(model, ds);

            //Download Pdf 
            if (isDownload)
            {
                if (model.lstCurrentPrincipals.Count == 0 && model.lstIndustryCodes.Count == 0 && model.lstRegistrationNumbers.Count == 0 && model.lstStockExchanges.Count == 0)
                {
                    SessionHelper.Preview_NodataMessage = PreviewMatchedDataLang.lblNoEnrichmentDataToDownload;
                    return RedirectToAction("Index", new { Parameters = Utility.Utility.GetEncryptedString(Convert.ToString(isDownload)).Replace("+", Utility.Utility.urlseparator) });
                }
                return new Rotativa.ViewAsPdf("~/Views/PreviewMatchData/Main/IndexPdf.cshtml", model) { FileName = model.Base.DnBDUNSNumber + " - " + model.Base.primaryName + ".pdf" };
            }
            ViewBag.DunsNumber = DunsNumber;
            return View("~/Views/PreviewMatchData/Main/Index.cshtml", model);
        }

    }
}