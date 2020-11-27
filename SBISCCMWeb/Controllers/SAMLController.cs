using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Configuration;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SBISCCMWeb.Controllers
{
    public class SAMLController : Controller
    {
        public const string AttributesSessionKey = "";
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [AllowAnonymous]
        public ActionResult AssertionConsumerService()
        {
            bool isInResponseTo = false;
            string partnerIdP = null;
            string authnContext = null;
            string userName = null;
            IDictionary<string, string> attributes = null;
            string targetUrl = null;


            // Receive and process the SAML assertion contained in the SAML response.
            // The SAML response is received either as part of IdP-initiated or SP-initiated SSO.
            SAMLServiceProvider.ReceiveSSO(Request, out isInResponseTo, out partnerIdP, out authnContext, out userName, out attributes, out targetUrl);

            // If no target URL is provided, provide a default.
            if (targetUrl == null)
            {
                targetUrl = "~/Account/SMALLogin?userName=" + userName;
            }

            // Login automatically using the asserted identity.
            // This example uses forms authentication. Your application can use any authentication method you choose.
            // There are no restrictions on the method of authentication.
            //FormsAuthentication.SetAuthCookie(userName, false);

            // Save the attributes.
            Session[AttributesSessionKey] = attributes;

            // Redirect to the target URL.
            return RedirectToLocal(targetUrl);
        }
        [AllowAnonymous]
        public ActionResult SLOService()
        {
            // Receive the single logout request or response.
            // If a request is received then single logout is being initiated by the identity provider.
            // If a response is received then this is in response to single logout having been initiated by the service provider.
            bool isRequest = false;
            string logoutReason = null;
            string partnerIdP = null;
            string relayState = null;

            SAMLServiceProvider.ReceiveSLO(Request, out isRequest, out logoutReason, out partnerIdP, out relayState);

            if (isRequest)
            {
                // Logout locally.
                FormsAuthentication.SignOut();

                // Respond to the IdP-initiated SLO request indicating successful logout.
                SAMLServiceProvider.SendSLO(Response, null);
            }
            else
            {
                // SP-initiated SLO has completed.
                FormsAuthentication.RedirectToLoginPage();
            }

            string targetUrl = "~/Account/LogOff";
            return RedirectToLocal(targetUrl);
            //return new EmptyResult();
        }

    }
}