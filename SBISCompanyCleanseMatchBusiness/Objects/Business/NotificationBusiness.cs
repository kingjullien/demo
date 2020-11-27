using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class NotificationBusiness : BusinessParent
    {
        NotificationRepository rep;

        public NotificationBusiness(string connectionString) : base(connectionString) { rep = new NotificationRepository(Connection); }


        public int InsertNotificationProfile(NotificationProfileEntity obj)
        {
            return rep.InsertNotificationProfile(obj);
        }

        public List<NotificationProfileEntity> GetNotificationProfile(int CredentialId)
        {
            return rep.GetNotificationProfile(CredentialId);
        }
        public void DeleteNotificationProfile(int id)
        {
            rep.DeleteNotificationProfile(id);
        }
        public NotificationProfileEntity GetNotificationProfileById(int id)
        {
            return rep.GetNotificationProfileById(id);
        }
        public List<NotificationProfileEntity> GetAllNotificationProfile()
        {
            return rep.GetAllNotificationProfile();
        }
        public bool CheckNotificationName(int ProfileID, string NotificationProfileName)
        {
            return rep.CheckNotificationName(ProfileID, NotificationProfileName);
        }
    }
}
