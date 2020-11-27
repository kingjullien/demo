using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class ImportJobDomainAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ImportJobDomainEntity> AdaptLists(DataTable dt)
        {
            List<ImportJobDomainEntity> results = new List<ImportJobDomainEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ImportJobDomainEntity clientlist = new ImportJobDomainEntity();
                clientlist = AdaptItemReport(rw, dt);
                results.Add(clientlist);
            }
            return results;
        }
        public ImportJobDomainEntity AdaptItemReport(DataRow rw, DataTable dt)
        {
            ImportJobDomainEntity results = new ImportJobDomainEntity();
            results.ApplicationSubDomain = SafeHelper.GetSafestring(rw["AppicationSubDomain"]);
            results.DataImportInProgress = SafeHelper.GetSafestring(rw["DataImportInProgress"]);
            results.DataImportReady = SafeHelper.GetSafestring(rw["DataImportReady"]);
            results.DataImportFailed = SafeHelper.GetSafestring(rw["DataImportFailed"]);
            return results;
        }
    }
}
