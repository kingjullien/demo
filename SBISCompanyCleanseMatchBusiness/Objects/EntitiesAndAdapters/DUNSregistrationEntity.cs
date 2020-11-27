namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DUNSregistrationEntity
    {
        public int MonitoringRegistrationId { get; set; }
        public int MonitoringProfileId { get; set; }
        public int NotificationProfileId { get; set; }
        public string MonitoringProfileName { get; set; }
        public string NotificationProfileName { get; set; }
        public bool TradeUpIndicator { get; set; }
        public bool AutoRenewalIndicator { get; set; }
        public string SubjectCategory { get; set; }
        public string CustomerReferenceText { get; set; }
        public string BillingEndorsementText { get; set; }
        public string Tags { get; set; }
        public int CredentialId { get; set; }
    }
}
