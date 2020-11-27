using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIAPICallAuditEntity
    {
        public int Id { get; set; }
        public DateTime TransactionTimeStamp { get; set; }
        public string SubDomain { get; set; }
        public string CallType { get; set; }
        public string CallURL { get; set; }
        public int CandidateCount { get; set; }
        public int ResultCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
