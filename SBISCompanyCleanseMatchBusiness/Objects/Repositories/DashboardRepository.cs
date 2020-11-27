using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class DashboardRepository : RepositoryParent
    {
        public DashboardRepository(string connectionString) : base(connectionString) { }

        #region "Common Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        #endregion

        internal DashboardEntity GetActiveClient()
        {
            DashboardEntity dict = new DashboardEntity();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            List<ClientApplicationEntity> results = new List<ClientApplicationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetActiveClients";
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
                DataTable dt2 = ds.Tables[2];
                results = new ClientApplicationAadpter().AdaptLists(dt2);
                dict.TotalActiveClients = Convert.ToInt32(ds.Tables[0].Rows[0]["ActiveClient"]).ToString();
                dict.TotalInActiveClients = Convert.ToInt32(ds.Tables[1].Rows[0]["InActiveClient"]).ToString();
                dict.clientApplication = results;
                foreach (ClientApplicationEntity application in results)
                {
                    application.clientApplicationList = new DashboardRepository(Connection).GetClientsApplication(application.ClientId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dict;
        }

        internal List<ClientApplicationListEntity> GetClientsApplication(int ClientId)
        {
            List<ClientApplicationListEntity> results = new List<ClientApplicationListEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetClientsApplication";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@ClientId", ClientId.ToString(), SQLServerDatatype.IntDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ClientApplicationListAdapter().AdaptLists(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        internal BackGroundProcessListViewModel DashboardGetBackgroundProcessList(bool ShowDetails)
        {
            BackGroundProcessListViewModel processListViewModel = new BackGroundProcessListViewModel();
            List<DashboardBackgroundProcessEntity> results = new List<DashboardBackgroundProcessEntity>();
            List<DashboardBackgroundProcessStatsEntity> stats = new List<DashboardBackgroundProcessStatsEntity>();
            try
            {
                DataSet ds = new DataSet();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetBackgroundProcessStats";
                sproc.StoredProceduresParameter.Add(GetParam("@ShowDetails", ShowDetails.ToString(), SQLServerDatatype.BitDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    processListViewModel.StatsList = new List<DashboardBackgroundProcessStatsEntity>();
                    processListViewModel.StatsList = new DashboardBackgroundProcessStatsAdapter().Adapt(ds.Tables[0]);
                    if (ds.Tables.Count > 1)
                    {
                        processListViewModel.ProcessList = new List<DashboardBackgroundProcessEntity>();
                        processListViewModel.ProcessList = new DashboardBackgroundProcessAdapter().Adapt(ds.Tables[1]);
                    }
                }
                return processListViewModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal List<BackgroundProcessExecutionDetailEntity> DashboardGetBackgroundProcessExecutionDetail(int ExecutionId)
        {
            List<BackgroundProcessExecutionDetailEntity> results = new List<BackgroundProcessExecutionDetailEntity>();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardGetBackgroundProcessExecutionDetail";
                sproc.StoredProceduresParameter.Add(GetParam("@ExecutionId", ExecutionId.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new BackgroundProcessExecutionDetailAdapter().Adapt(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return results;
        }

        #region "Dashboard V2"
        internal DashboardViewModel DashboardV2GetDataQueueStatistics(int UserId)
        {
            DashboardViewModel dashboardDatadt = new DashboardViewModel();
            try
            {
                DataSet ds = new DataSet();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetDataQueueStatistics";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (ds != null)
                {
                    if (ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            dashboardDatadt.primaryStats = new PrimaryStatsAdapter().AdaptItem(dr);
                        }
                    }
                    if (ds.Tables.Count > 6 && ds.Tables[6] != null && ds.Tables[6].Rows.Count > 0)
                    {
                        dashboardDatadt.importProcessTrend = new ImportProcessTrendAdapter().Adapt(ds.Tables[6]);
                    }
                }
                return dashboardDatadt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal List<DashboardImportProcessDataQueueStatisticsEntity> DashboardV2GetDataQueueStatisticsByImportProcess(int ImportProcessId = 0)
        {
            List<DashboardImportProcessDataQueueStatisticsEntity> dashboardDatadt = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetDataQueueStatisticsByImportProcess";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    dashboardDatadt = new DashboardImportProcessDataQueueStatisticsAdapter().Adapt(dt);
                }
                return dashboardDatadt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<DashboardImportProcessDataQueueStatisticsEntity> DashboardV2GetDataQueueStatisticsByTag()
        {
            List<DashboardImportProcessDataQueueStatisticsEntity> dashboardDatadt = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetDataQueueStatisticsByTags";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    dashboardDatadt = new DashboardImportProcessDataQueueStatisticsAdapter().Adapt(dt);
                }
                return dashboardDatadt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<DashboardBackgroundProcessStatsEntity> DashboardV2GetBackgroundProcessStats(bool ShowDetails)
        {
            List<DashboardBackgroundProcessStatsEntity> result = new List<DashboardBackgroundProcessStatsEntity>();
            try
            {
                DataSet ds = new DataSet();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetBackgroundProcessStats";
                sproc.StoredProceduresParameter.Add(GetParam("@ShowDetails", ShowDetails.ToString(), SQLServerDatatype.BitDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
                if (ds != null && ds.Tables.Count > 0)
                {
                    result = new DashboardBackgroundProcessStatsAdapter().Adapt(ds.Tables[0]);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal DashboardInvestigationStatisticsEntity DashboardV2GetInvestigationStatistics()
        {
            DashboardInvestigationStatisticsEntity result = new DashboardInvestigationStatisticsEntity();
            try
            {
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DashboardV2GetInvestigationStatistics";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        result = new DashboardInvestigationStatisticsAdapter().AdaptItem(item);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
