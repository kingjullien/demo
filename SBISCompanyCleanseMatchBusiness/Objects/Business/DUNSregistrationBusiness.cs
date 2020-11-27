using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DUNSregistrationBusiness : BusinessParent
    {
        DUNSregistrationRepository rep;
        public DUNSregistrationBusiness(string connectionString) : base(connectionString) { rep = new DUNSregistrationRepository(Connection); }
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
