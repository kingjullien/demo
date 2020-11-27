using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_NonMarketableReasons
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string NonMarketableReason { get; set; }
        public string NonMarketableReasonCode { get; set; }
    }
}