using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System;
using System.Data;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories.OI
{
    internal class OICompanyRepository : RepositoryParent
    {
        public OICompanyRepository(string connectionString) : base(connectionString) { }
        #region "Session"
        // DB Changes (MP-716)
        internal OIUserSessionFilterEntity OIGetUserSessionFilterText(int UserID)
        {

            OIUserSessionFilterEntity result = new OIUserSessionFilterEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.GetUserSessionFilterText";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserID.ToString(), SQLServerDatatype.IntDataType));

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new OIUserSessionFilterAdapter().Adapt(dt).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return result;
        }
        internal void OIDeleteUserSessionFilter(int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DeleteUserSessionFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Records
        public void DeleteCompanyData(int UserId, bool DeleteWithCandidates, bool DeleteWithoutCandidates, string InputId, string SrcRecordId, string City, string State, string CountryCode, string Tag, int CountryGroupId, string ImportProcess, bool GetCountOnly)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DeleteCompanyData";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId != 0 ? UserId.ToString().Trim() : "", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeleteWithCandidates", DeleteWithCandidates.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DeleteWithoutCandidates", DeleteWithoutCandidates.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId != null ? InputId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", City != null ? City.ToString().Trim() : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", State != null ? State.ToString().Trim() : "", SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryCode", CountryCode != null ? CountryCode.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", Tag != null ? Tag.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", CountryGroupId != 0 ? CountryGroupId.ToString().Trim() : "", SQLServerDatatype.SmallintDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", ImportProcess != null ? ImportProcess.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@GetCountOnly", GetCountOnly.ToString().Trim(), SQLServerDatatype.BitDataType));
                string Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region MP-770 Add reject data in Match data (ORB)
        public string RejectCandidate(int InputId, string orb_num)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.RejectCandidate";
                sproc.StoredProceduresParameter.Add(GetParam("@InputId", InputId != 0 ? InputId.ToString().Trim() : "", SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@orb_num", orb_num != null ? orb_num.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                string Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                string Message = ex.Message;
                return Message;
            }
        }
        #endregion

        #region "Other Methods"
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
