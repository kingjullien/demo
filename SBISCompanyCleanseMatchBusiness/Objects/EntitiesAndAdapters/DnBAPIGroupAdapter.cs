using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class DnBAPIGroupAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DnBAPIGroupEntity> Adapt(DataTable dt)
        {
            List<DnBAPIGroupEntity> results = new List<DnBAPIGroupEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DnBAPIGroupEntity matchCode = new DnBAPIGroupEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public DnBAPIGroupEntity AdaptItem(DataRow rw, DataTable dt)
        {
            DnBAPIGroupEntity result = new DnBAPIGroupEntity();
            if (dt.Columns.Contains("APIGroupName"))
            {
                result.APIGroupName = SafeHelper.GetSafestring(rw["APIGroupName"]);
            }

            if (dt.Columns.Contains("APIGroupId"))
            {
                result.APIGroupId = SafeHelper.GetSafeint(rw["APIGroupId"]);
            }

            if (dt.Columns.Contains("DnbAPIIds"))
            {
                result.DnbAPIIds = SafeHelper.GetSafestring(rw["DnbAPIIds"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("CountryGroupId"))
            {
                result.CountryGroupId = SafeHelper.GetSafeint(rw["CountryGroupId"]);
            }

            if (dt.Columns.Contains("CountryGroupName"))
            {
                result.CountryGroupName = SafeHelper.GetSafestring(rw["CountryGroupName"]);
            }

            if (dt.Columns.Contains("APIType"))
            {
                result.APIType = SafeHelper.GetSafestring(rw["APIType"]);
            }

            if (dt.Columns.Contains("CredentialId"))
            {
                result.CredentialId = SafeHelper.GetSafeint(rw["CredentialId"]);
            }

            return result;
        }
    }
}
