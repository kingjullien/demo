using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class Dashboard
    {
        public string ApiCount { get; set; }
        public string ActualApiCount { get; set; }
        public string YTD { get; set; }
        public string ActualYTD { get; set; }
        public string AllCount { get; set; }
        public string ActualAllCount { get; set; }

        public string ActiveQueueStatus { get; set; }
        public string ActualMatchRecordCount { get; set; }
        public string MatchRecordCount { get; set; }
        public string LCMCount { get; set; }
        public string ActualLCMCount { get; set; }
        public string ActualBadInputRecordCount { get; set; }
        public string BadInputRecordCount { get; set; }
        public string ProcessingQueueCnt { get; set; }
        public string EnrichmentQueueCount { get; set; }
        public string ExportRecordCount { get; set; }


        public string MatchExportRecordCount { get; set; }
        public string ActualMatchExportRecordCount { get; set; }
        public string EnrichmentExportRecordCount { get; set; }
        public string ActualEnrichmentExportRecordCount { get; set; }

        public string ActualFilesAwaitingImportCount { get; set; }
        public string FilesAwaitingImportCount { get; set; }
        public string InputRecordCount_Total { get; set; }
        public string ActualInputRecordCount_Total { get; set; }

    }
}