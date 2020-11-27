using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class MonitoringProfileUpdateSpecification
    {
        public string MonitoringProfileName { get; set; }
        public string ServiceVersionNumber { get; set; }
        public MonitoringElementDetail MonitoringElementDetail { get; set; }
        public string MonitoringProfileStatusText { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class UpdateMonitoringProfileRequestDetail
    {
        public MonitoringProfileUpdateSpecification MonitoringProfileUpdateSpecification { get; set; }
    }

    public class MonUpdateMonitoringProfileRequest
    {
        public string xmljson { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public UpdateMonitoringProfileRequestDetail UpdateMonitoringProfileRequestDetail { get; set; }
    }

    public class UpdateMonitoringRequest
    {
        public MonUpdateMonitoringProfileRequest MonUpdateMonitoringProfileRequest { get; set; }
    }
}