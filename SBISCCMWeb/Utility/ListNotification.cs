using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class NotificationProfileCreatedDate
    {
        public string _param { get; set; }
    }
    public class NotificationProfileDetail
    {
        public int? NotificationProfileID { get; set; }
        public string NotificationProfileName { get; set; }
        public string DeliveryChannelUserPreferenceName { get; set; }
        public string NotificationProfileDescription { get; set; }
        public string DeliveryMode { get; set; }
        public string DeliveryFrequency { get; set; }
        public string DeliveryFormat { get; set; }
        public string NotificationProfileStatusText { get; set; }
        public bool? StopDeliveryIndicator { get; set; }
        public NotificationProfileCreatedDate NotificationProfileCreatedDate { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
        public int? DisplaySequence { get; set; }
        public FormerNotificationProfileDetail FormerNotificationProfileDetail { get; set; }
    }

    public class ListNotificationProfileResponseDetail
    {
        public int CandidateMatchedQuantity { get; set; }
        public int CandidateReturnedQuantity { get; set; }
        public List<NotificationProfileDetail> NotificationProfileDetail { get; set; }
    }

    public class ListNotificationProfileResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public ListNotificationProfileResponseDetail ListNotificationProfileResponseDetail { get; set; }
    }

    public class ListNotification
    {
        public ListNotificationProfileResponse ListNotificationProfileResponse { get; set; }
    }
}