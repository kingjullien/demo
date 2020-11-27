using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using static SBISCompanyCleanseMatchBusiness.Objects.Helpers.EnumHelper;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    public class InvestigateRepository : RepositoryParent
    {
        public InvestigateRepository(string connectionString) : base(connectionString) { }

        #region OI Investigate Reports
        internal List<InvestigateViewEntity> GetCompanyInvestigationPaging(int SortOrder, int PgaeIndex, int PageSize, out int TotalCount)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            try
            {
                TotalCount = 0;
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "[oi].[GetCompanyInvestigationPaging]";
                StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
                sproc.StoredProceduresParameter.Add(GetParam("@SortOrder", SortOrder.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PgaeIndex.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalCount.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));

                string outParam = "";
                DataTable dt;
                dt = sql.ExecuteDataTableWithOutputParam(CommandType.StoredProcedure, sproc, out outParam, "", DBIntent.ReadWrite.ToString());
                if (dt != null && dt.Rows.Count > 0)
                {
                    results = new InvestigateUserAdapter().AdaptLists(dt);
                    TotalCount = Convert.ToInt32(outParam);
                }
            }
            catch (Exception)
            {
                //Put log to db here
                throw;
            }
            return results;
        }
        #endregion
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
