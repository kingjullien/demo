using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility.IdentityResolution
{
    public class TransactionDetail
    {
        public string transactionID { get; set; }
        public string transactionTimestamp { get; set; }
        public string inLanguage { get; set; }
        public string serviceVersion { get; set; }
    }

    public class Address
    {
        public string countryISOAlpha2Code { get; set; }
        public string addressLocality { get; set; }
    }

    public class InquiryDetail
    {
        public string inLanguage { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
        public bool isCleanseAndStandardizeInformationRequired { get; set; }
        public string duns { get; set; }
    }

    public class OperatingStatus
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class DunsControlStatus
    {
        public OperatingStatus operatingStatus { get; set; }
        public bool isMailUndeliverable { get; set; }
    }

    public class AddressCountry
    {
        public string isoAlpha2Code { get; set; }
        public string name { get; set; }
    }

    public class AddressLocality
    {
        public string name { get; set; }
    }

    public class AddressRegion
    {
        public string name { get; set; }
        public object abbreviatedName { get; set; }
    }

    public class StreetAddress
    {
        public string line1 { get; set; }
        public object line2 { get; set; }
    }

    public class PrimaryAddress
    {
        public AddressCountry addressCountry { get; set; }
        public AddressLocality addressLocality { get; set; }
        public AddressRegion addressRegion { get; set; }
        public string postalCode { get; set; }
        public object postalCodeExtension { get; set; }
        public StreetAddress streetAddress { get; set; }
    }
    public class MostSeniorPrincipal
    {
        public string fullName { get; set; }
    }

    public class FamilytreeRolesPlayed
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class CorporateLinkage
    {
        public List<FamilytreeRolesPlayed> familytreeRolesPlayed { get; set; }
    }


    public class MailingAddress
    {
        public AddressCountry addressCountry { get; set; }
        public AddressLocality addressLocality { get; set; }
        public AddressRegion addressRegion { get; set; }
        public string postalCode { get; set; }
        public object postalCodeExtension { get; set; }
        public StreetAddress streetAddress { get; set; }
    }
    public class Telephone
    {
        public string telephoneNumber { get; set; }
        public bool isUnreachable { get; set; }

    }
    public class RegistrationNumbers
    {
        public string registrationNumber { get; set; }
        public string typeDescription { get; set; }
        public string typeDnBCode { get; set; }
    }
    public class MostSeniorPrincipals
    {
        public string fullName { get; set; }
    }
    public class TradeStyleName
    {
        public string name { get; set; }
        public int priority { get; set; }
    }
    public class Website
    {
        public string url { get; set; }
        public string domainName { get; set; }
    }
    public class Organization
    {
        public string duns { get; set; }
        public List<Website> websiteAddress { get; set; }
        public DunsControlStatus dunsControlStatus { get; set; }
        public string primaryName { get; set; }
        public List<TradeStyleName> tradeStyleNames { get; set; }
        public List<Telephone> telephone { get; set; }
        public PrimaryAddress primaryAddress { get; set; }
        public MailingAddress mailingAddress { get; set; }
        public List<RegistrationNumbers> registrationNumbers { get; set; }
        public List<MostSeniorPrincipals> mostSeniorPrincipals { get; set; }
        public object isStandalone { get; set; }
        public CorporateLinkage corporateLinkage { get; set; }
    }

    public class MatchGradeComponent
    {
        public string componentType { get; set; }
        public string componentRating { get; set; }
    }
    public class MatchDataProfileComponent
    {
        public string componentType { get; set; }
        public string componentValue { get; set; }
    }
    public class MatchQualityInformation
    {
        public int confidenceCode { get; set; }
        public string matchGrade { get; set; }
        public int matchGradeComponentsCount { get; set; }
        public List<MatchGradeComponent> matchGradeComponents { get; set; }
        public string matchDataProfile { get; set; }
        public int matchDataProfileComponentsCount { get; set; }
        public List<MatchDataProfileComponent> matchDataProfileComponents { get; set; }
        public object nameMatchScore { get; set; }
    }

    public class MatchCandidate
    {
        public int displaySequence { get; set; }
        public Organization organization { get; set; }
        public MatchQualityInformation matchQualityInformation { get; set; }
    }

    public class CleanseAndStandardizeInformation
    {
    }
    public class Error
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string errorInformationURL { get; set; }
    }
    public class IdentityResolutionResponse
    {
        public Error error { get; set; }
        public TransactionDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public int candidatesMatchedQuantity { get; set; }
        public string matchDataCriteria { get; set; }
        public List<MatchCandidate> matchCandidates { get; set; }
        public CleanseAndStandardizeInformation cleanseAndStandardizeInformation { get; set; }
    }
}