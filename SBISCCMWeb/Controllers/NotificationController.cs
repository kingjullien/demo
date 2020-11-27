using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification]
    public class NotificationController : BaseController
    {
        // GET: Notification
        public ActionResult Index()
        {
            SystemNotificationFacade NFacade = new SystemNotificationFacade(Helper.GetMasterConnctionstring());
            List<SystemNotificationEntity> lstNotification = NFacade.GetActiveNotification(true);
            SystemNotificationFacade NFacade1 = new SystemNotificationFacade(this.CurrentClient.ApplicationDBConnectionString);
            DataTable dtNotificatin = NFacade1.GetDismissNotificationByUser(Helper.oUser.UserId);
            if (lstNotification != null && lstNotification.Count > 0 && dtNotificatin != null && dtNotificatin.Rows.Count != 0)
            {
                foreach (var itemNotification in lstNotification)
                {
                    foreach (DataRow row in dtNotificatin.Rows)
                    {
                        if (itemNotification.MessageId == Convert.ToInt32(row["NotificationId"]))
                        {
                            itemNotification.isRead = true;
                            if (Convert.ToBoolean(row["IsDismiss"]))
                            {
                                itemNotification.isDismiss = true;
                            }
                        }
                    }
                }
            }
            return View(lstNotification);
        }
        [HttpPost]
        public JsonResult DismissNotificationById(string MessageId)
        {
            SystemNotificationFacade NFacade = new SystemNotificationFacade(this.CurrentClient.ApplicationDBConnectionString);
            bool IsDismiss = true;
            bool IsUpdate = false;
            NFacade.UpdateDismissNotificationByUsers(Helper.oUser.UserId, MessageId, IsDismiss, IsUpdate);
            return Json(CommonMessagesLang.msgSuccess);
        }

        public JsonResult DismissNotification(string NotificationId)
        {
            SystemNotificationFacade fac = new SystemNotificationFacade(this.CurrentClient.ApplicationDBConnectionString);
            bool IsDismiss = false;
            fac.DismissNotificationByUsers(Helper.oUser.UserId, NotificationId, IsDismiss);
            return Json(CommonMessagesLang.msgSuccess);
        }
    }
}