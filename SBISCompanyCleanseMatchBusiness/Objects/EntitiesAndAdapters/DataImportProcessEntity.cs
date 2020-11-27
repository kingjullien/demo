using System;


namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DataImportProcessEntity
    {
        public int ImportProcessId { get; set; }
        public int UserId { get; set; }
        public string SourceType { get; set; }
        public string Source { get; set; }
        public DateTime ImportedDate { get; set; }
        public int ImportedRowCount { get; set; }
        public int ProcessedRowCount { get; set; }
        public int ProcessStatusId { get; set; }

    }
}
