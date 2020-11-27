using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_KeyFinancialFigures
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string FinancialPeriodDuration { get; set; }
        public string FinancialStatementToDate { get; set; }
        public string SalesRevenueAmount_UnitOfSize0 { get; set; }
        public string SalesRevenueAmount_CurrencyISOAlpha3Code0 { get; set; }
        public string SalesRevenueAmount_Reliability0 { get; set; }
        public string SalesRevenueAmount0 { get; set; }
        public string SalesRevenueAmount_UnitOfSize1 { get; set; }
        public string SalesRevenueAmount_CurrencyISOAlpha3Code1 { get; set; }
        public string SalesRevenueAmount_Reliability1 { get; set; }
        public string SalesRevenueAmount1 { get; set; }
        public string ProfitOrLossGrowthRate { get; set; }
        public string SalesTurnoverGrowthRate { get; set; }
        public string EmployeeQuantityGrowthRate { get; set; }
        public string ProfitOrLossAmount_UnitOfSize { get; set; }
        public string ProfitOrLossAmount_CurrencyISOAlpha3Code { get; set; }
        public string ProfitOrLossAmount { get; set; }
    }
}