using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class PreviewAutoAcceptanceRuleEntity
    {
        public int RowId { get; set; }
        public int InputId { get; set; }
        public string SrcRecordId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public DateTime TransactionTimestamp { get; set; }
        public string DnBDUNSNumber { get; set; }
        public string DnBOrganizationName { get; set; }
        public string DnBTradeStyleName { get; set; }
        public string DnBSeniorPrincipalName { get; set; }
        public string DnBStreetAddressLine { get; set; }
        public string DnBPrimaryTownName { get; set; }
        public string DnBCountryISOAlpha2Code { get; set; }
        public string DnBPostalCode { get; set; }
        public string DnBPostalCodeExtensionCode { get; set; }
        public string DnBTerritoryAbbreviatedName { get; set; }
        public bool DnBAddressUndeliverable { get; set; }
        public string DnBTelephoneNumber { get; set; }
        public string DnBOperatingStatus { get; set; }
        public string DnBFamilyTreeMemberRole { get; set; }
        public bool DnBStandaloneOrganization { get; set; }
        public int DnBConfidenceCode { get; set; }
        public string DnBMatchGradeText { get; set; }
        public string DnBMatchDataProfileText { get; set; }
        public int DnBMatchDataProfileComponentCount { get; set; }
        public int DnBDisplaySequence { get; set; }
        public string ExcludedCandidate { get; set; }
    }
}
