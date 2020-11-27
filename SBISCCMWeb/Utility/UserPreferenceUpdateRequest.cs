using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCCMWeb.Utility
{
    public class PreferenceUpdateSpecification
    {
        public string ApplicationAreaName { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceValueText { get; set; }
        public string PreferenceStatusText { get; set; }
        public string DefaultPreference { get; set; }
    }

    public class UpdatePreferenceRequestDetail
    {
        public PreferenceDetail PreferenceDetail { get; set; }
        public PreferenceUpdateSpecification PreferenceUpdateSpecification { get; set; }
    }

    public class UpdatePreferenceRequest
    {
        public string xmlns { get; set; }
        public TransactionDetail TransactionDetail { get; set; }
        public UpdatePreferenceRequestDetail UpdatePreferenceRequestDetail { get; set; }
    }

    public class UserPreferenceUpdateRequest
    {
        public UpdatePreferenceRequest UpdatePreferenceRequest { get; set; }
    }
}