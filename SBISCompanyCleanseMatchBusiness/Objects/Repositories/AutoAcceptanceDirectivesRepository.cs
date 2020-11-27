using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class AutoAcceptanceDirectivesRepository : RepositoryParent
    {
        public AutoAcceptanceDirectivesRepository(string connectionString) : base(connectionString) { }
        internal int UpdateAutoAcceptanceDirectives(AutoAcceptanceDirectivesEntity obj)
        {
            int result = 0;
            try
            {
                var dict = obj.ToDictionary();

                for (int index = 1; index <= 14; index++)
                {
                        StoredProcedureEntity sproc = new StoredProcedureEntity();
                        sproc.StoredProcedureName = "dnb.UpdateAutoAcceptanceDirective";
                        object Id = dict["AutoAcceptanceDirectiveId" + index];
                        object IsActive = dict["Active" + index];
                        object Tags = "";
                        if(index != 7)
                        {
                            Tags = dict["Tags" + index];
                        }
                        sproc.StoredProceduresParameter.Add(GetParam("@AutoAcceptanceDirectiveId", Convert.ToString(Id), SQLServerDatatype.IntDataType));
                        sproc.StoredProceduresParameter.Add(GetParam("@Active", Convert.ToString(IsActive), SQLServerDatatype.BitDataType));
                        sproc.StoredProceduresParameter.Add(GetParam("@Tags", Convert.ToString(Tags), SQLServerDatatype.VarcharDataType));
                        result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        internal List<AutoAcceptanceDirectives> GetAllAutoAcceptanceDirectives()
        {
            List<AutoAcceptanceDirectives> results = new List<AutoAcceptanceDirectives>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllAutoAcceptanceDirectives";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    AutoAcceptanceDirectivesAdapter ta = new AutoAcceptanceDirectivesAdapter();
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
            return results;
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
