using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIAutoAcceptanceEntity
    {
        public int RuleId { get; set; }
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
        public string Tags { get; set; }
        public int CountryGroupId { get; set; }
        public bool AcceptActiveOnly { get; set; }
        public bool PreferLinkedRecord { get; set; }
        public bool ExcludeFromAutoAccept { get; set; }
        public int UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string GroupName { get; set; }
    }
}
