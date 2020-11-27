using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.BeneficialOwnership
{
    public class BeneficialOwnership_Main
    {
        public CMPBOSV1_Base Base { get; set; }
        public List<CMPBOSV1_BeneficialOwnerRelationships> lstBeneficialOwnerRelationships { get; set; }
        public List<CMPBOSV1_BeneficialOwners> lstBeneficialOwners { get; set; }
        public List<CMPBOSV1_BeneficialOwnershipCountryWiseSummary> lstBeneficialOwnershipCountryWiseSummary { get; set; }
        public List<CMPBOSV1_CombinedData> lstCombinedData { get; set; }
        public GraphJson graphJson { get; set; }
        public List<CMPBOSV1_BeneficialOwnershipCountryWisePSCSummary> lstBeneficialOwnershipCountryWisePSCSummary { get; set; }
        public List<CMPBOSV1_BeneficialOwnershipNationalityWisePSCSummary> lstBeneficialOwnershipNationalityWisePSCSummary { get; set; }
        public List<CMPBOSV1_BeneficialOwnershipTypeWisePSCSummary> lstBeneficialOwnershipTypeWisePSCSummary { get; set; }
    }

    public class CMPBOSV1_Base
    {
        public string DnBDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string transactionID { get; set; }
        public string TransactionTimestamp { get; set; }
        public string inLanguage { get; set; }
        public string productID { get; set; }
        public string productVersion { get; set; }
        public string inquiryDetailDuns { get; set; }
        public string inquiryDetailProductID { get; set; }
        public string inquiryDetailProductVersion { get; set; }
        public string inquiryDetailTradeUp { get; set; }
        public string inquiryDetailCustomerReference { get; set; }
        public string inquiryOwnershipType { get; set; }
        public decimal inquiryOwnershipPercentage { get; set; }
        public int inquiryDegreeOfSeparation { get; set; }
        public string duns { get; set; }
        public string organizationName { get; set; }
        public string businessEntityType { get; set; }
        public int businessEntityTypeCode { get; set; }
        public string controlOwnershipType { get; set; }
        public int controlOwnershipTypeCode { get; set; }
        public byte isOutOfBusiness { get; set; }
        public string addressCountry { get; set; }
        public string addressCountryIsoAlpha2Code { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressCounty { get; set; }
        public string addressPostalCode { get; set; }
        public string addressStreetLine1 { get; set; }
        public string addressStreetLine2 { get; set; }
        public string addressStreetLine3 { get; set; }
        public string controlOwnershipConfidenceLevel { get; set; }
        public int controlOwnershipConfidenceLevelCode { get; set; }
        public string ownershipUnavailableReason0 { get; set; }
        public int ownershipUnavailableReason0Code { get; set; }
        public string ownershipUnavailableReason1 { get; set; }
        public int ownershipUnavailableReason1Code { get; set; }
        public string ownershipUnavailableReason2 { get; set; }
        public int ownershipUnavailableReason2Code { get; set; }
        public string pscUnavailableReason0 { get; set; }
        public int pscUnavailableReason0Code { get; set; }
        public string pscUnavailableReason0StartDate { get; set; }
        public string pscUnavailableReason1 { get; set; }
        public int pscUnavailableReason1Code { get; set; }
        public string pscUnavailableReason1StartDate { get; set; }
        public int beneficialOwnershipBeneficialOwnersCount { get; set; }
        public int beneficialOwnershipRelationshipsCount { get; set; }
        public int beneficialOwnershipMaximumDegreeOfSeparation { get; set; }
        public double beneficialOwnershipTotalAllocatedOwnershipPercentage { get; set; }
        public int beneficialOwnershipOrganizationsCount { get; set; }
        public int beneficialOwnershipIndividualsCount { get; set; }
        public int beneficialOwnershipUnclassifedOwnersCount { get; set; }
        public int beneficialOwnershipCorporateBeneficiariesCount { get; set; }
        public int beneficialOwnershipStateOwnedOrganizationsCount { get; set; }
        public int beneficialOwnershipGovernmentOrganizationsCount { get; set; }
        public int beneficialOwnershipPubliclyTradingOrganizationsCount { get; set; }
        public int beneficialOwnershipPrivatelyHeldOrganizationsCount { get; set; }
        public int beneficialOwnershipPscCount { get; set; }
        public int beneficialOwnershipPscUniqueTypeCount { get; set; }
        public int beneficialOwnershipNationalityUnknownPSCCount { get; set; }
        public int beneficialOwnershipCountryUnknownPSCCount { get; set; }
        public string globalUltimateDuns { get; set; }
        public string globalUltimateOrganizationName { get; set; }
        public string domesticUltimateDuns { get; set; }
        public string domesticUltimateOrganizationName { get; set; }
        public string parentDuns { get; set; }
        public string parentOrganizationName { get; set; }
    }
    public class CMPBOSV1_BeneficialOwnerRelationships
    {
        public string DnBDUNSNumber { get; set; }
        public string APIType { get; set; }
        public byte isFilteredOut { get; set; }
        public int natureOfControlClass { get; set; }
        public string natureOfControlClassCode { get; set; }
        public string natureOfControlStartDate { get; set; }
        public string natureOfControlType { get; set; }
        public int natureOfControlTypeCode { get; set; }
        public int relationshipID { get; set; }
        public string relationshipType { get; set; }
        public int relationshipTypeCode { get; set; }
        public double sharePercentage { get; set; }
        public int sourceMemberID { get; set; }
        public int targetMemberID { get; set; }
        public Int64 RowId { get; set; }
    }
    public class CMPBOSV1_BeneficialOwners
    {
        public string DnBDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string addressCity { get; set; }
        public string addressCountry { get; set; }
        public string addressCountryIsoAlpha2Code { get; set; }
        public string addressCounty { get; set; }
        public string addressPostalCode { get; set; }
        public string addressState { get; set; }
        public string addressStreetLine1 { get; set; }
        public string addressStreetLine2 { get; set; }
        public string addressStreetLine3 { get; set; }
        public double beneficialOwnershipPercentage { get; set; }
        public string beneficiaryEntityType { get; set; }
        public int beneficiaryEntityTypeCode { get; set; }
        public string beneficiaryType { get; set; }
        public int beneficiaryTypeCode { get; set; }
        public string birthDate { get; set; }
        public int degreeOfSeparation { get; set; }
        public double directOwnershipPercentage { get; set; }
        public string duns { get; set; }
        public double indirectOwnershipPercentage { get; set; }
        public byte isBeneficiary { get; set; }
        public byte isFilteredOut { get; set; }
        public byte isOutofBusiness { get; set; }
        public string legalAuthority { get; set; }
        public string legalForm { get; set; }
        public int memberID { get; set; }
        public string name { get; set; }
        public string nationality { get; set; }
        public string ownershipUnavailableReason0 { get; set; }
        public int ownershipUnavailableReason0Code { get; set; }
        public string ownershipUnavailableReason1 { get; set; }
        public int ownershipUnavailableReason1Code { get; set; }
        public string ownershipUnavailableReason2 { get; set; }
        public int ownershipUnavailableReason2Code { get; set; }
        public string personId { get; set; }
        public string residenceCountryName { get; set; }
        public string controlOwnershipType { get; set; }
        public string businessEntityType { get; set; }
        public Int64 RowId { get; set; }
    }
    public class CMPBOSV1_BeneficialOwnershipCountryWiseSummary
    {
        public string countryISOAlpha2Code { get; set; }
        public int beneficialOwnersCount { get; set; }
    }
    public class CMPBOSV1_CombinedData
    {
        public int memberID { get; set; }
        public int degreeOfSeparation { get; set; }
        public string name { get; set; }
        public double beneficialOwnershipPercentage { get; set; }
        public double directOwnershipPercentage { get; set; }
        public double indirectOwnershipPercentage { get; set; }
        public string beneficiaryType { get; set; }
        public byte isBeneficiary { get; set; }
        public string address { get; set; }
        public int? nbrChildren { get; set; }
        public int manager_id { get; set; }
        public string duns { get; set; }
        public string legalForm { get; set; }
        public string addressCountryIsoAlpha2Code { get; set; }
        public string controlOwnershipType { get; set; }
        public string businessEntityType { get; set; }
        public string nodeColor { get; set; }
        public string addressStreetLine1 { get; set; }
        public string addressStreetLine2 { get; set; }
        public string addressStreetLine3 { get; set; }
        public string addressCounty { get; set; }
        public string addressCity { get; set; }
        public string addressState { get; set; }
        public string addressPostalCode { get; set; }
        public DateTime lastScreenedDate { get; set; }
        public string lastScreenedResult { get; set; }
        public string lastScreenedAlertType { get; set; }
        public string lastScreenedAlertTypeCode { get; set; }
    }
    public class GraphJson
    {
        public string ResultJSON { get; set; }
    }
    public class CMPBOSV1_BeneficialOwnershipCountryWisePSCSummary
    {
        public string countryISOAlpha2Code { get; set; }
        public int pscCount { get; set; }
    }
    public class CMPBOSV1_BeneficialOwnershipNationalityWisePSCSummary
    {
        public string nationality { get; set; }
        public int pscCount { get; set; }
    }
    public class CMPBOSV1_BeneficialOwnershipTypeWisePSCSummary
    {
        public int pscTypeCode { get; set; }
        public string pscType { get; set; }
        public int pscCount { get; set; }
    }
    public class ScreenResponseEntityViewModel
    {
        public CMPBOSV1_CombinedData parentData { get; set; }
        public List<ScreenResponseEntity> lstScreenRespinseData { get; set; }
    }
}