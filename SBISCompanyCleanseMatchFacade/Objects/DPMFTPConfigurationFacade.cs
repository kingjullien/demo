using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DPMFTPConfigurationFacade : FacadeParent
    {
        DPMFTPConfigurationBusiness rep;

        public DPMFTPConfigurationFacade(string connectionString) : base(connectionString) { rep = new DPMFTPConfigurationBusiness(Connection); }

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
