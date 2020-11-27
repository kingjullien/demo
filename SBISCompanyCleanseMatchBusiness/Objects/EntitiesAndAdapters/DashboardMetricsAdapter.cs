using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class DashboardMetricsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DashboardMetricsEntity> Adapt(DataTable dt)
        {
            List<DashboardMetricsEntity> results = new List<DashboardMetricsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardMetricsEntity matchCode = new DashboardMetricsEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public DashboardMetricsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            DashboardMetricsEntity result = new DashboardMetricsEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("ApplicationId"))
            {
                result.ApplicationId = SafeHelper.GetSafeint(rw["ApplicationId"]);
            }

            if (dt.Columns.Contains("DatabaseName"))
            {
                result.DatabaseName = SafeHelper.GetSafestring(rw["DatabaseName"]);
            }

            if (dt.Columns.Contains("MetricCategory"))
            {
                result.MetricCategory = SafeHelper.GetSafestring(rw["MetricCategory"]);
            }

            if (dt.Columns.Contains("MetricName"))
            {
                result.MetricName = SafeHelper.GetSafestring(rw["MetricName"]);
            }

            if (dt.Columns.Contains("Metric"))
            {
                result.Metric = SafeHelper.GetSafeint(rw["Metric"]);
            }

            if (dt.Columns.Contains("MetricEndTime"))
            {
                result.MetricEndTime = SafeHelper.GetSafeDateTime(rw["MetricEndTime"]);
            }

            if (dt.Columns.Contains("IsAggregatable"))
            {
                result.IsAggregatable = SafeHelper.GetSafebool(rw["IsAggregatable"]);
            }

            return result;
        }
    }
}
