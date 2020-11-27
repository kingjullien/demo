using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, RequestFromSameDomain, TwoStepVerification]
    public class FeedbackController : BaseController
    {
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult AddFeedback(string Parameters)
        {
            string FeedbackType = ""; string Feedback = ""; string FeedbackPath = "";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                FeedbackType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Feedback = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                FeedbackPath = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
            }

            FeedbackEntity model = new FeedbackEntity();
            model.Feedback = Feedback;
            model.FeedbackType = FeedbackType;
            model.FeedbackPath = FeedbackPath;

            // Save the information of the Feedback of the current screen to the admin 
            if (ModelState.IsValid)
            {
                string EmailAddress = ConfigurationManager.AppSettings["ApplicationEmail"];
                FeedbackFacade fac = new FeedbackFacade();
                model.ClientInformation = Request.Url.Authority;
                model.EnteredBy = Convert.ToString(Helper.UserName);
                model.EmailAddress = Helper.oUser.EmailAddress;
                fac.InsertUpdateFeedback(model);

                string emailBody = string.Empty;
                emailBody += "User Feedback <br/><br/>";
                emailBody += "User Name : " + model.EnteredBy + " <br/><br/>";
                emailBody += "Email : " + model.EmailAddress + " <br/><br/>";
                emailBody += "FeedbackType : " + model.FeedbackType + " <br/><br/>";
                emailBody += "Feedback : " + model.Feedback + " <br/><br/>";
                emailBody += "Feedback Path: " + model.FeedbackPath + " <br/><br/>";
                if (!string.IsNullOrEmpty(EmailAddress))
                {
                    Helper.SendMail(EmailAddress, "Feedback received.", emailBody);
                }
                string Message = TicketLang.msgFeedbackMessage;
                return new JsonResult { Data = Message };
            }
            return new JsonResult { Data = CommonMessagesLang.msgCommanErrorMessage };
        }
    }
}