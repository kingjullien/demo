using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class MinConfidenceOverrideRepository : RepositoryParent
    {
        public MinConfidenceOverrideRepository(string connectionString) : base(connectionString) { }



        public List<MinConfidenceOverrideEntity> GetAllMinConfidenceOverrideListPaging(string LOBTag, int SortOrder, int PageNumber, int PageSize, out int TotalCount)
        {
            TotalCount = 0;
            List<MinConfidenceOverrideEntity> results = new List<MinConfidenceOverrideEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMinConfidenceOverrideListPaging";
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new MinConfidenceOverrideAdapter().Adapt(dt);
                }
                TotalCount = Convert.ToInt32(outParam);
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal MinConfidenceOverrideEntity GetAllMinConfidenceOverrideByID(int MinCCId)
        {
            MinConfidenceOverrideEntity results = new MinConfidenceOverrideEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetMinConfidenceOverrideByID";
                sproc.StoredProceduresParameter.Add(GetParam("@id", MinCCId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    MinConfidenceOverrideAdapter adta = new MinConfidenceOverrideAdapter();
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = adta.AdaptItem(rw, dt);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void InsertOrUpdateMinCCOverride(MinConfidenceOverrideEntity objMinCC)
        {

            string message = "";
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                if (objMinCC.Id > 0)
                {
                    sproc.StoredProcedureName = "dnb.InsertUpdateMinConfidenceCodeOverride";
                    sproc.StoredProceduresParameter.Add(GetParam("@id", objMinCC.Id.ToString(), SQLServerDatatype.IntDataType));
                }
                else
                {
                    sproc.StoredProcedureName = "dnb.InsertUpdateMinConfidenceCodeOverride";
                }

                sproc.StoredProceduresParameter.Add(GetParam("@MinConfidenceCode", objMinCC.MinConfidenceCode.ToString().Trim(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MaxCandidateQty", objMinCC.MaxCandidateQty.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", objMinCC.Tags.ToString().TrimEnd(','), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", objMinCC.userId.ToString().TrimEnd(','), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@IsActive", objMinCC.IsActive.ToString(), SQLServerDatatype.BitDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);

                message = "";
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw;
            }
        }
        internal void DeleteMinCCOverride(int Id)
        {
            StoredProcedureEntity sproc = new StoredProcedureEntity();
            sproc.StoredProcedureName = "dnb.DeleteMinConfidenceCodeOverride";
            sproc.StoredProceduresParameter.Add(GetParam("@id", Id.ToString(), SQLServerDatatype.IntDataType));
            sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
        }

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            return param;
        }
        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
    }
}
