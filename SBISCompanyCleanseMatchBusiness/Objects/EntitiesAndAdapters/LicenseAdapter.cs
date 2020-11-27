using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class LicenseAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<LicenseEntity> Adapt(DataTable dt)
        {
            List<LicenseEntity> results = new List<LicenseEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                LicenseEntity objLicense = new LicenseEntity();
                objLicense = AdaptItem(rw, dt);
                results.Add(objLicense);
            }
            return results;
        }
        public LicenseEntity AdaptItem(DataRow rw, DataTable dt)
        {
            LicenseEntity result = new LicenseEntity();
            if (dt.Columns.Contains("LicenseId"))
            {
                result.LicenseId = SafeHelper.GetSafeint(rw["LicenseId"]);
            }

            if (dt.Columns.Contains("LicenseCode"))
            {
                result.LicenseCode = SafeHelper.GetSafestring(rw["LicenseCode"]);
            }

            if (dt.Columns.Contains("LicenseDescription"))
            {
                result.LicenseDescription = SafeHelper.GetSafestring(rw["LicenseDescription"]);
            }

            return result;
        }
    }
}
