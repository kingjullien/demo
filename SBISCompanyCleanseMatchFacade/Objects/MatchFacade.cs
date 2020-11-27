using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MatchFacade : FacadeParent
    {
        MatchBusiness match;
        public MatchFacade(string connectionString) : base(connectionString) { match = new MatchBusiness(Connection); }
        public List<MDPCodeEntity> GetMDPCodes()
        {
            List<MDPCodeEntity> results = new List<MDPCodeEntity>();
            results = match.GetMDPCodes();

            return results;
        }
        public List<MatchCodeEntity> GetMDPValues()
        {
            List<MatchCodeEntity> results = new List<MatchCodeEntity>();
            results = match.GetMDPValues();
            return results;
        }
        public void AddCompanyRecord(MatchEntity Match, int UserId)
        {
            match.AddCompanyRecord(Match, UserId);
        }

        public string EncodeURL(string value)
        {
            return match.EncodeURL(value);
        }
        // Validate SrcId for checking duplicate records at "Add Match as a new Company".
        public bool ValidateCompanySrcId(string SrcRecordId)
        {
            try
            {
                match.ValidateCompanySrcId(SrcRecordId);
                return false;
            }
            catch (SqlException)
            {
                throw;
            }
        }
    }
}
