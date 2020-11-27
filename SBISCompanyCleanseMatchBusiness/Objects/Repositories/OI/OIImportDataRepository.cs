using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class OIImportDataRepository : RepositoryParent
    {
        public OIImportDataRepository(string connectionString) : base(connectionString) { }

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
        internal int InsertOIStgInputCompany(OIInpCompanyEntity objCompany)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.InsertOIStgInputCompany";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", objCompany.ImportProcessId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", objCompany.SrcRecordId.ToString().Trim(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", !string.IsNullOrEmpty(objCompany.CompanyName) ? objCompany.CompanyName.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address1", !string.IsNullOrEmpty(objCompany.Address1) ? objCompany.Address1.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Address2", !string.IsNullOrEmpty(objCompany.Address2) ? objCompany.Address2.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", !string.IsNullOrEmpty(objCompany.City) ? objCompany.City.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", !string.IsNullOrEmpty(objCompany.State) ? objCompany.State.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PostalCode", !string.IsNullOrEmpty(objCompany.PostalCode) ? objCompany.PostalCode.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Country", !string.IsNullOrEmpty(objCompany.Country) ? objCompany.Country.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PhoneNbr", !string.IsNullOrEmpty(objCompany.PhoneNbr) ? objCompany.PhoneNbr.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(objCompany.OrbSingleEntryTags) ? objCompany.OrbSingleEntryTags.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EIN", !string.IsNullOrEmpty(objCompany.EIN) ? objCompany.EIN.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrbNum", !string.IsNullOrEmpty(objCompany.OrbNum) ? objCompany.OrbNum.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CEOName", !string.IsNullOrEmpty(objCompany.CEOName) ? objCompany.CEOName.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Website", !string.IsNullOrEmpty(objCompany.Website) ? objCompany.Website.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCompanyName", !string.IsNullOrEmpty(objCompany.AltCompanyName) ? objCompany.AltCompanyName.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltAddress1", !string.IsNullOrEmpty(objCompany.AltAddress1) ? objCompany.AltAddress1.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltAddress2", !string.IsNullOrEmpty(objCompany.AltAddress2) ? objCompany.AltAddress2.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCity", !string.IsNullOrEmpty(objCompany.AltCity) ? objCompany.AltCity.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltState", !string.IsNullOrEmpty(objCompany.AltState) ? objCompany.AltState.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltPostalCode", !string.IsNullOrEmpty(objCompany.AltPostalCode) ? objCompany.AltPostalCode.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AltCountry", !string.IsNullOrEmpty(objCompany.AltCountry) ? objCompany.AltCountry.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Email", !string.IsNullOrEmpty(objCompany.Email) ? objCompany.Email.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal int InsertOIStgInputCompanyMatchRefresh(OIInpCompanyEntityMatchRefresh objCompany)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.OIInsertStgInputCompanyMatchRefresh";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", objCompany.ImportProcessId.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@orb_num", !string.IsNullOrEmpty(objCompany.OrbNumber) ? objCompany.OrbNumber.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal string OIProcessDataImport(int ImportProcessId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.OIProcessDataImport";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }
        internal string OIProcessDataForEnrichment(int ImportProcessId)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "ext.OIProcessDataForEnrichment";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcessId", ImportProcessId.ToString(), SQLServerDatatype.IntDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                return Message;
            }
            catch (Exception ex)
            {
                return Message = ex.Message;
            }
        }

        internal DataTable GetOIStgInputCompanyColumnsName()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetOIStgInputCompanyColumnsName]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
        internal DataTable GetDataImportProcess()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetDataImportProcess]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }
    }
}
