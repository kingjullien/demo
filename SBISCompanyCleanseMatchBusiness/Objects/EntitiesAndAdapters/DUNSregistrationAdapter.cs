using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class DUNSregistrationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<DUNSregistrationEntity> Adapt(DataTable dt)
        {
            List<DUNSregistrationEntity> results = new List<DUNSregistrationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DUNSregistrationEntity productDataEntity = new DUNSregistrationEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public DUNSregistrationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            DUNSregistrationEntity result = new DUNSregistrationEntity();
            result.MonitoringRegistrationId = SafeHelper.GetSafeint(rw["MonitoringRegistrationId"]);
            result.MonitoringProfileId = SafeHelper.GetSafeint(rw["MonitoringProfileId"]);
            result.NotificationProfileId = SafeHelper.GetSafeint(rw["NotificationProfileId"]);
            if (dt.Columns.Contains("TradeUpIndicator"))
            {
                result.TradeUpIndicator = SafeHelper.GetSafebool(rw["TradeUpIndicator"]);
            }

            if (dt.Columns.Contains("AutoRenewalIndicator"))
            {
                result.AutoRenewalIndicator = SafeHelper.GetSafebool(rw["AutoRenewalIndicator"]);
            }

            if (dt.Columns.Contains("SubjectCategory"))
            {
                result.SubjectCategory = SafeHelper.GetSafestring(rw["SubjectCategory"]);
            }

            if (dt.Columns.Contains("CustomerReferenceText"))
            {
                result.CustomerReferenceText = SafeHelper.GetSafestring(rw["CustomerReferenceText"]);
            }

            if (dt.Columns.Contains("BillingEndorsementText"))
            {
                result.BillingEndorsementText = SafeHelper.GetSafestring(rw["BillingEndorsementText"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("MonitoringProfileName"))
            {
                result.MonitoringProfileName = SafeHelper.GetSafestring(rw["MonitoringProfileName"]);
            }

            if (dt.Columns.Contains("NotificationProfileName"))
            {
                result.NotificationProfileName = SafeHelper.GetSafestring(rw["NotificationProfileName"]);
            }

            return result;
        }
    }
}
