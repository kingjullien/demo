using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class SettingEntity
    {
        public SettingEntity()
        {
            Settings = new List<SettingEntity>();
        }
        public int ProcessSettingsID { get; set; }
        public string SettingName { get; set; }
        public string SettingValue { get; set; }
        public string SettingDescription { get; set; }
        public string PossibleValues { get; set; }
        public bool PossibleValuesProvided { get; set; }
        public int MinRangeValue { get; set; }
        public int MaxRangeValue { get; set; }
        public bool RangeProvided { get; set; }

        public List<SettingEntity> Settings { get; set; }

    }
}
