using SBISCompanyCleanseMatchBusiness.Objects.Business.OI;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects.OI
{
    // DB Changes (MP-716)
    public class OIUserSessionFacade : FacadeParent
    {
        OIUserSessionBusiness rep;
        public OIUserSessionFacade(string connectionString) : base(connectionString) { rep = new OIUserSessionBusiness(Connection); }
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
