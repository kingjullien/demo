using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OverrideAPICredentialsEntity
    {
        public int Id { get; set; }
        [Required]
        public string APIKeyOverride { get; set; }
        [Required]
        public string APISecretOverride { get; set; }
        [Required]
        public int BatchSizeOverride { get; set; }
        [Required]
        public int WaitTimesBetweenBatchOverride { get; set; }
        public int MaxParallelThreadsOverride { get; set; }
        [Required]
        public string APILayerOverride { get; set; }
        public bool UseForCleanseMatchOverride { get; set; }
        public bool UseForEnrichOverride { get; set; }
    }
}
