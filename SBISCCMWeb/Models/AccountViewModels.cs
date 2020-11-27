using SBISCCMWeb.LanguageResources;
using System.ComponentModel.DataAnnotations;

namespace SBISCCMWeb.Models
{
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }

    public class LoginViewModel
    {
        //[Required]
        ////[EmailAddress]
        //[Display(Name = "LoginId")]
        //public string LoginId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        public bool Predefineuser { get; set; }
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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
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
    }
    public class EmailVerification
    {
        public int UserId { get; set; }
        [Required]
        [Display(Name = "An email has been send to your email address. Please check and enter the 4 digit verification code in the text box and click on Verify Code:")]
        public string VerificationCode { get; set; }
        [Display(Name = "Remember me on this machine (Do not select this option if you are logging in from a public machine)")]
        public bool RememberMachine { get; set; }
    }
}