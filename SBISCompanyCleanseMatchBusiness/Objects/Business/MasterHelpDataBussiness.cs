using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class MasterHelpDataBussiness : BusinessParent
    {
        MasterHelpDataRepository rep;
        public MasterHelpDataBussiness(string connectionString) : base(connectionString) { rep = new MasterHelpDataRepository(Connection); }

        #region "helpData"
        public List<MasterHelpDataEntity> GetActiveHelp()
        {
            //MasterHelpDataRepository rep = new MasterHelpDataRepository();
            List<MasterHelpDataEntity> results = new List<MasterHelpDataEntity>();
            results = rep.GetActiveHelp();
            return results;
        }
        #endregion
    }
}
