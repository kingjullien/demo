using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class APICallLogEntity
    {
        public string id { get; set; }

        public string ApiMethod { get; set; }
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public string Message { get; set; }
        public int HttpStatusCode { get; set; }
        public string InputPostBody { get; set; }
        public string subDomain { get; set; }
        public string Environment { get; set; }
        public double? DnbDuration { get; set; }
        public double? MatchbookAPIDuration { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    public class APICallLogMongoDBEntity
    {
        public Guid id { get; set; }
        public string Environment { get; set; }
        public string ApiMethod { get; set; }
        public string InputData { get; set; }
        public string OutputData { get; set; }
        public string Message { get; set; }
        public int HttpStatusCode { get; set; }
        public string InputPostBody { get; set; }
        public string SubDomain { get; set; }
        public double? EPDuration { get; set; }
        public double? MatchbookAPIDuration { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Provider { get; set; }
    }
}
