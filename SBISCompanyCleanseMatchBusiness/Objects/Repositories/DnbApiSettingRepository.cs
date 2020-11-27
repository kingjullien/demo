using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class DnbApiSettingRepository : RepositoryParent
    {
        public DnbApiSettingRepository(string connectionString) : base(connectionString) { }

        internal List<DnbAPIEntity> GetDnBAPIDetailList()
        {
            List<DnbAPIEntity> results = new List<DnbAPIEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetDnBAPIDetailList";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DnbAPIAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal int UpdateDnBAPIFamily(int DnBAPIId, string APIFamily)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateDnBAPIFamily";
                sproc.StoredProceduresParameter.Add(GetParam("@DnBAPIId", DnBAPIId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIFamily", APIFamily != null ? APIFamily.ToString() : "", SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
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
