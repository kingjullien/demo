using SBISCompanyCleanseMatchBusiness.Objects.Business;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchFacade.Objects
{
    public class UserCommentsFacade : FacadeParent
    {
        UserCommentsBusiness rep;
        public UserCommentsFacade(string connectionString) : base(connectionString) { rep = new UserCommentsBusiness(Connection); }

        public List<UserCommentsEntity> GetAllUserCommentsListPaging()
        {
            return rep.GetAllUserCommentsListPaging();
        }
        public void InsertUserComments(UserCommentsEntity objTags)
        {
            rep.InsertUserComments(objTags);
        }
        public void UpdateUserComments(UserCommentsEntity objTags)
        {
            rep.UpdateUserComments(objTags);
        }
        public List<UserCommentsEntity> GetUserCommentsByType(string CommentType)
        {
            return rep.GetUserCommentsByType(CommentType);
        }
        public string DeleteUserComment(int commentId)
        {
            return rep.DeleteUserComment(commentId);
        }
        public UserCommentsEntity GetUserCommentsById(int commentId)
        {
            return rep.GetUserCommentsById(commentId);
        }

    }
}
