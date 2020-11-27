using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class UserSessionFacade : FacadeParent
    {
        UserSessionBusiness rep;
        public UserSessionFacade(string connectionString) : base(connectionString) { rep = new UserSessionBusiness(Connection); }
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
