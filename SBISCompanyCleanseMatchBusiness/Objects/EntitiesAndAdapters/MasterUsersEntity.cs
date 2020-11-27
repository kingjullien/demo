using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;




namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MasterUsersEntity : ViewModelBase, IDataErrorInfo
    {
        //private int _UserId;
        //public int UserId
        //{
        //    get { return _UserId; }
        //    set { _UserId = value; }
        //}

        //private string _UserName;
        //public string UserName
        //{
        //    get { return _UserName; }
        //    set { _UserName = value; }
        //}

        //private string _EmailAddress;
        //public string EmailAddress
        //{
        //    get { return _EmailAddress; }
        //    set { _EmailAddress = value; }
        //}

        //private string _PasswordHash;
        //public string PasswordHash
        //{
        //    get { return _PasswordHash; }
        //    set { _PasswordHash = value; }
        //}

        //private string _SecurityStamp;
        //public string SecurityStamp
        //{
        //    get { return _SecurityStamp; }
        //    set { _SecurityStamp = value; }
        //}
        //private string _PasswordResetToken;
        //public string PasswordResetToken
        //{
        //    get { return _PasswordResetToken; }
        //    set { _PasswordResetToken = value; }
        //}

        //private DateTime _PasswordTokenCreateDate;
        //public DateTime PasswordTokenCreateDate
        //{
        //    get { return _PasswordTokenCreateDate; }
        //    set { _PasswordTokenCreateDate = value; }
        //}
        //private DateTime _DateAdded;
        //public DateTime DateAdded
        //{
        //    get { return _DateAdded; }
        //    set { _DateAdded = value; }
        //}

        private int _userId;
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string _UserName;
        [EmailAddress(ErrorMessage = "Please enter valid Email Address")]
        [Required(ErrorMessage = "Please enter Email Address")]
        [Display(Name = "Email Address")]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public string _clientUserName;

        [Required(ErrorMessage = "Please enter UserName")]
        [Display(Name = "UserName")]
        public string clientUserName
        {
            get { return _clientUserName; }
            set { _clientUserName = value; }
        }

        public string _PasswordHash;
        [Required(ErrorMessage = "Please enter Password")]
        [Display(Name = "Password")]
        public string PasswordHash
        {
            get { return _PasswordHash; }
            set { _PasswordHash = value; }
        }

        public string _SecurityStamp;
        public string SecurityStamp
        {
            get { return _SecurityStamp; }
            set { _SecurityStamp = value; }
        }


        public string _PasswordResetToken;
        public string PasswordResetToken
        {
            get { return _PasswordResetToken; }
            set { _PasswordResetToken = value; }
        }


        public DateTime _PasswordTokenCreateDate;
        public DateTime PasswordTokenCreateDate
        {
            get { return _PasswordTokenCreateDate; }
            set { _PasswordTokenCreateDate = value; }
        }


        public DateTime _dDateAdded;
        public DateTime DateAdded
        {
            get { return _dDateAdded; }
            set { _dDateAdded = value; }
        }



        #region IDataErrorInfo
        private string error = string.Empty;
        public string Error
        {
            get { return error; }
        }
        private bool IsValidUserName
        {
            get
            {
                return !string.IsNullOrEmpty(UserName);
            }
        }

        public object Helper { get; private set; }

        public string this[string columnName]
        {
            get
            {
                error = string.Empty;
                if (columnName == "UserName" && !IsValidUserName)
                {
                    error = "Please enter User Name.";
                }
                RaisePropertyChanged("IsValidSave");
                return error;
            }
        }

        //public int AddUser(string connectionstring)
        //{
        //    DatabaseGateway da = new DatabaseGateway();
        //    MasterUserFacade MUFacade = new MasterUserFacade(connectionstring);
        //    int result = 0;
        //    result = (int)da.ExecuteScalar("mapp.AddUser", _UserName, _clientUserName, _PasswordHash, _SecurityStamp, _PasswordResetToken, _PasswordTokenCreateDate);
        //    if (result > 0)
        //        this._userId = result;
        //    return result;
        //}
        #endregion
    }
}
