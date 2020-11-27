using System.ComponentModel;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CustomAttributeEntity : ViewModelBase, IDataErrorInfo
    {
        public int AttributeId { get; set; }
        public string TypeDescription { get; set; }
        private string attributeName;
        public string AttributeName
        {
            get { return attributeName; }
            set
            {
                attributeName = value;
                RaisePropertyChanged("AttributeName");
            }
        }
        private string attributeTypeCode;
        public string AttributeDataTypeCode
        {
            get
            {
                return attributeTypeCode;
            }
            set
            {
                attributeTypeCode = value;
                RaisePropertyChanged("AttributeDataTypeCode");
            }
        }
        private bool IsValidAttributeName
        {
            get
            {
                return !string.IsNullOrEmpty(AttributeName);
            }
        }
        private bool IsValidType
        {
            get
            {
                return (!string.IsNullOrEmpty(AttributeDataTypeCode) && AttributeDataTypeCode != "-1");
            }
        }
        public bool IsValidSave
        {
            get
            {
                return IsValidAttributeName && IsValidType;
            }
        }
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
                if (columnName == "AttributeName" && !IsValidAttributeName)
                {
                    error = "Please enter Attribute Name.";
                }
                else if (columnName == "AttributeDataTypeCode" && !IsValidType)
                {
                    error = "Please select Attribute Type.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;
            }
        }
        #endregion
    }
    public class AttributeTypes
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }
}
