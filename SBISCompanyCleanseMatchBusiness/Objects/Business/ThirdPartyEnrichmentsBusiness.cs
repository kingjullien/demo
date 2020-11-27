using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class ThirdPartyEnrichmentsBusiness : BusinessParent
    {
        ThirdPartyEnrichmentsRepository rep;
        public ThirdPartyEnrichmentsBusiness(string connectionString) : base(connectionString) { rep = new ThirdPartyEnrichmentsRepository(Connection); }

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
