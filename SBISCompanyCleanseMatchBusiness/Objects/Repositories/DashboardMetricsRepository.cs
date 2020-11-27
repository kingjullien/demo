using System;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{

    internal class DashboardMetricsRepository : RepositoryParent
    {
        public DashboardMetricsRepository(string connectionString) : base(connectionString) { }

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


        internal DataTable GetOperationsMonitoringMetrics()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "aud.OperationsMonitoringMetrics";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                return dt;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}