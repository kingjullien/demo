using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class OIAPICallAuditBusiness : BusinessParent
    {
        OIAPICallAuditRepository rep;
        public OIAPICallAuditBusiness(string connectionString) : base(connectionString) { rep = new OIAPICallAuditRepository(Connection); }

        public int InsertAPICallLog(OIAPICallAuditEntity objAPICallLog)
        {
            return rep.InsertAPICallLog(objAPICallLog);
        }
    }
}
