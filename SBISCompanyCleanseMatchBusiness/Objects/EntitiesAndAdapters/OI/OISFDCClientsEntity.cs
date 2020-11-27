using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    public class OISFDCClientsEntity
    {
        public int OIClientId { get; set; }
        [Display(Name = "Org Id")]
        public string OrgId { get; set; }
        [Display(Name = "License Type")]
        public string LicenseType { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Address1")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address2")]
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Display(Name = "License Key")]
        public string LicenseKey { get; set; }
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "Contact Phone")]
        public string ContactPhone { get; set; }
        public DateTime VerifiedDate { get; set; }
    }
}
