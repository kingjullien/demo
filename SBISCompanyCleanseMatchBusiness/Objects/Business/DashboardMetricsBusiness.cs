using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{

    public class DashboardMetricsBusiness : BusinessParent
    {
        DashboardMetricsRepository rep;
        public DashboardMetricsBusiness(string connectionString) : base(connectionString) { rep = new DashboardMetricsRepository(Connection); }
        public DataTable GetOperationsMonitoringMetrics()
        {
            return rep.GetOperationsMonitoringMetrics();
        }
    }
}
