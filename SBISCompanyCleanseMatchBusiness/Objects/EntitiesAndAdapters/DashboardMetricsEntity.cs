using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DashboardMetricsEntity
    {

        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string DatabaseName { get; set; }
        public string MetricCategory { get; set; }
        public string MetricName { get; set; }
        public int Metric { get; set; }
        public DateTime MetricEndTime { get; set; }
        public bool IsAggregatable { get; set; }
    }
}
