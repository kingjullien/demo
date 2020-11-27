using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class SystemNotificationFacade : FacadeParent
    {
        SystemNotificationBusiness rep;
        public SystemNotificationFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new SystemNotificationBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new SystemNotificationBusiness(Connection);
            }
        }

        public List<SystemNotificationEntity> GetAllNotification()
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            results = rep.GetAllNotification();
            return results;
        }

        // Gets all Notifications
        public List<SystemNotificationEntity> GetAllNotificationPaging(int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            return rep.GetAllNotificationPaging(SortOrder, PageNumber, PageSize, out TotalCount);
        }
        public List<SystemNotificationEntity> GetActiveNotification(bool IsActive)
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            results = rep.GetActiveNotification(IsActive);
            return results;
        }

        #region Add/Edit Notification
        public SystemNotificationEntity GetNotificationByMessageId(int MessageId)
        {
            SystemNotificationEntity results = new SystemNotificationEntity();
            results = rep.GetNotificationByMessageId(MessageId);
            return results;
        }
        public string InsertUpdateSystemNotification(SystemNotificationEntity objNotification)
        {
            return rep.InsertUpdateSystemNotification(objNotification);
        }
        #endregion
        public string ActiveNotificationMessage(int MessageId, bool IsActive, bool IsActiveUpdate)
        {
            return rep.ActiveNotificationMessage(MessageId, IsActive, IsActiveUpdate);
        }
        public void DismissNotificationByUsers(int UserId, string NotificationId, bool IsDismiss)
        {
            rep.DismissNotificationByUsers(UserId, NotificationId, IsDismiss);
        }
        public void UpdateDismissNotificationByUsers(int UserId, string NotificationId, bool IsDismiss, bool isUpdate)
        {
            rep.UpdateDismissNotificationByUsers(UserId, NotificationId, IsDismiss, isUpdate);
        }
        public DataTable GetDismissNotificationByUser(int userId)
        {
            return rep.GetDismissNotificationByUser(userId);
        }
    }
}
