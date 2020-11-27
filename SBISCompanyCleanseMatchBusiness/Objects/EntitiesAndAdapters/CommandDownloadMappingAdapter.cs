using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CommandDownloadMappingAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<CommandDownloadMappingEntity> Adapt(DataTable dt)
        {
            List<CommandDownloadMappingEntity> results = new List<CommandDownloadMappingEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                CommandDownloadMappingEntity matchCode = new CommandDownloadMappingEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public CommandDownloadMappingEntity AdaptItem(DataRow rw, DataTable dt)
        {
            CommandDownloadMappingEntity result = new CommandDownloadMappingEntity();

            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("ConfigurationName"))
            {
                result.ConfigurationName = SafeHelper.GetSafestring(rw["ConfigurationName"]);
            }

            if (dt.Columns.Contains("Tag"))
            {
                result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            }

            if (dt.Columns.Contains("LOBTag"))
            {
                result.LOBTag = SafeHelper.GetSafestring(rw["LOBTag"]);
            }

            if (dt.Columns.Contains("DownloadMatchOutput"))
            {
                result.DownloadMatchOutput = SafeHelper.GetSafebool(rw["DownloadMatchOutput"]);
            }

            if (dt.Columns.Contains("DownloadEnrichmentOutput"))
            {
                result.DownloadEnrichmentOutput = SafeHelper.GetSafebool(rw["DownloadEnrichmentOutput"]);
            }

            if (dt.Columns.Contains("DownloadMonitoringUpdates"))
            {
                result.DownloadMonitoringUpdates = SafeHelper.GetSafebool(rw["DownloadMonitoringUpdates"]);
            }

            if (dt.Columns.Contains("DownloadActiveDataQueue"))
            {
                result.DownloadActiveDataQueue = SafeHelper.GetSafebool(rw["DownloadActiveDataQueue"]);
            }

            if (dt.Columns.Contains("DownloadTransferDUNS"))
            {
                result.DownloadTransferDUNS = SafeHelper.GetSafebool(rw["DownloadTransferDUNS"]);
            }

            if (dt.Columns.Contains("DownloadFormat"))
            {
                result.DownloadFormat = SafeHelper.GetSafestring(rw["DownloadFormat"]);
            }

            if (dt.Columns.Contains("Formatvalue"))
            {
                result.Formatvalue = SafeHelper.GetSafestring(rw["Formatvalue"]);
            }

            if (dt.Columns.Contains("MarkAsExported"))
            {
                result.MarkAsExported = SafeHelper.GetSafebool(rw["MarkAsExported"]);
            }

            if (dt.Columns.Contains("IsDefault"))
            {
                result.IsDefault = SafeHelper.GetSafebool(rw["IsDefault"]);
            }

            if (dt.Columns.Contains("APILayer"))
            {
                result.APILayer = SafeHelper.GetSafestring(rw["APILayer"]);
            }

            if (dt.Columns.Contains("IsAppendDateTime"))
            {
                result.IsAppendDateTime = SafeHelper.GetSafebool(rw["IsAppendDateTime"]);
            }

            if (dt.Columns.Contains("IsFilePrefix"))
            {
                result.IsFilePrefix = SafeHelper.GetSafebool(rw["IsFilePrefix"]);
            }

            if (dt.Columns.Contains("DateTimeFileFormat"))
            {
                result.DateTimeFileFormat = SafeHelper.GetSafestring(rw["DateTimeFileFormat"]);
            }

            if (dt.Columns.Contains("FilePrefix"))
            {
                result.FilePrefix = SafeHelper.GetSafestring(rw["FilePrefix"]);
            }

            if (dt.Columns.Contains("ProviderType"))
            {
                result.ProviderType = SafeHelper.GetSafestring(rw["ProviderType"]);
            }

            if (dt.Columns.Contains("DownloadCompanyTree"))
            {
                result.DownloadCompanyTree = SafeHelper.GetSafebool(rw["DownloadCompanyTree"]);
            }

            if (dt.Columns.Contains("DownloadNoMatchQueue"))
            {
                result.DownloadNoMatchQueue = SafeHelper.GetSafebool(rw["DownloadNoMatchQueue"]);
            }

            if (dt.Columns.Contains("DownloadLCMQueue"))
            {
                result.DownloadLCMQueue = SafeHelper.GetSafebool(rw["DownloadLCMQueue"]);
            }

            if (dt.Columns.Contains("ApplyTextQualifierToAll"))
            {
                result.ApplyTextQualifierToAll = SafeHelper.GetSafebool(rw["ApplyTextQualifierToAll"]);
            }

            return result;
        }
    }
}
