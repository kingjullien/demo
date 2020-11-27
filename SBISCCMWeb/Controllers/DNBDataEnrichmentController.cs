using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBDataEnrichmentController : BaseController
    {
        // GET: DNBDataEnrichment
        public ActionResult Index()
        {
            return View();
        }
        #region "Data Block Groups"
        //[HttpGet]
        //[Route("DNB/DataBlocks")]
        //[OutputCache(NoStore = true, Duration = 0)]
        //public ActionResult IndexDataBlockGroups()
        //{
        //    List<DataBlockGroupsEntity> lstBlockGroups = new List<DataBlockGroupsEntity>();
        //    List<DataBlocksEntity> lstBlocks = new List<DataBlocksEntity>();
        //    try
        //    {
        //        DataBlockFacade blockFacade = new DataBlockFacade(this.CurrentClient.ApplicationDBConnectionString);
        //        lstBlockGroups = blockFacade.GetDataBlockGroups(-1);
        //        lstBlocks = blockFacade.GetAllDataBlocks();
        //        ViewBag.lstBlocks = lstBlocks;
        //    }
        //    catch (Exception)
        //    {
        //        //Empty catch block to stop from breaking
        //    }
        //    string indexDataBlockGroup = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBDataEnrichment/IndexDataBlockGroups.cshtml", lstBlockGroups);
        //    string upsertDataBlockGroup = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBDataEnrichment/UpsertDataBlockGroup.cshtml", lstBlockGroups.FirstOrDefault());

        //    // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
        //    if (Request.Headers["X-PJAX"] == "true")
        //    {
        //        return Json(new { result = true, indexDataBlockGroup = indexDataBlockGroup, upsertDataBlockGroup = upsertDataBlockGroup }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        ViewBag.SelectedTab = "Data Enrichment";
        //        ViewBag.SelectedIndividualTab = "Data Blocks";
        //        return View("~/Views/DandB/Index.cshtml", lstBlockGroups);
        //    }
        //}

        //[HttpGet]
        //public ActionResult UpsertDataBlockGroup(string Parameters)
        //{
        //    DataBlockGroupsEntity dataBlock = new DataBlockGroupsEntity();
        //    List<DataBlocksEntity> lstBlocks = new List<DataBlocksEntity>();
        //    try
        //    {
        //        int DataBlockGroupId = 0;
        //        if (!string.IsNullOrEmpty(Parameters))
        //        {
        //            DataBlockGroupId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
        //            DataBlockFacade blockFacade = new DataBlockFacade(this.CurrentClient.ApplicationDBConnectionString);
        //            dataBlock = blockFacade.GetDataBlockGroupsByGroupId(DataBlockGroupId);
        //            lstBlocks = blockFacade.GetAllDataBlocks();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //Empty catch block to stop from breaking
        //    }
        //    ViewBag.lstBlocks = lstBlocks;
        //    return PartialView(dataBlock);
        //}

        //[HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        //public ActionResult UpsertDataBlockGroup(DataBlockGroupsEntity obj)
        //{
        //    try
        //    {
        //        DataBlockFacade blockFacade = new DataBlockFacade(this.CurrentClient.ApplicationDBConnectionString);
        //        obj.APIURL = "https://plus.dnb.com/v1/data/duns/{dunsNumber}?blockIDs=" + HttpUtility.UrlEncode(obj.DataBlocks);
        //        if (!string.IsNullOrEmpty(obj.CustomerReference))
        //        {
        //            obj.APIURL += "&CustomerReference=" + obj.CustomerReference;
        //        }
        //        if (!string.IsNullOrEmpty(obj.TradeUp))
        //        {
        //            obj.APIURL += "&TradeUp=" + obj.TradeUp;
        //        }
        //        blockFacade.UpsertDataBlockGroups(obj);
        //        return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(new { result = true, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //[HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        //public JsonResult DeleteDataBlockGroups(string Parameters)
        //{
        //    try
        //    {
        //        int DataBlockGroupId = 0;
        //        if (!string.IsNullOrEmpty(Parameters))
        //        {
        //            DataBlockGroupId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
        //            DataBlockFacade blockFacade = new DataBlockFacade(this.CurrentClient.ApplicationDBConnectionString);
        //            blockFacade.DeleteDataBlockGroupsByGroupId(DataBlockGroupId);
        //            return Json(new { result = true, message = DandBSettingLang.msgCommonDeleteMessage }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        #endregion

        #region "Data Enrichment"
        [Route("DNB/DataEnrichment")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult indexDataEnrichmentSettings()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DnBAPIGroupEntity> lstBnBAPIGroups = fac.GetDnBAPIGroupList(Helper.oUser != null ? (Helper.oUser.LOBTag != null ? Helper.oUser.LOBTag : "") : "");

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true" || Request.IsAjaxRequest())
            {
                return PartialView(lstBnBAPIGroups);
            }
            else
            {
                ViewBag.SelectedTab = "Data Enrichment";
                ViewBag.SelectedIndividualTab = "Data Enrichments";
                return View("~/Views/DandB/Index.cshtml", lstBnBAPIGroups);
            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteAPIGroup(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // delete dnb api data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteAPIGroup(Convert.ToInt32(Parameters));
            return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDnBAPIList(string Parameters)
        {
            int credId = 0;
            string apiType = string.Empty;
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                apiType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                credId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lst = new List<ThirdPartyAPICredentialsEntity>();
            List<DnbAPIEntity> lstAPI = new List<DnbAPIEntity>();
            if (credId == 0)
                lst = CommonDropdown.GetCredentials(this.CurrentClient.ApplicationDBConnectionString, ThirdPartyProvider.DNB.ToString(), apiType);
            else
                lstAPI = fac.GetDnBAPIList(apiType, credId);
            if (lst.Count > 1)
            {
                lst.RemoveAt(0);
                lstAPI = fac.GetDnBAPIList(apiType, lst[0].CredentialId);
            }
            var result = new { data = lstAPI, data2 = lst };
            return new JsonResult { Data = result };
        }

        [HttpGet]
        public ActionResult InsertUpdateDataEnrichment(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (Parameters == "undefined" || Parameters.ToLower() == "null")
            {
                Parameters = null;
            }
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Open Popup for DnbAbi Data.
            int GroupId = Convert.ToInt32(Parameters);
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DnBAPIGroupEntity model = new DnBAPIGroupEntity();
            if (GroupId > 0)
            {
                model = fac.GetAPIGroupDetailsById(GroupId);
                model.lstDnbAPIs = fac.GetDnBAPIList(model.APIType, model.CredentialId);
                if (model != null)
                {
                    model.tmpName = model.APIGroupName;
                    if (model.DnbAPIIds != null)
                    {
                        model.lstDnBApiGrp = new List<DnbAPIEntity>();
                        foreach (var item in model.DnbAPIIds.Split(','))
                        {
                            var obj = model.lstDnbAPIs.FirstOrDefault(d => d.DnBAPIId == Convert.ToInt32(item));
                            if (obj != null)
                            {
                                model.lstDnBApiGrp.Add(obj);
                                model.lstDnbAPIs.Remove(obj);
                            }
                        }
                    }
                }
                else
                {
                    model.lstDnbAPIs = fac.GetDnBAPIList(null);
                }
            }

            else
            {
                model.lstDnbAPIs = fac.GetDnBAPIList(null);
            }
            return PartialView(model);
        }

        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult InsertUpdateDataEnrichment(DnBAPIGroupEntity model, string btnDnBApiGrp, string Tags)
        {
            Tags = Tags.TrimStart(',').TrimEnd(',');
            model.Tags = Tags == "0" ? null : Tags;
            // Save for DnbAbi Data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.lstDnbAPIs = fac.GetDnBAPIList(model.APIType);
            List<string> lstAPIIDS = model.DnbAPIIds != null ? model.DnbAPIIds.Split(',').ToList() : new List<string>();

            if (model.IsValidSave)
            {
                if (!IsAPIGroupExists(model))
                {
                    fac.InsertOrUpdateDnBAPIDetail(model);
                    ViewBag.APIMessage = model.APIGroupId == 0 ? CommonMessagesLang.msgCommanInsertMessage : CommonMessagesLang.msgCommanUpdateMessage;
                    return Json(new { result = true, message = model.APIGroupId == 0 ? CommonMessagesLang.msgCommanInsertMessage : CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.APIMessage = DandBSettingLang.msgGroupNameExist;
                    return Json(new { result = false, message = DandBSettingLang.msgGroupNameExist }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }
        // Validate Api Group is Exist or not if exist than display according Message.
        public bool IsAPIGroupExists(DnBAPIGroupEntity model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (fac.GetDnBAPIGroupList("").Exists(d => d.APIGroupName.ToLower() == model.APIGroupName.Trim().ToLower() && d.APIGroupId != model.APIGroupId))
            {
                return true;
            }
            else
                return false;
        }
        #endregion

        #region "Thirdparty Enrichment"
        public ActionResult GetThirdPartyAPICredentials(string Parameters)
        {
            string Provider = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Provider = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            List<ThirdPartyAPICredentialsEntity> lstThirdPartyAPICredentials = new List<ThirdPartyAPICredentialsEntity>();
            ThirdPartyAPICredentialsFacade fac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            lstThirdPartyAPICredentials = fac.GetThirdPartyAPICredentials(Provider);
            return Json(lstThirdPartyAPICredentials, JsonRequestBehavior.AllowGet);
        }
        [Route("DNB/ThirdPartyEnrichment")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult ThirdPartyEnrichmentsIndex()
        {
            List<ThirdPartyEnrichmentsEntity> lstEnrichments = new List<ThirdPartyEnrichmentsEntity>();
            List<CVRefEntity> lstProviders = new List<CVRefEntity>();
            lstProviders = CommonDropdown.GetThirdPartyProviders(0, this.CurrentClient.ApplicationDBConnectionString);
            ViewBag.IsThirdPartyProvidersAvailable = lstProviders.Any();
            if (lstProviders.Any())
            {
                ThirdPartyEnrichmentsFacade fac = new ThirdPartyEnrichmentsFacade(this.CurrentClient.ApplicationDBConnectionString);
                lstEnrichments = fac.GetThirdPartyEnrichments();
            }
            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
            {
                return PartialView(lstEnrichments);
            }
            else
            {
                ViewBag.SelectedTab = "Data Enrichment";
                ViewBag.SelectedIndividualTab = "Third Party Enrichment";
                return View("~/Views/DandB/Index.cshtml", lstEnrichments);
            }
        }

        [HttpGet, RequestFromSameDomain]
        public ActionResult UpsertThirdPartyEnrichments(string Parameters)
        {
            ThirdPartyEnrichmentsEntity thirdPartyEnrichments = new ThirdPartyEnrichmentsEntity();
            int enrichId = 0;
            if (!string.IsNullOrEmpty(Parameters) && Parameters != "null")
            {
                enrichId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            ThirdPartyEnrichmentsFacade fac = new ThirdPartyEnrichmentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (enrichId > 0)
                thirdPartyEnrichments = fac.GetThirdPartyEnrichmentsByEnrichmentId(enrichId);
            return PartialView(thirdPartyEnrichments);
        }

        [HttpPost, RequestFromSameDomain]
        public ActionResult UpsertThirdPartyEnrichments(ThirdPartyEnrichmentsEntity enrichmentsEntity)
        {
            try
            {
                ThirdPartyEnrichmentsFacade fac = new ThirdPartyEnrichmentsFacade(this.CurrentClient.ApplicationDBConnectionString);
                enrichmentsEntity.EnrichmentURL = ConfigurationManager.AppSettings[enrichmentsEntity.ThirdPartyProvider].ToString() + enrichmentsEntity.EnrichmentFields;
                fac.UpsertThirdPartyEnrichments(enrichmentsEntity, Helper.oUser.UserId);
                if (enrichmentsEntity.EnrichmentId > 0)
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeleteThirdPartyEnrichments(string Parameters)
        {
            try
            {
                int enrichId = 0;
                if (!string.IsNullOrEmpty(Parameters))
                {
                    enrichId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                }
                ThirdPartyEnrichmentsFacade fac = new ThirdPartyEnrichmentsFacade(this.CurrentClient.ApplicationDBConnectionString);
                if (enrichId > 0)
                {
                    fac.DeleteThirdPartyEnrichmentsByEnrichmentId(enrichId, Helper.oUser.UserId);
                    return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetFieldsForThirdPartyEnrichment(string Parameters)
        {
            int Provider = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Provider = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            SelectList selectLists = CommonDropdown.GetFieldsForThirdPartyEnrichment(this.CurrentClient.ApplicationDBConnectionString, Provider.ToString());
            List<DropDownReturn> dropDown = new List<DropDownReturn>();
            foreach (var item in selectLists)
            {
                dropDown.Add(new DropDownReturn { Value = item.Value, Text = item.Text });
            }
            return Json(dropDown, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}