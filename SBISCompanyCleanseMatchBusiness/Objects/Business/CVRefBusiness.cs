using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class CVRefBusiness : BusinessParent
    {
        CVRefRepository rep;
        public CVRefBusiness(string connectionString) : base(connectionString) { rep = new CVRefRepository(Connection); }
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
