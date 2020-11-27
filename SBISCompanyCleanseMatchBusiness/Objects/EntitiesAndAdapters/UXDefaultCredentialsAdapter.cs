using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UXDefaultCredentialsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<UXDefaultCredentialsEntity> Adapt(DataTable dt)
        {
            List<UXDefaultCredentialsEntity> results = new List<UXDefaultCredentialsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UXDefaultCredentialsEntity cust = new UXDefaultCredentialsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public UXDefaultCredentialsEntity AdaptItem(DataRow rw)
        {
            UXDefaultCredentialsEntity result = new UXDefaultCredentialsEntity();

            if (rw.Table.Columns["CredentialId"] != null)
            {
                result.CredentialId = SafeHelper.GetSafeint(rw["CredentialId"]);
            }

            if (rw.Table.Columns["Code"] != null)
            {
                result.Code = SafeHelper.GetSafestring(rw["Code"]);
            }

            return result;
        }
    }
}
