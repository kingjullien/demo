using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class InvestigateUserAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public InvestigateViewEntity Adapt(DataTable dt)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                InvestigateViewEntity cust = new InvestigateViewEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results.FirstOrDefault();
        }
        public List<InvestigateViewEntity> AdaptList(DataTable dt)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                InvestigateViewEntity cust = new InvestigateViewEntity();
                cust = AdaptItemReprot(rw);
                results.Add(cust);
            }
            return results;
        }
        public List<InvestigateViewEntity> AdaptLists(DataTable dt)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                InvestigateViewEntity cust = new InvestigateViewEntity();
                cust = AdaptItemReport(rw);
                results.Add(cust);
            }
            return results;
        }
        public InvestigateViewEntity AdaptInvestigate(DataTable dt)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                InvestigateViewEntity cust = new InvestigateViewEntity();
                cust = AdaptInvestigation(rw, dt);
                results.Add(cust);
            }
            return results.FirstOrDefault();
        }
        public InvestigateViewEntity AdaptItem(DataRow rw)
        {
            InvestigateViewEntity result = new InvestigateViewEntity();
            result.LangCode = SafeHelper.GetSafestring(rw["LangPreferCode"]);
            result.CharCode = SafeHelper.GetSafestring(rw["CharacterPreferCode"]);
            result.ProdCode = SafeHelper.GetSafestring(rw["ProdFormatPreferCode"]);
            result.OrderCode = SafeHelper.GetSafestring(rw["OrderReasonCode"]);
            result.ProirityCode = SafeHelper.GetSafestring(rw["PriorityCode"]);
            result.FirstName = SafeHelper.GetSafestring(rw["FirstName"]);
            result.LastName = SafeHelper.GetSafestring(rw["LastName"]);
            result.InvsetTelecommunicationNumber = SafeHelper.GetSafestring(rw["ContactNo"]);
            result.InvesDialingCode = SafeHelper.GetSafestring(rw["ContactCallingCode"]);
            result.IsMobileIndicator = SafeHelper.GetSafebool(rw["IsMobileIdicator"]);
            result.RequestorName = SafeHelper.GetSafestring(rw["ReqName"]);
            result.ReqAddress = SafeHelper.GetSafestring(rw["ReqAddress"]);
            result.ReqCountryCode = SafeHelper.GetSafestring(rw["ReqCountry"]);
            result.ReqCity = SafeHelper.GetSafestring(rw["ReqCity"]);
            result.ReqState = SafeHelper.GetSafestring(rw["ReqState"]);
            result.ReqPostalCode = SafeHelper.GetSafestring(rw["ReqPostalCode"]);
            result.ReqEmailAddress = SafeHelper.GetSafestring(rw["EmailID"]);
            result.ReqTelecommunicationNumber = SafeHelper.GetSafestring(rw["ReqPhoneNo"]);
            result.ReqDialingCode = SafeHelper.GetSafestring(rw["ReqPhoneCallinCode"]);
            result.ReqIsMobileIndicator = SafeHelper.GetSafebool(rw["ReqIsMobileIndicator"]);
            result.DeliveryMethod = SafeHelper.GetSafestring(rw["DelMethodCode"]);
            result.DelEmailAddress = SafeHelper.GetSafestring(rw["DelAddress"]);
            result.NotificationMethod = SafeHelper.GetSafestring(rw["NotMethodCode"]);
            result.NotificationEmailAddress = SafeHelper.GetSafestring(rw["NotEmailID"]);
            result.UserId = SafeHelper.GetSafeint(rw["UserId"]);

            return result;
        }
        public InvestigateViewEntity AdaptItemReprot(DataRow rw)
        {
            InvestigateViewEntity result = new InvestigateViewEntity();
            result.ApplicationTransactionID = SafeHelper.GetSafeint(rw["ApplicationTransactionID"]);
            result.OrganizationIdentificationNumber = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumber"]);
            result.DUNSNumber = SafeHelper.GetSafestring(rw["DUNSNo"]);
            result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            result.InvestigationTrackingID = SafeHelper.GetSafestring(rw["InvestigationTrackingID"]);
            result.ResponseDateTime = SafeHelper.GetSafestring(rw["RequestDateTime"]);
            result.InvestigationStatus = SafeHelper.GetSafestring(rw["InvestigationStatus"]);
            result.FirstName = SafeHelper.GetSafestring(rw["FirstName"]);
            result.ReceivedFile = SafeHelper.GetSafestring(rw["ReceivedFile"]);
            result.ReferenceId = SafeHelper.GetSafeint(rw["ReferenceId"]);
            result.EncryptedApplicationTransactionID = StringCipher.Encrypt(Convert.ToString(result.ApplicationTransactionID), General.passPhrase).Replace("+", "***");
            return result;
        }
        public InvestigateViewEntity AdaptItemReport(DataRow rw)
        {
            InvestigateViewEntity result = new InvestigateViewEntity();
            result.ApplicationTransactionID = SafeHelper.GetSafeint(rw["Id"]);
            result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            result.Address1 = SafeHelper.GetSafestring(rw["Address1"]);
            result.Address2 = SafeHelper.GetSafestring(rw["Address2"]);
            result.City = SafeHelper.GetSafestring(rw["City"]);
            result.State = SafeHelper.GetSafestring(rw["State"]);
            result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            result.PhoneNbr = SafeHelper.GetSafestring(rw["PhoneNbr"]);
            result.OrbNum = SafeHelper.GetSafestring(rw["OrbNum"]);
            result.EIN = SafeHelper.GetSafestring(rw["EIN"]);
            result.Website = SafeHelper.GetSafestring(rw["Website"]);
            result.Email = SafeHelper.GetSafestring(rw["Email"]);
            result.Subdomain = SafeHelper.GetSafestring(rw["Subdomain"]);
            result.UserName = SafeHelper.GetSafestring(rw["UserName"]);
            result.UserEmail = SafeHelper.GetSafestring(rw["UserEmail"]);
            result.Message = SafeHelper.GetSafestring(rw["Message"]);
            result.RequestedDateTime = SafeHelper.GetSafeDateTime(rw["RequestedDateTime"]);
            result.CompletedDateTime = SafeHelper.GetSafeDateTime(rw["CompletedDateTime"]);
            result.MatchOrbNumber = SafeHelper.GetSafestring(rw["MatchOrbNumber"]);
            result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            result.TicketNumber = SafeHelper.GetSafestring(rw["TicketNumber"]);
            result.EncryptedApplicationTransactionID = StringCipher.Encrypt(Convert.ToString(result.ApplicationTransactionID), General.passPhrase).Replace("+", "***");
            return result;
        }

        public InvestigateViewEntity AdaptInvestigation(DataRow rw, DataTable dt)
        {
            InvestigateViewEntity result = new InvestigateViewEntity();
            result.ApplicationTransactionID = SafeHelper.GetSafeint(rw["ApplicationTransactionID"]);
            result.TransactionTimestamp = SafeHelper.GetSafeDateTime(rw["TransactionTimestamp"]);
            result.SubmittingOfficeID = SafeHelper.GetSafestring(rw["SubmitingID"]);
            result.DUNSNumber = SafeHelper.GetSafestring(rw["DUNSNo"]);
            result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            result.Address = SafeHelper.GetSafestring(rw["Address"]);
            result.LangCode = SafeHelper.GetSafestring(rw["LangPreferCode"]);
            result.CountryCode = SafeHelper.GetSafestring(rw["Country"]);
            result.City = SafeHelper.GetSafestring(rw["PrimaryTownName"]);
            result.State = SafeHelper.GetSafestring(rw["TerritoryName"]);
            result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            result.telecommunicationNumber = SafeHelper.GetSafestring(rw["TelePhoneNumber"]);
            result.DialingCode = SafeHelper.GetSafestring(rw["TelePhoneCallingCode"]);
            result.OrganizationIdentificationNumber = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumber"]);
            result.OrganizationIdentificationNumberTypeCode = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumberTypeCode"]);
            result.OrganizationIdentificationNumber2 = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumber2"]);
            result.OrganizationIdentificationNumberTypeCode2 = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumberTypeCode2"]);
            result.OrganizationIdentificationNumber3 = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumber3"]);
            result.OrganizationIdentificationNumberTypeCode3 = SafeHelper.GetSafestring(rw["OrganizationIdentificationNumberTypeCode3"]);
            result.LangCode = SafeHelper.GetSafestring(rw["LangPreferCode"]);
            result.LangCode = SafeHelper.GetSafestring(rw["LangPreferCode"]);
            result.CharCode = SafeHelper.GetSafestring(rw["CharacterPreferCode"]);
            result.ProdCode = SafeHelper.GetSafestring(rw["ProdFormatPreferCode"]);
            result.OrderCode = SafeHelper.GetSafestring(rw["OrderReasonCode"]);
            result.ProirityCode = SafeHelper.GetSafestring(rw["PriorityCode"]);
            result.FirstName = SafeHelper.GetSafestring(rw["FirstName"]);
            result.LastName = SafeHelper.GetSafestring(rw["LastName"]);
            result.InvsetTelecommunicationNumber = SafeHelper.GetSafestring(rw["ContactNo"]);
            result.InvesDialingCode = SafeHelper.GetSafestring(rw["ContactCallingCode"]);
            result.IsMobileIndicator = SafeHelper.GetSafebool(rw["IsMobileIdicator"]);
            result.RequestorName = SafeHelper.GetSafestring(rw["ReqName"]);
            result.ReqAddress = SafeHelper.GetSafestring(rw["ReqAddress"]);
            result.ReqCountryCode = SafeHelper.GetSafestring(rw["ReqCountry"]);
            result.ReqCity = SafeHelper.GetSafestring(rw["ReqCity"]);
            result.ReqState = SafeHelper.GetSafestring(rw["ReqState"]);
            result.ReqPostalCode = SafeHelper.GetSafestring(rw["ReqPostalCode"]);
            result.ReqEmailAddress = SafeHelper.GetSafestring(rw["EmailID"]);
            result.ReqTelecommunicationNumber = SafeHelper.GetSafestring(rw["ReqPhoneNo"]);
            result.ReqDialingCode = SafeHelper.GetSafestring(rw["ReqPhoneCallinCode"]);
            result.ReqIsMobileIndicator = SafeHelper.GetSafebool(rw["ReqIsMobileIndicator"]);
            result.DeliveryMethod = SafeHelper.GetSafestring(rw["DelMethodCode"]);
            result.DelEmailAddress = SafeHelper.GetSafestring(rw["DelAddress"]);
            result.NotificationMethod = SafeHelper.GetSafestring(rw["NotMethodCode"]);
            result.NotificationEmailAddress = SafeHelper.GetSafestring(rw["NotEmailID"]);
            result.CustomerReferenceText = SafeHelper.GetSafestring(rw["CustomerReferenceText"]);
            result.CustomerBillingEndorsementText = SafeHelper.GetSafestring(rw["CustomerBillingEndorsementText"]);
            result.PortfolioAssetContainerID = SafeHelper.GetSafeint(rw["PortfolioAssetContainerID"]);
            result.InvestigationTrackingID = SafeHelper.GetSafestring(rw["InvestigationTrackingID"]);
            result.ResponseDateTime = SafeHelper.GetSafestring(rw["ResponseDateTime"]);
            result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            result.InvestigationStatus = SafeHelper.GetSafestring(rw["InvestigationStatus"]);
            if (dt.Columns.Contains("RemarksText"))
            {
                result.RemarkText = SafeHelper.GetSafestring(rw["RemarksText"]);
            }

            result.EncryptedApplicationTransactionID = StringCipher.Encrypt(Convert.ToString(result.ApplicationTransactionID), General.passPhrase).Replace("+", "***");
            return result;
        }
    }
}
