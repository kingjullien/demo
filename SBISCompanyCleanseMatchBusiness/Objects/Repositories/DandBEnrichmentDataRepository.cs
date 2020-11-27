using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    class DandBEnrichmentDataRepository : RepositoryParent
    {
        public DandBEnrichmentDataRepository(string ConnectionString) : base(ConnectionString) { }
        internal DataTable GetAPILayers()
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAPILayers";
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal List<DandBEnrichmentDataEntity> FindMetaData(string APILayer, string JSONPath)
        {
            List<DandBEnrichmentDataEntity> result = new List<DandBEnrichmentDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.FindMetaData";
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", string.IsNullOrEmpty(APILayer) ? null : APILayer.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@JSONPath", string.IsNullOrEmpty(JSONPath) ? "" : JSONPath.ToString(), SQLServerDatatype.VarcharDataType));

                DataTable dt = new DataTable();
                DataRow[] rows = dt.Select();

                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = new DandBEnrichmentDataAdapter().AdaptLists(dt, APILayer);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        internal string AddNewMappingMetaData(DandBEnrichmentDataEntity obj)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.AddNewMappingMetaData";
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", obj.APILayer.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@JSONPath", obj.JSONPath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SpecialHandling", !string.IsNullOrEmpty(obj.SpecialHandling) ? Convert.ToString(obj.SpecialHandling) : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MBSColumnName", obj.MBSColumnName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DataType", Convert.ToString(obj.DataType), SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
        internal string EditMappingMetaData(DandBEnrichmentDataEntity obj)
        {
            string result = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.EditMappingMetaData";
                sproc.StoredProceduresParameter.Add(GetParam("@MappingId", obj.MappingId.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@JSONPath", obj.JSONPath.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SpecialHandling", !string.IsNullOrEmpty(obj.SpecialHandling) ? Convert.ToString(obj.SpecialHandling) : "", SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MBSColumnName", obj.MBSColumnName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DataType", Convert.ToString(obj.DataType), SQLServerDatatype.VarcharDataType));
                result = Convert.ToString(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
        internal void UpdateMapping(string APILayer, int MappingId, bool Selected)
        {
            DataSet ds = new DataSet();
            List<DandBEnrichmentDataEntity> result = new List<DandBEnrichmentDataEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateMapping";
                sproc.StoredProceduresParameter.Add(GetParam("@APILayer", string.IsNullOrEmpty(APILayer) ? null : APILayer.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@MappingId", MappingId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Selected", Selected.ToString(), SQLServerDatatype.BitDataType));

                sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //internal DandBEnrichmentDataEntity FindMetadataByMappingID(int MappingId)
        //{
        //    DandBEnrichmentDataEntity result = new DandBEnrichmentDataEntity();
        //    try
        //    {
        //        StoredProcedureEntity sproc = new StoredProcedureEntity();
        //        sproc.StoredProcedureName = "dnb.FindMetadataByMappingID";
        //        sproc.StoredProceduresParameter.Add(GetParam("@MappingId", MappingId.ToString().Trim(), SQLServerDatatype.IntDataType));
        //        string outParam = "";
        //        DataTable dt;
        //        dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc);
        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            result = (from DataRow dr in dt.Rows
        //                       select new DandBEnrichmentDataEntity()
        //                       {
        //                           MappingId = Convert.ToInt32(dr["MappingId"]),
        //                           JSONPath = dr["JSONPath"].ToString(),
        //                           MBSColumnName = dr["MBSColumnName"].ToString(),
        //                           SpecialHandling = dr["SpecialHandling"].ToString(),
        //                           DataType = dr["DataType"].ToString()
        //                       }).FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return result;
        //}
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

