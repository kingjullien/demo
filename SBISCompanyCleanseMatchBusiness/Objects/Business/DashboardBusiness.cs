using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class DashboardBusiness : BusinessParent
    {
        DashboardRepository rep;
        public DashboardBusiness(string connectionString) : base(connectionString) { rep = new DashboardRepository(Connection); }
        public DashboardEntity GetActiveClient()
        {
            return rep.GetActiveClient();
        }
        public BackGroundProcessListViewModel DashboardGetBackgroundProcessList(bool ShowDetails)
        {
            return rep.DashboardGetBackgroundProcessList(ShowDetails);
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