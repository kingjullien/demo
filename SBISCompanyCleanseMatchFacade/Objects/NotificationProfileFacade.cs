using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class NotificationProfileFacade : FacadeParent
    {
        NotificationBusiness rep;
        public NotificationProfileFacade(string connectionString) : base(connectionString) { rep = new NotificationBusiness(Connection); }
        // Inserts Notifications
        public int InsertNotificationProfile(NotificationProfileEntity obj)
        {
            return rep.InsertNotificationProfile(obj);
        }

        public List<NotificationProfileEntity> GetNotificationProfile(int CredentialId)
        {
            return rep.GetNotificationProfile(CredentialId);
        }
        // Deletes Notifications
        public void DeleteNotificationProfile(int id)
        {
            rep.DeleteNotificationProfile(id);
        }
        public NotificationProfileEntity GetNotificationProfileById(int id)
        {
            return rep.GetNotificationProfileById(id);
        }
        // Get all Notifications
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
