using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class UserPreferenceRepository : RepositoryParent
    {
        public UserPreferenceRepository(string connectionString) : base(connectionString) { }

        internal int InsertUpdateUserPreference(UserPreferenceEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertUpdateUserPreference";
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceID", obj.PreferenceID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceName", obj.PreferenceName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceDescription", obj.PreferenceDescription.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceType", obj.PreferenceType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceValue", obj.PreferenceValue.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DefaultPreference", obj.DefaultPreference.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationAreaName", obj.ApplicationAreaName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", obj.ResultID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SeverityText", obj.SeverityText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultText", obj.ResultText.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestDateTime", obj.RequestDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseDateTime", obj.ResponseDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CreatedBy", obj.CreatedBy.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedBy", obj.ModifiedBy.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDeleted", obj.IsDeleted.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.IntDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }


        internal List<UserPreferenceEntity> GetUserPreference(int CredentialId)
        {
            List<UserPreferenceEntity> results = new List<UserPreferenceEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserPreference";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UserPreferenceAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal string DeleteUserPreference(int id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUserPreference";
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceID", id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        internal UserPreferenceEntity GetUserPreferenceById(int id)
        {
            UserPreferenceEntity results = new UserPreferenceEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserPreferenceById";
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceID", id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    UserPreferenceAdapter ta = new UserPreferenceAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.AdaptItem(rw, dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void UpdateUserPreference(UserPreferenceEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateUserPreference";
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceID", obj.PreferenceID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceName", obj.PreferenceName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceDescription", obj.PreferenceDescription.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceType", obj.PreferenceType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceValue", obj.PreferenceValue.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DefaultPreference", obj.DefaultPreference.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationAreaName", obj.ApplicationAreaName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedBy", obj.ModifiedBy.ToString(), SQLServerDatatype.IntDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal List<UserPreferenceEntity> GetAllUserPreference()
        {
            List<UserPreferenceEntity> results = new List<UserPreferenceEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllUserPreference";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    UserPreferenceAdapter ta = new UserPreferenceAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<UserPreferenceEntity> GetActiveUserPreference()
        {
            List<UserPreferenceEntity> results = new List<UserPreferenceEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetActiveUserPreference";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    UserPreferenceAdapter ta = new UserPreferenceAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal bool CheckUserPreferenceUsed(string UserPreference)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CheckUserPreferenceUsed";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserPreference", UserPreference.ToString(), SQLServerDatatype.VarcharDataType));
                return Convert.ToBoolean(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));

            }
            catch (SqlException)
            {
                throw;
            }

        }
        internal bool CheckUserPreferenceName(int PreferenceID, string UserPreference)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CheckUserPreferenceName";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@PreferenceID", PreferenceID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserPreference", UserPreference.ToString(), SQLServerDatatype.VarcharDataType));
                return Convert.ToBoolean(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));

            }
            catch (SqlException)
            {
                throw;
            }

        }
        #region "Common Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        #endregion
    }
}
