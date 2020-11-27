using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using SBISCCMWeb.Utility;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SBISCCMWeb
{
    public partial class Startup
    {
        internal static IDataProtectionProvider DataProtectionProvider { get; private set; }
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            DataProtectionProvider = app.GetDataProtectionProvider();
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "MatchbookAuth",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")

            });
        }
    }
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            string emailhost = Helper.GetAppSettingAsString("emailhost");
            int emailport = Convert.ToInt16(Helper.GetAppSettingAsString("emailport"));
            string emailFrom = Helper.GetAppSettingAsString("emailFrom");
            string emailuserName = Helper.GetAppSettingAsString("emailuserName");
            string emailpassword = Helper.GetAppSettingAsString("emailpassword");
            bool emailenableSsl = Convert.ToBoolean(Helper.GetAppSettingAsString("emailenableSsl"));

            // Configure the client:
            var client = new System.Net.Mail.SmtpClient()
            {
                Host = emailhost,
                Port = emailport,
                EnableSsl = emailenableSsl,
                Credentials = new System.Net.NetworkCredential(emailuserName, emailpassword)
            };

            MailAddress fromAddress = new MailAddress(emailFrom);

            // Create the message:
            var mail = new System.Net.Mail.MailMessage();
            mail.From = fromAddress;
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject.Replace('\r', ' ').Replace('\n', ' ');
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
        }
    }
}