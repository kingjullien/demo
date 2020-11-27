using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UserPreferenceEntity
    {
        public int PreferenceID { get; set; }
        public string PreferenceName { get; set; }
        public string PreferenceOldName { get; set; }
        public string PreferenceDescription { get; set; }
        public string PreferenceType { get; set; }
        public string PreferenceValue { get; set; }
        public bool DefaultPreference { get; set; }
        public string ApplicationAreaName { get; set; }
        public string ResultID { get; set; }
        public string SeverityText { get; set; }
        public string ResultText { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime ResponseDateTime { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public bool IsDeleted { get; set; }
        public int CredentialId { get; set; }

    }
}
