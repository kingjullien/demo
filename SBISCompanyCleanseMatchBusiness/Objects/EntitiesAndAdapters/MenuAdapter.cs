using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{

    class MenuAdapters
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MenuEntity> Adapt(DataTable dt)
        {
            List<MenuEntity> results = new List<MenuEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MenuEntity menuEntity = new MenuEntity();
                menuEntity = AdaptItem(rw);
                results.Add(menuEntity);
            }
            return results;
        }

        public MenuEntity AdaptItem(DataRow rw)
        {
            MenuEntity result = new MenuEntity();
            result.MenuId = SafeHelper.GetSafeint(rw["id"]);
            result.Name = SafeHelper.GetSafestring(rw["Name"]);
            result.WebPath = SafeHelper.GetSafestring(rw["WebPath"]);
            result.IsDisplay = SafeHelper.GetSafebool(rw["isDefault"]);
            return result;
        }
    }
}
