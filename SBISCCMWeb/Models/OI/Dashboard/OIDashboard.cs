using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class OIDashboard
    {
        public string ImportProcess { get; set; }
        public string AllCount { get; set; }
        public string InputRecordCount { get; set; }

        public string MatchRecordCount { get; set; }
        public string UnMatchRecordCount { get; set; }
        public string MatchedOutputQueueCount { get; set; }
        public string ArchivalQueueCount { get; set; }
        public string FirmographicsExportQueueCount { get; set; }
        public string CorporateTreeExportQueueCount { get; set; }
        public string FormatInputRecordCount { get; set; }
        public string FormatAllCount { get; set; }
        public string FormatMatchRecordCount { get; set; }
        public string FormatUnMatchRecordCount { get; set; }
        public string FormatMatchedOutputQueueCount { get; set; }
        public string FormatArchivalQueueCount { get; set; }
        public string FormatFirmographicsExportQueueCount { get; set; }
        public string FormatCorporateTreeExportQueueCount { get; set; }

        public string MonthCount { get; set; }
        public string YTDCount { get; set; }
        public string AllAPIUsageCount { get; set; }
        public string FormatMonthCount { get; set; }
        public string FormatYTDCount { get; set; }
        public string FormatAllAPIUsageCount { get; set; }
        public string HourlyAPIUsageCount { get; set; }
        public string FormatHourlyAPIUsageCount { get; set; }


        public string MatchMonth { get; set; }
        public string EnrichMonth { get; set; }
        public string MatchYear { get; set; }
        public string EnrichYear { get; set; }
        public string MatchAll { get; set; }
        public string EnrichAll { get; set; }


    }
}