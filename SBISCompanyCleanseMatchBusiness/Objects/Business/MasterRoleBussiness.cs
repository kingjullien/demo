using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MasterRoleBussiness : BusinessParent
    {
        MasterRoleRepository rep;

        public MasterRoleBussiness(string connectionString) : base(connectionString) { rep = new MasterRoleRepository(Connection); }
        #region Role
        // Gets Role list
        public List<MasterRoleEntity> GetRolePaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            return rep.GetRolePaging(SortOrder, PageNumber, PageSize, out TotalCount);
        }
        public MasterRoleEntity GetRoleById(int RoleId)
        {
            return rep.GetRoleById(RoleId);
        }
        public List<MasterRoleEntity> GetRole()
        {
            return rep.GetRole();
        }

        // Deletes selected role
        public void DeleteRole(int RoleId)
        {
            rep.DeleteRole(RoleId);
        }
        // Adds/Edits new role
        public int InsertUpdateRole(MasterRoleEntity obj)
        {
            return rep.InsertUpdateRole(obj);
        }

        #endregion
    }
}
