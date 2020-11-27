using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_OrganizationIDNumbers
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string PreferredOrganizationIdentificationNumberIndicator { get; set; }
        public string StartDate { get; set; }
        public string OrganizationIdentificationNumber { get; set; }
        public string RegistrationIssuerName { get; set; }
        public string FilingOrganizationName { get; set; }
        public string PrimaryTownName { get; set; }
        public string PostalCode { get; set; }
        public string TerritoryName { get; set; }
        public string StreetAddressLine { get; set; }
        public string RegistrationIssuerDUNSNumber { get; set; }
        public string RegistrationIssuer { get; set; }
        public string AssignmentMethodText { get; set; }
        public string AssignmentMethodCode { get; set; }

    }
}