using System;
using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OICompanyEntity
    {
        public string InputId { get; set; }
        public string SrcRecordId { get; set; }
        public int ImportProcessId { get; set; }
        public string InpCompanyName { get; set; }
        public string InpAddress1 { get; set; }
        public string InpAddress2 { get; set; }
        public string InpCity { get; set; }
        public string InpState { get; set; }
        public string InpPostalCode { get; set; }
        public string InpCountryISOAlpha2Code { get; set; }
        public string InpPhoneNbr { get; set; }
        public string InpOrbNum { get; set; }
        public string InpEIN { get; set; }
        public string InpCEOName { get; set; }
        public string InpWebsite { get; set; }
        public string InpEmail { get; set; }
        public string InpAltCompanyName { get; set; }
        public string InpAltAddress1 { get; set; }
        public string InpAltAddress2 { get; set; }
        public string InpAltCity { get; set; }
        public string InpAltState { get; set; }
        public string InpAltPostalCode { get; set; }
        public string InpAltCountryISOAlpha2Code { get; set; }
        public string Tags { get; set; }
        public string inpStdStreetNum { get; set; }
        public string inpStdStreetName { get; set; }
        public string InpStdCity { get; set; }
        public string InpStdState { get; set; }
        public string InpStdPostalCode { get; set; }
        public string inpStdAltStreetNum { get; set; }
        public string inpStdAltStreetName { get; set; }
        public string InpStdAltCity { get; set; }
        public string InpStdAltState { get; set; }
        public string InpStdAltPostalCode { get; set; }
        public int ProcessStatusId { get; set; }
        public bool Matched { get; set; }
        public string MatchedOrbNum { get; set; }
        public string MatchedOrbNumFirmographics { get; set; }
        public DateTime MatchedDateTime { get; set; }
        public string MatchedBy { get; set; }
        public bool Exported { get; set; }
        public DateTime ExportedDateTime { get; set; }
        public bool Deleted { get; set; }
        public int CandidateCount { get; set; }
        public int NbrMatchCalls { get; set; }
        public string OriginalSrcRecordId { get; set; }
    }
    public class LstOIMatchCompany
    {
        public List<OICompanyEntity> lstOICompany { get; set; }
        public string Message { get; set; }
    }

    public class SearchMatchEntity
    {
        public int SearchNum { get; set; }
        public string MatchId { get; set; }
        public string MatchType { get; set; }
        public int CandidateCount { get; set; }
        public string MatchURL { get; set; }
        public string Color { get; set; }
        public bool Selected { get; set; }
    }
    public class OICompanyMatchDetails
    {
        public string orb_num { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string entity_type { get; set; }
        public string iso_country_code { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string company_status { get; set; }
        public bool is_standalone_company { get; set; }
        public int subsidiaries_count { get; set; }
        public int branches_count { get; set; }
        public string parent_orb_num { get; set; }
        public string parent_name { get; set; }
        public string ultimate_parent_orb_num { get; set; }
        public string ultimate_parent_name { get; set; }
        public string ein0 { get; set; }
        public string fetch_url { get; set; }
        public int NbrInstances { get; set; }
        public int TotalCC { get; set; }
        public int AvgCC { get; set; }
        public int MaxCC { get; set; }
        public int MinCC { get; set; }
        public int MatchbookRank { get; set; }
        public string Color { get; set; }
    }

    public class OIlstMatchDetails
    {
        public OICompanyEntity lstOICompanyInput { get; set; }
        public List<SearchMatchEntity> lstOISearchMatch { get; set; }
        public List<OICompanyMatchDetails> lstOIMatchDetail { get; set; }
    }

    public class OIMatchQualityMetadata
    {
        public int MatchbookRank { get; set; }
        public int NbrInstances { get; set; }
        public int TotalCC { get; set; }
        public int AvgCC { get; set; }
        public int MaxCC { get; set; }
        public int MinCC { get; set; }
    }
    public class OIMatchMetaData
    {
        public int sequence_num { get; set; }
        public int MatchId { get; set; }
        public int result_number { get; set; }
        public string orb_num { get; set; }
        public string name { get; set; }
        public string address1 { get; set; }
        public string street_num { get; set; }
        public string street_name { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
        public string iso_country_code { get; set; }
        public string entity_type { get; set; }
        public string company_status { get; set; }
        public string is_standalone_company { get; set; }
        public string branches_count { get; set; }
        public string subsidiaries_count { get; set; }
        public string parent_orb_num { get; set; }
        public string parent_name { get; set; }
        public string ultimate_parent_orb_num { get; set; }
        public string ultimate_parent_name { get; set; }
        public string ein0 { get; set; }
        public string ein1 { get; set; }
        public string fetch_url { get; set; }
        public string confidence_score { get; set; }
        public string matched_name { get; set; }
        public string matched_other_name { get; set; }
        public string matched_address1 { get; set; }
        public string matched_city { get; set; }
        public string matched_state { get; set; }
        public string matched_zip { get; set; }
        public string matched_country { get; set; }
        public string matched_phone { get; set; }
        public string matched_ein { get; set; }
        public string matched_website { get; set; }
        public string mg_name { get; set; }
        public string mg_street_num { get; set; }
        public string mg_street_name { get; set; }
        public string mg_city { get; set; }
        public string mg_state { get; set; }
        public string mg_postal_code { get; set; }
        public string mg_phone { get; set; }
        public string mg_webdomain { get; set; }
        public string mg_country { get; set; }
        public string mg_ein { get; set; }
        public string score_name { get; set; }
        public string score_streetname { get; set; }
        public string mdp_name { get; set; }
        public string mdp_webdomain { get; set; }

        public string MatchUrl { get; set; }
    }

    public class OIlstMatchMetaDetails
    {
        public OICompanyEntity lstOICompanyInput { get; set; }
        public List<OIMatchQualityMetadata> lstMatchQualityMetadatas { get; set; }
        public List<OIMatchMetaData> lstMatchMetaDatas { get; set; }
    }
}
