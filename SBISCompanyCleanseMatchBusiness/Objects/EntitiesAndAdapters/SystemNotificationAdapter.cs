using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class SystemNotificationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<SystemNotificationEntity> Adapt(DataTable dt)
        {
            List<SystemNotificationEntity> results = new List<SystemNotificationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                SystemNotificationEntity productDataEntity = new SystemNotificationEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public SystemNotificationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            SystemNotificationEntity result = new SystemNotificationEntity();
            if (dt.Columns.Contains("MessageId"))
            {
                result.MessageId = SafeHelper.GetSafeint(rw["MessageId"]);
            }

            if (dt.Columns.Contains("Message"))
            {
                result.Message = SafeHelper.GetSafestring(rw["Message"]);
            }

            if (dt.Columns.Contains("StartDateTime"))
            {
                result.StartDateTime = SafeHelper.GetSafeDateTime(rw["StartDateTime"]);
            }

            if (dt.Columns.Contains("EndDateTime"))
            {
                result.EndDateTime = SafeHelper.GetSafeDateTime(rw["EndDateTime"]);
            }

            if (dt.Columns.Contains("FontColor"))
            {
                result.FontColor = SafeHelper.GetSafestring(rw["FontColor"]);
            }

            return result;
        }


    }
}
