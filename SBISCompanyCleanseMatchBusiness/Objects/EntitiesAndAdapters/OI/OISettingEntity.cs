namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OISettingEntity
    {
        public string ORB_API_KEY { get; set; }
        public string ORB_BATCH_SIZE { get; set; }
        public string ORB_BATCH_WAITTIME_SECS { get; set; }
        public string ORB_MAX_PARALLEL_THREADS { get; set; }
        public bool PAUSE_ORB_BATCHMATCH_ETL { get; set; }
        public string ORB_DATA_IMPORT_DUPLICATE_RESOLUTION { get; set; }
        public string ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS { get; set; }

        public bool ORB_ENABLE_CORPORATE_TREE_ENRICHMENT { get; set; }

    }
}
