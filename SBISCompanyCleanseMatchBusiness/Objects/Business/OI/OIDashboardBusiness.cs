using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class OIDashboardBusiness : BusinessParent
    {
        OIDashboardRepository rep;
        public OIDashboardBusiness(string connectionString) : base(connectionString) { rep = new OIDashboardRepository(Connection); }
        public DataSet GetDashboardQueueCount()
        {
            return rep.GetDashboardQueueCount();
        }
        public DataSet GetDashboardBackgroundProcess()
        {
            return rep.GetDashboardBackgroundProcess();
        }
    }
}
