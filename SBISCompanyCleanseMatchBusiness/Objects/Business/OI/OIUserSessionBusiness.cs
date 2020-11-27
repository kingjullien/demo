using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories.OI;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business.OI
{
    public class OIUserSessionBusiness : BusinessParent
    {
        // DB Changes (MP-716)
        OIUserSessionRepository rep;
        public OIUserSessionBusiness(string connectionString) : base(connectionString) { rep = new OIUserSessionRepository(Connection); }

        public OIUserSessionFilterEntity GetUserSessionFilterByUserId(int UserID)
        {
            return rep.GetUserSessionFilterByUserId(UserID);
        }

        public List<CountryEntity> GetCountries()
        {
            List<CountryEntity> results = new List<CountryEntity>();
            results = rep.GetCountries();
            return results;
        }

        public void InsertOrUpdateUserSessionFilter(OIUserSessionFilterEntity obj)
        {
            rep.InsertOrUpdateUserSessionFilter(obj);
        }
    }
}
