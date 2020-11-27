namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UserSessionFilterEntity //: ViewModelBase, IDataErrorInfo
    {
        public int UserId { get; set; }
        public string SrcRecordId { get; set; }
        public string CompanyName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string OrderByColumn { get; set; }
        public string ErrorCode { get; set; }

        public string FilterText { get; set; }
        public bool FilterExists { get; set; }
        public string Tag { get; set; }
        public int CountryGroupId { get; set; }
        public string ImportProcess { get; set; }
        public string AcceptedBy { get; set; }
        public string LOBTag { get; set; }
        public string ConfidenceCode { get; set; }
        public bool IsExactMatch { get; set; }
        public bool TopMatchCandidate { get; set; }
        public int NumberOfRecordsPerPage { get; set; }
    }
}
