using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class CleanseMatchExclusionsFacade : FacadeParent
    {
        CleanseMatchExclusionsBusiness rep;
        public CleanseMatchExclusionsFacade(string connectionString) : base(connectionString) { rep = new CleanseMatchExclusionsBusiness(Connection); }

        public void UpdateCleanseMatchExclusions(CleanseMatchExclusionsEntity obj)
        {
            rep.UpdateCleanseMatchExclusions(obj);
        }

        public List<CleanseMatchExclusions> GetAllCleanseMatchExclusions()
        {
            return rep.GetAllCleanseMatchExclusions();
        }
    }
}
