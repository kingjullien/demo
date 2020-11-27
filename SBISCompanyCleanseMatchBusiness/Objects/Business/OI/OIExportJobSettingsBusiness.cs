using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class OIExportJobSettingsBusiness : BusinessParent
    {
        OIExportJobSettingsRepository rep;

        public OIExportJobSettingsBusiness(string connectionString) : base(connectionString) { rep = new OIExportJobSettingsRepository(Connection); }

        public List<OIExportJobSettingsEntity> GetExportJobSettings()
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            results = rep.GetExportJobSettings();
            return results;
        }
        public int InserExportJobSettings(OIExportJobSettingsEntity obj)
        {
            return rep.InserExportJobSettings(obj);
        }
        public int UpdateExportJobDetails(OIExportJobSettingsEntity obj)
        {
            return rep.UpdateExportJobDetails(obj);
        }
        public void UpdateExportJobSettings(OIExportJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert)
        {
            rep.UpdateExportJobSettings(obj, IsPorcessStart, IsRevert);
        }
        public void UpdateExportJobSettingsSentMail(OIExportJobSettingsEntity obj, bool IsEmailSent)
        {
            rep.UpdateExportJobSettingsSentMail(obj, IsEmailSent);
        }
        public List<OIExportJobSettingsEntity> GetExportJobSettingsByUserId(string ExportType, int? UserId, int ApplicationId)
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            results = rep.GetExportJobSettingsByUserId(ExportType, UserId, ApplicationId);
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        public void DeleteOIExportJobSettings(OIExportJobSettingsEntity obj)
        {
            rep.DeleteOIExportJobSettings(obj);
        }
        public OIExportJobSettingsEntity GetExportJobSettingsById(int Id)
        {
            OIExportJobSettingsEntity results = new OIExportJobSettingsEntity();
            results = rep.GetExportJobSettingsById(Id);
            return results;
        }
        public void UpdateExportJobSettingsForDownload(int Id, int UserId)
        {
            rep.UpdateExportJobSettingsForDownload(Id, UserId);
        }

        public string IsExistExportFileName(string fileName, int Applicationid)
        {
            return rep.IsExistExportFileName(fileName, Applicationid);
        }
        public List<OIExportJobSettingsEntity> GetPurgeExportedFiles()
        {
            return rep.GetPurgeExportedFiles();
        }
        public void CancelExportJobSettings(OIExportJobSettingsEntity obj)
        {
            rep.CancelExportJobSettings(obj);
        }

        public SqlDataReader ExportCompanyFirmographicsDataReader(bool MarkAsExported)
        {
            return rep.ExportCompanyFirmographicsDataReader(MarkAsExported);
        }


        public SqlDataReader ExportCompanyTreeDataReader(bool MarkAsExported)
        {
            return rep.ExportCompanyTreeDataReader(MarkAsExported);
        }


        public SqlDataReader ExportActiveDataQueue(int UserId, string SrcRecordId, string LOBTag, string Tag, string ImportProcess)
        {
            return rep.ExportActiveDataQueue(UserId, SrcRecordId, LOBTag, Tag, ImportProcess);
        }
        public DataTable ExportActiveRecordsToExcel(string InputId)
        {
            return rep.ExportActiveRecordsToExcel(InputId);
        }
        public DataTable ExportActiveRecordToExcel(OIExportToExcel Model)
        {
            return rep.ExportActiveRecordToExcel(Model);
        }
        public SqlDataReader ExportActiveRecordsDataReader(string SrcRecID, string ImportProcess, string Tag)
        {
            return rep.ExportActiveRecordsDataReader(SrcRecID, ImportProcess, Tag);
        }
        #region OI Export Data
        public void FinalizeMatchExport(bool MarkAsExported)
        {
            rep.FinalizeMatchExport(MarkAsExported);
        }
        public SqlDataReader ExportMatchedDataReader(bool MarkAsExported, string LOBTag, string Tag, string ImportProcess, string SrcRecordId, bool SrcRecIdExactMatch)
        {
            return rep.ExportMatchedDataReader(MarkAsExported, LOBTag, Tag, ImportProcess, SrcRecordId, SrcRecIdExactMatch);
        }
        #endregion
        #region Notifications
        public string ExportOIJobSettingNotifications(int ApplicationId, int UserId, string ProviderType)
        {
            return rep.ExportOIJobSettingNotifications(ApplicationId, UserId, ProviderType);
        }
        public void UpdateExportOIJobSettingNotificationsStatus(int ApplicationId, int UserId, string ExportType)
        {
            rep.UpdateExportOIJobSettingNotificationsStatus(ApplicationId, UserId, ExportType);
        }
        #endregion

        public SqlDataReader getTableDetails(string tableName)
        {
            return rep.getTableDetails(tableName);
        }

    }
}
