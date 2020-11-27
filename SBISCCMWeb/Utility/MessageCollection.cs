using SBISCCMWeb.LanguageResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class MessageCollection
    {
        #region Email Text for DandB and Matchbook
        // Email text,account create text,support ticket and ticket create text added for DandB(when branding type is DandB)
        public static string DandBEmailText = "If you have any questions, please contact your Matchbook Services Administrator or our support desk at support@matchbookservices.com.";
        public static string DandBSupport = "Matchbook Services Support";
        public static string DandBTicketText = "We have received your helpdesk ticket. Here are the ticket details for your record. You can always review the status of your opened tickets by going to Reports/Help Desk from your matchbook portal";
        public static string DandBTicketCreated = "Matchbook Helpdesk ticket created";
        public static string DandBResetPassword = "If you did not request to reset your password, please contact your Matchbook Services Administrator or our support desk at support@matchbookservices.com.";
        public static string DandBAccountCreate = "Your account has been created for Matchbook Services. To log in, please go to matchbookservices.com. Click on login and enter your custom domain";
        public static string DandBActivateAccount = "Matchbook account has been activated";
        public static string DandBWelcomeText = "Welcome to Matchbook Services";

        // Email text,account create text,support ticket and ticket create text added for Matchbook(when branding type is Matchbook)
        public static string MatchbookEmailText = "If you have any questions, please contact your Matchbook Services Administrator or our support desk at support@matchbookservices.com.";
        public static string MatchbookSupport = "Matchbook Services Support";
        public static string MatchbookTicketText = "We have received your helpdesk ticket. Here are the ticket details for your record. You can always review the status of your opened tickets by going to Reports/Help Desk from your matchbook portal";
        public static string MatchbookTicketCreated = "Matchbook Helpdesk ticket created";
        public static string MatchbookResetPassword = "If you did not request to reset your password, please contact your Matchbook Services Administrator or our support desk at support@matchbookservices.com.";
        public static string MatchbookAccountCreate = "Your account has been created for Matchbook Services. To log in, please go to matchbookservices.com. Click on login and enter your custom domain";
        public static string MatchbookActivateAccount = "Matchbook account has been activated";
        public static string MatchbookWelcomeText = "Welcome to Matchbook Services";
        #endregion 

    }
    public class ConstantValues
    {
        public const string LOBTagCode = "601006";
    }
}


