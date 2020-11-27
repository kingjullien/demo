using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class MatchbookAPILogs
    {
        public string Subdomain { get; set; }
        public string CallType { get; set; }
        public string CallURL { get; set; }
        public int CandidateCount { get; set; }
        public int ResultCode { get; set; }
        public string ErrorDescription { get; set; }
        public double TotalDuration { get; set; }
        public double ORBDuration { get; set; }
        public string Environment { get; set; }
        public DateTime TimePeriod { get; set; }
    }
}