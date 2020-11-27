using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.Repositories;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.Business
{
    public class UserCommentsBusiness : BusinessParent
    {
        UserCommentsRepository rep;
        public UserCommentsBusiness(string connectionString) : base(connectionString) { rep = new UserCommentsRepository(Connection); }

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
