
using SBISCCMWeb.Controllers;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    // Validate if the reuest is came from the same domain or the diffrent domain
    public class RequestFromSameDomain : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if request was came from the outside or the other site at that time we redirect to the 404 page and we are not allow the reuest.
            if (string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Request.UrlReferrer)))
            {

                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Error/PageNotFound",
                    ViewData = filterContext.Controller.ViewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
    }

    // Validate the request is came through ajax call or not.
    public class RequestFromAjax : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if we set the attribute it mean request is ajax request and if not we generate to internal error.
            if (!new HttpRequestWrapper(System.Web.HttpContext.Current.Request).IsAjaxRequest())
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Error/InternalError",
                    ViewData = filterContext.Controller.ViewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
    }
    // Special Validation for  Approve Match data and check the IsApprove and Enable2StepUpdate
    public class AccessApproveMatch : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Helper.oUser != null)
            {
                if (!(Helper.oUser.IsApprover && Helper.oUser.Enable2StepUpdate))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Error/PageNotFound",
                        ViewData = filterContext.Controller.ViewData,
                        TempData = filterContext.Controller.TempData
                    };
                }
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Error/PageNotFound",
                    ViewData = filterContext.Controller.ViewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
    }
    // Validate custom methoad fot hhe antiforgory token for the ajax request.
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            //  Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                //  Ajax POSTs and normal form posts have to be treated differently when it comes
                //  to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;
                    if (request.Headers["__RequestVerificationToken"] != null)
                    {
                        AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                    }
                    else
                    {
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "~/Error/InternalError",
                            ViewData = filterContext.Controller.ViewData,
                            TempData = filterContext.Controller.TempData
                        };
                    }

                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
    public class ValidateAccount : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if request was came from the outside or the other site at that time we redirect to the 404 page and we are not allow the reuest.
            // Get connection string.
            //string GetConnctionstring = Convert.ToString(GetClientConnectionString());// StringCipher.Encrypt(SBISCompanyCleanseMatchBusiness.Objects.General.databaseConnectionString, General.passPhrase);
            //CompanyFacade cfac = new CompanyFacade(GetConnctionstring,Helper.UserName);
            //var oUser = new UsersEntity();
            //if (!string.IsNullOrEmpty(Helper.TempEmailAddress))
            //{
            //    oUser = (Helper.oUser as UsersEntity).Copy();
            //            (Helper.oUser as UsersEntity).Copy();
            //    oUser = cfac.GetUserByLoginId(Helper.TempEmailAddress);
            //}
            //else { oUser = null; }


            string UserStatusCode = "101003";//Convert.ToString(fac.GetUserStatus().Where(x => x.Value.ToLower() == "account locked").Select(x => x.Code).FirstOrDefault());
            if (Helper.oUser == null)
            {
                string GetConnctionstring = Convert.ToString(GetClientConnectionString());// StringCipher.Encrypt(SBISCompanyCleanseMatchBusiness.Objects.General.databaseConnectionString, General.passPhrase);
                CompanyFacade cfac = new CompanyFacade(GetConnctionstring, Helper.UserName);
                if (!string.IsNullOrEmpty(Helper.TempEmailAddress))
                {
                    Helper.oUser = cfac.GetUserByLoginId(Helper.TempEmailAddress);
                }
            }
            if (Helper.oUser != null && Helper.oUser.UserStatusCode == UserStatusCode)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Account/Login",
                    ViewData = filterContext.Controller.ViewData,
                    TempData = filterContext.Controller.TempData
                };
            }
        }
        public string GetClientConnectionString()
        {
            string _connectionString = "";
            MasterClientApplicationFacade Mfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());

            if (Helper.ApplicationData == null)
            {
                Helper.ApplicationData = Mfac.GetClientApplicationData(HttpContext.Current.Request.Url.Authority);
            }
            if (Helper.ApplicationData != null)
            {
                _connectionString = StringCipher.Decrypt(Helper.ApplicationData.ApplicationDBConnectionStringHash, General.passPhrase);
            }


            return _connectionString;
        }
    }

    public class TwoStepVerification : ActionFilterAttribute
    {


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var allowedRoutes = new string[] { "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account" };

            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (Helper.IsTowStepVerification && !allowedRoutes.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "UserSecurity" },
                            { "parameters", Helper.TowWayVerificationData },
                        });
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class AllowLicense : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            /*License for D&B Monitoring 2.0*/
            if (currentController.ToLower() == "dnbmonitoring")
            {
                if (!Helper.LicenseEnableMonitoring || Helper.oUser.UserType.ToLower() == "steward")
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B MonitoringDirectPlus*/
            if (currentController.ToLower() == "dnbmonitoringdirectplus")
            {
                if (!Helper.LicenseEnableDPM || Helper.oUser.UserType.ToLower() == "steward")
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B Investigation*/
            if (currentController.ToLower() == "investigateview")
            {
                if (!Helper.LicenseEnableInvestigations || !Helper.oUser.EnableInvestigations)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }

            /*License for D&B Feature,License,Identity Resolution,Data Enrichment*/
            if (currentController.ToLower() == "dnbfeature" || currentController.ToLower() == "dnbidentityresolution" || currentController.ToLower() == "dnblicence" || currentController.ToLower() == "dnbdataenrichment")
            {
                if ((!Helper.LicenseEnabledDNB || (Helper.oUser.UserType.ToLower() == "steward" && Helper.oUser.EnablePreviewMatchRules)) && (currentAction.ToLower() != "insertupdateautoacceptance" && currentAction.ToLower() != "previewautoacceptance"))
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
                else if (currentController.ToLower() == "dnbidentityresolution" || currentAction.ToLower() != "insertupdateautoacceptance" || currentAction.ToLower() != "previewautoacceptance")
                {
                    if (!Helper.oUser.EnablePreviewMatchRules && !string.IsNullOrEmpty(Helper.UserType) && Convert.ToString(Helper.UserType).ToLower() == "steward")
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                    }
                    else if (!Helper.LicenseEnabledDNB)
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                    }
                }
            }

            /*License for ORB Settings*/
            if (currentController.ToLower() == "oisetting")
            {
                if (!Helper.LicenseEnabledOrb)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B Build A List*/
            if (currentController.ToLower() == "buildlist")
            {
                string APItype = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_BUILD_A_LIST.ToString(), ThirdPartyProperties.APIType.ToString());
                if (!Helper.LicenseBuildAList || APItype.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B Family Tree*/
            if (currentController.ToLower() == "familytree")
            {
                if (!Helper.LicenseEnableFamilyTree)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Index" },
                    });
                }
            }
            /*License for D&B Direct Plus Monitoring Investigation*/
            if (currentController.ToLower() == "dpminvestigation")
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
            }
            /*License for D&B Research Investigation*/
            if (currentController.ToLower() == "researchinvestigation")
            {
                if (!Helper.LicenseEnableInvestigations || !Helper.oUser.EnableInvestigations || Helper.lstThirdPartyAPIs.Where(x => x.Code == "DNB_INVESTIGATIONS").Select(x => x.APIType).FirstOrDefault() != "DirectPlus")
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B Beneficial Owneserhuip*/
            if (currentController.ToLower() == "beneficialownership")
            {
                if (!Helper.LicenseEnableCompliance && !Helper.oUser.EnableCompliance)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B Portal*/
            if (currentController.ToLower() == "portal" && currentAction.ToLower() != "indexaboutus")
            {
                if (Helper.oUser.UserType.ToLower() == "steward")
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            /*License for D&B and ORB Export Data*/
            if (currentController.ToLower() == "exportview" || currentController.ToLower() == "oiexportview")
            {
                if (currentAction == "Index")
                {
                    if ((!Helper.oUser.EnableExportData) && (Helper.oUser.UserType.ToLower() == "steward"))
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                    }
                }
                else if (currentAction == "MonitoringExportindex")
                {
                    if (!Helper.LicenseEnableMonitoring && !Helper.LicenseEnableDPM)
                    {
                        if (Helper.oUser.UserType.ToLower() == "steward" && !Helper.oUser.EnableExportData)
                        {
                            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                            {
                                { "controller", "Home" },
                                { "action", "Index" },
                            });
                        }
                    }
                }
            }
            /*License for Command Line*/
            if (currentController.ToLower() == "commandmapping" && Helper.LicenseEnableCommandLine != null && !Helper.LicenseEnableCommandLine)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
            }
            /*License for Multi-Pass Matching*/
            if(currentController.ToLower() == "multipass" && (!Helper.LicenseEnableMultiPassMatching || !Helper.LicenseEnableTags))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class CheckLicenseStatus : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MasterClientApplicationFacade Mfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());
            DataTable dtCAppData = new DataTable();
            string AppicationSubDomain = HttpContext.Current.Request.Url.Authority;
            List<MasterClientApplicationEntity> lstmstrClientApplication = new List<MasterClientApplicationEntity>();
            var LicenseEndDate = Mfac.GetAllForClients().Where(x => x.AppicationSubDomain.Contains(AppicationSubDomain)).Select(x => x.LicenseEndDate).FirstOrDefault();  // MP-846 Admin database cleanup and code cleanup.-CLIENT

            if (string.IsNullOrEmpty(Convert.ToString(LicenseEndDate)))
            {
                //TimeSpan ts = DateTime.Now - Convert.ToDateTime(LicenseEndDate);
                //if (ts.Days > 0)
                //{
                var rd = filterContext.RequestContext.RouteData;
                string currentAction = rd.GetRequiredString("action");
                string currentController = rd.GetRequiredString("controller");
                if (currentAction != "LicenceExpire")
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "LicenceExpire" },
                        });
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
    public class EULAValidation : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            string[] allowedRoutes = new string[] { "PreEULA|Home", "ValidateEULA|Home", "UpdateSecurityQue|Home", "UserSecurity|Home" };
            if (currentController.ToLower() != "account" && !allowedRoutes.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                // if we set the attribute it mean request is ajax request and if not we generate to internal error.
                if (Helper.oUser.EULAAcceptedDateTime == null)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "PreEULA" },
                        });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
    public class IsGlobalUser : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Helper.AuthError = null;
            if (Helper.oUser != null && Helper.oUser.UserRole != UserRole.GLOBAL.ToString())
            {
                Helper.AuthError = CommonMessagesLang.msgAuthError;
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class OrbLicenseEnabled : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var allowedRoutes = new string[] { "GetEncryptedString|Home", "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account" };
            var allowedProvider = new string[] { "GetEncryptedString|Home", "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account",
                "ExportJobNotification|ExportView","ExportOIJobNotification|OIExportView" ,"ImportJobNotification|Data","OIImportJobNotification|OIData"};
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (!Helper.LicenseEnabledOrb && !allowedRoutes.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()) || Helper.Branding == Branding.DandB.ToString())
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" },
                        });
            }
            //Set Current Provider for display menu based on Provider OI or D&B
            if (!allowedProvider.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                Helper.CurrentProvider = ProviderType.OI.ToString();
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class DandBLicenseEnabled : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var allowedRoutes = new string[] { "GetEncryptedString|Home", "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account" };
            var allowedProvider = new string[] { "GetEncryptedString|Home", "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account",
                "ExportJobNotification|ExportView","ExportOIJobNotification|OIExportView" ,"ImportJobNotification|Data","OIImportJobNotification|OIData","GetImage|Image"};
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (!Helper.LicenseEnabledDNB && !allowedRoutes.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" },
                        });
            }

            //Set Current Provider for display menu based on Provider OI or D&B
            if (!allowedProvider.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                Helper.CurrentProvider = ProviderType.DandB.ToString();
            }
            base.OnActionExecuting(filterContext);
        }

    }
    public class DPMLicenseEnabled : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var allowedRoutes = new string[] { "GetEncryptedString|Home", "UserSecurity|Home", "BackgroundProcessStatistics|Home", "GetETLJobStatus|Home", "GetActiveUserData|Home", "getProfileImage|Home", "GetImage|Home", "GetBlobImage|Home", "EmailVerification|Account" };
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");

            if (!Helper.LicenseEnableDPM && !allowedRoutes.Any(x => x.ToLowerInvariant() == (currentAction + "|" + currentController).ToLowerInvariant()))
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Account" },
                            { "action", "Login" },
                        });
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class AllowDataStewardshipLicense : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            if (currentController.ToLower() == "review")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            if (currentController.ToLower() == "stewardshipportal")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }

            if (currentController.ToLower() == "badinputdata")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            if (currentController.ToLower() == "previewmatchdata")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            if (currentController.ToLower() == "familytree")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    if (Helper.LicenseEnableFamilyTree)
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Index" },
                    });
                    }
                }
            }
            if (currentController.ToLower() == "searchdata")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            if (currentController.ToLower() == "buildlist")
            {
                if (!Helper.LicenseEnableDataStewardship)
                {
                    string APIType = CommonMethod.GetThirdPartyProperty(ThirdPartyCode.DNB_BUILD_A_LIST.ToString(), ThirdPartyProperties.APIType.ToString());
                    if (Helper.LicenseBuildAList || APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower())
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class BrandingLicense : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            if (currentController.ToLower() == "ticket")
            {
                if (Helper.Branding != Branding.Matchbook.ToString())
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
    public class AllowLicenseType : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rd = filterContext.RequestContext.RouteData;
            string currentAction = rd.GetRequiredString("action");
            string currentController = rd.GetRequiredString("controller");
            if (currentController.ToLower() == "dandb")
            {
                if (Helper.LicenseSKU == ConfigurationManager.AppSettings["LicenseSKU"])
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            if (currentController.ToLower() == "portal")
            {
                if (Helper.LicenseSKU == ConfigurationManager.AppSettings["LicenseSKU"])
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "Index" },
                        });
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class HandleAntiforgeryTokenError : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary(new { action = "Login", controller = "Account" }));
        }
    }
}