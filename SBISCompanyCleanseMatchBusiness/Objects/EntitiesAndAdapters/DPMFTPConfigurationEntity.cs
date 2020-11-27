using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class DPMFTPConfigurationEntity
    {
        public int? Id { get; set; }
        [Required]
        public string Host { get; set; }
        public int? Port { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
