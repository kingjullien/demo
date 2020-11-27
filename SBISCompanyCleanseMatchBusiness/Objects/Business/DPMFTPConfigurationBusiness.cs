using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DPMFTPConfigurationBusiness : BusinessParent
    {
        DPMFTPConfigurationRepository rep;
        public DPMFTPConfigurationBusiness(string connectionString) : base(connectionString) { rep = new DPMFTPConfigurationRepository(Connection); }
        public int DPMInsertUpdateFTPConfiguration(DPMFTPConfigurationEntity obj)
        {
            return rep.DPMInsertUpdateFTPConfiguration(obj);
        }
        public List<DPMFTPConfigurationEntity> GetDPMFTPConfiguration()
        {
            return rep.GetDPMFTPConfiguration();
        }
        public void DeleteDPMFTPConfiguration(int id)
        {
            rep.DeleteDPMFTPConfiguration(id);
        }
        public DPMFTPConfigurationEntity GetDPMFTPConfigurationById(int Id)
        {
            return rep.GetDPMFTPConfigurationById(Id);
        }
        public void DPMProcessNotifications()
        {
            rep.DPMProcessNotifications();
        }
    }
}
