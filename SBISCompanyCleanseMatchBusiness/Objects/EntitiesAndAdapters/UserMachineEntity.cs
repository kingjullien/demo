using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class UserMachineEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string MachineDetails { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastLoggedinDateTime { get; set; }
        public string BrowserAgent { get; set; }
    }
}
