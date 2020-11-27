using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class NotificationProfileSpecification
    {
        public string NotificationProfileName { get; set; }
        public string NotificationProfileDescription { get; set; }
        public string DeliveryMode { get; set; }
        public string DeliveryChannelUserPreferenceName { get; set; }
        public string DeliveryFrequency { get; set; }
        public string DeliveryFormat { get; set; }
        public string StopDeliveryIndicator { get; set; }
    }
    public class CreateNotificationProfileRequestDetail
    {
        public NotificationProfileSpecification NotificationProfileSpecification { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class MonCreateNotificationProfileRequest
    {
        public string xmlnsmon { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public CreateNotificationProfileRequestDetail CreateNotificationProfileRequestDetail { get; set; }
    }

    public class NotificationRequest
    {
        public MonCreateNotificationProfileRequest MonCreateNotificationProfileRequest { get; set; }
    }
}