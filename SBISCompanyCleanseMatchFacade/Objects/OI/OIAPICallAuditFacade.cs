using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class OIAPICallAuditFacade : FacadeParent
    {
        OIAPICallAuditBusiness rep;
        public OIAPICallAuditFacade(string connectionString) : base(connectionString) { rep = new OIAPICallAuditBusiness(Connection); }

        public int InsertAPICallLog(OIAPICallAuditEntity objAPICallLog)
        {
            return rep.InsertAPICallLog(objAPICallLog);
        }
    }
}
