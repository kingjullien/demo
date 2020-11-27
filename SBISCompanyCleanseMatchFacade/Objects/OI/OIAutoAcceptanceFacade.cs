using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OIAutoAcceptanceFacade : FacadeParent
    {
        OIAutoAcceptanceBusiness rep;
        public OIAutoAcceptanceFacade(string connectionString) : base(connectionString) { rep = new OIAutoAcceptanceBusiness(Connection); }

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
