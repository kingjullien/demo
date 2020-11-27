using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories.OI;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business.OI
{
    public class OICompanyBusiness : BusinessParent
    {
        OICompanyRepository rep;
        public OICompanyBusiness(string connectionString) : base(connectionString) { rep = new OICompanyRepository(Connection); }
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
