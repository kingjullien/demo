using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DnbAPIAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DnbAPIEntity> Adapt(DataTable dt)
        {
            List<DnbAPIEntity> results = new List<DnbAPIEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DnbAPIEntity matchCode = new DnbAPIEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public DnbAPIEntity AdaptItem(DataRow rw)
        {
            DnbAPIEntity result = new DnbAPIEntity();
            if (rw.Table.Columns["DnBAPIId"] != null)
            {
                result.DnBAPIId = SafeHelper.GetSafeint(rw["DnBAPIId"]);
            }

            if (rw.Table.Columns["DnBAPICode"] != null)
            {
                result.DnBAPICode = SafeHelper.GetSafestring(rw["DnBAPICode"]);
            }

            if (rw.Table.Columns["DnBAPIName"] != null)
            {
                result.DnBAPIName = SafeHelper.GetSafestring(rw["DnBAPIName"]);
            }

            if (rw.Table.Columns["APIName"] != null)
            {
                result.APIName = SafeHelper.GetSafestring(rw["APIName"]);
            }

            if (rw.Table.Columns["APIType"] != null)
            {
                result.APIType = SafeHelper.GetSafestring(rw["APIType"]);
            }

            if (rw.Table.Columns["DUNSEnhancementAPI"] != null)
            {
                result.DUNSEnhancementAPI = SafeHelper.GetSafebool(rw["DUNSEnhancementAPI"]);
            }

            if (rw.Table.Columns["APIFamily"] != null)
            {
                result.APIFamily = SafeHelper.GetSafestring(rw["APIFamily"]);
            }

            return result;
        }
    }
}
