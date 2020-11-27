using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BingSearchModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Snippet { get; set; }
        public string DisplayUrl { get; set; }
    }
    public class webSearch
    {
        public BingSearchModel WSer { get; set; }
    }
}