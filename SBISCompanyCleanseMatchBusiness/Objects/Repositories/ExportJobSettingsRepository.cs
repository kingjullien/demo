using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class ExportJobSettingsRepository : RepositoryParent
    {
        public ExportJobSettingsRepository(string connectionString) : base(connectionString) { }

        internal int InserExportJobSettings(ExportJobSettingsEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InserExportJobSettings";    // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(obj.Tags) ? null : obj.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Input", string.IsNullOrEmpty(obj.Input) ? null : obj.Input.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(obj.LOBTag) ? null : obj.LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchOutPut", obj.MatchOutPut.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Enrichment", obj.Enrichment.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ActiveDataQueue", obj.ActiveDataQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", obj.MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Format", obj.Format == null ? null : obj.Format.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(obj.Delimiter) ? null : obj.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecID", string.IsNullOrEmpty(obj.SrcRecID) ? null : obj.SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsExactatch", string.IsNullOrEmpty(obj.SrcRecIdExactMatch.ToString()) ? "false" : obj.SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileName", string.IsNullOrEmpty(obj.FilePath.ToString()) ? "" : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LCMQueue", obj.LCMQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NoMatchQueue", obj.NoMatchQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TrasferedDuns", obj.TrasferedDuns.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", obj.ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyTree", obj.CompanyTree.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", obj.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasTextQualifierToAll", obj.HasTextQualifierToAll.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        internal string InserMonitoringNotificationsJobSettings(MonitoringNotificationJobSettingsEntity obj)
        {
            string result = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InserMonitoringNotificationsJobSettings";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Format", obj.Format == null ? null : obj.Format.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileName", string.IsNullOrEmpty(obj.FilePath.ToString()) ? "" : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", obj.MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(obj.Delimiter) ? null : obj.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", string.IsNullOrEmpty(obj.APILayer) ? null : obj.APILayer.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsMonitoringNotifications", obj.IsMonitoringNotifications.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", obj.ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", obj.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasTextQualifierToAll", obj.HasTextQualifierToAll.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                result = "";
            }
            catch (Exception Ex)
            {
                result = Ex.Message;
                return result;
            }

            return result;
        }


        internal List<ExportJobSettingsEntity> GetExportJobSettings()
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettings";  // MP-846 Admin database cleanup and code cleanup.
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<MonitoringNotificationJobSettingsEntity> GetMonitoringExportJobSettings()
        {
            List<MonitoringNotificationJobSettingsEntity> results = new List<MonitoringNotificationJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetMonitoringExportJobSettings";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringNotificationJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }


        internal void UpdateExportJobSettings(ExportJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert, int RetryCount, string ErrorMessage)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", obj.IsProcessComplete.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", obj.FilePath == null ? null : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsPorcessStart", IsPorcessStart.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsRevert", IsRevert.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RetryCount", RetryCount.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ErrorMessage", !string.IsNullOrEmpty(Convert.ToString(ErrorMessage)) ? ErrorMessage.ToString() : null, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateMonitoringExportJobSettings(MonitoringNotificationJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert, int RetryCount, string ErrorMessage)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", obj.IsProcessComplete.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", obj.FilePath == null ? null : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsPorcessStart", IsPorcessStart.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsRevert", IsRevert.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RetryCount ", RetryCount.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ErrorMessage", !string.IsNullOrEmpty(Convert.ToString(ErrorMessage)) ? ErrorMessage.ToString() : null, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateExportJobSettingsSentMail(ExportJobSettingsEntity obj, bool IsEmailSent)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettingsSentMail";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEmailSent", IsEmailSent.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void UpdateMonitoringExportJobSettingsSentMail(MonitoringNotificationJobSettingsEntity obj, bool IsEmailSent)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettingsSentMail";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEmailSent", IsEmailSent.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal List<ExportJobSettingsEntity> GetExportJobSettingsByUserId(string ExportType, int? UserId, int ApplicationId)
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettingsByUserId";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", !string.IsNullOrEmpty(Convert.ToString(UserId)) ? UserId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<MonitoringNotificationJobSettingsEntity> GetMonitoringExportJobSettingsByUserId(int? UserId, int ApplicationId)
        {
            List<MonitoringNotificationJobSettingsEntity> results = new List<MonitoringNotificationJobSettingsEntity>();
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetMonitoringExportJobSettingsByUserId";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", !string.IsNullOrEmpty(Convert.ToString(UserId)) ? UserId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MonitoringNotificationJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }


        // Deletes export job queue
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal void DeleteExportJobSettingsForClients(ExportJobSettingsEntity obj)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.DeleteExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));


                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal void DeleteExportJobSettingsForMaster(ExportJobSettingsEntity obj)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.DeleteExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));


                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Monitor Export Job Queue Details
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal ExportJobSettingsEntity GetExportJobSettingsByIdByClient(int Id)
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            try
            {
                DataTable dt;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettingsById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results.FirstOrDefault();
        }
        internal void UpdateExportJobSettingsForDownload(int Id, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateExportJobSettingsForDownload";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal string ExportJobSettingNotifications(int ApplicationId, int UserId, string ProviderType)
        {
            try
            {
                string results = "";
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.ExportJobSettingNotifications";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderType", ProviderType.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(results))
                        {
                            results = dt.Rows[i]["FilePath"].ToString();
                        }
                        else
                        {
                            results += ", " + dt.Rows[i]["FilePath"].ToString();
                        }
                    }
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateExportJobSettingNotificationsStatus(int ApplicationId, int UserId, string ExportType)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateExportJobSettingNotificationsStatus";    // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }




        internal string IsExistExportFileName(string fileName, int Applicationid)
        {
            string result = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.IsExistExportFileName";     // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", fileName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", Applicationid.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal int UnflagExportedRecords(ReExportDataEntity objReExport)
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UnflagExportedRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", objReExport.CountryGroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(objReExport.Tags) ? null : objReExport.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(objReExport.ImportProcess) ? null : objReExport.ImportProcess.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(objReExport.SrcRecordId) ? null : objReExport.SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordIdList", string.IsNullOrEmpty(objReExport.SrcRecordIdList) ? null : objReExport.SrcRecordIdList.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RecordType", objReExport.Recordtype.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objReExport.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountsOnly", objReExport.GetCountsOnly.ToString(), SQLServerDatatype.BitDataType));

                if (objReExport.GetCountsOnly == false)
                {
                    sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                }
                else
                {
                    count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }
        internal List<ExportJobSettingsEntity> GetPurgeExportedFiles()
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetPurgeExportedFiles";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void CancelExportJobSettings(ExportJobSettingsEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.CancelExportJobSettings";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
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
