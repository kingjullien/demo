using System;
using System.ComponentModel.DataAnnotations;


namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterUserEntity
    {
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


        public string _SSOUser;
        [Display(Name = "SSO User")]
        [Required(ErrorMessage = "Please enter SSO User")]
        public string SSOUser
        {
            get { return _SSOUser; }
            set { _SSOUser = value; }
        }
        public int _RoleId;
        public int RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }
        public string _RoleName;
        [Display(Name = "Role Name")]
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        public string Menu { get; set; }
        public bool _IsAdmin;
        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set { _IsAdmin = value; }
        }

    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


    }
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
        public string UserId { get; set; }
        public string ConnectionString { get; set; }

    }
}
