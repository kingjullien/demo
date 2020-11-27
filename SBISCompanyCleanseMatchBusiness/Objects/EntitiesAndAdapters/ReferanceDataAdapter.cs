using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ReferanceDataAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ReferanceDataEntity> Adapt(DataTable dt)
        {
            List<ReferanceDataEntity> results = new List<ReferanceDataEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ReferanceDataEntity referanceDataEntity = new ReferanceDataEntity();
                referanceDataEntity = AdaptItem(rw);
                results.Add(referanceDataEntity);
            }
            return results;
        }

        public ReferanceDataEntity AdaptItem(DataRow rw)
        {
            ReferanceDataEntity result = new ReferanceDataEntity();
            result.Code = SafeHelper.GetSafestring(rw["Code"]);
            result.Value = SafeHelper.GetSafestring(rw["Value"]);
            return result;
        }
    }
}
