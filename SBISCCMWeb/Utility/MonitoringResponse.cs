using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class CreateMonitoringProfileResponseDetail
    {
        public MonitoringProfileDetail MonitoringProfileDetail { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class CreateMonitoringProfileResponse
    {
        public string ServiceVersionNumber { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public CreateMonitoringProfileResponseDetail CreateMonitoringProfileResponseDetail { get; set; }
    }

    public class MonitoringResponse
    {
        public CreateMonitoringProfileResponse CreateMonitoringProfileResponse { get; set; }
    }
}