using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class iResearchEntity
    {
        public int ResearchRequestId { get; set; }
        public string ResearchRequestType { get; set; }
        [Required(ErrorMessage = "Business Name is required")]
        public string BusinessName { get; set; }
        [Required(ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }
        [Required(ErrorMessage = "Address Locality is required")]
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        [Required(ErrorMessage = "Postal Code is required")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "Country Code is required")]
        public string CountryISOAlpha2Code { get; set; }
        [Required(ErrorMessage = "Research Comment is required")]
        public string CustomerRequestorEmail { get; set; }
        public string CustomerTransactionReference { get; set; }

        [Required(ErrorMessage = "Research Comment is required")]
        public string ResearchComments { get; set; }
        public string UserId { get; set; }
        public string InputId { get; set; }
        public string SrcRecordId { get; set; }
        public string Tags { get; set; }
        public string RequestDateTime { get; set; }
        public string RequestBody { get; set; }
        public string RequestResponseJSON { get; set; }
        public int typeDnBCode { get; set; }
        public int roleType { get; set; }
        public bool HasError { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
    public class iResearchEntityTargetedEntity
    {
        public int ResearchRequestId { get; set; }
        public string ResearchRequestType { get; set; }
        public string CustomerRequestorEmail { get; set; }
        public string CustomerTransactionReference { get; set; }
        public string ResearchComments1 { get; set; }
        public string ResearchComments2 { get; set; }
        public string UserId { get; set; }
        public string InputId { get; set; }
        public string SrcRecordId { get; set; }
        public string Tags { get; set; }
        public string RequestDateTime { get; set; }
        public string RequestBody { get; set; }
        public string RequestResponseJSON { get; set; }
        public int typeDnBCode { get; set; }
        public int roleType { get; set; }
        public string ResearchSubTypes { get; set; }
        public string Duns { get; set; }
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string BusinessName { get; set; }
        public string StreetAddress { get; set; }
        public string AddressLocality { get; set; }
        public string AddressRegion { get; set; }
        public string AddressCity { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string DuplicateDuns { get; set; }
        public string TradeStyle { get; set; }
        public bool Status { get; set; }
    }

    public class iResearchChallengeEntity
    {
        public string customerTransactionID { get; set; }
        public string customerReference { get; set; }
        public int challengeReason { get; set; }
        public string comment { get; set; }
        public int typeDnBCode { get; set; }
        public int caseID { get; set; }
        public int researchRequestID { get; set; }
    }
}
