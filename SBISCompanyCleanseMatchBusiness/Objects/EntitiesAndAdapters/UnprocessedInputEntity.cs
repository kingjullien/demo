namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UnprocessedInputEntity
    {
        public int InputId { get; set; }
        public string SrcRecordId { get; set; }
        public string SourceName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public string DUNSNumber { get; set; }
        public string CEOName { get; set; }
        public string Website { get; set; }
        public string AltCompanyName { get; set; }
        public string AltAddress { get; set; }
        public string AltAddress1 { get; set; }
        public string AltCity { get; set; }
        public string AltState { get; set; }
        public string AltPostalCode { get; set; }
        public string AltCountry { get; set; }
        public string Email { get; set; }
        public string RegistrationNbr { get; set; }
        public string RegistrationType { get; set; }
        public string Tags { get; set; }
        public string inLanguage { get; set; }
        public string LatestErrorCode { get; set; }
    }
}
