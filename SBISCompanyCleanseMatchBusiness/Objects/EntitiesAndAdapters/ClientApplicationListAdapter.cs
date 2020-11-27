using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class ClientApplicationListAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ClientApplicationListEntity> AdaptLists(DataTable dt)
        {
            List<ClientApplicationListEntity> results = new List<ClientApplicationListEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ClientApplicationListEntity clientlist = new ClientApplicationListEntity();
                clientlist = AdaptItemReport(rw, dt);
                results.Add(clientlist);
            }
            return results;
        }
        public ClientApplicationListEntity AdaptItemReport(DataRow rw, DataTable dt)
        {
            ClientApplicationListEntity results = new ClientApplicationListEntity();
            if (dt.Columns.Contains("Id"))
            {
                results.ApplicationId = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("LicenseStartDate"))
            {
                results.LicenseStartDate = SafeHelper.GetSafestring(rw["LicenseStartDate"]);
            }

            if (dt.Columns.Contains("Domain"))
            {
                results.SubDomain = SafeHelper.GetSafestring(rw["Domain"]);
            }

            if (dt.Columns.Contains("EndDate"))
            {
                results.LicenseEndDate = SafeHelper.GetSafeDateTime(rw["EndDate"]);
            }

            return results;
        }
    }
}
