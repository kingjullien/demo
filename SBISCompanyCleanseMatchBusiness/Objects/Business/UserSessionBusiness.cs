using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class UserSessionBusiness : BusinessParent
    {
        UserSessionRepository rep;
        public UserSessionBusiness(string connectionString) : base(connectionString) { rep = new UserSessionRepository(Connection); }

        public UserSessionFilterEntity GetUserSessionFilterByUserId(int UserID)
        {
            return rep.GetUserSessionFilterByUserId(UserID);
        }

        public List<CountryEntity> GetCountries()
        {
            List<CountryEntity> results = new List<CountryEntity>();
            results = rep.GetCountries();
            return results;
        }

        public void InsertOrUpdateUserSessionFilter(UserSessionFilterEntity obj)
        {
            rep.InsertOrUpdateUserSessionFilter(obj);
        }
    }
}
