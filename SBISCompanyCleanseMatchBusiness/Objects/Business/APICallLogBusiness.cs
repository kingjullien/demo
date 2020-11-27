using SBISCompanyCleanseMatchBusiness.Objects.Repositories;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class APICallLogBusiness : BusinessParent
    {
        APICallLogRepository rep;
        public APICallLogBusiness(string connectionString) : base(connectionString) { rep = new APICallLogRepository(Connection); }
    }
}
