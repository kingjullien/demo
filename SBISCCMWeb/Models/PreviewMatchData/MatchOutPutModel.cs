using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData
{
    [Serializable]
    public class MatchOutPutModel
    {
        public string InputdId { get; set; }
        public string SrcRecordId { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public string DUNSNumber { get; set; }
        public string CEOName { get; set; }
        public string WebSite { get; set; }
        public string AltCompanyName { get; set; }
        public string AltAddress { get; set; }
        public string AltCity { get; set; }
        public string AltState { get; set; }
        public string AltPostalCode { get; set; }
        public string AltCountry { get; set; }
        public string Email { get; set; }
        public string RegistrationNbr { get; set; }
        public string RegistrationType { get; set; }
        public string Tag { get; set; }
        public string Source { get; set; }
        public string UpdatedCompanyName { get; set; }
        public string UpdatedAddress { get; set; }
        public string UpdatedCity { get; set; }
        public string UpdatedState { get; set; }
        public string UpdatedPostalCode { get; set; }
        public string UpdatedCountryISOAlpha2Code { get; set; }
        public string UpdatedPhoneNbr { get; set; }
        public string StandardizedOrganizationName { get; set; }
        public string StandardizedAddressLine { get; set; }
        public string StandardizedPrimaryTownName { get; set; }
        public string StandardizedCountyName { get; set; }
        public string StandardizedTerritoryAbbreviatedName { get; set; }
        public string StandardizedTerritoryName { get; set; }
        public string StandardizedPostalCode { get; set; }
        public string StandardizedPostalCodeExtensionCode { get; set; }
        public string StandardizedCountryISOAlpha2Code { get; set; }
        public string StandardizedCountryName { get; set; }
        public string StandardizedDeliveryPointValidationStatusValue { get; set; }
        public string StandardizedDeliveryPointValidationCMRAValue { get; set; }
        public string StandardizedAddressTypeValue { get; set; }
        public string StandardizedInexactAddressIndicator { get; set; }
        public string ServiceTransactionID { get; set; }
        public string TransactionTimestamp { get; set; }
        public string CandidateMatchedQty { get; set; }
        public string MatchDataCriteriaText { get; set; }
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
        public string DnBAddressUndeliverable { get; set; }
        public string DnBMailingStreetAddressLine { get; set; }
        public string DnBMailingPrimaryTownName { get; set; }
        public string DnBMailingCountryISOAlpha2Code { get; set; }
        public string DnBMailingPostalCode { get; set; }
        public string DnBMailingPostalCodeExtensionCode { get; set; }
        public string DnBMailingTerritoryAbbreviatedName { get; set; }
        public string DnBMailingAddressUndeliverable { get; set; }
        public string DnBTelephoneNumber { get; set; }
        public string DnBTelephoneNumberUnreachableIndicator { get; set; }
        public string DnBOperatingStatus { get; set; }
        public string DnBFamilyTreeMemberRole { get; set; }
        public string DnBStandaloneOrganization { get; set; }
        public string DnBConfidenceCode { get; set; }
        public string DnBMatchGradeText { get; set; }
        public string DnBMatchGradeComponentCount { get; set; }
        public string DnBMatchDataProfileText { get; set; }
        public string DnBMatchDataProfileComponentCount { get; set; }
        public string DnBDisplaySequence { get; set; }
        public string DnBMarketabilityIndicator { get; set; }
        public string MGCompany { get; set; }
        public string MGStreetNo { get; set; }
        public string MGStreetName { get; set; }
        public string MGCity { get; set; }
        public string MGState { get; set; }
        public string MGPOBox { get; set; }
        public string MGPhone { get; set; }
        public string MGPostalCode { get; set; }
        public string MGDensity { get; set; }
        public string MGUniqueness { get; set; }
        public string MGSIC { get; set; }
        public string MDPCompany { get; set; }
        public string MDPStreetNo { get; set; }
        public string MDPStreetName { get; set; }
        public string MDPCity { get; set; }
        public string MDPState { get; set; }
        public string MDPPOBox { get; set; }
        public string MDPPhone { get; set; }
        public string MDPPostalCode { get; set; }
        public string MDPDUNS { get; set; }
        public string MDPSIC { get; set; }
        public string MDPDensity { get; set; }
        public string MDPUniqueness { get; set; }
        public string MDPNationalID { get; set; }
        public string MDPURL { get; set; }
        public string AcceptedBy { get; set; }
        public int TotalRecordCount { get; set; }
    }
}