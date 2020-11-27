using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UserPreferenceAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        /// <summary>
        /// Product data
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<UserPreferenceEntity> Adapt(DataTable dt)
        {
            List<UserPreferenceEntity> results = new List<UserPreferenceEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UserPreferenceEntity PreferenceDataEntity = new UserPreferenceEntity();
                PreferenceDataEntity = AdaptItem(rw, dt);
                results.Add(PreferenceDataEntity);
            }
            return results;
        }

        public UserPreferenceEntity AdaptItem(DataRow rw, DataTable dt)
        {
            UserPreferenceEntity result = new UserPreferenceEntity();
            result.PreferenceID = SafeHelper.GetSafeint(rw["PreferenceID"]);

            if (dt.Columns.Contains("PreferenceName"))
            {
                result.PreferenceName = SafeHelper.GetSafestring(rw["PreferenceName"]);
            }

            if (dt.Columns.Contains("PreferenceName"))
            {
                result.PreferenceOldName = SafeHelper.GetSafestring(rw["PreferenceName"]);
            }

            if (dt.Columns.Contains("PreferenceDescription"))
            {
                result.PreferenceDescription = SafeHelper.GetSafestring(rw["PreferenceDescription"]);
            }

            if (dt.Columns.Contains("PreferenceType"))
            {
                result.PreferenceType = SafeHelper.GetSafestring(rw["PreferenceType"]);
            }

            if (dt.Columns.Contains("PreferenceValue"))
            {
                result.PreferenceValue = SafeHelper.GetSafestring(rw["PreferenceValue"]);
            }

            if (dt.Columns.Contains("DefaultPreference"))
            {
                result.DefaultPreference = SafeHelper.GetSafebool(rw["DefaultPreference"]);
            }

            if (dt.Columns.Contains("ApplicationAreaName"))
            {
                result.ApplicationAreaName = SafeHelper.GetSafestring(rw["ApplicationAreaName"]);
            }

            if (dt.Columns.Contains("ResultID"))
            {
                result.ResultID = SafeHelper.GetSafestring(rw["ResultID"]);
            }

            if (dt.Columns.Contains("SeverityText"))
            {
                result.SeverityText = SafeHelper.GetSafestring(rw["SeverityText"]);
            }

            if (dt.Columns.Contains("ResultText"))
            {
                result.ResultText = SafeHelper.GetSafestring(rw["ResultText"]);
            }

            if (dt.Columns.Contains("RequestDateTime"))
            {
                result.RequestDateTime = SafeHelper.GetSafeDateTime(rw["RequestDateTime"]);
            }

            if (dt.Columns.Contains("ResponseDateTime"))
            {
                result.ResponseDateTime = SafeHelper.GetSafeDateTime(rw["ResponseDateTime"]);
            }

            if (dt.Columns.Contains("CreatedBy"))
            {
                result.CreatedBy = SafeHelper.GetSafeint(rw["CreatedBy"]);
            }

            if (dt.Columns.Contains("ModifiedBy"))
            {
                result.ModifiedBy = SafeHelper.GetSafeint(rw["ModifiedBy"]);
            }

            if (dt.Columns.Contains("CreateDateTime"))
            {
                result.CreateDateTime = SafeHelper.GetSafeDateTime(rw["CreateDateTime"]);
            }

            if (dt.Columns.Contains("ModifiedDateTime"))
            {
                result.ModifiedDateTime = SafeHelper.GetSafeDateTime(rw["ModifiedDateTime"]);
            }

            if (dt.Columns.Contains("IsDeleted"))
            {
                result.IsDeleted = SafeHelper.GetSafebool(rw["IsDeleted"]);
            }

            return result;
        }
    }
}
