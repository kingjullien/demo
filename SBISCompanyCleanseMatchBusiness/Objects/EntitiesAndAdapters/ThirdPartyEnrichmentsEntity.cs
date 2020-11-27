using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ThirdPartyEnrichmentsEntity
    {
        public int EnrichmentId { get; set; }
        public int CredentialId { get; set; }
        public string EnrichmentDescription { get; set; }
        public string EnrichmentURL { get; set; }
        public string Tags { get; set; }
        public int CountryGroupId { get; set; }
        public bool EnablePeriodicRefresh { get; set; }
        public int PeriodicRefreshIntervalDays { get; set; }
        public string CountryGroupName { get; set; }
        public string EnrichmentFields { get; set; }
        public string ThirdPartyProviderCode { get; set; }
        public string ThirdPartyProvider { get; set; }
    }
}
