using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class InvestigateFacade : FacadeParent
    {
        InvestigateBusiness rep;
        public InvestigateFacade(string connectionString) : base(connectionString) { rep = new InvestigateBusiness(Connection); }

        #region OI Investigate Reports
        public List<InvestigateViewEntity> GetCompanyInvestigationPaging(int SortOrder, int PgaeIndex, int PageSize, out int TotalCount)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            results = rep.GetCompanyInvestigationPaging(SortOrder, PgaeIndex, PageSize, out TotalCount);
            return results;
        }
        #endregion
    }
}
