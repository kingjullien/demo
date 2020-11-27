using SBISCCMWeb.Utility.BuildList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BuildListSearchModel
    {
        public SearchCriteriaRequest Request { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public long SearchResultsId { get; set; }
        public int NoOfRecored { get; set; }
        public string RequestedDateTime { get; set; }

    }
}