using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ComplainceScreeningBusiness : BusinessParent
    {
        ComplainceScreeningRepository rep;
        public ComplainceScreeningBusiness(string connectionString) : base(connectionString) { rep = new ComplainceScreeningRepository(Connection); }
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
