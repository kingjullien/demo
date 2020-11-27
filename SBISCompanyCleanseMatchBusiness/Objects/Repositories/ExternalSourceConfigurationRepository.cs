using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class ExternalSourceConfigurationRepository : RepositoryParent
    {
        public ExternalSourceConfigurationRepository(string connectionString) : base(connectionString) { }
        internal List<DataSourceConfigurationEntity> GetExternalDataStore(int? Id)
        {
            List<DataSourceConfigurationEntity> results = new List<DataSourceConfigurationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetExternalDataStore";
                sproc.StoredProceduresParameter.Add(GetParam("@id", Id == null ? null : Convert.ToString(Id), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataSourceConfigurationAdapter ta = new DataSourceConfigurationAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        internal int InsertExternalDataStore(DataSourceConfigurationEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.InsertExternalDataStore";
                sproc.StoredProceduresParameter.Add(GetParam("@id", obj.Id == null ? "0" : Convert.ToString(obj.Id), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@externalDataStoreName", !string.IsNullOrEmpty(obj.ExternalDataStoreName) ? obj.ExternalDataStoreName : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@externalDataStoreTypeId", obj.externalDataStoreType.Id == null ? "0" : Convert.ToString(obj.externalDataStoreType.Id), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@dataStoreConfiguration", !string.IsNullOrEmpty(obj.DataStoreConfiguration) ? obj.DataStoreConfiguration : null, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@userId", obj.UserId == null ? "0" : Convert.ToString(obj.UserId), SQLServerDatatype.IntDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        internal List<ExternalDataStoreType> GetExternalDataSourceType(int? Id)
        {
            List<ExternalDataStoreType> results = new List<ExternalDataStoreType>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ref.GetExternalDataSourceType";
                sproc.StoredProceduresParameter.Add(GetParam("@id", Id == null ? null : Convert.ToString(Id), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataSourceConfigurationAdapter ta = new DataSourceConfigurationAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.AdaptExternalDataStore(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }
        internal void DeleteExternalDataStore(int externalDataStoreId, int userId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.DeleteExternalDataStore";
                sproc.StoredProceduresParameter.Add(GetParam("@externalDataStoreId", Convert.ToString(externalDataStoreId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@userId", Convert.ToString(userId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region "Other Methods"
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
