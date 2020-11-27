using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    class MasterUserAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterUserEntity> Adapt(DataTable dt)
        {
            List<MasterUserEntity> results = new List<MasterUserEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterUserEntity matchCode = new MasterUserEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public MasterUserEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterUserEntity result = new MasterUserEntity();
            result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            result.UserName = SafeHelper.GetSafestring(rw["UserName"]);
            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("EmailAddress"))
            {
                result.UserName = SafeHelper.GetSafestring(rw["EmailAddress"]);
            }

            if (dt.Columns.Contains("UserName"))
            {
                result.clientUserName = SafeHelper.GetSafestring(rw["UserName"]);
            }

            if (dt.Columns.Contains("PasswordHash"))
            {
                result.PasswordHash = SafeHelper.GetSafestring(rw["PasswordHash"]);
            }

            if (dt.Columns.Contains("SecurityStamp"))
            {
                result.SecurityStamp = SafeHelper.GetSafestring(rw["SecurityStamp"]);
            }

            if (dt.Columns.Contains("PasswordResetToken"))
            {
                result.PasswordResetToken = SafeHelper.GetSafestring(rw["PasswordResetToken"]);
            }

            if (dt.Columns.Contains("PasswordTokenCreateDate"))
            {
                result.PasswordTokenCreateDate = SafeHelper.GetSafeDateTime(rw["PasswordTokenCreateDate"]);
            }

            if (dt.Columns.Contains("DateAdded"))
            {
                result.DateAdded = SafeHelper.GetSafeDateTime(rw["DateAdded"]);
            }

            if (dt.Columns.Contains("SSOUser"))
            {
                result.SSOUser = SafeHelper.GetSafestring(rw["SSOUser"]);
            }

            if (dt.Columns.Contains("Menu"))
            {
                result.Menu = SafeHelper.GetSafestring(rw["Menu"]);
            }

            if (dt.Columns.Contains("RoleName"))
            {
                result.RoleName = SafeHelper.GetSafestring(rw["RoleName"]);
            }

            if (dt.Columns.Contains("IsAdmin"))
            {
                result.IsAdmin = SafeHelper.GetSafebool(rw["IsAdmin"]);
            }

            return result;
        }
    }
}
