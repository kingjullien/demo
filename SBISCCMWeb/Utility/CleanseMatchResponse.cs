using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{


    public class Address
    {
        public string CountryISOAlpha2Code { get; set; }
        public string TerritoryName { get; set; }
    }

    public class InquiryDetail
    {
        public string SubjectName { get; set; }
        public Address Address { get; set; }
    }

    public class MatchDataCriteriaText
    {
        public string _param { get; set; }
    }

    public class OrganizationName
    {
        public string _param { get; set; }
    }

    public class OrganizationPrimaryName
    {
        public OrganizationName _OrganizationName { get; set; }
    }

    public class StreetAddressLine
    {
        public string LineText { get; set; }
    }

    public class PrimaryAddress
    {
        public List<StreetAddressLine> StreetAddressLine { get; set; }
        public string PrimaryTownName { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PostalCode { get; set; }
        public string TerritoryAbbreviatedName { get; set; }
        public bool UndeliverableIndicator { get; set; }
        public string PostalCodeExtensionCode { get; set; }
    }

    public class StreetAddressLine2
    {
        public string LineText { get; set; }
    }

    public class MailingAddress
    {
        public string CountryISOAlpha2Code { get; set; }
        public bool UndeliverableIndicator { get; set; }
        public List<StreetAddressLine2> StreetAddressLine { get; set; }
        public string PrimaryTownName { get; set; }
        public string PostalCode { get; set; }
        public string PostalCodeExtensionCode { get; set; }
        public string TerritoryAbbreviatedName { get; set; }
    }

    public class TelephoneNumber
    {
        public string TelecommunicationNumber { get; set; }
        public bool UnreachableIndicator { get; set; }
    }

    public class OperatingStatusText
    {
        public string _param { get; set; }
    }

    public class FamilyTreeMemberRoleText
    {
        public string _param { get; set; }
    }

    public class FamilyTreeMemberRole
    {
        public FamilyTreeMemberRoleText FamilyTreeMemberRoleText { get; set; }
    }

    public class MatchBasisText
    {
        public string _param { get; set; }
    }

    public class MatchBasi
    {
        public bool EndIndicator { get; set; }
        public string SubjectTypeText { get; set; }
        public bool SeniorPrincipalIndicator { get; set; }
        public MatchBasisText MatchBasisText { get; set; }
    }

    public class MatchGradeComponentTypeText
    {
        public string _param { get; set; }
    }

    public class MatchGradeComponent
    {
        public MatchGradeComponentTypeText MatchGradeComponentTypeText { get; set; }
        public string MatchGradeComponentRating { get; set; }
        public double MatchGradeComponentScore { get; set; }
    }

    public class MatchDataProfileComponentTypeText
    {
        public string _param { get; set; }
    }

    public class MatchDataProfileComponent
    {
        public MatchDataProfileComponentTypeText MatchDataProfileComponentTypeText { get; set; }
        public string MatchDataProfileComponentValue { get; set; }
    }

    public class MatchQualityInformation
    {
        public int ConfidenceCodeValue { get; set; }
        public List<MatchBasi> MatchBasis { get; set; }
        public string MatchGradeText { get; set; }
        public int MatchGradeComponentCount { get; set; }
        public List<MatchGradeComponent> MatchGradeComponent { get; set; }
        public string MatchDataProfileText { get; set; }
        public int MatchDataProfileComponentCount { get; set; }
        public List<MatchDataProfileComponent> MatchDataProfileComponent { get; set; }
    }

    public class OrganizationName2
    {
        public string _param { get; set; }
    }

    public class TradeStyleName
    {
        public OrganizationName2 OrganizationName { get; set; }
    }
    public class SeniorPrincipalName
    {
        public string FullName { get; set; }

    }

    public class MatchCandidate
    {
        public string DUNSNumber { get; set; }
        public OrganizationPrimaryName OrganizationPrimaryName { get; set; }
        public PrimaryAddress PrimaryAddress { get; set; }
        public MailingAddress MailingAddress { get; set; }
        public TelephoneNumber TelephoneNumber { get; set; }
        public OperatingStatusText OperatingStatusText { get; set; }
        public List<FamilyTreeMemberRole> FamilyTreeMemberRole { get; set; }
        public bool StandaloneOrganizationIndicator { get; set; }
        public MatchQualityInformation MatchQualityInformation { get; set; }
        public int DisplaySequence { get; set; }
        public TradeStyleName TradeStyleName { get; set; }

        public SeniorPrincipalName SeniorPrincipalName { get; set; }

        public bool? MarketabilityIndicator { get; set; }
    }

    public class MatchResponseDetail
    {
        public MatchDataCriteriaText MatchDataCriteriaText { get; set; }
        public int CandidateMatchedQuantity { get; set; }
        public List<MatchCandidate> MatchCandidate { get; set; }
    }


    public class MatchResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public MatchResponseDetail MatchResponseDetail { get; set; }
    }

    public class MatchResponseMain
    {
        public MatchResponse MatchResponse { get; set; }
    }
    public class GetCleanseMatchResponseDetail
    {
        public InquiryDetail InquiryDetail { get; set; }
        public MatchResponseDetail MatchResponseDetail { get; set; }
        public object InquiryReferenceDetail { get; set; }
    }
    public class GetCleanseMatchResponse
    {
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public string ServiceVersionNumber { get; set; }
        public GetCleanseMatchResponseDetail GetCleanseMatchResponseDetail { get; set; }
    }
    public class GetCleanseMatchResponseMain
    {
        public GetCleanseMatchResponse GetCleanseMatchResponse { get; set; }
    }
}