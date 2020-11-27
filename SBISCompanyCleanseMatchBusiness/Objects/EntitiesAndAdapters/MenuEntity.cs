using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class MenuEntity
    {
        public int MenuId { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
        public bool IsActive { get; set; }
        public string WebPath { get; set; }
        public int ParentMenuId { get; set; }
        public bool IsDisplay { get; set; }
        public List<MenuEntity> lstChild { get; set; }
    }
}
