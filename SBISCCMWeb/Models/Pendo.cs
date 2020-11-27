using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class Pendo
    {
        public string type { get; set; }
        public string @event { get; set; }
        public string visitorId { get; set; }
        public string accountId { get; set; }
        public long timestamp { get; set; }
        public Properties properties { get; set; }
        public Context context { get; set; }

    }
    public class Properties
    {
        public string plan { get; set; }
        public string accountType { get; set; }
    }
    public class Context
    {
        public string ip { get; set; }
        public string userAgent { get; set; }
        public string url { get; set; }
        public string title { get; set; }
    }

}