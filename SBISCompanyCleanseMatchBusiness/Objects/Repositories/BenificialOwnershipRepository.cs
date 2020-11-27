using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class BenificialOwnershipRepository : RepositoryParent
    {
        public BenificialOwnershipRepository(string connectionString) : base(connectionString) { }
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }

        public UXBeneficialOwnershipURLEntity UXGetBeneficialOwnershipURL(string duns, string country)
        {
            UXBeneficialOwnershipURLEntity uRLEntity = new UXBeneficialOwnershipURLEntity();
            DataTable dt;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UXGetBeneficialOwnershipURL";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@DnBDUNSNumber", duns, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISO2AlphaCode", country, SQLServerDatatype.charDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if(dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        uRLEntity = CommonConvertMethods.GetItem<UXBeneficialOwnershipURLEntity>(item);
                    }
                }
                return uRLEntity;
            }
            catch (Exception ex)
            {
                //Put log to db here
                throw;
            }
        }

        internal DataSet PreviewBenificialOwnershipData(string DunsNumber)
        {
            DataSet ds;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.PreviewBenificialOwnershipData";
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(DunsNumber) ? DunsNumber.ToString() : null, SQLServerDatatype.VarcharDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw;
            }
            return ds;
        }

        internal string InsertScreenQueueAndResponseJSON(string source, int userId, int credId, string requestUrl, string searchJSON, string resultsJSON)
        {
            string Message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cmpl.InsertScreenQueueAndResponseJSON";
                sproc.StoredProceduresParameter.Add(GetParam("@source", source, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@userId", Convert.ToString(userId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@credentialId",Convert.ToString(credId), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@requestUrl", requestUrl, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@searchJSON", searchJSON, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@resultsJSON", resultsJSON, SQLServerDatatype.VarcharDataType));
                Message = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                Message = "error";
            }
            return Message;
        }

        public List<ScreenResponseEntity> GetScreenResponse(string alternateId, string beneficiaryType)
        {
            List<ScreenResponseEntity> lstScreenData = new List<ScreenResponseEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cmpl.GetScreenResponse";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@alternateId", alternateId, SQLServerDatatype.NvarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@beneficiaryType", beneficiaryType, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                lstScreenData = CommonConvertMethods.ConvertDataTable<ScreenResponseEntity>(dt);
                return lstScreenData;
            }
            catch (Exception ex)
            {
                //Put log to db here
                throw;
            }
        }

    }
}
