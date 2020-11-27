using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class DandBFeatureViewModel
    {
        public bool PAUSE_CLEANSE_MATCH_ETL { get; set; }
        public bool PAUSE_ENRICHMENT_ETL { get; set; }
        public string DATA_IMPORT_DUPLICATE_RESOLUTION { get; set; }
        public string DATA_IMPORT_ERROR_RESOLUTION { get; set; }
        public bool TRANSFER_DUNS_AUTO_ENRICH { get; set; }
        public string TRANSFER_DUNS_AUTO_ENRICH_TAG { get; set; }
        public string ENRICHMENT_STALE_NBR_DAYS { get; set; }
        public bool EnrichmentAlwaysRefresh { get; set; }
    }
}