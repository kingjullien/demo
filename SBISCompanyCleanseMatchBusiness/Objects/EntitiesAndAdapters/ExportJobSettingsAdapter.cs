using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ExportJobSettingsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<ExportJobSettingsEntity> Adapt(DataTable dt)
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                ExportJobSettingsEntity ExportJob = new ExportJobSettingsEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }

        public ExportJobSettingsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            ExportJobSettingsEntity result = new ExportJobSettingsEntity();
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
                //if (rw["ProcessStartDate"] == DBNull.Value)
                //{
                //    result.ProcessStartDate = null;
                //}
                //else
                //{
                result.ProcessStartDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessStartDate"]);
            }
            //}
            if (dt.Columns.Contains("ProcessEndDate"))
            {
                result.ProcessEndDate = SafeHelper.GetSafeDateTimeIfNull(rw["ProcessEndDate"]);
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
                result.ProcessCancelledDateTime = SafeHelper.GetSafeDateTime(rw["ProcessCancelledDateTime"]);
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

            if (dt.Columns.Contains("RetryCount"))
            {
                result.RetryCount = SafeHelper.GetSafeint(rw["RetryCount"]);
            }

            if (dt.Columns.Contains("ErrorMessage"))
            {
                result.ErrorMessage = SafeHelper.GetSafestring(rw["ErrorMessage"]);
            }

            if (dt.Columns.Contains("HasHeader"))
            {
                result.HasHeader = SafeHelper.GetSafebool(rw["HasHeader"]);
            }

            if (dt.Columns.Contains("HasTextQualifierToAll"))
            {
                result.HasTextQualifierToAll = SafeHelper.GetSafebool(rw["HasTextQualifierToAll"]);
            }

            return result;
        }
    }



    public class MonitoringNotificationJobSettingsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MonitoringNotificationJobSettingsEntity> Adapt(DataTable dt)
        {
            List<MonitoringNotificationJobSettingsEntity> results = new List<MonitoringNotificationJobSettingsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MonitoringNotificationJobSettingsEntity ExportJob = new MonitoringNotificationJobSettingsEntity();
                ExportJob = AdaptItem(rw, dt);
                results.Add(ExportJob);
            }
            return results;
        }

        public MonitoringNotificationJobSettingsEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MonitoringNotificationJobSettingsEntity result = new MonitoringNotificationJobSettingsEntity();
            if (dt.Columns.Contains("Id"))
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
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
                result.ProcessEndDate = SafeHelper.GetSafeDateTime(rw["ProcessEndDate"]);
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

            if (dt.Columns.Contains("IsEmailSent"))
            {
                result.IsEmailSent = SafeHelper.GetSafebool(rw["IsEmailSent"]);
            }

            if (dt.Columns.Contains("IsMonitoringNotifications"))
            {
                result.IsMonitoringNotifications = SafeHelper.GetSafebool(rw["IsMonitoringNotifications"]);
            }

            if (dt.Columns.Contains("APILayer"))
            {
                result.APILayer = SafeHelper.GetSafestring(rw["APILayer"]);
            }

            if (dt.Columns.Contains("IsCancelled"))
            {
                result.IsCancelled = SafeHelper.GetSafebool(rw["IsCancelled"]);
            }

            if (dt.Columns.Contains("ProcessCancelledDateTime"))
            {
                result.ProcessCancelledDateTime = SafeHelper.GetSafeDateTime(rw["ProcessCancelledDateTime"]);
            }

            if (dt.Columns.Contains("ExportType"))
            {
                result.ExportType = SafeHelper.GetSafestring(rw["ExportType"]);
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
}
