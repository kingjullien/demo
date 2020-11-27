using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ComplainceScreeningEntity
    {
        public int Id { get; set; }
        public string DUNSNo { get; set; }
        public string Clientsubdomain { get; set; }
        public string FileName { get; set; }
        public DateTime RequestedDate { get; set; }
        public DateTime ProcessStartDate { get; set; }
        public DateTime ProcessEndDate { get; set; }
        public bool IsProcessComplete { get; set; }
    }
}
