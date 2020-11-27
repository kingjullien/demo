using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.OI.CleanseMatch
{
    public class OICleanseMatchOutput
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
        public string std_streetnum { get; set; }
        public string std_streetname { get; set; }

        public string match_grade { get; set; }
    }
    public class OICleanseMatchViewModel
    {
        public List<OICleanseMatchOutput> oICleanseMatchOutputs { get; set; }
        public string Error { get; set; }
        public string ResponseJson { get; set; }
        public string MatchUrl { get; set; }

    }
}