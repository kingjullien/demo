using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.BeneficialOwnership
{
    public class ScreeningRequest
    {
        public string __type { get; set; }
        public string sguid { get; set; }
        public string stransid { get; set; }
        public string ssecno { get; set; }
        public string spassword { get; set; }
        public string smodes { get; set; }
        public string srpsgroupbypass { get; set; }
        public List<Search> searches { get; set; }
    }

    public class Search
    {
        public string __type { get; set; }
        public string soptionalid { get; set; }
        public string sname { get; set; }
        public string scompany { get; set; }
        public string saddress1 { get; set; }
        public string saddress2 { get; set; }
        public string saddress3 { get; set; }
        public string scity { get; set; }
        public string sstate { get; set; }
        public string szip { get; set; }
        public string scountry { get; set; }
        public string selective1 { get; set; }
        public string selective2 { get; set; }
        public string selective3 { get; set; }
        public string selective4 { get; set; }
        public string selective5 { get; set; }
        public string selective6 { get; set; }
        public string selective7 { get; set; }
        public string selective8 { get; set; }
    }

    public class ScreeningResponse
    {
        public int userId { get; set; }
        public int credentialId { get; set; }
        public string requestUrl { get; set; }
        public string searchJSON { get; set; }
        public string resultsJSON { get; set; }
        public string Errormsg { get; set; }
    }
}