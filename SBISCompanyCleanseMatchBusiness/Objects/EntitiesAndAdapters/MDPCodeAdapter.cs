using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class MDPCodeAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MDPCodeEntity> Adapt(DataTable dt)
        {
            List<MDPCodeEntity> results = new List<MDPCodeEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MDPCodeEntity cust = new MDPCodeEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public MDPCodeEntity AdaptItem(DataRow rw)
        {
            MDPCodeEntity result = new MDPCodeEntity();
            result.MatchCode = SafeHelper.GetSafestring(rw["MatchCode"]);
            result.MatchDescription = SafeHelper.GetSafestring(rw["MatchDescription"]);
            result.MatchField = SafeHelper.GetSafestring(rw["MatchField"]);
            result.MatchType = SafeHelper.GetSafestring(rw["MatchType"]);
            return result;
        }

    }
}
