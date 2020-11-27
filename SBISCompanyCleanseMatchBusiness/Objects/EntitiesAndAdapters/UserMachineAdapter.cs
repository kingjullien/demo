using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class UserMachineAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<UserMachineEntity> Adapt(DataTable dt)
        {
            List<UserMachineEntity> results = new List<UserMachineEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                UserMachineEntity cust = new UserMachineEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public UserMachineEntity AdaptItem(DataRow rw)
        {
            UserMachineEntity result = new UserMachineEntity();
            if (rw.Table.Columns["Id"] != null)
            {
                result.Id = SafeHelper.GetSafeint(rw["Id"]);
            }

            if (rw.Table.Columns["UserId"] != null)
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (rw.Table.Columns["MachineDetails"] != null)
            {
                result.MachineDetails = SafeHelper.GetSafestring(rw["MachineDetails"]);
            }

            if (rw.Table.Columns["CreatedDateTime"] != null)
            {
                result.CreatedDateTime = SafeHelper.GetSafeDateTime(rw["CreatedDateTime"]);
            }

            if (rw.Table.Columns["LastLoggedinDateTime"] != null)
            {
                result.LastLoggedinDateTime = SafeHelper.GetSafeDateTime(rw["LastLoggedinDateTime"]);
            }

            if (rw.Table.Columns["BrowserAgent"] != null)
            {
                result.BrowserAgent = SafeHelper.GetSafestring(rw["BrowserAgent"]);
            }

            return result;
        }
    }
}
