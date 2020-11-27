using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OICompanyMatchFacade : FacadeParent
    {
        OICompanyMatchBusiness rep;
        public OICompanyMatchFacade(string connectionString) : base(connectionString) { rep = new OICompanyMatchBusiness(Connection); }
        #region "OI"

        public LstOIMatchCompany GetOICompanyList(int UserID, bool IncludeWithCandidates, bool IncludeWithoutCandidates, int PgaeIndex, int PageSize, out int TotalCount)
        {
            return rep.GetOICompanyList(UserID, IncludeWithCandidates, IncludeWithoutCandidates, PgaeIndex, PageSize, out TotalCount);
        }
        public OIlstMatchDetails GetCompanyMatchDetails(int inputId, int UserId, bool ApplyFilter, string MatchIds = null)
        {
            return rep.GetCompanyMatchDetails(inputId, UserId, ApplyFilter, MatchIds);
        }
        public string StewDeleteOIMatch(int InputId, int MatchId)
        {
            return rep.StewDeleteOIMatch(InputId, MatchId);
        }
        public string StewUndoOIMatch(int InputId, int MatchId)
        {
            return rep.StewUndoOIMatch(InputId, MatchId);
        }
        public string GetNewSearch(int InputId, string MatchURL, string ResponseJSON)
        {
            return rep.GetNewSearch(InputId, MatchURL, ResponseJSON);
        }
        public string AssignStewMatchRecord(int inputId, string OrbNum, int UserId)
        {
            return rep.AssignStewMatchRecord(inputId, OrbNum, UserId);
        }
        public OIlstMatchMetaDetails GetStewOIMatchMetadata(int inputId, string OrbNum)
        {
            return rep.GetStewOIMatchMetadata(inputId, OrbNum);
        }
        #endregion
        public bool ValidateCompanySrcId(string SrcRecordId)
        {
            try
            {
                rep.ValidateCompanySrcId(SrcRecordId);
                return false;
            }
            catch (SqlException)
            {
                throw;
            }
        }
        #region OI Search Data Add Company
        public string OIAddRecordAsNewCompany(string MatchURL, string ResponseJSON, string OrbNumber, string SrcRecordId, string Tags, int UserId)
        {
            return rep.OIAddRecordAsNewCompany(MatchURL, ResponseJSON, OrbNumber, SrcRecordId, Tags, UserId);
        }
        #endregion
        #region OI Match Data Add Company
        public string OIAddRecordAsNewCompanyFromMatch(string InputId, string OrbNumber, string SrcRecordId, string Tags, int UserId)
        {
            return rep.OIAddRecordAsNewCompanyFromMatch(InputId, OrbNumber, SrcRecordId, Tags, UserId);
        }
        #endregion


        #region "Window Close Event"
        public void StewUserActivityCloseWindowOI(int UserId)
        {
            rep.StewUserActivityCloseWindowOI(UserId);
        }
        #endregion
        #region "Delete Company Data"
        public string DeleteCompanyData(OIExportToExcel Model)
        {
            return rep.DeleteCompanyData(Model);
        }

        #endregion
        #region "Refresh Stewardship Queue"
        public string StewRefreshUserStewardshipList(int UserId)
        {
            return rep.StewRefreshUserStewardshipList(UserId);
        }
        #endregion

    }
}
