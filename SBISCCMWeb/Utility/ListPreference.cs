using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class PreferenceCreatedDate
    {
        public string _param { get; set; }
    }
    public class PreferenceDetail
    {
        public string PreferenceType { get; set; }
        public string ApplicationAreaName { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceDescription { get; set; }
        public string PreferenceValueText { get; set; }
        public string PreferenceStatusText { get; set; }
        public PreferenceCreatedDate PreferenceCreatedDate { get; set; }
        public bool? DefaultPreference { get; set; }
        public int? DisplaySequence { get; set; }
        public FormerPreferenceDetail FormerPreferenceDetail { get; set; }
    }

    public class ListPreferenceResponseDetail
    {
        public int CandidateMatchedQuantity { get; set; }
        public int CandidateReturnedQuantity { get; set; }
        public List<PreferenceDetail> PreferenceDetail { get; set; }
    }

    public class ListPreferenceResponse
    {
        public string ServiceVersionNumber { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public ListPreferenceResponseDetail ListPreferenceResponseDetail { get; set; }
    }

    public class ListPreference
    {
        public ListPreferenceResponse ListPreferenceResponse { get; set; }
    }
}