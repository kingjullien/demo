using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class DataSourceConfigurationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<DataSourceConfigurationEntity> Adapt(DataTable dt)
        {
            List<DataSourceConfigurationEntity> results = new List<DataSourceConfigurationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DataSourceConfigurationEntity matchCode = new DataSourceConfigurationEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public List<ExternalDataStoreType> AdaptExternalDataStore(DataTable dt)
        {
            List<ExternalDataStoreType> results = new List<ExternalDataStoreType>();
            foreach (DataRow rw in dt.Rows)
            {
                ExternalDataStoreType matchCode = new ExternalDataStoreType();
                matchCode = AdaptsItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public DataSourceConfigurationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            DataSourceConfigurationEntity result = new DataSourceConfigurationEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }
            if (dt.Columns.Contains("ExternalDataStoreName"))
            {
                result.ExternalDataStoreName = SafeHelper.GetSafestring(rw["ExternalDataStoreName"]);
            }
            if (dt.Columns.Contains("ExternalDataStoreTypeId"))
            {
                result.ExternalDataStoreTypeId = SafeHelper.GetSafeint(rw["ExternalDataStoreTypeId"]);
            }
            if (dt.Columns.Contains("DataStoreConfiguration"))
            {
                result.DataStoreConfiguration = SafeHelper.GetSafestring(rw["DataStoreConfiguration"]);
            }
            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }
            return result;
        }
        public ExternalDataStoreType AdaptsItem(DataRow rw, DataTable dt)
        {
            ExternalDataStoreType result = new ExternalDataStoreType();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }
            if (dt.Columns.Contains("ExternalDataStoreTypeName"))
            {
                result.ExternalDataStoreTypeName = SafeHelper.GetSafestring(rw["ExternalDataStoreTypeName"]);
            }
            return result;
        }
    }
}
