using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class DUNSregistrationRepository : RepositoryParent
    {
        public DUNSregistrationRepository(string connectionString) : base(connectionString) { }
        internal int InsertDUNSregistration(DUNSregistrationEntity obj)
        {
            int result = 0;
            try
            {
                var dict = obj.ToDictionary();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertDUNSregistration";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationId", Convert.ToString(obj.MonitoringRegistrationId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringProfileId", Convert.ToString(obj.MonitoringProfileId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileId", Convert.ToString(obj.NotificationProfileId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TradeUpIndicator", Convert.ToString(obj.TradeUpIndicator), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AutoRenewalIndicator", Convert.ToString(obj.AutoRenewalIndicator), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SubjectCategory", obj.SubjectCategory, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerReferenceText", obj.CustomerReferenceText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BillingEndorsementText", obj.BillingEndorsementText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", obj.Tags, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.IntDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        internal List<DUNSregistrationEntity> GetAllDUNSregistration()
        {
            List<DUNSregistrationEntity> results = new List<DUNSregistrationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllDUNSregistration";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    DUNSregistrationAdapter ta = new DUNSregistrationAdapter();
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

        internal List<DUNSregistrationEntity> GetDUNSregistration(int CredentialId)
        {
            List<DUNSregistrationEntity> results = new List<DUNSregistrationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDUNSregistration";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DUNSregistrationAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void DeleteDUNSregistration(int id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteDUNSregistration";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationId", id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DUNSregistrationEntity GetDUNSregistrationById(int id)
        {
            DUNSregistrationEntity results = new DUNSregistrationEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDUNSregistrationById";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationId", id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    DUNSregistrationAdapter ta = new DUNSregistrationAdapter();
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

        internal List<MonitoringProfileEntity> GetAllMonitoringProfileNames()
        {
            List<MonitoringProfileEntity> results = new List<MonitoringProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMonitoringProfileNames";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringProductAdapter().AdaptProfile(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<NotificationProfileEntity> GetAllNotificationProfileNames()
        {
            List<NotificationProfileEntity> results = new List<NotificationProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetNotificationProfileNames";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new NotificationProfileAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal bool CheckMonitoringProfileUsed(int ProfileId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CheckMonitoringProfileUsed";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileId", ProfileId.ToString(), SQLServerDatatype.IntDataType));
                return Convert.ToBoolean(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));

            }
            catch (SqlException)
            {
                throw;
            }

        }

        internal bool CheckNotificationProfileUsed(int ProfileId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CheckNotificationProfileUsed";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@ProfileId", ProfileId.ToString(), SQLServerDatatype.IntDataType));
                return Convert.ToBoolean(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));

            }
            catch (SqlException)
            {
                throw;
            }

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
