namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DandBEnrichmentDataEntity
    {
        public string APILayer { get; set; }
        public int MappingId { get; set; }
        public string JSONPath { get; set; }
        public string SpecialHandling { get; set; }
        public string MBSColumnName { get; set; }
        public string DataType { get; set; }
        public bool Available { get; set; }
    }
}
