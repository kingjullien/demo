using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class GoogleAPIRepository : RepositoryParent
    {
        public GoogleAPIRepository(string connectionString) : base(connectionString) { }
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

        internal GoogleAPIEntity GetGoogleAPIs()
        {
            List<GoogleAPIEntity> results = new List<GoogleAPIEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetGoogleAPILicence";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    GoogleAPIAdapters ta = new GoogleAPIAdapters();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.Adapt(dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results.FirstOrDefault();
        }
        internal int InsertUpdateGoogleAPI(GoogleAPIEntity obj)
        {
            int result = 0;
            try
            {
                var dict = obj.ToDictionary();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.InsertUpdateGoogleAPILicence";
                if (obj.Id > 0)
                {
                    sproc.StoredProceduresParameter.Add(GetParam("@Id", Convert.ToString(obj.Id), DataType: SQLServerDatatype.IntDataType));
                }
                sproc.StoredProceduresParameter.Add(GetParam("@APIKey", Convert.ToString(obj.APIKey), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsDefault", Convert.ToString(obj.IsDefault), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal GoogleAPIEntity GetGoogleAPIById(int Id)
        {
            GoogleAPIEntity results = new GoogleAPIEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetGoogleAPILicence";
                sproc.StoredProceduresParameter.Add(GetParam("@Id", Id.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    GoogleAPIAdapters ta = new GoogleAPIAdapters();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.AdaptItem(rw, dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal string GetDefaultAPIKey()
        {
            string results = null;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetDefaultGoogleAPIKey";
                results = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc, DBIntent.Read.ToString()));
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
    }
}
