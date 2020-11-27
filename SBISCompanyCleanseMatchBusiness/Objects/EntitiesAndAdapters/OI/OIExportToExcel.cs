namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OIExportToExcel
    {
        public int CountryGroupId { get; set; }
        public string SrcRecordId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string ImportProcess { get; set; }
        public string Tag { get; set; }
        public bool ExportWithCandidates { get; set; }
        public bool ExportWithoutCandidates { get; set; }
        public int UserId { get; set; }
        public bool GetCountOnly { get; set; }

    }
}
