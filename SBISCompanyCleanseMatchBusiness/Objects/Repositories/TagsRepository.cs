using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class TagsRepository : RepositoryParent
    {
        public TagsRepository(string connectionString) : base(connectionString) { }

        internal List<TagsEntity> GetAllTags(string LOBTag)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllTags";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<TagsEntity> GetAutoAcceptanceFilterTags(string LOBTag)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAutoAcceptanceFilterTags";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal void InsertTags(TagsEntity objTags, int UserId)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.InsertTag";
                sproc.StoredProceduresParameter.Add(GetParam("@TagValue", objTags.TagValue, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CreatedUserId", objTags.CreatedUserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", objTags.Tag, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TagTypeCode", objTags.TagTypeCode, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", objTags.LOBTag, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void UpdateTags(TagsEntity objTags)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();

                sproc.StoredProcedureName = "dnb.UpdateTag";
                sproc.StoredProceduresParameter.Add(GetParam("@TagId", objTags.TagId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TagValue", objTags.TagValue.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CreatedUserId", objTags.CreatedUserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tag", objTags.Tag.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TagTypeCode", objTags.TagTypeCode.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(objTags.LOBTag) ? null : Convert.ToString(objTags.LOBTag), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal DataTable GetTagTypeCode()
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetTagTypeCode]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal List<TagsEntity> GetTagByTypeCode(string TagTypeCode)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetTagByTypeCode]";
                sproc.StoredProceduresParameter.Add(GetParam("@TagTypeCode", TagTypeCode.ToString(), SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal DataTable GetTagsByTypeCode(string TagTypeCode)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetTagByTypeCode]";
                sproc.StoredProceduresParameter.Add(GetParam("@TagTypeCode", TagTypeCode.ToString(), SQLServerDatatype.VarcharDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }
        internal string DeleteTag(int TagId, int UserId)
        {
            string message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteTag";
                sproc.StoredProceduresParameter.Add(GetParam("@TagId", TagId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", Convert.ToString(UserId), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<TagsEntity> GetAllTagsListPaging()
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetTagListPaging";

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal List<TagsEntity> GetExportDataTags(string LOBTag, string SecurityTags, int UserId)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetExportDataTags]";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SecurityTags", string.IsNullOrEmpty(SecurityTags) ? null : Convert.ToString(SecurityTags), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal DataTable GetExportDataTag(string LOBTag, string SecurityTags, int UserId)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetExportDataTags]";
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        internal TagsEntity GetTagByTagId(int TagId)
        {
            TagsEntity results = new TagsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetTagByTagId";
                sproc.StoredProceduresParameter.Add(GetParam("@TagId", TagId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    TagsAdapter ta = new TagsAdapter();
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
        internal List<TagsEntity> GetExportedDataTags(string LOBTag, string SecurityTags, int UserId)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetExportedDataTags]";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SecurityTags", string.IsNullOrEmpty(SecurityTags) ? null : Convert.ToString(SecurityTags), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal List<TagsEntity> GetAllTagsForUser(string LOBTag, int UserId, bool FilterNoTag)
        {
            List<TagsEntity> results = new List<TagsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetAllTagsForUser";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilterNoTag", FilterNoTag.ToString(), SQLServerDatatype.BitDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new TagsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        internal DataTable GetAllTagsForUserInFilter(string LOBTag, int UserId, bool FilterNoTag)
        {
            DataTable dt = new DataTable();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[dnb].[GetAllTagsForUser]";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", string.IsNullOrEmpty(LOBTag) ? null : Convert.ToString(LOBTag), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FilterNoTag", FilterNoTag.ToString(), SQLServerDatatype.BitDataType));
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
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
