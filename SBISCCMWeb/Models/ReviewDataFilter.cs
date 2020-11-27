using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class ReviewDataFilter
    {
        public bool TopMatchCandidate { get; set; }
        public int pagevalue { get; set; }
        public string CountryGroup { get; set; }
        public string Tags { get; set; }
        public string ConfidenceCode { get; set; }
        public string OrderBy { get; set; }
        public bool Export { get; set; }

    }
}