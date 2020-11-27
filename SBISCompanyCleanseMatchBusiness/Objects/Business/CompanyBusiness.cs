using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class CompanyBusiness : BusinessParent
    {
        CompanyRepository rep;
        public CompanyBusiness(string connectionString) : base(connectionString) { rep = new CompanyRepository(Connection); }

        #region "Company Match and Bad"
        public Tuple<List<CompanyEntity>, string> GetBIDCompany(int UserID, int PgaeIndex, int PageSize, out int TotalCount)
        {
            return rep.GetBIDCompany(UserID, PgaeIndex, PageSize, out TotalCount);
        }

        public Tuple<List<CompanyEntity>, string> GetLCMCompany(int UserID, int PgaeIndex, int PageSize, out int TotalCount, bool IsApprove = false)
        {
            return rep.GetLCMCompany(UserID, PgaeIndex, PageSize, out TotalCount, IsApprove);
            //return results;
        }
        public List<CompanyEntity> GetLCMCompany()
        {
            List<CompanyEntity> results = new List<CompanyEntity>();
            results = rep.GetLCMCompany();
            return results;
        }
        public List<CompanyEntity> GetBIDCompany()
        {
            List<CompanyEntity> results = new List<CompanyEntity>();
            results = rep.GetBIDCompany();
            return results;
        }

        public void AcceptLCMMatches(List<MatchEntity> Matches, string UserName)
        {
            rep.AcceptLCMMatches(Matches, UserName);
        }
        /// <summary>
        /// override after added new feture for approve match data - Build 120
        /// </summary>
        /// <param name="Matches"></param>
        public void AcceptLCMMatches(List<MatchEntity> Matches, string UserName, int UserId, bool IsApprove = true)
        {
            rep.AcceptLCMMatches(Matches, UserName, UserId, IsApprove);
        }

        public void RejectLCMMatches(List<CompanyEntity> Companies, string UserName)
        {
            rep.RejectLCMMatches(Companies, UserName);
        }
        /// <summary>
        /// override after added new feture for approve match data - Build 120
        /// </summary>
        /// <param name="Matches"></param>
        public void RejectLCMMatches(List<CompanyEntity> Companies, string UserName, int UserId, bool IsApprove = true)
        {
            rep.RejectLCMMatches(Companies, UserName, UserId, IsApprove);
        }
        public void AcceptBIDMatch(CompanyEntity company, MatchEntity matchEntity, string UserName)
        {
            rep.AcceptBIDMatch(company, matchEntity, UserName);
        }
        public void UpdateBIDRecord(List<CompanyEntity> Companies, string UserName)
        {
            rep.UpdateBIDRecord(Companies, UserName);
        }
        public void DeleteBIDData(List<CompanyEntity> Companies, string UserName)
        {
            rep.DeleteBIDData(Companies, UserName);
        }

        #endregion

        #region "Login"
        public UsersEntity StewUserLogIn(string EmailAddress, string DomainId, bool IsWebLogin = false)
        {
            return rep.StewUserLogIn(EmailAddress, DomainId, IsWebLogin);
        }
        public UsersEntity GetUserByEmail(string EmailId)
        {
            return rep.GetUserByEmail(EmailId);
        }
        public UsersEntity GetUserBySAMLUserName(string UserName)
        {
            return rep.GetUserBySAMLUserName(UserName);
        }
        public UsersEntity GetUserByLoginId(string EmailAddress)
        {
            return rep.GetUserByLoginId(EmailAddress);
        }
        public void StewUserLogOut(int userId)
        {
            rep.StewUserLogIn(userId);
        }
        public void InsertuserLoginAudit(int UserId, string IpAddress, int LoginStatus, string BrowserToken, string BrowserAgent)
        {
            rep.InsertuserLoginAudit(UserId, IpAddress, LoginStatus, BrowserToken, BrowserAgent);
        }
        #endregion

        #region "Session

        public void DeleteUserSessionFilter(int UserId)
        {
            rep.DeleteUserSessionFilter(UserId);
        }

        #endregion

        #region "Report"
        public DataTable GetFirstMatchCCMG()
        {
            return rep.GetFirstMatchCCMG();
        }
        public DataTable GetCompanyAuditData()
        {
            return rep.GetCompanyAuditData();
        }
        public DataTable GetInputCompanyData()
        {
            return rep.GetInputCompanyData();
        }
        public DataTable GetOutputCompanyData()
        {
            return rep.GetOutputCompanyData();
        }
        public DataTable GetAPIUsage()
        {
            return rep.GetAPIUsage();
        }
        public DataTable GetTopMatchGrades(bool flag)
        {

            return rep.GetTopMatchGrades(flag);
        }
        public DataTable GetAPIUsageCount()
        {
            return rep.GetAPIUsageCount();
        }
        public System.Data.DataSet GetReport(string strReportType)
        {
            return rep.GetReport(strReportType);
        }
        public System.Data.DataSet GetActiveDataStatistics(string LOBTag)
        {
            return rep.GetActiveDataStatistics(LOBTag);
        }
        public DataTable GetAPIDataCount()
        {
            return rep.GetAPIDataCount();
        }
        public DataTable GetDashboardGetDataQueueStatistics(string LOBTag, string Tag, int UserId)
        {
            return rep.GetDashboardGetDataQueueStatistics(LOBTag, Tag, UserId);
        }
        public DataSet DashboardGetLowConfidenceMatchStatistics(string LOBTag, string Tag, int UserId)
        {
            return rep.DashboardGetLowConfidenceMatchStatistics(LOBTag, Tag, UserId);
        }
        public DataSet GetStewardshipStatistics(string LOBTag, string Tag, int UserId, int ImportProcessId)
        {
            return rep.GetStewardshipStatistics(LOBTag, Tag, UserId, ImportProcessId);
        }
        #endregion

        #region "Other Method"
        public void ExecuteCleanseMatchETL()
        {
            rep.ExecuteCleanseMatchETL();
        }

        public void SetUserLayoutPreference(int userId, string userLayout)
        {
            rep.SetUserLayoutPreference(userId, userLayout);
        }
        public void AuthenticateApplication(string ClientGUID)
        {
            try
            {
                rep.AuthenticateApplication(ClientGUID);
            }
            catch
            {
                throw;
            }
        }
        public object GetETLJobStatus()
        {
            try
            {
                return rep.GetETLJobStatus();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetETLJobStatusMessage()
        {
            try
            {
                return rep.GetETLJobStatusMessage();
            }
            catch
            {
                throw;
            }
        }
        public DataTable GetActiveUserData()
        {
            try
            {
                return rep.GetActiveUserData();
            }
            catch
            {
                throw;
            }
        }

        public void InsertAPILogs(string ServiceTransactionID, DateTime? TransactionTimestamp, string SeverityText, string ResultID, string ResultText, string MatchDataCriteriaText, int MatchedQuantity, string DnBDUNSNumber)
        {
            rep.InsertAPILogs(ServiceTransactionID, TransactionTimestamp, SeverityText, ResultID, ResultText, MatchDataCriteriaText, MatchedQuantity, DnBDUNSNumber);
        }
        public void StewUserActivityCloseWindow(int UserId)
        {
            rep.StewUserActivityCloseWindow(UserId);
        }
        #endregion

        #region "Download MAtchbook API"







        #endregion

        #region "Review data - Build 119"
        public DataTable GetReviewData(int ConfidenceCode, string MatchGrade, string MDP, bool IsTopMatch, int CountryGroupId, string OrderByColumn, int PageSize, int PageIndex, out int TotalRecord)
        {
            return rep.GetReviewData(ConfidenceCode, MatchGrade, MDP, IsTopMatch, CountryGroupId, OrderByColumn, PageSize, PageIndex, out TotalRecord);
        }
        #endregion

        #region "Review All data - Build 121"
        public DataTable GetReviewAllData(bool IsTopMatch, int CountryGroupId, string LOBTag, string Tags, string ConfidenceCodes, int UserId, string OrderBy, int PageNumber, int PageSize, out int TotalRecords)
        {
            return rep.GetReviewAllData(IsTopMatch, CountryGroupId, LOBTag, Tags, ConfidenceCodes, UserId, OrderBy, PageNumber, PageSize, out TotalRecords);
        }
        #endregion

        #region "User Details"
        public List<UsersEntity> GetSecurityQuestion()
        {
            List<UsersEntity> results = new List<UsersEntity>();
            results = rep.GetSecurityQuestion();
            return results;
        }
        public void UpdateSecurityQue(int userid, int securityQueId, string securityAnswer)
        {
            rep.UpdateSecurityQue(userid, securityQueId, securityAnswer);
        }
        public void updateUserLoginFirstTime(string EmailAddress, bool IsUserLoginFirstTime, string VerificationCode)
        {
            rep.updateUserLoginFirstTime(EmailAddress, IsUserLoginFirstTime, VerificationCode);
        }
        public void updateUserEULA(DateTime EULAAcceptedDateTime, string EULAAcceptedIPAddress, int Id)
        {
            rep.updateUserEULA(EULAAcceptedDateTime, EULAAcceptedIPAddress, Id);
        }
        #endregion

        #region "Import Data"
        public int InsertStgInputCompany(InpCompanyEntity objCompany)
        {
            int Result = 0;
            Result = rep.InsertStgInputCompany(objCompany);
            return Result;
        }
        public int InsertDataImportProcess(DataImportProcessEntity objDataImport)
        {
            int Result = 0;
            Result = rep.InsertDataImportProcess(objDataImport);
            return Result;
        }
        public string ProcessDataImport(int ImportProcessId)
        {
            string message = "";
            message = rep.ProcessDataImport(ImportProcessId);
            return message;
        }
        public string ImportDataRefresh(int ImportProcessId)
        {
            string message = "";
            message = rep.ImportDataRefresh(ImportProcessId);
            return message;
        }
        #endregion

        #region search data
        public void InsertCleanseMatchCallResults(string SrcRecordId, string responseJson, string Apirequest, int UserId, string InputId)
        {
            rep.InsertCleanseMatchCallResults(SrcRecordId, responseJson, Apirequest, UserId, InputId);
        }
        #endregion

        #region Reject All
        public DataTable GetStewTags(bool IsMatchData, string LOBTag)
        {
            return rep.GetStewTags(IsMatchData, LOBTag);
        }
        public int StewRejectAllLCMRecords(string CountryISO2AlphaCode, int CountryGroupId, string Tag, string ImportProcess, int HighestConfidenceCode, string SrcRecordId, string City, string State, bool CityExactMatch, bool StateExactMatch, int UserId, bool GetCountsOnly, bool Purge)
        {
            return rep.StewRejectAllLCMRecords(CountryISO2AlphaCode, CountryGroupId, Tag, ImportProcess, HighestConfidenceCode, SrcRecordId, City, State, CityExactMatch, StateExactMatch, UserId, GetCountsOnly, Purge);
        }
        public int PurgeAllBIDRecords(string CountryISO2AlphaCode, int CountryGroupId, string Tag, string ImportProcess, string SrcRecordId, string City, string State, bool CityExactMatch, bool StateExactMatch, int UserId, bool GetCountsOnly)
        {
            return rep.PurgeAllBIDRecords(CountryISO2AlphaCode, CountryGroupId, Tag, ImportProcess, SrcRecordId, City, State, CityExactMatch, StateExactMatch, UserId, GetCountsOnly);
        }

        public DataTable GetImportProcessesByQueue(string Queue, bool IsMatchdata)
        {
            return rep.GetImportProcessesByQueue(Queue, IsMatchdata);
        }
        public int InsertStgImportDataRefresh(InpCompanyDataRefershEntity model)
        {
            return rep.InsertStgImportDataRefresh(model);
        }
        public void StewPurgeSingleRecord(string InputId, string SrcRecordId, int UserId, string Queue)
        {
            rep.StewPurgeSingleRecord(InputId, SrcRecordId, UserId, Queue);
        }
        #endregion

        #region "Export Data"

        public SqlDataReader ExportCompanyDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, int UserId)
        {
            return rep.ExportCompanyDataReader(Tag, ImportProcess, LOBTag, FlagExport, SrcRecID, SrcRecIdExactMatch, UserId); //D&B - Inclusive tags on ,while exporting data issue (MP-647)
        }

        public SqlDataReader ExportActiveQueueOutputDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch, int UserId)
        {
            return rep.ExportActiveQueueOutputDataReader(Tag, ImportProcess, LOBTag, FlagExport, SrcRecID, SrcRecIdExactMatch, UserId);
        }
        public DataTable GetExportedDataImportProcess()
        {
            return rep.GetExportedDataImportProcess();
        }
        public SqlDataReader GetExportDUNSTransferDataReader(bool FlagExport)
        {
            return rep.GetExportDUNSTransferDataReader(FlagExport);
        }
        public int GetExportActiveQueueOutputCnt()
        {
            return rep.GetExportActiveQueueOutputCnt();
        }
        public SqlDataReader ExportLCMQueueDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch)
        {
            return rep.ExportLCMQueueDataReader(Tag, ImportProcess, LOBTag, FlagExport, SrcRecID, SrcRecIdExactMatch);
        }
        public SqlDataReader ExportBadInputDataQueueExportBadInputDataQueueDataReader(string Tag, string ImportProcess, string LOBTag, bool FlagExport, string SrcRecID, bool SrcRecIdExactMatch)
        {
            return rep.ExportBadInputDataQueueExportBadInputDataQueueDataReader(Tag, ImportProcess, LOBTag, FlagExport, SrcRecID, SrcRecIdExactMatch);
        }
        public void FinalizeCompanyDataExport(bool FlagExport)
        {
            rep.FinalizeCompanyDataExport(FlagExport);
        }
        #endregion

        #region Report Data
        public DataTable GetDataQueueDashboardReport()
        {
            return rep.GetDataQueueDashboardReport();
        }
        public DataSet GetdtDataStewardStatisticsReport(string UserGroup)
        {
            return rep.GetdtDataStewardStatisticsReport(UserGroup);
        }
        public DataSet GetdtAPIReport()
        {
            return rep.GetdtAPIReport();
        }

        #endregion

        #region BuildList
        public object InsertBuildSearch(int UserId, string RequestedJson, string ResponseJson, DateTime? RequestedDate, DateTime? ResponseDate)
        {
            return rep.InsertBuildSearch(UserId, RequestedJson, ResponseJson, RequestedDate, ResponseDate);
        }
        public DataTable GetSearchListResults(int UserId)
        {
            return rep.GetSearchListResults(UserId);
        }
        public DataTable ExportBuildResult(long SearchResultsId)
        {
            return rep.ExportBuildResult(SearchResultsId);
        }

        public DataTable ViewHistory(long Id)
        {
            return rep.ViewHistory(Id);
        }
        #endregion

        #region "Re Match Records"
        //Implement re-match queue (MP-14)
        public string StewReMatchBadInputData(ReMatchRecordsEntity model)
        {
            return rep.StewReMatchBadInputData(model);
        }
        #endregion

        public string GetAPILayer(string DnBAPICode)
        {
            return rep.GetAPILayer(DnBAPICode);
        }
        public DataTable GetComanyAttribute(string SrcRec)
        {
            return rep.GetComanyAttribute(SrcRec);
        }
        /// <summary>
        /// Get FamilyTree
        /// </summary>
        /// <returns></returns>
        ///  
        #region Family Tree
        public DataTable GetFamilyTreeChild(string ParentFamilyTreeDetailId)
        {
            return rep.GetFamilyTreeChild(ParentFamilyTreeDetailId);
        }
        public DataTable GetFamilyTree(int FamilyTreeId)
        {
            return rep.GetFamilyTree(FamilyTreeId);
        }

        public DataTable GetListFamilyTree()
        {
            return rep.GetListFamilyTree();
        }

        public DataTable GetCorporateLinkageDuns()
        {
            return rep.GetCorporateLinkageDuns();
        }

        public DataTable GetFamilyTreeById(string id)
        {
            return rep.GetFamilyTreeById(id);
        }

        public string PopulateCorporateLinkageDuns(string duns)
        {
            return rep.PopulateCorporateLinkageDuns(duns);
        }

        public string DeleteFamilyTreeNode(int sourceFamilyTreeId, int sourceFamilyTreeDetailId, int userId)
        {
            return rep.DeleteFamilyTreeNode(sourceFamilyTreeId, sourceFamilyTreeDetailId, userId);
        }

        public string AddFamilyTreeNode(int familyTreeId, string detailId, string nodeName, string nodeDisplayDetail, string nodeType, int? parentFamilyTreeDetailId, int userId)
        {
            return rep.AddFamilyTreeNode(familyTreeId, detailId, nodeName, nodeDisplayDetail, nodeType, parentFamilyTreeDetailId, userId);
        }

        public string AddFamilyTree(string familyTreeName, string familyTreeType, string alternateId, int userId)
        {
            return rep.AddFamilyTree(familyTreeName, familyTreeType, alternateId, userId);
        }

        public string DeleteFamilyTree(int familyTreeId, int userId)
        {
            return rep.DeleteFamilyTree(familyTreeId, userId);
        }

        public string DuplicaeFamilyTree(int familyTreeId, string familyTreeName, string familyTreeType, int userId)
        {
            return rep.DuplicaeFamilyTree(familyTreeId, familyTreeName, familyTreeType, userId);
        }
        public bool MoveFamilyTree(int sourceFamilyTreeId, int sourceFamilyTreeDetailId, int destinationFamilyTreeId, int destinationFamilyTreeDetailId, string operation, int userId)
        {
            return rep.MoveFamilyTree(sourceFamilyTreeId, sourceFamilyTreeDetailId, destinationFamilyTreeId, destinationFamilyTreeDetailId, operation, userId);
        }
        #endregion

        #region OI BuildList
        // Inserting in the view history list
        public object InsertOIBuildSearch(int UserId, string RequestedJson, string ResponseJson, DateTime? RequestedDate, DateTime? ResponseDate, int ResultCount)
        {
            return rep.InsertOIBuildSearch(UserId, RequestedJson, ResponseJson, RequestedDate, ResponseDate, ResultCount);
        }
        // Getting OI Build search list
        public DataTable GetOIBuildSearchListResults(int UserId)
        {
            return rep.GetOIBuildSearchListResults(UserId);
        }
        // View the records selected from history popups
        public DataTable ViewOIHistory(long Id)
        {
            return rep.ViewOIHistory(Id);
        }
        #endregion
        #region Remove Data From File
        public string RemoveFileData(int ImportProcessId, int UserId)
        {
            return rep.RemoveFileData(ImportProcessId, UserId);
        }
        #endregion

        #region Preview Enrichment Data
        public DataTable UXGetFirmographicsURL(string DunsNumber, string Country)
        {
            return rep.UXGetFirmographicsURL(DunsNumber, Country);
        }
        public void ProcessDataForEnrichment(int APIRequestId, int DnBAPIId, string DnBDUNSNumber, string CountryISOAlpha2Code, string JSONResponse, string ResultID, DateTime TransactionTimestamp, int CredentialId)
        {
            rep.ProcessDataForEnrichment(APIRequestId, DnBAPIId, DnBDUNSNumber, CountryISOAlpha2Code, JSONResponse, ResultID, TransactionTimestamp, CredentialId);
        }
        #endregion
    }
}
