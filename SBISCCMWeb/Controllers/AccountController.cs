using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace SBISCCMWeb.Controllers
{
    public class AccountController : BaseController
    {
        public void InitializeController(RequestContext context)
        {
            base.Initialize(context);
        }

        #region Properties


        private UserManager<ApplicationUser> _userManager;
        private UserManager<ApplicationUser> UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = new UserManager<ApplicationUser>(new CustomUserSore<ApplicationUser>(this.CurrentClient.ApplicationDBConnectionString));
                    _userManager.EmailService = new EmailService();

                    var dataProtectionProvider = Startup.DataProtectionProvider;
                    _userManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
                    _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager) { AllowOnlyAlphanumericUserNames = false };
                }
                return _userManager;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        #endregion

        #region Login
        [AllowAnonymous, HandleAntiforgeryTokenError]
        public ActionResult Login(string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (Helper.LicenseEnabledDNB)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (Helper.LicenseEnabledOrb)
                {
                    return RedirectToAction("Index", "OIHome");
                }
                return RedirectToAction("Index", "OIHome");
            }
            else
            {
                //Set Return url, client name and logo to the temp variable.
                try
                {
                    string strMessge = StringCipher.Decrypt(returnUrl, General.passPhrase);
                    ModelState.AddModelError("", strMessge);
                }
                catch
                {
                    //Empty catch block to stop from breaking
                }
                Helper.SAMLSSO = false;
                string domain = System.Web.HttpContext.Current.Request.Url.Authority;
                MasterClientApplicationFacade Masfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());
                if (Helper.ApplicationData == null)
                {
                    Helper.ApplicationData = Masfac.GetClientApplicationData(domain);
                }

                Helper.SAMLSSO = Helper.ApplicationData.SAMLSSO;
                Helper.PartnerIdP = Helper.ApplicationData.PartnerIdP;
                SessionHelper.ClientName = CurrentClient.ClientName;
                Helper.ClientLogo = CurrentClient.ClientLogo;
                string MACAddress = GetCookie();
                LoginViewModel model = new LoginViewModel();
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                UserMachineEntity objUserMachineDetails;
                SessionHelper.PredefineUser = false;
                if (MACAddress != null)
                {
                    //get user id form machine address
                    objUserMachineDetails = fac.GetUserMachineDetails(0, MACAddress);
                    if (objUserMachineDetails != null)
                    {
                        //get all user Information form user Id
                        var OUser = fac.GetUserDetailsById(objUserMachineDetails.UserId);
                        if (OUser != null)
                        {
                            //set Email Address
                            model.EmailAddress = OUser.EmailAddress;
                            SessionHelper.PredefineUser = true;
                        }
                    }
                }
                else
                {
                    objUserMachineDetails = null;
                }
                if (objUserMachineDetails != null)
                {
                    model.RememberMe = true;
                }
                if (!string.IsNullOrEmpty(SessionHelper.ModelStateError))
                {
                    ModelState.AddModelError("", SessionHelper.ModelStateError);
                }
                return View(model);
            }
        }

        [HttpPost, ValidateInput(true), AllowAnonymous, ValidateAntiForgeryToken, HandleAntiforgeryTokenError]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Request.Browser.Browser
                    var BrowserName = Request.Browser.Browser;
                    if (Request.UserAgent.IndexOf("Edge") > -1)
                    {
                        BrowserName = "Edge";
                    }

                    #region "Declaration"
                    Helper.oUser = null;
                    string MACAddress = GetCookie();
                    SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);

                    CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string myIP = Helper.GetCurrentIpAddress();
                    // Get user status code for check user is locked, activate or not.
                    string UserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "account locked").Select(x => x.Code).FirstOrDefault());//"101003";
                    string DeleteUserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "account deleted").Select(x => x.Code).FirstOrDefault());//"101004";
                    DataTable dtlastLoginDetail;
                    bool IsValidate = true;
                    #endregion

                    #region "Sync and Get User Detail"

                    #region Code to handle password changed from support portal
                    // Gets user from email address
                    var userByEmail = await UserManager.FindByEmailAsync(model.EmailAddress);
                    if (userByEmail == null)
                    {
                        ModelState.AddModelError("", CommonMessagesLang.msgInvalidUsernamePassword);
                        goto Outer;
                    }
                    if (string.IsNullOrEmpty(userByEmail.PasswordHash) && string.IsNullOrEmpty(userByEmail.SecurityStamp))
                    {
                        // Generates new password
                        ResetPasswordViewModel objGeneratePassword = new ResetPasswordViewModel();
                        objGeneratePassword.UserId = userByEmail.UserId.ToString();
                        objGeneratePassword.Email = userByEmail.EmailAddress;
                        string password = Convert.ToString(userByEmail.EmailAddress.Split('@')[0]) + "@123";
                        objGeneratePassword.Password = password;
                        objGeneratePassword.ConfirmPassword = model.Password;
                        await NewUserResetPassword(objGeneratePassword);
                    }
                    else if (!string.IsNullOrEmpty(userByEmail.PasswordHash) && string.IsNullOrEmpty(userByEmail.SecurityStamp))
                    {
                        // Generates new password
                        ResetPasswordViewModel objGeneratePassword = new ResetPasswordViewModel();
                        objGeneratePassword.UserId = userByEmail.UserId.ToString();
                        objGeneratePassword.Email = userByEmail.EmailAddress;
                        objGeneratePassword.Password = userByEmail.PasswordHash;
                        objGeneratePassword.ConfirmPassword = userByEmail.PasswordHash;
                        await NewUserResetPassword(objGeneratePassword);
                    }

                    #endregion

                    // Validate user and if null than user does not exist.
                    var user = await UserManager.FindAsync(model.EmailAddress, model.Password);
                    Helper.TempEmailAddress = model.EmailAddress;
                    SessionHelper.PredefineUser = model.Predefineuser;
                    Helper.RememberMe = model.RememberMe;

                    #endregion

                    #region "Set Value for User"
                    // Set user information in session for display and further use.
                    if (user != null)
                    {
                        Helper.oUser = fac.GetUserDetailsById(user.UserId);

                        if (Helper.oUser == null)
                        {
                            Helper.oUser.EmailAddress = user.EmailAddress;
                            Helper.oUser.UserId = user.UserId;
                            Helper.oUser.UserFullName = Convert.ToString(user.UserFullName);
                            Helper.oUser.UserName = Convert.ToString(user.UserName);
                        }
                        Helper.Enable2StepUpdate = user.Enable2StepUpdate;
                        Helper.IsApprover = user.IsApprover;
                        Helper.UserType = Convert.ToString(user.UserType);
                        Helper.IsUserLoginFirstTime = user.IsUserLoginFirstTime;
                    }
                    //if user Helper is null than get user details and set Helper
                    if (Helper.oUser == null)
                    {
                        // Get user details by the UserId
                        Helper.oUser = cfac.GetUserByLoginId(model.EmailAddress);
                    }
                    #endregion

                    #region Set Api Token and Application Setting
                    CommonMethod.LicenseSetting(Request.Url.Authority, StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ToString(), General.passPhrase));
                    CommonMethod.GetAPIToken(this.CurrentClient.ApplicationDBConnectionString);
                    CommonMethod.GetThirdPartyAPICredentials(this.CurrentClient.ApplicationDBConnectionString);
                    CommonMethod.GetUXDefaultUXEnrichments(this.CurrentClient.ApplicationDBConnectionString);
                    #endregion

                    #region User Remember
                    //if User uncheck the remember me check box than delete the machine details from the database if exits
                    UserMachineEntity objUserEntity;
                    if (Helper.oUser != null && MACAddress != null)
                    {
                        objUserEntity = fac.GetUserMachineDetails(Helper.oUser.UserId, MACAddress);
                    }
                    else
                    {
                        objUserEntity = null;
                    }
                    // if the user uncheck the Remember me than remove cookies and also remove from the database
                    if (objUserEntity != null && !model.RememberMe)
                    {
                        //delete user Machine Details from database and Cookies 
                        fac.DelereUserMachineDetails(Helper.oUser.UserId, Convert.ToString(BrowserName));
                        Request.Cookies.Remove("RememberMachine");
                        MACAddress = GetCookie();
                        SessionHelper.RedirectUrl = "https://login.matchbookservices.com/cookie.php?type=delete";
                    }
                    #endregion

                    #region "Validate User is already login or suspended"

                    // Validate user is not locked and if locked than check if user is already login from the different location or IP with same credential.
                    if (Helper.oUser != null)
                    {
                        if (Helper.oUser.UserStatusCode == DeleteUserStatusCode)
                        {
                            ModelState.AddModelError("", CommonMessagesLang.msgInvalidUsernamePassword);
                            goto Outer;
                        }

                        if (Helper.oUser.UserStatusCode != UserStatusCode)
                        {
                            // Validate is User is already login from other location.
                            dtlastLoginDetail = fac.GetUserLoginActivity(Helper.oUser.UserId);
                            if (dtlastLoginDetail != null && dtlastLoginDetail.Rows.Count > 0)
                            {
                                // If user is logout from the different location than continue the process for login otherwise validate network or ip address.
                                int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "logout successfull").Select(x => x.Code).FirstOrDefault());
                                if (Convert.ToInt32(dtlastLoginDetail.Rows[0]["LoginStatus"]) != LogInStatus && !string.IsNullOrEmpty(Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"])))
                                {
                                    if (Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"]) != myIP)
                                    {
                                        ModelState.AddModelError("", CommonMessagesLang.msgUserAlreadyLoggedInAnotherLocation);
                                        goto Outer;
                                    }
                                    else if (Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"]) == myIP && Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserToken"]) != GetCookie())
                                    {
                                        if (!string.IsNullOrEmpty(GetCookie()))
                                        {
                                            if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                            {
                                                ModelState.AddModelError("", CommonMessagesLang.msgUserAlreadyLoggedIn);
                                                goto Outer;
                                            }
                                            else
                                            {
                                                ModelState.AddModelError("", CommonMessagesLang.msgUserAlreadyLoggedInAnotherLocation);
                                                goto Outer;
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                            {
                                                ModelState.AddModelError("", CommonMessagesLang.msgUserAlreadyLoggedIn);
                                                goto Outer;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                        {
                                            ModelState.AddModelError("", CommonMessagesLang.msgUserAlreadyLoggedIn);
                                            goto Outer;
                                        }
                                    }

                                }
                            }
                        }
                        else if (user != null && Helper.oUser.UserStatusCode == UserStatusCode)
                        {
                            // if new user enter password lock and attempt 5 time than account suspended and display message.
                            if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                            {
                                ModelState.AddModelError("", CommonMessagesLang.msgAccountSuspendedTemporarily);
                                goto Outer;
                            }
                            // If account is suspended than redirect Security changes page for enter security Question answer and active user account.
                            if (Helper.oUser.UserStatusCode != UserStatusCode)
                            {
                                return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(model.EmailAddress + "@#$" + model.RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your account is temporarily suspended");
                                return RedirectToAction("Login", "Account", new { returnUrl = "5ESfkrYrwiMHaylFISM9kyonTHKl2R0F8isYqYZbrpnHjIuiwtSJek5tU1xI46BB" });
                            }
                        }
                        else
                        {
                            if (Helper.oUser.UserStatusCode == UserStatusCode)
                            {
                                // if new user enter password lock and attempt 5 time than account suspended and display message. 
                                if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                                {
                                    ModelState.AddModelError("", CommonMessagesLang.msgAccountSuspendedTemporarily);
                                    goto Outer;
                                }
                                else
                                {
                                    // If account is suspended than redirect Security changes page for enter security Question answer and active user account.
                                    SendSecurityLockMail(model.EmailAddress);
                                    ViewBag.EmailAddress = StringCipher.Encrypt(model.EmailAddress, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==");
                                    return View("AccountSuspended");
                                }
                            }
                            else if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                            {
                                // If new user come from login mean first time login than we redirect to security changes page for enter security Question and answer for reactive user.
                                return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(model.EmailAddress + "@#$" + model.RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                            }
                            if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                            {
                                ModelState.AddModelError("", CommonMessagesLang.msgAccountSuspendedTemporarily);
                                goto Outer;
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your account is temporarily suspended.");
                                goto Outer;
                            }

                        }
                    }
                    #endregion

                    #region "Get User Detail and validate"
                    if (user != null)
                    {
                        user.UserName = user.UserFullName;
                        Helper.UserType = user.UserType;
                        await SignInAsync(user, false);
                        Helper.UserId = user.UserId;
                        if (user.IsUserLoginFirstTime)
                        {
                            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                            var oldpassword = Convert.ToString(model.EmailAddress.Split('@')[0]) + "@123";
                            if (model.Password == oldpassword)
                            {
                                return RedirectToAction("ResetPassword", "Account", new { code = code });
                            }
                            else
                            {
                                Random r = new Random();
                                //create random code for Verification send in mail
                                string VerificationCode = Convert.ToString(r.Next(1001, 9999));
                                cfac.updateUserLoginFirstTime(user.EmailAddress, user.IsUserLoginFirstTime, VerificationCode);
                                string emailBody = string.Empty;
                                if (Helper.Branding == Branding.Matchbook.ToString())
                                {
                                    emailBody += "Hi " + user.UserName + "," + "<br/><br/>";
                                    emailBody += "Your verification code: " + VerificationCode + "<br/><br/>";
                                    emailBody += MessageCollection.MatchbookEmailText + " <br/><br/>";
                                    emailBody += "Sincerely,<br/>";
                                    emailBody += MessageCollection.MatchbookSupport;
                                }
                                else if (Helper.Branding == Branding.DandB.ToString())
                                {
                                    emailBody += "Hi " + user.UserName + "," + "<br/><br/>";
                                    emailBody += "Your verification code: " + VerificationCode + "<br/><br/>";
                                    emailBody += MessageCollection.DandBEmailText + " <br/><br/>";
                                    emailBody += "Sincerely,<br/>";
                                    emailBody += MessageCollection.DandBSupport;
                                }
                                Helper.SendMail(user.EmailAddress, " One time verification code.", emailBody);
                                return RedirectToAction("EmailVerification", "Account", new { parameters = StringCipher.Encrypt(Convert.ToString(Helper.oUser.UserId), General.passPhrase) });
                            }
                        }
                        if (Helper.oUser != null)
                        {
                            //if get user detail mean loginId and password both are current just enter attempt in data base.
                            if (Helper.oUser.UserStatusCode != UserStatusCode)
                            {
                                int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                                cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
                                // On fifth time if user enter wrong user name or password than redirect to Account suspended page and mail to user with activation link
                                if (!string.IsNullOrEmpty(model.EmailAddress) && Helper.oUser.UserStatusCode == UserStatusCode)
                                {
                                    if (Helper.oUser.AttemptCount < 6)
                                    {
                                        SendSecurityLockMail(model.EmailAddress);
                                    }
                                    ViewBag.EmailAddress = StringCipher.Encrypt(model.EmailAddress, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==");
                                    return View("AccountSuspended");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", CommonMessagesLang.msgAccountSuspended);
                                goto Outer;
                            }
                            // If new user come from login mean first time login than we redirect to security changes page for enter security Question and answer for reactive user.
                            if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                            {
                                return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(model.EmailAddress + "@#$" + model.RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                            }
                        }
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            if (objUserEntity != null)
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(model.EmailAddress + "@#$" + model.RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                            }
                        }
                        if (objUserEntity != null)
                        {
                            int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                            cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
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
                            SessionHelper.NotRemember = false;
                            return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(model.EmailAddress + "@#$" + model.RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                        }
                    }
                    else
                    {
                        if (Helper.oUser != null)
                        {
                            // if user is locked than just enter attempt in database and if attempt is more than 5 time than redirect to security Question Answer page.
                            if (Helper.oUser.UserStatusCode != UserStatusCode)
                            {
                                int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login fail").Select(x => x.Code).FirstOrDefault());
                                cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
                                //On fifth time if user enter wrong user name or password than redirect to Account suspended page and mail to user with activation link
                                if (Helper.oUser.UserStatusCode == UserStatusCode)
                                {
                                    if (Helper.oUser.AttemptCount < 6)
                                    {
                                        SendSecurityLockMail(model.EmailAddress);
                                    }
                                    ViewBag.EmailAddress = StringCipher.Encrypt(model.EmailAddress, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==");
                                    return View("AccountSuspended");
                                }
                            }
                            else
                            {
                                ModelState.AddModelError("", CommonMessagesLang.msgAccountSuspended);
                                IsValidate = false;
                            }
                        }
                        else
                        {
                            // Return Proper message while User is not identify
                            ModelState.AddModelError("", CommonMessagesLang.msgInvalidUsernamePassword);
                            IsValidate = false;
                        }

                        if (IsValidate)
                        {
                            // Return Proper message while User is not identify
                            ModelState.AddModelError("", CommonMessagesLang.msgInvalidUsernamePassword);
                        }
                    }
                    #endregion

                }
            Outer:
                ViewBag.ClientName = CurrentClient.ClientName;
                Helper.ClientLogo = CurrentClient.ClientLogo;
                SessionHelper.PredefineUser = model.Predefineuser;
            }
            catch (HttpAntiForgeryException)
            {
                return View(model);
            }
            catch (Exception)
            {
                return View(model);
            }
            return View(model);
        }
        #endregion

        #region Logout

        [Authorize]
        public ActionResult LogOff()
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            Helper.UserName = string.IsNullOrEmpty(Helper.UserName) ? Convert.ToString(User.Identity.GetUserName()) : Helper.UserName;

            CompanyFacade cFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            // SignOut user from the application.
            AuthenticationManager.SignOut();

            // Checks if the branding type is Matchbook 
            string RedirectUrl = "";
            if (Helper.Branding == Branding.Matchbook.ToString())
            {
                // Redirect to Matchbook site page.
                RedirectUrl = Convert.ToString(ConfigurationManager.AppSettings["MatchBookRedirect"]);
            }
            // Checks if the branding type is DandB 
            else if (Helper.Branding == Branding.DandB.ToString())
            {
                // Redirect to DandB site page.
                RedirectUrl = Convert.ToString(ConfigurationManager.AppSettings["DandBRedirect"]);
            }
            // Clear all session. 
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["__RequestVerificationToken"] != null)
            {
                Response.Cookies["__RequestVerificationToken"].Value = string.Empty;
                Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            cFac.StewUserLogOut(Convert.ToInt32(User.Identity.GetUserId()));
            string myIP = Helper.GetCurrentIpAddress();
            // Change status in audit table so if user try to login with same credential with different IP than code allow to do that.
            int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "logout successfull").Select(x => x.Code).FirstOrDefault());
            cFac.InsertuserLoginAudit(Convert.ToInt32(User.Identity.GetUserId()), myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
            return Redirect(RedirectUrl);
        }
        #endregion

        #region Forgot Password
        [AllowAnonymous]
        public ActionResult ForgotPassword(string parameters)
        {
            string email = string.Empty;
            try
            {
                email = StringCipher.Decrypt(parameters, General.passPhrase);
            }
            catch
            {
                email = parameters;
            }
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();
            model.Email = email;
            ViewBag.ClientName = CurrentClient.ClientName;
            return View(model);
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "The user either does not exist or is not confirmed.");
                    return View();
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { code = code, parameters = StringCipher.Encrypt(Convert.ToString(System.DateTime.Now.Ticks), General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") }, protocol: Request.Url.Scheme);
                if (Helper.Branding == Branding.Matchbook.ToString())
                {
                    await UserManager.SendEmailAsync(user.Id, "Reset password", "Hi " + user.UserFullName + "," + " <br/><br/> We have received a request to reset your password. <br/><br/>" + MessageCollection.MatchbookResetPassword + "<br/><br/> To reset your password, please <a href=\"" + callbackUrl + "\">click here.</a> <br/><br/>" + " <br/><br/> Sincerely, <br/> " + MessageCollection.MatchbookSupport);
                }
                else if (Helper.Branding == Branding.DandB.ToString())
                {
                    await UserManager.SendEmailAsync(user.Id, "Reset password", "Hi " + user.UserFullName + "," + " <br/><br/> We have received a request to reset your password. <br/><br/>" + MessageCollection.DandBResetPassword + "<br/><br/> To reset your password, please <a href=\"" + callbackUrl + "\">click here.</a> <br/><br/>" + " <br/><br/> Sincerely, <br/> " + MessageCollection.DandBSupport);
                }

                ViewBag.ClientName = CurrentClient.ClientName;
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            // If we got this far, something failed, redisplay form
            ViewBag.ClientName = CurrentClient.ClientName;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region Reset Password
        [AllowAnonymous]
        public ActionResult ResetPassword(string code, string parameters = null)
        {
            ViewBag.IsValidTime = true;
            if (code == null)
            {
                return View("Error");
            }
            if (parameters != null)
            {
                DateTime dtresult = new DateTime(Convert.ToInt64(StringCipher.Decrypt(parameters.Replace("YqU9WupIOLK82SYeZlpZ2g==", "+"), General.passPhrase)));
                TimeSpan ts = DateTime.Now - dtresult;
                if (ts.Days >= 1)
                {
                    ViewBag.IsValidTime = false;
                }
            }
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken, ValidateInput(true)]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid && CommonMethod.ValidatePassword(model.Password))
            {
                //find user by email address 
                var user = await UserManager.FindByEmailAsync(model.Email);
                // if security code or token empty or null then generate code.
                if (string.IsNullOrEmpty(model.Code) && model.UserId != null)
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(model.UserId.ToString());
                    model.Code = code;
                }
                // if user not found from the database then display message.
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return View();
                }
                if (user.IsUserLoginFirstTime)
                {
                    string password = Convert.ToString(model.Email.Split('@')[0]) + "@123";
                    if (model.Password == password)
                    {
                        ModelState.AddModelError("", "Old password and Current password are same. Please use different password");
                        ViewBag.IsValidTime = true;
                        return View();
                    }
                }
                //reset password.
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);

                if (result.Succeeded)
                {
                    AuthenticationManager.SignOut();
                    // Clear all session. 
                    Session.Clear();
                    Session.Abandon();
                    Session.RemoveAll();
                    if (Request.Cookies["ASP.NET_SessionId"] != null)
                    {
                        Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                        Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
                    }
                    if (Request.Cookies["__RequestVerificationToken"] != null)
                    {
                        Response.Cookies["__RequestVerificationToken"].Value = string.Empty;
                        Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddMonths(-20);
                    }
                    if (user.IsUserLoginFirstTime)
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                else
                {
                    AddErrors(result);
                    return View();
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region Other Methods

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            // Return proper Error message to user
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion

        #region Account Suspended
        public ActionResult AccountSuspended()
        {
            return View();
        }
        #endregion

        #region Email Verfication
        [HttpGet]
        public ActionResult EmailVerification(string parameters)
        {
            Helper.IsTowStepVerification = true;
            //open page of email verification 
            int Id = Convert.ToInt32(StringCipher.Decrypt(parameters, General.passPhrase));
            EmailVerification objVerification = new Models.EmailVerification();
            objVerification.RememberMachine = Helper.RememberMe;
            objVerification.UserId = Convert.ToInt32(Id);
            return View(objVerification);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryToken, ValidateInput(true)]
        public ActionResult EmailVerification(EmailVerification model)
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            // Check the email verification code and also check the user does not enter the security question ans than redirect to user security Que. ans. page
            SettingFacade sfac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            string MacAddress = SetCookie();
            //use for get email verification code 
            Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
            if (model.VerificationCode == Helper.oUser.EmailVerificationCode)
            {
                CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                cfac.updateUserLoginFirstTime(Helper.oUser.EmailAddress, false, Helper.oUser.EmailVerificationCode);
                if (model.RememberMachine)
                {
                    UserMachineEntity objUserMachine = sfac.GetUserMachineDetails(model.UserId, MacAddress);
                    if (objUserMachine == null)
                    {
                        objUserMachine = new UserMachineEntity();
                    }
                    sfac.DelereUserMachineDetails(model.UserId, Convert.ToString(BrowserName));
                    objUserMachine.UserId = model.UserId;
                    objUserMachine.MachineDetails = MacAddress;
                    objUserMachine.CreatedDateTime = DateTime.Now;
                    objUserMachine.LastLoggedinDateTime = DateTime.Now;
                    objUserMachine.BrowserAgent = BrowserName;
                    sfac.InsertUpdateUserMachineDetails(objUserMachine);
                }
                if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                {
                    SessionHelper.NotRemember = true;
                    return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(Helper.oUser.EmailAddress + "@#$" + model.RememberMachine, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                }
                else
                {
                    Helper.IsTowStepVerification = false;
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
            }
            else
            {
                ModelState.AddModelError("", "Invalid verification code.");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult SendEmailVerificationCode(bool IsUserLoginFirstTime = false)
        {
            // Send Email verification code via mail
            SendMailVarificationCode(IsUserLoginFirstTime);
            return RedirectToAction("EmailVerification", "Account", new { parameters = StringCipher.Encrypt(Convert.ToString(Helper.oUser.UserId), General.passPhrase) });
        }

        public void SendMailVarificationCode(bool IsUserLoginFirstTime = false)
        {
            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            Random r = new Random();
            //create random code for Verification send in mail
            string VerificationCode = Convert.ToString(r.Next(1001, 9999));

            cfac.updateUserLoginFirstTime(Helper.oUser.EmailAddress, IsUserLoginFirstTime, VerificationCode);
            string emailBody = string.Empty;
            if (Helper.Branding == Branding.Matchbook.ToString())
            {
                emailBody += "Hi, " + Helper.oUser.UserName + "<br/><br/>";
                emailBody += "Your one time verification code is " + VerificationCode + "<br/><br/>";
                emailBody += MessageCollection.MatchbookEmailText + " <br/><br/>";
                emailBody += "Sincerely,<br/>";
                emailBody += MessageCollection.MatchbookSupport;
            }
            else if (Helper.Branding == Branding.DandB.ToString())
            {
                emailBody += "Hi, " + Helper.oUser.UserName + "<br/><br/>";
                emailBody += "Your one time verification code is " + VerificationCode + "<br/><br/>";
                emailBody += MessageCollection.DandBEmailText + " <br/><br/>";
                emailBody += "Sincerely,<br/>";
                emailBody += MessageCollection.DandBSupport;
            }
            Helper.SendMail(Helper.oUser.EmailAddress, "One time verification code.", emailBody);
        }

        public void SendSecurityLockMail(string EmailAddress)
        {
            // Send mail to user and contain account activation link 
            string strVal = string.Empty;
            try
            {
                strVal = StringCipher.Decrypt(EmailAddress, General.passPhrase);
            }
            catch
            {
                strVal = EmailAddress;
            }

            string Url = Request.Url.Scheme + "://" + Request.Url.Authority + "/Account/ActivateAccount?token=" + StringCipher.Encrypt(Convert.ToString(System.DateTime.Now.Ticks) + "@#$" + strVal, General.passPhrase);
            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            if (!string.IsNullOrEmpty(strVal) && Helper.oUser == null)
            {
                //if user Helper is null than get user details and set Helper
                // Get user details
                Helper.oUser = cfac.GetUserByLoginId(strVal);
            }
            string body = string.Empty;
            if (Helper.Branding == Branding.Matchbook.ToString())
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/FreezedAccountMail.html")))
                {
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", Helper.oUser.UserFullName);
                    body = body.Replace("{url}", Url.Replace("+", "YqU9WupIOLK82SYeZlpZ2g=="));
                }
            }
            else if (Helper.Branding == Branding.DandB.ToString())
            {
                using (StreamReader reader = new StreamReader(Server.MapPath("~/MailTemplate/FreezedAccountMailDandB.html")))
                {
                    body = reader.ReadToEnd();
                    body = body.Replace("{user}", Helper.oUser.UserFullName);
                    body = body.Replace("{url}", Url.Replace("+", "YqU9WupIOLK82SYeZlpZ2g=="));
                }
            }
            if (!string.IsNullOrEmpty(strVal))
            {
                Helper.SendMail(strVal, "Email verification.", body);
            }
        }
        #endregion

        #region Account Activate
        [AllowAnonymous]
        public ActionResult ActivateAccount(string token)
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            //if user account is suspended and then activate the suspended account
            string strVal = StringCipher.Decrypt(token.Replace("YqU9WupIOLK82SYeZlpZ2g==", "+"), General.passPhrase);
            DateTime dtresult = new DateTime(Convert.ToInt64(strVal.Split(new string[] { "@#$" }, StringSplitOptions.None)[0]));
            string EmailAddress = Convert.ToString(strVal.Split(new string[] { "@#$" }, StringSplitOptions.None)[1]);
            TimeSpan ts = DateTime.Now - dtresult;
            ViewBag.IsValid = false;
            ViewBag.EmailAddress = EmailAddress;

            // Check the time stamp if it is less than 1 day than we activate account
            if (ts.Days < 1)
            {
                ViewBag.IsValid = true;
                CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    //if user Helper is null than get user details and set Helper
                    if (Helper.oUser == null)
                    {
                        // Get user details by the email
                        Helper.oUser = cfac.GetUserByLoginId(EmailAddress);
                    }
                    SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                    Helper.oUser = fac.GetUserDetailsById(Helper.oUser.UserId);
                    Helper.oUser.UserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "active").Select(x => x.Code).FirstOrDefault());//"101001";
                    string Message = fac.InsertOrUpdateUsersDetails(Helper.oUser, Convert.ToInt32(User.Identity.GetUserId()));
                    int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                    string myIP = Helper.GetCurrentIpAddress();
                    cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
                }
            }
            return View();
        }
        #endregion

        #region Set/Get Cookies
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
            return (MachineId == "" ? null : MachineId);
        }
        public string GetCookie()
        {
            // Retrive the cookies
            string MachineId = string.Empty;
            HttpCookie myCookie = Request.Cookies["RememberMachine"];
            if (myCookie != null && !string.IsNullOrEmpty(myCookie.Values["MachineId"]))
            {
                MachineId = myCookie.Values["MachineId"].ToString();
            }
            return (MachineId == "" ? null : MachineId);
        }
        #endregion

        #region Login with Diffrent User
        [AllowAnonymous]
        public ActionResult DiffrentUserLogin()
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            // when User click on Not You User link Just remove cookie and also delete from the database as well
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(GetCookie()))
            {
                fac.DeleteUserMachineRecords(GetCookie(), Convert.ToString(BrowserName));
                Request.Cookies.Remove("RememberMachine");
                SessionHelper.RedirectUrl = "https://login.matchbookservices.com/cookie.php?type=delete";
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Resend mail on account suspended page
        [AllowAnonymous]
        public ActionResult Resendmail(string EmailAddress)
        {
            // Send mail on suspended page
            SendSecurityLockMail(EmailAddress.Replace("YqU9WupIOLK82SYeZlpZ2g==", "+"));
            ViewBag.EmailAddress = EmailAddress;
            SessionHelper.PredefineUser = false;
            return View("AccountSuspended");
        }
        #endregion
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

        #region LicenceExpire
        [AllowAnonymous]
        //Licence Expire on Licence End Date 
        public ActionResult LicenceExpire()
        {
            AuthenticationManager.SignOut();
            Session.Clear();
            Session.Abandon();
            return View();
        }

        #endregion
        [AllowAnonymous]
        public string GetCurrentConnectionString()
        {
            try
            {
                return this.CurrentClient.ApplicationDBConnectionString;
            }
            catch (Exception)
            {

                return "";
            }

        }

        #region "SAML"
        [AllowAnonymous]
        public ActionResult SingleSignOn(bool RememberMe)
        {
            ComponentSpace.SAML2.SAMLController.Configuration = null;
            string LocalServiceProviderConfigurationName = string.Empty;

            if (ComponentSpace.SAML2.SAMLController.Configuration != null)
            {
                LocalServiceProviderConfigurationName = ComponentSpace.SAML2.SAMLController.Configuration.LocalServiceProviderConfiguration.Name;
            }

            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Get SAML Configuration settings 
            DataTable dtSSOSettingData;
            dtSSOSettingData = fac.getSAMLSSOSetting();

            if (dtSSOSettingData != null && dtSSOSettingData.Rows.Count > 0)
            {


                // To login at the service provider, initiate single sign-on to the identity provider (SP-initiated SSO).
                SAMLConfiguration samlConfiguration = new SAMLConfiguration()
                {
                    LocalServiceProviderConfiguration = new LocalServiceProviderConfiguration()
                    {
                        Name = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["SPName"].ToString()) ? dtSSOSettingData.Rows[0]["SPName"].ToString() : ""),
                        AssertionConsumerServiceUrl = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["SPAssertionConsumerServiceUrl"].ToString()) ? dtSSOSettingData.Rows[0]["SPAssertionConsumerServiceUrl"].ToString() : ""),
                        LocalCertificateFile = @Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["SPLocalCertificateFile"].ToString()) ? dtSSOSettingData.Rows[0]["SPLocalCertificateFile"].ToString() : ""),
                        LocalCertificatePassword = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["SPPassword"].ToString()) ? dtSSOSettingData.Rows[0]["SPPassword"].ToString() : "")
                    }
                };

                samlConfiguration.AddPartnerIdentityProvider(
                            new PartnerIdentityProviderConfiguration()
                            {
                                Name = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPName"].ToString()) ? dtSSOSettingData.Rows[0]["IDPName"].ToString() : ""),
                                SignAuthnRequest = Convert.ToBoolean(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPSignAuthnRequest"].ToString()) ? dtSSOSettingData.Rows[0]["IDPSignAuthnRequest"].ToString() : "false"),
                                SingleSignOnServiceUrl = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPSingleSignOnServiceUrl"].ToString()) ? dtSSOSettingData.Rows[0]["IDPSingleSignOnServiceUrl"].ToString() : ""),
                                SingleLogoutServiceUrl = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPSingleLogoutServiceUrl"].ToString()) ? dtSSOSettingData.Rows[0]["IDPSingleLogoutServiceUrl"].ToString() : ""),
                                PartnerCertificateFile = @Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPPartnerCertificateFile"].ToString()) ? dtSSOSettingData.Rows[0]["IDPPartnerCertificateFile"].ToString() : ""),
                            });

                ComponentSpace.SAML2.SAMLController.Configuration = samlConfiguration;
                SessionHelper.IDPSingleLogoutServiceUrl = Convert.ToString(!string.IsNullOrEmpty(dtSSOSettingData.Rows[0]["IDPSingleLogoutServiceUrl"].ToString()) ? dtSSOSettingData.Rows[0]["IDPSingleLogoutServiceUrl"].ToString() : "");

                LocalServiceProviderConfigurationName += ComponentSpace.SAML2.SAMLController.Configuration.LocalServiceProviderConfiguration.Name + " after";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Server.MapPath("~\\Logs\\1.txt"), true))
                {
                    file.WriteLine(LocalServiceProviderConfigurationName);
                }
                SAMLServiceProvider.InitiateSSO(Response, null, Helper.PartnerIdP);
                SessionHelper.RememberMeSAML = RememberMe;
            }
            return new EmptyResult();
        }
        [AllowAnonymous]
        public async Task<ActionResult> SMALLogin(string userName, string returnUrl)
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            //Request.Browser.Browser
            Helper.oUser = null;
            string MACAddress = GetCookie();
            #region "Declaration"
            SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);

            CompanyFacade cfac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            string myIP = Helper.GetCurrentIpAddress();
            string UserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "account locked").Select(x => x.Code).FirstOrDefault());//"101003";
            string DeleteUserStatusCode = Convert.ToString(LoadUserStatus().Where(x => x.Value.ToLower() == "account deleted").Select(x => x.Code).FirstOrDefault());//"101004";
            DataTable dtlastLoginDetail;
            bool RememberMe = SessionHelper.RememberMeSAML;
            #endregion

            #region "Sync and Get User Detail"
            // Validate user and if null than user does not exist.
            var oUser = cfac.GetUserBySAMLUserName(userName);
            ApplicationUser user = new ApplicationUser(oUser);
            Helper.RememberMe = RememberMe;
            #endregion

            #region "Set Value for User"
            // Set user information in session for display and further use.
            if (oUser != null)
            {
                Helper.oUser = fac.GetUserDetailsById(user.UserId);
                if (Helper.oUser == null)
                {
                    Helper.oUser.EmailAddress = user.EmailAddress;
                    Helper.oUser.UserId = user.UserId;
                    Helper.oUser.UserFullName = Convert.ToString(user.UserFullName);
                    Helper.oUser.UserName = Convert.ToString(user.UserName);
                }
                Helper.Enable2StepUpdate = user.Enable2StepUpdate;
                Helper.IsApprover = user.IsApprover;
                Helper.UserType = Convert.ToString(user.UserType);
                Helper.IsUserLoginFirstTime = user.IsUserLoginFirstTime;
            }
            else
            {
                SessionHelper.ModelStateError = CommonMessagesLang.msgInvalidUser;
                goto Outer;
            }

            #endregion

            #region Set Api Token and Application Setting
            CommonMethod.LicenseSetting(Request.Url.Authority, StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ToString(), General.passPhrase));
            CommonMethod.GetAPIToken(this.CurrentClient.ApplicationDBConnectionString);
            CommonMethod.GetThirdPartyAPICredentials(this.CurrentClient.ApplicationDBConnectionString);
            CommonMethod.GetUXDefaultUXEnrichments(this.CurrentClient.ApplicationDBConnectionString);
            #endregion

            #region User Remember
            //if User uncheck the remember me check box than delete the machine details from the database if exits
            UserMachineEntity objUserEntity;
            if (Helper.oUser != null && MACAddress != null)
            {
                objUserEntity = fac.GetUserMachineDetails(Helper.oUser.UserId, MACAddress);
            }
            else
            {
                objUserEntity = null;
            }
            // if the user uncheck the Remember me than remove cookies and also remove from the database
            if (objUserEntity != null && !RememberMe)
            {
                //delete user Machine Details from database and Cookies 
                fac.DelereUserMachineDetails(Helper.oUser.UserId, Convert.ToString(BrowserName));
                Request.Cookies.Remove("RememberMachine");
                SessionHelper.RedirectUrl = "https://login.matchbookservices.com/cookie.php?type=delete";
            }
            #endregion

            #region "Validate User is already login"

            // Validate user is not locked and if locked than check if user is already login from the different location or IP with same credential.
            if (Helper.oUser != null)
            {
                if (Helper.oUser.UserStatusCode == DeleteUserStatusCode)
                {
                    SessionHelper.ModelStateError = CommonMessagesLang.msgInvalidUser;
                    goto Outer;
                }

                if (Helper.oUser.UserStatusCode != UserStatusCode)
                {
                    // Validate is User is already login from other location.
                    dtlastLoginDetail = fac.GetUserLoginActivity(Helper.oUser.UserId);
                    if (dtlastLoginDetail != null && dtlastLoginDetail.Rows.Count > 0)
                    {
                        // If user is logout from the different location than continue the process for login otherwise validate network or ip address.
                        int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "logout successfull").Select(x => x.Code).FirstOrDefault());
                        if (Convert.ToInt32(dtlastLoginDetail.Rows[0]["LoginStatus"]) != LogInStatus && !string.IsNullOrEmpty(Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"])))
                        {
                            string SAMLLogOffUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Url.Action("SAMLLogOff", "Account");
                            SAMLLogOffUrl = "<a href='#' id='copyaddress'>" + SAMLLogOffUrl + "</a>&nbsp;&nbsp;<i class='fa fa-clipboard' id='copylink' title='Please copy this link and paste to logged in browser to logout from your session.'></i>";
                            if (Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"]) != myIP)
                            {
                                SessionHelper.ModelStateError = CommonMessagesLang.msgUserAlreadyLoggedInAnotherLocation;
                                SessionHelper.SamlLogoutLink = SAMLLogOffUrl;
                                goto Outer;
                            }
                            else if (Convert.ToString(dtlastLoginDetail.Rows[0]["IpAddress"]) == myIP && Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserToken"]) != GetCookie())
                            {
                                if (!string.IsNullOrEmpty(GetCookie()))
                                {
                                    if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                    {
                                        SessionHelper.ModelStateError = CommonMessagesLang.msgUserAlreadyLoggedIn;
                                        SessionHelper.SamlLogoutLink = SAMLLogOffUrl;
                                        goto Outer;
                                    }
                                    else
                                    {
                                        SessionHelper.ModelStateError = CommonMessagesLang.msgUserAlreadyLoggedInAnotherLocation;
                                        SessionHelper.SamlLogoutLink = SAMLLogOffUrl;
                                        goto Outer;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                    {
                                        SessionHelper.ModelStateError = CommonMessagesLang.msgUserAlreadyLoggedIn;
                                        SessionHelper.SamlLogoutLink = SAMLLogOffUrl;
                                        goto Outer;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToString(dtlastLoginDetail.Rows[0]["BrowserAgent"]) != BrowserName)
                                {
                                    SessionHelper.ModelStateError = CommonMessagesLang.msgUserAlreadyLoggedIn;
                                    SessionHelper.SamlLogoutLink = SAMLLogOffUrl;
                                    goto Outer;
                                }
                            }
                        }
                    }
                }
                else if (user != null && Helper.oUser != null && Helper.oUser.UserStatusCode == UserStatusCode)
                {
                    // If account is suspended than redirect Security changes page for enter security Question answer and active user account.
                    SessionHelper.ModelStateError = CommonMessagesLang.msgAccountTemporarilySuspended;
                    return RedirectToAction("Login", "Account", new { returnUrl = "5ESfkrYrwiMHaylFISM9kyonTHKl2R0F8isYqYZbrpnHjIuiwtSJek5tU1xI46BB" });
                }
                else
                {
                    if (Helper.oUser.UserStatusCode == UserStatusCode)
                    {
                        // if new user enter password lock and attempt 5 time than account suspended and display message. 
                        if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                        {
                            SessionHelper.ModelStateError = CommonMessagesLang.msgAccountSuspendedTemporarily;
                            goto Outer;
                        }
                        else
                        {
                            // If account is suspended than redirect Security changes page for enter security Question answer and active user account.
                            SendSecurityLockMail(user.EmailAddress);
                            ViewBag.EmailAddress = StringCipher.Encrypt(user.EmailAddress, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==");
                            return View("AccountSuspended");
                        }
                    }
                    else
                    {
                        // If new user come from login mean first time login than we redirect to security changes page for enter security Question and answer for reactive user.
                        if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                        {
                            return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(user.EmailAddress + "@#$" + RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                        }
                    }
                    if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                    {
                        SessionHelper.ModelStateError = CommonMessagesLang.msgAccountSuspendedTemporarily;
                        goto Outer;
                    }
                    else
                    {
                        SessionHelper.ModelStateError = CommonMessagesLang.msgAccountSuspended;
                        goto Outer;
                    }

                }
            }
            #endregion


            #region "Get User Detail and validate"
            if (user != null)
            {
                user.UserName = user.UserFullName;
                Helper.UserType = user.UserType;
                await SignInAsync(user, false);
                Helper.UserId = user.UserId;
                if (Helper.oUser != null)
                {
                    //if get user detail mean loginId and password both are current just enter attempt in data base.
                    if (Helper.oUser.UserStatusCode != UserStatusCode)
                    {
                        int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                        cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
                    }
                    // If new user come from login mean first time login than we redirect to security changes page for enter security Question and answer for reactive user.
                    if (Helper.oUser.SecurityQuestionId <= 0 && string.IsNullOrEmpty(Helper.oUser.SecurityAnswer))
                    {
                        return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(user.EmailAddress + "@#$" + RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                    }
                }
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    if (objUserEntity != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(user.EmailAddress + "@#$" + RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                    }
                }
                if (objUserEntity != null)
                {
                    int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "login successfull").Select(x => x.Code).FirstOrDefault());
                    cfac.InsertuserLoginAudit(Helper.oUser.UserId, myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));
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
                    SessionHelper.NotRemember = true;
                    return RedirectToAction("UserSecurity", "Home", new { parameters = StringCipher.Encrypt(user.EmailAddress + "@#$" + RememberMe, General.passPhrase).Replace("+", "YqU9WupIOLK82SYeZlpZ2g==") });
                }
            }
            else
            {
                SessionHelper.ModelStateError = CommonMessagesLang.msgInvalidUser;
            }
        #endregion

        Outer:
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult SAMLLogOff()
        {
            var BrowserName = Request.Browser.Browser;
            if (Request.UserAgent.IndexOf("Edge") > -1)
            {
                BrowserName = "Edge";
            }
            bool SLOLogout = false;
            if (SAMLServiceProvider.CanSLO())
            {
                // Request logout at the identity provider.
                SAMLServiceProvider.InitiateSLO(Response, null, null, Helper.PartnerIdP);
                SLOLogout = true;
            }

            Helper.UserName = string.IsNullOrEmpty(Helper.UserName) ? Convert.ToString(User.Identity.GetUserName()) : Helper.UserName;
            CompanyFacade cFac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            // SignOut user from the application.
            AuthenticationManager.SignOut();
            // Clear all session. 
            Session.Clear();
            Session.Abandon();

            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["__RequestVerificationToken"] != null)
            {
                Response.Cookies["__RequestVerificationToken"].Value = string.Empty;
                Response.Cookies["__RequestVerificationToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            cFac.StewUserLogOut(Convert.ToInt32(User.Identity.GetUserId()));
            string myIP = Helper.GetCurrentIpAddress();
            // Change status in audit table so if user try to login with same credential with different IP than code allow to do that.
            int LogInStatus = Convert.ToInt32(GetAttributeTypeForLogIn().Where(x => x.Value.ToLower() == "logout successfull").Select(x => x.Code).FirstOrDefault());
            cFac.InsertuserLoginAudit(Convert.ToInt32(User.Identity.GetUserId()), myIP, LogInStatus, GetCookie(), Convert.ToString(BrowserName));

            if (SLOLogout)
            {
                return new EmptyResult();
            }
            else
            {
                // Redirect to Matchbook site page.
                string RedirectUrl = Convert.ToString(ConfigurationManager.AppSettings["MatchBookRedirect"]);
                return Redirect(RedirectUrl);
            }
        }
        #endregion

        #region "Reset password when create user"
        public async Task<Boolean> NewUserResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //find user by email address 
                var user = await UserManager.FindByEmailAsync(model.Email);
                // if security code or token empty or null then generate code.
                if ((model.Code == null || model.Code == "") && model.UserId != null)
                {
                    string code = await UserManager.GeneratePasswordResetTokenAsync(model.UserId.ToString());
                    model.Code = code;
                }
                // if user not found from the database then display message.
                if (user == null)
                {
                    ModelState.AddModelError("", "No user found.");
                    return false;
                }

                //reset password.
                IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Un-Authenticated Logout
        [Authorize]
        //this method execute when user try to login with cookie session or submit a post form with session(Fiddler)
        public ActionResult UnAuthenticatedLogOff()
        {
            // SignOut user from the application.
            AuthenticationManager.SignOut();
            // Checks if the branding type is Matchbook 
            string RedirectUrl = "";
            if (Helper.Branding == Branding.Matchbook.ToString())
            {
                // Redirect to Matchbook site page.
                RedirectUrl = Convert.ToString(ConfigurationManager.AppSettings["MatchBookRedirect"]);
            }
            // Checks if the branding type is DandB 
            else if (Helper.Branding == Branding.DandB.ToString())
            {
                // Redirect to DandB site page.
                RedirectUrl = Convert.ToString(ConfigurationManager.AppSettings["DandBRedirect"]);
            }
            // Clear all session. 
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            return Redirect(RedirectUrl);

        }
        #endregion
    }


}