using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class OIExportJobSettingsRepository : RepositoryParent
    {
        public OIExportJobSettingsRepository(string connectionString) : base(connectionString) { }

        internal List<OIExportJobSettingsEntity> GetExportJobSettings()
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettings";  // MP-846 Admin database cleanup and code cleanup.
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OIExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal int InserExportJobSettings(OIExportJobSettingsEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InserExportJobSettings";     // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(obj.Tags) ? null : obj.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Input", string.IsNullOrEmpty(obj.Input) ? null : obj.Input.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(obj.LOBTag) ? null : obj.LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchOutPut", obj.MatchOutPut.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Enrichment", obj.Enrichment.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ActiveDataQueue", obj.ActiveDataQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", obj.MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Format", obj.Format == null ? null : obj.Format.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(obj.Delimiter) ? null : obj.Delimiter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecID", string.IsNullOrEmpty(obj.SrcRecID) ? null : obj.SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsExactatch", string.IsNullOrEmpty(obj.SrcRecIdExactMatch.ToString()) ? "false" : obj.SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileName", string.IsNullOrEmpty(obj.FilePath.ToString()) ? "" : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LCMQueue", obj.LCMQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NoMatchQueue", obj.NoMatchQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TrasferedDuns", obj.TrasferedDuns.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", obj.ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyTree", obj.CompanyTree.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", obj.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasTextQualifierToAll", obj.HasTextQualifierToAll.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        internal int UpdateExportJobDetails(OIExportJobSettingsEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJob";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Input", Convert.ToString(obj.Input), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", string.IsNullOrEmpty(obj.FilePath) ? null : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchOutPut", obj.MatchOutPut.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Enrichment", obj.Enrichment.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TrasferedDuns", obj.TrasferedDuns.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ActiveDataQueue", obj.ActiveDataQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LCMQueue", obj.LCMQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NoMatchQueue", obj.NoMatchQueue.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsMonitoringNotifications", obj.IsMonitoringNotifications.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Format", obj.Format.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", string.IsNullOrEmpty(obj.APILayer) ? null : Convert.ToString(obj.APILayer), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delimiter", string.IsNullOrEmpty(obj.Delimiter) ? null : Convert.ToString(obj.Delimiter), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecID", string.IsNullOrEmpty(obj.SrcRecID) ? null : Convert.ToString(obj.SrcRecID), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsExactMatch", obj.SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(obj.Tags) ? null : obj.Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(obj.LOBTag) ? null : obj.LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestedDate", obj.RequestedDate != DateTime.MinValue ? obj.RequestedDate.ToString() : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessStartDate", obj.ProcessStartDate != DateTime.MinValue ? obj.ProcessStartDate.ToString() : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessEndDate", obj.ProcessEndDate != DateTime.MinValue ? obj.ProcessEndDate.ToString() : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", obj.MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", obj.IsProcessComplete.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsAlreadyDownloaded", obj.IsAlreadyDownloaded.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEmailSent", obj.IsEmailSent.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LastDownloadedDate", obj.LastDownloadedDate != DateTime.MinValue ? obj.LastDownloadedDate.ToString() : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDeleted", obj.IsDeleted.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsCancelled", obj.IsCancelled.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsAlreadyNotify", obj.IsAlreadyNotify.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@HasHeader", obj.HasHeader.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
        internal void UpdateExportJobSettings(OIExportJobSettingsEntity obj, bool IsPorcessStart, bool IsRevert)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", obj.IsProcessComplete.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", obj.FilePath == null ? null : obj.FilePath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsPorcessStart", IsPorcessStart.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsRevert", IsRevert.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateExportJobSettingsSentMail(OIExportJobSettingsEntity obj, bool IsEmailSent)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateExportJobSettingsSentMail";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsEmailSent", IsEmailSent.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal List<OIExportJobSettingsEntity> GetExportJobSettingsByUserId(string ExportType, int? UserId, int ApplicationId)
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettingsByUserId";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", !string.IsNullOrEmpty(Convert.ToString(UserId)) ? UserId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OIExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal void DeleteOIExportJobSettings(OIExportJobSettingsEntity obj)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.DeleteExportJobSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));


                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal OIExportJobSettingsEntity GetExportJobSettingsById(int Id)
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            try
            {
                DataTable dt;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetExportJobSettingsById";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OIExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results.FirstOrDefault();
        }
        internal void UpdateExportJobSettingsForDownload(int Id, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateExportJobSettingsForDownload";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal string IsExistExportFileName(string fileName, int Applicationid)
        {
            string result = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.IsExistExportFileName";      // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@FilePath", fileName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", Applicationid.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal List<OIExportJobSettingsEntity> GetPurgeExportedFiles()
        {
            List<OIExportJobSettingsEntity> results = new List<OIExportJobSettingsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetPurgeExportedFiles";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OIExportJobSettingsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void CancelExportJobSettings(OIExportJobSettingsEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.CancelExportJobSettings";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@Id", obj.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", obj.ApplicationId.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal SqlDataReader ExportCompanyFirmographicsDataReader(bool MarkAsExported)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportCompanyFirmographics";
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {

                throw;
            }
        }


        internal SqlDataReader ExportCompanyTreeDataReader(bool MarkAsExported)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportCompanyTree";
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //internal DataTable ExportActiveRecords(string SrcRecordId, string ImportProcess, string Tag)
        //{
        //    try
        //    {
        //        StoredProcedureEntity sproc = new StoredProcedureEntity();
        //        sproc.StoredProcedureName = "oi.ExportActiveRecords";
        //        sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecordId) ? null : SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
        //        sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
        //        sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));

        //        DataTable dt;
        //        dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
        //        return dt;
        //    }
        //    catch (Exception Ex)
        //    {

        //        throw;
        //    }
        //}
        internal SqlDataReader ExportActiveDataQueue(int UserId, string SrcRecordId, string LOBTag, string Tag, string ImportProcess)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportActiveDataQueue";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecordId) ? null : SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal DataTable ExportActiveRecordsToExcel(string InputId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportActiveRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId != null ? InputId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal DataTable ExportActiveRecordToExcel(OIExportToExcel Model)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportActiveRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Model.UserId == 0 ? null : Model.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", Model.SrcRecordId != null ? Model.SrcRecordId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City ", Model.City != null ? Model.City.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State ", Model.State != null ? Model.State.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryCode ", Model.CountryCode != null ? Model.CountryCode.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess ", Model.ImportProcess != null ? Model.ImportProcess.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", Model.CountryGroupId == 0 ? null : Model.CountryGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag ", Model.Tag != null ? Model.Tag.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportWithCandidates", Model.ExportWithCandidates.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportWithoutCandidates", Model.ExportWithoutCandidates.ToString().Trim(), SQLServerDatatype.BitDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal SqlDataReader ExportActiveRecordsDataReader(string SrcRecID, string ImportProcess, string Tag)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportActiveRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region OI Export Matched Data
        internal SqlDataReader ExportMatchedDataReader(bool MarkAsExported, string LOBTag, string Tag, string ImportProcess, string SrcRecordId, bool SrcRecIdExactMatch)
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.ExportMatchedData";
                sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tag) ? Tag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecordId) ? null : SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void FinalizeMatchExport(bool MarkAsExported)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "oi.FinalizeMatchExport";
            sproc.StoredProceduresParameter.Add(GetParam("@MarkAsExported", MarkAsExported.ToString(), SQLServerDatatype.BitDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        #endregion

        #region Notifications
        internal string ExportOIJobSettingNotifications(int ApplicationId, int UserId, string ProviderType)
        {
            try
            {
                string results = "";
                DataTable dt = new DataTable();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.ExportJobSettingNotifications";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderType", ProviderType.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(results))
                        {
                            results = dt.Rows[i]["FilePath"].ToString();
                        }
                        else
                        {
                            results += ", " + dt.Rows[i]["FilePath"].ToString();
                        }
                    }
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateExportOIJobSettingNotificationsStatus(int ApplicationId, int UserId, string ExportType)
        {

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateExportJobSettingNotificationsStatus";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ApplicationId", ApplicationId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportType", ExportType.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                //result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal SqlDataReader getTableDetails(string TableName)
        {
            SqlDataReader reader;
            try
            {
                using (var connection = new SqlConnection())
                {
                    string queryString = "SELECT * FROM " + TableName;
                    var command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    reader = command.ExecuteReader();
                }
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion



        #region "Common Method"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        #endregion
    }
}
