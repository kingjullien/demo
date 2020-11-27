using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MasterUsersAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterUsersEntity> Adapt(DataTable dt)
        {
            List<MasterUsersEntity> results = new List<MasterUsersEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterUsersEntity matchCode = new MasterUsersEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public MasterUsersEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterUsersEntity result = new MasterUsersEntity();
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

            return result;
        }


    }
}
