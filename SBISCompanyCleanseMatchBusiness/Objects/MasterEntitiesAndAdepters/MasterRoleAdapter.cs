using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterRoleAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterRoleEntity> Adapt(DataTable dt)
        {
            List<MasterRoleEntity> results = new List<MasterRoleEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterRoleEntity matchCode = new MasterRoleEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public MasterRoleEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterRoleEntity result = new MasterRoleEntity();
            if (dt.Columns.Contains("RoleId"))
            {
                result.RoleId = SafeHelper.GetSafeint(rw["RoleId"]);
            }

            if (dt.Columns.Contains("RoleName"))
            {
                result.RoleName = SafeHelper.GetSafestring(rw["RoleName"]);
            }

            if (dt.Columns.Contains("Description"))
            {
                result.Description = SafeHelper.GetSafestring(rw["Description"]);
            }

            if (dt.Columns.Contains("IsActive"))
            {
                result.IsActive = SafeHelper.GetSafebool(rw["IsActive"]);
            }

            if (dt.Columns.Contains("IsAdd"))
            {
                result.IsAdd = SafeHelper.GetSafebool(rw["IsAdd"]);
            }

            if (dt.Columns.Contains("IsEdit"))
            {
                result.IsEdit = SafeHelper.GetSafebool(rw["IsEdit"]);
            }

            if (dt.Columns.Contains("IsView"))
            {
                result.IsView = SafeHelper.GetSafebool(rw["IsView"]);
            }

            if (dt.Columns.Contains("IsDelete"))
            {
                result.IsDelete = SafeHelper.GetSafebool(rw["IsDelete"]);
            }

            if (dt.Columns.Contains("CreatedDate"))
            {
                result.CreatedDate = SafeHelper.GetSafeDateTime(rw["CreatedDate"]);
            }

            if (dt.Columns.Contains("LastUpdated"))
            {
                result.LastUpdated = SafeHelper.GetSafeDateTime(rw["LastUpdated"]);
            }

            return result;
        }
    }
}
