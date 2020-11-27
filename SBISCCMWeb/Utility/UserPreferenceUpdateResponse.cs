using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class FormerPreferenceDetail
    {
        public string ApplicationAreaName { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceValueText { get; set; }
        public string PreferenceStatusText { get; set; }
        public bool DefaultPreference { get; set; }
    }
    public class UpdatePreferenceResponseDetail
    {
        public int UpdatedPreferenceCount { get; set; }
        public PreferenceDetail PreferenceDetail { get; set; }
    }

    public class UpdatePreferenceResponse
    {
        public string ServiceVersionNumber { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public TransactionResult TransactionResult { get; set; }
        public UpdatePreferenceResponseDetail UpdatePreferenceResponseDetail { get; set; }
    }

    public class UserPreferenceUpdateResponse
    {
        public UpdatePreferenceResponse UpdatePreferenceResponse { get; set; }
    }
}