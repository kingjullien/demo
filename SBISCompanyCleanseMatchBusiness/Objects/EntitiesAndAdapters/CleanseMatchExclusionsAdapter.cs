using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class CleanseMatchExclusionsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<CleanseMatchExclusions> Adapt(DataTable dt)
        {
            List<CleanseMatchExclusions> results = new List<CleanseMatchExclusions>();
            foreach (DataRow rw in dt.Rows)
            {
                CleanseMatchExclusions productDataEntity = new CleanseMatchExclusions();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public CleanseMatchExclusions AdaptItem(DataRow rw, DataTable dt)
        {
            CleanseMatchExclusions result = new CleanseMatchExclusions();
            result.CleanseMatchExclusionId = SafeHelper.GetSafeint(rw["CleanseMatchExclusionId"]);
            if (dt.Columns.Contains("Exclusion"))
            {
                result.Exclusion = SafeHelper.GetSafestring(rw["Exclusion"]);
            }

            if (dt.Columns.Contains("Exclusion_DP"))
            {
                result.Exclusion_DP = SafeHelper.GetSafestring(rw["Exclusion_DP"]);
            }

            if (dt.Columns.Contains("Active"))
            {
                result.Active = SafeHelper.GetSafebool(rw["Active"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            return result;
        }
    }
}
