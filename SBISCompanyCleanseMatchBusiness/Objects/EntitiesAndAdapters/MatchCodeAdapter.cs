using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class MatchCodeAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MatchCodeEntity> Adapt(DataTable dt)
        {
            List<MatchCodeEntity> results = new List<MatchCodeEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MatchCodeEntity matchCode = new MatchCodeEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public MatchCodeEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MatchCodeEntity result = new MatchCodeEntity();
            result.MDPCode = SafeHelper.GetSafestring(rw["MDPCode"]);
            result.MDPValue = SafeHelper.GetSafestring(rw["MDPValue"]);
            if (dt.Columns.Contains("MDPType"))
            {
                result.MDPType = SafeHelper.GetSafestring(rw["MDPType"]);
            }

            return result;
        }
    }
}
