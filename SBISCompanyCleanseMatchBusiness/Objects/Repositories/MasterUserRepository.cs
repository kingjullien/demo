using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MasterUserRepository : RepositoryParent
    {

        public MasterUserRepository(string ConnectionString) : base(ConnectionString) { }

        #region Master Users
        internal int AddUser(string UserName, string clientUserName, string PasswordHash, string SecurityStamp, string PasswordResetToken, DateTime PasswordTokenCreateDate, string SSOUser, bool IsAdmin)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "mapp.AddUser";

            sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", UserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserName", Convert.ToString(clientUserName), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordHash", PasswordHash.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecurityStamp", Convert.ToString(SecurityStamp), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordResetToken", Convert.ToString(PasswordResetToken), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordTokenCreateDate", Convert.ToString(PasswordTokenCreateDate), SQLServerDatatype.DateTimeDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SSOUser", string.IsNullOrEmpty(SSOUser) ? "" : Convert.ToString(SSOUser), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@IsAdmin", Convert.ToString(IsAdmin), SQLServerDatatype.BitDataType));
            int UserId = 0;
            UserId = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            return UserId;
        }
        internal void UpdateUser(int Userid, string UserName, string clientUserName, string PasswordHash, string SecurityStamp, string PasswordResetToken, DateTime PasswordTokenCreateDate, string SSOUser, bool IsAdmin)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();

            sproc.StoredProcedureName = "mapp.UpdateUser";

            sproc.StoredProceduresParameter.Add(GetParam("@UserId", Userid.ToString().Trim(), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", UserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserName", Convert.ToString(clientUserName), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordHash", PasswordHash.ToString().Trim(), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SecurityStamp", Convert.ToString(SecurityStamp), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordResetToken", Convert.ToString(PasswordResetToken), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@PasswordTokenCreateDate", Convert.ToString(PasswordTokenCreateDate), SQLServerDatatype.DateTimeDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@SSOUser", string.IsNullOrEmpty(SSOUser) ? "" : Convert.ToString(SSOUser), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@IsAdmin", Convert.ToString(IsAdmin), SQLServerDatatype.BitDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal List<MasterUserEntity> GetAll()
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUsers";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUserAdapter().Adapt(dt);
                    foreach (MasterUserEntity comp in results)
                    {
                        results = new MasterUserAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        // Get Master Users
        internal List<MasterUserEntity> GetUsersPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUsersPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUserAdapter().Adapt(dt);
                }
                TotalCount = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }




        internal MasterUserEntity GetUserById(int userId)
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUserById";

                sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUserAdapter().Adapt(dt);
                    foreach (MasterUserEntity comp in results)
                    {
                        results = new MasterUserAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal MasterUserEntity Create(string username)
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUserByEmail";
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", username.ToString(), SQLServerDatatype.NvarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUserAdapter().Adapt(dt);
                    foreach (MasterUserEntity comp in results)
                    {
                        results = new MasterUserAdapter().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }

        internal MasterUserEntity GetUserBySMALUser(string SSOUser)
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUserBySMALUser";
                sproc.StoredProceduresParameter.Add(GetParam("@SSOUser", SSOUser.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUserAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }




        internal void InsertUserRole(string Menu, int RoleId, int UserId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.AddUserRoles";
            sproc.StoredProceduresParameter.Add(GetParam("@Menu", string.IsNullOrEmpty(Menu) ? "" : Convert.ToString(Menu.Trim()), SQLServerDatatype.VarcharDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@RoleId", Convert.ToString(RoleId), SQLServerDatatype.IntDataType));
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

        }

        internal void DeleteUserRoles(int UserId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "mapp.DeleteUserRole";
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

        }
        internal MasterUsersEntity StewMasterUserLogIn(string LoginId, string Password)
        {
            List<MasterUsersEntity> results = new List<MasterUsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetUserByEmail";
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", LoginId.ToString(), SQLServerDatatype.NvarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@Password", Password.ToString(), SQLServerDatatype.NvarcharDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterUsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        #endregion
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }


    }
}
