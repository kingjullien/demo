using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class CleanseMatchExclusionsBusiness : BusinessParent
    {
        CleanseMatchExclusionsRepository rep;
        public CleanseMatchExclusionsBusiness(string connectionString) : base(connectionString) { rep = new CleanseMatchExclusionsRepository(Connection); }

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
