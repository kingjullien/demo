using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class OICompanyMatchRepository : RepositoryParent
    {
        public OICompanyMatchRepository(string connectionString) : base(connectionString) { }
        #region "Comman Method"
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


        #region oi
        internal LstOIMatchCompany GetOICompanyList(int UserID, bool IncludeWithCandidates, bool IncludeWithoutCandidates, int PgaeIndex, int PageSize, out int TotalCount)
        {
            TotalCount = 0;

            LstOIMatchCompany lstOICompany = new LstOIMatchCompany();

            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewGetCompanyListPaging";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserID.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IncludeWithCandidates", IncludeWithCandidates.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IncludeWithoutCandidates", IncludeWithoutCandidates.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PgaeIndex.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                DataSet ds;
                ds = sql.ExecuteDataSetWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                lstOICompany.lstOICompany = new List<OICompanyEntity>();
                if (ds != null && ds.Tables.Count > 1)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        lstOICompany.lstOICompany = new OICompanyAdapter().Adapt(ds.Tables[0]);
                        TotalCount = Convert.ToInt32(outParam);
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        lstOICompany.Message = Convert.ToString(ds.Tables[1].Rows[0][0]);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return lstOICompany;
        }


        internal OIlstMatchDetails GetCompanyMatchDetails(int InputId, int UserId, bool ApplyFilter, string MatchIds = null)
        {
            OIlstMatchDetails oIlstMatchDetails = new OIlstMatchDetails();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewGetCompanyMatchDetails";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchIds", string.IsNullOrEmpty(MatchIds) ? null : MatchIds.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ApplyFilter", ApplyFilter.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataSet ds;
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.ReadWrite.ToString());
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        OICompanyAdapter ta = new OICompanyAdapter();
                        foreach (DataRow rw in ds.Tables[0].Rows)
                        {
                            oIlstMatchDetails.lstOICompanyInput = ta.AdaptItem(rw);
                        }
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                    {
                        oIlstMatchDetails.lstOISearchMatch = new OICompanyAdapter().SearchMatchAdapt(ds.Tables[1]);
                    }
                    if (ds.Tables[2] != null && ds.Tables[2].Rows != null && ds.Tables[2].Rows.Count > 0)
                    {
                        oIlstMatchDetails.lstOIMatchDetail = new OICompanyAdapter().OICompanyMatchDetailsAdapt(ds.Tables[2]);
                    }
                }
                return oIlstMatchDetails;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal String StewDeleteOIMatch(int InputId, int MatchId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewDeleteMatch";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchId", MatchId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return "";


            }
            catch (Exception ex)
            {
                //Put log to db here
                return ex.Message;
            }
        }
        internal String StewUndoOIMatch(int InputId, int MatchId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewUndoMatch";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchId", MatchId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return "";


            }
            catch (Exception ex)
            {
                //Put log to db here
                return ex.Message;
            }
        }
        internal string GetNewSearch(int InputId, string MatchURL, string ResponseJSON)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewNewSearch";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MatchURL", MatchURL.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", ResponseJSON.ToString(), SQLServerDatatype.NvarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
                //Put log to db here
            }
        }

        internal string AssignStewMatchRecord(int inputId, string OrbNum, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewMatchRecord";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", inputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNum", OrbNum.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (Exception ex)
            {
                //Put log to db here
                return ex.Message;
            }
        }
        internal OIlstMatchMetaDetails GetStewOIMatchMetadata(int inputId, string OrbNum)
        {
            OIlstMatchMetaDetails oIlstMatchDetails = new OIlstMatchMetaDetails();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewGetMatchMetadata";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", inputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNum", OrbNum.ToString(), SQLServerDatatype.VarcharDataType));
                DataSet ds;
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (ds != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows != null && ds.Tables[0].Rows.Count > 0)
                    {
                        OICompanyAdapter ta = new OICompanyAdapter();
                        foreach (DataRow rw in ds.Tables[0].Rows)
                        {
                            oIlstMatchDetails.lstOICompanyInput = ta.AdaptItem(rw);
                        }
                    }
                    if (ds.Tables[1] != null && ds.Tables[1].Rows != null && ds.Tables[1].Rows.Count > 0)
                    {
                        oIlstMatchDetails.lstMatchQualityMetadatas = new OICompanyAdapter().SearchMatchMetaDataAdapt(ds.Tables[1]);
                    }
                    if (ds.Tables[2] != null && ds.Tables[2].Rows != null && ds.Tables[2].Rows.Count > 0)
                    {
                        oIlstMatchDetails.lstMatchMetaDatas = new OICompanyAdapter().MatchMetaDataDetailAdapt(ds.Tables[2]);
                    }
                }
                return oIlstMatchDetails;
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }
        internal bool ValidateCompanySrcId(string SrcRecordId)
        {
            try
            {

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.StewCheckNewSrcRecordId";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return false;
            }
            catch (SqlException)
            {
                throw;
            }

        }

        #endregion
        #region OI Search Data Add Company
        internal string OIAddRecordAsNewCompany(string MatchURL, string ResponseJSON, string OrbNumber, string SrcRecordId, string Tags, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.AddRecordAsNewCompany";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@MatchURL", MatchURL.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ResponseJSON", ResponseJSON.ToString(), SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNumber", OrbNumber.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (SqlException)
            {
                throw;
            }

        }
        #endregion

        #region OI Match Data Add Company
        internal string OIAddRecordAsNewCompanyFromMatch(string InputId, string OrbNumber, string SrcRecordId, string Tags, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.AddRecordAsNewCompanyFromMatch";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@srcInputId", InputId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNumber", OrbNumber.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", SrcRecordId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", Tags.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteScalar(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (SqlException)
            {
                throw;
            }

        }
        #endregion
        #region "Window Close Event"
        internal void StewUserActivityCloseWindowOI(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewUserActivityCloseWindow";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (SqlException)
            {
                throw;
            }

        }
        #endregion

        #region "Delete Company Data"
        internal string DeleteCompanyData(OIExportToExcel Model)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DeleteCompanyData";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Model.UserId == 0 ? null : Model.UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId ", Model.SrcRecordId != null ? Model.SrcRecordId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City ", Model.City != null ? Model.City.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State ", Model.State != null ? Model.State.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryCode ", Model.CountryCode != null ? Model.CountryCode.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess ", Model.ImportProcess != null ? Model.ImportProcess.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", Model.CountryGroupId == 0 ? null : Model.CountryGroupId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag ", Model.Tag != null ? Model.Tag.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeleteWithCandidates", Model.ExportWithCandidates.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeleteWithoutCandidates", Model.ExportWithoutCandidates.ToString().Trim(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountOnly", Model.GetCountOnly.ToString().Trim(), SQLServerDatatype.BitDataType));
                string Message;
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region "Refresh Stewardship Queue"
        internal string StewRefreshUserStewardshipList(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.StewRefreshUserStewardshipList";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString().Trim(), SQLServerDatatype.IntDataType));
                string Message;
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
