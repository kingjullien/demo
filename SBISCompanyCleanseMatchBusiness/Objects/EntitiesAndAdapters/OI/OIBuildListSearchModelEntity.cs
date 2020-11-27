using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI
{
    public class OIBuildListSearchModelEntity
    {
        public RequestFields request_fields { get; set; }
        public int results_count { get; set; }
        public List<Result> results { get; set; }
        public int ResultCount { get; set; }
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public long SearchResultsId { get; set; }
        public int NoOfRecored { get; set; }
        public string RequestedDateTime { get; set; }
    }
    public class RequestFields
    {
        public string api_key { get; set; }
        public string limit { get; set; }
        public int offset { get; set; }
        public string entity_type { get; set; }
        public string parent_orb_num { get; set; }
        public string ultimate_parent_orb_num { get; set; }
        public string industry { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string techs { get; set; }
        public string tech_categories { get; set; }
        public string category { get; set; }
        public string naics_codes { get; set; }
        public string sic_codes { get; set; }
        public string rankings { get; set; }
        public string importance_score { get; set; }
        public string cik { get; set; }
        public string cusip { get; set; }
        public string ticker { get; set; }
        public string exchange { get; set; }
        public bool show_full_profile { get; set; }
        public string include { get; set; }
        public string orb_num { get; set; }
        public string[] employees { get; set; }
        public string[] revenue { get; set; }
        public string name { get; set; }
        public string company_status { get; set; }
        public bool is_standalone_company { get; set; }
        public bool branches_count { get; set; }
    }
    public class Result
    {
        public string orb_num { get; set; }
        public string name { get; set; }
        public string entity_type { get; set; }
        public string company_status { get; set; }
        public string parent_orb_num { get; set; }
        public string parent_name { get; set; }
        public string ultimate_parent_orb_num { get; set; }
        public string ultimate_parent_name { get; set; }
        public int subsidiaries_count { get; set; }
        public int branches_count { get; set; }
        public bool is_standalone_company { get; set; }
        public string address1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string iso_country_code { get; set; }
        public string fetch_url { get; set; }
        public int offset { get; set; }
        public FullProfile full_profile { get; set; }
    }
    public class FullProfile
    {
        public int orb_num { get; set; }
        public List<int> orb_nums { get; set; }
        public string name { get; set; }
        public int? parent_orb_num { get; set; }
        public string parent_name { get; set; }
        public int? ultimate_parent_orb_num { get; set; }
        public string ultimate_parent_name { get; set; }
        public int subsidiaries_count { get; set; }
        public int branches_count { get; set; }
        public List<string> names { get; set; }
        public string website { get; set; }
        public string webdomain { get; set; }
        public List<string> webdomains { get; set; }
        public List<WebDomain> webdomains_info { get; set; }
        public Address address { get; set; }
        public string industry { get; set; }
        public string naics_code { get; set; }
        public string naics_description { get; set; }
        public List<NaicsCode> naics_codes { get; set; }
        public string sic_code { get; set; }
        public string sic_description { get; set; }
        public List<SicCode> sic_codes { get; set; }
        public List<Category> categories { get; set; }
        public string employees_range { get; set; }
        public int? employees { get; set; }
        public string revenue_range { get; set; }
        public string revenue { get; set; }
        public int? year_founded { get; set; }
        public string description { get; set; }
        public LinkedinAccount linkedin_account { get; set; }
        public FacebookAccount facebook_account { get; set; }
        public TwitterAccount twitter_account { get; set; }
        public GoogleplusAccount googleplus_account { get; set; }
        public YoutubeAccount youtube_account { get; set; }
        public List<Technologies> technologies { get; set; }
        public List<string> rankings { get; set; }
        public List<Ranking> ranking_positions { get; set; }
        public List<object> eins { get; set; }
        public List<object> npis { get; set; }
        public string favicon { get; set; }
        public int? total_funding { get; set; }
        public int? last_funding_round_amount { get; set; }
        public int? last_funding_round_year { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string importance_score { get; set; }
        public List<Ticker> tickers { get; set; }
        public long? market_cap { get; set; }
        public string cik { get; set; }
        public string cusip { get; set; }
        public string fiscal_year_end { get; set; }
        public int? cidrs_count { get; set; }
        public int? liveramp_idl_count { get; set; }
        public int? liveramp_device_count { get; set; }
    }
    public class Namelst
    {
        public string name { get; set; }
    }
    public class Technologies
    {
        public string name { get; set; }
        public string category { get; set; }
    }
    public class Tech_Categories
    {
        public string name { get; set; }
        public List<string> technologies { get; set; }
    }
    public class Codes
    {
        public string code { get; set; }
        public string description { get; set; }
    }
    public class Categories
    {
        public string canonical { get; set; }
        public List<Categories> categories { get; set; }
    }
    public class StockExchange
    {
        public string exchange { get; set; }
        public string description { get; set; }
        public string country { get; set; }
    }
    public class NaicsCode
    {
        public string code { get; set; }
        public string description { get; set; }
    }
    public class Ranking
    {
        public string ranking { get; set; }
        public int? position { get; set; }
    }
    public class Ticker
    {
        public string ticker { get; set; }
        public string exchange { get; set; }
    }
    public class WebDomain
    {
        public string webdomain { get; set; }
        public int? orb_web_rank { get; set; }
        public string domain_has_email { get; set; }
        public string domain_has_website { get; set; }
        public string domain_is_email_hosting { get; set; }
    }
    public class SicCode
    {
        public string code { get; set; }
        public string description { get; set; }
    }
    public class Category
    {
        public string name { get; set; }
        public int? weight { get; set; }
    }
    public class LinkedinAccount
    {
        public string url { get; set; }
    }
    public class FacebookAccount
    {
        public string url { get; set; }
        public int? likes { get; set; }
    }
    public class TwitterAccount
    {
        public string url { get; set; }
        public int? followers { get; set; }
    }
    public class GoogleplusAccount
    {
        public string url { get; set; }
    }
    public class YoutubeAccount
    {
        public string url { get; set; }
    }
    public class Address
    {
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }
}
