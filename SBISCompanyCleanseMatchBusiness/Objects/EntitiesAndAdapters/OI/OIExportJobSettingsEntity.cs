using System;
using System.ComponentModel;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIExportJobSettingsEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Tags { get; set; }
        public string Input { get; set; }
        public string LOBTag { get; set; }
        public bool MatchOutPut { get; set; }           /*Same column is used for both D&B and OI*/
        public bool Enrichment { get; set; }
        public bool ActiveDataQueue { get; set; }
        public bool MarkAsExported { get; set; }
        public string Format { get; set; }
        public string FilePath { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
        public bool IsProcessComplete { get; set; }
        public bool IsAlreadyDownloaded { get; set; }
        public int ApplicationId { get; set; }
        public DateTime LastDownloadedDate { get; set; }
        public int LastDownloadedUserId { get; set; }
        public string Delimiter { get; set; }
        public string SrcRecID { get; set; }
        public bool SrcRecIdExactMatch { get; set; }
        public bool IsEmailSent { get; set; }
        public bool LCMQueue { get; set; }
        public bool NoMatchQueue { get; set; }
        public String APILayer { get; set; }
        public bool IsMonitoringNotifications { get; set; }
        public bool TrasferedDuns { get; set; }
        public string ApplicationName { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? ProcessCancelledDateTime { get; set; }
        public bool IsAlreadyNotify { get; set; }
        public bool IsDeleted { get; set; }
        public string ExportType { get; set; }
        public bool CompanyTree { get; set; }   /*This column added and is used for OI*/
        public string ErrorMessage { get; set; }
        public bool HasHeader { get; set; }
        public bool HasTextQualifierToAll { get; set; }
    }
    public class OIReExportDataEntity
    {
        public string SrcRecordId { get; set; }
        public string SrcRecordIdList { get; set; }
        public string ImportProcess { get; set; }
        public int CountryGroupId { get; set; }
        public string Tags { get; set; }
        public RecordType Recordtype { get; set; }
        public int UserId { get; set; }
        public bool GetCountsOnly { get; set; }
    }
    public enum OIRecordType
    {
        [Description("MATCHED")]
        MATCHED,
        [Description("ENRICHED")]
        ENRICHED,
        [Description("Both ")]
        Both
    }
}
