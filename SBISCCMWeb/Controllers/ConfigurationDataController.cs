using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System.Web.Mvc;
using SBISCCMWeb.Utility;
using System.Data;
using Microsoft.AspNet.Identity;
using SBISCompanyCleanseMatchBusiness.Objects;
using PagedList;
using OfficeOpenXml;
using System.Web;
using System.IO;
using System.Data.SqlClient;
using System.ComponentModel;

namespace SBISCCMWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification]
    public class ConfigurationDataController : BaseController
    {
        #region "Control Load"
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue, string viewType = null)
        {
            #region  pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

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
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            string DeleteUserCode = Convert.ToString(LoadUserStatus(this.CurrentClient.ApplicationDBConnectionString).Where(x => x.Value == "Account Deleted").Select(x => x.Code).FirstOrDefault());
            ViewBag.ValidCreateUser = ValidLimitOfUser(null);
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            model.users = fac.GetUsersListPaging(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();
            TempData.Keep();
            IPagedList<UsersEntity> pglstpagedMonitorProfile = new StaticPagedList<UsersEntity>(model.users.ToList(), currentPageIndex, pageSize, totalCount);
            model.usersPagingList = pglstpagedMonitorProfile;
            if (Request.IsAjaxRequest())
            {
                if (viewType == "CompanyAttribute" || viewType == "DataEnrichment" || viewType == "CountryGroup")
                    return PartialView("_Index", model);
                else
                    return PartialView("_userListPaging", model.usersPagingList);
            }
            return View(model);
        }
        #endregion

        #region "User Data"
        [HttpGet]
        public ActionResult popupConfigData(string Parameters)
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
                model.objUsers = new UsersEntity();
                model.objUsers.UserStatusCode = "101001";
                model.objUsers.UserTypeCode = "102002";
            }
            return PartialView("_popupConfigData", model.objUsers);
        }

        [HttpPost, ValidateInput(true), RequestFromSameDomain, ValidateAntiForgeryToken]
        public async Task<ActionResult> popupConfigData(UsersEntity UsersEntity, string btnConfigDataUser, string Tags)
        {
            // Save User Data while Edit
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (UsersEntity.IsValidSave)
            {
                if (!ValidLimitOfUser(Convert.ToString(UsersEntity.UserId)))
                {
                    if (!IsUserExists(UsersEntity))
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
                                    emailBody += "Hi, " + UsersEntity.UserName + "<br/><br/>";
                                    emailBody += "Your account has been created for Matchbook Services. To log in, please go to matchbookservices.com. Click on login and enter your custom domain " + domain + " and click continue.<br/><br/>";
                                    emailBody += "On the login page, please log in using " + UsersEntity.EmailAddress + " as your login id and " + password + " as your temporary password.<br/><br/>";
                                    emailBody += "If you have any questions, please don’t hesitate to contact Matchbook Services Technical Support team at support@matchbookservices.com<br/><br/>";
                                    emailBody += "Sincerely<br/>";
                                    emailBody += "Matchbook Services Support";

                                    Helper.SendMail(UsersEntity.EmailAddress, "Welcome to Matchbook Services", emailBody);
                                    AccountController account = new AccountController();
                                    account.InitializeController(this.Request.RequestContext);
                                    ResetPasswordViewModel objResetpass = new ResetPasswordViewModel();
                                    objResetpass.UserId = obj.UserId.ToString();
                                    objResetpass.Email = obj.EmailAddress;
                                    objResetpass.Password = password;
                                    objResetpass.ConfirmPassword = password;
                                    await account.ResetPassword(objResetpass);
                                }
                                ViewBag.Message = btnConfigDataUser == "Add" ? MessageCollection.CommanInsertMessage : MessageCollection.UpdateUser;
                            }
                            else
                            {
                                ViewBag.Message = MessageCollection.EmailValidation;
                            }
                        }
                        else
                        {
                            ViewBag.Message = Message;
                        }
                    }
                    else
                    {
                        ViewBag.Message = MessageCollection.DuplicateEmail;
                    }
                }
                else
                {
                    ViewBag.Message = MessageCollection.UserLimit;
                }
            }
            else
            {
                ViewBag.Message = MessageCollection.InvadilState;
            }

            if (btnConfigDataUser == "Add")
            {
                UsersEntity.Tags = "";
                UsersEntity.LOBTag = "";
            }
            var response = string.Format("<input id=\"Messgae\" name=\"Messgae\" type=\"hidden\" value=\"{0}\">", ViewBag.Message);
            return Content(response);
        }
        [HttpPost, RequestFromSameDomain]
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
                Message = MessageCollection.DeleteUser;
            }
            return Json(Message);
        }
        public List<UserStatus> LoadUserStatus(string ConnectionString)
        {
            // Get User Status Code 
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetUserStatus();
        }
        public List<UserStatus> LoadUserTypeCode(string ConnectionString)
        {
            // Get User Type Code 
            SettingFacade fac = new SettingFacade(ConnectionString);
            List<UserStatus> lstUsers = fac.GetUserTypeCode();
            List<UserStatus> lstnewUsers = new List<UserStatus>();
            if (lstUsers != null)
            {
                foreach (var item in lstUsers)
                {
                    if (item.Code != "-1")
                    {
                        lstnewUsers.Add(item);
                    }
                }
            }
            return lstnewUsers;
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
        [HttpGet]
        public ActionResult ResetPassword(int UserId, string EmailAddress)
        {
            // Open Reset password popup
            ViewBag.UserId = Convert.ToString(UserId);
            ViewBag.EmailAddress = Convert.ToString(EmailAddress);
            return PartialView("_PopUpResetPassword");
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public async Task<ActionResult> ResetPassword(string UserId, string EmailAddress, string PasswordHash)
        {
            // Save Reset password for reset password popup
            if (!string.IsNullOrEmpty(EmailAddress))
            {
                AccountController account = new AccountController();
                account.InitializeController(this.ControllerContext.RequestContext);
                ResetPasswordViewModel objResetpass = new ResetPasswordViewModel();
                objResetpass.UserId = UserId;
                objResetpass.Email = EmailAddress;
                objResetpass.Password = PasswordHash;
                objResetpass.ConfirmPassword = PasswordHash;
                await account.ResetPassword(objResetpass);
                ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){parent.backToparent();});</script>";
            }
            else
            {
                ViewBag.Message = MessageCollection.EmailValidation1;
            }
            return PartialView("_PopUpResetPassword");
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
                        emailBody += "Hi, " + OUser.UserName + "<br/><br/>";
                        emailBody += "Your account is active.<br/><br/>";
                        emailBody += "If you have any questions, please don’t hesitate to contact Matchbook Services Technical Support team at support@matchbookservices.com<br/><br/>";
                        emailBody += "Sincerely,<br/>";
                        emailBody += "Matchbook Services Support";
                        Helper.SendMail(OUser.EmailAddress, "Matchbook account has been activated", emailBody);
                    }
                    Message = MessageCollection.UserActivate;
                }
            }
            else
            {
                Message = MessageCollection.UserLimit;
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
                    if (Helper.LicenseNumberOfUsers == null)
                    {
                        MasterClientApplicationFacade mstrfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());
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

                if (Helper.LicenseNumberOfUsers == null)
                {
                    MasterClientApplicationFacade mstrfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());
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


        //Change Tag selection on LOB tag change
        [ValidateAntiForgeryTokenOnAllPosts, RequestFromAjax, RequestFromSameDomain, HttpPost]
        public JsonResult GetLOBTags(string LOBTag)
        {
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<TagsEntity> lstTag = new List<TagsEntity>();
            if (string.IsNullOrEmpty(LOBTag))
            {
                lstTag = fac.GetAllTags();
            }
            else
            {
                lstTag = fac.GetTagByLOBTag(LOBTag);
            }
            return Json(new { Data = lstTag }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Country Data"
        [HttpGet]
        public ActionResult popupConfigCountryGrp(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Open popup for Country.
            int id = Convert.ToInt32(Parameters);
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (id > 0)
            {
                model.objCountryGroup = fac.GetCountryGroupDetailsById(id);
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
            return PartialView("_popupConfigCountryGrp", model);
        }
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true), RequestFromSameDomain]
        public ActionResult popupConfigCountryGrp(UsersModel model, string btnConfigCountryGroup)
        {
            // Save data for Country.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.countries = fac.GetCountries();
            List<string> lstISOAlpha2Codes = model.objCountryGroup.ISOAlpha2Codes != null ? model.objCountryGroup.ISOAlpha2Codes.Split(',').ToList() : new List<string>();
            string ISOAlpha2Codes = string.Join(",", lstISOAlpha2Codes.Select(f => f.ToString()));
            switch (btnConfigCountryGroup)
            {
                case ">":
                    model.objCountryGroup.lstCountries = new List<CountryEntity>();
                    var obj = model.countries.FirstOrDefault(d => d.ISOAlpha2Code == model.objCountryGroup.AddSelectedCountry);
                    if (obj != null)
                    {
                        model.objCountryGroup.lstCountries.Add(obj);
                        model.countries.Remove(obj);
                    }
                    lstISOAlpha2Codes.Add(model.objCountryGroup.AddSelectedCountry);
                    ISOAlpha2Codes = string.Join(",", lstISOAlpha2Codes.Select(f => f));
                    break;
                case ">>":
                    model.objCountryGroup.lstCountries = fac.GetCountries();
                    ISOAlpha2Codes = string.Join(",", model.objCountryGroup.lstCountries.Select(f => f.ISOAlpha2Code));
                    break;
                case "<":
                    model.objCountryGroup.lstCountries = new List<CountryEntity>();
                    obj = model.countries.FirstOrDefault(d => d.ISOAlpha2Code == model.objCountryGroup.RemoveSelectedCountry);
                    if (obj != null)
                    {
                        model.objCountryGroup.lstCountries.Remove(obj);
                        model.countries.Add(obj);
                    }
                    lstISOAlpha2Codes.Remove(model.objCountryGroup.RemoveSelectedCountry);
                    ISOAlpha2Codes = string.Join(",", lstISOAlpha2Codes.Select(f => f));
                    break;
                case "<<":
                    ISOAlpha2Codes = "";
                    break;
                case "Update":
                case "Add":
                    if (model.objCountryGroup.IsValidSave)
                    {
                        if (!IsGroupExists(model))
                        {
                            fac.InsertOrUpdateCountryGroup(model.objCountryGroup);
                            TempData["CountryMessage"] = model.objCountryGroup.GroupId == 0 ? MessageCollection.CommanInsertMessage : MessageCollection.CommanUpdateMessage;
                        }
                        else
                        {
                            TempData["CountryMessage"] = MessageCollection.GroupNameExist;
                        }
                    }
                    break;
            }
            return RedirectToAction("ReturnpopupConfigCountryGrp", new { ISOAlpha2Codes = ISOAlpha2Codes, GroupId = model.objCountryGroup.GroupId, GroupName = model.objCountryGroup.GroupName });
        }

        public ActionResult ReturnpopupConfigCountryGrp(string ISOAlpha2Codes, int GroupId, string GroupName)
        {
            // Change Group from left to right and right to left in dnb popup
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            List<string> lstISOAlpha2Codes = ISOAlpha2Codes != null ? ISOAlpha2Codes.Split(',').ToList() : null;
            if (GroupId > 0)
            {
                model.objCountryGroup = fac.GetCountryGroupDetailsById(GroupId);
                model.objCountryGroup.tmpName = model.objCountryGroup.GroupName;
            }
            else
            {
                model.objCountryGroup = new CountryGroupEntity();
                model.objCountryGroup.GroupName = GroupName;
            }
            model.objCountryGroup.lstCountries = new List<CountryEntity>();
            model.countries = fac.GetCountries();
            if (lstISOAlpha2Codes != null)
            {
                foreach (var item in lstISOAlpha2Codes)
                {
                    if (!string.IsNullOrEmpty(item))
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
            if (ISOAlpha2Codes != null)
            {
                model.objCountryGroup.ISOAlpha2Codes = ISOAlpha2Codes;
            }
            else
            {
                model.objCountryGroup.ISOAlpha2Codes = "";
            }
            if (TempData["CountryMessage"] != null)
            {
                ViewBag.Message = TempData["CountryMessage"];
                if (GroupId == 0)
                    model.objCountryGroup = new CountryGroupEntity();
                if (Convert.ToString(TempData["CountryMessage"]).Contains("created") || Convert.ToString(TempData["CountryMessage"]).Contains("This Group Name already exist."))
                {
                    model.countries = fac.GetCountries();
                }
            }
            return PartialView("_popupConfigCountryGrp", model);
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
                fac.DeleteCountryGroup(Convert.ToInt32(Parameters));
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
            TempData.Keep();
            return Json(MessageCollection.CommanDeleteMessage);
        }
        // Validate Api Group is Exist or not if exist than display according Message.
        public bool IsGroupExists(UsersModel model)
        {
            //Check country is already exists
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (fac.GetCountryGroupByName(model.objCountryGroup.GroupName) != null)
            {
                if (model.objCountryGroup.tmpName != null)
                {
                    if (model.objCountryGroup.tmpName.Trim().ToLower() == model.objCountryGroup.GroupName.Trim().ToLower())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;
        }

        public ActionResult indexCountryGrp(int? Countrypage, int? Countrysortby, int? Countrysortorder, int? Countrypagevalue)
        {
            //listing Country group for the main screen of configuration
            #region  pagination
            int pageNumber = (Countrypage ?? 1);
            if (!(Countrysortby.HasValue && Countrysortby.Value > 0))
                Countrysortby = 1;

            if (!(Countrysortorder.HasValue && Countrysortorder.Value > 0))
                Countrysortorder = 1;

            int sortParam = int.Parse(Countrysortby.ToString() + Countrysortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = Countrypage.HasValue ? Countrypage.Value : 1;
            int pageSize = Countrypagevalue.HasValue ? Countrypagevalue.Value : 10;
            #endregion
            #region Set Viewbag
            ViewBag.CountrySortBy = Countrysortby;
            ViewBag.CountrySortOrder = Countrysortorder;
            ViewBag.Countrypageno = currentPageIndex;
            ViewBag.Countrypagevalue = pageSize;
            TempData["Countrypageno"] = currentPageIndex;
            TempData["Countrypagevalue"] = pageSize;
            string finalsortOrder = Convert.ToString(Countrysortby) + Convert.ToString(Countrysortorder);
            #endregion

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            // Fill Country Group and listing Country Group
            model.countryGroups = fac.GetCountryGroupList(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount);
            IPagedList<CountryGroupEntity> pglstManageTags = new StaticPagedList<CountryGroupEntity>(model.countryGroups.ToList(), currentPageIndex, pageSize, totalCount);
            TempData.Keep();
            return PartialView("_indexCountryGrp", pglstManageTags);
        }
        #region "Export Data"
        public ActionResult ExportToExcel()
        {
            // Export data to Excel Sheet .
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtCountry = fac.GetCountryGroupDetail();
            string fileName = "CountryGroup_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Country Group");
                worksheet.Cells.LoadFromDataTable(dtCountry, true);
                package.Workbook.Properties.Title = "Country Group";
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
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
                            DirectoryInfo di = Directory.CreateDirectory(directory);
                        }
                        FileInfo oFileInfo = new FileInfo(file.FileName);
                        string fileExtension = oFileInfo.Extension;
                        string fileName = System.DateTime.Now.Ticks + fileExtension;
                        TempData["fileName"] = fileName;
                        path = Path.Combine(directory, Path.GetFileName(fileName));
                        file.SaveAs(path);
                        //excel file is convert in DataTable
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
        public ActionResult CountryDataMatch()
        {
            DataTable dt = new DataTable();
            if (TempData["Data"] != null)
            {
                dt = (TempData["Data"] as DataTable).Copy();
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
            DataTable dtCountryGroup = new DataTable();
            //Get Get Country groups Columns Name
            dtCountryGroup = sfac.GetCountrygroupColumnsName();
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
            TempData.Keep();
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
            if (TempData["Data"] != null)
            {
                dt = (TempData["Data"] as DataTable).Copy();
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
            TempData.Keep();
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
                DataTable dtTempDataNew = new DataTable();
                dtTempDataNew = dt.Select(strRowFilter).CopyToDataTable();
                dt = dtTempDataNew;
                //bulk insert new records
                bool IsDataInsert = BulkInsert(dt, dtColumns, IsOverWrite);
                Message = Convert.ToString(TempData["BulkMessage"]);

            }
            catch (Exception ex)
            {
                Message = MessageCollection.CommanEnableFileImport;
            }
            TempData.Keep();
            return new JsonResult { Data = Message };
        }
        public bool BulkInsert(DataTable dt, DataTable dtColumns, bool IsOverWrite = false)
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
                        TempData["BulkMessage"] = MessageCollection.CommanInsertMessage;
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

        public JsonResult UpdateExamples(string CurrentColumn)
        {
            // While we upload the excel file at that time we display first column value on selection of dropdown
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


        #endregion

        #endregion

        #region "DNBAPI Group"
        [HttpGet]
        public ActionResult popupDnbAPIGrp(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Open Popup for DnbAbi Data.
            int GroupId = Convert.ToInt32(Parameters);
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (GroupId > 0)
            {
                model.objDnbGroupAPI = fac.GetAPIGroupDetailsById(GroupId);
                model.objDnbGroupAPI.tmpName = model.objDnbGroupAPI.APIGroupName;
                model.dnbAPIs = fac.GetDnBAPIList();
                if (model.objDnbGroupAPI.DnbAPIIds != null)
                {
                    model.objDnbGroupAPI.lstDnBApiGrp = new List<DnbAPIEntity>();
                    foreach (var item in model.objDnbGroupAPI.DnbAPIIds.Split(','))
                    {
                        var obj = model.dnbAPIs.FirstOrDefault(d => d.DnBAPIId == Convert.ToInt32(item));
                        if (obj != null)
                        {
                            model.objDnbGroupAPI.lstDnBApiGrp.Add(obj);
                            model.dnbAPIs.Remove(obj);
                        }
                    }
                }
            }
            else
            {
                model.dnbAPIs = fac.GetDnBAPIList();
                model.dnbAPIGroups = new List<DnBAPIGroupEntity>();
                model.objDnbGroupAPI = new DnBAPIGroupEntity();
            }
            return PartialView("_popUpConfigApiGrp", model);
        }

        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult popupDnbAPIGrp(UsersModel model, string btnDnBApiGrp, string Tags)
        {
            model.objDnbGroupAPI.Tags = Tags == "0" ? null : Tags;
            // Save for DnbAbi Data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            model.dnbAPIs = fac.GetDnBAPIList();
            List<string> lstAPIIDS = model.objDnbGroupAPI.DnbAPIIds != null ? model.objDnbGroupAPI.DnbAPIIds.Split(',').ToList() : new List<string>();
            string ApiIds = string.Join(",", lstAPIIDS.Select(f => f.ToString()));
            switch (btnDnBApiGrp)
            {
                case ">":
                    model.objDnbGroupAPI.lstDnBApiGrp = new List<DnbAPIEntity>();
                    var obj = model.dnbAPIs.FirstOrDefault(d => d.DnBAPIId == Convert.ToInt32(model.objDnbGroupAPI.APIIds));
                    if (obj != null)
                    {
                        model.objDnbGroupAPI.lstDnBApiGrp.Add(obj);
                        model.dnbAPIs.Remove(obj);
                    }
                    lstAPIIDS.Add(model.objDnbGroupAPI.APIIds);
                    ApiIds = string.Join(",", lstAPIIDS.Select(f => f));
                    break;
                case ">>":
                    model.objDnbGroupAPI.lstDnBApiGrp = fac.GetDnBAPIList();
                    ApiIds = string.Join(",", model.objDnbGroupAPI.lstDnBApiGrp.Select(f => f.DnBAPIId));
                    break;
                case "<":
                    model.objDnbGroupAPI.lstDnBApiGrp = new List<DnbAPIEntity>();
                    obj = model.dnbAPIs.FirstOrDefault(d => d.DnBAPIId == Convert.ToInt32(model.objDnbGroupAPI.RemoveAPIIds));
                    if (obj != null)
                    {
                        model.objDnbGroupAPI.lstDnBApiGrp.Remove(obj);
                        model.dnbAPIs.Add(obj);
                    }
                    lstAPIIDS.Remove(model.objDnbGroupAPI.RemoveAPIIds);
                    ApiIds = string.Join(",", lstAPIIDS.Select(f => f));
                    break;
                case "<<":
                    ApiIds = "";
                    break;
                case "Update":
                case "Add":
                    if (model.objDnbGroupAPI.IsValidSave)
                    {
                        if (!IsAPIGroupExists(model))
                        {
                            fac.InsertOrUpdateDnBAPIDetail(model.objDnbGroupAPI);
                            TempData["APIMessage"] = model.objDnbGroupAPI.APIGroupId == 0 ? MessageCollection.CommanInsertMessage : MessageCollection.CommanUpdateMessage;
                        }
                        else
                        {
                            TempData["APIMessage"] = MessageCollection.GroupNameExist;
                        }
                        model.objDnbGroupAPI.Tags = btnDnBApiGrp == "Update" ? model.objDnbGroupAPI.Tags : "";
                    }
                    break;
            }

            return RedirectToAction("ReturnpopupConfigApiGrp", new { ApiIds = ApiIds, APIGroupId = model.objDnbGroupAPI.APIGroupId, APIGroupName = model.objDnbGroupAPI.APIGroupName, model.objDnbGroupAPI.Tags });
        }
        public ActionResult ReturnpopupConfigApiGrp(string ApiIds, int APIGroupId, string APIGroupName, string Tags)
        {
            // Change Group from left to right and right to left in dnb popup
            UsersModel model = new UsersModel();
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<string> lstAPIIDS = ApiIds != null ? ApiIds.TrimEnd(',').Split(',').ToList() : null;
            if (APIGroupId > 0)
            {
                model.objDnbGroupAPI = fac.GetAPIGroupDetailsById(APIGroupId);
                model.objDnbGroupAPI.tmpName = model.objDnbGroupAPI.APIGroupName;
            }
            else
            {
                model.objDnbGroupAPI = new DnBAPIGroupEntity();
                model.objDnbGroupAPI.APIGroupName = APIGroupName;
            }
            model.objDnbGroupAPI.lstDnBApiGrp = new List<DnbAPIEntity>();
            model.dnbAPIs = fac.GetDnBAPIList();
            if (lstAPIIDS != null)
            {
                foreach (var item in lstAPIIDS)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var obj = model.dnbAPIs.FirstOrDefault(d => d.DnBAPIId == Convert.ToInt32(item));
                        if (obj != null)
                        {
                            model.objDnbGroupAPI.lstDnBApiGrp.Add(obj);
                            model.dnbAPIs.Remove(obj);
                        }
                    }
                }
            }
            if (ApiIds != null)
            {
                model.objDnbGroupAPI.DnbAPIIds = ApiIds;
            }
            else
            {
                model.objDnbGroupAPI.DnbAPIIds = "";
            }
            if (TempData["APIMessage"] != null)
            {
                ViewBag.Message = TempData["APIMessage"];
                //if (Convert.ToString(TempData["APIMessage"]).Contains("inserted") || model.dnbAPIs.Count == 0)
                if (Convert.ToString(TempData["APIMessage"]).Contains("created") || Convert.ToString(TempData["APIMessage"]).Contains("This Group Name already exist."))
                {
                    model.dnbAPIs = fac.GetDnBAPIList();
                }
                model.dnbAPIGroups = new List<DnBAPIGroupEntity>();
                if (APIGroupId == 0)
                    model.objDnbGroupAPI = new DnBAPIGroupEntity();
            }
            model.objDnbGroupAPI.Tags = Tags;
            return PartialView("_popUpConfigApiGrp", model);
        }
        // Validate Api Group is Exist or not if exist than display according Message.
        public bool IsAPIGroupExists(UsersModel model)
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);

            if (fac.GetDnBAPIGroupList().Exists(d => d.APIGroupName == model.objDnbGroupAPI.APIGroupName.Trim()))
            {
                if (model.objDnbGroupAPI.tmpName != null)
                {
                    if (model.objDnbGroupAPI.tmpName.Trim().ToLower() == model.objDnbGroupAPI.APIGroupName.Trim().ToLower())
                    {
                        return false;
                    }
                }
                return true;
            }
            else
                return false;


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
            return Json(MessageCollection.CommanDeleteMessage);
        }
        public ActionResult indexDataEnrichmentSettings()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            string DeleteUserCode = Convert.ToString(LoadUserStatus(this.CurrentClient.ApplicationDBConnectionString).Where(x => x.Value == "Account Deleted").Select(x => x.Code).FirstOrDefault());
            model.dnbAPIGroups = fac.GetDnBAPIGroupList();
            return PartialView("_indexDataEnrichmentSettings", model);
        }
        #endregion

        #region "Custom Attribute"
        [HttpGet]
        public ActionResult popupConfigCustomAttribute(string Parameters)
        {
            int AttributeId = 0;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // Open popup for Custom attribute.
            AttributeId = Convert.ToInt32(Parameters);
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            if (Convert.ToInt32(Parameters) > 0)
            {
                model.objCustomAttribute = fac.GetCustomAttributeDetailsById(AttributeId);
            }
            else
            {
                model.objCustomAttribute = new CustomAttributeEntity();
            }
            return View(model.objCustomAttribute);
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult popupConfigCustomAttribute(CustomAttributeEntity model)
        {
            // Save Custom attribute 
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                fac.InsertOrUpdateCustomAttributeDetails(model);
                ViewBag.Message = model.AttributeId > 0 ? MessageCollection.CommanUpdateMessage : MessageCollection.CommanInsertMessage;
                model = new CustomAttributeEntity();
            }
            catch (Exception)
            {
                ViewBag.Message = Helper.ErrorMessage;
            }
            var response = string.Format("<input id=\"Messgae\" name=\"Messgae\" type=\"hidden\" value=\"{0}\">", ViewBag.Message);
            return Content(response);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCustomAttribute(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (Parameters != null)
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                // Delete custom Attribute. 
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.DeleteCustomAttribute(Convert.ToInt32(Parameters));
                return Json(MessageCollection.CommanDeleteMessage);
            }
            return Json("");
        }
        public static List<AttributeTypes> GetAttributeType(string ConnectionString)
        {
            // get specific attribute like Taxid , Revenue etc..
            SettingFacade fac = new SettingFacade(ConnectionString);
            List<AttributeTypes> lstAttributeType = new List<AttributeTypes>();
            lstAttributeType = fac.GetAttributeType();
            return lstAttributeType;
        }
        public ActionResult indexCompanyAttribute()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            UsersModel model = new UsersModel();
            string DeleteUserCode = Convert.ToString(LoadUserStatus(this.CurrentClient.ApplicationDBConnectionString).Where(x => x.Value == "Account Deleted").Select(x => x.Code).FirstOrDefault());
            model.objCustomAttributes = fac.GetCustomAttributeList();
            return PartialView("_indexCompanyAttribute", model);
        }
        #endregion

        #region "Reset System Data"
        // Reset System Data
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ResetSystemData()
        {
            // Reset System Data
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.ResetSystemData();
                return new JsonResult { Data = MessageCollection.ResetDataSuccessfully };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = MessageCollection.ResetDataUnsuccessfully };
            }
        }
        // Reset System Data and Configuration
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult ResetAllData()
        {
            // Reset System Data and Configuration
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.ResetAllData();
                return new JsonResult { Data = MessageCollection.ResetDataSuccessfully };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = MessageCollection.ResetDataUnsuccessfully };
            }
        }
        #endregion

        #region Manage Tags
        public ActionResult indexManageTags(int? page, int? sortby, int? sortorder, int? pagevalue, string viewType = null)
        {
            #region  pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

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
            TempData["Tagpageno"] = currentPageIndex;
            TempData["Tagpagevalue"] = pageSize;
            #endregion
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            // Get All tags from the database 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAllTagsListPaging(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();


            IPagedList<TagsEntity> pglstManageTags = new StaticPagedList<TagsEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            return PartialView("_indexManageTags", pglstManageTags);
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
            message = fac.DeleteTag(Convert.ToInt32(Parameters));
            if (message == "")
            {
                message = MessageCollection.CommanDeleteMessage;
                return new JsonResult { Data = message };
            }
            return new JsonResult { Data = message };
        }
        #endregion

        #region "User Comment"
        public ActionResult IndexUserComments(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region  pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["UserCommentspageno"] = currentPageIndex;
            TempData["UserCommentsPageValue"] = pageSize;
            #endregion
            List<UserCommentsEntity> model = new List<UserCommentsEntity>();
            UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetAllUserCommentsListPaging(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();


            IPagedList<UserCommentsEntity> pglstManageTags = new StaticPagedList<UserCommentsEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            return PartialView("_indexUserComments", pglstManageTags);
        }

        [HttpGet]
        public ActionResult popupUserComments(string Parameters)
        {

            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            UserCommentsEntity model = new UserCommentsEntity();
            TempData.Keep();
            return View(model);
        }
        [HttpPost]
        public ActionResult popupUserComments(UserCommentsEntity model, string btnUsersComments)
        {
            // Save user comment to database
            UserCommentsFacade fac = new UserCommentsFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.InsertUserComments(model);
            TempData["UserCommentsMessage"] = model.CommentId == 0 ? MessageCollection.CommanInsertMessage : MessageCollection.CommanUpdateMessage;
            TempData.Keep();
            return View(model);
        }
        #endregion

        #region "Environment"
        public ActionResult indexEnvironment(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region  pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["CDSEnvironmentpageno"] = currentPageIndex;
            TempData["CDSEnvironmentPageValue"] = pageSize;
            #endregion

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetCDSEnvironmentListPaging(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount);
            IPagedList<dynamic> pglstEnvironment = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), currentPageIndex, pageSize, totalCount);
            return PartialView("_indexEnvironment", pglstEnvironment);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCDSEnvironment(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // delete dnb api data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteCDSEnvironment(Convert.ToString(Parameters));
            return Json(MessageCollection.CommanDeleteMessage);
        }
        [HttpGet]
        public ActionResult AddEnvironMent()
        {
            ViewBag.OpenFrom = "Configuration";
            return View("~/Views//Data/AddEnvironMent.cshtml");
        }
        [HttpPost]
        public ActionResult AddEnvironMent(string Environment, string OpenFrom)
        {
            if (!string.IsNullOrEmpty(Environment))
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                sfac.InsertCDSEnvironment(Environment);
                ViewBag.Environment = Environment;
                ViewBag.Message = MessageCollection.CommanInsertMessage;
            }
            ViewBag.OpenFrom = OpenFrom;
            return View("~/Views//Data/AddEnvironMent.cshtml");
        }
        #endregion
        #region "Entity"
        public ActionResult indexEntity(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region  pagination
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 1;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["CDSEntitypageno"] = currentPageIndex;
            TempData["CDSEntityPageValue"] = pageSize;
            #endregion

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dt = fac.GetCDSEntityListPaging(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount);
            IPagedList<dynamic> pglstEntity = new StaticPagedList<dynamic>(dt.AsDynamicEnumerable(), currentPageIndex, pageSize, totalCount);
            return PartialView("_indexEntity", pglstEntity);
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteCDSEntity(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            // delete dnb api data.
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            fac.DeleteCDSEntity(Convert.ToString(Parameters));
            return Json(MessageCollection.CommanDeleteMessage);
        }

        [HttpGet]
        public ActionResult AddEntity()
        {
            ViewBag.OpenFrom = "Configuration";
            return View("~/Views//Data/AddEntity.cshtml");
        }
        [HttpPost]
        public ActionResult AddEntity(string Entity, string OpenFrom)
        {
            if (!string.IsNullOrEmpty(Entity))
            {
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                sfac.InsertCDSEntity(Entity);
                ViewBag.Environment = Entity;
                ViewBag.Message = MessageCollection.CommanInsertMessage;
            }
            ViewBag.OpenFrom = OpenFrom;
            return View("~/Views//Data/AddEntity.cshtml");
        }
        #endregion
    }
}