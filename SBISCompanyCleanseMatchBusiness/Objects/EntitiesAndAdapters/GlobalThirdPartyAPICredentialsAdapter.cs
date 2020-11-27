using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class GlobalThirdPartyAPICredentialsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<GlobalThirdPartyAPICredentialsEntity> Adapt(DataTable dt)
        {
            List<GlobalThirdPartyAPICredentialsEntity> results = new List<GlobalThirdPartyAPICredentialsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                GlobalThirdPartyAPICredentialsEntity cust = new GlobalThirdPartyAPICredentialsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public GlobalThirdPartyAPICredentialsEntity AdaptItem(DataRow rw)
        {
            GlobalThirdPartyAPICredentialsEntity result = new GlobalThirdPartyAPICredentialsEntity();

            if (rw.Table.Columns["ThirdPartyProvider"] != null)
            {
                result.ThirdPartyProvider = SafeHelper.GetSafestring(rw["ThirdPartyProvider"]);
            }

            if (rw.Table.Columns["Code"] != null)
            {
                result.Code = SafeHelper.GetSafestring(rw["Code"]);
            }

            if (rw.Table.Columns["APICredential"] != null)
            {
                result.APICredential = SafeHelper.GetSafestring(rw["APICredential"]);
            }

            if (rw.Table.Columns["AuthToken"] != null)
            {
                result.AuthToken = SafeHelper.GetSafestring(rw["AuthToken"]);
            }

            if (rw.Table.Columns["APIType"] != null)
            {
                result.APIType = SafeHelper.GetSafestring(rw["APIType"]);
            }

            return result;
        }
    }
}
