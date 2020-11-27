using System;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class OIDashboardRepository : RepositoryParent
    {
        public OIDashboardRepository(string connectionString) : base(connectionString) { }
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

        internal DataSet GetDashboardQueueCount()
        {
            DataSet ds = new DataSet();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DashboardQueueCount";
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        internal DataSet GetDashboardBackgroundProcess()
        {
            DataSet ds = new DataSet();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "oi.DashboardBackgroundProcess";
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
    }
}
