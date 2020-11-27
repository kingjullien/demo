using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    public class OISearchListEntity
    {
        public long SearchResultsId { get; set; }
        public int UserId { get; set; }
        public int ResultCount { get; set; }
        public DateTime? ResponseDateTime { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public DateTime? RequestDateTime { get; set; }

    }
}
