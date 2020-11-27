using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class SettingRepository : RepositoryParent
    {
        public SettingRepository(string connectionString) : base(connectionString) { }



        #region "system setting"
        #region Process Settings
        internal List<SettingEntity> GetCleanseMatchSettings()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetProcessSettings";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SettingAdapter().Adapt(dt);

                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        #endregion
        //Remove UpdateDandBProcessSettings  and merge with UpdateProcessSettings
        internal void UpdateProcessSettings(string Section, string ENABLE_CHAT, string ENABLE_DATA_RESET, string EnablePurgeArchiveProcess, string ArchivePeriodDays, string InactiveDays, string EnablePauseCleanseMatchEtl, string EnablePauseEnrichmentEtl, string DATA_IMPORT_DUPLICATE_RESOLUTION, string TRANSFER_DUNS_AUTO_ENRICH, string TRANSFER_DUNS_AUTO_ENRICH_TAG, string DATA_IMPORT_ERROR_RESOLUTION, string EnrichmentPeriodDays)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateProcessSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Section.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ENABLE_DATA_RESET", string.IsNullOrEmpty(ENABLE_DATA_RESET) ? "" : ENABLE_DATA_RESET, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ENABLE_CHAT", string.IsNullOrEmpty(ENABLE_CHAT) ? "" : ENABLE_CHAT, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PAUSE_ARCHIVE", string.IsNullOrEmpty(EnablePurgeArchiveProcess) ? "" : EnablePurgeArchiveProcess, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ARCHIVE_RETENTION_PERIOD_DAYS", string.IsNullOrEmpty(ArchivePeriodDays) ? "" : ArchivePeriodDays, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@INACTIVITY_PERIOD_USER_LOCKOUT", string.IsNullOrEmpty(InactiveDays) ? "" : InactiveDays, SQLServerDatatype.VarcharDataType));

                sproc.StoredProceduresParameter.Add(GetParam("@PAUSE_CLEANSE_MATCH_ETL", string.IsNullOrEmpty(EnablePauseCleanseMatchEtl) ? "" : EnablePauseCleanseMatchEtl, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PAUSE_ENRICHMENT_ETL", string.IsNullOrEmpty(EnablePauseEnrichmentEtl) ? "" : EnablePauseEnrichmentEtl, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DATA_IMPORT_DUPLICATE_RESOLUTION", string.IsNullOrEmpty(DATA_IMPORT_DUPLICATE_RESOLUTION) ? "" : DATA_IMPORT_DUPLICATE_RESOLUTION, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TRANSFER_DUNS_AUTO_ENRICH", string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH) ? "" : TRANSFER_DUNS_AUTO_ENRICH, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TRANSFER_DUNS_AUTO_ENRICH_TAG", string.IsNullOrEmpty(TRANSFER_DUNS_AUTO_ENRICH_TAG) ? "" : TRANSFER_DUNS_AUTO_ENRICH_TAG, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DATA_IMPORT_ERROR_RESOLUTION", string.IsNullOrEmpty(DATA_IMPORT_ERROR_RESOLUTION) ? "" : DATA_IMPORT_ERROR_RESOLUTION, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ENRICHMENT_STALE_NBR_DAYS", string.IsNullOrEmpty(EnrichmentPeriodDays) ? "" : EnrichmentPeriodDays, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal void UpdateCleanseMatchSettingsAPIFamily(string APIFamily)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateCleanseMatchSettingsAPIFamily";
                sproc.StoredProceduresParameter.Add(GetParam("@APIFamily", APIFamily.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }




        internal List<SettingEntity> GetSystemSettings()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetProcessSettings";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SettingAdapter().Adapt(dt);

                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        internal List<SettingEntity> GetSystemAlerts()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewGetProcessSettings";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new SettingAdapter().Adapt(dt);

                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        // Updates process setting
        internal void UpdateSettings(List<SettingEntity> Settings)
        {
            string InpAsXML = "";
            InpAsXML = "<SettingsUpdate>";
            InpAsXML += "<UserId>" + Environment.UserDomainName + "</UserId>";
            foreach (SettingEntity Setting in Settings)
            {
                InpAsXML += "<Rec><ProcessSettingsID>" + Setting.ProcessSettingsID + "</ProcessSettingsID>";
                if (!String.IsNullOrEmpty(Setting.SettingValue))
                {
                    InpAsXML += "<SettingValue>" + Setting.SettingValue + "</SettingValue>";
                }
                else
                {
                    InpAsXML += "<SettingValue></SettingValue>";
                }

                InpAsXML += "</Rec>";
            }
            InpAsXML += "</SettingsUpdate>";

            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewUpdateProcessSettings";
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = "@inp";
            param._ParameterValue = InpAsXML;
            sproc.StoredProceduresParameter.Add(param);

            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        #endregion

        #region Country

        internal List<CountryEntity> GetCountries()
        {
            List<CountryEntity> results = new List<CountryEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryList";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal int InsertOrUpdateCountryGroup(CountryGroupEntity obj, int UserId)
        {
            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (obj.GroupId > 0)
                {
                    sproc.StoredProcedureName = "dnb.UpdateCountryGroupDetail";
                    sproc.StoredProceduresParameter.Add(GetParam("@GroupId", obj.GroupId.ToString(), SQLServerDatatype.IntDataType));
                }
                else
                {
                    sproc.StoredProcedureName = "dnb.InsertCountryGroupDetail";
                }

                sproc.StoredProceduresParameter.Add(GetParam("@GroupName", obj.GroupName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ISOAlpha2Codes", obj.ISOAlpha2Codes.ToString().TrimEnd(','), SQLServerDatatype.charDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                int rerult = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));

                return rerult;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw;
            }
        }

        internal List<CountryGroupEntity> GetCountryGroupList()
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroupListPaging";
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryGroupAdapter().Adapt(dt);

                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal CountryGroupEntity GetCountryGroupDetailsById(int GroupId)
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroupDetailById";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@GroupId";
                param.ParameterValue = GroupId.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryGroupAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }

        internal CountryGroupEntity GetCountryGroupByName(string GroupName)
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroupByGroupName";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@GroupName";
                param.ParameterValue = GroupName.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryGroupAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }

        internal void DeleteCountryGroup(int GroupId, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteCountryGroupDetail";
                sproc.StoredProceduresParameter.Add(GetParam("@GroupId", GroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }

        }

        internal DataTable GetCountrygroupColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetCountrygroupColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable GetCountryGroupDetail()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroupDetail";
                DataTable dt;
                return dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal string MergeCountryGroup(bool ReplaceExisting)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.MergeCountryGroup";
                sproc.StoredProceduresParameter.Add(GetParam("@ReplaceExisting", ReplaceExisting.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal List<CountryGroupEntity> GetCountryGropus()
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroup";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryGroupAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal DataTable GetCountryGroupsInFilter()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryGroup";
                DataTable dt;
                return dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Users

        internal List<UsersEntity> GetUsersListPaging(string LOBTag)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserListPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<UsersEntity> GetUsersList()
        {
            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserList";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal int GetActiveUsers()
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetActiveUsers";
                count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }
        internal List<UserStatus> GetUserStatus()
        {
            List<UserStatus> results = new List<UserStatus>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserStatus";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().AdaptUserUtil(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<UserStatus> GetUserTypeCode()
        {
            List<UserStatus> results = new List<UserStatus>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserType";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().AdaptUserUtil(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal UsersEntity GetUserDetailsById(int UserId)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserById";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@UserId";
                param.ParameterValue = UserId.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal UsersEntity GetUserDetailsByUserName(string UserName)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserByName";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@UserName";
                param.ParameterValue = UserName.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal string InsertOrUpdateUsersDetails(UsersEntity usersObj, int ModifiedByUserId = 0)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (usersObj.UserId > 0)
                {
                    sproc.StoredProcedureName = "dnb.UpdateUser";
                    sproc.StoredProceduresParameter.Add(GetParam("@UserId", usersObj.UserId.ToString(), SQLServerDatatype.IntDataType));
                }
                else
                {
                    sproc.StoredProcedureName = "dnb.InsertUser";
                }

                sproc.StoredProceduresParameter.Add(GetParam("@UserName", usersObj.UserName.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@LoginId", usersObj.LoginId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserStatusCode", usersObj.UserStatusCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserTypeCode", usersObj.UserTypeCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsApprover", usersObj.IsApprover.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@emailAddress", Convert.ToString(usersObj.EmailAddress), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedByUserId", ModifiedByUserId.ToString(), SQLServerDatatype.IntDataType));

                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(usersObj.Tags) ? usersObj.Tags : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableInvestigations", usersObj.EnableInvestigations.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableSearchByDUNS", usersObj.EnableSearchByDUNS.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableCreateAutoAcceptRules", usersObj.EnableCreateAutoAcceptRules.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangesByAdminPortal", usersObj.ChangesByAdminPortal.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnablePurgeData", usersObj.EnablePurgeData.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(usersObj.LOBTag) ? usersObj.LOBTag : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SSOUser", !string.IsNullOrEmpty(usersObj.SSOUser) ? usersObj.SSOUser : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnablePreviewMatchRules", usersObj.EnablePreviewMatchRules.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TagsInclusive", usersObj.TagsInclusive.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseAllowEnrichment", usersObj.LicenseAllowEnrichment.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableImportData", usersObj.EnableImportData.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableExportData", usersObj.EnableExportData.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnableCompliance", usersObj.EnableCompliance.ToString(), SQLServerDatatype.BitDataType));

                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return Message;
                //throw ex;

            }
        }
        internal void ResetUserPassword(string EmailAddress, string PasswordHash, string SecurityStamp)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.ResetUserPassword";
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", Convert.ToString(EmailAddress), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PasswordHash", Convert.ToString(PasswordHash != null ? PasswordHash.Trim() : null), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SecurityStamp", Convert.ToString(SecurityStamp != null ? SecurityStamp.Trim() : null), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Delete User
        internal string DeleteUser(int UserId, int ModifiedByUserId = 0, bool ChangesByAdminPortal = false)
        {
            string Message = string.Empty;
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedByUserId", ModifiedByUserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangesByAdminPortal", ChangesByAdminPortal.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
                //throw;
            }
        }
        #endregion
        #region Logout User
        internal string ForceUserLogout(int UserId)
        {
            string Message = string.Empty;
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.ForceUserLogout";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
                //throw;
            }
        }
        #endregion
        #region Activate User
        internal string ActivateUser(int UserId, int ModifiedByUserId = 0, bool ChangesByAdminPortal = false)
        {
            string Message = string.Empty;
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ActivateUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedByUserId", ModifiedByUserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangesByAdminPortal", ChangesByAdminPortal.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
                //throw;
            }
        }
        #endregion
        #endregion

        #region "Data Enrichment DnBAPI"

        internal List<DnBAPIGroupEntity> GetDnBAPIGroupList(string LOBTag)
        {
            List<DnBAPIGroupEntity> results = new List<DnBAPIGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDnBAPIGroupList";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag.ToString() : null, SQLServerDatatype.VarcharDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DnBAPIGroupAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<DnbAPIEntity> GetDnBAPIList(string APIType, int credId)
        {
            List<DnbAPIEntity> results = new List<DnbAPIEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDnBAPIList";
                sproc.StoredProceduresParameter.Add(GetParam("@APIType", !string.IsNullOrEmpty(APIType) ? APIType.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", credId > 0 ? credId.ToString() : null, SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DnbAPIAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void InsertOrUpdateDnBAPIDetail(DnBAPIGroupEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (obj.APIGroupId > 0)
                {
                    sproc.StoredProcedureName = "dnb.UpdateDnBAPIDetail";
                    sproc.StoredProceduresParameter.Add(GetParam("@GroupId", obj.APIGroupId.ToString(), SQLServerDatatype.IntDataType));
                }
                else
                {
                    sproc.StoredProcedureName = "dnb.InsertDnBAPIDetail";
                }

                sproc.StoredProceduresParameter.Add(GetParam("@GroupName", obj.APIGroupName.Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPIIds", obj.DnbAPIIds.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(obj.Tags) ? obj.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", obj.CountryGroupId.ToString(), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", obj.CredentialId.ToString(), SQLServerDatatype.SmallintDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal DnBAPIGroupEntity GetAPIGroupDetailById(int GroupId)
        {
            List<DnBAPIGroupEntity> results = new List<DnBAPIGroupEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                //sproc.StoredProcedureName = "dnb.GetAPIGroupDetailById";
                sproc.StoredProcedureName = "dnb.GetDnBAPIGroupList";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@GroupId";
                param.ParameterValue = GroupId.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DnBAPIGroupAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }

        internal void DeleteAPIGroup(int GroupId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteDnbAPIGroupDetail";
                sproc.StoredProceduresParameter.Add(GetParam("@GroupId", GroupId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region "Auto-Acceptance"
        internal List<AutoAdditionalAcceptanceCriteriaEntity> GetAutoAdditionalAcceptanceCriteriaPagedSorted(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, int ConfidenceCode, int CountyGroupId, string Tags, string LOBTag, bool Active)
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            TotalRecords = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAutoAcceptanceCriteria";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCode", ConfidenceCode == 0 ? null : ConfidenceCode.ToString(), SQLServerDatatype.IntDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@MatchGrade", MatchGrade == null ? null : MatchGrade.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GroupId", CountyGroupId == 0 ? null : CountyGroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(Tags) ? null : Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Active", Active.ToString(), SQLServerDatatype.BitDataType));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new AcceptanceCriteriaAdapter().Adapt(dt);
                }
                TotalRecords = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

        internal List<AutoAcceptanceCriteriaDetail> GetAutoAcceptanceCriteriaDetailByGroupId(int CriteriaGroupId)
        {
            List<AutoAcceptanceCriteriaDetail> result = new List<AutoAcceptanceCriteriaDetail>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAutoAcceptanceCriteriaDetailByGroupId";
                sproc.StoredProceduresParameter.Add(GetParam("@CriteriaGroupId", CriteriaGroupId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new AcceptanceCriteriaAdapterDetails().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return result;
        }

        internal AutoAdditionalAcceptanceCriteriaEntity GetAutoAcceptanceDetailByID(int ID)
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetSecondaryAutoAcceptanceCriteriaById";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@CriteriaGroupId ";
                param.ParameterValue = ID.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new AcceptanceCriteriaAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }

        internal DataTable GetSecondaryAutoAcceptanceCriteriaColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetSecondaryAutoAcceptanceCriteriaColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal void DeleteSecondaryAutoAcceptanceCriteria()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteSecondaryAutoAcceptanceCriteria";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal string MergeSecondaryAutoAcceptCriteria(bool ReplaceExisting, int UserId, int CommentId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.MergeSecondaryAutoAcceptCriteria";
                sproc.StoredProceduresParameter.Add(GetParam("@ReplaceExisting", ReplaceExisting.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CommentId", CommentId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
                //throw;
            }
        }
        internal void DeleteAcceptance(string CriteriaGroupId, int UserId, int CommentId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteSecondaryAutoAcceptanceCriteria";
                sproc.StoredProceduresParameter.Add(GetParam("@CriteriaGroupIds", CriteriaGroupId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CommentId", CommentId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }

        }
        internal DataTable GetAutoAcceptanceMatchGrade(string LOBTag)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAutoAcceptanceMatchGrade";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : LOBTag.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void InsertOrUpdateAcceptanceSettings(AutoAdditionalAcceptanceCriteriaEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertUpdateSecondaryAutoAcceptanceCriteria";

                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodes", obj.ConfidenceCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Company", obj.CompanyGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetNo", obj.StreetGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetName", obj.StreetNameGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_City", obj.CityGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_State", obj.StateGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_POBox", obj.AddressGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Phone", obj.PhoneGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_PostalCode", obj.ZipGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Density", obj.Density.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Uniqueness", obj.Uniqueness.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_SIC", obj.SIC.ToString().Trim(), SQLServerDatatype.VarcharDataType));

                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Company", obj.CompanyCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetNo", obj.StreetCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetName", obj.StreetNameCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_City", obj.CityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_State", obj.StateCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_POBox", obj.AddressCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Phone", obj.PhoneCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_PostalCode", obj.ZipCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_DUNS", obj.DUNSCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_SIC", obj.SICCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Density", obj.DensityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Uniqueness", obj.UniquenessCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_NationalID", obj.NationalIDCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_URL", obj.URLCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));

                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(obj.Tags) ? obj.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExcludeFromAutoAccept", obj.ExcludeFromAutoAccept.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", obj.GroupId.ToString().Trim(), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CriteriaGroupId", obj.CriteriaGroupId == 0 ? null : obj.CriteriaGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchGradeComponentCount", obj.MatchGradeComponentCount == 0 ? null : obj.MatchGradeComponentCount.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyScore", obj.CompanyScore == 0 ? null : obj.CompanyScore.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchDataCriteria", Convert.ToString(obj.MatchDataCriteria), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OperatingStatus", Convert.ToString(obj.OperatingStatus), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BusinessType", Convert.ToString(obj.BusinessType), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SingleCandidateMatchOnly", obj.SingleCandidateMatchOnly.ToString().Trim(), SQLServerDatatype.BitDataType));


                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal DataTable GetTopMatchGradeSettings(bool IsTopMatch)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[StewGetLCMMatchGrades]";
                sproc.StoredProceduresParameter.Add(GetParam("@Top1Match", IsTopMatch.ToString(), SQLServerDatatype.BitDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal int GetSecondaryAutoAcceptanceCriteriaGroupCount()
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetSecondaryAutoAcceptanceCriteriaGroupCount";
                count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }
        internal void RunAutoAcceptanceRule(int UserId)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.StewAutoAcceptMatches";
            sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }
        internal List<AutoAdditionalAcceptanceCriteriaEntity> GetAutoAdditionalAcceptanceCriteria()
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAutoAcceptanceCriteria";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new AcceptanceCriteriaAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        internal void UpdateSequence(int settingID, int sequenceNo)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateSecondaryAutoAcceptanceCriteriaSequence";
                sproc.StoredProceduresParameter.Add(GetParam("@CriteriaId", settingID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SequenceNo", sequenceNo.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal void ManageSecondaryAutoAcceptanceCriteriaActivation(int GroupId, bool Activate)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ManageSecondaryAutoAcceptanceCriteriaActivation";
                sproc.StoredProceduresParameter.Add(GetParam("@GroupId", GroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Activate", Activate.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }

        }
        #region "Preview Auto-Acceptance"
        internal List<PreviewAutoAcceptanceRuleEntity> GetStewPreviewAutoAcceptanceRule(AutoAdditionalAcceptanceCriteriaEntity obj, bool ApplyRule, bool Export)
        {
            List<PreviewAutoAcceptanceRuleEntity> result = new List<PreviewAutoAcceptanceRuleEntity>();
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewPreviewAutoAcceptanceRule";

                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodes", obj.ConfidenceCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Company", obj.CompanyGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetNo", obj.StreetGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetName", obj.StreetNameGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_City", obj.CityGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_State", obj.StateGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_POBox", obj.AddressGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Phone", obj.PhoneGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_PostalCode", obj.ZipGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Density", obj.Density.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Uniqueness", obj.Uniqueness.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_SIC", obj.SIC.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Company", obj.CompanyCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetNo", obj.StreetCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetName", obj.StreetNameCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_City", obj.CityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_State", obj.StateCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_POBox", obj.AddressCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Phone", obj.PhoneCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_PostalCode", obj.ZipCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_DUNS", obj.DUNSCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_SIC", obj.SICCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Density", obj.DensityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Uniqueness", obj.UniquenessCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_NationalID", obj.NationalIDCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_URL", obj.URLCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(obj.Tags) ? obj.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExcludeFromAutoAccept", obj.ExcludeFromAutoAccept.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", obj.GroupId.ToString().Trim(), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchGradeComponentCount", obj.MatchGradeComponentCount == 0 ? "0" : obj.MatchGradeComponentCount.ToString().Trim(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplyRule", ApplyRule.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Export", Export.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyScore", obj.CompanyScore == 0 ? "0" : obj.CompanyScore.ToString().Trim(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchDataCriteria", Convert.ToString(obj.MatchDataCriteria), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OperatingStatus", Convert.ToString(obj.OperatingStatus), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BusinessType", Convert.ToString(obj.BusinessType), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SingleCandidateMatchOnly", obj.SingleCandidateMatchOnly.ToString().Trim(), SQLServerDatatype.BitDataType));

                //sproc.StoredProceduresParameter.Add(GetParam("@CriteriaGroupId", obj.CriteriaGroupId == 0 ? null : obj.CriteriaGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                if (ApplyRule)
                {
                    sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                    return result;
                }
                else
                {
                    dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new PreviewAutoAcceptanceRuleAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return result;
        }
        internal DataTable ExportStewPreviewAutoAcceptanceRule(AutoAdditionalAcceptanceCriteriaEntity obj, bool ApplyRule, bool Export)
        {
            List<PreviewAutoAcceptanceRuleEntity> result = new List<PreviewAutoAcceptanceRuleEntity>();
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewPreviewAutoAcceptanceRule";

                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodes", obj.ConfidenceCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Company", obj.CompanyGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetNo", obj.StreetGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetName", obj.StreetNameGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_City", obj.CityGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_State", obj.StateGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_POBox", obj.AddressGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Phone", obj.PhoneGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_PostalCode", obj.ZipGrade.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Density", obj.Density.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Uniqueness", obj.Uniqueness.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_SIC", obj.SIC.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Company", obj.CompanyCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetNo", obj.StreetCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_StreetName", obj.StreetNameCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_City", obj.CityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_State", obj.StateCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_POBox", obj.AddressCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Phone", obj.PhoneCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_PostalCode", obj.ZipCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_DUNS", obj.DUNSCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_SIC", obj.SICCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Density", obj.DensityCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Uniqueness", obj.UniquenessCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_NationalID", obj.NationalIDCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_URL", obj.URLCode.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(obj.Tags) ? obj.Tags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExcludeFromAutoAccept", obj.ExcludeFromAutoAccept.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", obj.GroupId.ToString().Trim(), SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchGradeComponentCount", obj.MatchGradeComponentCount == 0 ? "0" : obj.MatchGradeComponentCount.ToString().Trim(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplyRule", ApplyRule.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Export", Export.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyScore", obj.CompanyScore == 0 ? "0" : obj.CompanyScore.ToString().Trim(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchDataCriteria", Convert.ToString(obj.MatchDataCriteria), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OperatingStatus", Convert.ToString(obj.OperatingStatus), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BusinessType", Convert.ToString(obj.BusinessType), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SingleCandidateMatchOnly", obj.SingleCandidateMatchOnly.ToString().Trim(), SQLServerDatatype.BitDataType));

                //sproc.StoredProceduresParameter.Add(GetParam("@CriteriaGroupId", obj.CriteriaGroupId == 0 ? null : obj.CriteriaGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                if (ApplyRule)
                {
                    sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                    return dt;
                }
                else
                {
                    dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        #endregion
        #endregion

        #region "Reset Data"
        internal void ResetSystemData()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ResetSystemData";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "Environment"
        internal DataTable GetAllCDSEnvironment(bool isPaging)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCDSEnvironment";
                sproc.StoredProceduresParameter.Add(GetParam("@isPaging", isPaging.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetCDSEnvironmentName(bool isPaging)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetCDSEnvironment]";
                sproc.StoredProceduresParameter.Add(GetParam("@isPaging", isPaging.ToString(), SQLServerDatatype.BitDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        public DataTable GetEnvironmentByName(string OrganizationUrl)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetCDSEnvironmentByOrganizationUrl]";
                sproc.StoredProceduresParameter.Add(GetParam("@OrganizationUrl", OrganizationUrl.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        #endregion

        #region "Entity"
        internal string InsertCDSEntity(string Entity)
        {
            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertCDSEntity";
                sproc.StoredProceduresParameter.Add(GetParam("@Entity", Convert.ToString(Entity), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                //throw ex;
            }
            return message;
        }
        internal DataTable GetAllCDSEntity()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllCDSEntity";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal DataTable GetCDSEntityListPaging(int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            TotalRecords = 0;
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCDSEntityListPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                TotalRecords = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal void DeleteCDSEntity(string Entity)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteCDSEntity";
                sproc.StoredProceduresParameter.Add(GetParam("@Entity", Entity.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "SAML SSO"
        internal DataTable getSAMLSSOSetting()
        {
            DataTable result = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.getSAMLSSOSettingByApplicationId";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                result = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return result;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal string UpdateSAMLSSOSettings(SAMLSSOSettingEntity SamlSettingObj)
        {
            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateSAMLSSOSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Convert.ToString(SamlSettingObj.SamlId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SPName", Convert.ToString(SamlSettingObj.SPName), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SPAssertionConsumerServiceUrl", Convert.ToString(SamlSettingObj.SPAssertionConsumerServiceUrl), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SPLocalCertificateFile", Convert.ToString(SamlSettingObj.SPLocalCertificateFile), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SPPassword", Convert.ToString(SamlSettingObj.SPPassword), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IDPName", Convert.ToString(SamlSettingObj.IDPName), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IDPSignAuthnRequest", Convert.ToString(SamlSettingObj.IDPSignAuthnRequest), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IDPSingleSignOnServiceUrl", Convert.ToString(SamlSettingObj.IDPSingleSignOnServiceUrl), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IDPSingleLogoutServiceUrl", Convert.ToString(SamlSettingObj.IDPSingleLogoutServiceUrl), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IDPPartnerCertificateFile", Convert.ToString(SamlSettingObj.IDPPartnerCertificateFile), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                //throw ex;
            }
            return message;
        }
        #endregion

        #region "Export and Download Data"
        internal DataTable GetExportTablesApiName(string ExportCategory)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetExportTablesApiName";
                sproc.StoredProceduresParameter.Add(GetParam("@ExportCategory ", ExportCategory.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal SqlDataReader ExportDataEnrichmentDataReader(string Tags, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, string ExportCategory = "Enrichment")
        {
            SqlDataReader reader;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.ExportDataEnrichment";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", !string.IsNullOrEmpty(Tags) ? Tags.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportCategory ", ExportCategory.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                reader = sql.ExecuteDataReader(CommandType.StoredProcedure, sproc);
                return reader;
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal void FinalizeDataEnrichmentExport(bool FlagExport)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.FinalizeDataEnrichmentExport";
                sproc.StoredProceduresParameter.Add(GetParam("@FlagExport", FlagExport.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetExportTablesDetails(string APIName, string ExportCategory)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetExportTablesDetails";
                sproc.StoredProceduresParameter.Add(GetParam("@APIName", APIName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExportCategory", ExportCategory.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "API Credentials"
        internal DataTable GetLicenseSetting(string Url)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetLicenseDetailBySubDomain";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@AppicationSubDomain", Url, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal OverrideAPICredentialsEntity GetOverrideAPICredentials()
        {
            List<OverrideAPICredentialsEntity> results = new List<OverrideAPICredentialsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetOverrideAPICredentials";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OverrideAPICredentialsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results.FirstOrDefault();
        }
        internal string UpdateOverrideAPICredentials(OverrideAPICredentialsEntity obj)
        {
            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateOverrideAPICredentials";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Convert.ToString(obj.Id), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIKey", Convert.ToString(obj.APIKeyOverride), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APISecret", Convert.ToString(obj.APISecretOverride), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", Convert.ToString(obj.APILayerOverride), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BatchSize", Convert.ToString(obj.BatchSizeOverride), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@WaitTimesBetweenBatch", Convert.ToString(obj.WaitTimesBetweenBatchOverride), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MaxParallelThreads", Convert.ToString(obj.MaxParallelThreadsOverride), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UseForCleanseMatch", Convert.ToString(obj.UseForCleanseMatchOverride), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UseForEnrich", Convert.ToString(obj.UseForEnrichOverride), SQLServerDatatype.BitDataType));


                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                //throw ex;
            }
            return message;
        }


        #endregion

        #region "Machine Detail"
        internal void InsertUpdateUserMachineDetails(UserMachineEntity objUserMachine)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.InsertUpdateUserMachineDetails";

                sproc.StoredProceduresParameter.Add(GetParam("@Id", objUserMachine.Id.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objUserMachine.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MachineDetails", objUserMachine.MachineDetails.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CreatedDateTime", objUserMachine.CreatedDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LastLoggedinDateTime", objUserMachine.LastLoggedinDateTime.ToString(), SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BrowserAgent", objUserMachine.BrowserAgent.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal UserMachineEntity GetUserMachineDetails(int UserId, string MachineDetails)
        {
            List<UserMachineEntity> results = new List<UserMachineEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.GetUserMachineDetails";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MachineDetails", MachineDetails.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UserMachineAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results.FirstOrDefault();
        }
        internal void DeleteUserMachineDetails(int UserId, string BrowserAgent)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.DeleteUserMachineDetails";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BrowserAgent", BrowserAgent.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void DeleteUserMachineRecords(string MachineDetails, string BrowserAgent)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.DeleteUserMachineRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@MachineDetails", MachineDetails.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@BrowserAgent", BrowserAgent.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "Import Data"
        internal DataTable GetInpCompanyColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetInpCompanyColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal DataTable GetImportDataRefreshColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetImportDataRefreshColumnsName]";
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

        #region "Match Grades"
        internal List<MatchGradeEntity> GetMatchGrades()
        {
            List<MatchGradeEntity> results = new List<MatchGradeEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMatchGrades";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MatchGradeAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        internal List<MatchCodeEntity> GetMatchMDPCodes(string matchFieldType)
        {
            List<MatchCodeEntity> results = new List<MatchCodeEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMDPCodeByMatchField";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@MatchField";
                param.ParameterValue = matchFieldType;
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MatchCodeAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        #endregion

        internal void InsertOrUpdateUsersImage(int userId, string imagePath)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (userId > 0)
                {
                    sproc.StoredProcedureName = "dnb.InsertOrUpdateUserImage";
                    sproc.StoredProceduresParameter.Add(GetParam("@UserId", userId.ToString(), SQLServerDatatype.IntDataType));
                }

                sproc.StoredProceduresParameter.Add(GetParam("@ImagePath", Convert.ToString(imagePath != null ? imagePath.Trim() : null), SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@ImagePath",  imagePath.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal DataTable GetActiveUsersCount()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetActiveUsersCount]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal void StewUserActivityHeartbeat(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewUserActivityHeartbeat";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable GetUserLoginActivity(int UserId)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetUserLoginActivity]";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal List<UserStatus> GetAttributeTypeForLogIn()
        {
            List<UserStatus> results = new List<UserStatus>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAttributeTypeForLogIn";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UsersAdapter().AdaptUserUtil(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal DataTable GetLanguageCodes()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetLanguageCodes";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());

                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal int GetMatchOutputCountByTag(string Tag)
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMatchOutputCountByTag";
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(Tag) ? null : Tag.ToString(), SQLServerDatatype.VarcharDataType));
                count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return count;
        }
        internal void UpdateMultipleUserStatus(string UserIds, string Status)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateMultipleUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserIds", UserIds.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Status", Status.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal string GetURLEncode(string SrcRecordId = "", string CompanyName = "", string Address0 = "", string Address1 = "", string PrimaryTownName = "", string TerritoryName = "", string FullPostalCode = "", string CountryISOAlpha2Code = "", string TelephoneNumber = "", string ExclusionText = "", string inLanguage = "", string APIFamily = "", int ConfidenceCodeLowerLevelThreshold = 0, string DUNSNumber = null, string Domain = null, string Email = null, string InputId = null, string RegistrationNumber = "")
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDnBCleanseMatchURL";
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", string.IsNullOrEmpty(CompanyName) ? "" : CompanyName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StreetAddressLine0", string.IsNullOrEmpty(Address0) ? "" : Address0, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StreetAddressLine1", string.IsNullOrEmpty(Address1) ? "" : Address1, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryTownName", string.IsNullOrEmpty(PrimaryTownName) ? "" : PrimaryTownName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TerritoryName", string.IsNullOrEmpty(TerritoryName) ? "" : TerritoryName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FullPostalCode", string.IsNullOrEmpty(FullPostalCode) ? "" : FullPostalCode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISOAlpha2Code", string.IsNullOrEmpty(CountryISOAlpha2Code) ? "" : CountryISOAlpha2Code, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TelephoneNumber", string.IsNullOrEmpty(TelephoneNumber) ? "" : TelephoneNumber, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ExclusionText", string.IsNullOrEmpty(ExclusionText) ? "" : ExclusionText, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@inLanguage", string.IsNullOrEmpty(inLanguage) ? "" : inLanguage, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIFamily", string.IsNullOrEmpty(APIFamily) ? "" : APIFamily, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodeLowerLevelThreshold", ConfidenceCodeLowerLevelThreshold.ToString(), SQLServerDatatype.TinyintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", DUNSNumber, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Domain", Domain, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Email", Email, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RegistrationNumber", RegistrationNumber, SQLServerDatatype.VarcharDataType));
                //DataTable dt = new DataTable();
                string strValue = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString()));
                return strValue;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void UpdateDefaultPageSize(int UserId, string Section, int PageSize = 10)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateDefaultPageSize";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Convert.ToString(Section), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal int GetDefaultPageSize(int UserId, string Section)
        {
            int count = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDefaultPageSize";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Section", Convert.ToString(Section), SQLServerDatatype.VarcharDataType));
                count = (Int32)sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return count;
        }
        internal string UpdateUserAttemptCountDetail(int UserId)
        {
            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateUserAttemptCountDetail";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                //throw ex;
            }
            return message;
        }

        #region  "No references found"
        internal DataTable GetAutoAdditionalAcceptanceCriteriaExportToExcel(bool LicenseEnableTags, int ConfidenceCode, string MatchGrade, int CountyGroupId, string Tags)
        {
            DataTable results = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetAutoAcceptanceCriteriaExportToExcel]";
                sproc.StoredProceduresParameter.Add(GetParam("@LicenseEnableTags", LicenseEnableTags.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCode", ConfidenceCode == 0 ? null : ConfidenceCode.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchGrade", string.IsNullOrEmpty(MatchGrade) ? null : MatchGrade.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GroupId", CountyGroupId == 0 ? null : CountyGroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(Tags) ? null : Tags.ToString(), SQLServerDatatype.VarcharDataType));
                results = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
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



        internal DataTable GetAcceptedBy()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetAcceptedBy]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        #region "Stewardship Portal(Match Data)"
        #region "Reject From FIle"
        internal DataTable GetRejectPurgeColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetRejectPurgeColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal string RejectPurgeDataFromImport(int UserId, bool IsPurgeData)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.RejectPurgeDataFromImport";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Purge", IsPurgeData.ToString(), SQLServerDatatype.BitDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        #endregion
        #region Accept From File
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


        internal DataTable GetImportDataForAcceptColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetImportDataAcceptColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        #endregion

        #region "De-Duplicate Data"
        internal string RemoveDuplicateRecords(int UserId, string Tag, string LOBTag, string CountryCode, int CountryGroupId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.RemoveDuplicateRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(Tag) ? null : Convert.ToString(Tag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", string.IsNullOrEmpty(CountryCode) ? null : Convert.ToString(CountryCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId == 0 ? null : CountryGroupId.ToString(), SQLServerDatatype.SmallintDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        #endregion
        #endregion
        #region "Monitoring DnB Direct Plus"
        internal void DPMInsertRegistration(string reference, string Tags, bool notificationsSuppressed, string productId, string versionId, string email, string fileTransferProfile, string description, string deliveryTrigger, string deliveryFrequency, int dunsCount, bool seedData, int CredentialId, string blockIds)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMInsertRegistration]";
                sproc.StoredProceduresParameter.Add(GetParam("@RegistrationName", string.IsNullOrEmpty(reference) ? null : reference, SQLServerDatatype.VarcharDataType));
                //sproc.StoredProceduresParameter.Add(GetParam("@Tags", Convert.ToString(Tags), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@notificationsSuppressed", notificationsSuppressed.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@productId", string.IsNullOrEmpty(productId) ? null : productId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@versionId", string.IsNullOrEmpty(versionId) ? null : versionId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@email", string.IsNullOrEmpty(email) ? null : email, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@fileTransferProfile", string.IsNullOrEmpty(fileTransferProfile) ? null : fileTransferProfile, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@description", string.IsNullOrEmpty(description) ? "" : description, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@deliveryTrigger", string.IsNullOrEmpty(deliveryTrigger) ? null : deliveryTrigger, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@deliveryFrequency", string.IsNullOrEmpty(deliveryFrequency) ? null : deliveryFrequency, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@dunsCount", dunsCount.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@seedData", seedData.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@blockIds", !string.IsNullOrEmpty(blockIds) ? blockIds : null, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal void DPMUpdateRegistration(string MonitoringRegistrationName, string Tags)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[dnb].[DPMUpdateRegistration]";
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", string.IsNullOrEmpty(Tags) ? null : Tags, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName, SQLServerDatatype.VarcharDataType));

                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal DataTable DPMGetRegistration()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetRegistration]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
            return dt;
        }
        internal void DPMAddDUNSRegistrationsToList()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMAddDUNSRegistrationsToList]";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal void DPMMergeDUNSRegistrations()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMMergeDUNSRegistrations]";
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal void DPMInsertRegisteredDUNS(string MonitoringRegistrationName, string DnBDUNSNumber, string FileDateTime, string FileType)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[dnb].[DPMInsertRegisteredDUNS]";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", DnBDUNSNumber, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileDateTime", FileDateTime, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileType", FileType, SQLServerDatatype.VarcharDataType));

                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal void DPMInsertNotification(int NotificationFileId, string NotificationFileName, string FileDate, string MonitoringRegistrationName, string FileType, string ResponseJSON, string ProcessStatusId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMInsertNotification]";
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationFileId", NotificationFileId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationFileName", NotificationFileName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileDate", FileDate, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FileType", FileType, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", ResponseJSON, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ProcessStatusId", ProcessStatusId, SQLServerDatatype.TinyintDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }
        internal void DPMFinalizeFileLoad(int NotificationFileId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMFinalizeFileLoad]";
                sproc.StoredProceduresParameter.Add(GetParam("@NotificationFileId", NotificationFileId.ToString(), SQLServerDatatype.IntDataType));

                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Empty catch block to stop from breaking
            }
        }

        internal DataTable DPMGetRegistrationNamesForDUNSRegistration()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetRegistrationNamesForDUNSRegistration]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable DPMGetDUNSForRegistration(string MonitoringRegistrationName)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetDUNSForRegistration]";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName, SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable GetDPMDunsRegistrationByRegistrationName(string MonitoringRegistrationName)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetDunsRegistrationByRegistrationName]";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName, SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal DataTable GetDPMDunsRegistrationList(string RegistrationName, string DnBDUNSNumber, int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            DataTable dt;
            TotalCount = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetDPMDunsRegistrationList]";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", RegistrationName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", DnBDUNSNumber.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                TotalCount = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal void DeleteAllDPMRegistration(string NotDeleteRegistration, int CredentialId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMDeleteAllRegistration]";
                sproc.StoredProceduresParameter.Add(GetParam("@NotDeleteRegistration", NotDeleteRegistration, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CredentialId", CredentialId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal void SuppressUnSupressNotifications(string referenceName, bool isSuprressed)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMSuppressUnSupressRegistration]";
                sproc.StoredProceduresParameter.Add(GetParam("@referenceName", referenceName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@isSuprressed", isSuprressed.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataTable checkFileExists(string FileName)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMCheckFileLoad]";
                sproc.StoredProceduresParameter.Add(GetParam("@FileName", FileName.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal DataTable GetDPMRegistrationByName(string MonitoringRegistrationName)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DPMGetRegistrationByName]";
                sproc.StoredProceduresParameter.Add(GetParam("@MonitoringRegistrationName", MonitoringRegistrationName.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }


        #endregion

        #region "OI"
        internal string GetOICleanseMatchURLEncode(string CustomerSubDomain = "", string Country = "", string SrcRecordId = "", string InputId = "", string CompanyName = "", string address1 = "", string address2 = "", string city = "", string state = "", string PostalCode = "", string TelephoneNumber = "", string ExclusionText = "", string CEOName = "", string Domain = "", string Email = "", string InpOrbNum = "", string EIN = "")
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.GetOICleanseMatchURL";
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerSubDomain", CustomerSubDomain, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@srcRecordId", string.IsNullOrEmpty(SrcRecordId) ? "" : SrcRecordId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", string.IsNullOrEmpty(CompanyName) ? "" : CompanyName, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StreetAddressLine0", string.IsNullOrEmpty(address1) ? "" : address1, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@StreetAddressLine1", string.IsNullOrEmpty(address2) ? "" : address2, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrimaryTownName", string.IsNullOrEmpty(city) ? "" : city, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TerritoryName", string.IsNullOrEmpty(state) ? "" : state, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FullPostalCode", string.IsNullOrEmpty(PostalCode) ? "" : PostalCode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISOAlpha2Code", string.IsNullOrEmpty(Country) ? "" : Country, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TelephoneNumber", string.IsNullOrEmpty(TelephoneNumber) ? "" : TelephoneNumber, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CEOName", string.IsNullOrEmpty(CEOName) ? "" : CEOName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Domain", Domain.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Email", Email, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InpOrbNum", InpOrbNum, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EIN", EIN, SQLServerDatatype.VarcharDataType));

                //DataTable dt = new DataTable();
                string strValue = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString()));
                return strValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        internal DataTable GetLicensedAPIType()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetLicensedAPIType";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        #region User Delete
        internal string DeleteUserAfterReassign(int UserId, int ModifiedByUserId, bool ChangesByAdminPortal, int ReassignToUserId)
        {
            string result = string.Empty;
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            try
            {
                sproc.StoredProcedureName = "dnb.DeleteUser";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ModifiedByUserId", ModifiedByUserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ChangesByAdminPortal", ChangesByAdminPortal.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ReassignToUserId", ReassignToUserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return result;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return result;
            }
        }
        #endregion

        internal void UpdateSoloProcessSettings(string settingName, string settingValue)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateSoloProcessSettings";
                sproc.StoredProceduresParameter.Add(GetParam("@SettingName", settingName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SettingValue", settingValue, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}
