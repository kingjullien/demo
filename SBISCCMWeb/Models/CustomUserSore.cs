using Microsoft.AspNet.Identity;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class CustomUserSore<T> : CustomUserSoreParent, IUserLoginStore<T>, IUserStore<T>, IUserPasswordStore<T>, IUserSecurityStampStore<T>, IUserRoleStore<T, string>, IUserEmailStore<T> where T : ApplicationUser
    {
        public System.Web.HttpContextBase contextBase = null;
        void IDisposable.Dispose()
        {
        }
        public CustomUserSore(string connectionString) : base(connectionString) { }
        Task IUserStore<T, string>.CreateAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                //user.UserId = Guid.NewGuid();
                //user.AddUser();
            });
        }
        Task IUserStore<T, string>.DeleteAsync(T user)
        {
            throw new NotImplementedException();
        }
        Task<T> IUserStore<T, string>.FindByIdAsync(string userId)
        {

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException("userId");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = GetClientConnectionString();
            }
            SettingFacade fac = new SettingFacade(_connectionString);
            int parsedUserId;
            if (!int.TryParse(userId, out parsedUserId))
                throw new ArgumentOutOfRangeException("userId", string.Format("'{0}' is not a valid GUID.", new { userId }));

            return Task.Factory.StartNew(() =>
            {
                UsersEntity oUser = fac.GetUserDetailsById(Convert.ToInt32(userId));
                if (oUser == null)
                    return null;
                else
                    return (T)(new ApplicationUser(oUser));
            });
        }


        Task<T> IUserStore<T, string>.FindByNameAsync(string EmailAddress)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = GetClientConnectionString();
            }
            string GetConnctionstring = Convert.ToString(_connectionString);
            CompanyFacade fac = new CompanyFacade(GetConnctionstring, Helper.UserName);
            if (string.IsNullOrWhiteSpace(EmailAddress))
                throw new ArgumentNullException("userName");

            return Task.Factory.StartNew(() =>
            {
                UsersEntity oUser = fac.StewUserLogIn(EmailAddress, null, true);
                if (oUser == null)
                    return null;
                else
                    return (T)(new ApplicationUser(oUser));
            });
        }

        Task IUserStore<T, string>.UpdateAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = GetClientConnectionString();
                }
                SettingFacade Facade = new SettingFacade(_connectionString);
                user.UserName = user.UserFullName;
                Facade.ResetUserPassword(user.EmailAddress, user.PasswordHash, user.SecurityStamp);
            });
        }

        Task<string> IUserPasswordStore<T, string>.GetPasswordHashAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.PasswordHash);
        }



        Task<bool> IUserPasswordStore<T, string>.HasPasswordAsync(T user)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        Task IUserPasswordStore<T, string>.SetPasswordHashAsync(T user, string passwordHash)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        Task<string> IUserSecurityStampStore<T, string>.GetSecurityStampAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.FromResult(user.SecurityStamp);
        }

        Task IUserSecurityStampStore<T, string>.SetSecurityStampAsync(T user, string stamp)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        Task IUserRoleStore<T, string>.AddToRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        Task<IList<string>> IUserRoleStore<T, string>.GetRolesAsync(T user)
        {


            if (user == null)
                throw new ArgumentNullException("user");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = GetClientConnectionString();
            }
            CompanyFacade fac = new CompanyFacade(_connectionString, Helper.UserName);
            UsersEntity oUser = fac.StewUserLogIn(user.EmailAddress, null, true);
            return Task.Factory.StartNew(() =>
            {
                IList<string> newList = null;
                List<string> role = new List<string>();
                role.Add("Admin");
                if (!string.IsNullOrEmpty(oUser.UserType))
                {
                    role.Add(oUser.UserType);
                }
                newList = role;
                return newList;
            });
        }

        Task<bool> IUserRoleStore<T, string>.IsInRoleAsync(T user, string roleName)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            return Task.Factory.StartNew(() =>
            {
                if (string.Compare(roleName, "Admin", true) == 0)
                    return true;
                return false;
            });
        }

        Task IUserRoleStore<T, string>.RemoveFromRoleAsync(T user, string roleName)
        {
            throw new NotImplementedException();
        }

        Task<T> IUserEmailStore<T, string>.FindByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = GetClientConnectionString();
            }
            CompanyFacade fac = new CompanyFacade(_connectionString, Helper.UserName);
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("email");

            return Task.Factory.StartNew(() =>
            {
                UsersEntity oUser = fac.GetUserByEmail(email);
                if (oUser == null)
                    return null;
                else
                    return (T)(new ApplicationUser(oUser));
            });
        }

        Task<string> IUserEmailStore<T, string>.GetEmailAsync(T user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return Task.FromResult(user.EmailAddress);
        }

        Task<bool> IUserEmailStore<T, string>.GetEmailConfirmedAsync(T user)
        {
            return Task.FromResult(true);
        }

        Task IUserEmailStore<T, string>.SetEmailAsync(T user, string email)
        {
            throw new NotImplementedException();
        }

        Task IUserEmailStore<T, string>.SetEmailConfirmedAsync(T user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        Task IUserLoginStore<T, string>.AddLoginAsync(T user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        Task<T> IUserLoginStore<T, string>.FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }


        Task<IList<UserLoginInfo>> IUserLoginStore<T, string>.GetLoginsAsync(T user)
        {
            throw new NotImplementedException();
        }

        Task IUserLoginStore<T, string>.RemoveLoginAsync(T user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public string GetClientConnectionString()
        {
            MasterClientApplicationFacade Mfac = new MasterClientApplicationFacade(Helper.GetMasterConnctionstring());


            if (Helper.ApplicationData == null)
            {
                Helper.ApplicationData = Mfac.GetClientApplicationData(HttpContext.Current.Request.Url.Authority);
            }
            if (Helper.ApplicationData != null)
            {
                _connectionString = StringCipher.Decrypt(Helper.ApplicationData.ApplicationDBConnectionStringHash, General.passPhrase);
            }
            return _connectionString;
        }
    }
}