using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class PreviewMatchDataBusiness : BusinessParent
    {
        PreviewMatchDataRepository rep;
        public PreviewMatchDataBusiness(string connectionString) : base(connectionString) { rep = new PreviewMatchDataRepository(Connection); }

        public DataSet SearchPreviewMatchData(string Tag, string ImportProcess, string LOBTag, string SrcRecID, bool SrcRecIdExactMatch, string ConfidenceCodes, string AcceptedBy, int UserId, int PageNumber, int PageSize, ref int totalRecord)
        {
            try
            {
                List<PreviewMatchDataAdapter> lst = new List<PreviewMatchDataAdapter>();
                return rep.SearchPreviewMatchData(Tag, ImportProcess, LOBTag, SrcRecID, SrcRecIdExactMatch, ConfidenceCodes, AcceptedBy, UserId, PageNumber, PageSize, out totalRecord);
            }
            catch
            {
                throw;
            }
        }
        public DataSet PreviewEnrichmentData(string DunsNumber)
        {
            return rep.PreviewEnrichmentData(DunsNumber);
        }
    }
}
