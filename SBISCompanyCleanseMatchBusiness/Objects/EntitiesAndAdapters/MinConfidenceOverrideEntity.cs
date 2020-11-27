using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MinConfidenceOverrideEntity
    {
        public int Id { get; set; }

        public int MinConfidenceCode { get; set; }
        [Required, Range(0, 50)]
        public int MaxCandidateQty { get; set; }
        [Required]
        public string Tags { get; set; }
        public bool IsActive { get; set; }
        public int userId { get; set; }
    }
}
