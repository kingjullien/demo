using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DashboardBackgroundProcessAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DashboardBackgroundProcessEntity> Adapt(DataTable dt)
        {
            List<DashboardBackgroundProcessEntity> results = new List<DashboardBackgroundProcessEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardBackgroundProcessEntity matchCode = new DashboardBackgroundProcessEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public DashboardBackgroundProcessEntity AdaptItem(DataRow rw)
        {
            DashboardBackgroundProcessEntity result = new DashboardBackgroundProcessEntity();
            if (rw.Table.Columns["ExecutionId"] != null)
            {
                result.ExecutionId = SafeHelper.GetSafeint(rw["ExecutionId"]);
            }

            if (rw.Table.Columns["ProcessStartDateTime"] != null)
            {
                result.AuditDateTime = SafeHelper.GetSafeDateTime(rw["ProcessStartDateTime"]);
            }

            if (rw.Table.Columns["RunDuration_ms"] != null)
            {
                result.Duration_ms = SafeHelper.GetSafeint(rw["RunDuration_ms"]);
            }

            if (rw.Table.Columns["Status"] != null)
            {
                result.ProcessStatus = SafeHelper.GetSafestring(rw["Status"]);
            }

            if (rw.Table.Columns["Message"] != null)
            {
                result.Message = SafeHelper.GetSafestring(rw["Message"]);
            }

            if (rw.Table.Columns["ETLType"] != null)
            {
                result.ETLType = SafeHelper.GetSafestring(rw["ETLType"]);
            }

            if (rw.Table.Columns["NextRunTime_Seconds"] != null)
            {
                result.NextRunTime_Seconds = SafeHelper.GetSafeint(rw["NextRunTime_Seconds"]);
            }

            if (rw.Table.Columns["RunDuration"] != null)
            {
                result.Duration = SafeHelper.GetSafestring(rw["RunDuration"]);
            }

            return result;
        }
    }
    public class BackgroundProcessExecutionDetailAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<BackgroundProcessExecutionDetailEntity> Adapt(DataTable dt)
        {
            List<BackgroundProcessExecutionDetailEntity> results = new List<BackgroundProcessExecutionDetailEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                BackgroundProcessExecutionDetailEntity matchCode = new BackgroundProcessExecutionDetailEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public BackgroundProcessExecutionDetailEntity AdaptItem(DataRow rw)
        {
            BackgroundProcessExecutionDetailEntity result = new BackgroundProcessExecutionDetailEntity();
            if (rw.Table.Columns["AuditId"] != null)
            {
                result.AuditId = SafeHelper.GetSafeint(rw["AuditId"]);
            }

            if (rw.Table.Columns["ETLType"] != null)
            {
                result.ETLType = SafeHelper.GetSafestring(rw["ETLType"]);
            }

            if (rw.Table.Columns["Message"] != null)
            {
                result.Message = SafeHelper.GetSafestring(rw["Message"]);
            }

            if (rw.Table.Columns["AuditDateTime"] != null)
            {
                result.AuditDateTime = SafeHelper.GetSafeDateTime(rw["AuditDateTime"]);
            }

            if (rw.Table.Columns["ProcessComplete"] != null)
            {
                result.ProcessComplete = SafeHelper.GetSafestring(rw["ProcessComplete"]);
            }

            return result;
        }
    }

    #region "Dashboard V2"
    public class DashboardBackgroundProcessStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DashboardBackgroundProcessStatsEntity> Adapt(DataTable dt)
        {
            List<DashboardBackgroundProcessStatsEntity> results = new List<DashboardBackgroundProcessStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardBackgroundProcessStatsEntity matchCode = new DashboardBackgroundProcessStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public DashboardBackgroundProcessStatsEntity AdaptItem(DataRow rw)
        {
            DashboardBackgroundProcessStatsEntity result = new DashboardBackgroundProcessStatsEntity();
            if (rw.Table.Columns["ETLType"] != null)
            {
                result.ETLType = SafeHelper.GetSafestring(rw["ETLType"]);
            }

            if (rw.Table.Columns["NbrSuccess"] != null)
            {
                result.NbrSuccess = SafeHelper.GetSafeint(rw["NbrSuccess"]);
            }

            if (rw.Table.Columns["NbrFailures"] != null)
            {
                result.NbrFailures = SafeHelper.GetSafeint(rw["NbrFailures"]);
            }

            if (rw.Table.Columns["NbrRunning"] != null)
            {
                result.NbrRunning = SafeHelper.GetSafeint(rw["NbrRunning"]);
            }

            if (rw.Table.Columns["LastExecutionMinutes"] != null)
            {
                result.LastExecutionMinutes = SafeHelper.GetSafeint(rw["LastExecutionMinutes"]);
            }

            if (rw.Table.Columns["ProcessPaused"] != null)
            {
                result.ProcessPaused = SafeHelper.GetSafebool(rw["ProcessPaused"]);
            }

            if (rw.Table.Columns["Message1"] != null)
            {
                result.Message1 = SafeHelper.GetSafestring(rw["Message1"]);
            }

            if (rw.Table.Columns["Message2"] != null)
            {
                result.Message2 = SafeHelper.GetSafestring(rw["Message2"]);
            }

            if (rw.Table.Columns["NextExecutionTimeSeconds"] != null)
            {
                result.NextExecutionTimeSeconds = SafeHelper.GetSafeint(rw["NextExecutionTimeSeconds"]);
            }

            if (rw.Table.Columns["AverageRunDuration"] != null)
            {
                result.AverageRunDuration = SafeHelper.GetSafeDateTime(rw["AverageRunDuration"]);
            }

            if (rw.Table.Columns["MaxRunDuration"] != null)
            {
                result.MaxRunDuration = SafeHelper.GetSafeDateTime(rw["MaxRunDuration"]);
            }

            if (rw.Table.Columns["MinRunDuration"] != null)
            {
                result.MinRunDuration = SafeHelper.GetSafeDateTime(rw["MinRunDuration"]);
            }

            result.NextExecutionTimeSpan = result.NextExecutionTimeSeconds > 0 ? TimeSpan.FromSeconds(result.NextExecutionTimeSeconds) : TimeSpan.FromSeconds(0);

            return result;
        }
    }

    public class PrimaryStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<PrimaryStatsEntity> Adapt(DataTable dt)
        {
            List<PrimaryStatsEntity> results = new List<PrimaryStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                PrimaryStatsEntity matchCode = new PrimaryStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public PrimaryStatsEntity AdaptItem(DataRow rw)
        {
            PrimaryStatsEntity result = new PrimaryStatsEntity();
            if (rw.Table.Columns["FilesAwaitingImportCount"] != null)
            {
                result.FilesAwaitingImportCount = SafeHelper.GetSafeint(rw["FilesAwaitingImportCount"]);
            }

            if (rw.Table.Columns["InputRecordCount_Total"] != null)
            {
                result.InputRecordCount_Total = SafeHelper.GetSafeint(rw["InputRecordCount_Total"]);
            }

            if (rw.Table.Columns["InputRecordCount_AwaitingProcessing"] != null)
            {
                result.InputRecordCount_AwaitingProcessing = SafeHelper.GetSafeint(rw["InputRecordCount_AwaitingProcessing"]);
            }

            if (rw.Table.Columns["InputRecordCount_Processing"] != null)
            {
                result.InputRecordCount_Processing = SafeHelper.GetSafeint(rw["InputRecordCount_Processing"]);
            }

            if (rw.Table.Columns["InputRecordCount_Failed"] != null)
            {
                result.InputRecordCount_Failed = SafeHelper.GetSafeint(rw["InputRecordCount_Failed"]);
            }

            if (rw.Table.Columns["InputRecordFileCount"] != null)
            {
                result.InputRecordFileCount = SafeHelper.GetSafeint(rw["InputRecordFileCount"]);
            }

            if (rw.Table.Columns["LowConfidenceMatchRecordCount"] != null)
            {
                result.LowConfidenceMatchRecordCount = SafeHelper.GetSafeint(rw["LowConfidenceMatchRecordCount"]);
            }

            if (rw.Table.Columns["NoMatchRecordCount"] != null)
            {
                result.NoMatchRecordCount = SafeHelper.GetSafeint(rw["NoMatchRecordCount"]);
            }

            if (rw.Table.Columns["MatchProcessingRecordCount"] != null)
            {
                result.MatchProcessingRecordCount = SafeHelper.GetSafeint(rw["MatchProcessingRecordCount"]);
            }

            if (rw.Table.Columns["EnrichmentProcessingCount"] != null)
            {
                result.EnrichmentProcessingCount = SafeHelper.GetSafeint(rw["EnrichmentProcessingCount"]);
            }

            if (rw.Table.Columns["EnrichmentProcessingDUNSCount"] != null)
            {
                result.EnrichmentProcessingDUNSCount = SafeHelper.GetSafeint(rw["EnrichmentProcessingDUNSCount"]);
            }

            if (rw.Table.Columns["MatchExportRecordCount"] != null)
            {
                result.MatchExportRecordCount = SafeHelper.GetSafeint(rw["MatchExportRecordCount"]);
            }

            if (rw.Table.Columns["EnrichmentExportDUNSCount"] != null)
            {
                result.EnrichmentExportDUNSCount = SafeHelper.GetSafeint(rw["EnrichmentExportDUNSCount"]);
            }

            if (rw.Table.Columns["MonitoringExportDUNSCount"] != null)
            {
                result.MonitoringExportDUNSCount = SafeHelper.GetSafeint(rw["MonitoringExportDUNSCount"]);
            }

            if (rw.Table.Columns["LatestMonitoringChangeDateTime"] != null)
            {
                result.LatestMonitoringChangeDateTime = SafeHelper.GetSafeDateTime(rw["LatestMonitoringChangeDateTime"]);
            }

            if (rw.Table.Columns["QueueStatus"] != null)
            {
                result.QueueStatus = SafeHelper.GetSafestring(rw["QueueStatus"]);
            }

            return result;
        }
    }
    public class LowConfidenceMatchStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<LowConfidenceMatchStatsEntity> Adapt(DataTable dt)
        {
            List<LowConfidenceMatchStatsEntity> results = new List<LowConfidenceMatchStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                LowConfidenceMatchStatsEntity matchCode = new LowConfidenceMatchStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public LowConfidenceMatchStatsEntity AdaptItem(DataRow rw)
        {
            LowConfidenceMatchStatsEntity result = new LowConfidenceMatchStatsEntity();
            if (rw.Table.Columns["DnBConfidenceCode"] != null)
            {
                result.DnBConfidenceCode = SafeHelper.GetSafeint(rw["DnBConfidenceCode"]);
            }

            if (rw.Table.Columns["NbrRecords"] != null)
            {
                result.NbrRecords = SafeHelper.GetSafeint(rw["NbrRecords"]);
            }

            return result;
        }
    }
    public class NoMatchStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<NoMatchStatsEntity> Adapt(DataTable dt)
        {
            List<NoMatchStatsEntity> results = new List<NoMatchStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                NoMatchStatsEntity matchCode = new NoMatchStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public NoMatchStatsEntity AdaptItem(DataRow rw)
        {
            NoMatchStatsEntity result = new NoMatchStatsEntity();
            if (rw.Table.Columns["ErrorDescription"] != null)
            {
                result.ErrorDescription = SafeHelper.GetSafestring(rw["ErrorDescription"]);
            }

            if (rw.Table.Columns["NbrRecords"] != null)
            {
                result.NbrRecords = SafeHelper.GetSafeint(rw["NbrRecords"]);
            }

            return result;
        }
    }
    public class MatchOutputStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MatchOutputStatsEntity> Adapt(DataTable dt)
        {
            List<MatchOutputStatsEntity> results = new List<MatchOutputStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MatchOutputStatsEntity matchCode = new MatchOutputStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public MatchOutputStatsEntity AdaptItem(DataRow rw)
        {
            MatchOutputStatsEntity result = new MatchOutputStatsEntity();
            if (rw.Table.Columns["MatchType"] != null)
            {
                result.MatchType = SafeHelper.GetSafestring(rw["MatchType"]);
            }

            if (rw.Table.Columns["AcceptanceType"] != null)
            {
                result.AcceptanceType = SafeHelper.GetSafestring(rw["AcceptanceType"]);
            }

            if (rw.Table.Columns["NbrRecords"] != null)
            {
                result.NbrRecords = SafeHelper.GetSafeint(rw["NbrRecords"]);
            }

            return result;
        }
    }
    public class EnrichmentProcessingQueueStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<EnrichmentProcessingQueueStatsEntity> Adapt(DataTable dt)
        {
            List<EnrichmentProcessingQueueStatsEntity> results = new List<EnrichmentProcessingQueueStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                EnrichmentProcessingQueueStatsEntity matchCode = new EnrichmentProcessingQueueStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public EnrichmentProcessingQueueStatsEntity AdaptItem(DataRow rw)
        {
            EnrichmentProcessingQueueStatsEntity result = new EnrichmentProcessingQueueStatsEntity();
            if (rw.Table.Columns["DnBAPIName"] != null)
            {
                result.DnBAPIName = SafeHelper.GetSafestring(rw["DnBAPIName"]);
            }

            if (rw.Table.Columns["NbrDUNS"] != null)
            {
                result.NbrDUNS = SafeHelper.GetSafeint(rw["NbrDUNS"]);
            }

            return result;
        }
    }
    public class DataEnrichmentStatsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DataEnrichmentStatsEntity> Adapt(DataTable dt)
        {
            List<DataEnrichmentStatsEntity> results = new List<DataEnrichmentStatsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DataEnrichmentStatsEntity matchCode = new DataEnrichmentStatsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public DataEnrichmentStatsEntity AdaptItem(DataRow rw)
        {
            DataEnrichmentStatsEntity result = new DataEnrichmentStatsEntity();
            if (rw.Table.Columns["DnBAPIName"] != null)
            {
                result.DnBAPIName = SafeHelper.GetSafestring(rw["DnBAPIName"]);
            }

            if (rw.Table.Columns["NbrDUNS"] != null)
            {
                result.NbrDUNS = SafeHelper.GetSafeint(rw["NbrDUNS"]);
            }

            return result;
        }
    }
    public class ImportProcessTrendAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportProcessTrendEntity> Adapt(DataTable dt)
        {
            List<ImportProcessTrendEntity> results = new List<ImportProcessTrendEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportProcessTrendEntity matchCode = new ImportProcessTrendEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public ImportProcessTrendEntity AdaptItem(DataRow rw)
        {
            ImportProcessTrendEntity result = new ImportProcessTrendEntity();
            if (rw.Table.Columns["ImportedDate"] != null)
            {
                result.ImportedDate = SafeHelper.GetSafeDateTime(rw["ImportedDate"]);
            }

            if (rw.Table.Columns["ImportedRecordCount"] != null)
            {
                result.ImportedRecordCount = SafeHelper.GetSafeint(rw["ImportedRecordCount"]);
            }

            if (rw.Table.Columns["EpochDate"] != null)
            {
                result.EpochDate = SafeHelper.GetSafedouble(rw["EpochDate"]) * 1000;
            }

            return result;
        }
    }

    public class DashboardImportProcessDataQueueStatisticsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DashboardImportProcessDataQueueStatisticsEntity> Adapt(DataTable dt)
        {
            List<DashboardImportProcessDataQueueStatisticsEntity> results = new List<DashboardImportProcessDataQueueStatisticsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardImportProcessDataQueueStatisticsEntity matchCode = new DashboardImportProcessDataQueueStatisticsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }

        public DashboardImportProcessDataQueueStatisticsEntity AdaptItem(DataRow rw)
        {
            DashboardImportProcessDataQueueStatisticsEntity result = new DashboardImportProcessDataQueueStatisticsEntity();
            if (rw.Table.Columns["ImportProcessId"] != null)
            {
                result.ImportProcessId = SafeHelper.GetSafeint(rw["ImportProcessId"]);
            }

            if (rw.Table.Columns["ImportDate"] != null)
            {
                result.ImportDate = SafeHelper.GetSafeDateTime(rw["ImportDate"]);
            }

            if (rw.Table.Columns["ImportProcess"] != null)
            {
                result.ImportProcess = SafeHelper.GetSafestring(rw["ImportProcess"]);
            }

            if (rw.Table.Columns["ImportedRowCount"] != null)
            {
                result.ImportedRowCount = SafeHelper.GetSafeint(rw["ImportedRowCount"]);
            }

            if (rw.Table.Columns["InputRecordCount"] != null)
            {
                result.InputRecordCount = SafeHelper.GetSafeint(rw["InputRecordCount"]);
            }

            if (rw.Table.Columns["DS_LowConfidenceMatchRecordCount"] != null)
            {
                result.DS_LowConfidenceMatchRecordCount = SafeHelper.GetSafeint(rw["DS_LowConfidenceMatchRecordCount"]);
            }

            if (rw.Table.Columns["DS_NoMatchRecordCount"] != null)
            {
                result.DS_NoMatchRecordCount = SafeHelper.GetSafeint(rw["DS_NoMatchRecordCount"]);
            }

            if (rw.Table.Columns["MatchProcessingRecordCount"] != null)
            {
                result.MatchProcessingRecordCount = SafeHelper.GetSafeint(rw["MatchProcessingRecordCount"]);
            }

            if (rw.Table.Columns["EnrichmentProcessingDUNSCount"] != null)
            {
                result.EnrichmentProcessingDUNSCount = SafeHelper.GetSafeint(rw["EnrichmentProcessingDUNSCount"]);
            }

            if (rw.Table.Columns["MatchExportRecordCount"] != null)
            {
                result.MatchExportRecordCount = SafeHelper.GetSafeint(rw["MatchExportRecordCount"]);
            }

            if (rw.Table.Columns["EnrichmentExportDUNSCount"] != null)
            {
                result.EnrichmentExportDUNSCount = SafeHelper.GetSafeint(rw["EnrichmentExportDUNSCount"]);
            }

            if (rw.Table.Columns["ArchivalQueueCount"] != null)
            {
                result.ArchivalQueueCount = SafeHelper.GetSafeint(rw["ArchivalQueueCount"]);
            }

            if (rw.Table.Columns["DS_RecordsUnderInvestigationCount"] != null)
            {
                result.DS_RecordsUnderInvestigationCount = SafeHelper.GetSafeint(rw["DS_RecordsUnderInvestigationCount"]);
            }

            if (rw.Table.Columns["Tag"] != null)
            {
                result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            }

            return result;
        }
    }

    public class DashboardInvestigationStatisticsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<DashboardInvestigationStatisticsEntity> Adapt(DataTable dt)
        {
            List<DashboardInvestigationStatisticsEntity> results = new List<DashboardInvestigationStatisticsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardInvestigationStatisticsEntity matchCode = new DashboardInvestigationStatisticsEntity();
                matchCode = AdaptItem(rw);
                results.Add(matchCode);
            }
            return results;
        }
        public DashboardInvestigationStatisticsEntity AdaptItem(DataRow rw)
        {
            DashboardInvestigationStatisticsEntity result = new DashboardInvestigationStatisticsEntity();
            if (rw.Table.Columns["RequestType"] != null)
            {
                result.RequestType = SafeHelper.GetSafestring(rw["RequestType"]);
            }

            if (rw.Table.Columns["NbrOpenedCases"] != null)
            {
                result.NbrOpenedCases = SafeHelper.GetSafeint(rw["NbrOpenedCases"]);
            }

            if (rw.Table.Columns["NbrClosedCases"] != null)
            {
                result.NbrClosedCases = SafeHelper.GetSafeint(rw["NbrClosedCases"]);
            }

            if (rw.Table.Columns["NbrCaseOpenedLastWeek"] != null)
            {
                result.NbrCaseOpenedLastWeek = SafeHelper.GetSafeint(rw["NbrCaseOpenedLastWeek"]);
            }

            if (rw.Table.Columns["AverageResolutionTime_Minutes"] != null)
            {
                result.AverageResolutionTime_Minutes = SafeHelper.GetSafeint(rw["AverageResolutionTime_Minutes"]);
            }

            return result;
        }
    }
    #endregion
}
