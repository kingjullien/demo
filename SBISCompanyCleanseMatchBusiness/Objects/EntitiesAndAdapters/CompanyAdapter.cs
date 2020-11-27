using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;
namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class CompanyAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CompanyEntity> Adapt(DataTable dt)
        {
            List<CompanyEntity> results = new List<CompanyEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CompanyEntity cust = new CompanyEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public CompanyEntity AdaptItem(DataRow rw)
        {
            CompanyEntity result = new CompanyEntity();
            result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            result.Address = SafeHelper.GetSafestring(rw["Address"]);
            result.City = SafeHelper.GetSafestring(rw["City"]);
            result.State = SafeHelper.GetSafestring(rw["State"]);
            result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            result.PhoneNbr = SafeHelper.GetSafestring(rw["PhoneNbr"]);
            result.EncryptedSrcRecordId = StringCipher.Encrypt(result.SrcRecordId, General.passPhrase).Replace("+", "***");

            if (rw.Table.Columns["ErrorCode"] != null)
            {
                result.ErrorCode = SafeHelper.GetSafestring(rw["ErrorCode"]);
            }

            if (rw.Table.Columns["ErrorDescription"] != null)
            {
                result.ErrorDescription = SafeHelper.GetSafestring(rw["ErrorDescription"]);
            }

            if (rw.Table.Columns["StewardshipNotes"] != null)
            {
                result.StewardshipNotes = SafeHelper.GetSafestring(rw["StewardshipNotes"]);
            }

            if (rw.Table.Columns["RejectAllMatches"] != null)
            {
                result.RejectAllMatches = SafeHelper.GetSafebool(rw["RejectAllMatches"]);
            }

            result.RejectCompany = false;
            result.IsEdited = false;
            if (rw.Table.Columns["DUNSNumber"] != null)
            {
                result.DUNSNumber = SafeHelper.GetSafestring(rw["DUNSNumber"]);
            }

            if (rw.Table.Columns["CEOName"] != null)
            {
                result.CEOName = SafeHelper.GetSafestring(rw["CEOName"]);
            }

            if (rw.Table.Columns["Website"] != null)
            {
                result.Website = SafeHelper.GetSafestring(rw["Website"]);
            }

            if (rw.Table.Columns["AltCompanyName"] != null)
            {
                result.AltCompanyName = SafeHelper.GetSafestring(rw["AltCompanyName"]);
            }

            if (rw.Table.Columns["AltAddress"] != null)
            {
                result.AltAddress = SafeHelper.GetSafestring(rw["AltAddress"]);
            }

            if (rw.Table.Columns["AltCity"] != null)
            {
                result.AltCity = SafeHelper.GetSafestring(rw["AltCity"]);
            }

            if (rw.Table.Columns["AltState"] != null)
            {
                result.AltState = SafeHelper.GetSafestring(rw["AltState"]);
            }

            if (rw.Table.Columns["AltPostalCode"] != null)
            {
                result.AltPostalCode = SafeHelper.GetSafestring(rw["AltPostalCode"]);
            }

            if (rw.Table.Columns["AltCountry"] != null)
            {
                result.AltCountry = SafeHelper.GetSafestring(rw["AltCountry"]);
            }

            if (rw.Table.Columns["Email"] != null)
            {
                result.Email = SafeHelper.GetSafestring(rw["Email"]);
            }

            if (rw.Table.Columns["RegistrationNbr"] != null)
            {
                result.RegistrationNbr = SafeHelper.GetSafestring(rw["RegistrationNbr"]);
            }

            if (rw.Table.Columns["RegistrationType"] != null)
            {
                result.RegistrationType = SafeHelper.GetSafestring(rw["RegistrationType"]);
            }

            if (rw.Table.Columns["Tags"] != null)
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (rw.Table.Columns["InputId"] != null)
            {
                result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            }

            if (rw.Table.Columns["inLanguage"] != null)
            {
                result.inLanguage = SafeHelper.GetSafestring(rw["inLanguage"]);
            }

            if (rw.Table.Columns["Address1"] != null)
            {
                result.Address1 = SafeHelper.GetSafestring(rw["Address1"]);
            }

            if (rw.Table.Columns["AltAddress1"] != null)
            {
                result.AltAddress1 = SafeHelper.GetSafestring(rw["AltAddress1"]);
            }

            if (rw.Table.Columns["FullAddress"] != null)
            {
                result.FullAddress = SafeHelper.GetSafestring(rw["FullAddress"]);
            }
            
            return result;
        }
    }
}
