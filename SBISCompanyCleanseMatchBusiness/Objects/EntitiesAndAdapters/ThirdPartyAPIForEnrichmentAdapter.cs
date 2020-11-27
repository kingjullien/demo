using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ThirdPartyAPIForEnrichmentAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ThirdPartyAPIForEnrichmentEntity> Adapt(DataTable dt)
        {
            List<ThirdPartyAPIForEnrichmentEntity> results = new List<ThirdPartyAPIForEnrichmentEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ThirdPartyAPIForEnrichmentEntity cust = new ThirdPartyAPIForEnrichmentEntity();
                cust = AdaptItems(rw);
                results.Add(cust);
            }
            return results;
        }
        public ThirdPartyAPIForEnrichmentEntity AdaptItems(DataRow rw)
        {
            ThirdPartyAPIForEnrichmentEntity result = new ThirdPartyAPIForEnrichmentEntity();

            if (rw.Table.Columns["CredentialName"] != null)
            {
                result.CredentialName = SafeHelper.GetSafestring(rw["CredentialName"]);
            }

            if (rw.Table.Columns["CredentialId"] != null)
            {
                result.CredentialId = SafeHelper.GetSafeint(rw["CredentialId"]);
            }

            if (rw.Table.Columns["DnBAPIId"] != null)
            {
                result.DnBAPIId = SafeHelper.GetSafeint(rw["DnBAPIId"]);
            }

            if (rw.Table.Columns["EnrichmentType"] != null)
            {
                result.EnrichmentType = SafeHelper.GetSafestring(rw["EnrichmentType"]);
            }

            if (rw.Table.Columns["DnBAPIName"] != null)
            {
                result.DnBAPIName = SafeHelper.GetSafestring(rw["DnBAPIName"]);
            }

            if (rw.Table.Columns["APIType"] != null)
            {
                result.APIType = SafeHelper.GetSafestring(rw["APIType"]);
            }

            if (rw.Table.Columns["APIFamily"] != null)
            {
                result.APIFamily = SafeHelper.GetSafestring(rw["APIFamily"]);
            }

            return result;
        }
    }
}
