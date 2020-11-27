using SBISCompanyCleanseMatchBusiness.Objects.Business.OI;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;

namespace SBISCompanyCleanseMatchFacade.Objects.OI
{
    public class OICompanyFacade : FacadeParent
    {
        OICompanyBusiness rep;
        public OICompanyFacade(string connectionString, string UserName) : base(connectionString, UserName) { rep = new OICompanyBusiness(Connection); }

        public int TotalRecord = 0;
        #region "Session"
        // DB Changes (MP-716)
        public OIUserSessionFilterEntity OIGetUserSessionFilterText(int UserID)
        {
            return rep.OIGetUserSessionFilterText(UserID);
        }
        public void OIDeleteUserSessionFilter(int UserId)
        {
            rep.OIDeleteUserSessionFilter(UserId);
        }
        #endregion

        #region Delete Records
        public void DeleteCompanyData(int UserId, bool DeleteWithCandidates, bool DeleteWithoutCandidates, string InputId, string SrcRecordId, string City, string State, string CountryCode, string Tag, int CountryGroupId, string ImportProcess, bool GetCountOnly)
        {
            rep.DeleteCompanyData(UserId, DeleteWithCandidates, DeleteWithoutCandidates, InputId, SrcRecordId, City, State, CountryCode, Tag, CountryGroupId, ImportProcess, GetCountOnly);
        }
        #endregion

        #region MP-770 Add reject data in Match data (ORB)
        public string RejectCandidate(int InputId, string orb_num)
        {
            string Message = rep.RejectCandidate(InputId, orb_num);
            return Message;
        }
        #endregion
    }
}
