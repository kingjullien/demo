using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class UserSessionRepository : RepositoryParent
    {
        public UserSessionRepository(string connectionString) : base(connectionString) { }



        internal UserSessionFilterEntity GetUserSessionFilterByUserId(int UserID)
        {
            UserSessionFilterEntity results = new UserSessionFilterEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserSessionFilterByUserId";

                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                param.ParameterName = "@UserId";
                param.ParameterValue = UserID.ToString();
                sproc.StoredProceduresParameter.Add(param);

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UserSessionFilterAdapter().Adapt(dt).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }


        internal List<CountryEntity> GetCountries()
        {
            List<CountryEntity> results = new List<CountryEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetCountryList";

                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new CountryAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }


        internal void InsertOrUpdateUserSessionFilter(UserSessionFilterEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.AddUserSessionFilter";
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", obj.UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", (obj.SrcRecordId != null) ? obj.SrcRecordId.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CompanyName", (obj.CompanyName != null) ? obj.CompanyName.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@City", (obj.City != null) ? obj.City.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@State", (obj.State != null) ? obj.State.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryISOAlpha2Code", (obj.CountryISOAlpha2Code != null) ? obj.CountryISOAlpha2Code.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@OrderByColumn", (obj.OrderByColumn != null) ? obj.OrderByColumn.ToString().Trim() : "SrcRecordId", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", (obj.Tag != null) ? obj.Tag.ToString().Trim() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CountryGroupId", (obj.CountryGroupId > 0) ? obj.CountryGroupId.ToString() : null, SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", (obj.ImportProcess != null) ? obj.ImportProcess.ToString().Trim() : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ErrorCode", (!string.IsNullOrEmpty(obj.ErrorCode)) ? obj.ErrorCode.Trim() : null, SQLServerDatatype.VarcharDataType));

                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            return param;
        }
    }
}
