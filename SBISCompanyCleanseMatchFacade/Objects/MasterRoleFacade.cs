using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MasterRoleFacade : FacadeParent
    {
        MasterRoleBussiness rep;
        public MasterRoleFacade(string connectionString, string UserName = "") : base(connectionString)
        {
            try
            {
                rep = new MasterRoleBussiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new MasterRoleBussiness(Connection);
            }
        }
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
        // Gets Role
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
