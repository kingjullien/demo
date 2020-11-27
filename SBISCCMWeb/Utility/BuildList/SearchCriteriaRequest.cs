using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility.BuildList
{
    public class YearlyRevenue
    {
        public int? minimumValue { get; set; }
        public int? maximumValue { get; set; }
    }

    public class LocationRadius
    {
        public double lat { get; set; }
        public double lon { get; set; }
        public double radius { get; set; }
        public string unit { get; set; }
    }

    public class GlobalUltimateFamilyTreeMembersCount
    {
        public int minimumValue { get; set; }
        public int maximumValue { get; set; }
    }

    public class NumberOfEmployees
    {
        public int informationScope { get; set; }
        public int? minimumValue { get; set; }
        public int? maximumValue { get; set; }
    }

    public class SearchCriteriaRequest
    {
        public string searchTerm { get; set; }
        public string countryISOAlpha2Code { get; set; }
        public string duns { get; set; }
        public bool? isOutOfBusiness { get; set; }
        public bool? isMarketable { get; set; }
        public bool? isMailUndeliverable { get; set; }
        public List<string> usSicV4 { get; set; }
        public YearlyRevenue yearlyRevenue { get; set; }
        public bool? isTelephoneDisconnected { get; set; }
        public string telephoneNumber { get; set; }
        public string domain { get; set; }
        public List<string> registrationNumbers { get; set; }
        public List<int> businessEntityType { get; set; }
        public string addressLocality { get; set; }
        public string addressRegion { get; set; }
        public string streetAddressLine1 { get; set; }
        public string postalCode { get; set; }
        public LocationRadius locationRadius { get; set; }
        public string primaryName { get; set; }
        public string tradeStyleName { get; set; }
        public string tickerSymbol { get; set; }
        public List<int> familytreeRolesPlayed { get; set; }
        public GlobalUltimateFamilyTreeMembersCount globalUltimateFamilyTreeMembersCount { get; set; }
        public NumberOfEmployees numberOfEmployees { get; set; }
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }


}