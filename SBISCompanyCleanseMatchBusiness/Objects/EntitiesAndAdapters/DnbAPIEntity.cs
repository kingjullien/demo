using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DnbAPIEntity
    {
        public List<DnbAPIEntity> lstDnbAPIEntity = new List<DnbAPIEntity>();
        public int DnBAPIId { get; set; }
        public string DnBAPICode { get; set; }
        public string DnBAPIName { get; set; }
        public string APIName { get; set; }
        public string APIType { get; set; }
        public bool DUNSEnhancementAPI { get; set; }
        public string APIFamily { get; set; }
    }
}
