using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class AutoAcceptanceDirectivesFacade : FacadeParent
    {
        AutoAcceptanceDirectivesBusiness rep;
        public AutoAcceptanceDirectivesFacade(string connectionString) : base(connectionString) { rep = new AutoAcceptanceDirectivesBusiness(Connection); }

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
