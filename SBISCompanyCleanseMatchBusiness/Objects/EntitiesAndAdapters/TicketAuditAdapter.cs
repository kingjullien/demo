using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class TicketAuditAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<TicketAuditEntity> Adapt(DataTable dt)
        {
            List<TicketAuditEntity> results = new List<TicketAuditEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                TicketAuditEntity matchCode = new TicketAuditEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public TicketAuditEntity AdaptItem(DataRow rw, DataTable dt)
        {
            TicketAuditEntity result = new TicketAuditEntity();
            result.Id = SafeHelper.GetSafeint(rw["Id"]);
            if (dt.Columns.Contains("Status"))
            {
                result.Status = SafeHelper.GetSafeint(rw["Status"]);
            }

            if (dt.Columns.Contains("StatusValue"))
            {
                result.StatusValue = SafeHelper.GetSafestring(rw["StatusValue"]);
            }

            if (dt.Columns.Contains("AuditDateTime"))
            {
                result.AuditDateTime = SafeHelper.GetSafeDateTime(rw["AuditDateTime"]);
            }

            if (dt.Columns.Contains("AssignedTo"))
            {
                result.AssignedTo = SafeHelper.GetSafeint(rw["AssignedTo"]);
            }

            if (dt.Columns.Contains("ChangedByUser"))
            {
                result.ChangedByUser = SafeHelper.GetSafestring(rw["ChangedByUser"]);
            }

            if (dt.Columns.Contains("Priority"))
            {
                result.Priority = SafeHelper.GetSafeint(rw["Priority"]);
            }

            if (dt.Columns.Contains("PriorityValue"))
            {
                result.PriorityValue = SafeHelper.GetSafestring(rw["PriorityValue"]);
            }

            if (dt.Columns.Contains("Notes"))
            {
                result.Notes = SafeHelper.GetSafestring(rw["Notes"]);
            }

            if (dt.Columns.Contains("AssignedToName"))
            {
                result.AssignedToName = SafeHelper.GetSafestring(rw["AssignedToName"]);
            }

            return result;
        }
    }
}
