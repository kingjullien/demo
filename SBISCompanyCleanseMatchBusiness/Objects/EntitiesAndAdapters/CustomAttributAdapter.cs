using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CustomAttributAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CustomAttributeEntity> Adapt(DataTable dt)
        {
            List<CustomAttributeEntity> results = new List<CustomAttributeEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CustomAttributeEntity customAttribute = new CustomAttributeEntity();
                customAttribute = AdaptItem(rw, dt);
                results.Add(customAttribute);
            }
            return results;
        }

        public CustomAttributeEntity AdaptItem(DataRow rw, DataTable dt)
        {
            CustomAttributeEntity result = new CustomAttributeEntity();
            if (dt.Columns.Contains("AttributeId"))
            {
                result.AttributeId = SafeHelper.GetSafeint(rw["AttributeId"]);
            }

            if (dt.Columns.Contains("AttributeName"))
            {
                result.AttributeName = SafeHelper.GetSafestring(rw["AttributeName"]);
            }

            if (dt.Columns.Contains("AttributeDataTypeCode"))
            {
                result.AttributeDataTypeCode = SafeHelper.GetSafestring(rw["AttributeDataTypeCode"]);
            }

            if (dt.Columns.Contains("TypeDescription"))
            {
                result.TypeDescription = SafeHelper.GetSafestring(rw["TypeDescription"]);
            }

            return result;
        }
        public List<AttributeTypes> AdaptAttributeTyeps(DataTable dt)
        {
            List<AttributeTypes> results = new List<AttributeTypes>();
            foreach (DataRow item in dt.Rows)
            {
                AttributeTypes matchCode = new AttributeTypes()
                {
                    Code = SafeHelper.GetSafestring(item["Code"]),
                    Value = SafeHelper.GetSafestring(item["Description"]),
                };
                results.Add(matchCode);
            }
            return results;
        }

    }
}
