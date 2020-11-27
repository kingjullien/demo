using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class ImportJobQueueRepository : RepositoryParent
    {
        public ImportJobQueueRepository(string connectionString) : base(connectionString) { }

        internal int InsertImportJobQueue(ImportJobQueueEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InsertImportJobQueue";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceType", obj.SourceType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceFileName", obj.SourceFileName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ColumnMapping", obj.ColumnMapping.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportType", obj.ImportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(obj.Tags) ? null : obj.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InLanguage", string.IsNullOrEmpty(obj.InLanguage) ? null : obj.InLanguage.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", obj.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderType", obj.ProvidersType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsHeader", obj.IsHeader.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        internal List<ImportJobQueueEntity> GetImportJobSettings()
        {
            List<ImportJobQueueEntity> results = new List<ImportJobQueueEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetImportJobSettings";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportJobQueueAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void UpdateImportDataSetting(ImportJobQueueEntity obj, bool IsPorcessStart, bool IsRevert, int RetryCount, string ErrorMessage)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateImportDataSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", obj.IsProcessComplete.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsPorcessStart", IsPorcessStart.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsRevert", IsRevert.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RetryCount ", RetryCount.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ErrorMessage", !string.IsNullOrEmpty(Convert.ToString(ErrorMessage)) ? ErrorMessage.ToString() : null, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void UpdateImportFileSentMail(ImportJobQueueEntity obj, bool IsEmailSent)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateImportFileSentMail";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEmailSent", IsEmailSent.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal List<ImportJobQueueEntity> GetImportJobSettingsByUserId(int? UserId, int ApplicationId, int SortOrder, int PageNumber, int PageSize, out int TotalRecords, string ProvidersType)
        {
            List<ImportJobQueueEntity> results = new List<ImportJobQueueEntity>();
            try
            {
                TotalRecords = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetImportJobSettingsByUserId";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", !string.IsNullOrEmpty(Convert.ToString(UserId)) ? UserId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderType", ProvidersType.ToString(), SQLServerDatatype.VarcharDataType));
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportJobQueueAdapter().Adapt(dt);
                    TotalRecords = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal string ImportJobSettingNotifications(int ApplicationId, int UserId, string ProviderType)
        {
            try
            {
                string results = "";
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.ImportJobSettingNotifications";     // MP-846 Admin database cleanup and code cleanup.
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
                            results = dt.Rows[i]["SourceFileName"].ToString();
                        }
                        else
                        {
                            results += ", " + dt.Rows[i]["SourceFileName"].ToString();
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
        internal List<ImportJobQueueEntity> UpdateImportJobSettingNotificationsStatus(int ApplicationId, int UserId)
        {
            List<ImportJobQueueEntity> results = new List<ImportJobQueueEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateImportJobSettingNotificationsStatus";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportJobQueueAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
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
