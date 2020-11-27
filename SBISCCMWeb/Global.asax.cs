using Elmah;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace SBISCCMWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-Powered-By");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            #region Set License and Api Token
            MasterClientApplicationFacade Mfac = new MasterClientApplicationFacade(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ToString());
            DataTable dtCAppData = new DataTable();
            ClientApplicationDataEntity objApplicationdata = new ClientApplicationDataEntity();
            objApplicationdata = Mfac.GetClientApplicationData(HttpContext.Current.Request.Url.Authority);

            if (objApplicationdata != null)
            {
                Helper.ApplicationData = objApplicationdata;
                string connectionString = StringCipher.Decrypt(objApplicationdata.ApplicationDBConnectionStringHash, General.passPhrase);
                CommonMethod.LicenseSetting(HttpContext.Current.Request.Url.Authority, StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ToString(), General.passPhrase));
                CommonMethod.GetAPIToken(connectionString);
            }
            #endregion
        }
        void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            bool sendMail = false;

            var context = e.Context as HttpContext;

            var error = new Error(e.Exception, context);
            try
            {
                string ErrorMailKey = Convert.ToString(ConfigurationManager.AppSettings["ErrorMailKey"]);
                if (ErrorMailKey != null)
                {
                    if (ErrorMailKey.Contains(","))
                    {
                        string[] getErrorTypes = ErrorMailKey.Split(',');

                        foreach (string err in getErrorTypes)
                        {
                            int StatusCode = Convert.ToInt32(err.Split(':')[0]);
                            string errType = err.Split(':')[1].ToString();

                            if (error.StatusCode == StatusCode && error.Type == errType)
                            {
                                sendMail = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        int StatusCode = Convert.ToInt32(ErrorMailKey.Split(':')[0]);
                        string errType = ErrorMailKey.Split(':')[1].ToString();
                        if (error.StatusCode == StatusCode && error.Type == errType)
                            sendMail = true;
                    }

                    if (!sendMail)
                        e.Dismiss();

                }
                else
                {
                    e.Dismiss();
                }
            }
            catch (Exception)
            {
                e.Dismiss();
            }
        }


        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if ((Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
                    && Context.Request.IsAuthenticated && Helper.UserId <= 0
                    && !Request.Url.AbsolutePath.Equals("/Account/UnAuthenticatedLogOff"))
            {
                Session.Abandon();
                if (!Request.Url.AbsolutePath.Equals("/Account/LicenceExpire"))
                {
                    Context.Response.Redirect("~/Account/LicenceExpire", true);
                }
                Context.Response.Redirect("~/Account/UnAuthenticatedLogOff", true);
            }
        }
    }
}
