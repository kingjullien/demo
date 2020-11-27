using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class SettingFacade : FacadeParent
    {
        SettingBusiness rep;
        public SettingFacade(string connectionString) : base(connectionString) { rep = new SettingBusiness(this.Connection); }


        #region "system setting"
        public DataTable GetActiveUsersCount()
        {
            return rep.GetActiveUsersCount();
        }
        #region Process Settings
        public List<SettingEntity> GetCleanseMatchSettings()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            results = rep.GetCleanseMatchSettings();
            return results;
        }
        #endregion
        //Remove UpdateDandBProcessSettings  and merge with UpdateProcessSettings
        public void UpdateProcessSettings(string Section, string ENABLE_CHAT, string ENABLE_DATA_RESET, string EnablePurgeArchiveProcess, string ArchivePeriodDays, string InactiveDays, string EnablePauseCleanseMatchEtl, string EnablePauseEnrichmentEtl, string DATA_IMPORT_DUPLICATE_RESOLUTION, string TRANSFER_DUNS_AUTO_ENRICH, string TRANSFER_DUNS_AUTO_ENRICH_TAG, string DATA_IMPORT_ERROR_RESOLUTION, string EnrichmentPeriodDays)
        {
            rep.UpdateProcessSettings(Section, ENABLE_CHAT, ENABLE_DATA_RESET, EnablePurgeArchiveProcess, ArchivePeriodDays, InactiveDays, EnablePauseCleanseMatchEtl, EnablePauseEnrichmentEtl, DATA_IMPORT_DUPLICATE_RESOLUTION, TRANSFER_DUNS_AUTO_ENRICH, TRANSFER_DUNS_AUTO_ENRICH_TAG, DATA_IMPORT_ERROR_RESOLUTION, EnrichmentPeriodDays);
        }

        public void UpdateCleanseMatchSettingsAPIFamily(string APIFamily)
        {
            rep.UpdateCleanseMatchSettingsAPIFamily(APIFamily);
        }
        public List<SettingEntity> GetSystemSettings()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            results = rep.GetSystemSettings();
            return results;
        }
        public List<SettingEntity> GetSystemAlerts()
        {
            List<SettingEntity> results = new List<SettingEntity>();
            results = rep.GetSystemAlerts();
            return results;
        }
        // Updates process setting
        public void UpdateCleanseMatchSettings(List<SettingEntity> Settings)
        {
            rep.UpdateSettings(Settings);
        }
        #endregion

        #region Group Country
        public List<CountryGroupEntity> GetCountryGroup()
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            results = rep.GetCountryGroups();
            return results;
        }
        public DataTable GetCountryGroupsInFilter()
        {
            return rep.GetCountryGroupsInFilter();
        }
        public List<CountryEntity> GetCountries()
        {
            List<CountryEntity> results = new List<CountryEntity>();
            results = rep.GetCountries();
            return results;
        }
        public int InsertOrUpdateCountryGroup(CountryGroupEntity obj, int UserId)
        {
            return rep.InsertOrUpdateCountryGroup(obj, UserId);
        }
        public List<CountryGroupEntity> GetCountryGroupList()
        {
            List<CountryGroupEntity> results = new List<CountryGroupEntity>();
            results = rep.GetCountryGroupList();
            return results;
        }
        public CountryGroupEntity GetCountryGroupByName(string GroupName)
        {
            CountryGroupEntity result = new CountryGroupEntity();
            result = rep.GetCountryGroupByName(GroupName);
            return result;
        }
        public CountryGroupEntity GetCountryGroupDetailsById(int ID)
        {
            CountryGroupEntity result = new CountryGroupEntity();
            result = rep.GetCountryGroupDetailsById(ID);
            return result;
        }
        public void DeleteCountryGroup(int ID, int UserId)
        {
            rep.DeleteCountryGroup(ID, UserId);
        }

        public DataTable GetCountrygroupColumnsName()
        {
            return rep.GetCountrygroupColumnsName();
        }
        public DataTable GetCountryGroupDetail()
        {
            return rep.GetCountryGroupDetail();
        }
        public string MergeCountryGroup(bool ReplaceExisting)
        {
            return rep.MergeCountryGroup(ReplaceExisting);
        }
        #endregion

        #region Users
        public List<UsersEntity> GetUsersListPaging(string LOBTag)
        {
            List<UsersEntity> results = new List<UsersEntity>();
            results = rep.GetUsersListPaging(LOBTag);
            return results;
        }
        public List<UsersEntity> GetUsersList()
        {
            List<UsersEntity> results = new List<UsersEntity>();
            results = rep.GetUsersList();
            return results;
        }
        public int GetActiveUsers()
        {
            return rep.GetActiveUsers();
        }
        public List<UserStatus> GetUserTypeCode()
        {
            List<UserStatus> results = new List<UserStatus>();
            results = rep.GetUserTypeCode();
            return results;
        }
        public List<UserStatus> GetUserStatus()
        {
            List<UserStatus> results = new List<UserStatus>();
            results = rep.GetUserStatus();
            return results;
        }
        public UsersEntity GetUserDetailsById(int userId)
        {
            UsersEntity result = new UsersEntity();
            result = rep.GetUserDetailsById(userId);
            return result;
        }
        public UsersEntity GetUserDetailsByName(string userName)
        {
            UsersEntity result = new UsersEntity();
            result = rep.GetUserDetailsByName(userName);
            return result;
        }
        public string InsertOrUpdateUsersDetails(UsersEntity usersObj, int ModifiedByUserId = 0)
        {
            string Message = string.Empty;
            Message = rep.InsertOrUpdateUsersDetails(usersObj, ModifiedByUserId);
            return Message;
        }
        public void ResetUserPassword(string EmailAddress, string PasswordHash, string SecurityStamp)
        {
            rep.ResetUserPassword(EmailAddress, PasswordHash, SecurityStamp);
        }
        #region Delete User
        public string DeleteUser(int UserId, int ModifiedByUserId = 0, bool ChangesByAdminPortal = false)
        {
            string Message = string.Empty;
            Message = rep.DeleteUser(UserId, ModifiedByUserId, ChangesByAdminPortal);
            return Message;
        }
        #endregion
        #region Logout User
        public string ForceUserLogout(int UserId)
        {
            return rep.ForceUserLogout(UserId);
        }
        #endregion
        #region Activate User
        public string ActivateUser(int UserId, int ModifiedByUserId = 0, bool ChangesByAdminPortal = false)
        {
            string Message = string.Empty;
            Message = rep.ActivateUser(UserId, ModifiedByUserId, ChangesByAdminPortal);
            return Message;
        }
        #endregion
        #endregion

        #region "Data Enrichment DnBAPI"
        public List<DnBAPIGroupEntity> GetDnBAPIGroupList(string LOBTag)
        {
            List<DnBAPIGroupEntity> results = new List<DnBAPIGroupEntity>();
            results = rep.GetDnBAPIGroupList(LOBTag);
            return results;
        }
        public List<DnbAPIEntity> GetDnBAPIList(string APIType, int credId = 0)
        {
            List<DnbAPIEntity> results = new List<DnbAPIEntity>();
            results = rep.GetDnBAPIList(APIType, credId);
            return results;
        }

        public void InsertOrUpdateDnBAPIDetail(DnBAPIGroupEntity obj)
        {
            rep.InsertOrUpdateDnBAPIDetail(obj);
        }

        public DnBAPIGroupEntity GetAPIGroupDetailsById(int ID)
        {
            DnBAPIGroupEntity result = new DnBAPIGroupEntity();
            result = rep.GetAPIGroupDetailsById(ID);
            return result;
        }

        public void DeleteAPIGroup(int ID)
        {
            rep.DeleteAPIGroup(ID);
        }
        #endregion


        #region "Auto-Acceptance"
        public List<AutoAdditionalAcceptanceCriteriaEntity> GetAutoAcceptanceDetailsPagedSorted(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, int ConfidenceCode, int CountyGroup, string Tags, string LOBTag, bool Active)
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            results = rep.GetAutoAcceptanceDetailsPagedSorted(SortOrder, PageNumber, PageSize, out TotalRecords, ConfidenceCode, CountyGroup, Tags, LOBTag, Active);
            return results;
        }
        public List<AutoAcceptanceCriteriaDetail> GetAutoAcceptanceCriteriaDetailByGroupId(int CriteriaGroupId)
        {
            List<AutoAcceptanceCriteriaDetail> results = new List<AutoAcceptanceCriteriaDetail>();
            results = rep.GetAutoAcceptanceCriteriaDetailByGroupId(CriteriaGroupId);
            return results;
        }
        public AutoAdditionalAcceptanceCriteriaEntity GetAutoAcceptanceDetailByID(int ID)
        {
            AutoAdditionalAcceptanceCriteriaEntity result = new AutoAdditionalAcceptanceCriteriaEntity();
            result = rep.GetAutoAcceptanceDetailByID(ID);
            return result;
        }
        public void InsertOrUpdateAcceptanceSettings(AutoAdditionalAcceptanceCriteriaEntity obj)
        {
            rep.InsertOrUpdateAcceptanceSettings(obj);
        }
        public DataTable GetSecondaryAutoAcceptanceCriteriaColumnsName()
        {
            return rep.GetSecondaryAutoAcceptanceCriteriaColumnsName();
        }
        public void DeleteAcceptance(string CriteriaGroupId, int UserId, int CommentId)
        {
            rep.DeleteAcceptance(CriteriaGroupId, UserId, CommentId);
        }
        public DataTable GetAutoAcceptanceMatchGrade(string LOBTag)
        {
            return rep.GetAutoAcceptanceMatchGrade(LOBTag);
        }
        public DataTable GetTopMatchGradeSettings(bool IsTopMatch)
        {
            return rep.GetTopMatchGradeSetting(IsTopMatch);
        }
        public string MergeSecondaryAutoAcceptCriteria(bool ReplaceExisting, int UserId, int CommentId)
        {
            string Message = string.Empty;
            Message = rep.MergeSecondaryAutoAcceptCriteria(ReplaceExisting, UserId, CommentId);
            return Message;
        }
        public int GetSecondaryAutoAcceptanceCriteriaGroupCount()
        {
            return rep.GetSecondaryAutoAcceptanceCriteriaGroupCount();
        }
        public void RunAutoAcceptanceRule(int UserId = 0)
        {
            rep.RunAutoAcceptanceRule(UserId);
        }
        public void InsertAcceptanceSettingsRunRules(AutoAdditionalAcceptanceCriteriaEntity obj)
        {
            rep.InsertAcceptanceSettingsRunRules(obj);
        }
        public void DeleteSecondaryAutoAcceptanceCriteria()
        {
            rep.DeleteSecondaryAutoAcceptanceCriteria();
        }
        public List<AutoAdditionalAcceptanceCriteriaEntity> GetAutoAcceptanceDetails()
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            results = rep.GetAutoAcceptanceDetails();
            return results;
        }
        public void UpdateSequence(int CriteriaId, int sequenceNo)
        {
            rep.UpdateSequence(CriteriaId, sequenceNo);
        }
        public void ManageSecondaryAutoAcceptanceCriteriaActivation(int GroupId, bool Activate)
        {
            rep.ManageSecondaryAutoAcceptanceCriteriaActivation(GroupId, Activate);
        }
        #region "Preview Auto-Acceptance"
        public List<PreviewAutoAcceptanceRuleEntity> GetStewPreviewAutoAcceptanceRule(AutoAdditionalAcceptanceCriteriaEntity obj, bool ApplyRule, bool Export)
        {
            return rep.GetStewPreviewAutoAcceptanceRule(obj, ApplyRule, Export);
        }
        public DataTable ExportStewPreviewAutoAcceptanceRule(AutoAdditionalAcceptanceCriteriaEntity obj, bool ApplyRule, bool Export)
        {
            return rep.ExportStewPreviewAutoAcceptanceRule(obj, ApplyRule, Export);
        }
        #endregion
        #endregion

        #region "Reset System data"
        public void ResetSystemData()
        {
            rep.ResetSystemData();
        }
        #endregion

        #region "Environment"
        public DataTable GetAllCDSEnvironment(bool isPaging)
        {
            return rep.GetAllCDSEnvironment(isPaging);
        }
        public DataTable GetCDSEnvironmentName(bool isPaging)
        {
            return rep.GetCDSEnvironmentName(isPaging);
        }
        public DataTable GetEnvironmentByName(string OrganizationUrl)
        {
            return rep.GetEnvironmentByName(OrganizationUrl);
        }
        #endregion

        #region "Entity"
        public string InsertCDSEntity(string Entity)
        {
            return rep.InsertCDSEntity(Entity);
        }
        public DataTable GetAllCDSEntity()
        {
            return rep.GetAllCDSEntity();
        }
        public DataTable GetCDSEntityListPaging(int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            DataTable results = new DataTable();
            results = rep.GetCDSEntityListPaging(SortOrder, PageNumber, PageSize, out TotalRecords);
            return results;
        }
        public void DeleteCDSEntity(string Entity)
        {
            rep.DeleteCDSEntity(Entity);
        }

        #endregion

        #region  "SMAL Setting"
        public DataTable getSAMLSSOSetting()
        {
            return rep.getSAMLSSOSetting();
        }
        public string UpdateSAMLSSOSettings(SAMLSSOSettingEntity SamlSettingObj)
        {
            return rep.UpdateSAMLSSOSettings(SamlSettingObj);
        }
        #endregion

        #region "Match Grades"
        public List<MatchGradeEntity> GetMatchGrades()
        {
            List<MatchGradeEntity> results = new List<MatchGradeEntity>();
            results = rep.GetMatchGrades();
            return results;
        }

        public List<MatchCodeEntity> GetMatchMDPCodes(string matchFieldType)
        {
            List<MatchCodeEntity> results = new List<MatchCodeEntity>();
            results = rep.GetMatchMDPCodes(matchFieldType);
            return results;
        }
        #endregion

        #region "User Profile"
        public void InsertOrUpdateUsersImage(int userId, string ImagePath)
        {
            rep.InsertOrUpdateUsersImage(userId, ImagePath);
        }
        #endregion

        #region "Export and Download Data"
        public DataTable GetExportTablesApiName(string ExportCategory)
        {
            DataTable dt = rep.GetExportTablesApiName(ExportCategory);
            return dt;
        }

        public SqlDataReader ExportDataEnrichmentDataReader(string Tags, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, string ExportCategory = "Enrichment")
        {
            return rep.ExportDataEnrichmentDataReader(Tags, ImportProcess, LOBTag, FlagExport, SrcRecID, SrcRecIdExactMatch, ExportCategory);

        }

        public void FinalizeDataEnrichmentExport(bool FlagExport)
        {
            rep.FinalizeDataEnrichmentExport(FlagExport);
        }
        public DataTable GetExportTablesDetails(string APIName, string ExportCategory)
        {
            return rep.GetExportTablesDetails(APIName, ExportCategory);
        }
        #endregion

        #region "Machine Detail"
        public void InsertUpdateUserMachineDetails(UserMachineEntity objUserMachine)
        {
            rep.InsertUpdateUserMachineDetails(objUserMachine);
        }
        public UserMachineEntity GetUserMachineDetails(int UserId, string MachineDetails)
        {
            return rep.GetUserMachineDetails(UserId, MachineDetails);
        }
        public void DelereUserMachineDetails(int UserId, string BrowserAgent)
        {
            rep.DelereUserMachineDetails(UserId, BrowserAgent);
        }
        public void DeleteUserMachineRecords(string MachineDetails, string BrowserAgent)
        {
            rep.DeleteUserMachineRecords(MachineDetails, BrowserAgent);
        }
        #endregion

        #region "API Credentials"
        public DataTable GetLicenseSetting(string Url)
        {
            DataTable dt = rep.GetLicenseSetting(Url);
            return dt;
        }

        public OverrideAPICredentialsEntity GetOverrideAPICredentials()
        {
            return rep.GetOverrideAPICredentials();
        }
        public string UpdateOverrideAPICredentials(OverrideAPICredentialsEntity obj)
        {
            return rep.UpdateOverrideAPICredentials(obj);
        }


        #endregion

        #region "Import Data"
        public DataTable GetInpCompanyColumnsName()
        {
            return rep.GetInpCompanyColumnsName();
        }
        public DataTable GetImportDataRefreshColumnsName()
        {
            return rep.GetImportDataRefreshColumnsName();
        }
        #endregion

        #region Get Login User Details and status
        public List<UserStatus> GetAttributeTypeForLogIn()
        {
            List<UserStatus> results = new List<UserStatus>();
            results = rep.GetAttributeTypeForLogIn();
            return results;
        }
        #endregion

        public void StewUserActivityHeartbeat(int userId)
        {
            rep.StewUserActivityHeartbeat(userId);
        }
        public DataTable GetUserLoginActivity(int UserId)
        {
            return rep.GetUserLoginActivity(UserId);
        }
        public DataTable GetLanguageCodes()
        {
            return rep.GetLanguageCodes();
        }
        public int GetMatchOutputCountByTag(string Tag)
        {
            return rep.GetMatchOutputCountByTag(Tag);
        }
        public void UpdateMultipleUserStatus(string UserIds, string Status)
        {
            rep.UpdateMultipleUserStatus(UserIds, Status);
        }
        public string GetURLEncode(string SrcRecordId = "", string CompanyName = "", string Address0 = "", string Address1 = "", string PrimaryTownName = "", string TerritoryName = "", string FullPostalCode = "", string CountryISOAlpha2Code = "", string TelephoneNumber = "", string ExclusionText = "", string inLanguage = "", string APIFamily = "", int ConfidenceCodeLowerLevelThreshold = 0, string DUNSNumber = null, string Domain = null, string Email = null, string InputId = null, string RegistrationNumber = "")
        {
            return rep.GetURLEncode(SrcRecordId, CompanyName, Address0, Address1, PrimaryTownName, TerritoryName, FullPostalCode, CountryISOAlpha2Code, TelephoneNumber, ExclusionText, inLanguage, APIFamily, ConfidenceCodeLowerLevelThreshold, DUNSNumber, Domain, Email, InputId, RegistrationNumber);
        }
        public void UpdateDefaultPageSize(int UserId, string Section, int PageSize = 10)
        {
            rep.UpdateDefaultPageSize(UserId, Section, PageSize);
        }
        public int GetDefaultPageSize(int UserId, string Section)
        {
            return rep.GetDefaultPageSize(UserId, Section);
        }
        public string UpdateUserAttemptCountDetail(int UserId)
        {
            return rep.UpdateUserAttemptCountDetail(UserId);
        }

        #region  "No references found"
        public DataTable GetAutoAcceptanceDetailsExportToExcel(bool LicenseEnableTags, int ConfidenceCode, string MatchGrade, int CountyGroupId, string Tags)
        {
            DataTable results = new DataTable();
            results = rep.GetAutoAcceptanceDetailsExportToExcel(LicenseEnableTags, ConfidenceCode, MatchGrade, CountyGroupId, Tags);
            return results;
        }
        #endregion


        public DataTable GetAcceptedBy()
        {
            return rep.GetAcceptedBy();
        }
        #region "Stewardship Portal(Match Data)"
        #region "Reject From FIle"
        public DataTable GetRejectPurgeColumnsName()
        {
            return rep.GetRejectPurgeColumnsName();
        }
        public string RejectPurgeDataFromImport(int UserId, bool IsPurgeData)
        {
            return rep.RejectPurgeDataFromImport(UserId, IsPurgeData);
        }
        #endregion

        #region "Accept From File"
        public string AcceptLCMDataFromImport(int UserId)
        {
            return rep.AcceptLCMDataFromImport(UserId);
        }
        public string AcceptOIMatchDataFromImport(int UserId)
        {
            return rep.AcceptOIMatchDataFromImport(UserId);
        }
        public DataTable GetImportDataForAcceptColumnsName()
        {
            return rep.GetImportDataForAcceptColumnsName();
        }
        #endregion

        #region "De-Duplicate Data"
        public string RemoveDuplicateRecords(int UserId, string Tag, string LOBTag, string CountryCode, int CountryGroupId)
        {
            return rep.RemoveDuplicateRecords(UserId, Tag, LOBTag, CountryCode, CountryGroupId);
        }
        #endregion

        #endregion

        #region "Monitoring DnB Direct Plus"
        public void DPMUpdateRegistration(string MonitoringRegistrationName, string Tags)
        {
            rep.DPMUpdateRegistration(MonitoringRegistrationName, Tags);
        }
        public void DPMInsertRegistration(string reference, string Tags, bool notificationsSuppressed, string productId, string versionId, string email, string fileTransferProfile, string description, string deliveryTrigger, string deliveryFrequency, int dunsCount, bool seedData, int CredentialId,string blockIds)
        {
            rep.DPMInsertRegistration(reference, Tags, notificationsSuppressed, productId, versionId, email, fileTransferProfile, description, deliveryTrigger, deliveryFrequency, dunsCount, seedData, CredentialId,blockIds);
        }
        public DataTable DPMGetRegistration()
        {
            return rep.DPMGetRegistration();
        }
        public void DPMAddDUNSRegistrationsToList()
        {
            rep.DPMAddDUNSRegistrationsToList();
        }
        public void DPMMergeDUNSRegistrations()
        {
            rep.DPMMergeDUNSRegistrations();
        }
        public void DPMInsertRegisteredDUNS(string MonitoringRegistrationName, string DnBDUNSNumber, string FileDateTime, string FileType)
        {
            rep.DPMInsertRegisteredDUNS(MonitoringRegistrationName, DnBDUNSNumber, FileDateTime, FileType);
        }
        public void DPMInsertNotification(int NotificationFileId, string NotificationFileName, string FileDate, string MonitoringRegistrationName, string FIleType, string ResponseJSON, string ProcessStatusId)
        {
            rep.DPMInsertNotification(NotificationFileId, NotificationFileName, FileDate, MonitoringRegistrationName, FIleType, ResponseJSON, ProcessStatusId);
        }
        public void DPMFinalizeFileLoad(int NotificationFileId)
        {
            rep.DPMFinalizeFileLoad(NotificationFileId);
        }
        public DataTable DPMGetRegistrationNamesForDUNSRegistration()
        {
            return rep.DPMGetRegistrationNamesForDUNSRegistration();
        }
        public DataTable DPMGetDUNSForRegistration(string MonitoringRegistrationName)
        {
            return rep.DPMGetDUNSForRegistration(MonitoringRegistrationName);
        }
        public DataTable GetDPMDunsRegistrationByRegistrationName(string MonitoringRegistrationName)
        {
            return rep.GetDPMDunsRegistrationByRegistrationName(MonitoringRegistrationName);
        }
        public DataTable GetDPMDunsRegistrationList(string RegistrationName, string DnBDUNSNumber, int SortOrder, int PageNumber, int PageSize, out int TotalRecords)
        {
            return rep.GetDPMDunsRegistrationList(RegistrationName, DnBDUNSNumber, SortOrder, PageNumber, PageSize, out TotalRecords);
        }
        public void DeleteAllDPMRegistration(string NotDeleteRegistration, int CredentialId)
        {
            rep.DeleteAllDPMRegistration(NotDeleteRegistration, CredentialId);
        }
        public void SuppressUnSupressNotifications(string referenceName, bool isSuprressed)
        {
            rep.SuppressUnSupressNotifications(referenceName, isSuprressed);
        }
        public DataTable checkFileExists(string FileName)
        {
            return rep.checkFileExists(FileName);
        }
        public DataTable GetDPMRegistrationByName(string MonitoringRegistrationName)
        {
            return rep.GetDPMRegistrationByName(MonitoringRegistrationName);
        }

        #endregion

        #region "OI"
        public string GetOICleanseMatchURLEncode(string CustomerSubDomain = "", string Country = "", string SrcRecordId = "", string InputId = "", string CompanyName = "", string address1 = "", string address2 = "", string city = "", string state = "", string PostalCode = "", string TelephoneNumber = "", string ExclusionText = "", string CEOName = "", string Domain = "", string Email = "", string InpOrbNum = "", string EIN = "")
        {
            return rep.GetOICleanseMatchURLEncode(CustomerSubDomain, Country, SrcRecordId, InputId, CompanyName, address1, address2, city, state, PostalCode, TelephoneNumber, ExclusionText, CEOName, Domain, Email, InpOrbNum, EIN);
        }
        #endregion


        public DataTable GetLicensedAPIType()
        {
            return rep.GetLicensedAPIType();
        }
        #region User Delete
        public string DeleteUserAfterReassign(int UserId, int ModifiedByUserId, bool ChangesByAdminPortal, int ReassignToUserId)
        {
            return rep.DeleteUserAfterReassign(UserId, ModifiedByUserId, ChangesByAdminPortal, ReassignToUserId);
        }
        #endregion

        public void UpdateSoloProcessSettings(string settingName, string settingValue)
        {
            rep.UpdateSoloProcessSettings(settingName, settingValue);
        }
    }
}
