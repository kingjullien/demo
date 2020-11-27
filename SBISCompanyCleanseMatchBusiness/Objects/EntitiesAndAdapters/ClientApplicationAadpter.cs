using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class ClientApplicationAadpter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ClientApplicationEntity> AdaptLists(DataTable dt)
        {
            List<ClientApplicationEntity> results = new List<ClientApplicationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ClientApplicationEntity clientlist = new ClientApplicationEntity();
                clientlist = AdaptItemReport(rw, dt);
                results.Add(clientlist);
            }
            return results;
        }
        public ClientApplicationEntity AdaptItemReport(DataRow rw, DataTable dt)
        {
            ClientApplicationEntity results = new ClientApplicationEntity();
            results.ClientId = SafeHelper.GetSafeint(rw["ClientId"]);
            results.ClientName = SafeHelper.GetSafestring(rw["ClientName"]);
            return results;
        }
    }
}
