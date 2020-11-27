using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class TagsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<TagsEntity> Adapt(DataTable dt)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                TagsEntity cust = new TagsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public TagsEntity AdaptItem(DataRow rw)
        {
            TagsEntity result = new TagsEntity();
            if (rw.Table.Columns["TagId"] != null)
            {
                result.TagId = SafeHelper.GetSafeint(rw["TagId"]);
            }

            if (rw.Table.Columns["TagValue"] != null)
            {
                result.TagValue = SafeHelper.GetSafestring(rw["TagValue"]);
            }

            if (rw.Table.Columns["TagName"] != null)
            {
                result.TagName = SafeHelper.GetSafestring(rw["TagName"]);
            }

            if (rw.Table.Columns["CreatedDateTime"] != null)
            {
                result.CreatedDateTime = SafeHelper.GetSafeDateTime(rw["CreatedDateTime"]);
            }

            if (rw.Table.Columns["CreatedUserId"] != null)
            {
                result.CreatedUserId = SafeHelper.GetSafeint(rw["CreatedUserId"]);
            }

            if (rw.Table.Columns["Tag"] != null)
            {
                result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            }

            if (rw.Table.Columns["TagTypeCode"] != null)
            {
                result.TagTypeCode = SafeHelper.GetSafestring(rw["TagTypeCode"]);
            }

            if (rw.Table.Columns["TagDescription"] != null)
            {
                result.TagDescription = SafeHelper.GetSafestring(rw["TagDescription"]);
            }

            if (rw.Table.Columns["LOBTag"] != null)
            {
                result.LOBTag = SafeHelper.GetSafestring(rw["LOBTag"]);
            }

            return result;
        }
    }
}
