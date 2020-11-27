using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects
{
    public class ImportFileConfigurationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportFileConfigurationEntity> Adapt(DataTable dt)
        {
            List<ImportFileConfigurationEntity> results = new List<ImportFileConfigurationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportFileConfigurationEntity matchCode = new ImportFileConfigurationEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public ImportFileConfigurationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ImportFileConfigurationEntity result = new ImportFileConfigurationEntity();

            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("ConfigurationName"))
            {
                result.ConfigurationName = SafeHelper.GetSafestring(rw["ConfigurationName"]);
            }

            if (dt.Columns.Contains("ExternalDataStoreId"))
            {
                result.ExternalDataStoreId = SafeHelper.GetSafeint(rw["ExternalDataStoreId"]);
            }
            if (dt.Columns.Contains("SourceFolderPath"))
            {
                result.SourceFolderPath = SafeHelper.GetSafestring(rw["SourceFolderPath"]);
            }
            if (dt.Columns.Contains("TemplateId"))
            {
                result.TemplateId = SafeHelper.GetSafeint(rw["TemplateId"]);
            }
            if (dt.Columns.Contains("FileNamePattern"))
            {
                result.FileNamePattern = SafeHelper.GetSafestring(rw["FileNamePattern"]);
            }
            if (dt.Columns.Contains("PostLoadAction"))
            {
                result.PostLoadAction = SafeHelper.GetSafestring(rw["PostLoadAction"]);
            }
            if (dt.Columns.Contains("PostLoadActionParameters"))
            {
                result.PostLoadActionParameters = SafeHelper.GetSafestring(rw["PostLoadActionParameters"]);
            }
            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }
            return result;
        }
    }
}
