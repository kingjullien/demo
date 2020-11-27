using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class ComplainceScreeningRepository : RepositoryParent
    {
        public ComplainceScreeningRepository(string connectionString) : base(connectionString) { }
        #region "Other Method"
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

        internal List<ComplainceScreeningEntity> GetComplainceScreening()
        {
            List<ComplainceScreeningEntity> results = new List<ComplainceScreeningEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetComplainceScreening";  // MP-846 Admin database cleanup and code cleanup.
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new ComplainceScreeningAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void UpdateComplainceScreeningCompleted(int Id, bool IsProcessComplete)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.UpdateComplainceScreeningCompleted";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsProcessComplete", IsProcessComplete.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

