using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class AutoAcceptanceDirectivesBusiness : BusinessParent
    {
        AutoAcceptanceDirectivesRepository rep;
        public AutoAcceptanceDirectivesBusiness(string connectionString) : base(connectionString) { rep = new AutoAcceptanceDirectivesRepository(Connection); }

        public void UpdateAutoAcceptanceDirectives(AutoAcceptanceDirectivesEntity obj)
        {
            rep.UpdateAutoAcceptanceDirectives(obj);
        }

        public List<AutoAcceptanceDirectives> GetAllAutoAcceptanceDirectives()
        {
            return rep.GetAllAutoAcceptanceDirectives();
        }
    }
}
