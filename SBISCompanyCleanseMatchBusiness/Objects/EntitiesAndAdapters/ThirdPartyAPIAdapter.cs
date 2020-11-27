using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ThirdPartyAPIAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ThirdPartyAPIEntity> Adapt(DataTable dt)
        {
            List<ThirdPartyAPIEntity> results = new List<ThirdPartyAPIEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ThirdPartyAPIEntity cust = new ThirdPartyAPIEntity();
                cust = AdaptItem(rw, dt);
                results.Add(cust);
            }
            return results;
        }
        public ThirdPartyAPIEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ThirdPartyAPIEntity result = new ThirdPartyAPIEntity();

            if (rw.Table.Columns["DNB_BUILD_A_LIST"] != null)
            {
                result.DNB_BUILD_A_LIST = SafeHelper.GetSafestring(rw["DNB_BUILD_A_LIST"]);
            }

            if (rw.Table.Columns["DNB_INVESTIGATIONS"] != null)
            {
                result.DNB_INVESTIGATIONS = SafeHelper.GetSafestring(rw["DNB_INVESTIGATIONS"]);
            }

            if (rw.Table.Columns["DNB_SINGLE_ENTITY_SEARCH"] != null)
            {
                result.DNB_SINGLE_ENTITY_SEARCH = SafeHelper.GetSafestring(rw["DNB_SINGLE_ENTITY_SEARCH"]);
            }

            if (rw.Table.Columns["GOOGLE"] != null)
            {
                result.GOOGLE = SafeHelper.GetSafestring(rw["GOOGLE"]);
            }

            if (rw.Table.Columns["ORB_BUILD_A_LIST"] != null)
            {
                result.ORB_BUILD_A_LIST = SafeHelper.GetSafestring(rw["ORB_BUILD_A_LIST"]);
            }

            if (rw.Table.Columns["ORB_SINGLE_ENTITY_SEARCH"] != null)
            {
                result.ORB_SINGLE_ENTITY_SEARCH = SafeHelper.GetSafestring(rw["ORB_SINGLE_ENTITY_SEARCH"]);
            }

            return result;
        }
    }
}
