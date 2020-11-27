using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class FeedbackRepository
    {
        SqlHelper sql;
        internal FeedbackRepository()
        {
            sql = new SqlHelper(General.masterDatabaseConnectionString);
        }
        internal int InsertUpdateFeedback(FeedbackEntity objFeedback)
        {
            int result = 0;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "capp.InsertFeedback";  // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@ClientInformation", objFeedback.ClientInformation.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EnteredBy", objFeedback.EnteredBy.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@EmailAddress", objFeedback.EmailAddress.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FeedbackType", objFeedback.FeedbackType.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Feedback", objFeedback.Feedback.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FeedbackPath", objFeedback.FeedbackPath.ToString(), SQLServerDatatype.VarcharDataType));
                result = Convert.ToInt32(sql.ExecuteScalar(CommandType.StoredProcedure, sproc));
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }
        #region Feedback
        // Gets All Feedback
        internal List<FeedbackEntity> GetAllFeedback(int SortOrder, int PageNumber, int PageSize, out int TotalRecords, string HostName, string FeedbackType)
        {
            List<FeedbackEntity> results = new List<FeedbackEntity>();
            try
            {
                TotalRecords = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "mapp.GetAllFeedback";   // MP-846 Admin database cleanup and code cleanup.
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                sproc.StoredProceduresParameter.Add(GetParam("@Host", string.IsNullOrEmpty(HostName) ? null : HostName.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@FeedbackType", string.IsNullOrEmpty(FeedbackType) ? null : FeedbackType.ToString(), SQLServerDatatype.VarcharDataType));
                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new FeedbackAdapter().Adapt(dt);
                    TotalRecords = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return results;
        }
        #endregion
        #region "Comman Method"
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
