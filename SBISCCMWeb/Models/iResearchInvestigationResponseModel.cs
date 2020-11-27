//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace SBISCCMWeb.Models
//{
//    public class iResearchInvestigationResponseModel
//    {
//        public TransactionsDetail transactionDetail { get; set; }
//        public InquiryDetail inquiryDetail { get; set; }
//        public int researchRequestID { get; set; }
//        public List<CaseDetail> caseDetails { get; set; }
//        public string responseJSON { get; set; }
//        public string requestJSON { get; set; }
//    }
//    public class TransactionsDetail
//    {
//        public string transactionID { get; set; }
//        public string customerTransactionID { get; set; }
//        public DateTime transactionTimestamp { get; set; }
//        public string serviceVersion { get; set; }
//    }

//    public class InquiryDetail
//    {
//        public string customerReference { get; set; }
//        public string researchRequestType { get; set; }
//        public Organization organization { get; set; }
//    }

//    public class ResearchType
//    {
//        public string description { get; set; }
//        public int dnbCode { get; set; }
//    }

//    public class ResearchSubType
//    {
//        public string description { get; set; }
//        public int dnbCode { get; set; }
//    }

//    public class SubjectResearchType
//    {
//        public ResearchType researchType { get; set; }
//        public ResearchSubType researchSubType { get; set; }
//    }

//    public class CaseDetail
//    {
//        public int caseID { get; set; }
//        public List<SubjectResearchType> subjectResearchTypes { get; set; }
//    }
//}