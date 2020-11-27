using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class TagsEntity
    {
        public int TagId { get; set; }
        public string TagValue { get; set; }
        public string TagName { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int CreatedUserId { get; set; }
        public string Tag { get; set; }
        public string TagTypeCode { get; set; }
        public string TagDescription { get; set; }
        public string LOBTag { get; set; }
    }


}
