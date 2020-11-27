using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CVRefAadpter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CVRefEntity> Adapt(DataTable dt)
        {
            List<CVRefEntity> results = new List<CVRefEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CVRefEntity clientlist = new CVRefEntity();
                clientlist = AdaptItem(rw, dt);
                results.Add(clientlist);
            }
            return results;
        }
        public CVRefEntity AdaptItem(DataRow rw, DataTable dt)
        {
            CVRefEntity results = new CVRefEntity();
            if (dt.Columns.Contains("CVRefId"))
            {
                results.CVRefId = SafeHelper.GetSafeint(rw["CVRefId"]);
            }

            if (dt.Columns.Contains("Code"))
            {
                results.Code = SafeHelper.GetSafestring(rw["Code"]);
            }

            if (dt.Columns.Contains("Value"))
            {
                results.Value = SafeHelper.GetSafestring(rw["Value"]);
            }

            if (dt.Columns.Contains("Description"))
            {
                results.Description = SafeHelper.GetSafestring(rw["Description"]);
            }

            if (dt.Columns.Contains("TypeCode"))
            {
                results.TypeCode = SafeHelper.GetSafestring(rw["TypeCode"]);
            }

            return results;
        }
    }
}
