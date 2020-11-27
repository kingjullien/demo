using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_IndustryCodes
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string DisplaySequence { get; set; }
        public string IndustryCode { get; set; }
        public string IndustryCodeType { get; set; }
        public string LanguageCode { get; set; }
        public string DescriptionLengthCode { get; set; }
        public string IndustryCodeDescription { get; set; }
        public string IndustryCodeTypeCode { get; set; }
        public string SalesPercentage { get; set; }
    }
}