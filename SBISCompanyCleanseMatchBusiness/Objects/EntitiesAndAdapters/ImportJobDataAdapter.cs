using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportJobDataAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportJobDataEntity> Adapt(DataTable dt)
        {
            List<ImportJobDataEntity> results = new List<ImportJobDataEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportJobDataEntity ImportJob = new ImportJobDataEntity();
                ImportJob = AdaptItem(rw, dt);
                results.Add(ImportJob);
            }
            return results;
        }

        public ImportJobDataEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ImportJobDataEntity result = new ImportJobDataEntity();
            if (dt.Columns.Contains("FileImportQueueId"))
            {
                result.FileImportQueueId = SafeHelper.GetSafeint(rw["FileImportQueueId"]);
            }

            if (dt.Columns.Contains("ImportType"))
            {
                result.ImportType = SafeHelper.GetSafestring(rw["ImportType"]);
            }

            if (dt.Columns.Contains("SourceType"))
            {
                result.SourceType = SafeHelper.GetSafestring(rw["SourceType"]);
            }

            if (dt.Columns.Contains("ExcelWorksheetName"))
            {
                result.ExcelWorksheetName = SafeHelper.GetSafestring(rw["ExcelWorksheetName"]);
            }

            if (dt.Columns.Contains("Delimiter"))
            {
                result.Delimiter = SafeHelper.GetSafestring(rw["Delimiter"]);
            }

            if (dt.Columns.Contains("HasHeader"))
            {
                result.HasHeader = SafeHelper.GetSafebool(rw["HasHeader"]);
            }

            if (dt.Columns.Contains("SourceFileName"))
            {
                result.SourceFileName = SafeHelper.GetSafestring(rw["SourceFileName"]);
            }

            if (dt.Columns.Contains("SourcePath"))
            {
                result.SourcePath = SafeHelper.GetSafestring(rw["SourcePath"]);
            }

            if (dt.Columns.Contains("FileColumnMetadata"))
            {
                result.FileColumnMetadata = SafeHelper.GetSafestring(rw["FileColumnMetadata"]);
            }

            if (dt.Columns.Contains("ColumnMappings"))
            {
                result.ColumnMappings = SafeHelper.GetSafestring(rw["ColumnMappings"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("InLanguage"))
            {
                result.InLanguage = SafeHelper.GetSafestring(rw["InLanguage"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("ProcessStatusId"))
            {
                result.ProcessStatusId = SafeHelper.GetSafeint(rw["ProcessStatusId"]);
            }

            if (dt.Columns.Contains("ProcessStatus"))
            {
                result.ProcessStatus = SafeHelper.GetSafestring(rw["ProcessStatus"]);
            }

            if (dt.Columns.Contains("RequestedDateTime"))
            {
                result.RequestedDateTime = SafeHelper.GetSafeDateTime(rw["RequestedDateTime"]);
            }

            if (dt.Columns.Contains("ProcessStartDateTime"))
            {
                result.ProcessStartDateTime = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessStartDateTime"]);
            }

            if (dt.Columns.Contains("ProcessEndDateTime"))
            {
                result.ProcessEndDateTime = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessEndDateTime"]);
            }

            if (dt.Columns.Contains("ImportProcessId"))
            {
                result.ImportProcessId = SafeHelper.GetSafeint(rw["ImportProcessId"]);
            }

            if (dt.Columns.Contains("RetryCount"))
            {
                result.RetryCount = SafeHelper.GetSafeint(rw["RetryCount"]);
            }

            if (dt.Columns.Contains("ErrorMessage"))
            {
                result.ErrorMessage = SafeHelper.GetSafestring(rw["ErrorMessage"]);
            }

            if (dt.Columns.Contains("Message"))
            {
                result.Message = SafeHelper.GetSafestring(rw["Message"]);
            }

            return result;
        }

        public List<ImportFileTemplates> AdaptTemplate(DataTable dt)
        {
            List<ImportFileTemplates> results = new List<ImportFileTemplates>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportFileTemplates ImportJob = new ImportFileTemplates();
                ImportJob = AdaptTemplateItem(rw, dt);
                results.Add(ImportJob);
            }
            return results;
        }

        public ImportFileTemplates AdaptTemplateItem(DataRow rw, DataTable dt)
        {
            ImportFileTemplates result = new ImportFileTemplates();
            if (dt.Columns.Contains("TemplateId"))
            {
                result.TemplateId = SafeHelper.GetSafeint(rw["TemplateId"]);
            }

            if (dt.Columns.Contains("FileFormat"))
            {
                result.FileFormat = SafeHelper.GetSafestring(rw["FileFormat"]);
            }

            if (dt.Columns.Contains("ImportType"))
            {
                result.ImportType = SafeHelper.GetSafestring(rw["ImportType"]);
            }

            if (dt.Columns.Contains("TemplateName"))
            {
                result.TemplateName = SafeHelper.GetSafestring(rw["TemplateName"]);
            }

            if (dt.Columns.Contains("Delimiter"))
            {
                result.Delimiter = SafeHelper.GetSafestring(rw["Delimiter"]);
            }

            if (dt.Columns.Contains("HasHeader"))
            {
                result.HasHeader = SafeHelper.GetSafebool(rw["HasHeader"]);
            }

            if (dt.Columns.Contains("ExcelWorksheetName"))
            {
                result.ExcelWorksheetName = SafeHelper.GetSafestring(rw["ExcelWorksheetName"]);
            }

            if (dt.Columns.Contains("FileColumnMetadata"))
            {
                result.FileColumnMetadata = SafeHelper.GetSafestring(rw["FileColumnMetadata"]);
            }

            if (dt.Columns.Contains("ColumnMappings"))
            {
                result.ColumnMappings = SafeHelper.GetSafestring(rw["ColumnMappings"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("InLanguage"))
            {
                result.InLanguage = SafeHelper.GetSafestring(rw["InLanguage"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("IsUnicode"))
            {
                result.IsUnicode = SafeHelper.GetSafeboolIfNull(rw["IsUnicode"]);
            }

            if (dt.Columns.Contains("UpdatedDateTime"))
            {
                result.UpdatedDateTime = SafeHelper.GetSafeDateTime(rw["UpdatedDateTime"]);
            }
            if (dt.Columns.Contains("ImportFileConfigurationId"))
            {
                result.ImportFileConfigurationId = SafeHelper.GetSafestring(rw["ImportFileConfigurationId"]);
            }
            return result;
        }
    }
}
