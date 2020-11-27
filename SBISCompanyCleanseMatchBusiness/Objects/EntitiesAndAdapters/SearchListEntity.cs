using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class SearchListEntity
    {
        public long SearchResultsId { get; set; }
        public int UserId { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public DateTime? RequestDateTime { get; set; }
        public DateTime? ResponseDateTime { get; set; }
    }
}
