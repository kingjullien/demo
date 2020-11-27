using System;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DashboardEntity
    {
        public string TotalActiveClients { get; set; }
        public string TotalInActiveClients { get; set; }
        public List<ClientApplicationEntity> clientApplication { get; set; }
        public ImportJob importJob { get; set; }
        public ExportJob exportJob { get; set; }
    }
    public class ImportJob
    {
        public string ProviderType { get; set; }
        public DateTime DateFilter { get; set; }
        public List<ImportJobDomainEntity> importjobDomain { get; set; }
        public List<ImportJobDomainEntity> importjobDomainMatchRefresh { get; set; }
    }
    public class ExportJob
    {
        public string ProviderType { get; set; }
        public DateTime DateFilter { get; set; }
        public List<ExportJobDomainEntity> exportjobDomain { get; set; }
        public List<ExportJobDomainEntity> exportjobDomainMatchRefresh { get; set; }
    }


    public class DashboardBackgroundProcessEntity
    {
        public int ExecutionId { get; set; }
        public DateTime AuditDateTime { get; set; }
        public int Duration_ms { get; set; }
        public string ProcessStatus { get; set; }
        public string Message { get; set; }
        public string ETLType { get; set; }
        public int NextRunTime_Seconds { get; set; }
        public string Duration { get; set; }
        public decimal Duration_seconds { get; set; }
    }
    public class BackgroundProcessExecutionDetailEntity
    {
        public int AuditId { get; set; }
        public string ETLType { get; set; }
        public string Message { get; set; }
        public DateTime AuditDateTime { get; set; }
        public string ProcessComplete { get; set; }
    }

    #region "Dashboard V2"
    public class DashboardBackgroundProcessStatsEntity
    {
        public string ETLType { get; set; }
        public int NbrSuccess { get; set; }
        public int NbrFailures { get; set; }
        public int NbrRunning { get; set; }
        public int LastExecutionMinutes { get; set; }
        public bool ProcessPaused { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public int NextExecutionTimeSeconds { get; set; }
        public bool ShowErrorSymbol { get; set; }
        public TimeSpan NextExecutionTimeSpan { get; set; }
        public DateTime AverageRunDuration { get; set; }
        public DateTime MaxRunDuration { get; set; }
        public DateTime MinRunDuration { get; set; }
    }
    public class DashboardImportProcessDataQueueStatisticsEntity
    {
        public int ImportProcessId { get; set; }
        public DateTime ImportDate { get; set; }
        public string Tag { get; set; }
        public string ImportProcess { get; set; }
        public int ImportedRowCount { get; set; }
        public int InputRecordCount { get; set; }
        public int DS_LowConfidenceMatchRecordCount { get; set; }
        public int DS_NoMatchRecordCount { get; set; }
        public int MatchProcessingRecordCount { get; set; }
        public int EnrichmentProcessingDUNSCount { get; set; }
        public int MatchExportRecordCount { get; set; }
        public int EnrichmentExportDUNSCount { get; set; }
        public int ArchivalQueueCount { get; set; }
        public int DS_RecordsUnderInvestigationCount { get; set; }
    }


    public class DashboardViewModel
    {
        public PrimaryStatsEntity primaryStats { get; set; }
        public List<LowConfidenceMatchStatsEntity> lowConfidenceMatchStats { get; set; }
        public List<NoMatchStatsEntity> noMatchStats { get; set; }
        public List<MatchOutputStatsEntity> matchOutputStats { get; set; }
        public List<EnrichmentProcessingQueueStatsEntity> enrichmentProcessingQueueStats { get; set; }
        public List<DataEnrichmentStatsEntity> dataEnrichmentStats { get; set; }
        public List<ImportProcessTrendEntity> importProcessTrend { get; set; }
        public DashboardInvestigationStatisticsEntity dashboardInvestigationStatisticsEntity { get; set; }
    }
    public class PrimaryStatsEntity
    {
        public int FilesAwaitingImportCount { get; set; }
        public int FilesAwaitingImportCount_MATCH { get; set; }
        public int FilesAwaitingImportCount_REFRESH { get; set; }
        public int FilesFailedImportCount_MATCH { get; set; }
        public int FilesFailedImportCount_REFRESH { get; set; }
        public int InputRecordCount_Total { get; set; }
        public int InputRecordCount_AwaitingProcessing { get; set; }
        public int InputRecordCount_Processing { get; set; }
        public int InputRecordCount_Failed { get; set; }
        public int InputRecordFileCount { get; set; }
        public int EnrichmentProcessingDUNSCount { get; set; }
        public int LowConfidenceMatchRecordCount { get; set; }
        public int NoMatchRecordCount { get; set; }
        public int MatchProcessingRecordCount { get; set; }
        public int EnrichmentProcessingCount { get; set; }
        public int MatchExportRecordCount { get; set; }
        public int EnrichmentExportDUNSCount { get; set; }
        public int MonitoringExportDUNSCount { get; set; }
        public DateTime LatestMonitoringChangeDateTime { get; set; }
        public string QueueStatus { get; set; }
    }
    public class LowConfidenceMatchStatsEntity
    {
        public int DnBConfidenceCode { get; set; }
        public int NbrRecords { get; set; }
    }
    public class NoMatchStatsEntity
    {
        public string ErrorDescription { get; set; }
        public int NbrRecords { get; set; }
    }
    public class MatchOutputStatsEntity
    {
        public string MatchType { get; set; }
        public string AcceptanceType { get; set; }
        public int NbrRecords { get; set; }
    }
    public class EnrichmentProcessingQueueStatsEntity
    {
        public string DnBAPIName { get; set; }
        public int NbrDUNS { get; set; }
    }
    public class DataEnrichmentStatsEntity
    {
        public string DnBAPIName { get; set; }
        public int NbrDUNS { get; set; }
    }
    public class ImportProcessTrendEntity
    {
        public DateTime ImportedDate { get; set; }
        public int ImportedRecordCount { get; set; }
        public double EpochDate { get; set; }
    }
    public class DashboardInvestigationStatisticsEntity
    {
        public string RequestType { get; set; }
        public int NbrOpenedCases { get; set; }
        public int NbrClosedCases { get; set; }
        public int NbrCaseOpenedLastWeek { get; set; }
        public int AverageResolutionTime_Minutes { get; set; }
    }

    #endregion
}
