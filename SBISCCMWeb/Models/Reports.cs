using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class Reports
    {

    }
    public class DataQueueChart
    {
        public string x;
        public decimal y;
    }
    public class DataStewrdStatisticsChart
    {
        public string x;
        public decimal y;
        public string userGroup;
    }
    public class APIUsagesCurntMonthCntChart
    {
        public string x;
        public decimal y;
    }

    public class APIUsagesCurntYearCntChart
    {
        public string name;
        public decimal y;
    }

    public class LineTotalCntChart
    {
        public string Month;
        public decimal year;
        public string TotalCalls;

    }

}