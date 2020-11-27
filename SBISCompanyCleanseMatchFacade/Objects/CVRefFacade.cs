using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class CVRefFacade : FacadeParent
    {
        CVRefBusiness rep;
        public CVRefFacade(string connectionString) : base(connectionString) { rep = new CVRefBusiness(Connection); }
        public List<CVRefEntity> GetAPItype(int typeCode)
        {
            return rep.GetAPItype(typeCode);
        }

        public List<CVRefEntity> GetThirdPartyProviders(int fullList)
        {
            return rep.GetThirdPartyProviders(fullList);
        }

        public DataTable GetThirdPartyProviderEnrichments(int typeCode)
        {
            return rep.GetThirdPartyProviderEnrichments(typeCode);
        }
    }
}
