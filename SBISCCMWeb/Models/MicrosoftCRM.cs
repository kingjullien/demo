using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class MicrosoftCRM
    {
    }

    public class Value
    {
        public string name { get; set; }
        public string accountnumber { get; set; }
        public string address1_city { get; set; }
        public string address1_country { get; set; }
        public object address1_name { get; set; }
        public string address1_line1 { get; set; }
        public string address1_stateorprovince { get; set; }
        public object address1_line2 { get; set; }
        public object address2_line1 { get; set; }
        public string emailaddress1 { get; set; }
        public object address1_telephone2 { get; set; }
        public string accountid { get; set; }
        public DateTime modifiedon { get; set; }
        public string address1_composite { get; set; }
        public string address1_postalcode { get; set; }
        public object address1_telephone1 { get; set; }
    }
    public class DynamicCRMCustomView
    {
        public string context { get; set; }
        public List<Value> value { get; set; }
    }
    public class Oauth2Token
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string id_token { get; set; }
    }
}