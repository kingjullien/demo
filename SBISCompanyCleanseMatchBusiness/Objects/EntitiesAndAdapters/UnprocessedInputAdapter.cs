using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UnprocessedInputAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<UnprocessedInputEntity> Adapt(DataTable dt)
        {
            List<UnprocessedInputEntity> results = new List<UnprocessedInputEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UnprocessedInputEntity cust = new UnprocessedInputEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public UnprocessedInputEntity AdaptItem(DataRow rw)
        {
            UnprocessedInputEntity result = new UnprocessedInputEntity();
            if (rw.Table.Columns["InputId"] != null)
            {
                result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            }

            if (rw.Table.Columns["SrcRecordId"] != null)
            {
                result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            }

            if (rw.Table.Columns["SourceName"] != null)
            {
                result.SourceName = SafeHelper.GetSafestring(rw["SourceName"]);
            }

            if (rw.Table.Columns["CompanyName"] != null)
            {
                result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            }

            if (rw.Table.Columns["Address"] != null)
            {
                result.Address = SafeHelper.GetSafestring(rw["Address"]);
            }

            if (rw.Table.Columns["Address1"] != null)
            {
                result.Address1 = SafeHelper.GetSafestring(rw["Address1"]);
            }

            if (rw.Table.Columns["City"] != null)
            {
                result.City = SafeHelper.GetSafestring(rw["City"]);
            }

            if (rw.Table.Columns["State"] != null)
            {
                result.State = SafeHelper.GetSafestring(rw["State"]);
            }

            if (rw.Table.Columns["PostalCode"] != null)
            {
                result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            }

            if (rw.Table.Columns["CountryISOAlpha2Code"] != null)
            {
                result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            }

            if (rw.Table.Columns["PhoneNbr"] != null)
            {
                result.PhoneNbr = SafeHelper.GetSafestring(rw["PhoneNbr"]);
            }

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

            if (rw.Table.Columns["AltAddress1"] != null)
            {
                result.AltAddress1 = SafeHelper.GetSafestring(rw["AltAddress1"]);
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

            if (rw.Table.Columns["inLanguage"] != null)
            {
                result.inLanguage = SafeHelper.GetSafestring(rw["inLanguage"]);
            }

            if (rw.Table.Columns["LatestErrorCode"] != null)
            {
                result.LatestErrorCode = SafeHelper.GetSafestring(rw["LatestErrorCode"]);
            }

            return result;
        }
    }
}
