using System;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Repositories
{
    internal class PreviewMatchDataRepository : RepositoryParent
    {
        public PreviewMatchDataRepository(string connectionString) : base(connectionString) { }

        private StoredProceduresParameterEntity GetParam(string ParameterName, string ParameterValue, SQLServerDatatype DataType, ParameterDirection Direction = ParameterDirection.Input)
        {
            StoredProceduresParameterEntity param = new StoredProceduresParameterEntity();
            param.ParameterName = ParameterName;
            param.ParameterValue = ParameterValue;
            param.Datatype = DataType;
            param.Direction = Direction;
            return param;
        }
        internal DataSet SearchPreviewMatchData(string Tag, string ImportProcess, string LOBTag, string SrcRecID, bool SrcRecIdExactMatch, string ConfidenceCodes, string AcceptedBy, int UserId, int PageNumber, int PageSize, out int TotalRecords)
        {
            try
            {
                TotalRecords = 0;

                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.PreviewCompanyData";
                sproc.StoredProceduresParameter.Add(GetParam("@LOBTag", !string.IsNullOrEmpty(LOBTag) ? LOBTag.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@Tags", !string.IsNullOrEmpty(Tag) ? Tag.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ImportProcess", !string.IsNullOrEmpty(ImportProcess) ? ImportProcess.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecordId", string.IsNullOrEmpty(SrcRecID) ? null : SrcRecID.ToString(), SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@SrcRecIdExactMatch", SrcRecIdExactMatch.ToString(), SQLServerDatatype.BitDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@ConfidenceCodes", !string.IsNullOrEmpty(ConfidenceCodes) ? ConfidenceCodes.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@AcceptedBy", !string.IsNullOrEmpty(AcceptedBy) ? AcceptedBy.ToString() : null, SQLServerDatatype.VarcharDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@UserId", UserId.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageNumber", PageNumber.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@PageSize", PageSize.ToString(), SQLServerDatatype.IntDataType));
                sproc.StoredProceduresParameter.Add(GetParam("@TotalRecords", TotalRecords.ToString(), SQLServerDatatype.IntDataType, ParameterDirection.Output));
                string outParam = "";
                DataSet ds;
                DataTable dt = new DataTable(), dtMessage = new DataTable();
                ds = sql.ExecuteDataSetWithOutputParam(CommandType.StoredProcedure, sproc, out outParam);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    TotalRecords = Convert.ToInt32(outParam);
                }
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        internal DataSet PreviewEnrichmentData(string DunsNumber)
        {
            DataSet ds;
            try
            {
                StoredProcedureEntity sproc = new StoredProcedureEntity();
                sproc.StoredProcedureName = "dnb.PreviewEnrichmentData";
                sproc.StoredProceduresParameter.Add(GetParam("@DUNSNumber", !string.IsNullOrEmpty(DunsNumber) ? DunsNumber.ToString() : null, SQLServerDatatype.VarcharDataType));
                ds = sql.ExecuteDataSet(CommandType.StoredProcedure, sproc);
            }
            catch (Exception)
            {

                throw;
            }
            return ds;
        }
    }
}
