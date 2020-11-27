using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportJobQueueAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportJobQueueEntity> Adapt(DataTable dt)
        {
            List<ImportJobQueueEntity> results = new List<ImportJobQueueEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportJobQueueEntity ExportJob = new ImportJobQueueEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }
        public ImportJobQueueEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ImportJobQueueEntity result = new ImportJobQueueEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("ApplicationId"))
            {
                result.ApplicationId = SafeHelper.GetSafeint(rw["ApplicationId"]);
            }

            if (dt.Columns.Contains("SourceType"))
            {
                result.SourceType = SafeHelper.GetSafestring(rw["SourceType"]);
            }

            if (dt.Columns.Contains("SourceFileName"))
            {
                result.SourceFileName = SafeHelper.GetSafestring(rw["SourceFileName"]);
            }

            if (dt.Columns.Contains("ImportedDate"))
            {
                result.ImportedDate = SafeHelper.GetSafeDateTime(rw["ImportedDate"]);
            }

            if (dt.Columns.Contains("ProcessStartDate"))
            {
                result.ProcessStartDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessStartDate"]);
            }

            if (dt.Columns.Contains("ProcessEndDate"))
            {
                //if (rw["ProcessEndDate"] == DBNull.Value)
                //{
                //    result.ProcessEndDate = DateTime.MinValue;
                //}
                //else
                //{
                result.ProcessEndDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessEndDate"]);
                //}
            }

            if (dt.Columns.Contains("IsProcessComplete"))
            {
                result.IsProcessComplete = SafeHelper.GetSafebool(rw["IsProcessComplete"]);
            }

            if (dt.Columns.Contains("IsNotify"))
            {
                result.IsNotify = SafeHelper.GetSafebool(rw["IsNotify"]);
            }

            if (dt.Columns.Contains("ColumnMapping"))
            {
                result.ColumnMapping = SafeHelper.GetSafestring(rw["ColumnMapping"]);
            }

            if (dt.Columns.Contains("ImportType"))
            {
                result.ImportType = SafeHelper.GetSafestring(rw["ImportType"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("InLanguage"))
            {
                result.InLanguage = SafeHelper.GetSafestring(rw["InLanguage"]);
            }

            if (dt.Columns.Contains("InLanguage"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("Delimiter"))
            {
                result.Delimiter = SafeHelper.GetSafestring(rw["Delimiter"]);
            }

            if (dt.Columns.Contains("IsEmailSent"))
            {
                result.IsEmailSent = SafeHelper.GetSafebool(rw["IsEmailSent"]);
            }

            if (dt.Columns.Contains("ProviderType"))
            {
                result.ProvidersType = SafeHelper.GetSafestring(rw["ProviderType"]);
            }

            if (dt.Columns.Contains("IsHeader"))
            {
                result.IsHeader = SafeHelper.GetSafebool(rw["IsHeader"]);
            }

            if (dt.Columns.Contains("RetryCount"))
            {
                result.RetryCount = SafeHelper.GetSafeint(rw["RetryCount"]);
            }

            if (dt.Columns.Contains("ErrorMessage"))
            {
                result.ErrorMessage = SafeHelper.GetSafestring(rw["ErrorMessage"]);
            }

            return result;
        }
    }



    public class ImportExportJobQueueAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportExportJobQueueEntity> Adapt(DataTable dt)
        {
            List<ImportExportJobQueueEntity> results = new List<ImportExportJobQueueEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportExportJobQueueEntity ExportJob = new ImportExportJobQueueEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }


        public ImportExportJobQueueEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ImportExportJobQueueEntity result = new ImportExportJobQueueEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("Host"))
            {
                result.ApplicationSubDomain = SafeHelper.GetSafestring(rw["Host"]);
            }

            if (dt.Columns.Contains("FileName"))
            {
                result.FileName = SafeHelper.GetSafestring(rw["FileName"]);
            }

            if (dt.Columns.Contains("Format"))
            {
                result.Format = SafeHelper.GetSafestring(rw["Format"]);
            }

            if (dt.Columns.Contains("RequestedDate"))
            {
                result.RequestedDate = SafeHelper.GetSafeDateTime(rw["RequestedDate"]);
            }

            if (dt.Columns.Contains("ProcessStartDate"))
            {
                //if (rw["ProcessStartDate"] == DBNull.Value)
                //{
                //    result.ProcessStartDate = DateTime.MinValue;
                //}
                //else
                //{
                result.ProcessStartDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessStartDate"]);
                //}
            }
            if (dt.Columns.Contains("ProcessEndDate"))
            {
                //if (rw["ProcessEndDate"] == DBNull.Value)
                //{
                //    result.ProcessEndDate = DateTime.MinValue;
                //}
                //else
                //{
                result.ProcessEndDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessEndDate"]);
                //}
            }
            if (dt.Columns.Contains("IsProcessComplete"))
            {
                result.IsProcessComplete = SafeHelper.GetSafebool(rw["IsProcessComplete"]);
            }

            if (dt.Columns.Contains("isDeleted"))
            {
                result.isDeleted = SafeHelper.GetSafebool(rw["isDeleted"]);
            }

            if (dt.Columns.Contains("ExportType"))
            {
                result.ExportType = SafeHelper.GetSafestring(rw["ExportType"]);
            }

            return result;
        }

    }
}
