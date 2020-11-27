using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;
namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class SettingAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<SettingEntity> Adapt(DataTable dt)
        {
            List<SettingEntity> results = new List<SettingEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                SettingEntity cust = new SettingEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public SettingEntity AdaptItem(DataRow rw)
        {
            SettingEntity result = new SettingEntity();
            result.ProcessSettingsID = SafeHelper.GetSafeint(rw["ProcessSettingsID"]);
            result.SettingName = SafeHelper.GetSafestring(rw["SettingName"]);
            result.SettingValue = SafeHelper.GetSafestring(rw["SettingValue"]);
            result.SettingDescription = SafeHelper.GetSafestring(rw["SettingDescription"]);
            result.PossibleValues = SafeHelper.GetSafestring(rw["PossibleValues"]);
            result.PossibleValuesProvided = SafeHelper.GetSafebool(rw["PossibleValuesProvided"]);
            result.MinRangeValue = SafeHelper.GetSafeint(rw["MinRangeValue"]);
            result.MaxRangeValue = SafeHelper.GetSafeint(rw["MaxRangeValue"]);
            result.RangeProvided = SafeHelper.GetSafebool(rw["RangeProvided"]);
            return result;
        }
    }
}
