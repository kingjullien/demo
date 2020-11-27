using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_Competitors
    {
        public string DnBDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string OrganizationName { get; set; }
        public string SalesRevenueAmount_UnitOfSize { get; set; }
        public string SalesRevenueAmount_CurrencyISOAlpha3Code { get; set; }
        public string SalesRevenueAmount { get; set; }
        public string IssuedShareCapitalAmount_CurrencyISOAlpha3Code { get; set; }
        public string IssuedShareCapitalAmount_UnitOfSize { get; set; }
        public string IssuedShareCapitalAmount { get; set; }
        public string IndividualEmployeeQuantity { get; set; }
    }
}