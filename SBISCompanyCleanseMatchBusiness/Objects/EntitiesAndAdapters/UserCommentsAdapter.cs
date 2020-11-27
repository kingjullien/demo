using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class UserCommentsAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<UserCommentsEntity> Adapt(DataTable dt)
        {
            List<UserCommentsEntity> results = new List<UserCommentsEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UserCommentsEntity cust = new UserCommentsEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public UserCommentsEntity AdaptItem(DataRow rw)
        {
            UserCommentsEntity result = new UserCommentsEntity();
            if (rw.Table.Columns["CommentId"] != null)
            {
                result.CommentId = SafeHelper.GetSafeint(rw["CommentId"]);
            }

            if (rw.Table.Columns["CommentType"] != null)
            {
                result.CommentType = SafeHelper.GetSafestring(rw["CommentType"]);
            }

            if (rw.Table.Columns["Comment"] != null)
            {
                result.Comment = SafeHelper.GetSafestring(rw["Comment"]);
            }

            return result;
        }
    }
}
