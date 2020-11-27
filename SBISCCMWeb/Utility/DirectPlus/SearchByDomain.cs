using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility.SearchByDomain
{
    public class TransactionDetail
    {
        public string transactionID { get; set; }
        public string transactionTimestamp { get; set; }
        public string inLanguage { get; set; }
        public string productID { get; set; }
        public string productVersion { get; set; }
    }

    public class InquiryDetail
    {
        public string domain { get; set; }
    }

    public class InquiryMatch
    {
        public string countryISOAlpha2Code { get; set; }
        public string ipDomainName { get; set; }
        public string duns { get; set; }
        public string url { get; set; }
    }

    public class AddressCountry
    {
        public string name { get; set; }
        public string isoAlpha2Code { get; set; }
    }

    public class StreetAddress
    {
        public string line1 { get; set; }
        public object line2 { get; set; }
    }

    public class PrimaryAddress
    {
        public AddressCountry addressCountry { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public StreetAddress streetAddress { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class MailingAddress
    {
        public AddressCountry addressCountry { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string postalCode { get; set; }
        public StreetAddress streetAddress { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class NumberOfEmployees
    {
        public int value { get; set; }
    }

    public class YearlyRevenue
    {
        public double value { get; set; }
    }

    public class Telephone
    {
        public string number { get; set; }
        public string isdCode { get; set; }
    }

    public class PrimaryIndustryCode
    {
        public string usSicV4 { get; set; }
        public string usSicV4Description { get; set; }
    }

    public class Organization
    {
        public string duns { get; set; }
        public string name { get; set; }
        public PrimaryAddress primaryAddress { get; set; }
        public MailingAddress mailingAddress { get; set; }
        public object isFortune1000Listed { get; set; }
        public object isForbesLargestPrivateCompaniesListed { get; set; }
        public NumberOfEmployees numberOfEmployees { get; set; }
        public YearlyRevenue yearlyRevenue { get; set; }
        public Telephone telephone { get; set; }
        public object tickerSymbol { get; set; }
        public PrimaryIndustryCode primaryIndustryCode { get; set; }
    }
    public class Error
    {
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
        public string errorInformationURL { get; set; }
    }
    public class Result
    {
        public string actionStatus { get; set; }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
    public class SearchByDomainResponse
    {
        public Result result { get; set; }
        public Error error { get; set; }
        public TransactionDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public InquiryMatch inquiryMatch { get; set; }
        public Organization organization { get; set; }
    }
}