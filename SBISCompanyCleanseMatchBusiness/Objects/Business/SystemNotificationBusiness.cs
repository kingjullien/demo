using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{

    public class SystemNotificationBusiness : BusinessParent
    {
        SystemNotificationRepository rep;

        public SystemNotificationBusiness(string connectionString) : base(connectionString) { rep = new SystemNotificationRepository(Connection); }

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
