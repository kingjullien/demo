using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;


namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class CDSEnvironmentAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CDSEnvironmentEntity> Adapt(DataTable dt)
        {
            List<CDSEnvironmentEntity> results = new List<CDSEnvironmentEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CDSEnvironmentEntity cust = new CDSEnvironmentEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public CDSEnvironmentEntity AdaptItem(DataRow rw)
        {
            CDSEnvironmentEntity result = new CDSEnvironmentEntity();
            if (rw.Table.Columns["EnvironmentId"] != null)
            {
                result.TenantId = new Guid(SafeHelper.GetSafestring(rw["EnvironmentId"]));
            }

            if (rw.Table.Columns["EnvironmentName"] != null)
            {
                result.EnvironmentName = SafeHelper.GetSafestring(rw["EnvironmentName"]);
            }

            if (rw.Table.Columns["OrganizationUrl"] != null)
            {
                result.OrganizationUrl = SafeHelper.GetSafestring(rw["OrganizationUrl"]);
            }

            return result;
        }
    }
}

