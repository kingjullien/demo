using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CommandUploadMappingAdapter
    {

        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CommandUploadMappingEntity> Adapt(DataTable dt)
        {
            List<CommandUploadMappingEntity> results = new List<CommandUploadMappingEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CommandUploadMappingEntity matchCode = new CommandUploadMappingEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public CommandUploadMappingEntity AdaptItem(DataRow rw, DataTable dt)
        {
            CommandUploadMappingEntity result = new CommandUploadMappingEntity();

            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("ConfigurationName"))
            {
                result.ConfigurationName = SafeHelper.GetSafestring(rw["ConfigurationName"]);
            }

            if (dt.Columns.Contains("ImportType"))
            {
                result.ImportType = SafeHelper.GetSafestring(rw["ImportType"]);
            }

            if (dt.Columns.Contains("FileFormat"))
            {
                result.FileFormat = SafeHelper.GetSafestring(rw["FileFormat"]);
            }

            if (dt.Columns.Contains("Formatvalue"))
            {
                result.Formatvalue = SafeHelper.GetSafestring(rw["Formatvalue"]);
            }

            if (dt.Columns.Contains("ColumnMapping"))
            {
                result.ColumnMapping = SafeHelper.GetSafestring(rw["ColumnMapping"]);
            }

            if (dt.Columns.Contains("IsDefault"))
            {
                result.IsDefault = SafeHelper.GetSafebool(rw["IsDefault"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("CreatedDate"))
            {
                result.CreatedDate = SafeHelper.GetSafeDateTime(rw["CreatedDate"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("InLanguage"))
            {
                result.InLanguage = SafeHelper.GetSafestring(rw["InLanguage"]);
            }

            if (dt.Columns.Contains("OriginalColumns"))
            {
                result.OriginalColumns = SafeHelper.GetSafestring(rw["OriginalColumns"]);
            }

            return result;
        }
    }
}
