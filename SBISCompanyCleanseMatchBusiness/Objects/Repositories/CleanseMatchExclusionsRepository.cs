using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class CleanseMatchExclusionsRepository : RepositoryParent
    {
        public CleanseMatchExclusionsRepository(string connectionString) : base(connectionString) { }
        internal int UpdateCleanseMatchExclusions(CleanseMatchExclusionsEntity obj)
        {
            int result = 0;
            try
            {
                var dict = obj.ToDictionary();
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateCleanseMatchExclusions";
                for (int index = 1; index <= 5; index++)
                {
                    object Id = dict["CleanseMatchExclusionId" + index];
                    object IsActive = dict["Active" + index];
                    object Tags = dict["Tags" + index];
                    sproc.StoredProceduresParameter.Add(GetParam("@CleanseMatchExclusionId", Convert.ToString(Id), SQLServerDatatype.IntDataType));
                    sproc.StoredProceduresParameter.Add(GetParam("@Active", Convert.ToString(IsActive), SQLServerDatatype.BitDataType));
                    sproc.StoredProceduresParameter.Add(GetParam("@Tags", Convert.ToString(Tags), SQLServerDatatype.VarcharDataType));
                    result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
                    sproc = new StoredProcedureEntity();
                    sproc.StoredProcedureName = "dnb.UpdateCleanseMatchExclusions";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        internal List<CleanseMatchExclusions> GetAllCleanseMatchExclusions()
        {
            List<CleanseMatchExclusions> results = new List<CleanseMatchExclusions>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllCleanseMatchExclusions";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    CleanseMatchExclusionsAdapter ta = new CleanseMatchExclusionsAdapter();
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
