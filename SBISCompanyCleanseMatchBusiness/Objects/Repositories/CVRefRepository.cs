using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class CVRefRepository : RepositoryParent
    {
        public CVRefRepository(string connectionString) : base(connectionString) { }
        #region "Other Methods"
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            return param;
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
        #endregion
        internal List<CVRefEntity> GetAPItype(int typeCode)
        {
            List<CVRefEntity> results = new List<CVRefEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetCVRefByTypeCode]";
                sproc.StoredProceduresParameter.Add(GetParam("@TypeCode", typeCode.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CVRefAadpter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<CVRefEntity> GetThirdPartyProviders(int fullList)
        {
            List<CVRefEntity> results = new List<CVRefEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetThirdPartyProviders]";
                sproc.StoredProceduresParameter.Add(GetParam("@FullList", fullList.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CVRefAadpter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                //throw;
            }
            return results;
        }

        internal DataTable GetThirdPartyProviderEnrichments(int typeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetThirdPartyProviderEnrichments]";
                sproc.StoredProceduresParameter.Add(GetParam("@ProviderCode", typeCode.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                //throw;
            }
            return dt;
        }
    }
}
