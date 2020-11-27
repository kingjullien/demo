using System;
using System.ComponentModel.DataAnnotations;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    public class MasterRoleEntity
    {
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Please enter Role Name")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsView { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
