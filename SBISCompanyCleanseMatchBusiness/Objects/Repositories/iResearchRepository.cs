using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class iResearchRepository : RepositoryParent
    {
        public iResearchRepository(string connectionString) : base(connectionString) { }
        internal string InsertResearchInvestigation(iResearchEntity objViewEntity)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[dnb].[iResearchInsertInvestigation]";
                sproc.StoredProceduresParameter.Add(GetParam("@ResearchRequestId", objViewEntity.ResearchRequestId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestType", !string.IsNullOrEmpty(objViewEntity.ResearchRequestType) ? objViewEntity.ResearchRequestType : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objViewEntity.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", !string.IsNullOrEmpty(objViewEntity.InputId) ? objViewEntity.InputId : "0", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", !string.IsNullOrEmpty(objViewEntity.SrcRecordId) ? objViewEntity.SrcRecordId : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(objViewEntity.Tags) ? objViewEntity.Tags : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestBody", !string.IsNullOrEmpty(objViewEntity.RequestBody) ? objViewEntity.RequestBody : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestResponseJSON", !string.IsNullOrEmpty(objViewEntity.RequestResponseJSON) ? objViewEntity.RequestResponseJSON : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }
        internal string InsertiResearchInvestigationFailedCalls(iResearchEntity objViewEntity)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[dnb].[InsertiResearchInvestigationFailedCalls]";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objViewEntity.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestBody", !string.IsNullOrEmpty(objViewEntity.RequestBody) ? objViewEntity.RequestBody : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", !string.IsNullOrEmpty(objViewEntity.RequestResponseJSON) ? objViewEntity.RequestResponseJSON : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }
        internal int InsertInvestigationCase(int ResearchRequestId, int CaseId, string SubjectResearchTypes, string CaseStatusResponseJSON, string CaseStatus, string LastestStatusDateTime)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[aud].[iResearchInsertInvestigationCase]";
                sproc.StoredProceduresParameter.Add(GetParam("@ResearchRequestId", ResearchRequestId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CaseId", CaseId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SubjectResearchTypes", !string.IsNullOrEmpty(SubjectResearchTypes) ? SubjectResearchTypes : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CaseStatusResponseJSON", !string.IsNullOrEmpty(CaseStatusResponseJSON) ? CaseStatusResponseJSON : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CaseStatus", !string.IsNullOrEmpty(CaseStatus) ? CaseStatus : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LastestStatusDateTime", !string.IsNullOrEmpty(LastestStatusDateTime) ? LastestStatusDateTime : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }


        internal List<IResearchInvestigationEntity> GetIResearchInvestigation()
        {
            List<IResearchInvestigationEntity> results = new List<IResearchInvestigationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetiResearchInvestigation";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    IResearchInvestigationAdapter ta = new IResearchInvestigationAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<IResearchInvestigationEntity> GetFilterIResearchInvestigation(string SrcRecordId, string Status, string RequestStartDateTime, string RequestendDateTime, string Keyword)
        {
            List<IResearchInvestigationEntity> results = new List<IResearchInvestigationEntity>();
            List<IResearchInvestigationEntity> resultsFailed = new List<IResearchInvestigationEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetiResearchInvestigation";
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", !string.IsNullOrEmpty(SrcRecordId) ? SrcRecordId : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CaseStatus", !string.IsNullOrEmpty(Status) ? Status : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestStartDateTime", !string.IsNullOrEmpty(RequestStartDateTime) ? RequestStartDateTime : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestendDateTime", !string.IsNullOrEmpty(RequestendDateTime) ? Convert.ToDateTime(RequestendDateTime).Add(DateTime.MaxValue.TimeOfDay).ToString() : null, SQLServerDatatype.DateTimeDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SearchKeyword", !string.IsNullOrEmpty(Keyword) ? Keyword.Trim() : null, SQLServerDatatype.NvarcharDataType));
                DataSet ds;
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
                if (ds != null)
                {
                    IResearchInvestigationAdapter ta = new IResearchInvestigationAdapter();
                    if (ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow rw in ds.Tables[0].Rows)
                        {
                            results = ta.Adapt(ds.Tables[0]);
                        }
                    }
                    if (ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow rw in ds.Tables[1].Rows)
                        {
                            resultsFailed = ta.Adapt(ds.Tables[1]);
                        }
                        if (!string.IsNullOrEmpty(SrcRecordId))
                        {
                            resultsFailed = resultsFailed.Where(x => x.RequestBodylst.customerTransactionID.ToLower() == SrcRecordId.ToLower()).ToList();
                        }
                        resultsFailed.ForEach(x =>
                        {
                            x.SrcRecordId = x.RequestBodylst.customerTransactionID;
                            x.RequestType = x.RequestBodylst.researchRequestType;
                            if (x.RequestBodylst.researchRequestType.ToLower() == "targeted")
                            {
                                x.ResolutionDUNS = x.RequestBodylst?.organization?.duns;
                            }
                        });
                        results.AddRange(resultsFailed);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<DashboardV2GetInvestigationStatistics> GetDashboardV2GetInvestigationStatistics()
        {
            List<DashboardV2GetInvestigationStatistics> lstStats = new List<DashboardV2GetInvestigationStatistics>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[DashboardV2GetInvestigationStatistics]";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                IResearchInvestigationAdapter ta = new IResearchInvestigationAdapter();
                lstStats = ta.AdaptInvestigationStatistics(dt);
            }
            catch (Exception)
            {
                throw;
            }
            return lstStats;
        }


        internal bool iResearchUpdateRequestStatus(int researchRequestId, string result)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[iResearchUpdateRequestStatus]";
                sproc.StoredProceduresParameter.Add(GetParam("@ResearchRequestId", researchRequestId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CaseStatusResponseJSON", result, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
            return false;
        }
        internal IResearchInvestigationEntity iResearchInvestigationIsExists(string InputId, string SrcRecordId, string Duns)
        {
            IResearchInvestigationEntity results;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[CheckiResearchInvestigated]";
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(Duns) ? Duns : null, SQLServerDatatype.VarcharDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    IResearchInvestigationAdapter ta = new IResearchInvestigationAdapter();
                    results = ta.AdaptItem(dt.Rows[0], dt);
                    return results;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal DataTable GetiResearchInvestigationCaseLookup()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetiResearchInvestigationCaseLookup]";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal string InsertResearchInvestigation(iResearchEntityTargetedEntity objViewEntity)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "[dnb].[iResearchInsertInvestigation]";
                sproc.StoredProceduresParameter.Add(GetParam("@ResearchRequestId", objViewEntity.ResearchRequestId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestType", !string.IsNullOrEmpty(objViewEntity.ResearchRequestType) ? objViewEntity.ResearchRequestType : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objViewEntity.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", !string.IsNullOrEmpty(objViewEntity.InputId) ? objViewEntity.InputId : "", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", !string.IsNullOrEmpty(objViewEntity.SrcRecordId) ? objViewEntity.SrcRecordId : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(objViewEntity.Tags) ? objViewEntity.Tags : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestBody", !string.IsNullOrEmpty(objViewEntity.RequestBody) ? objViewEntity.RequestBody : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestResponseJSON", !string.IsNullOrEmpty(objViewEntity.RequestResponseJSON) ? objViewEntity.RequestResponseJSON : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(objViewEntity.Duns) ? objViewEntity.Duns : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                return result = ex.ToString();
            }

            return result;
        }
        internal string InsertiResearchInvestigationFailedCalls(iResearchEntityTargetedEntity objViewEntity)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[InsertiResearchInvestigationFailedCalls]";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objViewEntity.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@RequestBody", !string.IsNullOrEmpty(objViewEntity.RequestBody) ? objViewEntity.RequestBody : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", !string.IsNullOrEmpty(objViewEntity.RequestResponseJSON) ? objViewEntity.RequestResponseJSON : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                return result;
            }

            return result;
        }
        internal List<iResearchEntityTargetedEntity> GetDnBReferenceDataBycategoryID(int categoryID)
        {
            List<iResearchEntityTargetedEntity> results = new List<iResearchEntityTargetedEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDnBReferenceDataBycategoryID";
                sproc.StoredProceduresParameter.Add(GetParam("@categoryID", Convert.ToString(categoryID), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new IResearchInvestigationAdapter().AdaptItems(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal DataTable GetiResearchMarketApplicability(string MarketApplicability,int fullList)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetiResearchMarketApplicability";
                sproc.StoredProceduresParameter.Add(GetParam("@MarketApplicability", MarketApplicability.ToLower() == "us" ? "US Only" : "Global", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FullList", Convert.ToBoolean(fullList).ToString(), SQLServerDatatype.BitDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
    }
}
