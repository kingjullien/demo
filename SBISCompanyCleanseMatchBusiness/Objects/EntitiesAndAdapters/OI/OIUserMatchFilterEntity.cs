using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIUserMatchFilterEntity
    {
        public int FilterId { get; set; }
        public int ConfidenceCodeMin { get; set; }
        public int ConfidenceCodeMax { get; set; }
        public string MG_Company { get; set; }
        public string MG_StreetNo { get; set; }
        public string MG_StreetName { get; set; }
        public string MG_City { get; set; }
        public string MG_State { get; set; }
        public string MG_PostalCode { get; set; }
        public string MG_Phone { get; set; }
        public string MG_Webdomain { get; set; }
        public string MG_Country { get; set; }
        public string MG_EIN { get; set; }
        public string MDP_Company { get; set; }
        public string MDP_Webdomain { get; set; }
        public int Score_Company { get; set; }
        public int Score_StreetName { get; set; }
        public bool ActiveOnly { get; set; }
        public bool Exclude { get; set; }
        public bool Enabled { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string GroupName { get; set; }
    }
}
