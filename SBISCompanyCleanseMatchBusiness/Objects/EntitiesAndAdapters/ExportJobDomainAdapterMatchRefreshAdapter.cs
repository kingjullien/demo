using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects
{
    class ExportJobDomainAdapterMatchRefreshAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ExportJobDomainEntity> AdaptLists(DataTable dt)
        {
            List<ExportJobDomainEntity> results = new List<ExportJobDomainEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ExportJobDomainEntity clientlist = new ExportJobDomainEntity();
                clientlist = AdaptItemReport(rw, dt);
                results.Add(clientlist);
            }
            return results;
        }
        public ExportJobDomainEntity AdaptItemReport(DataRow rw, DataTable dt)
        {
            ExportJobDomainEntity results = new ExportJobDomainEntity();
            results.ApplicationSubDomain = SafeHelper.GetSafestring(rw["AppicationSubDomain"]);
            results.MonitoringNotificationsInProgress = SafeHelper.GetSafestring(rw["MonitoringNotificationsInProgress"]);
            results.MonitoringNotificationsReady = SafeHelper.GetSafestring(rw["MonitoringNotificationsReady"]);
            results.MonitoringNotificationsFailed = SafeHelper.GetSafestring(rw["MonitoringNotificationsFailed"]);
            return results;
        }
    }
}
