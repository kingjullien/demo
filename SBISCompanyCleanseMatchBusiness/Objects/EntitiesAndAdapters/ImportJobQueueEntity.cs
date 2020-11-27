using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportJobQueueEntity
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string SourceType { get; set; }
        public string SourceFileName { get; set; }
        public DateTime ImportedDate { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
        public bool IsProcessComplete { get; set; }
        public bool IsNotify { get; set; }
        public string ColumnMapping { get; set; }
        public string ImportType { get; set; }
        public string Tags { get; set; }
        public string InLanguage { get; set; }
        public int UserId { get; set; }
        public string Delimiter { get; set; }
        public bool IsEmailSent { get; set; }
        public string ApplicationName { get; set; }
        public string ProvidersType { get; set; }
        public bool IsHeader { get; set; }
        public int RetryCount { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ImportExportJobQueueEntity
    {
        public int Id { get; set; }
        public string ApplicationSubDomain { get; set; }
        public string FileName { get; set; }
        public string Format { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public DateTime? ProcessEndDate { get; set; }
        public bool IsProcessComplete { get; set; }
        public bool isDeleted { get; set; }
        public string ExportType { get; set; }
    }
}
