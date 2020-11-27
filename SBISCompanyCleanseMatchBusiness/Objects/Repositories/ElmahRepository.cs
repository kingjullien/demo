using System;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class ElmahRepository : RepositoryParent
    {
        public ElmahRepository(string connectionString) : base(connectionString) { }

        internal void DeleteElmahErrorLogs(int DeleteElmahLogsDays)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.DeleteElmahErrorLogs";
                sproc.StoredProceduresParameter.Add(GetParam("@DeleteElmahLogsDays", DeleteElmahLogsDays.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {
                throw;
            }
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
