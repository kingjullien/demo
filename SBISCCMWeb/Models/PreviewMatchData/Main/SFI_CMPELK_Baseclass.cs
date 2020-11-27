using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.Main
{
    public class DunsInfo
    {
        public SFI_Parent_Input Input { get; set; }
        public SFI_CMPELK_Baseclass Base { get; set; }
        public List<SFI_CMPELK_CurrentPrincipalsclass> lstCurrentPrincipals { get; set; }
        public List<SFI_CMPELK_IndustryCodesclass> lstIndustryCodes { get; set; }
        public List<SFI_CMPELK_RegistrationNumbersclass> lstRegistrationNumbers { get; set; }
        public List<SFI_CMPELK_StockExchangesclass> lstStockExchanges { get; set; }
    }

    public class SFI_Parent_Input
    {
        public string DunsNumber { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Postal { get; set; }
        public string Country { get; set; }
        public string SrcId { get; set; }
        public string Phone { get; set; }
        public string RegistrationNum { get; set; }
        public string Website { get; set; }
    }

    public class SFI_CMPELK_Baseclass
    {
        public string DnBDUNSNumber { get; set; }       // DnBDUNSNumber
        public string startDate { get; set; }        // startDate
        public bool OutOfBusinessIndicator { get; set; }       // OutOfBusinessIndicator
        public string operatingStatus { get; set; }           // operatingStatus
        public Int32 operatingStatusCode { get; set; }   // operatingStatusCode
        public bool isMarketable { get; set; }           // isMarketable
        public bool isMailUndeliverable { get; set; }             // isMailUndeliverable
        public bool isTelephoneDisconnected { get; set; }    // isTelephoneDisconnected
        public bool isDelisted { get; set; }   // isDelisted
        public string dunsControlStatusDescription { get; set; }              // dunsControlStatusDescription
        public string dunsControlStatusCode { get; set; }           // dunsControlStatusCode
        public string dunsControlStatusFullReportDate { get; set; }          // dunsControlStatusFullReportDate
        public string dunsControlStatusLastUpdateDate { get; set; }        // dunsControlStatusLastUpdateDate
        public string primaryName { get; set; }              // primaryName
        public string tradeStyleName0 { get; set; }        // tradeStyleName0
        public Int32 tradeStylepriority0 { get; set; }       // tradeStylepriority0
        public string tradeStyleName1 { get; set; }        // tradeStyleName1
        public Int32 tradeStylepriority1 { get; set; }       // tradeStylepriority1
        public string tradeStyleName2 { get; set; }        // tradeStyleName2
        public Int32 tradeStylepriority2 { get; set; }       // tradeStylepriority2
        public string tradeStyleName3 { get; set; }        // tradeStyleName3
        public Int32 tradeStylepriority3 { get; set; }       // tradeStylepriority3
        public string tradeStyleName4 { get; set; }        // tradeStyleName4
        public Int32 tradeStylepriority4 { get; set; }       // tradeStylepriority4
        public string websiteAddressUrl0 { get; set; }    // websiteAddressUrl0
        public string websiteAddressDomainName0 { get; set; }              // websiteAddressDomainName0
        public string websiteAddressUrl1 { get; set; }    // websiteAddressUrl1
        public string websiteAddressDomainName1 { get; set; }              // websiteAddressDomainName1
        public string websiteAddressUrl2 { get; set; }    // websiteAddressUrl2
        public string websiteAddressDomainName2 { get; set; }              // websiteAddressDomainName2
        public string websiteAddressUrl3 { get; set; }    // websiteAddressUrl3
        public string websiteAddressDomainName3 { get; set; }              // websiteAddressDomainName3
        public string telephoneNumber { get; set; }      // telephoneNumber
        public string telephoneIsdCode { get; set; }       // telephoneIsdCode
        public string isUnreachable { get; set; }              // isUnreachable
        public string faxNumber { get; set; }     // faxNumber
        public string isdCode { get; set; }          // isdCode
        public string primaryAddressLanguageDescription { get; set; }     // primaryAddressLanguageDescription
        public Int32 primaryAddressLanguageDnbCode { get; set; }        // primaryAddressLanguageDnbCode
        public string primaryAddressCountry { get; set; }           // primaryAddressCountry
        public string primaryAddressCountryIsoAlpha2Code { get; set; } // primaryAddressCountryIsoAlpha2Code
        public string primaryAddressCountryFipsCode { get; set; }           // primaryAddressCountryFipsCode
        public string primaryAddressContinentalState { get; set; }              // primaryAddressContinentalState
        public string primaryAddressCity { get; set; }    // primaryAddressCity
        public string primaryAddressMinorTownName { get; set; }          // primaryAddressMinorTownName
        public string primaryAddressState { get; set; }  // primaryAddressState
        public string primaryAddressStateAbbreviatedName { get; set; }              // primaryAddressStateAbbreviatedName
        public string primaryAddressStateFipsCode { get; set; }              // primaryAddressStateFipsCode
        public string primaryAddressCounty { get; set; }            // primaryAddressCounty
        public string primaryAddressCountyFipsCode { get; set; }              // primaryAddressCountyFipsCode
        public string primaryAddressPostalCode { get; set; }     // primaryAddressPostalCode
        public string primaryAddressPostalCodePosition { get; set; }        // primaryAddressPostalCodePosition
        public Int32 primaryAddressPostalCodePositionCode { get; set; }            // primaryAddressPostalCodePositionCode
        public string primaryAddressStreetNumber { get; set; }              // primaryAddressStreetNumber
        public string primaryAddressStreetName { get; set; }    // primaryAddressStreetName
        public string primaryAddressStreetLine1 { get; set; }     // primaryAddressStreetLine1
        public string primaryAddressStreetLine2 { get; set; }     // primaryAddressStreetLine2
        public string primaryAddressPostOfficeBoxNumber { get; set; }  // primaryAddressPostOfficeBoxNumber
        public string primaryAddressPostOfficeBoxTypeDescription { get; set; }  // primaryAddressPostOfficeBoxTypeDescription
        public Int32 primaryAddressPostOfficeBoxTypeCode { get; set; }             // primaryAddressPostOfficeBoxTypeCode
        public string primaryAddressLatitude { get; set; }           // primaryAddressLatitude
        public string primaryAddressLongitude { get; set; }        // primaryAddressLongitude
        public string primaryAddressGeographicalPrecisionDescription { get; set; }              // primaryAddressGeographicalPrecisionDescription
        public Int32 primaryAddressGeographicalPrecisionCode { get; set; }       // primaryAddressGeographicalPrecisionCode
        public bool primaryAddressIsRegistered { get; set; }              // primaryAddressIsRegistered
        public string primaryAddressStatisticalAreaCbsaName { get; set; }           // primaryAddressStatisticalAreaCbsaName
        public string primaryAddressStatisticalAreaCbsaCode { get; set; }             // primaryAddressStatisticalAreaCbsaCode
        public string primaryAddressStatisticalAreaEconomicAreaOfInfluenceCode { get; set; }              // primaryAddressStatisticalAreaEconomicAreaOfInfluenceCode
        public string primaryAddressStatisticalAreaPopulationRankNumber { get; set; }              // primaryAddressStatisticalAreaPopulationRankNumber
        public string primaryAddressStatisticalAreaPopulationRankCode { get; set; }              // primaryAddressStatisticalAreaPopulationRankCode
        public string primaryAddressStatisticalAreaPopulationRank { get; set; }   // primaryAddressStatisticalAreaPopulationRank
        public string primaryAddressLocationOwnershipDescription { get; set; }  // primaryAddressLocationOwnershipDescription
        public string primaryAddressLocationOwnershipCode { get; set; }            // primaryAddressLocationOwnershipCode
        public string primaryAddressPremisesAreaMeasurement { get; set; }       // primaryAddressPremisesAreaMeasurement
        public string primaryAddressPremisesAreaUnitDescription { get; set; }    // primaryAddressPremisesAreaUnitDescription
        public Int32 primaryAddressPremisesAreaUnitCode { get; set; } // primaryAddressPremisesAreaUnitCode
        public string primaryAddressPremisesAreaReliabilityDescription { get; set; }              // primaryAddressPremisesAreaReliabilityDescription
        public Int32 primaryAddressPremisesAreaReliabilityCode { get; set; }     // primaryAddressPremisesAreaReliabilityCode
        public bool primaryAddressIsManufacturingLocation { get; set; }            // primaryAddressIsManufacturingLocation
        public string registeredAddressLanguage { get; set; }    // registeredAddressLanguage
        public Int32 registeredAddressLanguageDnbCode { get; set; }    // registeredAddressLanguageDnbCode
        public string registeredAddressCountry { get; set; }       // registeredAddressCountry
        public string registeredAddressIsoAlpha2Code { get; set; }           // registeredAddressIsoAlpha2Code
        public string registeredAddressCity { get; set; }              // registeredAddressCity
        public string registeredAddressMinorTownName { get; set; }      // registeredAddressMinorTownName
        public string registeredAddressState { get; set; }            // registeredAddressState
        public string registeredAddressStateAbbreviatedName { get; set; }          // registeredAddressStateAbbreviatedName
        public string registeredAddressCounty { get; set; }        // registeredAddressCounty
        public string registeredAddressPostalCode { get; set; } // registeredAddressPostalCode
        public string registeredAddressPostalCodePositionDescription { get; set; }              // registeredAddressPostalCodePositionDescription
        public Int32 registeredAddressPostalCodePositionCode { get; set; }        // registeredAddressPostalCodePositionCode
        public string registeredAddressStreetNumber { get; set; }              // registeredAddressStreetNumber
        public string registeredAddressStreetName { get; set; }              // registeredAddressStreetName
        public string registeredAddressStreetLine1 { get; set; } // registeredAddressStreetLine1
        public string registeredAddressStreetLine2 { get; set; } // registeredAddressStreetLine2
        public string registeredAddressStreetLine3 { get; set; } // registeredAddressStreetLine3
        public string registeredAddressStreetLine4 { get; set; } // registeredAddressStreetLine4
        public string registeredAddressPostOfficeBoxNumber { get; set; }            // registeredAddressPostOfficeBoxNumber
        public string registeredAddressPostOfficeBoxType { get; set; }    // registeredAddressPostOfficeBoxType
        public Int32 registeredAddressPostOfficeBoxTypeCode { get; set; }         // registeredAddressPostOfficeBoxTypeCode
        public string mailingAddressLanguageDescription { get; set; }      // mailingAddressLanguageDescription
        public Int32 mailingAddressLanguageCode { get; set; } // mailingAddressLanguageCode
        public string mailingAddressCountry { get; set; }            // mailingAddressCountry
        public string mailingAddressCountryIsoAlpha2Code { get; set; }  // mailingAddressCountryIsoAlpha2Code
        public string mailingAddressContinentalState { get; set; }              // mailingAddressContinentalState
        public string mailingAddressCity { get; set; }     // mailingAddressCity
        public string mailingAddressMinorTownName { get; set; }           // mailingAddressMinorTownName
        public string mailingAddressState { get; set; }   // mailingAddressState
        public string mailingAddressStateAbbreviatedName { get; set; } // mailingAddressStateAbbreviatedName
        public string mailingAddressCounty { get; set; }             // mailingAddressCounty
        public string mailingAddressPostalCode { get; set; }      // mailingAddressPostalCode
        public string mailingAddressPostalCodePositionDescription { get; set; }   // mailingAddressPostalCodePositionDescription
        public Int32 mailingAddressPostalCodePositionCode { get; set; }             // mailingAddressPostalCodePositionCode
        public string mailingAddressPostalRoute { get; set; }     // mailingAddressPostalRoute
        public string mailingAddressStreetNumber { get; set; } // mailingAddressStreetNumber
        public string mailingAddressStreetName { get; set; }     // mailingAddressStreetName
        public string mailingAddressStreetAddressLine1 { get; set; }        // mailingAddressStreetAddressLine1
        public string mailingAddressStreetAddressLine2 { get; set; }        // mailingAddressStreetAddressLine2
        public string mailingAddressPostOfficeBoxNumber { get; set; }   // mailingAddressPostOfficeBoxNumber
        public string mailingAddressPostOfficeBoxTypeDescription { get; set; }   // mailingAddressPostOfficeBoxTypeDescription
        public string thirdPartyAssessmentDescription { get; set; }           // thirdPartyAssessmentDescription
        public Int32 thirdPartyAssessmentCode { get; set; }      // thirdPartyAssessmentCode
        public string thirdPartyAssessmentDate { get; set; }      // thirdPartyAssessmentDate
        public string thirdPartyAssessmentValue { get; set; }     // thirdPartyAssessmentValue
        public string businessEntityTypeDescription { get; set; }              // businessEntityTypeDescription
        public Int32 businessEntityTypeCode { get; set; }           // businessEntityTypeCode
        public string controlOwnershipDate { get; set; }             // controlOwnershipDate
        public string controlOwnershipTypeDescription { get; set; }         // controlOwnershipTypeDescription
        public Int32 controlOwnershipTypeCode { get; set; }     // controlOwnershipTypeCode
        public bool isAgent { get; set; }       // isAgent
        public bool isImporter { get; set; }  // isImporter
        public bool isExporter { get; set; }   // isExporter
        public string financialStatementToDate { get; set; }       // financialStatementToDate
        public string financialStatementDuration { get; set; }    // financialStatementDuration
        public string financialinformationScopeDescription { get; set; }   // financialinformationScopeDescription
        public Int32 financialInformationScopeDnBCode { get; set; }       // financialInformationScopeDnBCode
        public string financialsReliabilityDescription { get; set; }              // financialsReliabilityDescription
        public Int32 financialsReliabilityDnbCode { get; set; }    // financialsReliabilityDnbCode
        public string financialsUnitCode { get; set; }      // financialsUnitCode
        public string financialsAccountantName { get; set; }      // financialsAccountantName
        public string financialsYearlyRevenueValue1 { get; set; }              // financialsYearlyRevenueValue1
        public string financialsYearlyRevenueCurrency1 { get; set; }         // financialsYearlyRevenueCurrency1
        public string financialsYearlyRevenueValue2 { get; set; }              // financialsYearlyRevenueValue2
        public string financialsYearlyRevenueCurrency2 { get; set; }         // financialsYearlyRevenueCurrency2
        public string financialsYearlyRevenueTrendTimePeriodDescription0 { get; set; }              // financialsYearlyRevenueTrendTimePeriodDescription0
        public Int32 financialsYearlyRevenueTrendTimePeriodCode0 { get; set; }              // financialsYearlyRevenueTrendTimePeriodCode0
        public string financialsYearlyRevenueTrenGrowthRate0 { get; set; }         // financialsYearlyRevenueTrenGrowthRate0
        public string financialsYearlyRevenueTrendTimePeriodDescription1 { get; set; }              // financialsYearlyRevenueTrendTimePeriodDescription1
        public Int32 financialsYearlyRevenueTrendTimePeriodCode1 { get; set; }              // financialsYearlyRevenueTrendTimePeriodCode1
        public string financialsYearlyRevenueTrenGrowthRate1 { get; set; }         // financialsYearlyRevenueTrenGrowthRate1
        public string mostSeniorPrincipalGivenName { get; set; }              // mostSeniorPrincipalGivenName
        public string mostSeniorPrincipalFamilyName { get; set; }              // mostSeniorPrincipalFamilyName
        public string mostSeniorPrincipalFullName { get; set; } // mostSeniorPrincipalFullName
        public string mostSeniorPrincipalNamePreFix { get; set; }              // mostSeniorPrincipalNamePreFix
        public string mostSeniorPrincipalNameSuffix { get; set; }              // mostSeniorPrincipalNameSuffix
        public string mostSeniorPrincipalGender { get; set; }     // mostSeniorPrincipalGender
        public string mostSeniorPrincipalJobTitle { get; set; }    // mostSeniorPrincipalJobTitle
        public string mostSeniorPrincipalManagementResponsibilitiesDescription0 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesDescription0
        public string mostSeniorPrincipalManagementResponsibilitiesMrcCode0 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesMrcCode0
        public string mostSeniorPrincipalManagementResponsibilitiesDescription1 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesDescription1
        public string mostSeniorPrincipalManagementResponsibilitiesMrcCode1 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesMrcCode1
        public string mostSeniorPrincipalManagementResponsibilitiesDescription2 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesDescription2
        public string mostSeniorPrincipalManagementResponsibilitiesMrcCode2 { get; set; }              // mostSeniorPrincipalManagementResponsibilitiesMrcCode2
        public bool socioEconomicInformationIsMinorityOwned { get; set; }      // socioEconomicInformationIsMinorityOwned
        public bool socioEconomicInformationIsSmallBusiness { get; set; }         // socioEconomicInformationIsSmallBusiness
        public bool isStandalone { get; set; }            // isStandalone
        public string corporateLinkageFamilytreeRolesPlayedDescription1 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDescription1
        public Int32 corporateLinkageFamilytreeRolesPlayedDnbCode1 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDnbCode1
        public string corporateLinkageFamilytreeRolesPlayedDescription2 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDescription2
        public Int32 corporateLinkageFamilytreeRolesPlayedDnbCode2 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDnbCode2
        public string corporateLinkageFamilytreeRolesPlayedDescription3 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDescription3
        public Int32 corporateLinkageFamilytreeRolesPlayedDnbCode3 { get; set; }              // corporateLinkageFamilytreeRolesPlayedDnbCode3
        public Int32 corporateLinkageHierarchyLevel { get; set; }              // corporateLinkageHierarchyLevel
        public Int32 corporateLinkageGlobalUltimateFamilyTreeMembersCount { get; set; }              // corporateLinkageGlobalUltimateFamilyTreeMembersCount
        public string corporateLinkageGlobalUltimateDuns { get; set; }   // corporateLinkageGlobalUltimateDuns
        public string corporateLinkageGlobalUltimatePrimaryName { get; set; }  // corporateLinkageGlobalUltimatePrimaryName
        public string corporateLinkageGlobalUltimateCountry { get; set; }            // corporateLinkageGlobalUltimateCountry
        public string corporateLinkageGlobalUltimateCountryIsoAlpha2Code { get; set; }              // corporateLinkageGlobalUltimateCountryIsoAlpha2Code
        public string corporateLinkageGlobalUltimateCountryFipsCode { get; set; }              // corporateLinkageGlobalUltimateCountryFipsCode
        public string corporateLinkaglobalUltimateContinentalState { get; set; }  // corporateLinkaglobalUltimateContinentalState
        public string corporateLinkageGlobalUltimateCity { get; set; }     // corporateLinkageGlobalUltimateCity
        public string corporateLinkageGlobalUltimateState { get; set; }   // corporateLinkageGlobalUltimateState
        public string corporateLinkageGlobalUltimateStateAbbreviatedName { get; set; }              // corporateLinkageGlobalUltimateStateAbbreviatedName
        public string corporateLinkageGlobalUltimatePostalCode { get; set; }      // corporateLinkageGlobalUltimatePostalCode
        public string corporateLinkageGlobalUltimateStreetAddressLine1 { get; set; }              // corporateLinkageGlobalUltimateStreetAddressLine1
        public string corporateLinkageGlobalUltimateStreetAddressLine2 { get; set; }              // corporateLinkageGlobalUltimateStreetAddressLine2
        public string corporateLinkageDomesticUltimateDuns { get; set; }            // corporateLinkageDomesticUltimateDuns
        public string corporateLinkageDomesticUltimatePrimaryName { get; set; }              // corporateLinkageDomesticUltimatePrimaryName
        public string corporateLinkageDomesticUltimateCountry { get; set; }       // corporateLinkageDomesticUltimateCountry
        public string corporateLinkageDomesticUltimateIsoAlpha2Code { get; set; }              // corporateLinkageDomesticUltimateIsoAlpha2Code
        public string corporateLinkageDomesticUltimateCountryFipsCode { get; set; }              // corporateLinkageDomesticUltimateCountryFipsCode
        public string corporateLinkageDomesticUltimateContinentalState { get; set; }              // corporateLinkageDomesticUltimateContinentalState
        public string corporateLinkageDomesticUltimateCity { get; set; }              // corporateLinkageDomesticUltimateCity
        public string corporateLinkageDomesticUltimateState { get; set; }            // corporateLinkageDomesticUltimateState
        public string corporateLinkageDomesticUltimateStateAbbreviatedName { get; set; }              // corporateLinkageDomesticUltimateStateAbbreviatedName
        public string corporateLinkageDomesticUltimatePostalCode { get; set; } // corporateLinkageDomesticUltimatePostalCode
        public string corporateLinkageDomesticUltimateStreetAddressLine1 { get; set; }              // corporateLinkageDomesticUltimateStreetAddressLine1
        public string corporateLinkageDomesticUltimateStreetAddressLine2 { get; set; }              // corporateLinkageDomesticUltimateStreetAddressLine2
        public string corporateLinkageParentDuns { get; set; }  // corporateLinkageParentDuns
        public string corporateLinkageParentName { get; set; }              // corporateLinkageParentName
        public string corporateLinkageParentCountry { get; set; }              // corporateLinkageParentCountry
        public string corporateLinkageParentIsoAlpha2Code { get; set; } // corporateLinkageParentIsoAlpha2Code
        public string corporateLinkageParentCounrtyFipsCode { get; set; }           // corporateLinkageParentCounrtyFipsCode
        public string corporateLinkageParentContinentalState { get; set; }           // corporateLinkageParentContinentalState
        public string corporateLinkageParentCity { get; set; }    // corporateLinkageParentCity
        public string corporateLinkageParentState { get; set; }  // corporateLinkageParentState
        public string corporateLinkageParentStateAbbreviatedName { get; set; }              // corporateLinkageParentStateAbbreviatedName
        public string corporateLinkageParentPostalCode { get; set; }       // corporateLinkageParentPostalCode
        public string corporateLinkageParentStreetAddressLine1 { get; set; }       // corporateLinkageParentStreetAddressLine1
        public string corporateLinkageParentStreetAddressLine2 { get; set; }       // corporateLinkageParentStreetAddressLine2
        public string corporateLinkageHeadQuarterDuns { get; set; }       // corporateLinkageHeadQuarterDuns
        public string corporateLinkageHeadQuarter { get; set; }              // corporateLinkageHeadQuarter
        public string corporateLinkageHeadQuarterCountry { get; set; }  // corporateLinkageHeadQuarterCountry
        public string corporateLinkageHeadQuarterIsoAlpha2Code { get; set; }   // corporateLinkageHeadQuarterIsoAlpha2Code
        public string corporateLinkageHeadQuarterCountryFipscode { get; set; }              // corporateLinkageHeadQuarterCountryFipscode
        public string corporateLinkageHeadQuarterContinentalState { get; set; }              // corporateLinkageHeadQuarterContinentalState
        public string corporateLinkageHeadQuarterCity { get; set; }         // corporateLinkageHeadQuarterCity
        public string corporateLinkageHeadQuarterState { get; set; }      // corporateLinkageHeadQuarterState
        public string corporateLinkageHeadQuarterStateAbbreviatedName { get; set; }              // corporateLinkageHeadQuarterStateAbbreviatedName
        public string corporateLinkageHeadQuarterPostalCode { get; set; }          // corporateLinkageHeadQuarterPostalCode
        public string corporateLinkageHeadQuarterStreetAddressLine1 { get; set; }              // corporateLinkageHeadQuarterStreetAddressLine1
        public string corporateLinkageHeadQuarterStreetAddressLine2 { get; set; }              // corporateLinkageHeadQuarterStreetAddressLine2
        public string PrimarySic { get; set; }      // PrimarySic
        public string PrimarySicDesc { get; set; }            // PrimarySicDesc
        public string SecondSic { get; set; }       // SecondSic
        public string SecondSicDesc { get; set; }             // SecondSicDesc
        public string ThirdSic { get; set; }          // ThirdSic
        public string ThirdSicDesc { get; set; }   // ThirdSicDesc
        public string FourthSic { get; set; }        // FourthSic
        public string FourthSicDesc { get; set; }              // FourthSicDesc
        public string FifthSic { get; set; }           // FifthSic
        public string FifthSicDesc { get; set; }    // FifthSicDesc
        public string SixthSic { get; set; }           // SixthSic
        public string SixthSicDesc { get; set; }   // SixthSicDesc
        public string PrimaryNaics { get; set; }  // PrimaryNaics
        public string PrimaryNaicsDesc { get; set; }       // PrimaryNaicsDesc
        public string SecondNaics { get; set; }   // SecondNaics
        public string SecondNaicsDesc { get; set; }        // SecondNaicsDesc
        public string ThirdNaics { get; set; }      // ThirdNaics
        public string ThirdNaicsDesc { get; set; }            // ThirdNaicsDesc
        public string FourthNaics { get; set; }    // FourthNaics
        public string FourthNaicsDesc { get; set; }         // FourthNaicsDesc
        public string FifthNaics { get; set; }       // FifthNaics
        public string FifthNaicsDesc { get; set; }             // FifthNaicsDesc
        public string SixthNaics { get; set; }      // SixthNaics
        public string SixthNaicsDesc { get; set; }            // SixthNaicsDesc
        public string PrimarySic8 { get; set; }    // PrimarySic8
        public string PrimarySic8Desc { get; set; }          // PrimarySic8Desc
        public string SecondSic8 { get; set; }     // SecondSic8
        public string SecondSic8Desc { get; set; }           // SecondSic8Desc
        public string ThirdSic8 { get; set; }        // ThirdSic8
        public string ThirdSic8Desc { get; set; }              // ThirdSic8Desc
        public string FourthSic8 { get; set; }      // FourthSic8
        public string FourthSic8Desc { get; set; }            // FourthSic8Desc
        public string FifthSic8 { get; set; }         // FifthSic8
        public string FifthSic8Desc { get; set; } // FifthSic8Desc
        public string SixthSic8 { get; set; }         // SixthSic8
        public string SixthSic8Desc { get; set; } // SixthSic8Desc
        public string UsTaxId { get; set; }          // UsTaxId
        public string ConsolidatedNumberOfEmployees { get; set; }         // ConsolidatedNumberOfEmployees
        public string ConsolidatedReliabilityDescription { get; set; }         // ConsolidatedReliabilityDescription
        public string ConsolidatedReliabilityDnBCode { get; set; }              // ConsolidatedReliabilityDnBCode
        public string ConsolidatedGrowthRate { get; set; }         // ConsolidatedGrowthRate
        public string ConsolidatedTimePeriod { get; set; }          // ConsolidatedTimePeriod
        public string IndividualNumberOfEmployees { get; set; }              // IndividualNumberOfEmployees
        public string IndividualReliabilityDescription { get; set; }              // IndividualReliabilityDescription
        public string IndividualReliabilityDnBCode { get; set; }  // IndividualReliabilityDnBCode
        public string IndividualGrowthRate { get; set; }              // IndividualGrowthRate
        public string IndividualTimePeriod { get; set; }  // IndividualTimePeriod
        public string primaryStockSymbol { get; set; }   // primaryStockSymbol
        public string primaryStockExchange { get; set; }             // primaryStockExchange
        public string primaryStockExchangeCountryIsoAlpha2Code { get; set; }   // primaryStockExchangeCountryIsoAlpha2Code
    }
    public class SFI_CMPELK_CurrentPrincipalsclass
    {
        public string DnbDUNSNumber { get; set; }       // DnbDUNSNumber
        public string APIType { get; set; }          // APIType
        public string TransactionTimestamp { get; set; }             // TransactionTimestamp
        public string givenName { get; set; }     // givenName
        public string familyName { get; set; }   // familyName
        public string fullName { get; set; }        // fullName
        public string namePrefix { get; set; }     // namePrefix
        public string nameSuffix { get; set; }     // nameSuffix
        public string gender { get; set; }            // gender
        public string jobTitle { get; set; }           // jobTitle
        public string managementResponsibilitieCode0 { get; set; }         // managementResponsibilitieCode0
        public string managementResponsibilitie0 { get; set; }  // managementResponsibilitie0
        public string managementResponsibilitieCode1 { get; set; }         // managementResponsibilitieCode1
        public string managementResponsibilitie1 { get; set; }  // managementResponsibilitie1
        public string managementResponsibilitieCode2 { get; set; }         // managementResponsibilitieCode2
        public string managementResponsibilitie2 { get; set; }  // managementResponsibilitie2
        public string RecordIdent { get; set; }   // RecordIdent

    }
    public class SFI_CMPELK_IndustryCodesclass
    {
        public string DnbDUNSNumber { get; set; }       // DnbDUNSNumber
        public string APIType { get; set; }          // APIType
        public string TransactionTimestamp { get; set; }             // TransactionTimestamp
        public Int32 priority { get; set; }            // priority
        public string industryCode { get; set; } // industryCode
        public string industryDescription { get; set; }    // industryDescription
        public string typeCode { get; set; }        // typeCode
        public string typeDescription { get; set; }           // typeDescription
        public string RowId { get; set; }             // RowId
        public string RecordIdent { get; set; }   // RecordIdent

    }
    public class SFI_CMPELK_RegistrationNumbersclass
    {
        public string DnbDUNSNumber { get; set; }       // DnbDUNSNumber
        public string APIType { get; set; }          // APIType
        public string TransactionTimestamp { get; set; }             // TransactionTimestamp
        public string registrationNumber { get; set; }    // registrationNumber
        public string registrationNumbersTypeDescription { get; set; }    // registrationNumbersTypeDescription
        public Int32 registrationNumbersTypeCode { get; set; }              // registrationNumbersTypeCode
        public string RecordIdent { get; set; }   // RecordIdent
    }
    public class SFI_CMPELK_StockExchangesclass
    {

        public string DnbDUNSNumber { get; set; }       // DnbDUNSNumber
        public string APIType { get; set; }          // APIType
        public string TransactionTimestamp { get; set; }             // TransactionTimestamp
        public string tickerName { get; set; }    // tickerName
        public string description { get; set; }     // description
        public string countryIsoAlpha2Code { get; set; }             // countryIsoAlpha2Code
        public string isPrimary { get; set; }        // isPrimary
        public string RecordIdent { get; set; }   // RecordIdent
    }
}