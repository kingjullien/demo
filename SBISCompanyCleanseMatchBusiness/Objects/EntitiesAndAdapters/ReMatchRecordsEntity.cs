namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{

    public class ReMatchRecordsEntity
    {
        //Implement re-match queue (MP-14)
        public string SrcRecordId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ImportProcess { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public int CountryGroupId { get; set; }
        public string Tag { get; set; }
        public bool CityExactMatch { get; set; }
        public bool StateExactMatch { get; set; }
        public bool GetCountsOnly { get; set; }
        public int UserId { get; set; }
    }
}
