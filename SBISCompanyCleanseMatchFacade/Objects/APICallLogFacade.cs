using SBISCompanyCleanseMatchBusiness.Objects.Business;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class APICallLogFacade : FacadeParent
    {
        APICallLogBusiness rep;
        public APICallLogFacade(string connectionString) : base(connectionString) { rep = new APICallLogBusiness(Connection); }
    }
}
