using System;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class TicketAuditEntity
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public string StatusValue { get; set; }
        public DateTime AuditDateTime { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedToName { get; set; }
        public string ChangedByUser { get; set; }
        public int Priority { get; set; }
        public string PriorityValue { get; set; }
        public string Notes { get; set; }
        public int TicketId { get; set; }
    }

    public class TicketHistory
    {
        public string ChangedByName { get; set; }
        public List<ItemModification> itemModification { get; set; }
        public DateTime ModificationDate { get; set; }
        public string Note { get; set; }
    }

    public class ItemModification
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
