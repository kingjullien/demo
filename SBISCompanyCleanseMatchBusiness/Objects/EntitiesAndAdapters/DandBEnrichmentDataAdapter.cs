using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class DandBEnrichmentDataAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DandBEnrichmentDataEntity> AdaptLists(DataTable dt, string APILayer)
        {
            List<DandBEnrichmentDataEntity> results = new List<DandBEnrichmentDataEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DandBEnrichmentDataEntity clientlist = new DandBEnrichmentDataEntity();
                clientlist = AdaptItemReport(rw, dt, APILayer);
                results.Add(clientlist);
            }
            return results;
        }
        public DandBEnrichmentDataEntity AdaptItemReport(DataRow rw, DataTable dt, string APILayer)
        {
            DandBEnrichmentDataEntity results = new DandBEnrichmentDataEntity();
            results.APILayer = APILayer;
            results.MappingId = SafeHelper.GetSafeint(rw["MappingId"]);
            results.JSONPath = SafeHelper.GetSafestring(rw["JSONPath"]);
            results.SpecialHandling = SafeHelper.GetSafestring(rw["SpecialHandling"]);
            results.MBSColumnName = SafeHelper.GetSafestring(rw["MBSColumnName"]);
            results.DataType = SafeHelper.GetSafestring(rw["DataType"]);
            results.Available = SafeHelper.GetSafebool(rw["Available"]);
            return results;
        }
    }
}
