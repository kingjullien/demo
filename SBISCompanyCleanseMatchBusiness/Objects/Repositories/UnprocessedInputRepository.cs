using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class UnprocessedInputRepository : RepositoryParent
    {
        public UnprocessedInputRepository(string connectionString) : base(connectionString) { }
        #region common Methods
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
        public List<UnprocessedInputEntity> GetUnprocessedInputRecords(string importProcess, int PageSize, int PageNumber, out int TotalRecords)
        {
            List<UnprocessedInputEntity> results = new List<UnprocessedInputEntity>();
            TotalRecords = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUnprocessedInputRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(importProcess) ? null : importProcess, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.Read.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UnprocessedInputAdapter().Adapt(dt);
                }
                TotalRecords = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal bool DeleteUnprocessedInputRecords(string importProcess)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUnprocessedInputRecords";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(importProcess) ? null : importProcess, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal bool MoveUnprocessedInputRecordsToBID(string importProcess)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.MoveUnprocessedInputRecordsToBID";
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", string.IsNullOrEmpty(importProcess) ? null : importProcess, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
