using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class OISettingRepository : RepositoryParent
    {
        public OISettingRepository(string connectionString) : base(connectionString) { }

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

        internal void UpdateOrbCredentials(OISettingEntity orbEntity, string Section)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.OrbProcessSettingsUpdate";

                sproc.StoredProceduresParameter.Add(GetParam("@Section", Section, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbAPIKey", orbEntity.ORB_API_KEY.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }

        }
        internal void UpdateOrbBackgroundSetting(OISettingEntity orbEntity, string Section)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.OrbProcessSettingsUpdate";
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Section, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_BATCH_SIZE", orbEntity.ORB_BATCH_SIZE.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_BATCH_WAITTIME_SECS", orbEntity.ORB_BATCH_WAITTIME_SECS.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_MAX_PARALLEL_THREADS", orbEntity.ORB_MAX_PARALLEL_THREADS.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PAUSE_ORB_BATCHMATCH_ETL", orbEntity.PAUSE_ORB_BATCHMATCH_ETL.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_ENABLE_CORPORATE_TREE_ENRICHMENT", orbEntity.ORB_ENABLE_CORPORATE_TREE_ENRICHMENT.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateOrbDataImportHandling(OISettingEntity orbEntity, string Section)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.OrbProcessSettingsUpdate";
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Section, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_DATA_IMPORT_DUPLICATE_RESOLUTION", Convert.ToString(orbEntity.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS", Convert.ToString(orbEntity.ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Reset Data
        internal void ResetOISystemData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.ResetOrbData";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion
        #region OI License
        // MP-846 Admin database cleanup and code cleanup.-MASTER
        internal void UpdateOIAPILicenseForMaster(string CustomerSubDomain, string APIKey)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateOIAPILicense";
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerSubDomain", string.IsNullOrEmpty(CustomerSubDomain) ? null : CustomerSubDomain, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIKey", string.IsNullOrEmpty(APIKey) ? null : APIKey, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }

        }
        // MP-846 Admin database cleanup and code cleanup.-CLIENT
        internal void UpdateOIAPILicenseForClients(string CustomerSubDomain, string APIKey)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.UpdateOIAPILicense";
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerSubDomain", string.IsNullOrEmpty(CustomerSubDomain) ? null : CustomerSubDomain, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIKey", string.IsNullOrEmpty(APIKey) ? null : APIKey, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }

        }
        internal DataTable GetOIAPILicense(string CustomerSubDomain = null)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetOIAPILicense";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerSubDomain", string.IsNullOrEmpty(CustomerSubDomain) ? null : CustomerSubDomain, SQLServerDatatype.VarcharDataType));
                DataTable dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Accept From File

        //Add process to import SrcRecordId, InputId, orb_num import match data. 
        internal DataTable GetOIImportDataForAcceptColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetOIImportDataAcceptColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        //Add process to import SrcRecordId, InputId, DUNSNumber for accepting LCM Records. MP-435 
        internal string AcceptLCMDataFromImport(int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.AcceptLCMDataFromImport";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal string AcceptOIMatchDataFromImport(int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.AcceptMatchesFromImport";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        #endregion
        #region Import Data
        internal DataTable GetOIImportDataColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetOIImportDataColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        #endregion

        #region OI Delete From Files
        internal DataTable GetStgInputDataForPurgeColumnName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetStgInputDataForPurgeColumnName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal string DeleteCompanyDataFromImport(int UserId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DeleteCompanyDataFromImport";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        #endregion



        internal DataTable GetAllOrbTags(string LOBTag, string SecurityTags, string UserId)
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.GetStewTags";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SecurityTags", string.IsNullOrEmpty(SecurityTags) ? null : SecurityTags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", string.IsNullOrEmpty(UserId) ? null : UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
