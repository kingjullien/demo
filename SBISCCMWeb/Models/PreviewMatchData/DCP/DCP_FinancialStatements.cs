using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_FinancialStatements
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string AccountantName { get; set; }
        public string FinancialRatioItem { get; set; }
        public string FinancialRatioItemDescription { get; set; }
        public string FinancialRatioItemDescriptionCode { get; set; }
        public string BalanceSheet_TotalAssetsAmount { get; set; }
        public string BalanceSheet_Liabilities_TotalEquityAmount { get; set; }
        public string UnitOfSize { get; set; }
        public string FinancialStatementToDate { get; set; }
        public string FinancialPeriodDuration { get; set; }
        public string CurrencyISOAlpha3Code { get; set; }

    }
}