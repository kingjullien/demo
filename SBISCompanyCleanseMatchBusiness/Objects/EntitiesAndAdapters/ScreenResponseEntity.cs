using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class ScreenResponseEntity
    {
        public string AlertType { get; set; }
        public string DPID { get; set; }
        public string List { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressCountry { get; set; }
        public string AddressCountryCode { get; set; }
        public string Notes { get; set; }
        public string FRC { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FRServe { get; set; }
        public string OptionalId { get; set; }
        public string LastScreenedAlertType { get; set; }

    }
}
