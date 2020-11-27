using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class SearchModel
    {
        public string DUNS { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhoneNbr { get; set; }
        public string Zip { get; set; }
        public bool ExcludeNonHeadQuarters { get; set; }
        public bool ExcludeNonMarketable { get; set; }
        public bool ExcludeOutofBusiness { get; set; }
        public bool ExcludeUndeliverable { get; set; }
        public bool ExcludeUnreachable { get; set; }
        public List<MatchEntity> lstMatch { get; set; }
        public string Address2 { get; set; }
        public string Country { get; set; }
        public string Language { get; set; }
    }

    public class cShowMatchedItesDetailsModel
    {
        public string id { get; set; }
        public string childButtonId { get; set; }
        public string dataNext { get; set; }
        public string dataPrev { get; set; }
        public string count { get; set; }
        public string type { get; set; }
        public bool IsPartialView { get; set; }
    }
}