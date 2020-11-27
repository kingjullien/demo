using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ThirdPartyAPICredentialsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ThirdPartyAPICredentialsEntity> Adapt(DataTable dt)
        {
            List<ThirdPartyAPICredentialsEntity> results = new List<ThirdPartyAPICredentialsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ThirdPartyAPICredentialsEntity cust = new ThirdPartyAPICredentialsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }


        public ThirdPartyAPICredentialsEntity AdaptItem(DataRow rw)
        {
            ThirdPartyAPICredentialsEntity result = new ThirdPartyAPICredentialsEntity();
            if (rw.Table.Columns["CredentialId"] != null)
            {
                result.CredentialId = SafeHelper.GetSafeint(rw["CredentialId"]);
            }

            if (rw.Table.Columns["CredentialName"] != null)
            {
                result.CredentialName = SafeHelper.GetSafestring(rw["CredentialName"]);
            }

            if (rw.Table.Columns["ThirdPartyProviderCode"] != null)
            {
                result.ThirdPartyProviderCode = SafeHelper.GetSafestring(rw["ThirdPartyProviderCode"]);
            }

            if (rw.Table.Columns["ThirdPartyProvider"] != null)
            {
                result.ThirdPartyProvider = SafeHelper.GetSafestring(rw["ThirdPartyProvider"]);
            }

            if (rw.Table.Columns["APICredential"] != null)
            {
                result.APICredential = SafeHelper.GetSafestring(rw["APICredential"]);
            }

            if (rw.Table.Columns["APIPassword"] != null)
            {
                result.APIPassword = SafeHelper.GetSafestring(rw["APIPassword"]);
            }

            if (rw.Table.Columns["GetAuthTokenURL"] != null)
            {
                result.GetAuthTokenURL = SafeHelper.GetSafestring(rw["GetAuthTokenURL"]);
            }

            if (rw.Table.Columns["AuthToken"] != null)
            {
                result.AuthToken = SafeHelper.GetSafestring(rw["AuthToken"]);
            }

            if (rw.Table.Columns["AuthTokenExpirationDate"] != null)
            {
                result.AuthTokenExpirationDate = SafeHelper.GetSafeDateTime(rw["AuthTokenExpirationDate"]);
            }

            if (rw.Table.Columns["APIType"] != null)
            {
                result.APIType = SafeHelper.GetSafestring(rw["APIType"]);
            }

            if (rw.Table.Columns["ErrorCode"] != null)
            {
                result.ErrorCode = SafeHelper.GetSafestring(rw["ErrorCode"]);
            }

            if (rw.Table.Columns["ErrorDescription"] != null)
            {
                result.ErrorDescription = SafeHelper.GetSafestring(rw["ErrorDescription"]);
            }

            if (rw.Table.Columns["ErrorDateTime"] != null)
            {
                result.ErrorDateTime = SafeHelper.GetSafeDateTime(rw["ErrorDateTime"]);
            }

            if (rw.Table.Columns["MinConfidenceCode"] != null)
            {
                result.MinConfidenceCode = SafeHelper.GetSafeint(rw["MinConfidenceCode"]);
            }

            if (rw.Table.Columns["MaxCandidateQty"] != null)
            {
                result.MaxCandidateQty = SafeHelper.GetSafeint(rw["MaxCandidateQty"]);
            }

            if (rw.Table.Columns["Tag"] != null)
            {
                result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            }

            if (rw.Table.Columns["Id"] != null)
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            return result;
        }
    }
}
