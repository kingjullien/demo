using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_ConsolidatedEmployees
    {
        public string DnBDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string TotalEmployeeQuantity { get; set; }
        public string Reliability { get; set; }
        public string ReliabilityCode { get; set; }
        public string EmployeeFiguresDate { get; set; }
    }
}