using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MultiPassRepository: RepositoryParent
    {
        public MultiPassRepository(string connectionString) : base(connectionString) { }

        internal DataTable GetProviderLookups(int providerCode)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.GetProviderLookups";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode",Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }

        internal void ModifyVerificationGroup(int providerCode, string tag, string VGNamesAndLookupIds)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.ModifyVerificationGroup";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", tag, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@VGNamesAndLookupIds", VGNamesAndLookupIds, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal void ModifyPrecedence(int providerCode, string tag, string steps)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.ModifyPrecedence";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", tag, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Steps", steps, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
        }

        internal List<MPMSummary> GetMPMSummaryByTag(int providerCode, string tag)
        {
            List<MPMSummary> lstGroups = new List<MPMSummary>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.GetMPMSummaryByTag";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag",string.IsNullOrEmpty(tag) ? null : tag, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                lstGroups = CommonConvertMethods.ConvertDataTable<MPMSummary>(dt);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return lstGroups;
        }

        internal List<MultiPassGroupConfiguration> GetVerificationGroupLookupList(int providerCode, string tag)
        {
            List<MultiPassGroupConfiguration> lstGroups = new List<MultiPassGroupConfiguration>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.GetLookupsByVerificationGroup";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(tag) ? null : tag, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                lstGroups = CommonConvertMethods.ConvertDataTable<MultiPassGroupConfiguration>(dt);
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return lstGroups;
        }

        internal string ModifyRule(int providerCode, string tag, string verificationGroups, string precedenceSteps, bool delete)
        {
            string message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.ModifyRule";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", tag, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@VerificationGroups", string.IsNullOrEmpty(verificationGroups) ? "" : verificationGroups, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PrecedenceSteps", string.IsNullOrEmpty(precedenceSteps) ? "" : precedenceSteps, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Delete", delete.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }
            return message;
        }

        internal string GetPrecedenceSteps(int providerCode, string tag)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.GetPrecedenceSteps";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", string.IsNullOrEmpty(tag) ? null : tag, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc, ""));
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return result;
        }

        internal DataTable GetTagsForMPM(int providerCode)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mpm.GetTagsForMPM";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", Convert.ToString(providerCode), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return dt;
        }

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
