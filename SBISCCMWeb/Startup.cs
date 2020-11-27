using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using SBISCCMWeb.Controllers;
using SBISCCMWeb.Infrastructure;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(SBISCCMWeb.Startup))]
namespace SBISCCMWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConnectionMultiplexer.SetFeatureFlag("preventthreadtheft", true);
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new MultiLanguageControllerActivator()));
            ConfigureAuth(app);
            app.MapSignalR();
            //string ConnectionString = StringCipher.Decrypt(ConfigurationManager.ConnectionStrings["SolidQMasterWeb"].ConnectionString, General.passPhrase);
            //GlobalConfiguration.Configuration.UseSqlServerStorage(ConnectionString);

            //BackgroundJobServerOptions option = new BackgroundJobServerOptions();
            //option.ServerName = "MatchBook Services";
            //app.UseHangfireDashboard("/hangfireDashboard", new DashboardOptions
            //{
            //    Authorization = new[] { new MyAuthorizationFilter() }
            //});
            //app.UseHangfireServer(option);
            //Job.ExportDataJob();
        }
    }

    //public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    //{
    //    public bool Authorize(DashboardContext context)
    //    {
    //        var owinContext = new OwinContext(context.GetOwinEnvironment());
    //        if (HttpContext.Current != null && HttpContext.Current.Request != null && Helper.oUser != null)
    //        {
    //            return owinContext.Authentication.User.Identity.IsAuthenticated && (Helper.oUser.EmailAddress == "rmehta@sequelbisolutions.com" || Helper.oUser.EmailAddress == "rmehta@matchbookservices.com" || Helper.oUser.EmailAddress == "pl9@estatic-infotech.in");

    //        }
    //        else
    //            return false;

    //    }
    //}
}
