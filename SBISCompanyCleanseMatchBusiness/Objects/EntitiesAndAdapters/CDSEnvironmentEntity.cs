using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class CDSEnvironmentEntity
    {
        [Required]
        public string OrganizationUrl { get; set; }
        [Required]
        public Guid TenantId { get; set; }
        [Required]
        public string EnvironmentName { get; set; }
        public string TxtTenantId { get; set; }
    }
}
