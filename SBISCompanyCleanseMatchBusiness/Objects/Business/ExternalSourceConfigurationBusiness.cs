using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ExternalSourceConfigurationBusiness : BusinessParent
    {
        ExternalSourceConfigurationRepository rep;
        public ExternalSourceConfigurationBusiness(string connectionString) : base(connectionString) { rep = new ExternalSourceConfigurationRepository(Connection); }
        public List<DataSourceConfigurationEntity> GetExternalDataStore(int? Id)
        {
            return rep.GetExternalDataStore(Id);
        }
        public int InsertExternalDataStore(DataSourceConfigurationEntity obj)
        {
            return rep.InsertExternalDataStore(obj);
        }
        public List<ExternalDataStoreType> GetExternalDataSourceType(int? Id)
        {
            return rep.GetExternalDataSourceType(Id);
        }
        public void DeleteExternalDataStore(int externalDataStoreId, int userId)
        {
            rep.DeleteExternalDataStore(externalDataStoreId, userId);
        }
    }
}
