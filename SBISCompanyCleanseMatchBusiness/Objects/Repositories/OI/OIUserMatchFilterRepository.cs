using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class OIUserMatchFilterRepository : RepositoryParent
    {
        public OIUserMatchFilterRepository(string connectionString) : base(connectionString) { }
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

        internal int InsertUpdateUserMatchFilter(OIUserMatchFilterEntity obj)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.InsertUpdateUserMatchFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodeMin", obj.ConfidenceCodeMin.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodeMax", obj.ConfidenceCodeMax.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Company", obj.MG_Company.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetNo", obj.MG_StreetNo.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_StreetName", obj.MG_StreetName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_City", obj.MG_City.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_State", obj.MG_State.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_PostalCode", obj.MG_PostalCode.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Phone", obj.MG_Phone.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Webdomain", obj.MG_Webdomain.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_Country", obj.MG_Country.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MG_EIN", obj.MG_EIN.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Company", obj.MDP_Company.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MDP_Webdomain", obj.MDP_Webdomain.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Score_Company", obj.Score_Company.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Score_StreetName", obj.Score_StreetName.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ActiveOnly", obj.ActiveOnly.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Exclude", obj.Exclude.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilterId", obj.FilterId == 0 ? null : obj.FilterId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Enabled", obj.Enabled.ToString(), SQLServerDatatype.BitDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        internal void EnableDisableUserMatchFilter(int FilterId, int UserId, bool Enabled)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.EnableDisableUserMatchFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@FilterId", FilterId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Enabled", Enabled.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal void DeleteUserMatchFilter(int FilterId, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.DeleteUserMatchFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@FilterId", FilterId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal List<OIUserMatchFilterEntity> GetUserMatchFilterList(int UserId)
        {

            List<OIUserMatchFilterEntity> results = new List<OIUserMatchFilterEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.GetUserMatchFilterList";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new OIUserMatchFilterAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal OIUserMatchFilterEntity GetUserMatchFilterById(int UserId, int FilterId)
        {

            OIUserMatchFilterEntity results = new OIUserMatchFilterEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "OI.GetUserMatchFilterById";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilterId", FilterId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    OIUserMatchFilterAdapter ta = new OIUserMatchFilterAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = ta.AdaptItem(rw);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }



    }
}
