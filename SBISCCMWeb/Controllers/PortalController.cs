using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
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
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Controllers
{
    [Authorize, ValidateInput(true), TwoStepVerification, AllowLicense]
    public class PortalController : BaseController
    {
        // GET: Portal
        [Route("Portal/Features")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Index()
        {
            CommonMethod objCommon = new CommonMethod();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
            string IsEnableDataReset = objCommon.GetSettingIDs(objResult, "ENABLE_DATA_RESET");
            string IsPauseCleanseMatchEtl = objCommon.GetSettingIDs(objResult, "PAUSE_CLEANSE_MATCH_ETL");
            string IsPauseEnrichmentEtl = objCommon.GetSettingIDs(objResult, "PAUSE_ENRICHMENT_ETL");
            string INACTIVITY_PERIOD_USER_LOCKOUT = objCommon.GetSettingIDs(objResult, "INACTIVITY_PERIOD_USER_LOCKOUT");
            string DATA_IMPORT_DUPLICATE_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_DUPLICATE_RESOLUTION");//Ability to remove duplicates from the Active queue MP-466
            //Settings UI to Pause Data Purging as well as Duration(MP - 382)
            string PurgeArchivetProcess = objCommon.GetSettingIDs(objResult, "PAUSE_ARCHIVE");
            string ArchiveRetentionPeriodDays = objCommon.GetSettingIDs(objResult, "ARCHIVE_RETENTION_PERIOD_DAYS");
            ViewBag.IsPauseCleanseMatchEtl = string.IsNullOrEmpty(IsPauseCleanseMatchEtl) ? "false" : IsPauseCleanseMatchEtl;
            ViewBag.IsPauseEnrichmentEtl = string.IsNullOrEmpty(IsPauseEnrichmentEtl) ? "false" : IsPauseEnrichmentEtl;
            ViewBag.IsEnableDataReset = string.IsNullOrEmpty(IsEnableDataReset) ? "false" : IsEnableDataReset;
            //Settings UI to Pause Data Purging as well as Duration(MP - 382)
            ViewBag.PurgeArchivetProcess = string.IsNullOrEmpty(PurgeArchivetProcess) ? "false" : PurgeArchivetProcess;
            ViewBag.ArchiveRetentionPeriodDays = string.IsNullOrEmpty(ArchiveRetentionPeriodDays) ? "" : ArchiveRetentionPeriodDays;
            ViewBag.INACTIVITY_PERIOD_USER_LOCKOUT = string.IsNullOrEmpty(INACTIVITY_PERIOD_USER_LOCKOUT) ? "" : INACTIVITY_PERIOD_USER_LOCKOUT;
            ViewBag.DATA_IMPORT_DUPLICATE_RESOLUTION = string.IsNullOrEmpty(DATA_IMPORT_DUPLICATE_RESOLUTION) ? "false" : DATA_IMPORT_DUPLICATE_RESOLUTION;

            //New Process settings for transfer duns enrichment MP-507
            string TRANSFER_DUNS_AUTO_ENRICH = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH");
            string TRANSFER_DUNS_AUTO_ENRICH_TAG = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH_TAG");
            ViewBag.TRANSFER_DUNS_AUTO_ENRICH = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH) ? "false" : TRANSFER_DUNS_AUTO_ENRICH;
            ViewBag.TRANSFER_DUNS_AUTO_ENRICH_TAG = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH_TAG) ? "" : TRANSFER_DUNS_AUTO_ENRICH_TAG;

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            ViewBag.SelectedTab = "Features";
            if (Request.Headers["X-PJAX"] == "true")
            {
                return PartialView("_indexFeature");
            }
            else
            {
                return View();
            }

        }

        #region "Feature"
        public ActionResult IndexFeature()
        {
            CommonMethod objCommon = new CommonMethod();
            var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
            string IsEnableDataReset = objCommon.GetSettingIDs(objResult, "ENABLE_DATA_RESET");
            string IsPauseCleanseMatchEtl = objCommon.GetSettingIDs(objResult, "PAUSE_CLEANSE_MATCH_ETL");
            string IsPauseEnrichmentEtl = objCommon.GetSettingIDs(objResult, "PAUSE_ENRICHMENT_ETL");
            string INACTIVITY_PERIOD_USER_LOCKOUT = objCommon.GetSettingIDs(objResult, "INACTIVITY_PERIOD_USER_LOCKOUT");
            string DATA_IMPORT_DUPLICATE_RESOLUTION = objCommon.GetSettingIDs(objResult, "DATA_IMPORT_DUPLICATE_RESOLUTION");//Ability to remove duplicates from the Active queue MP-466
            //Settings UI to Pause Data Purging as well as Duration(MP - 382)
            string PurgeArchivetProcess = objCommon.GetSettingIDs(objResult, "PAUSE_ARCHIVE");
            string ArchiveRetentionPeriodDays = objCommon.GetSettingIDs(objResult, "ARCHIVE_RETENTION_PERIOD_DAYS");
            ViewBag.IsPauseCleanseMatchEtl = string.IsNullOrEmpty(IsPauseCleanseMatchEtl) ? "false" : IsPauseCleanseMatchEtl;
            ViewBag.IsPauseEnrichmentEtl = string.IsNullOrEmpty(IsPauseEnrichmentEtl) ? "false" : IsPauseEnrichmentEtl;
            ViewBag.IsEnableDataReset = string.IsNullOrEmpty(IsEnableDataReset) ? "false" : IsEnableDataReset;
            //Settings UI to Pause Data Purging as well as Duration(MP - 382)
            ViewBag.PurgeArchivetProcess = string.IsNullOrEmpty(PurgeArchivetProcess) ? "false" : PurgeArchivetProcess;
            ViewBag.ArchiveRetentionPeriodDays = string.IsNullOrEmpty(ArchiveRetentionPeriodDays) ? "" : ArchiveRetentionPeriodDays;
            ViewBag.INACTIVITY_PERIOD_USER_LOCKOUT = string.IsNullOrEmpty(INACTIVITY_PERIOD_USER_LOCKOUT) ? "" : INACTIVITY_PERIOD_USER_LOCKOUT;
            ViewBag.DATA_IMPORT_DUPLICATE_RESOLUTION = string.IsNullOrEmpty(DATA_IMPORT_DUPLICATE_RESOLUTION) ? "false" : DATA_IMPORT_DUPLICATE_RESOLUTION;

            //New Process settings for transfer duns enrichment MP-507
            string TRANSFER_DUNS_AUTO_ENRICH = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH");
            string TRANSFER_DUNS_AUTO_ENRICH_TAG = objCommon.GetSettingIDs(objResult, "TRANSFER_DUNS_AUTO_ENRICH_TAG");
            ViewBag.TRANSFER_DUNS_AUTO_ENRICH = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH) ? "false" : TRANSFER_DUNS_AUTO_ENRICH;
            ViewBag.TRANSFER_DUNS_AUTO_ENRICH_TAG = string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH_TAG) ? "" : TRANSFER_DUNS_AUTO_ENRICH_TAG;


            return PartialView("_indexFeature");
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult FeatureSetting(string Parameters)
        {
            string EnableDataReset = "", EnableChat = "", EnablePurgeArchiveProcess = "", ArchivePeriodDays = "", InactiveDays = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                EnableChat = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                EnableDataReset = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                EnablePurgeArchiveProcess = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                ArchivePeriodDays = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                InactiveDays = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
            }
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Remove UpdateDandBProcessSettings and merge with UpdateProcessSettings
            fac.UpdateProcessSettings("Feature", EnableChat, EnableDataReset, EnablePurgeArchiveProcess, ArchivePeriodDays, InactiveDays, null, null, null, null, null, null, null);
            return Json(DandBSettingLang.msgProcessSettingUpdate);
        }

        #endregion

        #region "Users"
        //listing Users Details
        [Route("Portal/Users")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexUsers()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            ViewBag.ValidCreateUser = ValidLimitOfUser(null);
            //get all Users Details
            model.users = fac.GetUsersListPaging(Helper.oUser.LOBTag);

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexUsers", model.users);
            else
            {
                ViewBag.SelectedTab = "Users";
                return View("Index", model.users);
            }
        }
        [HttpGet]
        public ActionResult AddUpdateUsers(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Get User Data while Edit
            int id = Convert.ToInt32(Parameters);
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (id > 0)
            {
                model.objUsers = fac.GetUserDetailsById(id);
                model.objUsers.tmpName = model.objUsers.EmailAddress.ToLower();
            }
            else
            {
                int tempUserid = 0;
                List<UsersEntity> lst = fac.GetUsersListPaging(Helper.oUser.LOBTag);
                if (lst != null && lst.Any())
                {
                    tempUserid = lst.FirstOrDefault().UserId;
                }
                model.objUsers = fac.GetUserDetailsById(tempUserid);
                model.objUsers.tmpName = model.objUsers.EmailAddress.ToLower();


            }
            return PartialView("_addUpdateUsers", model.objUsers);
        }
        [HttpPost, ValidateInput(true), RequestFromSameDomain, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUpdateUsers(UsersEntity UsersEntity, string btnConfigDataUser, string Tags)
        {
            // Save User Data while Edit
            UsersEntity.Tags = Tags.TrimStart(',').TrimEnd(',');
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (UsersEntity.IsValidSave)
            {
                if (!ValidLimitOfUser(Convert.ToString(UsersEntity.UserId)))
                {
                    if (!IsUserExists(UsersEntity))
                    {
                        if (!IsSamlUserExists(UsersEntity))
                        {
                            UsersEntity.ChangesByAdminPortal = false;
                            string Message = fac.InsertOrUpdateUsersDetails(UsersEntity, Convert.ToInt32(User.Identity.GetUserId()));

                            if (string.IsNullOrEmpty(Message))
                            {
                                CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                                if (!string.IsNullOrEmpty(UsersEntity.EmailAddress))
                                {
                                    var obj = cfac.GetUserByEmail(UsersEntity.EmailAddress);
                                    if (obj != null && UsersEntity.UserId == 0)
                                    {
                                        string password = Convert.ToString(UsersEntity.EmailAddress.Split('@')[0]) + "@123";
                                        string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
                                        string[] hostParts = new System.Uri(url).Host.Split('.');
                                        string domain = hostParts[0];
                                        string emailBody = string.Empty;
                                        if (Helper.Branding == Branding.Matchbook.ToString())
                                        {
                                            emailBody += "Hi, " + UsersEntity.UserName + "<br/><br/>";
                                            emailBody += MessageCollection.MatchbookAccountCreate + domain + " and click continue.<br/><br/>";
                                            emailBody += "On the login page, please log in using " + UsersEntity.EmailAddress + " as your login id and " + password + " as your temporary password.<br/><br/>";
                                            emailBody += MessageCollection.MatchbookEmailText + "<br/><br/>";
                                            emailBody += "Sincerely<br/>";
                                            emailBody += MessageCollection.MatchbookSupport;
                                            Helper.SendMail(UsersEntity.EmailAddress, MessageCollection.MatchbookWelcomeText, emailBody);
                                        }
                                        else if (Helper.Branding == Branding.DandB.ToString())
                                        {
                                            emailBody += "Hi, " + UsersEntity.UserName + "<br/><br/>";
                                            emailBody += MessageCollection.DandBAccountCreate + domain + " and click continue.<br/><br/>";
                                            emailBody += "On the login page, please log in using " + UsersEntity.EmailAddress + " as your login id and " + password + " as your temporary password.<br/><br/>";
                                            emailBody += MessageCollection.DandBEmailText + "<br/><br/>";
                                            emailBody += "Sincerely<br/>";
                                            emailBody += MessageCollection.DandBSupport;
                                            Helper.SendMail(UsersEntity.EmailAddress, MessageCollection.DandBWelcomeText, emailBody);
                                        }
                                        AccountController account = new AccountController();
                                        account.InitializeController(this.Request.RequestContext);
                                        ResetPasswordViewModel objResetpass = new ResetPasswordViewModel();
                                        objResetpass.UserId = obj.UserId.ToString();
                                        objResetpass.Email = obj.EmailAddress;
                                        objResetpass.Password = password;
                                        objResetpass.ConfirmPassword = password;
                                        await account.NewUserResetPassword(objResetpass);
                                    }
                                    return Json(new { result = true, message = btnConfigDataUser == "Add" ? CommonMessagesLang.msgCommanInsertMessage : DandBSettingLang.msgUpdateUser }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { result = false, message = DandBSettingLang.msgEmailValidation }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                return Json(new { result = false, message = Message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = DandBSettingLang.msgDuplicateSAMLuser }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = DandBSettingLang.msgDuplicateEmail }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = DandBSettingLang.msgUserLimit }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult DeleteUser(string Parameters)
        {
            int UserId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            UserId = Convert.ToInt32(Parameters);
            // Delete specific user
            string Message = string.Empty;
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            Message = fac.DeleteUser(UserId, Convert.ToInt32(User.Identity.GetUserId()), false);
            if (Message == "")
            {
                Message = DandBSettingLang.msgDeleteUser;
            }
            return Json(Message);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult Activateuser(string Parameters)
        {
            int UserId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            UserId = Convert.ToInt32(Parameters);
            string Message = string.Empty;
            if (!ValidLimitOfUser(Convert.ToString(UserId)))
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                Message = fac.ActivateUser(UserId, Convert.ToInt32(User.Identity.GetUserId()), false);
                if (Message == "")
                {
                    var OUser = fac.GetUserDetailsById(UserId);
                    if (OUser != null)
                    {
                        string emailBody = string.Empty;
                        if (Helper.Branding == Branding.Matchbook.ToString())
                        {
                            emailBody += "Hi, " + OUser.UserName + "<br/><br/>";
                            emailBody += "Your account is active.<br/><br/>";
                            emailBody += MessageCollection.MatchbookEmailText + "<br/><br/>";
                            emailBody += "Sincerely,<br/>";
                            emailBody += MessageCollection.MatchbookSupport;
                            Helper.SendMail(OUser.EmailAddress, MessageCollection.MatchbookActivateAccount, emailBody);
                        }
                        else if (Helper.Branding == Branding.DandB.ToString())
                        {
                            emailBody += "Hi, " + OUser.UserName + "<br/><br/>";
                            emailBody += "Your account is active.<br/><br/>";
                            emailBody += MessageCollection.DandBEmailText + "<br/><br/>";
                            emailBody += "Sincerely,<br/>";
                            emailBody += MessageCollection.DandBSupport;
                            Helper.SendMail(OUser.EmailAddress, MessageCollection.DandBActivateAccount, emailBody);
                        }
                    }
                    Message = DandBSettingLang.msgUserActivate;
                }
            }
            else
            {
                Message = DandBSettingLang.msgUserLimit;
            }
            return Json(Message);
        }

        public bool ValidLimitOfUser(string UserId)
        {
            //Check the User limit of this domain if number of users are already in site we return false else true.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            var Ouser = new UsersEntity();
            int NumberofUser = 0;
            if (!string.IsNullOrEmpty(UserId) && UserId != "0")
            {
                Ouser = fac.GetUserDetailsById(Convert.ToInt32(UserId));
                if (Ouser.UserStatusCode == "101004")
                {
                    if (Helper.LicenseNumberOfUsers <= 0)
                    {
                        string AppicationSubDomain = Request.Url.Authority;
                        DataTable dt = fac.GetLicenseSetting(AppicationSubDomain);
                        NumberofUser = Convert.ToInt32(!string.IsNullOrEmpty(dt.Rows[0]["LicenseNumberOfUsers"].ToString()) ? dt.Rows[0]["LicenseNumberOfUsers"].ToString() : "0");
                    }
                    else
                    {
                        NumberofUser = Convert.ToInt32(Helper.LicenseNumberOfUsers);
                    }
                    var tempNumberofUser = fac.GetActiveUsers();
                    if (NumberofUser <= tempNumberofUser)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (Helper.LicenseNumberOfUsers <= 0)
                {
                    string AppicationSubDomain = Request.Url.Authority;
                    DataTable dt = fac.GetLicenseSetting(AppicationSubDomain);
                    NumberofUser = Convert.ToInt32(!string.IsNullOrEmpty(dt.Rows[0]["LicenseNumberOfUsers"].ToString()) ? dt.Rows[0]["LicenseNumberOfUsers"].ToString() : "0");
                }
                else
                {
                    NumberofUser = Convert.ToInt32(Helper.LicenseNumberOfUsers);
                }
                int tempNumberofUser = fac.GetActiveUsers();
                if (NumberofUser <= tempNumberofUser)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsUserExists(UsersEntity modelUsers)
        {
            // Validate User is already exists or not.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (fac.GetUsersList().Exists(d => d.EmailAddress.Trim().ToLower() == modelUsers.EmailAddress.Trim().ToLower()))
            {
                if (modelUsers.tmpName == modelUsers.EmailAddress.Trim().ToLower())
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        [ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain, HttpPost]
        public JsonResult GetLOBTags(string LOBTag)
        {
            //Change Tag selection on LOB tag change
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<TagsEntity> lstTag = new List<TagsEntity>();
            lstTag = fac.GetAllTags(LOBTag);
            return Json(new { Data = lstTag }, JsonRequestBehavior.AllowGet);
        }

        public bool IsSamlUserExists(UsersEntity modelUsers)
        {
            // Validate SMAL User is already exists or not.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!Helper.SAMLSSO)
            {
                return false;
            }
            modelUsers.SSOUser = string.IsNullOrEmpty(modelUsers.SSOUser) ? "" : modelUsers.SSOUser.Trim().ToLower();
            if (fac.GetUsersList().Exists(d => d.SSOUser.Trim().ToLower() == modelUsers.SSOUser.Trim().ToLower() && d.UserId != modelUsers.UserId))
            {
                if (string.IsNullOrEmpty(modelUsers.SSOUser))
                {
                    return false;
                }
                return true;
            }
            return false;

        }
        #endregion

        #region "Common"

        #region "Manage Tags"
        [Route("Portal/Tags")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult indexManageTags()
        {
            // Get All tags from the database 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAllTagsListPaging();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexManageTags", model);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "Tags";
                return View("Index", model);
            }
        }
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult AddUpdateCompanyTags(string Parameters)
        {
            //open form of Tags
            if (Parameters == "undefined")
            {
                Parameters = null;
            }
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            if (Parameters == "undefined")
            {
                Parameters = null;
            }


            int TagId = Convert.ToInt32(Parameters);
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            TagsEntity objTags = new TagsEntity();
            objTags = fac.GetTagByTagId(TagId);
            if (objTags != null)
            {
                objTags.TagValue = string.IsNullOrEmpty(objTags.TagValue) ? "" : objTags.TagValue.TrimStart('[').TrimEnd(']');
            }
            return PartialView("_addUpdateCompanyTags", objTags);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteTag(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            //delete Tag
            string message = string.Empty;
            message = fac.DeleteTag(Convert.ToInt32(Parameters), Helper.oUser.UserId);
            if (message == "")
            {
                message = CommonMessagesLang.msgCommanDeleteMessage;
                return new JsonResult { Data = message };
            }
            return new JsonResult { Data = message };
        }


        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken]
        public ActionResult AddTags(string Parameters)
        {
            // Decrypted Value and fill the parameter value 
            string Tags = "", Tag = "", TagTypeCode = "", LOBTags = "";
            int TagId = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Tag = "[" + Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1) + "::" + Tags + "]";
                TagTypeCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                LOBTags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1).Replace("&#58&#58", "::");
                TagId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1));
            }
            string strOption = "";
            if (!string.IsNullOrEmpty(Tags.Trim()) && !string.IsNullOrEmpty(TagTypeCode.Trim()))
            {
                if (CommonMethod.isValidTagName(Tags))
                {

                    TagsEntity objTags = new TagsEntity();
                    objTags.TagId = TagId;
                    objTags.TagValue = Tags.Replace("[", "").Replace("]", "");
                    objTags.Tag = Tag;
                    objTags.TagTypeCode = TagTypeCode;
                    objTags.CreatedUserId = Convert.ToInt32(User.Identity.GetUserId());
                    objTags.LOBTag = LOBTags;
                    TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
                    if (TagId > 0)
                    {
                        fac.UpdateTags(objTags);
                        strOption = CommonMessagesLang.msgCommanUpdateMessage;
                    }
                    else
                    {
                        if (!IsTasExists(Tag))// Validate tag is already exists or not
                        {
                            // Insert Tag into database.
                            fac.InsertTags(objTags, Helper.oUser.UserId);
                            strOption = Tag;
                        }
                        else
                        {
                            strOption = CommonMessagesLang.msgExistsFail;
                        }
                    }
                }
                else
                {
                    strOption = CommonMessagesLang.msgValidCharacters;
                }
            }
            return new JsonResult { Data = strOption };
        }
        public bool IsTasExists(string tagValue)
        {
            // Validate tag is already exists or not
            bool IsExists = false;
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            int TagId = fac.GetAllTags("").Where(x => x.Tag.ToLower() == tagValue.ToLower()).Select(x => x.TagId).FirstOrDefault();
            if (TagId > 0)
            {
                IsExists = true;
            }
            return IsExists;
        }
        #endregion

        #region "Country Group"
        [Route("Portal/CountryGroup")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult indexCountryGrp()
        {
            //listing Country group for the main screen of configuration
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            //Fill Country Group and listing Country Group
            model.countryGroups = fac.GetCountryGroupList();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexCountryGrp", model.countryGroups);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "Country Groups";
                return View("Index", model.countryGroups);
            }
        }

        [HttpGet]
        public ActionResult AddUpdateCountryGrp(string Parameters)
        {
            if (Parameters == "undefined")
            {
                Parameters = null;
            }
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Open Country Group.
            int id = Convert.ToInt32(Parameters);
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (id == 0)
            {
                List<CountryGroupEntity> lst = fac.GetCountryGroupList();
                if (lst != null && lst.Any())
                {
                    id = lst.FirstOrDefault().GroupId;
                }
            }
            model.objCountryGroup = fac.GetCountryGroupDetailsById(id);
            if (model.objCountryGroup != null)
            {

                model.objCountryGroup.tmpName = model.objCountryGroup.GroupName;
                if (model.objCountryGroup.ISOAlpha2Codes != null)
                {
                    List<string> lstISOAlpha2Codes = model.objCountryGroup.ISOAlpha2Codes.Split(',').ToList();
                    model.objCountryGroup.lstCountries = new List<CountryEntity>();
                    model.countries = fac.GetCountries();
                    foreach (var item in lstISOAlpha2Codes)
                    {
                        var obj = model.countries.FirstOrDefault(d => d.ISOAlpha2Code == item);
                        if (obj != null)
                        {
                            model.objCountryGroup.lstCountries.Add(obj);
                            model.countries.Remove(obj);
                        }
                    }
                }
            }
            else
            {
                model.objCountryGroup = new CountryGroupEntity();
                model.countries = fac.GetCountries();
            }
            return PartialView("_addUpdateCountryGrp", model);
        }
        [HttpGet]
        public ActionResult AddCountryGrpPopup()
        {
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.objCountryGroup = new CountryGroupEntity();
            model.countries = fac.GetCountries();
            return PartialView("_addCountyGroupPopup", model);
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true), RequestFromSameDomain, IsGlobalUser]
        public ActionResult AddUpdateCountryGrp(UsersModel model, string btnConfigCountryGroup)
        {
            if (String.IsNullOrEmpty(Helper.AuthError))
            {
                // Save data for Country.
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                int CompanyGroupId = 0;
                if (model.objCountryGroup.IsValidSave)
                {
                    if (!IsGroupExists(model))
                    {
                        CompanyGroupId = fac.InsertOrUpdateCountryGroup(model.objCountryGroup, Helper.oUser.UserId);
                        return Json(new { result = true, message = model.objCountryGroup.GroupId == 0 ? CommonMessagesLang.msgCommanInsertMessage : CommonMessagesLang.msgCommanUpdateMessage, CountryId = CompanyGroupId }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = DandBSettingLang.msgGroupNameExist }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = Helper.AuthError }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCountryGroup(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // delete dnb Country data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                fac.DeleteCountryGroup(Convert.ToInt32(Parameters), Helper.oUser.UserId);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
        }
        public bool IsGroupExists(UsersModel model)
        {
            //Check country is already exists
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CountryGroupEntity tempobj = new CountryGroupEntity();
            tempobj = fac.GetCountryGroupByName(model.objCountryGroup.GroupName);

            if (tempobj != null)
            {
                if (tempobj.GroupName.Trim().ToLower() == model.objCountryGroup.GroupName.Trim().ToLower() && tempobj.GroupId != model.objCountryGroup.GroupId)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        #region "Export Data"
        public ActionResult ExportToExcel()
        {
            // Export data to Excel Sheet .
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtCountry = fac.GetCountryGroupDetail();
            string fileName = "CountryGroup_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "Country Group";
            byte[] response = CommonExportMethods.ExportExcelFile(dtCountry, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
        #endregion

        #region "Import Data"
        public ActionResult CountryImportData()
        {
            return View();
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CountryImportData(HttpPostedFileBase file, bool header)
        {
            // export data of the country group
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
                            Directory.CreateDirectory(directory);
                        }
                        FileInfo oFileInfo = new FileInfo(file.FileName);
                        string fileExtension = oFileInfo.Extension;
                        string fileName = System.DateTime.Now.Ticks + fileExtension;
                        path = Path.Combine(directory, Path.GetFileName(fileName));
                        file.SaveAs(path);
                        //excel file is convert in DataTable
                        dt = CommonImportMethods.ExcelToDataTable(path, header);
                        Session["Portal_Data"] = dt;
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
                    return new JsonResult { Data = CommonMessagesLang.msgCommanFileEmpty };
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgCommanChechExcelFile };
            }
            return new JsonResult { Data = "success" };
        }
        [HttpGet]
        public ActionResult CountryDataMatch()
        {
            DataTable dt = new DataTable();
            if (Session["Portal_Data"] != null)
            {
                dt = Session["Portal_Data"] as DataTable;
            }

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
                }
            }
            // Get Country Group Data Column Name
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Get Get Country groups Columns Name
            DataTable dtCountryGroup = sfac.GetCountrygroupColumnsName();
            List<string> columnName = new List<string>();
            if (dtCountryGroup.Rows.Count > 0)
            {
                for (int k = 0; k < dtCountryGroup.Rows.Count; k++)  //loop through the columns. 
                {
                    columnName.Add(Convert.ToString(dtCountryGroup.Rows[k][0]));
                }
            }
            ViewBag.ColumnList = columnName;
            ViewBag.ExternalColumn = lstAllFilter;
            IPagedList<dynamic> pagedProducts = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), 1, 100000, 0);
            return View(pagedProducts);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CountryDataMatch(string[] OrgColumnName, string[] ExcelColumnName, bool IsOverWrite = false)
        {
            string Message = string.Empty;
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = new DataTable();
            DataTable dtOrgColumns = new DataTable();
            DataTable dtColumns = new DataTable();
            if (Session["Portal_Data"] != null)
            {
                dt = Session["Portal_Data"] as DataTable;
            }
            //Get Get Country groups Columns Name
            dtOrgColumns = sfac.GetCountrygroupColumnsName();
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
                string strRowFilter = string.Empty;
                for (int m = 0; m < dtOrgColumns.Rows.Count; m++)
                {
                    if (Convert.ToString(dtOrgColumns.Rows[m][1]).ToLower() == "no")
                    {
                        for (int tt = 0; tt < dtColumns.Rows.Count; tt++)
                        {
                            if (Convert.ToString(dtColumns.Rows[tt][0]) == Convert.ToString(dtOrgColumns.Rows[m][0]))
                            {
                                strRowFilter += "([" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> '' OR [" + Convert.ToString(dtColumns.Rows[tt][1]) + "] <> 'NULL') AND";
                            }
                        }
                    }
                }
                strRowFilter = strRowFilter.Remove(strRowFilter.Length - 3, 3);
                DataTable dtTempDataNew = dt.Select(strRowFilter).CopyToDataTable();
                dt = dtTempDataNew;
                //bulk insert new records
                bool IsDataInsert = BulkInsert(dt, dtColumns, out Message, IsOverWrite);

            }
            catch (Exception)
            {
                Message = CommonMessagesLang.msgCommanEnableFileImport;
            }
            return new JsonResult { Data = Message };
        }
        private bool BulkInsert(DataTable dt, DataTable dtColumns, out string msg, bool IsOverWrite = false)
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
                    bulkCopy.DestinationTableName = "ext.StgImportCountryGroup";
                    try
                    {
                        bulkCopy.WriteToServer(dt);
                        trans.Commit();
                        DataInsert = true;
                        SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string Message = sfac.MergeCountryGroup(IsOverWrite);
                        msg = CommonMessagesLang.msgCommanInsertMessage;
                        if (!string.IsNullOrEmpty(Message))
                        {
                            msg = Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message.ToString();
                        DataInsert = false;
                    }
                }
            }
            return DataInsert;
        }

        public JsonResult UpdateExamples(string CurrentColumn)
        {
            // While we upload the excel file at that time we display first column value on selection of dropdown
            string strValue = string.Empty;
            DataTable dt = new DataTable();
            if (Session["Portal_Data"] != null)
            {
                dt = (Session["Portal_Data"] as DataTable).AsEnumerable().Take(1).CopyToDataTable();
            }
            if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(CurrentColumn) > 0)
            {
                strValue = Convert.ToString(dt.Rows[0][Convert.ToInt32(CurrentColumn) - 1]);
            }
            return new JsonResult { Data = strValue };
        }


        #endregion
        #endregion

        #region "User Comment"
        [Route("Portal/UserComments")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexUserComments()
        {
            // Display User Comments List 
            List<UserCommentsEntity> model = new List<UserCommentsEntity>();
            UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAllUserCommentsListPaging();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexUserComments", model);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "User Comments";
                return View("Index", model);
            }
        }
        [RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public ActionResult AddUpdateUserComments(string Parameters)
        {
            //open User Comments
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            if (Parameters == "undefined")
            {
                Parameters = "0";
            }
            UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            UserCommentsEntity objUserComments = fac.GetUserCommentsById(Convert.ToInt32(Parameters));
            if (objUserComments == null)
            {
                objUserComments = new UserCommentsEntity();
            }
            return PartialView("_addUpdateUserComments", objUserComments);
        }

        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult popupUserComments(UserCommentsEntity model, string btnUsersComments)
        {
            if (ModelState.IsValid)
            {
                // Save user comment to database
                UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
                if (model.CommentId == 0)
                {
                    fac.InsertUserComments(model);
                }
                else
                {
                    fac.UpdateUserComments(model);
                }
                return Json(new { result = true, message = model.CommentId == 0 ? CommonMessagesLang.msgCommanInsertMessage : CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteUserComment(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            //delete user comment
            string message = string.Empty;
            message = fac.DeleteUserComment(Convert.ToInt32(Parameters));
            if (message == "")
            {
                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region "Entity"
        public ActionResult indexEntity(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region  pagination
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            #endregion

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetCDSEntityListPaging(sortParam, currentPageIndex, pageSize, out totalCount);
            IPagedList<dynamic> pglstEntity = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), currentPageIndex, pageSize, totalCount);
            return PartialView("_indexEntity", pglstEntity);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCDSEntity(string Parameters)
        {
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                }
                // delete dnb api data.
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.DeleteCDSEntity(Convert.ToString(Parameters));
                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult AddEntity()
        {
            ViewBag.OpenFrom = "Configuration";
            return View("~/Views//Data/AddEntity.cshtml");
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult AddEntity(string Entity, string OpenFrom)
        {
            if (!string.IsNullOrEmpty(Entity))
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                string message = sfac.InsertCDSEntity(Entity);
                ViewBag.Environment = Entity;
                if (message == "")
                {
                    ViewBag.Message = CommonMessagesLang.msgCommanInsertMessage;
                    SessionHelper.EntityMessage = CommonMessagesLang.msgCommanInsertMessage;
                }
                else
                {
                    ViewBag.Message = message;
                    SessionHelper.EntityMessage = message;
                }
            }
            ViewBag.OpenFrom = OpenFrom;
            return RedirectToAction("indexEntity");
        }
        #endregion

        #region "DPM FTP Configuration"
        [Route("Portal/DPMFTPConfiguration")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexDPMFTPConfiguration()
        {
            DPMFTPConfigurationFacade fac = new DPMFTPConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DPMFTPConfigurationEntity> lstFTPConfiguration = new List<DPMFTPConfigurationEntity>();
            lstFTPConfiguration = fac.GetDPMFTPConfiguration();

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexDPMFTPConfiguration", lstFTPConfiguration);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "DPMFTP Configuration";
                return View("Index", lstFTPConfiguration);
            }
        }

        //Display DPM FTP Configuration form 
        [HttpGet, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts, AllowLicenseType]
        public ActionResult AddDPMFTPConfiguration(string Parameters)
        {
            //Display DPM FTP Configuration form     
            DPMFTPConfigurationEntity objFTPConfiguration = new DPMFTPConfigurationEntity();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (Parameters == "undefined")
                {
                    return PartialView("_addUpdateDPMFTPConfiguration", objFTPConfiguration);
                }
                DPMFTPConfigurationFacade fac = new DPMFTPConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                objFTPConfiguration = fac.GetDPMFTPConfigurationById(Convert.ToInt32(Parameters));
            }
            return PartialView("_addUpdateDPMFTPConfiguration", objFTPConfiguration);
        }

        //Insert or Update DPM FTP Configuration
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult AddDPMFTPConfiguration(DPMFTPConfigurationEntity objFTPConfiguration)
        {
            bool result = false;
            string Message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    bool Result = CommonMethod.CheckFTPConnection(objFTPConfiguration);
                    if (Result)
                    {
                        try
                        {
                            DPMFTPConfigurationFacade fac = new DPMFTPConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                            objFTPConfiguration.Password = StringCipher.Encrypt(objFTPConfiguration.Password, General.passPhrase);
                            if (objFTPConfiguration.Id > 0)
                            {
                                return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (Exception)
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(new { result = false, message = "Please enter valid FTP information." }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = DandBSettingLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
            }
        }

        // delete DPM FTP Configuration.
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteDPMFTPConfiguration(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            DPMFTPConfigurationFacade fac = new DPMFTPConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            // delete DPM FTP Configuration.
            fac.DeleteDPMFTPConfiguration(Convert.ToInt32(Parameters));
            return Json(CommonMessagesLang.msgCommanDeleteMessage);
        }
        #endregion

        #region Configuration Settings
        // Add configuration settings to UI - Client Portal
        // Getting list of external source configuration
        [Route("Portal/ExternalSource")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexConfigurationSettings()
        {
            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DataSourceConfigurationEntity> lstExternalSourceConfiguration = new List<DataSourceConfigurationEntity>();
            lstExternalSourceConfiguration = efac.GetExternalDataStore(null);

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("IndexDataSourceConfiguration", lstExternalSourceConfiguration);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "Configuration Settings";
                return View("Index", lstExternalSourceConfiguration);
            }
        }
        [HttpGet, RequestFromSameDomain]
        public ActionResult InsertUpdateExternalDataSourceConfiguration(string Parameters)
        {
            DataSourceConfigurationEntity model = new DataSourceConfigurationEntity();

            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            int DataSourceCode = 0;
            if (!string.IsNullOrEmpty(Parameters))
            {
                DataSourceCode = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                model.lstExternalSourceConfiguration = efac.GetExternalDataStore(DataSourceCode);
                if(model.lstExternalSourceConfiguration != null && model.lstExternalSourceConfiguration.Count > 0)
                {
                    model = model.lstExternalSourceConfiguration.FirstOrDefault();

                    if (model.ExternalDataStoreTypeId == Convert.ToInt32(ExternalDataSources.AZURE))
                    {
                        Azure objAzure = new Azure();
                        objAzure.AzureConfiguration = model.DataStoreConfiguration;
                        objAzure = JsonConvert.DeserializeObject<Azure>(objAzure.AzureConfiguration);
                        model.azure = objAzure;
                        model.azure.AzureExternalDataStoreName = model.ExternalDataStoreName;
                        return View(model);
                    }
                    else if (model.ExternalDataStoreTypeId == Convert.ToInt32(ExternalDataSources.FTP))
                    {
                        FTP objFtp = new FTP();
                        objFtp.FTPConfiguration = model.DataStoreConfiguration;
                        objFtp = JsonConvert.DeserializeObject<FTP>(objFtp.FTPConfiguration);
                        model.ftp = objFtp;
                        model.ftp.FTPExternalDataStoreName = model.ExternalDataStoreName;
                        return View(model);
                    }
                    else if (model.ExternalDataStoreTypeId == Convert.ToInt32(ExternalDataSources.SFTP))
                    {
                        SFTP objSftp = new SFTP();
                        objSftp.SFTPConfiguration = model.DataStoreConfiguration;
                        objSftp = JsonConvert.DeserializeObject<SFTP>(objSftp.SFTPConfiguration);
                        model.sftp = objSftp;
                        model.sftp.SFTPExternalDataStoreName = model.ExternalDataStoreName;
                        return View(model);
                    }
                    else
                    {
                        AWS objAws = new AWS();
                        objAws.AWSConfiguration = model.DataStoreConfiguration;
                        objAws = JsonConvert.DeserializeObject<AWS>(objAws.AWSConfiguration);
                        model.amazon = objAws;
                        model.amazon.AWSExternalDataStoreName = model.ExternalDataStoreName;
                        return View(model);
                    }
                }
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost, RequestFromSameDomain]
        public async Task<ActionResult> InsertUpdateExternalDataSourceConfiguration(DataSourceConfigurationEntity model)
        {
            JSONAttributes jsonObj = new JSONAttributes();
            if (model.DataSourceCode == ExternalDataSources.AWS.ToString())
            {
                jsonObj = await AmazonConfiguration(model);
            }
            else if(model.DataSourceCode == ExternalDataSources.AZURE.ToString())
            {
                jsonObj = await AzureConfiguration(model);
            }
            else if(model.DataSourceCode == ExternalDataSources.FTP.ToString())
            {
                jsonObj = FTPConfiguration(model);
            }
            else
            {
                jsonObj = SFTPConfiguration(model);
            }
            return Json(new { result = jsonObj.result, message = jsonObj.message });
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult DeleteExternalDataSourceConfiguration(string Parameters)
        {
            string Message = string.Empty;
            int externalDataStoreId = 0, userId = 0;
            bool IsDeleted = false;
            try
            {
                // Get Query string in Encrypted mode and decrypt Query string and set Parameters
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    externalDataStoreId =  Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    userId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                }
                ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                // Deleting external source configuration
                efac.DeleteExternalDataStore(externalDataStoreId, userId);                
                Message = CommonMessagesLang.msgCommanDeleteMessage;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("can not be deleted because it is in use."))
                {
                    Message = ex.Message;
                }
                else
                {
                    Message = "Error occurred.Please try again";
                }
                IsDeleted = false;
            }
            return Json(new{ result = IsDeleted, message = Message });
        }

        #region Amazon Configuration
        public async Task<JSONAttributes> AmazonConfiguration(DataSourceConfigurationEntity model)
        {
            bool result = false;
            string Message = string.Empty, dataStoreConfigurationJSON = string.Empty;

            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ExternalDataStoreType> lstExternalSourceConfiguration = new List<ExternalDataStoreType>();
            lstExternalSourceConfiguration = efac.GetExternalDataSourceType(null);
            model.UserId = Helper.oUser.UserId;
            try
            {
                bool isExist = await CommonMethod.CheckAmazonConnection(model); //Check amazon connection
                if (isExist)
                {
                    try
                    {
                        model.DataStoreConfiguration = JsonConvert.SerializeObject(model.amazon);
                        model.ExternalDataStoreName = model.amazon.AWSExternalDataStoreName;
                        model.externalDataStoreType = lstExternalSourceConfiguration.Where(x => x.Id == Convert.ToInt32(ExternalDataSources.AWS)).FirstOrDefault();
                        int Id = efac.InsertExternalDataStore(model);
                        Message = CommonMessagesLang.msgCommanInsertMessage;
                        if (model.Id > 0)
                        {
                            Message = CommonMessagesLang.msgCommanUpdateMessage;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgCommanInsertMessage;
                        }
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UQ_cfg_ExternalDataStore_ExternalDataStoreName"))
                        {
                            Message = PortalSettingLang.lblDuplicateInformationEntered + " " + model.DataSourceCode;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgSomethingWrong;
                        }
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    Message = PortalSettingLang.lblAmazonConnectionNotDone;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid URI"))
                {
                    Message = "Invalid URI: The format of the URI could not be determined.";
                }
                else
                {
                    Message = PortalSettingLang.lblEnterValidAmazonDetails;
                }
                result = false;
            }
            JSONAttributes jsonData = new JSONAttributes() { result = result, message = Message };
            return jsonData;
        }
        #endregion

        #region Azure Configuration
        public async Task<JSONAttributes> AzureConfiguration(DataSourceConfigurationEntity model)
        {
            bool result = false;
            string Message = string.Empty, dataStoreConfigurationJSON = string.Empty;

            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ExternalDataStoreType> lstExternalSourceConfiguration = new List<ExternalDataStoreType>();
            lstExternalSourceConfiguration = efac.GetExternalDataSourceType(null);
            model.UserId = Helper.oUser.UserId;
            try
            {
                bool isExist = await CommonMethod.CheckAzureConnection(model); //Check azure connection
                if (isExist)
                {
                    try
                    {
                        model.DataStoreConfiguration = JsonConvert.SerializeObject(model.azure);
                        model.ExternalDataStoreName = model.azure.AzureExternalDataStoreName;
                        model.externalDataStoreType = lstExternalSourceConfiguration.Where(x => x.Id == Convert.ToInt32(ExternalDataSources.AZURE)).FirstOrDefault();
                        int Id = efac.InsertExternalDataStore(model);
                        Message = CommonMessagesLang.msgCommanInsertMessage;
                        if (model.Id > 0)
                        {
                            Message = CommonMessagesLang.msgCommanUpdateMessage;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgCommanInsertMessage;
                        }
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UQ_cfg_ExternalDataStore_ExternalDataStoreName"))
                        {
                            Message = PortalSettingLang.lblDuplicateInformationEntered + " " + model.DataSourceCode;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgSomethingWrong;
                        }
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    Message = PortalSettingLang.lblAzureConnectionNotDone;
                }
            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("Base-64 char array"))
                {
                    Message = "Invalid length for a Base-64 char array or string";
                }
                else
                {
                    Message = PortalSettingLang.lblEnterValidAzureDetails;
                }
                result = false;
            }
            JSONAttributes jsonData = new JSONAttributes() { result = result, message = Message };
            return jsonData;
        }
        #endregion
        
        #region FTP Configuration
        public JSONAttributes FTPConfiguration(DataSourceConfigurationEntity model) 
        {
            bool result = false;
            string Message = string.Empty, dataStoreConfigurationJSON = string.Empty;

            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ExternalDataStoreType> lstExternalSourceConfiguration = new List<ExternalDataStoreType>();
            lstExternalSourceConfiguration = efac.GetExternalDataSourceType(null);
            model.UserId = Helper.oUser.UserId;
            try
            {
                DPMFTPConfigurationEntity objFTPConfiguration = new DPMFTPConfigurationEntity();
                objFTPConfiguration.Id = model.Id;
                objFTPConfiguration.Host = model.ftp.Host;
                objFTPConfiguration.Port = model.ftp.Port;
                objFTPConfiguration.Password = model.ftp.Password;
                objFTPConfiguration.UserName = model.ftp.UserName;
                bool isExist = CommonMethod.CheckFTPConnection(objFTPConfiguration); //Check ftp connection
                if (isExist)
                {
                    try
                    {
                        DPMFTPConfigurationFacade fac = new DPMFTPConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                        model.DataStoreConfiguration = JsonConvert.SerializeObject(model.ftp);
                        model.ExternalDataStoreName = model.ftp.FTPExternalDataStoreName;
                        model.externalDataStoreType = lstExternalSourceConfiguration.Where(x => x.Id == Convert.ToInt32(ExternalDataSources.FTP)).FirstOrDefault();
                        int Id = efac.InsertExternalDataStore(model);
                        if (model.Id > 0)
                        {
                            Message = CommonMessagesLang.msgCommanUpdateMessage;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgCommanInsertMessage;
                        }
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UQ_cfg_ExternalDataStore_ExternalDataStoreName"))
                        {
                            Message = PortalSettingLang.lblDuplicateInformationEntered + " " + model.DataSourceCode;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgSomethingWrong;
                        }
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    Message = PortalSettingLang.lblFTPConnectionNotDone;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("530"))
                {
                    Message = "The remote server returned an error: (530) Not logged in.";
                }
                else
                {
                    Message = PortalSettingLang.lblEnterValidFTPDetails;
                }
                result = false;
            }
            JSONAttributes jsonData = new JSONAttributes() { result = result, message = Message };
            return jsonData;
        }
        #endregion
        
        #region SFTP Configuration
        public JSONAttributes SFTPConfiguration(DataSourceConfigurationEntity model)
        {
            bool result = false;
            string Message = string.Empty, dataStoreConfigurationJSON = string.Empty;

            ExternalSourceConfigurationFacade efac = new ExternalSourceConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ExternalDataStoreType> lstExternalSourceConfiguration = new List<ExternalDataStoreType>();
            lstExternalSourceConfiguration = efac.GetExternalDataSourceType(null);
            model.UserId = Helper.oUser.UserId;
            try
            {
                string ApplicationSubDomain = Helper.hostName.Contains(":") ? Helper.hostName.Split(':')[0] : Helper.hostName;
                Stream objStream = new MemoryStream();
                string isFileURL = string.Empty;
                string path = string.Empty;
                string prevFileName = model.sftp.SSHFilePath != null ? model.sftp.SSHFilePath.Split('/').Last() : string.Empty;
                // If file is updated then delete previous one
                if (model.sftp.SSHFileForUpdate != null)
                {
                    try
                    {
                        CommonMethod.DeleteBlobFile(ApplicationSubDomain, prevFileName);
                        string containerName = ExternalDataSources.SFTP.ToString().ToLower();
                        isFileURL = CommonMethod.CreateAndUploadBlobFile(ApplicationSubDomain, model.sftp.SSHFileForUpdate, model.sftp.SSHFileForUpdate.FileName, containerName);
                        if (!string.IsNullOrEmpty(isFileURL))
                        {
                            objStream = CommonMethod.DownloadBlobFile(ApplicationSubDomain, model.sftp.SSHFileForUpdate.FileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        result = false;
                        Message = PortalSettingLang.lblEnterSFTPDetails;
                    }
                }
                else
                {
                    if (model.sftp.SSHFile != null)
                    {
                        try
                        {
                            string containerName = ExternalDataSources.SFTP.ToString().ToLower();
                            isFileURL = CommonMethod.CreateAndUploadBlobFile(ApplicationSubDomain, model.sftp.SSHFile, model.sftp.SSHFile.FileName, containerName);
                            if (!string.IsNullOrEmpty(isFileURL))
                            {
                                objStream = CommonMethod.DownloadBlobFile(ApplicationSubDomain, model.sftp.SSHFile.FileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            result = false;
                            Message = PortalSettingLang.lblEnterSFTPDetails;
                        }
                    }
                }
                bool isExist = false;
                if (!string.IsNullOrEmpty(isFileURL))
                {
                    isExist = CommonMethod.CheckSFTPConnection(model, objStream); //Check sftp connection
                }
                if (isExist)
                {
                    try
                    {
                        SFTP objSftp = new SFTP();
                        objSftp.SFTPHost = model.sftp.SFTPHost;
                        objSftp.SFTPPort = model.sftp.SFTPPort;
                        objSftp.SFTPUserName = model.sftp.SFTPUserName;
                        objSftp.SSHFilePath = isFileURL.Trim().ToString();
                        model.DataStoreConfiguration = JsonConvert.SerializeObject(objSftp);
                        model.ExternalDataStoreName = model.sftp.SFTPExternalDataStoreName;
                        model.externalDataStoreType = lstExternalSourceConfiguration.Where(x => x.Id == Convert.ToInt32(ExternalDataSources.SFTP)).FirstOrDefault();
                        int Id = efac.InsertExternalDataStore(model);
                        Message = CommonMessagesLang.msgCommanInsertMessage;
                        if (model.Id > 0)
                        {
                            Message = CommonMessagesLang.msgCommanUpdateMessage;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgCommanInsertMessage;
                        }
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("UQ_cfg_ExternalDataStore_ExternalDataStoreName"))
                        {
                            Message = PortalSettingLang.lblDuplicateInformationEntered + " " + model.DataSourceCode;
                        }
                        else
                        {
                            Message = CommonMessagesLang.msgSomethingWrong;
                        }
                        result = false;
                    }
                }
                else
                {
                    result = false;
                    Message = PortalSettingLang.lblSFTPConnectionNotDone;
                }

            }
            catch (Exception ex)
            {
                if(ex.Message.Contains("An established connection was aborted by the server"))
                {
                    Message = ex.Message;
                }
                else if(ex.InnerException != null)
                {
                    Message = ex.InnerException.Message;
                }
                else
                {
                    Message = PortalSettingLang.lblEnterSFTPDetails;
                }
                result = false;
            }
            JSONAttributes jsonData = new JSONAttributes(){ result = result, message = Message };
            return jsonData;
        }
        #endregion

        #endregion

        #endregion

        #region Data Governance 
        #region Configure Imports
        //User Story 105-File Import Configuration settings page - Client Portal
        [Route("Portal/ConfigureImports")]
        public ActionResult IndexConfigureImports()
        {
            ImportFileConfigurationFacade tac = new ImportFileConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ImportFileConfigurationEntity> model = new List<ImportFileConfigurationEntity>();
            //Get Import File Configuration List            
            model = tac.GetImportFileConfiguration(null);

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView("_indexConfigureImports", model);
            else
            {
                ViewBag.SelectedTab = "Data Governance";
                ViewBag.SelectedIndividualTab = "Configure Imports";
                return View("~/Views/Portal/Index.cshtml", model);
            }
        }
        //GET : Insert Update Configure Imports
        public ActionResult InsertUpdateConfigureImports(string Parameters, bool IsFromImportData = false, int TemplateId = 0, string TemplateName = null)
        {
            ImportFileConfigurationFacade tac = new ImportFileConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ImportFileConfigurationEntity> lstImportConfiguration = new List<ImportFileConfigurationEntity>();
            ImportFileConfigurationEntity model = new ImportFileConfigurationEntity();
            int id = 0;            
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                id = Convert.ToInt32(Parameters);
                if (id > 0)
                {
                    lstImportConfiguration = tac.GetImportFileConfiguration(id);
                    if(lstImportConfiguration != null && lstImportConfiguration.Count > 0)
                    {
                        model = lstImportConfiguration.FirstOrDefault();
                    }
                    if (!string.IsNullOrEmpty(model.PostLoadActionParameters))
                    {
                        if (model.PostLoadAction == "ARCHIVE")
                        {
                            ArchiveJSON objArchiveJSON = JsonConvert.DeserializeObject<ArchiveJSON>(model.PostLoadActionParameters);
                            model.AppendUTCTimestamp = objArchiveJSON.AppendUTCTimestamp == 1 ? true : false;
                            model.PostLoadActionParameters = objArchiveJSON.ArchivePath;
                        }
                        else if (model.PostLoadAction == "RENAME")
                        {
                            RenameJSON objRenameJSON = JsonConvert.DeserializeObject<RenameJSON>(model.PostLoadActionParameters);
                            model.AppendUTCTimestamp = objRenameJSON.AppendUTCTimestamp == 1 ? true : false;
                            model.PostLoadActionParameters = objRenameJSON.NewFileExtension;
                        }
                    }
                }
            }
            ViewBag.IsFromImportData = IsFromImportData;
            if(IsFromImportData == true)
            {
                ViewBag.TemplateId = TemplateId;
                ViewBag.TemplateName = TemplateName;
            }
            return PartialView("_insertUpdateConfigureImports", model);
        }
        //POST : Insert Update Configure Imports
        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult InsertUpdateConfigureImports(ImportFileConfigurationEntity obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ImportFileConfigurationFacade fac = new ImportFileConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                    obj.UserId = Helper.oUser.UserId;
                    if (!string.IsNullOrEmpty(obj.PostLoadActionParameters))
                    {
                        if (obj.PostLoadAction == "ARCHIVE")
                        {
                            ArchiveJSON objArchiveJSON = new ArchiveJSON();
                            objArchiveJSON.PostLoadAction = obj.PostLoadAction;
                            objArchiveJSON.ArchivePath = obj.PostLoadActionParameters;
                            objArchiveJSON.AppendUTCTimestamp = obj.AppendUTCTimestamp == true ? 1 : 0;
                            obj.PostLoadActionParameters = JsonConvert.SerializeObject(objArchiveJSON);
                        }
                        else if(obj.PostLoadAction == "RENAME")
                        {
                            RenameJSON objRenameJSON = new RenameJSON();
                            objRenameJSON.PostLoadAction = obj.PostLoadAction;
                            objRenameJSON.NewFileExtension = obj.PostLoadActionParameters;
                            objRenameJSON.AppendUTCTimestamp = obj.AppendUTCTimestamp == true ? 1 : 0;
                            obj.PostLoadActionParameters = JsonConvert.SerializeObject(objRenameJSON);
                        }

                    }
                    string message = fac.InsertImportFileConfiguration(obj);
                    // set message for insert & update
                    if (string.IsNullOrEmpty(message))
                    {
                        if (obj.Id > 0)
                        {
                            return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("Cannot insert duplicate key in object"))
                    {
                        return Json(new { result = false, message = "Confiuguration Name should be unique" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                ViewBag.Message = DandBSettingLang.msgInvadilState;
            }
            return View("_insertUpdateConfigureImports", obj);
        }
        // Delete Configure Imports
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteConfigureImports(string Parameters)
        {
            string Message = string.Empty;
            int id = 0;
            try
            {
                // Get Query string in Encrypted mode and decrypt Query string and set Parameters
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    id = Convert.ToInt32(Parameters);
                }
                ImportFileConfigurationFacade fac = new ImportFileConfigurationFacade(this.CurrentClient.ApplicationDBConnectionString);
                // delete DPM FTP Configuration.
                fac.DeleteImportFileConfiguration(id);
                Message = CommonMessagesLang.msgCommanDeleteMessage;
            }
            catch (Exception ex)
            {
                Message = "Error occurred.Please try again";
            }
            return Json(new { result = true, message = Message });
        }
        #endregion
        #endregion

        #region "About Us"
        [Route("Portal/AboutUs")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexAboutUs()
        {
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Get Current User Name
            Helper.UserName = Convert.ToString(User.Identity.GetUserName());
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            //Get User detail by the user Id.
            if (Helper.oUser == null)
            {
                Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            }
            UsersModel Users = new UsersModel();
            //Get Login User Detail 
            Users.objUsers = fac.StewUserLogIn(Helper.oUser.EmailAddress, null, true);
            if (Users.objUsers != null)
            {
                ViewBag.ClientGUID = Users.objUsers.ClientGUID;
                ViewBag.ClientName = Users.objUsers.ClientName;
            }
            ViewBag.User = Helper.oUser.UserFullName + "/" + Helper.oUser.EmailAddress;
            // Get Database detail from the connection string.
            using (SqlConnection connection = new SqlConnection(this.CurrentClient.ApplicationDBConnectionString))
            {
                ViewBag.ServerName = connection.DataSource;
                ViewBag.DataBaseName = connection.Database;
            }

            //set IpAddress and Version
            ViewBag.IpAddress = Helper.GetCurrentIpAddress();
            ViewBag.BuildVersion = Convert.ToString(ConfigurationManager.AppSettings["BuildVersion"]);
            ViewBag.FinaldVersion = Convert.ToString(ConfigurationManager.AppSettings["FinalVersion"]);

            string url = Request.Url.Authority;

            sfac = new SettingFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
            DataTable dt = sfac.GetLicenseSetting(url);

            string[] hostParts = new System.Uri(Request.Url.Scheme + "://" + url).Host.Split('.');
            string subDomain = hostParts[0];
            if (dt != null)
            {
                //set all License elements
                ViewBag.LicenseSKU = Convert.ToString(dt.Rows[0]["LicenseSKU"].ToString());
                ViewBag.LicenseNumberOfUsers = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseNumberOfUsers"])) ? Convert.ToInt32(dt.Rows[0]["LicenseNumberOfUsers"]) : 0;
                ViewBag.LicenseNumberOfTransactions = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseNumberOfTransactions"])) ? Convert.ToInt32(dt.Rows[0]["LicenseNumberOfTransactions"]) : 0;
                ViewBag.LicenseEnableLiveAPI = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableLiveAPI"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableLiveAPI"]) : false;
                ViewBag.LicenseEnableTags = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableTags"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableTags"]) : false;
                ViewBag.LicenseEndDate = !string.IsNullOrEmpty(dt.Rows[0]["LicenseEndDate"].ToString()) ? Convert.ToDateTime(dt.Rows[0]["LicenseEndDate"]).ToDatetimeShort() : string.Empty;
                ViewBag.MonitorProfileDirect20 = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableMonitoring"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableMonitoring"]) : false;
                ViewBag.MonitorProfileDirectPlus = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableDPM"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableDPM"]) : false;
                ViewBag.Investigation = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["LicenseEnableInvestigations"])) ? Convert.ToBoolean(dt.Rows[0]["LicenseEnableInvestigations"]) : false;
                SessionHelper.Portal_APIKEY = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ApiKey"])) ? Convert.ToString(dt.Rows[0]["ApiKey"]) : "";
                SessionHelper.Portal_APISecret = !string.IsNullOrEmpty(Convert.ToString(dt.Rows[0]["ApiSecret"])) ? Convert.ToString(dt.Rows[0]["ApiSecret"]) : "";
                SessionHelper.Portal_SubDomain = subDomain;
            }
            else
            {
                //set all License elements empty or default when data is null in database
                ViewBag.LicenseSKU = "";
                ViewBag.LicenseNumberOfUsers = "0";
                ViewBag.LicenseNumberOfTransactions = "0";
                ViewBag.LicenseEnableLiveAPI = false;
                ViewBag.LicenseEnableTags = false;
                ViewBag.LicenseEndDate = "";
                ViewBag.MonitorProfileDirect20 = false;
                ViewBag.MonitorProfileDirectPlus = false;
                ViewBag.Investigation = false;
            }

            if (Helper.UserType.ToLower() != "steward")
            {
                // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
                if (Request.Headers["X-PJAX"] == "true")
                    return PartialView("_indexAboutUs");
                else
                {
                    ViewBag.SelectedTab = "About Us";
                    return View("Index");
                }
            }
            else
            {
                ViewBag.SelectedTab = "About Us";
                return View("Index");
            }

        }

        // Display API Credential 
        public ActionResult APICredential()
        {
            return View();
        }

        #endregion

        #region "Export Page Settings"
        [Route("Portal/ExportPageSettings")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexExportPageSettings()
        {
            CleanseMatchSettingsModel model = new CleanseMatchSettingsModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Fill all dropdown and value for setting value.
            model.Settings = fac.GetCleanseMatchSettings();
            if(model.Settings != null)
            {
                GetSettingIDs(model);
                SetMatchGradeContent(model);
                Clear(model);
            }

            // MP-1046 Create Individual URL redirection for all Tabs to make better format for URL
            if (Request.Headers["X-PJAX"] == "true")
                return PartialView(model);
            else
            {
                ViewBag.SelectedTab = "Common";
                ViewBag.SelectedIndividualTab = "Export Page Settings";
                return View("Index", model);
            }
        }
        //new settings page size override settings(MP-322)
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateInput(true)]
        public JsonResult IndexExportPageSettings(FormCollection model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CleanseMatchSettingsModel oldmodel = new CleanseMatchSettingsModel();
            oldmodel.Settings = fac.GetCleanseMatchSettings();
            GetSettingIDs(oldmodel);
            //set Properties of  page size override settings



            string PAGE_SIZE_MATCH_OUTPUT = model["Settings[" + oldmodel.PAGE_SIZE_MATCH_OUTPUT + "].SettingValue"];
            string PAGE_SIZE_ENRICHMENT_OUTPUT = model["Settings[" + oldmodel.PAGE_SIZE_ENRICHMENT_OUTPUT + "].SettingValue"];
            string PAGE_SIZE_MONITORING_OUTPUT = model["Settings[" + oldmodel.PAGE_SIZE_MONITORING_OUTPUT + "].SettingValue"];
            string PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT = model["Settings[" + oldmodel.PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT + "].SettingValue"];
            if (CommonMethod.IsDigitsOnly(PAGE_SIZE_MATCH_OUTPUT) &&
                CommonMethod.IsDigitsOnly(PAGE_SIZE_ENRICHMENT_OUTPUT) &&
                CommonMethod.IsDigitsOnly(PAGE_SIZE_MONITORING_OUTPUT) &&
                CommonMethod.IsDigitsOnly(PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT))
            {
                oldmodel.Settings[oldmodel.PAGE_SIZE_MATCH_OUTPUT].SettingValue = PAGE_SIZE_MATCH_OUTPUT;
                oldmodel.Settings[oldmodel.PAGE_SIZE_ENRICHMENT_OUTPUT].SettingValue = PAGE_SIZE_ENRICHMENT_OUTPUT;
                oldmodel.Settings[oldmodel.PAGE_SIZE_MONITORING_OUTPUT].SettingValue = PAGE_SIZE_MONITORING_OUTPUT;
                oldmodel.Settings[oldmodel.PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT].SettingValue = PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT;
                //update Cleanse Match Settings
                fac.UpdateCleanseMatchSettings(oldmodel.Settings);
                return Json(DandBSettingLang.msgSettingUpdate);
            }
            else
            {
                return Json(DandBSettingLang.msgInvadilState);
            }
        }

        private void GetSettingIDs(CleanseMatchSettingsModel CleanseMatchSettingsModel)
        {
            //Get Cleanse Match Settings
            for (int i = 0; i < CleanseMatchSettingsModel.Settings.Count; i++)
            {
                string settingname = CleanseMatchSettingsModel.Settings[i].SettingName;
                switch (settingname)
                {
                    case "AUTO_CORRECTION_THRESHOLD":
                        CleanseMatchSettingsModel.AUTO_CORRECTION_THRESHOLD = i; break;
                    case "MAX_PARALLEL_THREAD":
                        CleanseMatchSettingsModel.MAX_PARALLEL_THREAD = i; break;
                    case "MATCH_GRADE_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NO_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NO_THRESHOLD = i; break;
                    case "MATCH_GRADE_STREET_NAME_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STREET_NAME_THRESHOLD = i; break;
                    case "MATCH_GRADE_CITY_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_CITY_THRESHOLD = i; break;
                    case "MATCH_GRADE_STATE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_STATE_THRESHOLD = i; break;
                    case "MATCH_GRADE_TELEPHONE_THRESHOLD":
                        CleanseMatchSettingsModel.MATCH_GRADE_TELEPHONE_THRESHOLD = i; break;
                    case "MATCH_GRADE_POBOX_THRESHOLD":   //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)
                        CleanseMatchSettingsModel.MATCH_GRADE_POBOX_THRESHOLD = i; break;
                    case "APPLY_MATCH_GRADE_TO_LCM":
                        CleanseMatchSettingsModel.APPLY_MATCH_GRADE_TO_LCM = i; break;
                    case "BATCH_SIZE":
                        CleanseMatchSettingsModel.BATCH_SIZE = i; break;
                    case "WAIT_TIME_BETWEEN_BATCHES_SECS":
                        CleanseMatchSettingsModel.WAIT_TIME_BETWEEN_BATCHES_SECS = i; break;

                    //new settings page size override settings(MP-322)
                    case "PAGE_SIZE_MATCH_OUTPUT":
                        CleanseMatchSettingsModel.PAGE_SIZE_MATCH_OUTPUT = i; break;
                    case "PAGE_SIZE_ENRICHMENT_OUTPUT":
                        CleanseMatchSettingsModel.PAGE_SIZE_ENRICHMENT_OUTPUT = i; break;
                    case "PAGE_SIZE_MONITORING_OUTPUT":
                        CleanseMatchSettingsModel.PAGE_SIZE_MONITORING_OUTPUT = i; break;
                    case "PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT":
                        CleanseMatchSettingsModel.PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT = i; break;

                    default:
                        break;
                }
            }
        }
        public void SetMatchGradeContent(CleanseMatchSettingsModel model)
        {
            string p1 = "#"; string p2 = "#"; string p3 = "#"; string p4 = "#"; string p5 = "#"; string p6 = "#"; string p7 = "#";

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
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_POBOX_THRESHOLD].SettingValue))
                p6 = "A";    //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)
            if (Convert.ToBoolean(model.Settings[model.MATCH_GRADE_TELEPHONE_THRESHOLD].SettingValue))
                p7 = "A";


            string matchGrade = p1 + p2 + p3 + p4 + p5 + p6 + p7 + "####";
            model.MatchGrade = matchGrade;

        }
        public void Clear(CleanseMatchSettingsModel model)
        {
            //assignee model as empty 
            model.objAutoSetting = new AutoAdditionalAcceptanceCriteriaEntity();
        }
        #endregion

        #region User Delete
        [HttpGet, Authorize, RequestFromSameDomain]
        public ActionResult DeleteUsers(string Parameters)
        {
            int UserId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            UserId = Convert.ToInt32(Parameters);

            UsersEntity model = new UsersEntity();
            model.UserId = UserId;
            ViewBag.UserId = UserId;
            return View(model);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteUserAfterReassign(string Parameters)
        {
            string Message = string.Empty;
            int ReassignToUserId = 0, UserId = 0, ModifiedByUserId = 0;
            bool ChangesByAdminPortal = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                UserId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                ReassignToUserId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
            }
            UsersEntity model = new UsersEntity();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (ModelState.IsValid)
            {
                try
                {
                    ModifiedByUserId = Helper.oUser.UserId;
                    Message = fac.DeleteUserAfterReassign(UserId, ModifiedByUserId, ChangesByAdminPortal, ReassignToUserId);
                    if (string.IsNullOrEmpty(Message))
                    {
                        return Json(new { result = true, message = PortalSettingLang.msgUserDeleted }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { result = false, message = Message }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception)
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}