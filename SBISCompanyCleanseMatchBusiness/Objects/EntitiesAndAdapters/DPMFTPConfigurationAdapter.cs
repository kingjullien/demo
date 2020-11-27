using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{

    class DPMFTPConfigurationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DPMFTPConfigurationEntity> Adapt(DataTable dt)
        {
            List<DPMFTPConfigurationEntity> results = new List<DPMFTPConfigurationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DPMFTPConfigurationEntity matchCode = new DPMFTPConfigurationEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public DPMFTPConfigurationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            DPMFTPConfigurationEntity result = new DPMFTPConfigurationEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("Host"))
            {
                result.Host = SafeHelper.GetSafestring(rw["Host"]);
            }

            if (dt.Columns.Contains("Port"))
            {
                if (rw["Port"] == DBNull.Value)
                {
                    result.Port = null;
                }
                else
                {
                    result.Port = SafeHelper.GetSafeint(rw["Port"]);
                }
            }
            if (dt.Columns.Contains("Password"))
            {
                result.Password = SafeHelper.GetSafestring(rw["Password"]);
            }

            if (dt.Columns.Contains("UserName"))
            {
                result.UserName = SafeHelper.GetSafestring(rw["UserName"]);
            }

            return result;
        }
    }
}
