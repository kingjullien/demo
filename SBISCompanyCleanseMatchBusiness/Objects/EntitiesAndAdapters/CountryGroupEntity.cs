using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CountryGroupEntity : ViewModelBase, IDataErrorInfo
    {
        public int GroupId { get; set; }
        private string groupName;
        [Required]
        public string GroupName
        {
            get { return groupName; }
            set
            {
                groupName = value;
                RaisePropertyChanged("GroupName");
            }
        }

        private bool IsValidGroupName
        {
            get
            {
                return !string.IsNullOrEmpty(GroupName.Trim());
            }
        }
        [Required]
        public string ISOAlpha2Codes { get; set; }

        private bool IsValidISOAlpha2Codes
        {
            get
            {
                return !string.IsNullOrEmpty(ISOAlpha2Codes.Trim());
            }
        }
        public bool IsValidSave
        {
            get
            {
                return IsValidGroupName && IsValidISOAlpha2Codes;
            }
        }



        public bool CanDeletable { get; set; }

        #region IDataErrorInfo
        private string error = string.Empty;
        public string Error
        {
            get { return error; }
        }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;

                if (columnName == "GroupName" && !IsValidGroupName)
                {
                    error = "Please enter Group Name.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;
            }
        }
        public List<CountryEntity> lstCountries { get; set; }

        public string AddSelectedCountry { get; set; }

        public string RemoveSelectedCountry { get; set; }

        public string tmpName { get; set; }
        #endregion

    }

}
