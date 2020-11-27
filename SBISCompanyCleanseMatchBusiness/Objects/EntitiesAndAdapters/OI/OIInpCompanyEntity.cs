using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIInpCompanyEntity
    {
        public string ImportRowId { get; set; }
        public int ImportProcessId { get; set; }
        [Required(ErrorMessage = "The SrcRecordId field is required..")]
        public string SrcRecordId { get; set; }
        //[Required(ErrorMessage = "The company name field is required..")]
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "The country name field is required..")]
        public string Country { get; set; }
        public string PhoneNbr { get; set; }
        public string OrbSingleEntryTags { get; set; }
        public string EIN { get; set; }
        public string OrbNum { get; set; }
        public string CEOName { get; set; }
        public string Website { get; set; }
        public string AltCompanyName { get; set; }
        public string AltAddress1 { get; set; }
        public string AltAddress2 { get; set; }
        public string AltCity { get; set; }
        public string AltState { get; set; }
        public string AltPostalCode { get; set; }
        public string AltCountry { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
    public class OIInpCompanyEntityMatchRefresh
    {
        public string ImportRowId { get; set; }
        public int ImportProcessId { get; set; }
        [Required]
        public string OrbNumber { get; set; }
    }

}
