using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ExportJobSettingsFacade : FacadeParent
    {
        ExportJobSettingsBusiness rep;
        public ExportJobSettingsFacade(string connectionString) : base(connectionString) { rep = new ExportJobSettingsBusiness(Connection); }
        public List<ExportJobSettingsEntity> GetExportJobSettings()
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            results = rep.GetExportJobSettings();
            return results;
        }
        public List<MonitoringNotificationJobSettingsEntity> GetMonitoringExportJobSettings()
        {
            return rep.GetMonitoringExportJobSettings();
        }

        public int InserExportJobSettings(ExportJobSettingsEntity obj)
        {
            return rep.InserExportJobSettings(obj);
        }
        public string InserMonitoringNotificationsJobSettings(MonitoringNotificationJobSettingsEntity obj)
        {
            return rep.InserMonitoringNotificationsJobSettings(obj);
        }
        public void UpdateExportJobSettings(ExportJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert, int RetryCount = 0, string ErrorMessage = null)
        {
            rep.UpdateExportJobSettings(obj, IsPorcessStart, IsRevert, RetryCount, ErrorMessage);
        }
        public void UpdateMonitoringExportJobSettings(MonitoringNotificationJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert, int RetryCount = 0, string ErrorMessage = null)
        {
            rep.UpdateMonitoringExportJobSettings(obj, IsPorcessStart, IsRevert, RetryCount, ErrorMessage);
        }
        public void UpdateExportJobSettingsSentMail(ExportJobSettingsEntity obj, bool IsEmailSent)
        {
            rep.UpdateExportJobSettingsSentMail(obj, IsEmailSent);
        }
        public void UpdateMonitoringExportJobSettingsSentMail(MonitoringNotificationJobSettingsEntity obj, bool IsEmailSent)
        {
            rep.UpdateMonitoringExportJobSettingsSentMail(obj, IsEmailSent);
        }
        public List<ExportJobSettingsEntity> GetExportJobSettingsByUserId(string ExportType, int? UserId, int ApplicationId)
        {
            List<ExportJobSettingsEntity> results = new List<ExportJobSettingsEntity>();
            results = rep.GetExportJobSettingsByUserId(ExportType, UserId, ApplicationId);
            return results;
        }
        public List<MonitoringNotificationJobSettingsEntity> GetMonitoringExportJobSettingsByUserId(int? UserId, int ApplicationId)
        {
            List<MonitoringNotificationJobSettingsEntity> results = new List<MonitoringNotificationJobSettingsEntity>();
            results = rep.GetMonitoringExportJobSettingsByUserId(UserId, ApplicationId);
            return results;
        }

        // Deletes export job queue
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public void DeleteExportJobSettingsForClients(ExportJobSettingsEntity obj)
        {
            rep.DeleteExportJobSettingsForClients(obj);
        }
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        public void DeleteExportJobSettingsForMaster(ExportJobSettingsEntity obj)
        {
            rep.DeleteExportJobSettingsForMaster(obj);
        }
        // Monitor Export Job Queue Details
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public ExportJobSettingsEntity GetExportJobSettingsByIdByClient(int Id)
        {
            ExportJobSettingsEntity results = new ExportJobSettingsEntity();
            results = rep.GetExportJobSettingsByIdByClient(Id);
            return results;
        }
        public void UpdateExportJobSettingsForDownload(int Id, int UserId)
        {
            rep.UpdateExportJobSettingsForDownload(Id, UserId);
        }
        public string ExportJobSettingNotifications(int ApplicationId, int UserId, string ProviderType)
        {
            return rep.ExportJobSettingNotifications(ApplicationId, UserId, ProviderType);
        }
        public void UpdateExportJobSettingNotificationsStatus(int ApplicationId, int UserId, string ExportType)
        {
            rep.UpdateExportJobSettingNotificationsStatus(ApplicationId, UserId, ExportType);
        }
        public string IsExistExportFileName(string fileName, int Applicationid)
        {
            return rep.IsExistExportFileName(fileName, Applicationid);
        }
        public int UnflagExportedRecords(ReExportDataEntity objReExport)
        {
            return rep.UnflagExportedRecords(objReExport);
        }
        public List<ExportJobSettingsEntity> GetPurgeExportedFiles()
        {
            return rep.GetPurgeExportedFiles();
        }
        public void CancelExportJobSettings(ExportJobSettingsEntity obj)
        {
            rep.CancelExportJobSettings(obj);
        }
    }
}
