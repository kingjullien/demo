namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ImportJobDomainEntity
    {
        public string ApplicationSubDomain { get; set; }
        public string DataImportInProgress { get; set; }
        public string DataImportReady { get; set; }
        public string DataImportFailed { get; set; }
        public string MatchRefreshInProgress { get; set; }
        public string MatchRefreshReady { get; set; }
        public string MatchRefreshFailed { get; set; }
    }
}
