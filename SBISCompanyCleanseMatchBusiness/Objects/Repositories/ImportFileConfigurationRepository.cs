using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class ImportFileConfigurationRepository : RepositoryParent
    {
        internal ImportFileConfigurationRepository(string connectionString) : base(connectionString) { }
        internal List<ImportFileConfigurationEntity> GetImportFileConfiguration(int? Id)
        {
            List<ImportFileConfigurationEntity> results = new List<ImportFileConfigurationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetImportFileConfiguration";
                if(Id > 0)
                {
                    sproc.StoredProceduresParameter.Add(GetParam("@id", Convert.ToString(Id), SQLServerDatatype.IntDataType));
                }
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ImportFileConfigurationAdapter().Adapt(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        internal string InsertImportFileConfiguration(ImportFileConfigurationEntity objImportFileConfiguration)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.InsertImportFileConfiguration";
                sproc.StoredProceduresParameter.Add(GetParam("@id", Convert.ToString(objImportFileConfiguration.Id), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@configurationName", objImportFileConfiguration.ConfigurationName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@externalDataStoreId", Convert.ToString(objImportFileConfiguration.ExternalDataStoreId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@sourceFolderPath", string.IsNullOrEmpty(objImportFileConfiguration.SourceFolderPath) ? "" : objImportFileConfiguration.SourceFolderPath, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@templateId", Convert.ToString(objImportFileConfiguration.TemplateId), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@fileNamePattern", string.IsNullOrEmpty(objImportFileConfiguration.FileNamePattern) ? "" : objImportFileConfiguration.FileNamePattern, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@postLoadAction", string.IsNullOrEmpty(objImportFileConfiguration.PostLoadAction) ? "" : objImportFileConfiguration.PostLoadAction, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@postLoadActionParameters", string.IsNullOrEmpty(objImportFileConfiguration.PostLoadActionParameters) ? "" : objImportFileConfiguration.PostLoadActionParameters, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@userId", Convert.ToString(objImportFileConfiguration.UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal void DeleteImportFileConfiguration(int id)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.DeleteImportFileConfiguration";
                sproc.StoredProceduresParameter.Add(GetParam("@id", Convert.ToString(id), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
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
