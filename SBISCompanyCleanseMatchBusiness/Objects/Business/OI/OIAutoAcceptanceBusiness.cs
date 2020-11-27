using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class OIAutoAcceptanceBusiness : BusinessParent
    {
        OIAutoAcceptanceRepository rep;
        public OIAutoAcceptanceBusiness(string connectionString) : base(connectionString) { rep = new OIAutoAcceptanceRepository(Connection); }

        public int InsertUpdateAutoAcceptanceRules(OIAutoAcceptanceEntity obj)
        {
            return rep.InsertUpdateAutoAcceptanceRules(obj);
        }
        public List<OIAutoAcceptanceEntity> GetAutoAcceptanceRulesPaging(int PageSize, int PageNumber, out int TotalRecords)
        {
            return rep.GetAutoAcceptanceRulesPaging(PageSize, PageNumber, out TotalRecords);
        }
        public OIAutoAcceptanceEntity GetAutoAcceptanceRuleById(int RuleId)
        {
            return rep.GetAutoAcceptanceRuleById(RuleId);
        }
        public void DeleteAutoAcceptance(string RuleId)
        {
            rep.DeleteAutoAcceptance(RuleId);
        }
    }
}