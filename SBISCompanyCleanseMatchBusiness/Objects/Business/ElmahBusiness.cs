using SBISCompanyCleanseMatchBusiness.Objects.Repositories;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ElmahBusiness : BusinessParent
    {
        ElmahRepository rep;
        public ElmahBusiness(string connectionString) : base(connectionString) { rep = new ElmahRepository(Connection); }

        public void DeleteElmahErrorLogs(int DeleteElmahLogsDays)
        {
            rep.DeleteElmahErrorLogs(DeleteElmahLogsDays);
        }
    }
}
