using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UserCommentsEntity
    {
        public int CommentId { get; set; }
        public string CommentType { get; set; }
        [Required]
        public string Comment { get; set; }
    }
}
