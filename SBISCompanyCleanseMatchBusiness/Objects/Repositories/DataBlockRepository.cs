using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class DataBlockRepository : RepositoryParent
    {
        internal DataBlockRepository(string connectionString) : base(connectionString) { }
        #region "Other Methods"
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
        #endregion

        internal List<DataBlocksEntity> GetAllDataBlocks()
        {
            List<DataBlocksEntity> results = new List<DataBlocksEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllDataBlocks";
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DataBlocksAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<DataBlockGroupsEntity> GetDataBlockGroups(int DataBlockGroupId)
        {
            List<DataBlockGroupsEntity> results = new List<DataBlockGroupsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetDataBlockGroupsByGroupId";
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlockGroupId", DataBlockGroupId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new DataBlockGroupsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal DataBlockGroupsEntity GetDataBlockGroupsByGroupId(int DataBlockGroupId)
        {
            DataBlockGroupsEntity results = new DataBlockGroupsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.GetDataBlockGroupsByGroupId";
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlockGroupId", DataBlockGroupId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow rw in dt.Rows)
                    {
                        results = new DataBlockGroupsAdapter().AdaptItem(rw);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal string DeleteDataBlockGroupsByGroupId(int DataBlockGroupId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.DeleteDataBlockGroupsByGroupId";
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlockGroupId", DataBlockGroupId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal string UpsertDataBlockGroups(DataBlockGroupsEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "cfg.UpsertDataBlockGroups";
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlockGroupId", obj.DataBlockGroupId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlockGroupName", obj.DataBlockGroupName, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlocksIds", obj.DataBlocksIds, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@DataBlocks", obj.DataBlocks, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@APIURL", obj.APIURL, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TradeUp", string.IsNullOrEmpty(obj.TradeUp) ? null : obj.TradeUp, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CustomerReference", string.IsNullOrEmpty(obj.CustomerReference) ? "" : obj.CustomerReference, SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
