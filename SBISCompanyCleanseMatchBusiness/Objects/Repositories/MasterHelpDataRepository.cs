using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class MasterHelpDataRepository : RepositoryParent
    {
        public MasterHelpDataRepository(string ConnectionString) : base(ConnectionString) { }
        internal List<MasterHelpDataEntity> GetActiveHelp()
        {
            List<MasterHelpDataEntity> results = new List<MasterHelpDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.GetHelp";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MasterHelpDataAdepters().Adapt(dt);
                    foreach (MasterHelpDataEntity comp in results)
                    {
                        results = new MasterHelpDataAdepters().Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }

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
