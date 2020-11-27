using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;


namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class GoogleAPIAdapters
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<GoogleAPIEntity> Adapt(DataTable dt)
        {
            List<GoogleAPIEntity> results = new List<GoogleAPIEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                GoogleAPIEntity matchCode = new GoogleAPIEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public GoogleAPIEntity AdaptItem(DataRow rw, DataTable dt)
        {
            GoogleAPIEntity result = new GoogleAPIEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("APIKey"))
            {
                result.APIKey = SafeHelper.GetSafestring(rw["APIKey"]);
            }

            if (dt.Columns.Contains("IsDefault"))
            {
                result.IsDefault = SafeHelper.GetSafebool(rw["IsDefault"]);
            }

            return result;
        }
    }
}
