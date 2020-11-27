using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MasterUserBussiness : BusinessParent
    {


        MasterUserRepository rep;
        public MasterUserBussiness(string connectionString) : base(connectionString) { rep = new MasterUserRepository(Connection); }


        #region Master Users
        public int AddUser(string UserName, string clientUserName, string PasswordHash, string SecurityStamp, string PasswordResetToken, DateTime PasswordTokenCreateDate, string SSOUser, bool IsAdmin)
        {
            return rep.AddUser(UserName, clientUserName, PasswordHash, SecurityStamp, PasswordResetToken, PasswordTokenCreateDate, SSOUser, IsAdmin);
        }
        public void UpdateUser(int UserId, string UserName, string clientUserName, string PasswordHash, string SecurityStamp, string PasswordResetToken, DateTime PasswordTokenCreateDate, string SSOUser, bool IsAdmin)
        {
            rep.UpdateUser(UserId, UserName, clientUserName, PasswordHash, SecurityStamp, PasswordResetToken, PasswordTokenCreateDate, SSOUser, IsAdmin);
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
        public MasterUserEntity Create(string username)
        {
            MasterUserEntity results = new MasterUserEntity();
            results = rep.Create(username);
            return results;
        }
        public MasterUserEntity GetUserBySMALUser(string SSOUser)
        {
            return rep.GetUserBySMALUser(SSOUser);
        }
        public void InsertUserRole(string Menu, int RoleId, int UserId)
        {
            rep.InsertUserRole(Menu, RoleId, UserId);
        }
        public void DeleteUserRoles(int UserId)
        {
            rep.DeleteUserRoles(UserId);
        }
        #endregion
    }
}
