using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ElmahFacade : FacadeParent
    {
        ElmahBusiness rep;
        public ElmahFacade(string connectionString) : base(connectionString)
        {
            try
            {
                rep = new ElmahBusiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new ElmahBusiness(Connection);
            }
        }
        // Deletes elmah error logs
        public void DeleteElmahErrorLogs(int DeleteElmahLogsDays)
        {
            rep.DeleteElmahErrorLogs(DeleteElmahLogsDays);
        }

    }
}
