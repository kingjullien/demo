using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{

    public class MonitoringChange
    {
        public string ChangeCondition { get; set; }
    }


    public class MonitoringElement
    {
        public string PCMElementXPATHText { get; set; }
        public List<MonitoringChanges> MonitoringChanges { get; set; }
        public bool ContinuousChangeNotificationIndicator { get; set; }
    }


    public class FormerMonitoringProfileDetail
    {
        public string MonitoringProfileName { get; set; }
        public string ServiceVersionNumber { get; set; }
        public MonitoringElementDetail MonitoringElementDetail { get; set; }
        public string MonitoringProfileStatusText { get; set; }
        public bool ReturnChangedDataOnlyIndicator { get; set; }
        public InquiryReferenceText InquiryReferenceText { get; set; }
    }



    public class UpdateMonitoringProfileResponseDetail
    {
        public MonitoringProfileDetail MonitoringProfileDetail { get; set; }
    }

    public class UpdateMonitoringProfileResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public UpdateMonitoringProfileResponseDetail UpdateMonitoringProfileResponseDetail { get; set; }
    }

    public class UpdateMonitoringResponse
    {
        public UpdateMonitoringProfileResponse UpdateMonitoringProfileResponse { get; set; }
    }
}