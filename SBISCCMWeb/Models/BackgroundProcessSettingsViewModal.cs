using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BackgroundProcessSettingsViewModal
    {
        public string MAX_PARALLEL_THREAD { get; set; }
        public string BATCH_SIZE { get; set; }
        public string WAIT_TIME_BETWEEN_BATCHES_SECS { get; set; }
    }
}