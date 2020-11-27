using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ClientApplicationListEntity
    {
        public int ApplicationId { get; set; }
        public string SubDomain { get; set; }
        public string LicenseStartDate { get; set; }
        public DateTime LicenseEndDate { get; set; }
    }
}
