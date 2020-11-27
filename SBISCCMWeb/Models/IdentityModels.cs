using Microsoft.AspNet.Identity;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class ApplicationUser : UsersEntity, IUser
    {

        public BaseModel objBaseModel;
        public string Id
        {
            get { return UserId.ToString(); }
        }

        public ApplicationUser() { }
        public ApplicationUser(UsersEntity oUser)
        {
            if (oUser != null)
            {
                this.UserId = oUser.UserId;
                this.UserName = oUser.UserFullName;
                this.PasswordHash = oUser.PasswordHash;
                this.SecurityStamp = oUser.SecurityStamp;
                //this.LoginId = oUser.LoginId;
                this.EmailAddress = oUser.EmailAddress;
                this.IsApprover = oUser.IsApprover;
                this.Enable2StepUpdate = oUser.Enable2StepUpdate;
                this.UserStatusCode = oUser.UserStatusCode;
                this.UserTypeCode = oUser.UserTypeCode;
                this.UserFullName = oUser.UserName;
                this.UserType = oUser.UserType;
                this.IsUserLoginFirstTime = oUser.IsUserLoginFirstTime;

            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}