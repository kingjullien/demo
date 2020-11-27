namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class SAMLSSOSettingEntity
    {
        public int SamlId { get; set; }
        public string SPName { get; set; }
        public string SPAssertionConsumerServiceUrl { get; set; }
        public string SPLocalCertificateFile { get; set; }
        public string SPPassword { get; set; }
        public string IDPName { get; set; }
        public string IDPSignAuthnRequest { get; set; }
        public string IDPSingleSignOnServiceUrl { get; set; }
        public string IDPSingleLogoutServiceUrl { get; set; }
        public string IDPPartnerCertificateFile { get; set; }
    }
}
