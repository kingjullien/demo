using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class OverrideAPICredentialsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OverrideAPICredentialsEntity> Adapt(DataTable dt)
        {
            List<OverrideAPICredentialsEntity> results = new List<OverrideAPICredentialsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OverrideAPICredentialsEntity cust = new OverrideAPICredentialsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public OverrideAPICredentialsEntity AdaptItem(DataRow rw)
        {
            OverrideAPICredentialsEntity result = new OverrideAPICredentialsEntity();
            if (rw.Table.Columns["Id"] != null)
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (rw.Table.Columns["APIKey"] != null)
            {
                result.APIKeyOverride = SafeHelper.GetSafestring(rw["APIKey"]);
            }

            if (rw.Table.Columns["APISecret"] != null)
            {
                result.APISecretOverride = SafeHelper.GetSafestring(rw["APISecret"]);
            }

            if (rw.Table.Columns["BatchSize"] != null)
            {
                result.BatchSizeOverride = SafeHelper.GetSafeint(rw["BatchSize"]);
            }

            if (rw.Table.Columns["WaitTimesBetweenBatch"] != null)
            {
                result.WaitTimesBetweenBatchOverride = SafeHelper.GetSafeint(rw["WaitTimesBetweenBatch"]);
            }

            if (rw.Table.Columns["MaxParallelThreads"] != null)
            {
                result.MaxParallelThreadsOverride = SafeHelper.GetSafeint(rw["MaxParallelThreads"]);
            }

            if (rw.Table.Columns["APILayer"] != null)
            {
                result.APILayerOverride = SafeHelper.GetSafestring(rw["APILayer"]);
            }

            if (rw.Table.Columns["UseForCleanseMatch"] != null)
            {
                result.UseForCleanseMatchOverride = SafeHelper.GetSafebool(rw["UseForCleanseMatch"]);
            }

            if (rw.Table.Columns["UseForEnrich"] != null)
            {
                result.UseForEnrichOverride = SafeHelper.GetSafebool(rw["UseForEnrich"]);
            }

            return result;
        }

    }
}
