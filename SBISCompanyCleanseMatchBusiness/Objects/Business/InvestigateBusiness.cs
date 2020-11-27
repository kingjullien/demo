using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class InvestigateBusiness : BusinessParent
    {
        InvestigateRepository rep;
        public InvestigateBusiness(string connectionString) : base(connectionString) { rep = new InvestigateRepository(Connection); }

        #region OI Investiage Reports
        public List<InvestigateViewEntity> GetCompanyInvestigationPaging(int SortOrder, int PgaeIndex, int PageSize, out int TotalCount)
        {
            List<InvestigateViewEntity> results = new List<InvestigateViewEntity>();
            results = rep.GetCompanyInvestigationPaging(SortOrder, PgaeIndex, PageSize, out TotalCount);
            return results;
        }
        #endregion
    }
}
