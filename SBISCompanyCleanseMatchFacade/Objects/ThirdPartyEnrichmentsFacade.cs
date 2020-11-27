using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class ThirdPartyEnrichmentsFacade : FacadeParent
    {
        ThirdPartyEnrichmentsBusiness rep;
        public ThirdPartyEnrichmentsFacade(string connectionString) : base(connectionString) { rep = new ThirdPartyEnrichmentsBusiness(Connection); }
        public List<ThirdPartyEnrichmentsEntity> GetThirdPartyEnrichments()
        {
            return rep.GetThirdPartyEnrichments();
        }

        public ThirdPartyEnrichmentsEntity GetThirdPartyEnrichmentsByEnrichmentId(int enrichId)
        {
            return rep.GetThirdPartyEnrichmentsByEnrichmentId(enrichId);
        }

        public void UpsertThirdPartyEnrichments(ThirdPartyEnrichmentsEntity obj, int UserId)
        {
            rep.UpsertThirdPartyEnrichments(obj, UserId);
        }

        public void DeleteThirdPartyEnrichmentsByEnrichmentId(int enrichId, int UserId)
        {
            rep.DeleteThirdPartyEnrichmentsByEnrichmentId(enrichId, UserId);
        }

    }
}
