using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OICompanyInformationEntity
    {
        public int Id { get; set; }
        public string SrcRecordId { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public string OrbNum { get; set; }
        public string EIN { get; set; }
        public string CEOName { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Subdomain { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public string MatchOrbNumber { get; set; }
        public int InputId { get; set; }
        public string Tags { get; set; }
        public string TicketNumber { get; set; }
    }
}
