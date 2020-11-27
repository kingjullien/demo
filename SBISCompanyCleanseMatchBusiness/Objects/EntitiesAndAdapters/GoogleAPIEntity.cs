using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class GoogleAPIEntity
    {
        public int Id { get; set; }
        [Required]
        public string APIKey { get; set; }
        public bool IsDefault { get; set; }
    }
}
