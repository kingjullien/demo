using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class UpdateNotificationProfileRequestDetail
    {
        public NotificationProfileDetail NotificationProfileDetail { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class MonUpdateNotificationProfileRequest
    {
        public string xmlnsmon { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public UpdateNotificationProfileRequestDetail UpdateNotificationProfileRequestDetail { get; set; }
    }

    public class NotificationUpdateRequest
    {
        public MonUpdateNotificationProfileRequest MonUpdateNotificationProfileRequest { get; set; }
    }
}