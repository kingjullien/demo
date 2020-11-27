using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class NotificationRepository : RepositoryParent
    {
        public NotificationRepository(string connectionString) : base(connectionString) { }

        internal int InsertNotificationProfile(NotificationProfileEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertNotificationProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileName", obj.NotificationProfileName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileDescription", obj.NotificationProfileDescription, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeliveryMode", obj.DeliveryMode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeliveryChannelUserPreferenceName", obj.DeliveryChannelUserPreferenceName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeliveryFrequency", obj.DeliveryFrequency, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeliveryFormat", obj.DeliveryFormat, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StopDeliveryIndicator", obj.StopDeliveryIndicator.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompressedProductIndicator", obj.CompressedProductIndicator.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InquiryReferenceText", obj.InquiryReferenceText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultID", obj.ResultID, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SeverityText", obj.SeverityText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResultText", obj.ResultText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileID", obj.NotificationProfileID.ToString(), SQLServerDatatype.IntDataType));
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

        internal List<NotificationProfileEntity> GetNotificationProfile(int CredentialId)
        {
            List<NotificationProfileEntity> results = new List<NotificationProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetNotificationProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
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
        internal void DeleteNotificationProfile(int id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteNotificationProfile";
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileID", id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal NotificationProfileEntity GetNotificationProfileById(int id)
        {
            NotificationProfileEntity results = new NotificationProfileEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetNotificationProfileById";
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileID", id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    NotificationProfileAdapter ta = new NotificationProfileAdapter();
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

        internal List<NotificationProfileEntity> GetAllNotificationProfile()
        {
            List<NotificationProfileEntity> results = new List<NotificationProfileEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllNotificationProfile";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    NotificationProfileAdapter ta = new NotificationProfileAdapter();
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
        internal bool CheckNotificationName(int ProfileID, string NotificationProfileName)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.CheckNotificationName";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileID", ProfileID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationProfileName", NotificationProfileName.ToString(), SQLServerDatatype.VarcharDataType));
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
