using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    [Serializable]
    public class ThirdPartyAPIForEnrichmentEntity
    {
        public int CredentialId { get; set; }
        public string CredentialName { get; set; }
        public string EnrichmentType { get; set; }
        public int DnBAPIId { get; set; }
        public string DnBAPIName { get; set; }
        public string APIType { get; set; }
        public string APIFamily { get; set; }
    }
}
