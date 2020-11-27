namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    // DB Changes (MP-716)
    public class OIUserSessionFilterEntity
    {
        public int UserId { get; set; }
        public string SrcRecordId { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string OrderByColumn { get; set; }

        public string FilterText { get; set; }
        public bool FilterExists { get; set; }
        public string Tag { get; set; }
        public int CountryGroupId { get; set; }
        public string ImportProcess { get; set; }
    }
}
