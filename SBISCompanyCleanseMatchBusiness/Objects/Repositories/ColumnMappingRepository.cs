using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class ColumnMappingRepository : RepositoryParent
    {
        internal ColumnMappingRepository(string connectionString) : base(connectionString) { }
        #region Command Upload
        internal string InsertUpdateCommandUploadMapping(CommandUploadMappingEntity objCommandMapping)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.InsertUpdateCommandUploadMapping";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", objCommandMapping.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfigurationName", objCommandMapping.ConfigurationName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportType", objCommandMapping.ImportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileFormat", objCommandMapping.FileFormat.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FormatValue", objCommandMapping.Formatvalue == null ? null : objCommandMapping.Formatvalue.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ColumnMapping", objCommandMapping.ColumnMapping.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDefault", objCommandMapping.IsDefault.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objCommandMapping.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", objCommandMapping.Tags == null ? null : objCommandMapping.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InLanguage", objCommandMapping.InLanguage == null ? null : objCommandMapping.InLanguage.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OriginalColumns", objCommandMapping.OriginalColumns == null ? null : objCommandMapping.OriginalColumns.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal List<CommandUploadMappingEntity> GetCommandMapping()
        {
            List<CommandUploadMappingEntity> results = new List<CommandUploadMappingEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCommandMapping";
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CommandUploadMappingAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void DeleteCommandMapping(int Id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteCommandMapping";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal CommandUploadMappingEntity GetCommandMappingById(int Id)
        {
            CommandUploadMappingEntity results = new CommandUploadMappingEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCommandMappingById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    CommandUploadMappingAdapter ta = new CommandUploadMappingAdapter();
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

        #endregion

        #region Command Download
        internal string InsertUpdateCommandDownloadMapping(CommandDownloadMappingEntity objCommandDownloadMapping)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.InsertUpdateCommandDownloadMapping";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", objCommandDownloadMapping.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfigurationName", objCommandDownloadMapping.ConfigurationName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(objCommandDownloadMapping.Tag) ? "" : objCommandDownloadMapping.Tag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(objCommandDownloadMapping.LOBTag) ? "" : objCommandDownloadMapping.LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadMatchOutput", objCommandDownloadMapping.DownloadMatchOutput.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadEnrichmentOutput", objCommandDownloadMapping.DownloadEnrichmentOutput.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadMonitoringUpdates", objCommandDownloadMapping.DownloadMonitoringUpdates.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadActiveDataQueue", objCommandDownloadMapping.DownloadActiveDataQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadTransferDUNS", objCommandDownloadMapping.DownloadTransferDUNS.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadFormat", string.IsNullOrEmpty(objCommandDownloadMapping.DownloadFormat) ? "" : objCommandDownloadMapping.DownloadFormat.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FormatValue", string.IsNullOrEmpty(objCommandDownloadMapping.Formatvalue) ? "" : objCommandDownloadMapping.Formatvalue.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", objCommandDownloadMapping.MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDefault", objCommandDownloadMapping.IsDefault.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", string.IsNullOrEmpty(objCommandDownloadMapping.APILayer) ? "" : objCommandDownloadMapping.APILayer.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsAppendDateTime", objCommandDownloadMapping.IsAppendDateTime.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsFilePrefix", objCommandDownloadMapping.IsFilePrefix.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DateTimeFileFormat", string.IsNullOrEmpty(objCommandDownloadMapping.DateTimeFileFormat) ? "" : objCommandDownloadMapping.DateTimeFileFormat.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilePrefix", string.IsNullOrEmpty(objCommandDownloadMapping.FilePrefix) ? "" : objCommandDownloadMapping.FilePrefix.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderType", string.IsNullOrEmpty(objCommandDownloadMapping.ProviderType) ? "" : objCommandDownloadMapping.ProviderType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadCompanyTree", objCommandDownloadMapping.DownloadCompanyTree.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadLCMQueue", objCommandDownloadMapping.DownloadLCMQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DownloadNoMatchQueue", objCommandDownloadMapping.DownloadNoMatchQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplyTextQualifierToAll", objCommandDownloadMapping.ApplyTextQualifierToAll.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(objCommandDownloadMapping.UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        internal List<CommandDownloadMappingEntity> GetCommandDownloadMapping()
        {
            List<CommandDownloadMappingEntity> results = new List<CommandDownloadMappingEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCommandDownloadMapping";
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CommandDownloadMappingAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void DeleteCommandDownloadMapping(int Id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteCommandDownloadMapping";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal CommandDownloadMappingEntity GetCommandDownloadMappingById(int Id)
        {
            CommandDownloadMappingEntity results = new CommandDownloadMappingEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCommandDownloadMappingById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    CommandDownloadMappingAdapter ta = new CommandDownloadMappingAdapter();
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
        #endregion

        #region "Other Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            return param;
        }
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
