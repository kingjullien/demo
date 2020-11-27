using System;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ResearchInvestigationResponseEntity
    {
        public TransactionsDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public int researchRequestID { get; set; }
        public List<CaseDetail> caseDetails { get; set; }
        public string responseJSON { get; set; }
        public string requestJSON { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string errorMessage { get; set; }
        public string errorCode { get; set; }
        public List<ErrorDetails> errorDetails { get; set; }
    }

    public class ErrorDetails
    {
        public string parameter { get; set; }
        public string description { get; set; }
    }

    public class ResearchInvestigationTargetedResponseEntity
    {
        public TransactionsDetail transactionDetail { get; set; }
        public InquiryDetail inquiryDetail { get; set; }
        public int researchRequestID { get; set; }
        public List<CaseDetail> caseDetails { get; set; }
        public string responseJSON { get; set; }
        public string requestJSON { get; set; }
    }
    public class TransactionsDetail
    {
        public string transactionID { get; set; }
        public string customerTransactionID { get; set; }
        public DateTime transactionTimestamp { get; set; }
        public string serviceVersion { get; set; }
    }

    public class InquiryDetail
    {
        public string customerReference { get; set; }
        public int researchRequestID { get; set; }
        public int caseID { get; set; }
        public int challengeReason { get; set; }
        public List<ResearchComment> researchComments { get; set; }
        public string researchRequestType { get; set; }
        public Organization organization { get; set; }
    }

    public class ResearchType
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class ResearchSubType
    {
        public string description { get; set; }
        public int dnbCode { get; set; }
    }

    public class SubjectResearchType
    {
        public ResearchType researchType { get; set; }
        public ResearchSubType researchSubType { get; set; }
    }

    public class CaseDetail
    {
        public int caseID { get; set; }
        public List<SubjectResearchType> subjectResearchTypes { get; set; }
    }

    public class AddressCountry
    {
        public string isoAlpha2Code { get; set; }
    }

    public class AddressRegion
    {
        public string name { get; set; }
    }

    public class AddressLocality
    {
        public string name { get; set; }
    }

    public class StreetAddress
    {
        public string line1 { get; set; }
    }

    public class PrimaryAddress
    {
        public AddressCountry addressCountry { get; set; }
        public AddressRegion addressRegion { get; set; }
        public AddressLocality addressLocality { get; set; }
        public string postalCode { get; set; }
        public StreetAddress streetAddress { get; set; }
    }

    public class Organization
    {
        public string primaryName { get; set; }
        public string countryISOAlpha2Code { get; set; }
        public PrimaryAddress primaryAddress { get; set; }
        public string duns { get; set; }
    }
}
