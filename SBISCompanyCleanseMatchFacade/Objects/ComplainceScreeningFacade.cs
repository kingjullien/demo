using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ComplainceScreeningFacade : FacadeParent
    {
        ComplainceScreeningBusiness rep;
        public ComplainceScreeningFacade(string connectionString) : base(connectionString) { rep = new ComplainceScreeningBusiness(Connection); }
        public List<ComplainceScreeningEntity> GetComplainceScreening()
        {
            return rep.GetComplainceScreening();
        }
        public void UpdateComplainceScreeningCompleted(int Id, bool IsProcessComplete)
        {
            rep.UpdateComplainceScreeningCompleted(Id, IsProcessComplete);
        }
    }
}
