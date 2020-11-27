using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class TicketAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<TicketEntity> Adapt(DataTable dt)
        {
            List<TicketEntity> results = new List<TicketEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                TicketEntity matchCode = new TicketEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }
        public TicketEntity AdaptItem(DataRow rw, DataTable dt)
        {
            TicketEntity result = new TicketEntity();
            result.Id = SafeHelper.GetSafeint(rw["Id"]);
            if (dt.Columns.Contains("ClientInformation"))
            {
                result.ClientInformation = SafeHelper.GetSafestring(rw["ClientInformation"]);
            }

            if (dt.Columns.Contains("ApplicationUser"))
            {
                result.ApplicationUser = SafeHelper.GetSafestring(rw["ApplicationUser"]);
            }

            if (dt.Columns.Contains("EnteredBy"))
            {
                result.EnteredBy = SafeHelper.GetSafestring(rw["EnteredBy"]);
            }

            if (dt.Columns.Contains("PrimaryEmailAddress"))
            {
                result.PrimaryEmailAddress = SafeHelper.GetSafestring(rw["PrimaryEmailAddress"]);
            }

            if (dt.Columns.Contains("PrimaryContactNumber"))
            {
                result.PrimaryContactNumber = SafeHelper.GetSafestring(rw["PrimaryContactNumber"]);
            }

            if (dt.Columns.Contains("IssueDescription"))
            {
                result.IssueDescription = SafeHelper.GetSafestring(rw["IssueDescription"]);
            }

            if (dt.Columns.Contains("Files"))
            {
                result.Files = SafeHelper.GetSafestring(rw["Files"]);
            }

            if (dt.Columns.Contains("AddDateTime"))
            {
                result.AddDateTime = SafeHelper.GetSafeDateTime(rw["AddDateTime"]);
            }

            if (dt.Columns.Contains("AssignedTo"))
            {
                result.AssignedTo = SafeHelper.GetSafeint(rw["AssignedTo"]);
            }

            if (dt.Columns.Contains("Priority"))
            {
                result.Priority = SafeHelper.GetSafeint(rw["Priority"]);
            }

            if (dt.Columns.Contains("CurrentStatus"))
            {
                result.CurrentStatus = SafeHelper.GetSafeint(rw["CurrentStatus"]);
            }

            if (dt.Columns.Contains("ResolutionDescription"))
            {
                result.ResolutionDescription = SafeHelper.GetSafestring(rw["ResolutionDescription"]);
            }

            if (dt.Columns.Contains("DateTimeCompleted"))
            {
                result.DateTimeCompleted = SafeHelper.GetSafeDateTime(rw["DateTimeCompleted"]);
            }

            if (dt.Columns.Contains("TicketSource"))
            {
                result.TicketSource = SafeHelper.GetSafeint(rw["TicketSource"]);
            }

            if (dt.Columns.Contains("TicketType"))
            {
                result.TicketType = SafeHelper.GetSafeint(rw["TicketType"]);
            }

            if (dt.Columns.Contains("PriorityValue"))
            {
                result.PriorityValue = SafeHelper.GetSafestring(rw["PriorityValue"]);
            }

            if (dt.Columns.Contains("CurrentStatusValue"))
            {
                result.CurrentStatusValue = SafeHelper.GetSafestring(rw["CurrentStatusValue"]);
            }

            if (dt.Columns.Contains("TicketSourceValue"))
            {
                result.TicketSourceValue = SafeHelper.GetSafestring(rw["TicketSourceValue"]);
            }

            if (dt.Columns.Contains("TicketTypeValue"))
            {
                result.TicketTypeValue = SafeHelper.GetSafestring(rw["TicketTypeValue"]);
            }

            if (dt.Columns.Contains("Title"))
            {
                result.Title = SafeHelper.GetSafestring(rw["Title"]);
            }

            if (dt.Columns.Contains("AssignedToName"))
            {
                result.AssignedToName = SafeHelper.GetSafestring(rw["AssignedToName"]);
            }

            return result;
        }

        public List<TicketStatus> AdaptTicketUtil(DataTable dt)
        {
            List<TicketStatus> results = new List<TicketStatus>();
            foreach (DataRow item in dt.Rows)
            {
                TicketStatus matchCode = new TicketStatus()
                {
                    Code = SafeHelper.GetSafestring(item["Code"]),
                    Value = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(SafeHelper.GetSafestring(item["Value"]).ToLower()),
                    TypeCode = SafeHelper.GetSafestring(item["TypeCode"]),
                };
                results.Add(matchCode);
            }
            return results;
        }

    }
}
