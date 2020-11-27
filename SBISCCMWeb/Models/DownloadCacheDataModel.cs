using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class DownloadCacheDataModel
    {
        public int CleanseMatchResultId { get; set; }
        public string ClientCode { get; set; }
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryISOAlpha2Code { get; set; }
        public string PhoneNbr { get; set; }
        public string DUNSNumber { get; set; }
        public string ResponseJSON { get; set; }
        public string APIFamily { get; set; }
    }
}