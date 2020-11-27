using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class SystemNotificationRepository : RepositoryParent
    {
        public SystemNotificationRepository(string ConnectionString) : base(ConnectionString) { }

        internal List<SystemNotificationEntity> GetAllNotification()
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllNotification";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SystemNotificationAdapter().Adapt(dt);
                    foreach (SystemNotificationEntity comp in results)
                    {
                        results = new SystemNotificationAdapter().Adapt(dt);
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



        // Gets all Notifications
        internal List<SystemNotificationEntity> GetAllNotificationPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllNotificationPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SystemNotificationAdapter().Adapt(dt);
                }
                TotalCount = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<SystemNotificationEntity> GetActiveNotification(bool IsActive)
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetAllNotification";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@IsActive", IsActive.ToString().Trim(), SQLServerDatatype.BitDataType));


                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SystemNotificationAdapter().Adapt(dt);
                    foreach (SystemNotificationEntity comp in results)
                    {
                        results = new SystemNotificationAdapter().Adapt(dt);
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
        #region Add/Edit Notification
        internal SystemNotificationEntity GetNotificationByMessageId(int MessageId)
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllNotification";
                sproc.StoredProceduresParameter.Add(GetParam("@MessageId", MessageId.ToString().Trim(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SystemNotificationAdapter().Adapt(dt);
                    foreach (SystemNotificationEntity comp in results)
                    {
                        results = new SystemNotificationAdapter().Adapt(dt);
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
        internal string InsertUpdateSystemNotification(SystemNotificationEntity objNotification)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertUpdateSystemNotification";

                if (objNotification.MessageId > 0)
                {
                    sproc.StoredProceduresParameter.Add(GetParam("@MessageId", objNotification.MessageId.ToString().Trim(), SQLServerDatatype.IntDataType));
                }
                sproc.StoredProceduresParameter.Add(GetParam("@Message", objNotification.Message.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StartDateTime", objNotification.StartDateTime.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EndDateTime", objNotification.EndDateTime.ToString().Trim(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FontColor", objNotification.FontColor.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@IsActive", objNotification.IsActive.ToString().Trim(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        #endregion
        internal string ActiveNotificationMessage(int MessageId, bool IsActive, bool IsActiveUpdate)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.InsertUpdateSystemNotification";
                sproc.StoredProceduresParameter.Add(GetParam("@MessageId", MessageId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsActive", IsActive.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsActiveUpdate", IsActiveUpdate.ToString().Trim(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        internal void DismissNotificationByUsers(int UserId, string NotificationId, bool IsDismiss)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.DissmissNotificationByUsers";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationId", NotificationId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDismiss", IsDismiss.ToString().Trim(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateDismissNotificationByUsers(int UserId, string NotificationId, bool IsDismiss, bool IsUpdate)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.DissmissNotificationByUsers";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationId", NotificationId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDismiss", IsDismiss.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsUpdate", IsUpdate.ToString().Trim(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetDismissNotificationByUser(int UserId)
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.GetDissmissNotificationByUserId";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                result = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return result;
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
