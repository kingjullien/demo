using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class CreateNotificationProfileResponseDetail
    {
        public NotificationProfileDetail NotificationProfileDetail { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class CreateNotificationProfileResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public CreateNotificationProfileResponseDetail CreateNotificationProfileResponseDetail { get; set; }
    }

    public class NotificationResponse
    {
        public CreateNotificationProfileResponse CreateNotificationProfileResponse { get; set; }
    }
}