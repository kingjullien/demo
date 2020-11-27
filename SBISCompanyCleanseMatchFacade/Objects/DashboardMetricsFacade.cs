using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{

    public class DashboardMetricsFacade : FacadeParent
    {
        DashboardMetricsBusiness rep;
        public DashboardMetricsFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new DashboardMetricsBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new DashboardMetricsBusiness(Connection);
            }
        }
        public DataTable GetOperationsMonitoringMetrics()
        {
            return rep.GetOperationsMonitoringMetrics();
        }
    }

}
