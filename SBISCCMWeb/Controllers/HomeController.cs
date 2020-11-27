using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Pjax.Mvc5;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
namespace SBISCCMWeb.Controllers
{
    [Authorize, ValidateAccount, TwoStepVerification, DandBLicenseEnabled]
    public class HomeController : BaseController
    {
        public UserManager<ApplicationUser> UserManager { get; private set; }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        public HomeController() : this(new UserManager<ApplicationUser>(new CustomUserSore<ApplicationUser>(""))) { }
        public HomeController(UserManager<ApplicationUser> userManager)
        {

            // make constructor and set user information to user manager.
            var dataProtectionProvider = Startup.DataProtectionProvider;
            userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager) { AllowOnlyAlphanumericUserNames = false };
            userManager.EmailService = new EmailService();
            UserManager = userManager;
        }
        #region "Page Load"
        // GET: Home
        [Authorize]
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index(string Parameters = null)
        {
            Helper.CurrentProvider = ProviderType.DandB.ToString();
            if (!Helper.IsTowStepVerification)
            {
                //if user Helper is null than get user details and set Helper
                if (Helper.oUser == null)
                {
                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    // Get user details by the UserId
                    Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
                }
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
                if (Convert.ToInt32(User.Identity.GetUserId()) > 0)
                {

                    if (Helper.oUser != null)
                    {
                        if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                        {
                            SessionHelper.NotRemember = true;
                            return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(Helper.oUser.EmailAddress + "@#$" + false, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }


                // Set Data to API Usage Statistics
                DataTable dtResult = fac.GetAPIUsageCount();
                Dashboard Model = new Dashboard();

                Model.ApiCount = CommonMethod.FormatNumber(Convert.ToString(dtResult.Rows[0]["CurrentMonthCount"]));
                Model.YTD = CommonMethod.FormatNumber(Convert.ToString(dtResult.Rows[0]["YearToDateCount"]));
                Model.AllCount = CommonMethod.FormatNumber(Convert.ToString(dtResult.Rows[0]["CountSinceInception"]));
                // Set Data to Active Data Queue Statistics to View bag for Display in Dashboard


                Model.ActualApiCount = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["CurrentMonthCount"])) ? Convert.ToInt32(dtResult.Rows[0]["CurrentMonthCount"]).ToString("#,##0") : "0";
                Model.ActualYTD = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["YearToDateCount"])) ? Convert.ToInt32(dtResult.Rows[0]["YearToDateCount"]).ToString("#,##0") : "0";
                Model.ActualAllCount = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["CountSinceInception"])) ? Convert.ToInt32(dtResult.Rows[0]["CountSinceInception"]).ToString("#,##0") : "0";

                DashboardFacade dashboardFac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
                DashboardViewModel dashboardDataQueue = dashboardFac.DashboardV2GetDataQueueStatistics(Helper.oUser.UserId);
                if (dashboardDataQueue != null && dashboardDataQueue.primaryStats != null)
                {
                    Model.ActiveQueueStatus = Convert.ToString(dashboardDataQueue.primaryStats.QueueStatus);

                    Model.ActualInputRecordCount_Total = Convert.ToString(dashboardDataQueue.primaryStats.InputRecordCount_Total);
                    Model.InputRecordCount_Total = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.InputRecordCount_Total));

                    Model.ActualFilesAwaitingImportCount = Convert.ToString(dashboardDataQueue.primaryStats.FilesAwaitingImportCount);
                    Model.FilesAwaitingImportCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.FilesAwaitingImportCount));


                    Model.ActualMatchRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.MatchProcessingRecordCount);
                    Model.MatchRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.MatchProcessingRecordCount));

                    Model.LCMCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.LowConfidenceMatchRecordCount));
                    Model.ActualLCMCount = Convert.ToString(dashboardDataQueue.primaryStats.LowConfidenceMatchRecordCount);

                    Model.ActualBadInputRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.NoMatchRecordCount);
                    Model.BadInputRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.NoMatchRecordCount));

                    Model.ProcessingQueueCnt = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.MatchProcessingRecordCount));

                    Model.EnrichmentQueueCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentProcessingCount));

                    Model.MatchExportRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.MatchExportRecordCount));
                    Model.ActualMatchExportRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.MatchExportRecordCount);

                    Model.EnrichmentExportRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentExportDUNSCount));
                    Model.ActualEnrichmentExportRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentExportDUNSCount);
                }
                if (Request.Headers["X-PJAX"] == "true")
                {
                    this.PjaxFullLoad();
                    return View(Model);
                }
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_index", Model);
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("UserSecurity", "Home", new { parameters = Helper.TowWayVerificationData });
            }
        }
        #endregion

        #region "API Usage Statistics"
        //view report of API Usage Statistics  
        public ActionResult APIUsageStatisticsReport()
        {
            return View();
        }

        //Block Refresh API Usage statistics
        public JsonResult APIUsagestatisticsRefresh()
        {
            string ApiCount = string.Empty, YTD = string.Empty, AllCount = string.Empty, ActualApiCount = string.Empty, ActualYTD = string.Empty, ActualAllCount = string.Empty;
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            DataTable dtResult = fac.GetAPIUsageCount();
            if (dtResult != null && dtResult.Rows.Count != 0)
            {

                ActualApiCount = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["CurrentMonthCount"])) ? Convert.ToInt32(dtResult.Rows[0]["CurrentMonthCount"]).ToString("#,##0") : "0";
                ActualYTD = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["YearToDateCount"])) ? Convert.ToInt32(dtResult.Rows[0]["YearToDateCount"]).ToString("#,##0") : "0";
                ActualAllCount = !string.IsNullOrEmpty(Convert.ToString(dtResult.Rows[0]["CountSinceInception"])) ? Convert.ToInt32(dtResult.Rows[0]["CountSinceInception"]).ToString("#,##0") : "0";
                ApiCount = CommonMethod.FormatNumber(ActualApiCount);
                YTD = CommonMethod.FormatNumber(ActualYTD);
                AllCount = CommonMethod.FormatNumber(ActualAllCount);
            }
            return Json(new
            {
                ApiCount = ApiCount,
                YTD = YTD,
                AllCount = AllCount,
                ActualApiCount = ActualApiCount,
                ActualYTD = ActualYTD,
                ActualAllCount = ActualAllCount
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Profile Details"
        public ActionResult ProfileDetails()
        {
            // Get Profile details and image and display in Profile.            
            Helper.UserName = Convert.ToString(User.Identity.GetUserName());
            UsersEntity model = new UsersEntity();
            ModelState.Remove("Imagepath");

            model = Helper.oUser.Copy();
            var oUser = new UsersEntity();
            if (!string.IsNullOrEmpty(model.EmailAddress))
            {
                oUser = Helper.oUser.Copy();
            }
            else
            {
                oUser = null;
            }
            var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);

            model.SecurityQuestionId = oUser.SecurityQuestionId;
            model.SecurityAnswer = StringCipher.Encrypt("", General.passPhrase);
            if (oUser.SecurityQuestionId > 0)
            {
                model.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == oUser.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
            }
            if (model.Imagepath != "")
            {
                string[] p = model.Imagepath.Split('/');
                string Propicpath = p[3];
                var path = Path.Combine(Server.MapPath("~/Content/UserProfileImage"),
                                                        Path.GetFileName(Propicpath));
                if (!System.IO.File.Exists(path))
                {
                    model.Imagepath = "";
                }
            }
            return View("ProfileDetails", model);
        }
        [OutputCache(Duration = 3600, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult getProfileImage()
        {
            if (Request.UrlReferrer == null)
            {
                return View("Error/PageNotFound");
            }
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            Helper.UserName = Convert.ToString(User.Identity.GetUserName());
            ModelState.Remove("Imagepath");
            //if user Helper is null than get user details and set Helper
            if (Helper.oUser == null)
            {
                // Get user details by the UserId
                Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            }
            if (Helper.oUser != null && !string.IsNullOrEmpty(Helper.oUser.Imagepath))
            {
                string[] p = Helper.oUser.Imagepath.Split('/');
                string thumbpath = "thumb_" + p[3];
                var path = Path.Combine(Server.MapPath("~/Content/UserProfileImage"), Path.GetFileName(thumbpath));
                if (System.IO.File.Exists(path))
                {
                    return File(path, "image/jpg");
                }
                else
                {
                    string _thumbpath = "no-image.jpg";
                    var _path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(_thumbpath));
                    return File(_path, "image/jpg");
                }
            }
            else
            {
                string thumbpath = "no-image.jpg";
                var path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(thumbpath));
                return File(path, "image/jpg");
            }
        }

        [HttpPost, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult RemoveImage(int UserId, string UserName, string Imgpath)
        {
            // remove profile image form the view and also update in database.
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            string imgpath1 = Imgpath.Replace(UserName, "thumb_" + UserName);
            if (System.IO.File.Exists(Server.MapPath(Imgpath)))
            {
                System.IO.File.Delete(Server.MapPath(Imgpath));
                System.IO.File.Delete(Server.MapPath(imgpath1));
                sfac.InsertOrUpdateUsersImage(UserId, null);
            }
            Helper.UserName = Convert.ToString(User.Identity.GetUserName());
            // Get user details by the UserId
            if (Helper.oUser == null)
            {
                // Get user details by the UserId
                Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            }

            if (Helper.oUser.Imagepath != "")
            {
                string[] p = Helper.oUser.Imagepath.Split('/');
                string Propicpath = p[3];
                var path = Path.Combine(Server.MapPath("~/Content/UserProfileImage"),
                                                        Path.GetFileName(Propicpath));
                if (!System.IO.File.Exists(path))
                {
                    Helper.oUser.Imagepath = "";
                }
            }
            var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);
            Helper.oUser.SecurityAnswer = StringCipher.Encrypt("", General.passPhrase);
            if (Helper.oUser.SecurityQuestionId > 0)
            {
                Helper.oUser.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == Helper.oUser.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
            }
            return View("ProfileDetails", Helper.oUser);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult UploadImage(HttpPostedFileBase file, int UserId, string UserName, string Imgpath)
        {
            // upload image to database as well as also display ion view .
            if (file != null && file.ContentLength > 0)
                try
                {
                    FileInfo oFileInfo = new FileInfo(file.FileName);
                    string fileExtension = oFileInfo.Extension;
                    if (CommonMethod.CheckFileType(".jpeg,.jpg,.png,.gif,.bmp", file.FileName.ToLower()))
                    {
                        string dr = Server.MapPath("~/Content/UserProfileImage");
                        if (!Directory.Exists(dr))
                        {
                            Directory.CreateDirectory(dr);
                        }

                        string imgpath1 = Imgpath.Replace(Helper.oUser.UserName.Replace(" ", ""), "thumb_" + Helper.oUser.UserName.Replace(" ", ""));
                        // Also delete previous image from the folder and maintain image from the application as well as database.
                        if (System.IO.File.Exists(Server.MapPath(Imgpath)))
                        {
                            System.IO.File.Delete(Server.MapPath(Imgpath));
                            System.IO.File.Delete(Server.MapPath(imgpath1));
                        }
                        string flName = Helper.oUser.UserName.Replace(" ", "") + "_" + System.DateTime.Now.Ticks + fileExtension;
                        string[] splttik = flName.Split('_')[1].Split('.');
                        string flNameThumb = "thumb_" + Helper.oUser.UserName.Replace(" ", "") + "_" + splttik[0] + fileExtension;
                        string path = Path.Combine(Server.MapPath("~/Content/UserProfileImage"),
                                                   Path.GetFileName(flName));
                        //image save directory
                        file.SaveAs(path);
                        System.Drawing.Image img1 = System.Drawing.Image.FromFile(Server.MapPath("~/Content/UserProfileImage/") + flName);
                        System.Drawing.Image bmp1 = img1.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                        bmp1.Save(Server.MapPath("~/Content/UserProfileImage/") + flNameThumb);
                        bmp1.Dispose();
                        img1.Dispose();
                        SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                        string path1 = "/Content/UserProfileImage/" + flName;
                        fac.InsertOrUpdateUsersImage(UserId, path1);
                        ViewBag.Message = CommonMessagesLang.msgFileUploaded;
                        Helper.oUser.Imagepath = path1;
                        return new JsonResult { Data = CommonMessagesLang.msgSuccess };
                    }
                    else
                    {
                        ViewBag.Message = CommonMessagesLang.msgAllowedFormats;
                        return new JsonResult { Data = CommonMessagesLang.msgWrongFormat };
                    }
                }
                catch (Exception)
                {
                    return new JsonResult { Data = CommonMessagesLang.msgWrongFormat };
                }
            else
            {
                ViewBag.Message = CommonMessagesLang.msgFileNotSpecified;
                return new JsonResult { Data = CommonMessagesLang.msgFailer };

            }
        }
        #region "Reset Password Popup"
        [HttpGet]
        public ActionResult ResetPassword(string Parameters)
        {

            int UserId = 0;
            string EmailAddress = "";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                UserId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                EmailAddress = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }

            // Open reset password popup.
            ViewBag.UserId = Convert.ToString(UserId);
            ViewBag.EmailAddress = Convert.ToString(EmailAddress);
            return PartialView("_PopUpResetPassword");
        }


        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public async Task<ActionResult> ResetPassword(string UserId, string EmailAddress, string PasswordHash)
        {
            if (!string.IsNullOrEmpty(EmailAddress) && CommonMethod.ValidatePassword(PasswordHash))
            {
                // update password with identity and when password reset than logout user and redirect to login page.
                AccountController account = new AccountController();
                account.InitializeController(this.ControllerContext.RequestContext);
                ResetPasswordViewModel objResetpass = new ResetPasswordViewModel();
                objResetpass.UserId = UserId;
                objResetpass.Email = EmailAddress;
                objResetpass.Password = PasswordHash;
                objResetpass.ConfirmPassword = PasswordHash;
                await account.ResetPassword(objResetpass);

                ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){backToparent();});</script>";
                ViewBag.Message = CommonMessagesLang.msgPasswordReset;
            }
            else
            {
                ViewBag.Message = CommonMessagesLang.msgRequiredEmail;
            }
            return PartialView("_PopUpResetPassword");
        }
        #endregion
        #endregion

        #region "BackGround Process Message Display"
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetETLJobStatus()
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dt = fac.GetETLJobStatusMessage();
            return Json(new { Data1 = dt.Rows[0][0], Data2 = dt.Rows[0][1] }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region "Active User Message Display"
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetActiveUserData()
        {
            // Display active user detail is slider of left side bar.
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dt = fac.GetActiveUserData();
            List<UserDetails> lstuser = new List<UserDetails>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                UserDetails objmatchchart = new UserDetails();
                objmatchchart.UserName = dt.Rows[i]["UserName"].ToString();
                objmatchchart.Image_path = "/Image/GetImage?imagepath=" + HttpUtility.UrlEncode(SBISCCMWeb.Utility.Utility.GetEncryptedString(dt.Rows[i]["Image_path"].ToString()));
                lstuser.Add(objmatchchart);
            }
            return Json(lstuser, JsonRequestBehavior.AllowGet);
        }
        #endregion



        [AllowAnonymous]
        public ActionResult UserSecurity(string parameters)
        {
            Helper.IsTowStepVerification = true;
            Helper.TowWayVerificationData = parameters;
            // Decrypted Value and fill the parameter value 
            try
            {
                if (!string.IsNullOrEmpty(parameters))
                {
                    parameters = StringCipher.Decrypt(parameters.Replace("YqU9WupIOLK82SYeZlpZ2g==", "+"), General.passPhrase);
                    string[] strSeparatorValue = Utility.Utility.SplitString(parameters);
                    string EmailAddress = strSeparatorValue[0];
                    bool RememberMe = Convert.ToBoolean(strSeparatorValue[1]);
                    ViewBag.RememberMe = RememberMe;

                    // Get User security Question and answer.
                    SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);
                    var oUser = new UsersEntity();
                    if (!string.IsNullOrEmpty(EmailAddress))
                    {
                        oUser = Helper.oUser.Copy();
                    }
                    else
                    {
                        oUser = null;
                    }
                    //Add Helper.oUser
                    if (Helper.oUser == null)
                    {
                        // Get user details by the UserId
                        Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
                        oUser = Helper.oUser.Copy();
                    }
                    oUser.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == oUser.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
                    oUser.SecurityAnswer = "";
                    ViewBag.NotRemember = SessionHelper.NotRemember;
                    return View(oUser);
                }
                else
                {
                    if (Helper.IsUserLoginFirstTime && Helper.oUser.UserId > 0)
                    {
                        return RedirectToAction("EmailVerification", "Account", new { parameters = StringCipher.Encrypt(Convert.ToString(Helper.oUser.UserId), General.passPhrase) });
                    }
                    return RedirectToAction("LogOff", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("LogOff", "Account");
            }

        }
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, RequestFromSameDomain]
        public async Task<ActionResult> UserSecurity(UsersEntity model)
        {
            // Set User security Question and answer.
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            var user = await UserManager.FindByIdAsync(Convert.ToString(model.UserId));
            //if user Helper is null than get user details and set Helper
            if (Helper.oUser == null)
            {
                // Get user details by the UserId
                Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            }
            Helper.Enable2StepUpdate = Helper.oUser.Enable2StepUpdate;
            Helper.IsApprover = Helper.oUser.IsApprover;
            Helper.UserName = Convert.ToString(Helper.oUser.UserFullName);
            Helper.UserType = Convert.ToString(Helper.oUser.UserType);

            if (Helper.oUser.SecurityQuestionId == 0)
            {
                // When already user fill the security Question-Answer than redirect to home page else select first Security Question Answer.
                if (model.SecurityQuestionId > 0 && !string.IsNullOrEmpty(model.SecurityAnswer))
                {
                    cfac.UpdateSecurityQue(model.UserId, model.SecurityQuestionId, StringCipher.Encrypt(model.SecurityAnswer, General.passPhrase));
                    await SignInAsync(user, false);
                    Helper.IsTowStepVerification = false;
                    Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
                    if (Helper.oUser.EULAAcceptedDateTime == null)
                    {
                        return RedirectToAction("PreEULA", "Home");
                    }
                    else
                    {
                        if (Helper.LicenseEnabledDNB)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (Helper.LicenseEnabledOrb)
                        {
                            return RedirectToAction("Index", "OIHome");
                        }
                    }
                }
                else
                {
                    model.SecurityQuestionId = 0;
                    goto Outer;
                }
            }
            if (!string.IsNullOrEmpty(Helper.oUser.SecurityAnswer) && !string.IsNullOrEmpty(model.SecurityAnswer))
            {
                try
                {
                    Helper.oUser.SecurityAnswer = StringCipher.Decrypt(Helper.oUser.SecurityAnswer, General.passPhrase);
                }
                catch
                {
                    Helper.oUser.SecurityAnswer = Helper.oUser.SecurityAnswer;
                }

                if (Helper.oUser.SecurityAnswer == model.SecurityAnswer)
                {
                    Helper.IsTowStepVerification = false;
                    Helper.oUser.UserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "active").Select(x => x.Code).FirstOrDefault());//"101001";
                    SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    try
                    {
                        fac.InsertOrUpdateUsersDetails(Helper.oUser, Convert.ToInt32(User.Identity.GetUserId()));
                    }
                    catch
                    {
                        //Empty catch block to stop from breaking
                    }

                    await SignInAsync(user, false);
                    string myIP = Helper.GetCurrentIpAddress();
                    int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn(this.CurrentClient.ApplicationDBConnectionString).Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                    cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, "", Convert.ToString(Request.Browser.Browser));
                    string MacAddress = "";
                    if (model.RememberMachineDetails)
                    {

                        MacAddress = SetCookie();
                        SessionHelper.RedirectUrl = "https://login.matchbookservices.com/cookie.php?email=" + Helper.oUser.EmailAddress + "&domain=" + Request.Url.Scheme + "://" + Request.Url.Authority + "&name=" + Helper.UserName + "&days=365" + "&type=add";
                        UserMachineEntity objUserMachine = fac.GetUserMachineDetails(model.UserId, MacAddress);
                        fac.DelereUserMachineDetails(Helper.oUser.UserId, Convert.ToString(Request.Browser.Browser));
                        if (objUserMachine == null)
                        {
                            objUserMachine = new UserMachineEntity();
                        }
                        objUserMachine.UserId = model.UserId;
                        objUserMachine.MachineDetails = MacAddress;
                        objUserMachine.CreatedDateTime = DateTime.Now;
                        objUserMachine.LastLoggedinDateTime = DateTime.Now;
                        objUserMachine.BrowserAgent = Request.Browser.Browser;
                        fac.InsertUpdateUserMachineDetails(objUserMachine);
                    }
                    cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, MacAddress, Convert.ToString(Request.Browser.Browser));
                    if (Helper.oUser.EULAAcceptedDateTime == null)
                    {
                        return RedirectToAction("PreEULA", "Home");
                    }
                    else
                    {
                        if (Helper.LicenseEnabledDNB)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (Helper.LicenseEnabledOrb)
                        {
                            return RedirectToAction("Index", "OIHome");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("IncorrectAnswerError", "Incorrect security answer");
                    goto Outer;
                }
            }
            else
            {
                ModelState.AddModelError("IncorrectAnswerError", "Incorrect security answer");
                goto Outer;
            }
        Outer:
            var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);
            model.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == Helper.oUser.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
            ViewBag.RememberMe = model.RememberMachineDetails;
            return View(model);
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        [HttpGet]
        public ActionResult UpdateSecurityQue()
        {
            var oUser = Helper.oUser.Copy();
            oUser.SecurityAnswer = StringCipher.Encrypt("", General.passPhrase);
            var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);
            if (oUser.SecurityQuestionId > 0)
            {
                oUser.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == oUser.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
            }
            return View("ProfileDetails", oUser);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult UpdateSecurityQue(UsersEntity model)
        {
            //Update Security Question answer for the specific user.
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            ModelState.Remove("UserName");
            ModelState.Remove("UserStatusCode");
            ModelState.Remove("UserTypeCode");
            ModelState.Remove("EmailAddress");
            if (ModelState.IsValid)
            {
                CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                model.SecurityAnswer = StringCipher.Encrypt(model.SecurityAnswer, General.passPhrase);
                cfac.UpdateSecurityQue(model.UserId, model.SecurityQuestionId, model.SecurityAnswer);
                ViewBag.Message = "Security answer updated successfully.";
                ModelState.Clear();
            }
            model = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            Helper.oUser = model.Copy();
            var SecurityQuestions = UsersModel.GetSecurityQuestion(this.CurrentClient.ApplicationDBConnectionString);
            model.SecurityQuestion = Convert.ToString(SecurityQuestions.Where(x => x.SecurityQuestionId == model.SecurityQuestionId).Select(x => x.SecurityQuestion).FirstOrDefault());
            model.SecurityAnswer = StringCipher.Encrypt("", General.passPhrase);
            return View("ProfileDetails", model);
        }

        // Validate File or image exists of the path or not.
        [RequestFromAjax]
        public JsonResult FileIsExist(string file)
        {
            return Json(System.IO.File.Exists(Server.MapPath(file)), JsonRequestBehavior.AllowGet);
        }
        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        #region Encrypted String
        [RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetEncryptedString(string strValue)
        {
            if (!string.IsNullOrEmpty(strValue))
            {
                strValue = StringCipher.Encrypt(strValue, General.passPhrase);
            }
            return new JsonResult { Data = strValue };
        }
        #endregion

        #region Set and Get Cookie
        public string SetCookie()
        {
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("Login", "Account");
            string[] hostParts = new System.Uri(url).Host.Split('.');

            string MachineId = Convert.ToString(DateTime.Now.Ticks);
            //create a cookie
            HttpCookie myCookie = new HttpCookie("RememberMachine");
            myCookie.HttpOnly = true;
            //Add key-values in the cookie
            myCookie.Values.Add("MachineId", MachineId);
            //set cookie expiry date-time. Made it to last for next years.
            myCookie.Expires = DateTime.Now.AddYears(1);
            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);
            return MachineId == "" ? null : MachineId;
        }
        public string GetCookie()
        {
            string MachineId = string.Empty;
            HttpCookie myCookie = Request.Cookies["RememberMachine"];
            if (myCookie != null && !string.IsNullOrEmpty(myCookie.Values["MachineId"]))
            {
                MachineId = myCookie.Values["MachineId"].ToString();
            }
            return MachineId == "" ? null : MachineId;
        }
        #endregion

        #region Get Login User Details and status
        public static List<UserStatus> GetAttributeTypeForLogIn(string ConnectionString)
        {
            SettingFacade fac = new SettingFacade(ConnectionString);
            return fac.GetAttributeTypeForLogIn();
        }
        public List<UserStatus> LoadUserStatus()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            return fac.GetUserStatus();
        }
        public List<UserStatus> GetAttributeTypeForLogIn()
        {
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            return fac.GetAttributeTypeForLogIn();
        }
        #endregion

        #region  Open EULA Popup
        [HttpGet]
        public async Task<ActionResult> PreEULA()
        {
            if (Helper.oUser.EULAAcceptedDateTime == null)
            {
                var user = await UserManager.FindByIdAsync(Convert.ToString(Helper.oUser.UserId));
                if (!Helper.SAMLSSO)//check User is Normal User or SAML USER
                {
                    var UserCheck = await UserManager.CheckPasswordAsync(user, Helper.oUser.EmailAddress.Split('@')[0] + "@123");
                    if (UserCheck)
                    {
                        string code = await UserManager.GeneratePasswordResetTokenAsync(Convert.ToString(user.UserId));
                        return RedirectToAction("ResetPassword", "Account", new { code = code });
                    }
                }
                return View();
            }
            else
            {
                if (Helper.LicenseEnabledDNB)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (Helper.LicenseEnabledOrb)
                {
                    return RedirectToAction("Index", "OIHome");
                }
                return View();
            }

        }
        [HttpGet]
        public ActionResult ValidateEULA()
        {
            //open PreEULA popup
            return View();
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult ValidateEULA(bool IsAccept)
        {
            //validate PreEULA for user
            if (IsAccept)
            {
                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserFullName);
                DateTime dttime = DateTime.Now;
                string myIP = Helper.GetCurrentIpAddress();
                fac.updateUserEULA(dttime, myIP, Helper.oUser.UserId);
                Helper.oUser.EULAAcceptedDateTime = dttime;
                Helper.oUser.EULAAcceptedIPAddress = myIP;
                return Json(new { result = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region  Remove Data From File
        // Removes any record in Data Queue Statistics popup from the UI (right - click UI option)
        [HttpPost, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts, RequestFromSameDomain]
        public JsonResult RemoveDataFromFile(string Parameters)
        {
            int ImportProcessId = 0;

            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ImportProcessId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.oUser.UserName);
            string Message = fac.RemoveFileData(ImportProcessId, Helper.oUser.UserId);
            return new JsonResult { Data = Message };
        }
        #endregion

        public ActionResult RebindUXAuthTokens()
        {
            CommonMethod.GetThirdPartyAPICredentials(this.CurrentClient.ApplicationDBConnectionString);
            return Json(true);
        }
        //ADA Compliance
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult StoreComplianceValue(bool ComplianceReset)
        {
            Helper.LicenseADACompliance = ComplianceReset;
            return new JsonResult { Data = DandBSettingLang.msgSettingUpdate };
        }


        #region "Dashboard V2"
        public ActionResult DataQueueStatistics()
        {
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DashboardViewModel dashboardDataQueue = fac.DashboardV2GetDataQueueStatistics(Helper.oUser.UserId);
            List<DashboardBackgroundProcessStatsEntity> lstStats = fac.DashboardV2GetBackgroundProcessStats(false);
            if (dashboardDataQueue == null)
            {
                dashboardDataQueue = new DashboardViewModel();
            }

            dashboardDataQueue.dashboardInvestigationStatisticsEntity = fac.DashboardV2GetInvestigationStatistics();

            #region "Set new objects"
            if (dashboardDataQueue.primaryStats == null)
            {
                dashboardDataQueue.primaryStats = new PrimaryStatsEntity();
            }
            if (dashboardDataQueue.lowConfidenceMatchStats == null)
            {
                dashboardDataQueue.lowConfidenceMatchStats = new List<LowConfidenceMatchStatsEntity>();
            }
            if (dashboardDataQueue.noMatchStats == null)
            {
                dashboardDataQueue.noMatchStats = new List<NoMatchStatsEntity>();
            }
            if (dashboardDataQueue.matchOutputStats == null)
            {
                dashboardDataQueue.matchOutputStats = new List<MatchOutputStatsEntity>();
            }
            if (dashboardDataQueue.enrichmentProcessingQueueStats == null)
            {
                dashboardDataQueue.enrichmentProcessingQueueStats = new List<EnrichmentProcessingQueueStatsEntity>();
            }
            if (dashboardDataQueue.dataEnrichmentStats == null)
            {
                dashboardDataQueue.dataEnrichmentStats = new List<DataEnrichmentStatsEntity>();
            }
            if (dashboardDataQueue.importProcessTrend == null)
            {
                dashboardDataQueue.importProcessTrend = new List<ImportProcessTrendEntity>();
            }
            if (dashboardDataQueue.dashboardInvestigationStatisticsEntity == null)
            {
                dashboardDataQueue.dashboardInvestigationStatisticsEntity = new DashboardInvestigationStatisticsEntity();
            }
            #endregion

            ViewBag.lstStats = lstStats;
            return PartialView(dashboardDataQueue);
        }
        public ActionResult ImportProcessDataQueueStatistics(string Parameters)
        {
            bool byTag = false;
            List<DashboardImportProcessDataQueueStatisticsEntity> dashboardDataQueue = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                byTag = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }
            if (!byTag)
                dashboardDataQueue = fac.DashboardV2GetDataQueueStatisticsByImportProcess();
            else
                dashboardDataQueue = fac.DashboardV2GetDataQueueStatisticsByTag();

            ViewBag.ByTag = byTag;
            return View(dashboardDataQueue);
        }
        #endregion

        #region "BackgroundProcess"
        public ActionResult BackgroundProcessList()
        {
            return PartialView();
        }
        public ActionResult BackgroundProcessExecutionDetail(int ExecutionId)
        {
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<BackgroundProcessExecutionDetailEntity> lstBackgroundProcessesDetail = fac.DashboardGetBackgroundProcessExecutionDetail(ExecutionId);
            return PartialView(lstBackgroundProcessesDetail);
        }
        #endregion
        [HttpGet]
        public ActionResult ImportProcessQueue(bool ByTags)
        {
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DashboardImportProcessDataQueueStatisticsEntity> dashboardDataQueue = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            if (!ByTags)
                dashboardDataQueue = fac.DashboardV2GetDataQueueStatisticsByImportProcess();
            else
                dashboardDataQueue = fac.DashboardV2GetDataQueueStatisticsByTag();

            if (dashboardDataQueue == null)
            {
                dashboardDataQueue = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            }
            string fileName = "Import Queue Statistics_" + DateTime.Now.Ticks.ToString() + ".csv";
            var lstData = dashboardDataQueue;
            var sb = new StringBuilder();
            List<string> Columnname = new List<string>();
            foreach (PropertyInfo p in typeof(DashboardImportProcessDataQueueStatisticsEntity).GetProperties())
            {
                Columnname.Add(p.Name);
                Columnname.Remove("ImportProcessId");
                Columnname.Remove("ImportDate");
                if (ByTags)
                {
                    Columnname.Remove("ImportProcess");
                    Columnname.Remove("ImportedRowCount");
                }
                else
                {
                    Columnname.Remove("Tag");
                }
            }
            string columnnames = string.Join(",", Columnname);
            sb.AppendLine(columnnames);
            foreach (var data in lstData)
            {
                if (!ByTags)
                    sb.AppendLine(data.ImportProcess + "," + data.ImportedRowCount + "," + data.InputRecordCount + "," + data.DS_LowConfidenceMatchRecordCount + "," + data.DS_NoMatchRecordCount + "," + data.MatchProcessingRecordCount + "," + data.EnrichmentProcessingDUNSCount + "," + data.MatchExportRecordCount + "," + data.EnrichmentExportDUNSCount + "," + data.ArchivalQueueCount + "," + data.DS_RecordsUnderInvestigationCount);
                else
                    sb.AppendLine(data.Tag + "," + data.InputRecordCount + "," + data.DS_LowConfidenceMatchRecordCount + "," + data.DS_NoMatchRecordCount + "," + data.MatchProcessingRecordCount + "," + data.EnrichmentProcessingDUNSCount + "," + data.MatchExportRecordCount + "," + data.EnrichmentExportDUNSCount + "," + data.ArchivalQueueCount + "," + data.DS_RecordsUnderInvestigationCount);
            }
            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", fileName);
        }

        #region "Update Process Setting"
        public JsonResult PauseDataMethod(string Parameters)
        {
            string settingName = string.Empty;
            string settingValue = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    settingName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                    settingValue = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                }
                SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                sfac.UpdateSoloProcessSettings(settingName, settingValue);
                return new JsonResult { Data = DandBSettingLang.msgSettingUpdate };
            }
            catch (Exception)
            {
                return new JsonResult { Data = DandBSettingLang.msgSettingNotUpdate };
            }
        }
        #endregion
        public class ListtoDataTable
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties by using reflection   
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names  
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {

                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
        }

        #region "Filter BackGroundProcess List"
        public JsonResult GetStatusType()
        {
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            lstAllFilter.Add(new DropDownReturn { Value = "FAILED", Text = "FAILED" });
            lstAllFilter.Add(new DropDownReturn { Value = "SUCCESS", Text = "SUCCESS" });
            lstAllFilter.Add(new DropDownReturn { Value = "RUNNING", Text = "RUNNING" });
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetETLTypeDD()
        {
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            List<DashboardBackgroundProcessStatsEntity> lstETLtype = JsonConvert.DeserializeObject<List<DashboardBackgroundProcessStatsEntity>>(SessionHelper.BackgroundProcessStats);
            List<string> lstTypes = lstETLtype.Select(x => x.ETLType).ToList();
            foreach (var item in lstTypes)
            {
                lstAllFilter.Add(new DropDownReturn { Value = item, Text = item });
            }
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDurationHoursDD()
        {
            List<DropDownReturn> lstAllFilter = new List<DropDownReturn>();
            lstAllFilter.Add(new DropDownReturn { Value = "3", Text = "Last 3 hours" });
            lstAllFilter.Add(new DropDownReturn { Value = "6", Text = "Last 6 hours" });
            lstAllFilter.Add(new DropDownReturn { Value = "12", Text = "Last 12 hours" });
            lstAllFilter.Add(new DropDownReturn { Value = "24", Text = "Last 24 hours" });
            lstAllFilter.Add(new DropDownReturn { Value = "48", Text = "Last 48 hours" });
            return Json(new { Data = lstAllFilter }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FilterBackGroundProcess(List<FilterData> filters)
        {
            BackGroundProcessListViewModel listViewModel = new BackGroundProcessListViewModel();
            DashboardFacade fac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            listViewModel = fac.DashboardGetBackgroundProcessList(true);
            try
            {
                foreach (var item in filters)
                {
                    var valLst = item.FilterValue.Split(',').ToList();
                    if (item.Operator == "equalto")
                    {
                        if (item.FieldName == "StatusType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => valLst.Contains(x.ProcessStatus)).ToList();
                        }
                        else if (item.FieldName == "ProcessType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => valLst.Contains(x.ETLType)).ToList();
                            listViewModel.StatsList = listViewModel.StatsList.Where(x => valLst.Contains(x.ETLType)).ToList();
                        }
                        else if (item.FieldName == "TimePeriod")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => x.AuditDateTime >= DateTime.UtcNow.AddHours((Convert.ToInt32(item.FilterValue) * -1))).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !string.IsNullOrEmpty(x.Message) && x.Message.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                    }
                    else if (item.Operator == "notEqualTo")
                    {
                        if (item.FieldName == "StatusType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !valLst.Contains(x.ProcessStatus)).ToList();
                        }
                        else if (item.FieldName == "ProcessType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !valLst.Contains(x.ETLType)).ToList();
                            listViewModel.StatsList = listViewModel.StatsList.Where(x => !valLst.Contains(x.ETLType)).ToList();
                        }
                        else if (item.FieldName == "TimePeriod")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !(x.AuditDateTime >= DateTime.UtcNow.AddHours((Convert.ToInt32(item.FilterValue) * -1)))).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !x.Message.Contains(item.FilterValue)).ToList();
                        }
                    }
                    else if (item.Operator == "contains")
                    {
                        if (item.FieldName == "StatusType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => x.ProcessStatus.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "ProcessType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => x.ETLType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                            listViewModel.StatsList = listViewModel.StatsList.Where(x => x.ETLType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "TimePeriod")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => x.AuditDateTime >= DateTime.UtcNow.AddHours((Convert.ToInt32(item.FilterValue) * -1))).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !string.IsNullOrEmpty(x.Message) && x.Message.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                    }
                    else if (item.Operator == "notContains")
                    {
                        if (item.FieldName == "StatusType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !(x.ProcessStatus.ToLower().Contains(item.FilterValue.ToLower()))).ToList();
                        }
                        else if (item.FieldName == "ProcessType")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !x.ETLType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                            listViewModel.StatsList = listViewModel.StatsList.Where(x => !x.ETLType.ToLower().Contains(item.FilterValue.ToLower())).ToList();
                        }
                        else if (item.FieldName == "TimePeriod")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !(x.AuditDateTime >= DateTime.UtcNow.AddHours((Convert.ToInt32(item.FilterValue) * -1)))).ToList();
                        }
                        else if (item.FieldName == "Message")
                        {
                            listViewModel.ProcessList = listViewModel.ProcessList.Where(x => !x.Message.Contains(item.FilterValue)).ToList();
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            listViewModel.ProcessList.ForEach(x => x.Duration_seconds = x.Duration_ms > 0 ? x.Duration_ms / 1000 : 0);
            return PartialView("_backgroundProcessList", listViewModel);
        }
        #endregion


        #region "Old Dashboard Code"
        #region "Active Data Queue Statistics"
        [RequestFromAjax, RequestFromSameDomain]
        //Filter on Active Data Queue Statistics with Tags and LOBTags
        public JsonResult ActiveStatisticsFilter(string Parameters)
        {
            string LOBTag = string.Empty, Tags = string.Empty;
            // Decrypted Value and fill the parameter value 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Tags = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1).Replace("&#58&#58", "::");
                LOBTag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1).Replace("&#58&#58", "::");
            }

            //Get information of active queue data statistics counts on top left side 
            Dashboard Model = new Dashboard();
            DashboardFacade dashboardFac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DashboardViewModel dashboardDataQueue = dashboardFac.DashboardV2GetDataQueueStatistics(Helper.oUser.UserId);
            if (dashboardDataQueue != null && dashboardDataQueue.primaryStats != null)
            {
                Model.ActiveQueueStatus = Convert.ToString(dashboardDataQueue.primaryStats.QueueStatus);

                Model.ActualInputRecordCount_Total = Convert.ToString(dashboardDataQueue.primaryStats.InputRecordCount_Total);
                Model.InputRecordCount_Total = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.InputRecordCount_Total));

                Model.ActualFilesAwaitingImportCount = Convert.ToString(dashboardDataQueue.primaryStats.FilesAwaitingImportCount);
                Model.FilesAwaitingImportCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.FilesAwaitingImportCount));

                Model.LCMCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.LowConfidenceMatchRecordCount));
                Model.ActualLCMCount = Convert.ToString(dashboardDataQueue.primaryStats.LowConfidenceMatchRecordCount);

                Model.ActualBadInputRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.NoMatchRecordCount);
                Model.BadInputRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.NoMatchRecordCount));

                Model.ProcessingQueueCnt = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.MatchProcessingRecordCount));

                Model.EnrichmentQueueCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentProcessingCount));

                Model.MatchExportRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.MatchExportRecordCount));
                Model.ActualMatchExportRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.MatchExportRecordCount);

                Model.EnrichmentExportRecordCount = CommonMethod.FormatNumber(Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentExportDUNSCount));
                Model.ActualEnrichmentExportRecordCount = Convert.ToString(dashboardDataQueue.primaryStats.EnrichmentExportDUNSCount);
            }
            return Json(new
            {
                ActiveQueueStatus = Model.ActiveQueueStatus,
                ActualInputRecordCount_Total = Model.ActualInputRecordCount_Total,
                InputRecordCount_Total = Model.InputRecordCount_Total,
                ActualFilesAwaitingImportCount = Model.ActualFilesAwaitingImportCount,
                FilesAwaitingImportCount = Model.FilesAwaitingImportCount,
                LCMCount = Model.LCMCount,
                ActualLCMCount = Model.ActualLCMCount,


                ActualBadInputRecordCount = Model.ActualBadInputRecordCount,
                BadInputRecordCount = Model.BadInputRecordCount,
                ProcessingQueueCnt = Model.ProcessingQueueCnt,
                EnrichmentQueueCount = Model.EnrichmentQueueCount,
                MatchExportRecordCount = Model.MatchExportRecordCount,
                ActualMatchExportRecordCount = Model.ActualMatchExportRecordCount,
                EnrichmentExportRecordCount = Model.EnrichmentExportRecordCount,
                ActualEnrichmentExportRecordCount = Model.ActualEnrichmentExportRecordCount,
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region "Background Process Statistics"
        [RequestFromAjax]
        public ActionResult BackgroundProcessStatistics(string Parameters)
        {
            // get information of Background process on Top Center side 

            DashboardFacade Dashboardfac = new DashboardFacade(this.CurrentClient.ApplicationDBConnectionString);
            DashboardBackgroundProcessStatsEntity importDetail = new DashboardBackgroundProcessStatsEntity();
            DashboardBackgroundProcessStatsEntity clansMatchDetail = new DashboardBackgroundProcessStatsEntity();
            DashboardBackgroundProcessStatsEntity EnrichDetail = new DashboardBackgroundProcessStatsEntity();
            string viewString = string.Empty;
            List<DashboardBackgroundProcessStatsEntity> lstStats = Dashboardfac.DashboardV2GetBackgroundProcessStats(false);
            if (lstStats != null && lstStats.Count > 0)
            {
                string result = JsonConvert.SerializeObject(lstStats);
                if (string.IsNullOrEmpty(SessionHelper.BackgroundProcessStats))
                    SessionHelper.BackgroundProcessStats = result;
                importDetail = lstStats.FirstOrDefault(x => x.ETLType == "IMPORT");
                clansMatchDetail = lstStats.FirstOrDefault(x => x.ETLType == "CLEANSE_MATCH");
                EnrichDetail = lstStats.FirstOrDefault(x => x.ETLType == "ENRICHMENT");


                bool IMPORTShowErrorSymbol = importDetail == null ? false : importDetail.ShowErrorSymbol;
                bool CLEANSE_MATCHShowErrorSymbol = clansMatchDetail == null ? false : clansMatchDetail.ShowErrorSymbol;
                bool EnrichmentShowErrorSymbol = EnrichDetail == null ? false : EnrichDetail.ShowErrorSymbol;
                ViewBag.lstStats = lstStats;

                CommonMethod objCommon = new CommonMethod();
                var objResult = objCommon.LoadCleanseMatchSettings(this.CurrentClient.ApplicationDBConnectionString);
                string isPauseFileImportProcessETL = objCommon.GetSettingIDs(objResult, "PAUSE_FILE_IMPORT_PROCESS_ETL");
                ViewBag.isPauseFileImportProcessETL = string.IsNullOrEmpty(isPauseFileImportProcessETL) ? false : Convert.ToBoolean(isPauseFileImportProcessETL);
                viewString = RenderRazorViewToString("BackgroundProcessStatistics", lstStats);
            }
            return Json(new { ImportNextTime = importDetail?.NextExecutionTimeSpan.ToString(), CleanseNextTime = clansMatchDetail?.NextExecutionTimeSpan.ToString(), EnrichNextTime = EnrichDetail?.NextExecutionTimeSpan.ToString(), view = viewString }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region "Low Confidence Match Candidate Statistics"
        public JsonResult GetLowConfidenceMatchStatistics(string Parameters)
        {
            // Get Top  match Records and Display in Chart on Home Page
            string LOBTag = Helper.oUser != null ? Helper.oUser.LOBTag : null;
            string Tag = Helper.oUser != null ? Helper.oUser.Tags : null;

            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Tag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1).Replace("&#58&#58", "::");
                LOBTag = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1).Replace("&#58&#58", "::");
            }
            Tag = Tag == "0" ? "" : Tag;
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataSet ds = fac.DashboardGetLowConfidenceMatchStatistics(LOBTag, Tag, Helper.oUser.UserId);
            string dtStatus = CommonMethod.DataSetToJSON(ds.Tables[0]);
            string dtresult = CommonMethod.DataSetToJSON(ds.Tables[1]);
            return Json(new { Data1 = dtStatus, Data2 = dtresult }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region "Matches By Users"
        [RequestFromAjax]
        public JsonResult GetMatchUserCount(string Parameters)
        {
            int ImportProcessId = 0;
            string LOBTag = Helper.oUser != null ? Helper.oUser.LOBTag : null;
            string Tag = Helper.oUser != null ? Helper.oUser.Tags : null;
            // Decrypted Value and fill the parameter value 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ImportProcessId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
            }
            Tag = Tag == "0" ? "" : Tag;

            //Get match User Count and Display in Chart on Home Page.
            ChartCount objMatch = new ChartCount();
            objMatch.lstMatchUser = new List<MatchUserChart>();
            objMatch.lstMatchConfidenceCode = new List<MatchConfidenceCodeChart>();
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataSet dataset = fac.GetStewardshipStatistics(LOBTag, Tag, Helper.oUser.UserId, ImportProcessId);
            DataTable dtMatch = dataset.Tables[0];
            dtMatch.DefaultView.Sort = "DnBConfidenceCode ASC";
            // set Match User Count data for Chart
            objMatch.lstMatchUser = dtMatch.AsEnumerable().GroupBy(r => r.Field<string>("UserId")).Select(g =>
            {
                MatchUserChart obj = new MatchUserChart();
                obj.name = g.Key;
                obj.y = Convert.ToDecimal(g.Sum(r => r.Field<int>("NumberOfRecs")));
                return obj;
            }).ToList();
            // set Match Confidence Code data for Chart
            objMatch.lstMatchConfidenceCode = dtMatch.AsEnumerable().OrderBy(r => r.Field<int>("DnBConfidenceCode")).GroupBy(r => r.Field<int>("DnBConfidenceCode")).Select(g =>
            {
                MatchConfidenceCodeChart obj = new MatchConfidenceCodeChart();

                obj.x = Convert.ToString(g.Key);
                obj.y = Convert.ToDecimal(g.Sum(r => r.Field<int>("NumberOfRecs")));
                return obj;
            }).ToList();
            string dataStatus = CommonMethod.DataSetToJSON(dataset.Tables[1]);
            return Json(new { objMatch = objMatch, dataStatus = dataStatus }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        public ActionResult ChangeLanguage(string language)
        {
            ChangeCulture(language);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public void ChangeCulture(string lang)
        {
            try
            {
                Response.Cookies.Remove("Language");
                HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
                if (languageCookie == null)
                {
                    languageCookie = new HttpCookie("Language");
                    languageCookie.HttpOnly = true;
                }
                languageCookie.Value = lang;
                languageCookie.Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["LanguageCookieExpirationDays"]));
                languageCookie.SameSite = SameSiteMode.None;
                languageCookie.Secure = true;
                languageCookie.HttpOnly = true;
                Response.SetCookie(languageCookie);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
    }
}