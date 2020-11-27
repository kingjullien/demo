using SBISCompanyCleanseMatchBusiness.Objects.Business;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OIDashboardFacade : FacadeParent
    {
        OIDashboardBusiness rep;
        public OIDashboardFacade(string connectionString) : base(connectionString) { rep = new OIDashboardBusiness(Connection); }
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
