using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models.PreviewMatchData.DCP
{
    public class DCP_StockExchange
    {
        public string DnbDUNSNumber { get; set; }
        public string APIType { get; set; }
        public string TransactionTimestamp { get; set; }
        public string SocialMediaPlatformName { get; set; }
        public string SocialMediaPlatformCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string StockExchangeTickerName { get; set; }
        public string StockExchangeName { get; set; }
        public string PrimaryStockExchangeIndicator { get; set; }
    }
}