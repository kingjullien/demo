namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ExportJobDomainEntity
    {
        public string ApplicationSubDomain { get; set; }
        public string CompanyDataInProgress { get; set; }
        public string CompanyDataReady { get; set; }
        public string CompanyDataFailed { get; set; }
        public string MonitoringNotificationsInProgress { get; set; }
        public string MonitoringNotificationsReady { get; set; }
        public string MonitoringNotificationsFailed { get; set; }
    }
}
