using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ExternalSourceConfigurationFacade : FacadeParent
    {
        ExternalSourceConfigurationBusiness rep;
        public ExternalSourceConfigurationFacade(string connectionString) : base(connectionString) { rep = new ExternalSourceConfigurationBusiness(Connection); }
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
