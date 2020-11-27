using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class ExportDataModel
    {
        public string ActualMatchOutputCount { get; set; }
        public string ActualDataEnrichmentCount { get; set; }
        public string ActualMonitoringOutputcount { get; set; }
        public string ActualActiveQueueOutputcount { get; set; }
        public string MatchOutputCount { get; set; }
        public string DataEnrichmentCount { get; set; }
        public string MonitoringOutputcount { get; set; }
        public string ActiveQueueOutputcount { get; set; }
        public List<DataEnrichmentApiCount> lstDataEnrichmentApiCount { get; set; }
    }
    public class DataEnrichmentApiCount
    {
        public string APIName { get; set; }
        public string NbrRecords { get; set; }
    }
    public class MonitoringApiCount
    {
        public string APIName { get; set; }
        public string NbrRecords { get; set; }
    }

}