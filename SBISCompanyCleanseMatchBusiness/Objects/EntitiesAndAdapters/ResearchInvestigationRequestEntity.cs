using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ResearchInvestigationRequestEntity
    {
        public string customerTransactionID { get; set; }
        public string customerReference { get; set; }
        public List<RequestorEmail> requestorEmails { get; set; }
        public string researchRequestType { get; set; }
        public List<object> researchSubTypes { get; set; }
        public Organizations organization { get; set; }
        public List<ResearchComment> researchComments { get; set; }
    }
    public class ResearchInvestigationTargetedRequestEntity
    {
        public string customerTransactionID { get; set; }
        public string customerReference { get; set; }
        public List<RequestorEmail> requestorEmails { get; set; }
        public string researchRequestType { get; set; }
        public int[] researchSubTypes { get; set; }
        public Organizations organization { get; set; }
        public List<ResearchComment> researchComments { get; set; }
    }
    public class RequestorEmail
    {
        public string email { get; set; }
        public int roleType { get; set; }
    }
    public class Organizations
    {
        public string primaryName { get; set; }
        public string countryISOAlpha2Code { get; set; }
        public PrimaryAddress primaryAddress { get; set; }
        public string duns { get; set; }
        public string tradeStyleName { get; set; }
        public bool? isActiveBusiness { get; set; }
        public string[] duplicateDUNS { get; set; }
    }
    public class ResearchComment
    {
        public int typeDnBCode { get; set; }
        public string comment { get; set; }
    }
    public class ResearchInvestigationChallengeRequestEntity
    {
        public string customerTransactionID { get; set; }
        public string customerReference { get; set; }
        public int challengeReason { get; set; }
        public List<ResearchComment> researchComments { get; set; }
    }
}
