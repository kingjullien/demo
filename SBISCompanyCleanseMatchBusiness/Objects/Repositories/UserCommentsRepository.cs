using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{

    internal class UserCommentsRepository : RepositoryParent
    {
        public UserCommentsRepository(string connectionString) : base(connectionString) { }

        internal List<UserCommentsEntity> GetAllUserCommentsListPaging()
        {
            List<UserCommentsEntity> results = new List<UserCommentsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserCommentsListPaging";
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UserCommentsAdapter().Adapt(dt);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }

        internal void InsertUserComments(UserCommentsEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.InsertUserComments";
                sproc.StoredProceduresParameter.Add(GetParam("@CommentType", obj.CommentType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Comment", obj.Comment.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal void UpdateUserComments(UserCommentsEntity obj)
        {
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.UpdateUserComments";
                sproc.StoredProceduresParameter.Add(GetParam("@CommentId", obj.CommentId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@CommentType", obj.CommentType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Comment", obj.Comment.ToString(), SQLServerDatatype.VarcharDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal List<UserCommentsEntity> GetUserCommentsByType(string CommentType)
        {
            List<UserCommentsEntity> results = new List<UserCommentsEntity>();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserCommentsByType";
                sproc.StoredProceduresParameter.Add(GetParam("@CommentType", CommentType, SQLServerDatatype.VarcharDataType));
                DataTable dt = new DataTable();
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "", DBIntent.Read.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new UserCommentsAdapter().Adapt(dt);
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal string DeleteUserComment(int commentId)
        {
            string message = string.Empty;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.DeleteUserComment";
                sproc.StoredProceduresParameter.Add(GetParam("@CommentId", commentId.ToString(), SQLServerDatatype.IntDataType));
                sql.ExecuteNoReturn(CommandType.StoredProcedure, sproc);
                return message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        internal UserCommentsEntity GetUserCommentsById(int commentId)
        {
            UserCommentsEntity results = new UserCommentsEntity();
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.GetUserCommentsById";
                sproc.StoredProceduresParameter.Add(GetParam("@CommentId", commentId.ToString(), SQLServerDatatype.IntDataType));
                DataTable dt;
                dt = sql.ExecuteDataTable(CommandType.StoredProcedure, sproc, "");
                if (dt != null && dt.Rows.Count > 0)
                {
                    UserCommentsAdapter ta = new UserCommentsAdapter();
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
