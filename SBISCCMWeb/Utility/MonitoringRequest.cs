using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class MonitoringChanges
    {
        public string ChangeCondition { get; set; }
        public string ChangeValue { get; set; }
    }
    public class MonitoringElementDetail
    {
        public List<MonitoringElement> MonitoringElement { get; set; }
    }

    public class MonitoringProfileSpecification
    {
        public string MonitoringProfileName { get; set; }
        public string MonitoringProfileDescription { get; set; }
        public string DNBProductID { get; set; }
        public string MonitoringLevel { get; set; }
        public MonitoringElementDetail MonitoringElementDetail { get; set; }
    }

    public class InquiryReferenceText
    {
        public List<string> CustomerReferenceText { get; set; }
    }

    public class CreateMonitoringProfileRequestDetail
    {
        public MonitoringProfileSpecification MonitoringProfileSpecification { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }

    public class MonCreateMonitoringProfileRequest
    {
        public string xmljson { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public CreateMonitoringProfileRequestDetail CreateMonitoringProfileRequestDetail { get; set; }

    }
    public class MonitoringRequest
    {
        public MonCreateMonitoringProfileRequest CreateMonitoringProfileRequestMain { get; set; }
    }
}