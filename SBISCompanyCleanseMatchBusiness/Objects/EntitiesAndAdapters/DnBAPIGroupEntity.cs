using System.Collections.Generic;
using System.ComponentModel;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DnBAPIGroupEntity : ViewModelBase, IDataErrorInfo
    {
        public int APIGroupId { get; set; }
        public string DnbAPIIds { get; set; }

        private string apiGroupName;
        public string APIGroupName
        {
            get { return apiGroupName; }
            set
            {
                apiGroupName = value;
                RaisePropertyChanged("APIGroupName");
            }
        }

        private bool IsValidGroupName
        {
            get
            {
                return !string.IsNullOrEmpty(APIGroupName);
            }
        }

        public bool IsValidSave
        {
            get
            {
                return IsValidGroupName;
            }
        }
        public string Tags { get; set; }
        public int CountryGroupId { get; set; }
        public string CountryGroupName { get; set; }

        public int CredentialId { get; set; }
        public string APIType { get; set; }
        public List<DnbAPIEntity> lstDnbAPIs { get; set; }


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

                if (columnName == "APIGroupName" && !IsValidGroupName)
                {
                    error = "Please enter Group Name.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;
            }
        }
        public List<DnbAPIEntity> lstDnBApiGrp { get; set; }

        public string strAPIIDs { get; set; }
        public string APIIds { get; set; }
        public string RemoveAPIIds { get; set; }
        public string tmpName { get; set; }
        #endregion
    }
}
