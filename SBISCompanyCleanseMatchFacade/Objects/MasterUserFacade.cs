using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MasterUserFacade : FacadeParent
    {
        MasterUserBussiness rep;
        public MasterUserFacade(string connectionString, string UserName = "") : base(connectionString)
        {
            try
            {
                rep = new MasterUserBussiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new MasterUserBussiness(Connection);
            }
        }


        #region Master Users
        public int AddUser(MasterUserEntity user)
        {
            return rep.AddUser(user.UserName, user.clientUserName, user.PasswordHash, user.SecurityStamp, user.PasswordResetToken, user.PasswordTokenCreateDate, user.SSOUser, user.IsAdmin);
        }
        public void UpdateUser(MasterUserEntity user)
        {
            rep.UpdateUser(user.UserId, user.UserName, user.clientUserName, user.PasswordHash, user.SecurityStamp, user.PasswordResetToken, user.PasswordTokenCreateDate, user.SSOUser, user.IsAdmin);
        }
        public List<MasterUserEntity> GetAll()
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            results = rep.GetAll();
            return results;
        }
        // Get Master Users
        public List<MasterUserEntity> GetUsersPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            return rep.GetUsersPaging(SortOrder, PageNumber, PageSize, out TotalCount);
        }


        public MasterUserEntity GetUserById(int userId)
        {
            MasterUserEntity results = new MasterUserEntity();
            results = rep.GetUserById(userId);
            return results;
        }
        public MasterUserEntity create(string username)
        {
            MasterUserEntity results = new MasterUserEntity();
            results = rep.Create(username);
            return results;
        }
        public MasterUserEntity GetUserBySMALUser(string SSOUser)
        {
            return rep.GetUserBySMALUser(SSOUser);
        }

        // Inserts User Role
        public void InsertUserRole(string Menu, int RoleId, int UserId)
        {
            rep.InsertUserRole(Menu, RoleId, UserId);
        }
        // Deletes User Role
        public void DeleteUserRoles(int UserId)
        {
            rep.DeleteUserRoles(UserId);
        }
        #endregion
    }
}
