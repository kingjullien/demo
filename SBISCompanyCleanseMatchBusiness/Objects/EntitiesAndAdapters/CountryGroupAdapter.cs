using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class CountryGroupAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CountryGroupEntity> Adapt(DataTable dt)
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CountryGroupEntity matchCode = new CountryGroupEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public CountryGroupEntity AdaptItem(DataRow rw, DataTable dt)
        {
            CountryGroupEntity result = new CountryGroupEntity();
            result.GroupName = SafeHelper.GetSafestring(rw["GroupName"]);
            result.GroupId = SafeHelper.GetSafeint(rw["GroupId"]);
            if (dt.Columns.Contains("ISOAlpha2Codes"))
            {
                result.ISOAlpha2Codes = SafeHelper.GetSafestring(rw["ISOAlpha2Codes"]);
            }

            if (dt.Columns.Contains("CanDeletable"))
            {
                result.CanDeletable = !SafeHelper.GetSafebool(rw["CanDeletable"]);
            }

            return result;
        }
    }
}
