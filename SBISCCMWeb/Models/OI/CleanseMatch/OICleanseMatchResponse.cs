using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.OI.CleanseMatch
{

    public class Input
    {
        public string CustomerSubDomain { get; set; }
        public string Country { get; set; }
        public string SrcRecordId { get; set; }
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public object Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public object PhoneNbr { get; set; }
        public object Tags { get; set; }
        public object EIN { get; set; }
        public object OrbNum { get; set; }
        public object CEOName { get; set; }
        public object Website { get; set; }
        public object AltCompanyName { get; set; }
        public object AltAddress1 { get; set; }
        public object AltAddress2 { get; set; }
        public object AltCity { get; set; }
        public object AltState { get; set; }
        public object AltPostalCode { get; set; }
        public object AltCountry { get; set; }
        public object Email { get; set; }
        public object InputId { get; set; }
        public string StdStreetNum { get; set; }
        public string Predirection { get; set; }
        public string Street { get; set; }
        public string Suffix { get; set; }
        public object Postdirection { get; set; }
        public object StdCity { get; set; }
        public object StdState { get; set; }
        public object StdZip { get; set; }
        public string StdStreetName { get; set; }
    }

    public class MatchMask
    {
        public string ein { get; set; }
        public string main_name { get; set; }
        public string corp_elem { get; set; }
        public string other_names { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string webdomain { get; set; }
        public string phone { get; set; }
    }

    public class MatchedField
    {
        public string name { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string other_name { get; set; }
    }

    public class MatchQualifier
    {
        public string mg_name { get; set; }
        public string score_name { get; set; }
        public string mdp_name { get; set; }
        public string mg_streetnum { get; set; }
        public string mg_streetname { get; set; }
        public string score_streetname { get; set; }
        public string mg_city { get; set; }
        public string mg_state { get; set; }
        public string mg_zip { get; set; }
        public string mg_phone { get; set; }
        public string mg_webdomain { get; set; }
        public string mdp_webdomain { get; set; }
        public string mg_country { get; set; }
        public string mg_ein { get; set; }
    }

    public class Result
    {
        public int result_number { get; set; }
        public string orb_num { get; set; }
        public List<int> orb_nums { get; set; }
        public string name { get; set; }
        public string entity_type { get; set; }
        public string company_status { get; set; }
        public int? parent_orb_num { get; set; }
        public string parent_name { get; set; }
        public int? ultimate_parent_orb_num { get; set; }
        public string ultimate_parent_name { get; set; }
        public int subsidiaries_count { get; set; }
        public int branches_count { get; set; }
        public bool is_standalone_company { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string iso_country_code { get; set; }
        public string fetch_url { get; set; }
        public string confidence_score { get; set; }
        public MatchMask match_mask { get; set; }
        public MatchedField matched_field { get; set; }
        public MatchQualifier match_qualifier { get; set; }
        public string std_streetnum { get; set; }
        public string std_streetname { get; set; }
    }
    /// <summary>
    /// This object is used for parsing json for OI CleanseMatch
    /// </summary>
    public class OICleanseMatchResponse
    {
        public Input input { get; set; }
        public int results_count { get; set; }
        public List<Result> results { get; set; }
        public DateTime transaction_timestamp { get; set; }
        public string ErrorMessage { get; set; }
    }
}