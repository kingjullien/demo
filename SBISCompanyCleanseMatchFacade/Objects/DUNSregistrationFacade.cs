using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DUNSregistrationFacade : FacadeParent
    {
        DUNSregistrationBusiness rep;
        public DUNSregistrationFacade(string connectionString) : base(connectionString) { rep = new DUNSregistrationBusiness(Connection); }
        public int InsertDUNSregistration(DUNSregistrationEntity obj)
        {
            int result = rep.InsertDUNSregistration(obj);
            return result;
        }

        public List<DUNSregistrationEntity> GetAllDUNSregistration()
        {
            return rep.GetAllDUNSregistration();
        }

        public List<DUNSregistrationEntity> GetDUNSregistration(int CredentialId)
        {
            return rep.GetDUNSregistration(CredentialId);
        }

        public void DeleteDUNSregistration(int id)
        {
            rep.DeleteDUNSregistration(id);
        }

        public DUNSregistrationEntity GetDUNSregistrationById(int id)
        {
            return rep.GetDUNSregistrationById(id);
        }

        public List<MonitoringProfileEntity> GetAllMonitoringProfileNames()
        {
            return rep.GetAllMonitoringProfileNames();
        }

        public List<NotificationProfileEntity> GetAllNotificationProfileNames()
        {
            return rep.GetAllNotificationProfileNames();
        }

        public bool CheckMonitoringProfileUsed(int ProfileId)
        {
            return rep.CheckMonitoringProfileUsed(ProfileId);
        }

        public bool CheckNotificationProfileUsed(int ProfileId)
        {
            return rep.CheckNotificationProfileUsed(ProfileId);
        }
    }
}
