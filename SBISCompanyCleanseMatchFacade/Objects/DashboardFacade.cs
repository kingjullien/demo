using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class DashboardFacade : FacadeParent
    {
        DashboardBusiness rep;
        public DashboardFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new DashboardBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new DashboardBusiness(Connection);
            }
        }
        public DashboardEntity GetActiveClient()
        {
            return rep.GetActiveClient();
        }
        public BackGroundProcessListViewModel DashboardGetBackgroundProcessList(bool ShoeDetails)
        {
            return rep.DashboardGetBackgroundProcessList(ShoeDetails);
        }
        public List<BackgroundProcessExecutionDetailEntity> DashboardGetBackgroundProcessExecutionDetail(int ExecutionId)
        {
            return rep.DashboardGetBackgroundProcessExecutionDetail(ExecutionId);
        }
        #region "Dashboard V2"
        public DashboardViewModel DashboardV2GetDataQueueStatistics(int UserId)
        {
            return rep.DashboardV2GetDataQueueStatistics(UserId);
        }
        public List<DashboardImportProcessDataQueueStatisticsEntity> DashboardV2GetDataQueueStatisticsByImportProcess(int ImportProcessId = 0)
        {
            return rep.DashboardV2GetDataQueueStatisticsByImportProcess(ImportProcessId);
        }

        public List<DashboardImportProcessDataQueueStatisticsEntity> DashboardV2GetDataQueueStatisticsByTag()
        {
            return rep.DashboardV2GetDataQueueStatisticsByTag();
        }
        public List<DashboardBackgroundProcessStatsEntity> DashboardV2GetBackgroundProcessStats(bool ShowDetails)
        {
            return rep.DashboardV2GetBackgroundProcessStats(ShowDetails);
        }
        public DashboardInvestigationStatisticsEntity DashboardV2GetInvestigationStatistics()
        {
            return rep.DashboardV2GetInvestigationStatistics();
        }
        #endregion
    }
}
