using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Models
{
    public class CleanseMatchSettingsModel
    {
        //public List<SettingEntity> Settings = new List<SettingEntity>();


        public List<SettingEntity> Settings { get; set; }

        public List<AutoAdditionalAcceptanceCriteriaEntity> AcceptanceCriteria { get; set; }
        public AutoAdditionalAcceptanceCriteriaEntity objAcceptance { get; set; }

        public List<AutoAdditionalAcceptanceCriteriaEntity> AutoAcceptanceCriteria { get; set; }
        public AutoAdditionalAcceptanceCriteriaEntity objAutoSetting { get; set; }

        public CleanseMatchExclusionsEntity oCleanseMatchExclusionsEntity { get; set; }
        public AutoAcceptanceDirectivesEntity oAutoAcceptanceDirectivesEntity { get; set; }

        public string MatchGrade { get; set; }



        public int AUTO_CORRECTION_THRESHOLD { get; set; }
        public int MAX_PARALLEL_THREAD { get; set; }
        public int MATCH_GRADE_NAME_THRESHOLD { get; set; }
        public int MATCH_GRADE_STREET_NO_THRESHOLD { get; set; }
        public int MATCH_GRADE_STREET_NAME_THRESHOLD { get; set; }
        public int MATCH_GRADE_CITY_THRESHOLD { get; set; }
        public int MATCH_GRADE_STATE_THRESHOLD { get; set; }
        public int MATCH_GRADE_TELEPHONE_THRESHOLD { get; set; }
        public int MATCH_GRADE_POBOX_THRESHOLD { get; set; }   //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)
        public int APPLY_MATCH_GRADE_TO_LCM { get; set; }
        public int BATCH_SIZE { get; set; }
        public int WAIT_TIME_BETWEEN_BATCHES_SECS { get; set; }

        public bool boolBusinessName { get; set; }
        public bool boolStreet { get; set; }
        public bool boolStreetName { get; set; }
        public bool boolCity { get; set; }
        public bool boolState { get; set; }
        public bool boolTelephone { get; set; }
        public bool boolPoBox { get; set; }  //changing the SettingName in the ProcessSettings table to MATCH_GRADE_POBOX_THRESHOLD from MATCH_GRADE_ZIPCODE_THRESHOLD(MP-338)

        [Display(Name = "Apply Match Grade to Low Confidence Matches")]
        public bool boolApplyMatchGradeLCM { get; set; }


        //new settings page size override settings(MP-322)
        public int PAGE_SIZE_MATCH_OUTPUT { get; set; }
        public int PAGE_SIZE_ENRICHMENT_OUTPUT { get; set; }
        public int PAGE_SIZE_MONITORING_OUTPUT { get; set; }
        public int PAGE_SIZE_ACTIVE_DATA_QUEUE_OUTPUT { get; set; }


        public int ORB_API_KEY { get; set; }
        public int ORB_BATCH_SIZE { get; set; }
        public int ORB_BATCH_WAITTIME_SECS { get; set; }
        public int ORB_MAX_PARALLEL_THREADS { get; set; }
        public int PAUSE_ORB_BATCHMATCH_ETL { get; set; }
        public int ORB_DATA_IMPORT_DUPLICATE_RESOLUTION { get; set; }
        public int ORB_DATA_IMPORT_DUPLICATE_RESOLUTION_TAGS { get; set; }

        public int ORB_ENABLE_CORPORATE_TREE_ENRICHMENT { get; set; }
        public int DATA_STUB_CLIENT_CODE { get; set; }
        public int USE_DATA_STUB { get; set; }
        public int USE_DATA_STUB_FOR_ENRICHMENT { get; set; }
        public int PAUSE_FILE_IMPORT_PROCESS_ETL { get; set; }
        public bool USE_DATA_STUB_VALUE { get; set; }
        public bool USE_DATA_STUB_FOR_ENRICHMENT_VALUE { get; set; }
        public bool MISSING_DATA_FROM_PROVIDER { get; set; }
        // Tune import process to handle bad data import(MP-681)
        public string DATA_IMPORT_ERROR_RESOLUTION { get; set; }
    }
}