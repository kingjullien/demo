using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SBISCCMWeb.Utility.BuildList
{
    public class AdditionalDetail
    {
        public string Key1 { get; set; }
        public string Value1 { get; set; }
    }

    public class TransactionDetail
    {
        public string transactionID { get; set; }
        public string transactionTimestamp { get; set; }
        public string inLanguage { get; set; }
        public string serviceVersion { get; set; }
        public List<AdditionalDetail> additionalDetail { get; set; }
    }
    public class InquiryDetail
    {
        public string searchTerm { get; set; }
        public string countryISOAlpha2Code { get; set; }
        public string duns { get; set; }
        public bool isOutOfBusiness { get; set; }
        public bool isMarketable { get; set; }
        public List<string> usSicV4 { get; set; }
        public YearlyRevenue yearlyRevenue { get; set; }
        public bool isMailUndeliverable { get; set; }
        public bool isTelephoneDisconnected { get; set; }
        public string telephoneNumber { get; set; }
        public string domain { get; set; }
        public List<string> registrationNumbers { get; set; }
        public List<int> businessEntityType { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string streetAddressLine1 { get; set; }
        public string postalCode { get; set; }
        public LocationRadius locationRadius { get; set; }
        public string primaryName { get; set; }
        public string tradeStyleName { get; set; }
        public string tickerSymbol { get; set; }
        public List<int> familytreeRolesPlayed { get; set; }
        public GlobalUltimateFamilyTreeMembersCount globalUltimateFamilyTreeMembersCount { get; set; }
        public NumberOfEmployees numberOfEmployees { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }

    public class DunsControlStatus
    {
        public bool isOutOfBusiness { get; set; }
        public bool isMarketable { get; set; }
        public bool isMailUndeliverable { get; set; }
        public bool isTelephoneDisconnected { get; set; }
    }

    public class TradeStyleName
    {
        public string name { get; set; }
        public int priority { get; set; }
    }

    public class AddressCountry
    {
        public string isoAlpha2Code { get; set; }
    }

    public class AddressLocality
    {
        public string name { get; set; }
    }

    public class AddressRegion
    {
        public string name { get; set; }
    }

    public class StreetAddress
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

    public class PrimaryAddress
    {
        public AddressCountry addressCountry { get; set; }
        public AddressLocality addressLocality { get; set; }
        public AddressRegion addressRegion { get; set; }
        public string postalCode { get; set; }
        public StreetAddress streetAddress { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class AddressCountry2
    {
        public string isoAlpha2Code { get; set; }
    }

    public class AddressLocality2
    {
        public string name { get; set; }
    }

    public class AddressRegion2
    {
        public string name { get; set; }
    }

    public class StreetAddress2
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

    public class RegisteredAddress
    {
        public AddressCountry2 addressCountry { get; set; }
        public AddressLocality2 addressLocality { get; set; }
        public AddressRegion2 addressRegion { get; set; }
        public string postalCode { get; set; }
        public StreetAddress2 streetAddress { get; set; }
    }

    public class AddressCountry3
    {
        public string isoAlpha2Code { get; set; }
    }

    public class AddressLocality3
    {
        public string name { get; set; }
    }

    public class AddressRegion3
    {
        public string name { get; set; }
    }

    public class StreetAddress3
    {
        public string line1 { get; set; }
        public string line2 { get; set; }
    }

    public class MailingAddress
    {
        public AddressCountry3 addressCountry { get; set; }
        public AddressLocality3 addressLocality { get; set; }
        public AddressRegion3 addressRegion { get; set; }
        public string postalCode { get; set; }
        public StreetAddress3 streetAddress { get; set; }
    }

    public class Telephone
    {
        public string telephoneNumber { get; set; }
    }

    public class RegistrationNumber
    {
        public string registrationNumber { get; set; }
        public string typeDescription { get; set; }
        public int typeDnBCode { get; set; }
    }

    public class NumberOfEmployee
    {
        public int value { get; set; }
        public string informationScopeDescription { get; set; }
        public int informationScopeDnBCode { get; set; }
        public string reliabilityDescription { get; set; }
        public int reliabilityDnBCode { get; set; }
    }

    public class BusinessEntityType
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class YearlyRevenue2
    {
        public double value { get; set; }
        public string currency { get; set; }
    }

    public class Financial
    {
        public List<YearlyRevenue2> yearlyRevenue { get; set; }
    }

    public class FamilytreeRolesPlayed
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class CorporateLinkage
    {
        public bool isBranch { get; set; }
        public List<FamilytreeRolesPlayed> familytreeRolesPlayed { get; set; }
        public int globalUltimateFamilyTreeMembersCount { get; set; }
    }

    public class PrimaryIndustryCode
    {
        public string usSicV4 { get; set; }
        public string usSicV4Description { get; set; }
    }

    [Serializable]
    public class Organization
    {
        public string duns { get; set; }
        public DunsControlStatus dunsControlStatus { get; set; }
        public string primaryName { get; set; }
        public List<TradeStyleName> tradeStyleNames { get; set; }
        public PrimaryAddress primaryAddress { get; set; }
        public RegisteredAddress registeredAddress { get; set; }
        public MailingAddress mailingAddress { get; set; }
        public List<Telephone> telephone { get; set; }
        public string domain { get; set; }
        public List<RegistrationNumber> registrationNumbers { get; set; }
        public List<NumberOfEmployee> numberOfEmployees { get; set; }
        public BusinessEntityType businessEntityType { get; set; }
        public string tickerSymbol { get; set; }
        public List<Financial> financials { get; set; }
        public CorporateLinkage corporateLinkage { get; set; }
        public List<PrimaryIndustryCode> primaryIndustryCodes { get; set; }
    }

    [Serializable]
    public class SearchCandidate
    {
        public int displaySequence { get; set; }
        public Organization organization { get; set; }
    }

    public class Links
    {
        public string prev { get; set; }
        public string next { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Errors
    {
        public string _param { get; set; }
    }

    public class ErrorDetail
    {
        public string parameter { get; set; }
        public Errors errors { get; set; }
    }

    public class Error
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public List<ErrorDetail> errorDetails { get; set; }
    }

    public class SearchCriteriaResponse
    {
        public TransactionDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public int candidatesMatchedQuantity { get; set; }
        public int candidatesReturnedQuantity { get; set; }
        public List<SearchCandidate> searchCandidates { get; set; }
        public Links links { get; set; }

        public Error error { get; set; }
    }

    public static class CommonGenerator
    {
        public static string FormattedAddress(PrimaryAddress address)
        {
            StringBuilder sb = new StringBuilder();

            if (address == null)
            {
                return string.Empty;
            }
            else
            {
                sb.Append("Hello");
            }

            return sb.ToString();
        }
    }

}