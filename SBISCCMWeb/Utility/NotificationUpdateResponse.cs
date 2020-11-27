using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class FormerNotificationProfileDetail
    {
        public string NotificationProfileName { get; set; }
        public string DeliveryChannelUserPreferenceName { get; set; }
        public string DeliveryMode { get; set; }
        public string DeliveryFrequency { get; set; }
        public string DeliveryFormat { get; set; }
        public string NotificationProfileStatusText { get; set; }
        public bool StopDeliveryIndicator { get; set; }
    }
    public class UpdateNotificationProfileResponseDetail
    {
        public NotificationProfileDetail NotificationProfileDetail { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class UpdateNotificationProfileResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public UpdateNotificationProfileResponseDetail UpdateNotificationProfileResponseDetail { get; set; }
    }

    public class NotificationUpdateResponse
    {
        public UpdateNotificationProfileResponse UpdateNotificationProfileResponse { get; set; }
    }
}