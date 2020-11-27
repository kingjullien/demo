using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.MasterEntitiesAndAdepters
{
    class MasterHelpDataAdepters
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MasterHelpDataEntity> Adapt(DataTable dt)
        {
            List<MasterHelpDataEntity> results = new List<MasterHelpDataEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MasterHelpDataEntity matchCode = new MasterHelpDataEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public MasterHelpDataEntity AdaptItem(DataRow rw, DataTable dt)
        {
            MasterHelpDataEntity result = new MasterHelpDataEntity();
            result.HelpDataId = rw.Table.Columns.Contains("HelpDataId") ? SafeHelper.GetSafeint(rw["HelpDataId"]) : 0;
            result.Helpdata = rw.Table.Columns.Contains("Helpdata") ? SafeHelper.GetSafestring(rw["Helpdata"]) : string.Empty;
            result.Active = rw.Table.Columns.Contains("Active") ? SafeHelper.GetSafebool(rw["Active"]) : false;

            //Section Master
            result.SectionMasterId = rw.Table.Columns.Contains("SectionMasterId") ? SafeHelper.GetSafeint(rw["SectionMasterId"]) : 0;
            result.PageName = rw.Table.Columns.Contains("PageName") ? SafeHelper.GetSafestring(rw["PageName"]) : string.Empty;
            result.SectionName = rw.Table.Columns.Contains("SectionName") ? SafeHelper.GetSafestring(rw["SectionName"]) : string.Empty;

            return result;
        }
    }
}
