using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class NotificationProfileAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<NotificationProfileEntity> Adapt(DataTable dt)
        {
            List<NotificationProfileEntity> results = new List<NotificationProfileEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                NotificationProfileEntity productDataEntity = new NotificationProfileEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public NotificationProfileEntity AdaptItem(DataRow rw, DataTable dt)
        {
            NotificationProfileEntity result = new NotificationProfileEntity();

            if (dt.Columns.Contains("NotificationProfileName"))
            {
                result.NotificationProfileName = SafeHelper.GetSafestring(rw["NotificationProfileName"]);
            }

            if (dt.Columns.Contains("NotificationProfileDescription"))
            {
                result.NotificationProfileDescription = SafeHelper.GetSafestring(rw["NotificationProfileDescription"]);
            }

            if (dt.Columns.Contains("DeliveryMode"))
            {
                result.DeliveryMode = SafeHelper.GetSafestring(rw["DeliveryMode"]);
            }

            if (dt.Columns.Contains("DeliveryChannelUserPreferenceName"))
            {
                result.DeliveryChannelUserPreferenceName = SafeHelper.GetSafestring(rw["DeliveryChannelUserPreferenceName"]);
            }

            if (dt.Columns.Contains("DeliveryFrequency"))
            {
                result.DeliveryFrequency = SafeHelper.GetSafestring(rw["DeliveryFrequency"]);
            }

            if (dt.Columns.Contains("DeliveryFormat"))
            {
                result.DeliveryFormat = SafeHelper.GetSafestring(rw["DeliveryFormat"]);
            }

            if (dt.Columns.Contains("StopDeliveryIndicator"))
            {
                result.StopDeliveryIndicator = SafeHelper.GetSafebool(rw["StopDeliveryIndicator"]);
            }

            if (dt.Columns.Contains("CompressedProductIndicator"))
            {
                result.CompressedProductIndicator = SafeHelper.GetSafebool(rw["CompressedProductIndicator"]);
            }

            if (dt.Columns.Contains("InquiryReferenceText"))
            {
                result.InquiryReferenceText = SafeHelper.GetSafestring(rw["InquiryReferenceText"]);
            }

            if (dt.Columns.Contains("ResultID"))
            {
                result.ResultID = SafeHelper.GetSafestring(rw["ResultID"]);
            }

            if (dt.Columns.Contains("SeverityText"))
            {
                result.SeverityText = SafeHelper.GetSafestring(rw["SeverityText"]);
            }

            if (dt.Columns.Contains("ResultText"))
            {
                result.ResultText = SafeHelper.GetSafestring(rw["ResultText"]);
            }

            if (dt.Columns.Contains("NotificationProfileID"))
            {
                result.NotificationProfileID = SafeHelper.GetSafeint(rw["NotificationProfileID"]);
            }

            if (dt.Columns.Contains("RequestDateTime"))
            {
                result.RequestDateTime = SafeHelper.GetSafeDateTime(rw["RequestDateTime"]);
            }

            if (dt.Columns.Contains("ResponseDateTime"))
            {
                result.ResponseDateTime = SafeHelper.GetSafeDateTime(rw["ResponseDateTime"]);
            }

            if (dt.Columns.Contains("CreatedBy"))
            {
                result.CreatedBy = SafeHelper.GetSafeint(rw["CreatedBy"]);
            }

            if (dt.Columns.Contains("ModifiedBy"))
            {
                result.ModifiedBy = SafeHelper.GetSafeint(rw["ModifiedBy"]);
            }

            if (dt.Columns.Contains("CreatedDateTime"))
            {
                result.CreatedDateTime = SafeHelper.GetSafeDateTime(rw["CreatedDateTime"]);
            }

            if (dt.Columns.Contains("ModifiedDateTime"))
            {
                result.ModifiedDateTime = SafeHelper.GetSafeDateTime(rw["ModifiedDateTime"]);
            }

            if (dt.Columns.Contains("IsDeleted"))
            {
                result.IsDeleted = SafeHelper.GetSafebool(rw["IsDeleted"]);
            }

            return result;
        }


    }
}
