using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class MonitoringProfileCreatedDate
    {
        public string name { get; set; }
    }

    public class MonitoringProfileDetail
    {
        public string MonitoringProfileName { get; set; }
        public string MonitoringProfileDescription { get; set; }
        public int MonitoringProfileID { get; set; }
        public string DNBProductID { get; set; }
        public string MonitoringLevel { get; set; }
        public MonitoringElementDetail MonitoringElementDetail { get; set; }
        public string MonitoringProfileStatusText { get; set; }
        public MonitoringProfileCreatedDate MonitoringProfileCreatedDate { get; set; }
        public bool ReturnChangedDataOnlyIndicator { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
        public int DisplaySequence { get; set; }
        public string ServiceVersionNumber { get; set; }
        public FormerMonitoringProfileDetail FormerMonitoringProfileDetail { get; set; }
    }

    public class ListMonitoringProfileResponseDetail
    {
        public int CandidateMatchedQuantity { get; set; }
        public int CandidateReturnedQuantity { get; set; }
        public List<MonitoringProfileDetail> MonitoringProfileDetail { get; set; }

    }

    public class ListMonitoringProfileResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public ListMonitoringProfileResponseDetail ListMonitoringProfileResponseDetail { get; set; }
    }

    public class ListMonitoring
    {
        public ListMonitoringProfileResponse ListMonitoringProfileResponse { get; set; }
    }
}