using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportJobDataEntity
    {
        public ImportJobDataEntity()
        {
            lstTemplates = new List<ImportFileTemplates>();
            selectedTemplated = new ImportFileTemplates();
            AllUploadedFileColumn = new List<SelectListItem>();
        }
        public List<ImportFileTemplates> lstTemplates { get; set; }
        public ImportFileTemplates selectedTemplated { get; set; }
        public List<SelectListItem> AllUploadedFileColumn { get; set; }
        public int FileImportQueueId { get; set; }
        public string ImportType { get; set; }
        public string SourceType { get; set; }
        public string ExcelWorksheetName { get; set; }
        public string Delimiter { get; set; }
        public bool HasHeader { get; set; }
        public string SourceFileName { get; set; }
        public string SourcePath { get; set; }
        public string FileColumnMetadata { get; set; }
        public string ColumnMappings { get; set; }
        public string Tags { get; set; }
        public string InLanguage { get; set; }
        public int UserId { get; set; }
        public int ProcessStatusId { get; set; }
        public string ProcessStatus { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public DateTime? ProcessStartDateTime { get; set; }
        public DateTime? ProcessEndDateTime { get; set; }
        public int ImportProcessId { get; set; }
        public int RetryCount { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string MainColumnMapping { get; set; }
        public bool? IsUnicode { get; set; }
        public string UnMappedColumns { get; set; }
        public string ClientFileName { get; set; }
    }

    public class ImportFileTemplates
    {
        public ImportFileTemplates()
        {
            lstcolumnName = new List<ColumnClass>();
            lstMappingColumn = new List<string>();
        }
        public int TemplateId { get; set; }
        public string FileFormat { get; set; }
        public string ImportType { get; set; }
        public string TemplateName { get; set; }
        public bool HasHeader { get; set; }
        public string Delimiter { get; set; }
        public string ExcelWorksheetName { get; set; }
        public string FileColumnMetadata { get; set; }
        public string ColumnMappings { get; set; }
        public string Tags { get; set; }
        public string InLanguage { get; set; }
        public int UserId { get; set; }
        public bool? IsUnicode { get; set; }
        public string UnMappedColumns { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public string ImportFileConfigurationId { get; set; }
        public List<ColumnClass> lstcolumnName { get; set; }
        public List<string> lstMappingColumn { get; set; }
    }
    public class ColumnClass
    {
        public String DBColumn { get; set; }
        public String ExcelColumn { get; set; }
    }
}
