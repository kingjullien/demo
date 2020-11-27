using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MinConfidenceOverrideAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MinConfidenceOverrideEntity> Adapt(DataTable dt)
        {
            List<MinConfidenceOverrideEntity> results = new List<MinConfidenceOverrideEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MinConfidenceOverrideEntity cust = new MinConfidenceOverrideEntity();
                cust = AdaptItem(rw, dt);
                results.Add(cust);
            }
            return results;
        }

        public MinConfidenceOverrideEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MinConfidenceOverrideEntity result = new MinConfidenceOverrideEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("MinConfidenceCode"))
            {
                result.MinConfidenceCode = SafeHelper.GetSafeint(rw["MinConfidenceCode"]);
            }

            if (dt.Columns.Contains("MaxCandidateQty"))
            {
                result.MaxCandidateQty = SafeHelper.GetSafeint(rw["MaxCandidateQty"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("IsActive"))
            {
                result.IsActive = SafeHelper.GetSafebool(rw["IsActive"]);
            }

            return result;
        }
    }
}
