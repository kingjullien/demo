using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class dnb_Authentication
    {
        public string access_token { get; set; }
        public int expiresIn { get; set; }
        public string errorMessage { get; set; }
        public int errorCode { get; set; }
    }
}