using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class CountryAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CountryEntity> Adapt(DataTable dt)
        {
            List<CountryEntity> results = new List<CountryEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CountryEntity matchCode = new CountryEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public CountryEntity AdaptItem(DataRow rw)
        {
            CountryEntity result = new CountryEntity();
            result.CountryName = SafeHelper.GetSafestring(rw["CountryName"]);
            result.ISOAlpha2Code = SafeHelper.GetSafestring(rw["ISOAlpha2Code"]);
            result.ISOAlpha3Code = SafeHelper.GetSafestring(rw["ISOAlpha3Code"]);
            result.ISO3Code = SafeHelper.GetSafestring(rw["ISO3Code"]);
            result.CountryWithISOCode = SafeHelper.GetSafestring(rw["CountryWithISOCode"]);

            return result;
        }
    }
}
