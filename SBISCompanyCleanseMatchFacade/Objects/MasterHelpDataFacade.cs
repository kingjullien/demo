using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class MasterHelpDataFacade : FacadeParent
    {
        MasterHelpDataBussiness rep;
        public MasterHelpDataFacade(string connectionString, string UserName = "") : base(connectionString)
        {
            try
            {
                rep = new MasterHelpDataBussiness(StringCipher.Decrypt(Connection, General.passPhrase));
            }
            catch
            {
                rep = new MasterHelpDataBussiness(Connection);
            }
        }
        public List<MasterHelpDataEntity> GetActiveHelp()
        {
            //MasterHelpDataBussiness rep = new MasterHelpDataBussiness();
            List<MasterHelpDataEntity> results = new List<MasterHelpDataEntity>();
            results = rep.GetActiveHelp();
            return results;
        }
    }
}
