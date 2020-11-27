using Microsoft.AspNet.Identity;
using SBISCCMWeb.Models;
using System.Threading.Tasks;

namespace SBISCCMWeb.App_Start
{

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public override async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var result = await base.ResetPasswordAsync(userId, token, newPassword);
            return result;
        }

    }

}