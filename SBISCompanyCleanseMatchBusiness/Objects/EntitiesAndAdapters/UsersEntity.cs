using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    [Serializable]
    public class UsersEntity : ViewModelBase, IDataErrorInfo
    {
        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                RaisePropertyChanged("UserName");
            }
        }

        //public string _loginId;
        //public string LoginId
        //{
        //    get
        //    {
        //        return _loginId;
        //    }
        //    set
        //    {
        //        _loginId = value;
        //        RaisePropertyChanged("LoginId");
        //    }
        //}

        public int UserId { get; set; }

        private string _UserStatusCode;
        public string UserStatusCode
        {
            get
            {
                return _UserStatusCode;
            }
            set
            {
                _UserStatusCode = value;
                RaisePropertyChanged("UserStatusCode");
            }
        }

        private string _UserTypeCode;
        public string UserTypeCode
        {
            get
            {
                return _UserTypeCode;
            }
            set
            {
                _UserTypeCode = value;
                RaisePropertyChanged("UserTypeCode");
            }
        }
        public bool _IsApprover;
        public bool IsApprover
        {
            get
            {
                return _IsApprover;
            }
            set
            {
                _IsApprover = value;
                RaisePropertyChanged("IsApprover");
            }
        }
        public string Imagepath { get; set; }
        public bool Enable2StepUpdate { get; set; }
        public string StatusDescription { get; set; }
        public string TypeDescription { get; set; }
        public string UserType { get; set; }
        public string ClientName { get; set; }
        public string UserLayoutPreference { get; set; }
        public string ClientGUID { get; set; }

        public DateTime SessionStartTime { get; set; }
        public DateTime LastActivityTime { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        private string _EmailAddress { get; set; }
        public string EmailAddress
        {
            get
            {
                return _EmailAddress;
            }
            set
            {
                _EmailAddress = value;
                RaisePropertyChanged("EmailAddress");
            }
        }
        private string _IpAddress { get; set; }
        public string IpAddress
        {
            get
            {
                return _IpAddress;
            }
            set
            {
                _IpAddress = value;
                RaisePropertyChanged("IpAddress");
            }
        }
        public DateTime PasswordResetDate { get; set; }
        public string UserFullName { get; set; }

        public string tmpName { get; set; }

        [Required(ErrorMessage = "Please enter security answer")]
        [Display(Name = "Security Answer")]
        public string SecurityAnswer { get; set; }

        [Required(ErrorMessage = "Please select security question")]
        public int SecurityQuestionId { get; set; }

        public string SecurityQuestion { get; set; }

        private bool IsValidUserName
        {
            get
            {
                return !string.IsNullOrEmpty(UserName);
            }
        }

        private bool IsValidLoginId
        {
            get
            {
                //return !string.IsNullOrEmpty(LoginId);
                return true;
            }
        }

        private bool IsValidStatus
        {
            get
            {
                return (!string.IsNullOrEmpty(UserStatusCode) && UserStatusCode != "-1");
            }
        }

        private bool IsValidType
        {
            get
            {
                return (!string.IsNullOrEmpty(UserTypeCode) && UserTypeCode != "-1");
            }
        }
        private bool IsValidEmailAddress
        {
            get
            {
                return !string.IsNullOrEmpty(EmailAddress);
            }
        }
        private bool IsValidEmail
        {
            get
            {
                return Regex.IsMatch(EmailAddress, @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            }

        }

        public bool IsValidSave
        {
            get
            {
                return IsValidUserName && IsValidLoginId && IsValidStatus && IsValidType && IsValidEmailAddress && IsValidEmail;
            }
        }
        public string ConnectionString { get; set; }
        public DateTime? EULAAcceptedDateTime { get; set; }
        public string EULAAcceptedIPAddress { get; set; }
        public string SSOUser { get; set; }
        //public int SecurityQuestionId { get; set; }
        //public string SecurityAnswer { get; set; }
        //public string SecurityQuestion { get; set; }

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
                if (columnName == "UserName" && !IsValidUserName)
                {
                    error = "Please enter User Name.";
                }
                else if (columnName == "EmailAddress" && !IsValidEmailAddress)
                {
                    error = "Please enter email address.";
                }
                else if (columnName == "EmailAddress" && !IsValidEmail)
                {
                    error = "Please enter valid email address.";
                }
                else if (columnName == "LoginId" && !IsValidLoginId)
                {
                    error = "Please enter Login Id.";
                }
                else if (columnName == "UserStatusCode" && !IsValidStatus)
                {
                    error = "Please select user Status.";
                }
                else if (columnName == "UserTypeCode" && !IsValidType)
                {
                    error = "Please select user Type.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;
            }
        }
        public bool IsUserLoginFirstTime { get; set; }
        public string EmailVerificationCode { get; set; }
        public bool RememberMachineDetails { get; set; }
        public int AttemptCount { get; set; }
        #endregion

        public string Tags { get; set; }
        public bool EnableInvestigations { get; set; }
        public bool EnableSearchByDUNS { get; set; }
        public bool EnableCreateAutoAcceptRules { get; set; }
        public bool ChangesByAdminPortal { get; set; }
        public bool EnablePurgeData { get; set; }
        public string LOBTag { get; set; }
        public string UserRole { get; set; }
        public bool EnablePreviewMatchRules { get; set; }
        public bool LicenseAllowEnrichment { get; set; }
        //User gets locked out after account is re-activated (MP-625)
        public bool TagsInclusive { get; set; }
        public bool EnableImportData { get; set; }
        public bool EnableExportData { get; set; }
        public bool EnableCompliance { get; set; }
    }

    public class UserStatus
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }

}
