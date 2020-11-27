using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class SystemSettingsModel
    {
        public List<SettingEntity> Settings { get; set; }
        public int RETAIN_GOOD_MATCH_ARCHIVE { get; set; }
        public int RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE { get; set; }
        public int RETAIN_AUDIT_ARCHIVE { get; set; }
        public int RETAIN_STEWARDSHIP_AUDIT_ARCHIVE { get; set; }
        public int RETAIN_SRC_COMPANY_INFO { get; set; }
        public int RETAIN_OUTPUT_DATA { get; set; }
        public int ENCRYPT_ALL_DATA { get; set; }
        public int AUDIT_RETENTION_PERIOD_DAYS { get; set; }
        public int CUSTOM_SETTINGS { get; set; }
        public int PRE_SET_VALUE { get; set; }



        public bool isRetainGMArchive { get; set; }
        public bool isRetainLCMArchive { get; set; }
        public bool isRetainAuditArchive { get; set; }
        public bool isRetainStewAuditArchive { get; set; }
        public bool isRetainOutputData { get; set; }
        public bool isRetainSrcCompanyInfo { get; set; }
        public bool isEncryptAll { get; set; }
        public string ArchiveDurationDays { get; set; }
        public bool isCustomSettings { get; set; }
        public int sldPresets { get; set; }


    }
}