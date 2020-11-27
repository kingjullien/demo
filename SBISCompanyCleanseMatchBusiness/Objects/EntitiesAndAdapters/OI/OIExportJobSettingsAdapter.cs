using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    public class OIExportJobSettingsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OIExportJobSettingsEntity> Adapt(DataTable dt)
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OIExportJobSettingsEntity ExportJob = new OIExportJobSettingsEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }
        public OIExportJobSettingsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            OIExportJobSettingsEntity result = new OIExportJobSettingsEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("Input"))
            {
                result.Input = SafeHelper.GetSafestring(rw["Input"]);
            }

            if (dt.Columns.Contains("LOBTag"))
            {
                result.LOBTag = SafeHelper.GetSafestring(rw["LOBTag"]);
            }

            if (dt.Columns.Contains("MatchOutPut"))
            {
                result.MatchOutPut = SafeHelper.GetSafebool(rw["MatchOutPut"]);
            }

            if (dt.Columns.Contains("Enrichment"))
            {
                result.Enrichment = SafeHelper.GetSafebool(rw["Enrichment"]);
            }

            if (dt.Columns.Contains("ActiveDataQueue"))
            {
                result.ActiveDataQueue = SafeHelper.GetSafebool(rw["ActiveDataQueue"]);
            }

            if (dt.Columns.Contains("MarkAsExported"))
            {
                result.MarkAsExported = SafeHelper.GetSafebool(rw["MarkAsExported"]);
            }

            if (dt.Columns.Contains("Format"))
            {
                result.Format = SafeHelper.GetSafestring(rw["Format"]);
            }

            if (dt.Columns.Contains("FilePath"))
            {
                result.FilePath = SafeHelper.GetSafestring(rw["FilePath"]);
            }

            if (dt.Columns.Contains("RequestedDate"))
            {
                result.RequestedDate = SafeHelper.GetSafeDateTime(rw["RequestedDate"]);
            }

            if (dt.Columns.Contains("ProcessStartDate"))
            {
                if (rw["ProcessStartDate"] == DBNull.Value)
                {
                    result.ProcessStartDate = null;
                }
                else
                {
                    result.ProcessStartDate = Convert.ToDateTime(rw["ProcessStartDate"]);
                }
            }

            if (dt.Columns.Contains("ProcessEndDate"))
            {
                if (rw["ProcessEndDate"] == DBNull.Value)
                {
                    result.ProcessEndDate = null;
                }
                else
                {
                    result.ProcessEndDate = Convert.ToDateTime(rw["ProcessEndDate"]);
                }
            }

            if (dt.Columns.Contains("IsProcessComplete"))
            {
                result.IsProcessComplete = SafeHelper.GetSafebool(rw["IsProcessComplete"]);
            }

            if (dt.Columns.Contains("IsAlreadyDownloaded"))
            {
                result.IsAlreadyDownloaded = SafeHelper.GetSafebool(rw["IsAlreadyDownloaded"]);
            }

            if (dt.Columns.Contains("ApplicationId"))
            {
                result.ApplicationId = SafeHelper.GetSafeint(rw["ApplicationId"]);
            }

            if (dt.Columns.Contains("LastDownloadedDate"))
            {
                result.LastDownloadedDate = SafeHelper.GetSafeDateTime(rw["LastDownloadedDate"]);
            }

            if (dt.Columns.Contains("LastDownloadedUserId"))
            {
                result.LastDownloadedUserId = SafeHelper.GetSafeint(rw["LastDownloadedUserId"]);
            }

            if (dt.Columns.Contains("Delimiter"))
            {
                result.Delimiter = SafeHelper.GetSafestring(rw["Delimiter"]);
            }

            if (dt.Columns.Contains("SrcRecID"))
            {
                result.SrcRecID = SafeHelper.GetSafestring(rw["SrcRecID"]);
            }

            if (dt.Columns.Contains("IsExactMatch"))
            {
                result.SrcRecIdExactMatch = SafeHelper.GetSafebool(rw["IsExactMatch"]);
            }

            if (dt.Columns.Contains("IsEmailSent"))
            {
                result.IsEmailSent = SafeHelper.GetSafebool(rw["IsEmailSent"]);
            }

            if (dt.Columns.Contains("LCMQueue"))
            {
                result.LCMQueue = SafeHelper.GetSafebool(rw["LCMQueue"]);
            }

            if (dt.Columns.Contains("NoMatchQueue"))
            {
                result.NoMatchQueue = SafeHelper.GetSafebool(rw["NoMatchQueue"]);
            }

            if (dt.Columns.Contains("TrasferedDuns"))
            {
                result.TrasferedDuns = SafeHelper.GetSafebool(rw["TrasferedDuns"]); //Add DUNS Transfer to Export(MP-367)
            }

            if (dt.Columns.Contains("IsCancelled"))
            {
                result.IsCancelled = SafeHelper.GetSafebool(rw["IsCancelled"]);
            }

            if (dt.Columns.Contains("ProcessCancelledDateTime"))
            {
                if (rw["ProcessCancelledDateTime"] == DBNull.Value)
                {
                    result.ProcessCancelledDateTime = null;
                }
                else
                {
                    result.ProcessCancelledDateTime = Convert.ToDateTime(rw["ProcessCancelledDateTime"]);
                }
            }

            if (dt.Columns.Contains("IsDeleted"))
            {
                result.IsDeleted = SafeHelper.GetSafebool(rw["IsDeleted"]);
            }

            if (dt.Columns.Contains("IsAlreadyNotify"))
            {
                result.IsAlreadyNotify = SafeHelper.GetSafebool(rw["IsAlreadyNotify"]);
            }

            if (dt.Columns.Contains("ExportType"))
            {
                result.ExportType = SafeHelper.GetSafestring(rw["ExportType"]);
            }

            if (dt.Columns.Contains("CompanyTree"))
            {
                result.CompanyTree = SafeHelper.GetSafebool(rw["CompanyTree"]);
            }

            if (dt.Columns.Contains("ErrorMessage"))
            {
                result.ErrorMessage = SafeHelper.GetSafestring(rw["ErrorMessage"]);
            }

            if (dt.Columns.Contains("HasTextQualifierToAll"))
            {
                result.HasTextQualifierToAll = SafeHelper.GetSafebool(rw["HasTextQualifierToAll"]);
            }

            if (dt.Columns.Contains("HasHeader"))
            {
                result.HasHeader = SafeHelper.GetSafebool(rw["HasHeader"]);
            }

            return result;
        }
    }
}
