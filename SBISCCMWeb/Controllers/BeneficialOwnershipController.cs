using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.BeneficialOwnership;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, AllowLicense]
    public class BeneficialOwnershipController : BaseController
    {
        // GET: BeneficalOwnership
        public ActionResult Index()
        {
            SessionHelper.BeneficialOwnershipDataSet = string.Empty;
            SessionHelper.BeneficialOwnershipData = string.Empty;
            return View();
        }
        [RequestFromSameDomain]
        public ActionResult SearchDataForBenificiary(SearchModel objmodel)
        {
            /*Set values for helper*/
            Helper.CompanyName = Convert.ToString(objmodel.CompanyName);
            Helper.Address = Convert.ToString(objmodel.Address);
            Helper.City = Convert.ToString(objmodel.City);
            Helper.State = Convert.ToString(objmodel.State);
            Helper.PhoneNbr = Convert.ToString(objmodel.PhoneNbr);
            Helper.Zip = Convert.ToString(objmodel.Zip);
            Helper.Address1 = Convert.ToString(objmodel.Address2);

            APIResponse response;
            MainMatchEntity objmainMatchEntity = new MainMatchEntity();
            CommonSearchData common = new CommonSearchData();
            if (string.IsNullOrEmpty(objmodel.DUNS))
            {
                objmainMatchEntity = common.LoadData(objmodel.CompanyName, objmodel.Address, objmodel.Address2, objmodel.City, objmodel.State, objmodel.Country, objmodel.Zip, objmodel.PhoneNbr, objmodel.ExcludeNonHeadQuarters, objmodel.ExcludeNonMarketable, objmodel.ExcludeOutofBusiness, objmodel.ExcludeUndeliverable, objmodel.ExcludeUnreachable, objmodel.Language, null, this.CurrentClient.ApplicationDBConnectionString, null);
            }
            else
            {
                CommonSearchData objCommonSearch = new CommonSearchData();
                CommonMethod objCommon = new CommonMethod();
                response = objCommonSearch.SearchByDUNS(objmodel.DUNS, this.CurrentClient.ApplicationDBConnectionString);
                objmainMatchEntity.ResponseErroeMessage = Helper.ResponseErroeMessage;
                if (response != null)
                {
                    objCommon.InsertAPILogs(response.TransactionResponseDetail, this.CurrentClient.ApplicationDBConnectionString);
                    CompanyFacade fcd = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                    fcd.InsertCleanseMatchCallResults("", response.ResponseJSON, response.APIRequest, Helper.oUser.UserId, "");
                    objmainMatchEntity.lstMatches = response.MatchEntities;
                }
                if (!string.IsNullOrEmpty(response?.ResponseJSON))
                {
                    dynamic data = JObject.Parse(response.ResponseJSON);
                    if (data.error != null && !string.IsNullOrEmpty(data.error.errorMessage.Value))
                    {
                        objmainMatchEntity.ResponseErroeMessage = data.error.errorMessage.Value;
                        return Json(new { result = false, message = objmainMatchEntity.ResponseErroeMessage });
                    }
                }
            }
            if (objmainMatchEntity.lstMatches == null)
            {
                objmainMatchEntity.lstMatches = new List<MatchEntity>();
            }
            SessionHelper.SearchMatch = JsonConvert.SerializeObject(objmainMatchEntity.lstMatches);
            SessionHelper.SearchModel = JsonConvert.SerializeObject(objmodel);
            return View("_Index", objmainMatchEntity);
        }
        [RequestFromSameDomain]
        public ActionResult SearchBeneficialOwnershipData(string DUNSNumber, string Country, bool isModalView = false, bool isRefresh = false)
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            ViewBag.isModalView = isModalView;
            if (!string.IsNullOrEmpty(DUNSNumber))
            {
                UXBeneficialOwnershipURLEntity uRLEntity;
                BenificialOwnershipFacade fac = new BenificialOwnershipFacade(this.CurrentClient.ApplicationDBConnectionString);
                uRLEntity = fac.UXGetBeneficialOwnershipURL(DUNSNumber, Country);
                if (string.IsNullOrEmpty(uRLEntity.AuthToken))
                {
                    modal.Base = new CMPBOSV1_Base();
                    modal.lstBeneficialOwnerRelationships = new List<CMPBOSV1_BeneficialOwnerRelationships>();
                    modal.lstBeneficialOwners = new List<CMPBOSV1_BeneficialOwners>();
                    return View(modal);
                }
                if (uRLEntity.EnrichmentExists && !isRefresh)
                {
                    DataSet ds = fac.PreviewBenificialOwnershipData(DUNSNumber);
                    CommonMethod.BenificialOwnershipDataPreview(modal, ds);
                    SessionHelper.BeneficialOwnershipData = JsonConvert.SerializeObject(modal);
                    SessionHelper.BeneficialOwnershipDataSet = JsonConvert.SerializeObject(ds);
                }
                else
                {
                    Utility.BenificialOwnershipData api = new Utility.BenificialOwnershipData();
                    string benificialDataResponse = api.GetBenificialOwnershipData(uRLEntity.EnrichmentURL, uRLEntity.AuthToken);
                    dynamic data = JObject.Parse(benificialDataResponse);
                    if (data != null && uRLEntity.APIFamily.ToLower() == ApiLayerType.Directplus.ToString().ToLower())
                    {
                        if ((data.Message != null && !string.IsNullOrEmpty(data.Message.Value)) || (data.error != null && data.error.errorMessage != null && !string.IsNullOrEmpty(data.error.errorMessage.Value)))
                        {
                            modal.Base = new CMPBOSV1_Base();
                            modal.lstBeneficialOwnerRelationships = new List<CMPBOSV1_BeneficialOwnerRelationships>();
                            modal.lstBeneficialOwners = new List<CMPBOSV1_BeneficialOwners>();
                            return View(modal);
                        }
                        else
                        {
                            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                            DateTime transactionTimestamp = Convert.ToDateTime(data.transactionDetail.transactionTimestamp.Value);
                            cfac.ProcessDataForEnrichment(0, uRLEntity.DnBAPIId, DUNSNumber, Country, benificialDataResponse, "", transactionTimestamp, uRLEntity.CredentialId);
                            DataSet ds = fac.PreviewBenificialOwnershipData(DUNSNumber);
                            CommonMethod.BenificialOwnershipDataPreview(modal, ds);
                            SessionHelper.BeneficialOwnershipData = JsonConvert.SerializeObject(modal);
                            SessionHelper.BeneficialOwnershipDataSet = JsonConvert.SerializeObject(ds);
                        }
                    }
                }
            }
            return View(modal);
        }
        [RequestFromSameDomain]
        public ActionResult DownloadBenificiaryDataAsPDF(string view)
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
            {
                modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipData);
            }
            ViewBag.viewName = view;
            //return View("DownloadBenificiaryDataAsPDF",modal);
            return new Rotativa.ViewAsPdf("~/Views/BeneficialOwnership/DownloadBenificiaryDataAsPDF.cshtml", modal) { FileName = modal.Base.DnBDUNSNumber + " - " + modal.Base.organizationName + ".pdf",PageOrientation = Rotativa.Options.Orientation.Landscape };
        }
        [RequestFromSameDomain]
        public ActionResult ExportToExcel()
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
            {
                modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipData);
            }
            byte[] responseBase;
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipDataSet))
            {
                ds = JsonConvert.DeserializeObject<DataSet>(SessionHelper.BeneficialOwnershipDataSet);
                using (ExcelPackage package = new ExcelPackage())
                {
                    
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Benificiary_Base");
                        worksheet.Cells.LoadFromDataTable(ds.Tables[0], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[0].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            worksheet.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            worksheet.Cells[2, dc, ds.Tables[0].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                    {
                        ExcelWorksheet worksheetBeneficialOwnerRelationships = package.Workbook.Worksheets.Add("BeneficialOwnerRelationships");
                        worksheetBeneficialOwnerRelationships.Cells.LoadFromDataTable(ds.Tables[1], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[1].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            worksheetBeneficialOwnerRelationships.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            worksheetBeneficialOwnerRelationships.Cells[2, dc, ds.Tables[1].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                    {
                        ExcelWorksheet worksheetBeneficialOwners = package.Workbook.Worksheets.Add("BeneficialOwners");
                        worksheetBeneficialOwners.Cells.LoadFromDataTable(ds.Tables[2], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[2].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            worksheetBeneficialOwners.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            worksheetBeneficialOwners.Cells[2, dc, ds.Tables[2].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0)
                    {
                        ExcelWorksheet BeneficialOwnershipCountryWiseSummary = package.Workbook.Worksheets.Add("BeneficialOwnershipCountryWiseSummary");
                        BeneficialOwnershipCountryWiseSummary.Cells.LoadFromDataTable(ds.Tables[3], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[3].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            BeneficialOwnershipCountryWiseSummary.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            BeneficialOwnershipCountryWiseSummary.Cells[2, dc, ds.Tables[3].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 6 && ds.Tables[6].Rows.Count > 0)
                    {
                        ExcelWorksheet BeneficialOwnershipCountryWisePSCSummary = package.Workbook.Worksheets.Add("BeneficialOwnershipCountryWisePSCSummary");
                        BeneficialOwnershipCountryWisePSCSummary.Cells[1, 1].LoadFromDataTable(ds.Tables[6], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[6].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            BeneficialOwnershipCountryWisePSCSummary.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            BeneficialOwnershipCountryWisePSCSummary.Cells[2, dc, ds.Tables[6].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 7 && ds.Tables[7].Rows.Count > 0)
                    {
                        ExcelWorksheet BeneficialOwnershipNationalityWisePSCSummary = package.Workbook.Worksheets.Add("BeneficialOwnershipNationalityWisePSCSummary");
                        BeneficialOwnershipNationalityWisePSCSummary.Cells.LoadFromDataTable(ds.Tables[7], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[7].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            BeneficialOwnershipNationalityWisePSCSummary.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            BeneficialOwnershipNationalityWisePSCSummary.Cells[2, dc, ds.Tables[7].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    
                    if (ds != null && ds.Tables.Count > 8 && ds.Tables[8].Rows.Count > 0)
                    {
                        ExcelWorksheet BeneficialOwnershipTypeWisePSCSummary = package.Workbook.Worksheets.Add("BeneficialOwnershipTypeWisePSCSummary");
                        BeneficialOwnershipTypeWisePSCSummary.Cells.LoadFromDataTable(ds.Tables[8], true);
                        //Code to handle date format after the excel file gets downloaded
                        var dateColumns = from DataColumn d in ds.Tables[8].Columns
                                          where d.DataType == typeof(DateTime) || d.ColumnName.Contains("Date")
                                          select d.Ordinal + 1;

                        foreach (var dc in dateColumns)
                        {
                            BeneficialOwnershipTypeWisePSCSummary.DefaultColWidth = 12; // Set default column width and date comes in mm/dd/yyyy format 
                            BeneficialOwnershipTypeWisePSCSummary.Cells[2, dc, ds.Tables[8].Rows.Count + 2, dc].Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                    }

                    package.Workbook.Properties.Title = "Benificiary Details";

                    responseBase = package.GetAsByteArray();
                }
                return File(responseBase, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "UBO_" + modal.Base.duns + ".xlsx");
            }
            return View();
        }
        [RequestFromSameDomain]
        public ActionResult GraphViewPopup()
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
            {
                modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipData);
            }
            return PartialView(modal);
        }

        #region "Benificiary Ownership Filters"
        [RequestFromSameDomain]
        public JsonResult GetTypeDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "all", Text = "All" });
            lstType.Add(new DropDownReturn { Value = "Business", Text = "Business" });
            lstType.Add(new DropDownReturn { Value = "Individual", Text = "Individual" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }
        [RequestFromSameDomain]
        public JsonResult GetDegreeOfSeprationDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "0", Text = "0" });
            lstType.Add(new DropDownReturn { Value = "1", Text = "1" });
            lstType.Add(new DropDownReturn { Value = "2", Text = "2" });
            lstType.Add(new DropDownReturn { Value = "3", Text = "3" });
            lstType.Add(new DropDownReturn { Value = "4", Text = "4" });
            lstType.Add(new DropDownReturn { Value = "5", Text = "5" });
            lstType.Add(new DropDownReturn { Value = "6", Text = "6" });
            lstType.Add(new DropDownReturn { Value = "7", Text = "7" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }
        [RequestFromSameDomain]
        public JsonResult GetIsbenificiaryDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "true", Text = "true" });
            lstType.Add(new DropDownReturn { Value = "false", Text = "false" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }
        [RequestFromSameDomain]
        public ActionResult FilterBenificiaryData(List<FilterData> filters)
        {
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            BeneficialOwnership_Main result;
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
            {
                modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipData);
            }
            result = modal.Copy();
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "Type")
                        {
                            if (item.FilterValue != "all")
                                result.lstCombinedData = result.lstCombinedData.Where(x => valLst.Contains(x.beneficiaryType)).ToList();
                        }
                        else if (item.FieldName == "DegreeOfSeparation")
                        {
                            result.lstCombinedData = result.lstCombinedData.Where(x => valLst.Contains(Convert.ToString(x.degreeOfSeparation))).ToList();
                        }
                        else if (item.FieldName == "MinimumDirectPercentage")
                        {
                            result.lstCombinedData = result.lstCombinedData.Where(x => x.directOwnershipPercentage >= Convert.ToDouble(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "MinimumInDirectPercentage")
                        {
                            result.lstCombinedData = result.lstCombinedData.Where(x => x.indirectOwnershipPercentage >= Convert.ToDouble(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "MinimumBenificialPercentage")
                        {
                            result.lstCombinedData = result.lstCombinedData.Where(x => x.beneficialOwnershipPercentage >= Convert.ToDouble(item.FilterValue)).ToList();
                        }
                        else if (item.FieldName == "IsBenificiary")
                        {
                            if (item.FilterValue == "true")
                                result.lstCombinedData = result.lstCombinedData.Where(x => x.isBeneficiary == 1).ToList();
                            else if (item.FilterValue == "false")
                                result.lstCombinedData = result.lstCombinedData.Where(x => x.isBeneficiary == 0).ToList();
                            else
                                result.lstCombinedData = result.lstCombinedData.ToList();

                        }
                        else if (item.FieldName == "AlertType")
                        {
                            if (valLst.Count < 5)
                            {
                                result.lstCombinedData = result.lstCombinedData.Where(x => valLst.Contains(x.lastScreenedAlertType)).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            SessionHelper.BeneficialOwnershipFilteredData = JsonConvert.SerializeObject(result);
            return PartialView("ListBenificiaryData", result);
        }
        #endregion
        [RequestFromSameDomain]
        public ActionResult SendDataForScreening(string Parameters = "")
        {
            string message = string.Empty;
            bool result = false;
            try
            {
                int memberId = 0;
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    memberId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                }
                BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
                if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
                {
                    modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipFilteredData);
                }
                Utility.BenificialOwnershipData api = new Utility.BenificialOwnershipData();
                ScreeningResponse screeningResponse = api.ScreeningMultiPleData(modal, memberId);
                BenificialOwnershipFacade fac = new BenificialOwnershipFacade(this.CurrentClient.ApplicationDBConnectionString);
                message = fac.InsertScreenQueueAndResponseJSON("DNBCOMP", screeningResponse.userId, screeningResponse.credentialId, screeningResponse.requestUrl, screeningResponse.searchJSON, screeningResponse.resultsJSON);
                if (!string.IsNullOrEmpty(screeningResponse.Errormsg))
                {
                    result = false;
                    message = screeningResponse.Errormsg;
                }
                else if (message != "error")
                {
                    result = true;
                    message = "Screening completed successfully.";
                }
                else
                {
                    result = false;
                    message = CommonMessagesLang.msgCommanErrorMessage;
                }
            }
            catch (Exception)
            {
                result = false;
                message = CommonMessagesLang.msgCommanErrorMessage;
            }
            return Json(new { result = result, message = message });
        }

        [HttpGet, RequestFromSameDomain]
        public ActionResult GetScreenResponse(string Parameters)
        {
            ScreenResponseEntityViewModel screenResponseEntityView = new ScreenResponseEntityViewModel();
            string alternateId = string.Empty, beneficiaryType = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                alternateId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                beneficiaryType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                ViewBag.Name = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
            }
            BeneficialOwnership_Main modal = new BeneficialOwnership_Main();
            if (!string.IsNullOrEmpty(SessionHelper.BeneficialOwnershipData))
            {
                modal = JsonConvert.DeserializeObject<BeneficialOwnership_Main>(SessionHelper.BeneficialOwnershipFilteredData);
            }
            screenResponseEntityView.parentData = modal.lstCombinedData.FirstOrDefault(x => x.memberID == Convert.ToInt32(alternateId));
            BenificialOwnershipFacade fac = new BenificialOwnershipFacade(this.CurrentClient.ApplicationDBConnectionString);
            screenResponseEntityView.lstScreenRespinseData = fac.GetScreenResponse(alternateId, beneficiaryType);
            SessionHelper.ScreenResultData = JsonConvert.SerializeObject(screenResponseEntityView.lstScreenRespinseData);
            return PartialView("_ScreenResponseDetails", screenResponseEntityView);
        }

        [RequestFromSameDomain]
        public ActionResult DownloadScreeningResult(string name)
        {
            List<ScreenResponseEntity> lstScreenRespinseData = JsonConvert.DeserializeObject<List<ScreenResponseEntity>>(SessionHelper.ScreenResultData);
            DataTable dt = CommonMethod.ToDataTable<ScreenResponseEntity>(lstScreenRespinseData);
            string filename = name + " - ScreeningDetails.xlsx";
            byte[] response = CommonExportMethods.ExportExcelFile(dt,filename,"ScreeningDetails");
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }

        #region "Screening Result Filter"
        [RequestFromSameDomain]
        public JsonResult GetScreenCategoryDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            List<ScreenResponseEntity> lstScreenData = JsonConvert.DeserializeObject<List<ScreenResponseEntity>>(SessionHelper.ScreenResultData);
            var categories = lstScreenData.Select(x => x.Category).Distinct();
            foreach (var item in categories)
            {
                lstType.Add(new DropDownReturn { Value = item, Text = item });
            }
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        [RequestFromSameDomain]
        public JsonResult GetAlertTypeDD()
        {
            List<DropDownReturn> lstType = new List<DropDownReturn>();
            lstType.Add(new DropDownReturn { Value = "Green", Text = "Green" });
            lstType.Add(new DropDownReturn { Value = "Yellow", Text = "Yellow" });
            lstType.Add(new DropDownReturn { Value = "Risk Country", Text = "Risk Country" });
            lstType.Add(new DropDownReturn { Value = "Red", Text = "Red" });
            lstType.Add(new DropDownReturn { Value = "Double Red", Text = "Double Red" });
            lstType.Add(new DropDownReturn { Value = "Triple Red", Text = "Triple Red" });
            return Json(new { Data = lstType }, JsonRequestBehavior.AllowGet);
        }

        [RequestFromSameDomain]
        public ActionResult FilterScreeningRecords(List<FilterData> filters)
        {
            List<ScreenResponseEntity> lstScreenData = JsonConvert.DeserializeObject<List<ScreenResponseEntity>>(SessionHelper.ScreenResultData);
            List<ScreenResponseEntity> result = lstScreenData.Copy();
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "Category")
                        {
                            result = result.Where(x => valLst.Contains(x.Category)).ToList();
                        }
                        else if (item.FieldName == "AlertType")
                        {
                            if (valLst.Count < 5)
                            {
                                result = result.Where(x => valLst.Contains(x.LastScreenedAlertType)).ToList();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            return PartialView("ListScreeningResult",result);
        }


        #endregion

        public ActionResult SaveGraphImage(string img)
        {
            SessionHelper.GraphImage = img;
            return Json(new { result = true, message = "" });
        }

        private bool BulkInsert(DataTable dt, out string msg)
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
                    bulkCopy.ColumnMappings.Add("", "");
                    bulkCopy.DestinationTableName = "cmpl.ScreeningQueue";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        msg = DandBSettingLang.msgInsertUser;
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }
    }
}