using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class AutoAcceptanceDirectivesAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<AutoAcceptanceDirectives> Adapt(DataTable dt)
        {
            List<AutoAcceptanceDirectives> results = new List<AutoAcceptanceDirectives>();
            foreach (DataRow rw in dt.Rows)
            {
                AutoAcceptanceDirectives productDataEntity = new AutoAcceptanceDirectives();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public AutoAcceptanceDirectives AdaptItem(DataRow rw, DataTable dt)
        {
            AutoAcceptanceDirectives result = new AutoAcceptanceDirectives();
            result.AutoAcceptanceDirectiveId = SafeHelper.GetSafeint(rw["AutoAcceptanceDirectiveId"]);
            if (dt.Columns.Contains("Directive"))
            {
                result.Directive = SafeHelper.GetSafestring(rw["Directive"]);
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
