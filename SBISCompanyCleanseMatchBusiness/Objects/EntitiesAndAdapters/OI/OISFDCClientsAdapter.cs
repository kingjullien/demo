using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    public class OISFDCClientsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OISFDCClientsEntity> Adapt(DataTable dt)
        {
            List<OISFDCClientsEntity> results = new List<OISFDCClientsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OISFDCClientsEntity ExportJob = new OISFDCClientsEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }
        public OISFDCClientsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            OISFDCClientsEntity result = new OISFDCClientsEntity();
            if (dt.Columns.Contains("OIClientId"))
            {
                result.OIClientId = SafeHelper.GetSafeint(rw["OIClientId"]);
            }

            if (dt.Columns.Contains("OrgId"))
            {
                result.OrgId = SafeHelper.GetSafestring(rw["OrgId"]);
            }

            if (dt.Columns.Contains("LicenseType"))
            {
                result.LicenseType = SafeHelper.GetSafestring(rw["LicenseType"]);
            }

            if (dt.Columns.Contains("StartDate"))
            {
                result.StartDate = SafeHelper.GetSafeDateTime(rw["StartDate"]);
            }

            if (dt.Columns.Contains("EndDate"))
            {
                result.EndDate = SafeHelper.GetSafeDateTime(rw["EndDate"]);
            }

            if (dt.Columns.Contains("CompanyName"))
            {
                result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            }

            if (dt.Columns.Contains("AddressLine1"))
            {
                result.AddressLine1 = SafeHelper.GetSafestring(rw["AddressLine1"]);
            }

            if (dt.Columns.Contains("AddressLine2"))
            {
                result.AddressLine2 = SafeHelper.GetSafestring(rw["AddressLine2"]);
            }

            if (dt.Columns.Contains("City"))
            {
                result.City = SafeHelper.GetSafestring(rw["City"]);
            }

            if (dt.Columns.Contains("State"))
            {
                result.State = SafeHelper.GetSafestring(rw["State"]);
            }

            if (dt.Columns.Contains("PostalCode"))
            {
                result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            }

            if (dt.Columns.Contains("Country"))
            {
                result.Country = SafeHelper.GetSafestring(rw["Country"]);
            }

            if (dt.Columns.Contains("LicenseKey"))
            {
                result.LicenseKey = SafeHelper.GetSafestring(rw["LicenseKey"]);
            }

            if (dt.Columns.Contains("ContactName"))
            {
                result.ContactName = SafeHelper.GetSafestring(rw["ContactName"]);
            }

            if (dt.Columns.Contains("ContactEmail"))
            {
                result.ContactEmail = SafeHelper.GetSafestring(rw["ContactEmail"]);
            }

            if (dt.Columns.Contains("ContactPhone"))
            {
                result.ContactPhone = SafeHelper.GetSafestring(rw["ContactPhone"]);
            }

            if (dt.Columns.Contains("VerifiedDate"))
            {
                result.VerifiedDate = SafeHelper.GetSafeDateTime(rw["VerifiedDate"]);
            }

            return result;
        }
    }
}
