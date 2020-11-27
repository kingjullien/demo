using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class ImportJobDataRepository : RepositoryParent
    {
        public ImportJobDataRepository(string connectionString) : base(connectionString) { }

        internal List<ImportJobDataEntity> GetNewFileImportRequestByUserID(int? UserId, string Section)
        {
            List<ImportJobDataEntity> results = new List<ImportJobDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.GetNewFileImportRequestByUserID";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", !string.IsNullOrEmpty(Convert.ToString(UserId)) ? UserId.ToString() : "0", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Section, SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportJobDataAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<ImportFileTemplates> GetAllImportFileTemplates()
        {
            List<ImportFileTemplates> results = new List<ImportFileTemplates>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetAllImportFileTemplates";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportJobDataAdapter().AdaptTemplate(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal bool InsertNewFileImportRequest(ImportJobDataEntity importJobData)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.InsertNewFileImportRequest";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportType", importJobData.ImportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceType", importJobData.SourceType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExcelWorksheetName", string.IsNullOrEmpty(importJobData.ExcelWorksheetName) ? null : importJobData.ExcelWorksheetName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(importJobData.Delimiter) ? null : importJobData.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", importJobData.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourceFileName", importJobData.SourceFileName.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SourcePath", importJobData.SourcePath.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileColumnMetadata", string.IsNullOrEmpty(importJobData.FileColumnMetadata) ? null : importJobData.FileColumnMetadata.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ColumnMappings", importJobData.ColumnMappings.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(importJobData.Tags) ? null : importJobData.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InLanguage", string.IsNullOrEmpty(importJobData.InLanguage) ? null : importJobData.InLanguage.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", importJobData.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessStatusId", importJobData.ProcessStatusId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessStartDateTime", null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessEndDateTime", null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RetryCount", "0", SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ErrorMessage", null, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsUnicode", importJobData.IsUnicode.HasValue ? importJobData.IsUnicode.ToString() : null, SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UnMappedColumns", string.IsNullOrEmpty(importJobData.UnMappedColumns) ? null : importJobData.UnMappedColumns, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ClientFileName", string.IsNullOrEmpty(importJobData.ClientFileName) ? null : importJobData.ClientFileName, SQLServerDatatype.NvarcharDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal string UpsertImportFileTemplates(ImportFileTemplates importJobData)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpsertImportFileTemplates";
                sproc.StoredProceduresParameter.Add(GetParam("@TemplateId", importJobData.TemplateId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileFormat", importJobData.FileFormat.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportType", importJobData.ImportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TemplateName", importJobData.TemplateName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", importJobData.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(importJobData.Delimiter) ? null : importJobData.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExcelWorksheetName", string.IsNullOrEmpty(importJobData.ExcelWorksheetName) ? null : importJobData.ExcelWorksheetName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileColumnMetadata", string.IsNullOrEmpty(importJobData.FileColumnMetadata) ? null : importJobData.FileColumnMetadata.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ColumnMappings", importJobData.ColumnMappings.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(importJobData.Tags) ? null : importJobData.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InLanguage", string.IsNullOrEmpty(importJobData.InLanguage) ? null : importJobData.InLanguage.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", importJobData.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsUnicode", importJobData.IsUnicode.HasValue ? importJobData.IsUnicode.ToString() : null, SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UnMappedColumns", string.IsNullOrEmpty(importJobData.UnMappedColumns) ? null : importJobData.UnMappedColumns.ToString(), SQLServerDatatype.NvarcharDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                Message = "Error :" + ex.Message;
            }
            return Message;
        }
        internal List<DashboardBackgroundProcessStatsEntity> DashboardV2GetBackgroundProcessStats()
        {
            List<DashboardBackgroundProcessStatsEntity> result = new List<DashboardBackgroundProcessStatsEntity>();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetBackgroundProcessStats";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new DashboardBackgroundProcessStatsAdapter().Adapt(dt);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal ImportFileTemplates GetImportFileTemplateByTemplateId(int templateId)
        {
            ImportFileTemplates result = new ImportFileTemplates();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[cfg].[GetImportFileTemplateByTemplateId]";
                sproc.StoredProceduresParameter.Add(GetParam("@TemplateId", templateId.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    ImportJobDataAdapter IJ = new ImportJobDataAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        result = IJ.AdaptTemplateItem(rw, dt);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal void DeleteImportFileTemplateByTemplateId(int TemplateId, string TemplateName, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.DeleteImportFileTemplateByTemplateId";
                sproc.StoredProceduresParameter.Add(GetParam("@TemplateId", Convert.ToString(TemplateId), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TemplateName", TemplateName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
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
