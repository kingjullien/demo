using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SBISCCMWeb.Infrastructure
{
    public class MultiLanguageControllerActivator : IControllerActivator
    {
        private string FallBackLanguage = "en-US";
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            HttpCookie languageCookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            if (languageCookie != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCookie.Value);
            }
            else
            {
                if (requestContext.HttpContext.Request.UserLanguages != null)
                {
                    FallBackLanguage = requestContext.HttpContext.Request.UserLanguages[0] ?? FallBackLanguage;
                    if (FallBackLanguage.Contains("en"))
                        FallBackLanguage = "en-US";
                    else if (FallBackLanguage.Contains("zh"))
                        FallBackLanguage = "zh-CN";
                    else if (FallBackLanguage.Contains("es"))
                        FallBackLanguage = "es-ES";
                    else
                        FallBackLanguage = "en-US";
                    languageCookie = new HttpCookie("Language");
                    languageCookie.Value = FallBackLanguage;
                    languageCookie.Expires = DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["LanguageCookieExpirationDays"]));
                    languageCookie.SameSite = SameSiteMode.None;
                    languageCookie.Secure = true;
                    languageCookie.HttpOnly = true;
                    requestContext.HttpContext.Response.SetCookie(languageCookie);
                }
                Thread.CurrentThread.CurrentCulture = new CultureInfo(FallBackLanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(FallBackLanguage);
            }

            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}