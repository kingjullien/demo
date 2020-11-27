using SBISCompanyCleanseMatchBusiness.Objects.Business;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class PreviewMatchDataFacade : FacadeParent
    {
        PreviewMatchDataBusiness rep;
        public PreviewMatchDataFacade(string connectionString, string UserName) : base(connectionString, UserName) { rep = new PreviewMatchDataBusiness(Connection); }
        public DataSet SearchPreviewMatchData(string Tag, string ImportProcess, string LOBTag, string SrcRecID, bool SrcRecIdExactMatch, string ConfidenceCodes, string AcceptedBy, int UserId, int PageNumber, int PageSize, ref int totalRecord)
        {
            return rep.SearchPreviewMatchData(Tag, ImportProcess, LOBTag, SrcRecID, SrcRecIdExactMatch, ConfidenceCodes, AcceptedBy, UserId, PageNumber, PageSize, ref totalRecord);
        }
        public DataSet PreviewEnrichmentData(string DunsNumber)
        {
            return rep.PreviewEnrichmentData(DunsNumber);
        }
    }
}
