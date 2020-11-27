using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class OICompanyAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<OICompanyEntity> Adapt(DataTable dt)
        {
            List<OICompanyEntity> results = new List<OICompanyEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                OICompanyEntity cust = new OICompanyEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public OICompanyEntity AdaptItem(DataRow rw)
        {
            OICompanyEntity result = new OICompanyEntity();
            if (rw.Table.Columns["InputId"] != null)
            {
                result.InputId = SafeHelper.GetSafestring(rw["InputId"]);
            }

            if (rw.Table.Columns["SrcRecordId"] != null)
            {
                result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            }

            if (rw.Table.Columns["ImportProcessId"] != null)
            {
                result.ImportProcessId = SafeHelper.GetSafeint(rw["ImportProcessId"]);
            }

            if (rw.Table.Columns["InpCompanyName"] != null)
            {
                result.InpCompanyName = SafeHelper.GetSafestring(rw["InpCompanyName"]);
            }

            if (rw.Table.Columns["InpAddress1"] != null)
            {
                result.InpAddress1 = SafeHelper.GetSafestring(rw["InpAddress1"]);
            }

            if (rw.Table.Columns["InpAddress2"] != null)
            {
                result.InpAddress2 = SafeHelper.GetSafestring(rw["InpAddress2"]);
            }

            if (rw.Table.Columns["InpCity"] != null)
            {
                result.InpCity = SafeHelper.GetSafestring(rw["InpCity"]);
            }

            if (rw.Table.Columns["InpState"] != null)
            {
                result.InpState = SafeHelper.GetSafestring(rw["InpState"]);
            }

            if (rw.Table.Columns["InpPostalCode"] != null)
            {
                result.InpPostalCode = SafeHelper.GetSafestring(rw["InpPostalCode"]);
            }

            if (rw.Table.Columns["InpCountryISOAlpha2Code"] != null)
            {
                result.InpCountryISOAlpha2Code = SafeHelper.GetSafestring(rw["InpCountryISOAlpha2Code"]);
            }

            if (rw.Table.Columns["InpPhoneNbr"] != null)
            {
                result.InpPhoneNbr = SafeHelper.GetSafestring(rw["InpPhoneNbr"]);
            }

            if (rw.Table.Columns["InpOrbNum"] != null)
            {
                result.InpOrbNum = SafeHelper.GetSafestring(rw["InpOrbNum"]);
            }

            if (rw.Table.Columns["InpEIN"] != null)
            {
                result.InpEIN = SafeHelper.GetSafestring(rw["InpEIN"]);
            }

            if (rw.Table.Columns["InpCEOName"] != null)
            {
                result.InpCEOName = SafeHelper.GetSafestring(rw["InpCEOName"]);
            }

            if (rw.Table.Columns["InpWebsite"] != null)
            {
                result.InpWebsite = SafeHelper.GetSafestring(rw["InpWebsite"]);
            }

            if (rw.Table.Columns["InpEmail"] != null)
            {
                result.InpEmail = SafeHelper.GetSafestring(rw["InpEmail"]);
            }

            if (rw.Table.Columns["InpAltCompanyName"] != null)
            {
                result.InpAltCompanyName = SafeHelper.GetSafestring(rw["InpAltCompanyName"]);
            }

            if (rw.Table.Columns["InpAltAddress1"] != null)
            {
                result.InpAltAddress1 = SafeHelper.GetSafestring(rw["InpAltAddress1"]);
            }

            if (rw.Table.Columns["InpAltAddress2"] != null)
            {
                result.InpAltAddress2 = SafeHelper.GetSafestring(rw["InpAltAddress2"]);
            }

            if (rw.Table.Columns["InpAltCity"] != null)
            {
                result.InpAltCity = SafeHelper.GetSafestring(rw["InpAltCity"]);
            }

            if (rw.Table.Columns["InpAltState"] != null)
            {
                result.InpAltState = SafeHelper.GetSafestring(rw["InpAltState"]);
            }

            if (rw.Table.Columns["InpAltPostalCode"] != null)
            {
                result.InpAltPostalCode = SafeHelper.GetSafestring(rw["InpAltPostalCode"]);
            }

            if (rw.Table.Columns["InpAltCountryISOAlpha2Code"] != null)
            {
                result.InpAltCountryISOAlpha2Code = SafeHelper.GetSafestring(rw["InpAltCountryISOAlpha2Code"]);
            }

            if (rw.Table.Columns["Tags"] != null)
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (rw.Table.Columns["inpStdStreetNum"] != null)
            {
                result.inpStdStreetNum = SafeHelper.GetSafestring(rw["inpStdStreetNum"]);
            }

            if (rw.Table.Columns["inpStdStreetName"] != null)
            {
                result.inpStdStreetName = SafeHelper.GetSafestring(rw["inpStdStreetName"]);
            }

            if (rw.Table.Columns["InpStdCity"] != null)
            {
                result.InpStdCity = SafeHelper.GetSafestring(rw["InpStdCity"]);
            }

            if (rw.Table.Columns["InpStdState"] != null)
            {
                result.InpStdState = SafeHelper.GetSafestring(rw["InpStdState"]);
            }

            if (rw.Table.Columns["InpStdPostalCode"] != null)
            {
                result.InpStdPostalCode = SafeHelper.GetSafestring(rw["InpStdPostalCode"]);
            }

            if (rw.Table.Columns["inpStdAltStreetNum"] != null)
            {
                result.inpStdAltStreetNum = SafeHelper.GetSafestring(rw["inpStdAltStreetNum"]);
            }

            if (rw.Table.Columns["inpStdAltStreetName"] != null)
            {
                result.inpStdAltStreetName = SafeHelper.GetSafestring(rw["inpStdAltStreetName"]);
            }

            if (rw.Table.Columns["InpStdAltCity"] != null)
            {
                result.InpStdAltCity = SafeHelper.GetSafestring(rw["InpStdAltCity"]);
            }

            if (rw.Table.Columns["InpStdAltState"] != null)
            {
                result.InpStdAltState = SafeHelper.GetSafestring(rw["InpStdAltState"]);
            }

            if (rw.Table.Columns["InpStdAltPostalCode"] != null)
            {
                result.InpStdAltPostalCode = SafeHelper.GetSafestring(rw["InpStdAltPostalCode"]);
            }

            if (rw.Table.Columns["ProcessStatusId"] != null)
            {
                result.ProcessStatusId = SafeHelper.GetSafeint(rw["ProcessStatusId"]);
            }

            if (rw.Table.Columns["Matched"] != null)
            {
                result.Matched = SafeHelper.GetSafebool(rw["Matched"]);
            }

            if (rw.Table.Columns["MatchedOrbNum"] != null)
            {
                result.MatchedOrbNum = SafeHelper.GetSafestring(rw["MatchedOrbNum"]);
            }

            if (rw.Table.Columns["MatchedOrbNumFirmographics"] != null)
            {
                result.MatchedOrbNumFirmographics = SafeHelper.GetSafestring(rw["MatchedOrbNumFirmographics"]);
            }

            if (rw.Table.Columns["MatchedDateTime"] != null)
            {
                result.MatchedDateTime = SafeHelper.GetSafeDateTime(rw["MatchedDateTime"]);
            }

            if (rw.Table.Columns["MatchedBy"] != null)
            {
                result.MatchedBy = SafeHelper.GetSafestring(rw["MatchedBy"]);
            }

            if (rw.Table.Columns["Exported"] != null)
            {
                result.Exported = SafeHelper.GetSafebool(rw["Exported"]);
            }

            if (rw.Table.Columns["ExportedDateTime"] != null)
            {
                result.ExportedDateTime = SafeHelper.GetSafeDateTime(rw["ExportedDateTime"]);
            }

            if (rw.Table.Columns["Deleted"] != null)
            {
                result.Deleted = SafeHelper.GetSafebool(rw["Deleted"]);
            }

            if (rw.Table.Columns["CandidateCount"] != null)
            {
                result.CandidateCount = SafeHelper.GetSafeint(rw["CandidateCount"]);
            }

            if (rw.Table.Columns["NbrMatchCalls"] != null)
            {
                result.NbrMatchCalls = SafeHelper.GetSafeint(rw["NbrMatchCalls"]);
            }

            return result;
        }



        public List<SearchMatchEntity> SearchMatchAdapt(DataTable dt)
        {
            List<SearchMatchEntity> results = new List<SearchMatchEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                SearchMatchEntity cust = new SearchMatchEntity();
                cust = SearchMatchAdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public SearchMatchEntity SearchMatchAdaptItem(DataRow rw)
        {
            SearchMatchEntity result = new SearchMatchEntity();
            if (rw.Table.Columns["SearchNum"] != null)
            {
                result.SearchNum = SafeHelper.GetSafeint(rw["SearchNum"]);
            }

            if (rw.Table.Columns["MatchId"] != null)
            {
                result.MatchId = SafeHelper.GetSafestring(rw["MatchId"]);
            }

            if (rw.Table.Columns["MatchType"] != null)
            {
                result.MatchType = SafeHelper.GetSafestring(rw["MatchType"]);
            }

            if (rw.Table.Columns["CandidateCount"] != null)
            {
                result.CandidateCount = SafeHelper.GetSafeint(rw["CandidateCount"]);
            }

            if (rw.Table.Columns["MatchURL"] != null)
            {
                result.MatchURL = SafeHelper.GetSafestring(rw["MatchURL"]);
            }

            if (rw.Table.Columns["Color"] != null)
            {
                result.Color = SafeHelper.GetSafestring(rw["Color"]);
            }

            if (rw.Table.Columns["Selected"] != null)
            {
                result.Selected = SafeHelper.GetSafebool(rw["Selected"]);
            }

            return result;
        }


        public List<OICompanyMatchDetails> OICompanyMatchDetailsAdapt(DataTable dt)
        {
            List<OICompanyMatchDetails> results = new List<OICompanyMatchDetails>();
            foreach (DataRow rw in dt.Rows)
            {
                OICompanyMatchDetails cust = new OICompanyMatchDetails();
                cust = OICompanyMatchDetailsAdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public OICompanyMatchDetails OICompanyMatchDetailsAdaptItem(DataRow rw)
        {
            OICompanyMatchDetails result = new OICompanyMatchDetails();
            if (rw.Table.Columns["orb_num"] != null)
            {
                result.orb_num = SafeHelper.GetSafestring(rw["orb_num"]);
            }

            if (rw.Table.Columns["name"] != null)
            {
                result.name = SafeHelper.GetSafestring(rw["name"]);
            }

            if (rw.Table.Columns["address1"] != null)
            {
                result.address1 = SafeHelper.GetSafestring(rw["address1"]);
            }

            if (rw.Table.Columns["entity_type"] != null)
            {
                result.entity_type = SafeHelper.GetSafestring(rw["entity_type"]);
            }

            if (rw.Table.Columns["iso_country_code"] != null)
            {
                result.iso_country_code = SafeHelper.GetSafestring(rw["iso_country_code"]);
            }

            if (rw.Table.Columns["city"] != null)
            {
                result.city = SafeHelper.GetSafestring(rw["city"]);
            }

            if (rw.Table.Columns["state"] != null)
            {
                result.state = SafeHelper.GetSafestring(rw["state"]);
            }

            if (rw.Table.Columns["zip"] != null)
            {
                result.zip = SafeHelper.GetSafestring(rw["zip"]);
            }

            if (rw.Table.Columns["company_status"] != null)
            {
                result.company_status = SafeHelper.GetSafestring(rw["company_status"]);
            }

            if (rw.Table.Columns["is_standalone_company"] != null)
            {
                result.is_standalone_company = SafeHelper.GetSafebool(rw["is_standalone_company"]);
            }

            if (rw.Table.Columns["subsidiaries_count"] != null)
            {
                result.subsidiaries_count = SafeHelper.GetSafeint(rw["subsidiaries_count"]);
            }

            if (rw.Table.Columns["branches_count"] != null)
            {
                result.branches_count = SafeHelper.GetSafeint(rw["branches_count"]);
            }

            if (rw.Table.Columns["parent_orb_num"] != null)
            {
                result.parent_orb_num = SafeHelper.GetSafestring(rw["parent_orb_num"]);
            }

            if (rw.Table.Columns["parent_name"] != null)
            {
                result.parent_name = SafeHelper.GetSafestring(rw["parent_name"]);
            }

            if (rw.Table.Columns["ultimate_parent_orb_num"] != null)
            {
                result.ultimate_parent_orb_num = SafeHelper.GetSafestring(rw["ultimate_parent_orb_num"]);
            }

            if (rw.Table.Columns["ein0"] != null)
            {
                result.ein0 = SafeHelper.GetSafestring(rw["ein0"]);
            }

            if (rw.Table.Columns["fetch_url"] != null)
            {
                result.fetch_url = SafeHelper.GetSafestring(rw["fetch_url"]);
            }

            if (rw.Table.Columns["NbrInstances"] != null)
            {
                result.NbrInstances = SafeHelper.GetSafeint(rw["NbrInstances"]);
            }

            if (rw.Table.Columns["TotalCC"] != null)
            {
                result.TotalCC = SafeHelper.GetSafeint(rw["TotalCC"]);
            }

            if (rw.Table.Columns["AvgCC"] != null)
            {
                result.AvgCC = SafeHelper.GetSafeint(rw["AvgCC"]);
            }

            if (rw.Table.Columns["MaxCC"] != null)
            {
                result.MaxCC = SafeHelper.GetSafeint(rw["MaxCC"]);
            }

            if (rw.Table.Columns["MinCC"] != null)
            {
                result.MinCC = SafeHelper.GetSafeint(rw["MinCC"]);
            }

            if (rw.Table.Columns["MatchbookRank"] != null)
            {
                result.MatchbookRank = SafeHelper.GetSafeint(rw["MatchbookRank"]);
            }

            if (rw.Table.Columns["Color"] != null)
            {
                result.Color = SafeHelper.GetSafestring(rw["Color"]);
            }

            return result;
        }



        public List<OIMatchQualityMetadata> SearchMatchMetaDataAdapt(DataTable dt)
        {
            List<OIMatchQualityMetadata> results = new List<OIMatchQualityMetadata>();
            foreach (DataRow rw in dt.Rows)
            {
                OIMatchQualityMetadata cust = new OIMatchQualityMetadata();
                cust = SearchMatchMetaDataAdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public OIMatchQualityMetadata SearchMatchMetaDataAdaptItem(DataRow rw)
        {
            OIMatchQualityMetadata result = new OIMatchQualityMetadata();
            if (rw.Table.Columns["MatchbookRank"] != null)
            {
                result.MatchbookRank = SafeHelper.GetSafeint(rw["MatchbookRank"]);
            }

            if (rw.Table.Columns["NbrInstances"] != null)
            {
                result.NbrInstances = SafeHelper.GetSafeint(rw["NbrInstances"]);
            }

            if (rw.Table.Columns["TotalCC"] != null)
            {
                result.TotalCC = SafeHelper.GetSafeint(rw["TotalCC"]);
            }

            if (rw.Table.Columns["AvgCC"] != null)
            {
                result.AvgCC = SafeHelper.GetSafeint(rw["AvgCC"]);
            }

            if (rw.Table.Columns["MaxCC"] != null)
            {
                result.MaxCC = SafeHelper.GetSafeint(rw["MaxCC"]);
            }

            if (rw.Table.Columns["MinCC"] != null)
            {
                result.MinCC = SafeHelper.GetSafeint(rw["MinCC"]);
            }

            return result;
        }


        public List<OIMatchMetaData> MatchMetaDataDetailAdapt(DataTable dt)
        {
            List<OIMatchMetaData> results = new List<OIMatchMetaData>();
            foreach (DataRow rw in dt.Rows)
            {
                OIMatchMetaData cust = new OIMatchMetaData();
                cust = MatchMetaDataDetailAdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }
        public OIMatchMetaData MatchMetaDataDetailAdaptItem(DataRow rw)
        {
            OIMatchMetaData result = new OIMatchMetaData();
            if (rw.Table.Columns["sequence_num"] != null)
            {
                result.sequence_num = SafeHelper.GetSafeint(rw["sequence_num"]);
            }

            if (rw.Table.Columns["MatchId"] != null)
            {
                result.MatchId = SafeHelper.GetSafeint(rw["MatchId"]);
            }

            if (rw.Table.Columns["result_number"] != null)
            {
                result.result_number = SafeHelper.GetSafeint(rw["result_number"]);
            }

            if (rw.Table.Columns["orb_num"] != null)
            {
                result.orb_num = SafeHelper.GetSafestring(rw["orb_num"]);
            }

            if (rw.Table.Columns["name"] != null)
            {
                result.name = SafeHelper.GetSafestring(rw["name"]);
            }

            if (rw.Table.Columns["address1"] != null)
            {
                result.address1 = SafeHelper.GetSafestring(rw["address1"]);
            }

            if (rw.Table.Columns["street_num"] != null)
            {
                result.street_num = SafeHelper.GetSafestring(rw["street_num"]);
            }

            if (rw.Table.Columns["city"] != null)
            {
                result.city = SafeHelper.GetSafestring(rw["city"]);
            }

            if (rw.Table.Columns["state"] != null)
            {
                result.state = SafeHelper.GetSafestring(rw["state"]);
            }

            if (rw.Table.Columns["zip"] != null)
            {
                result.zip = SafeHelper.GetSafestring(rw["zip"]);
            }

            if (rw.Table.Columns["country"] != null)
            {
                result.country = SafeHelper.GetSafestring(rw["country"]);
            }

            if (rw.Table.Columns["iso_country_code"] != null)
            {
                result.iso_country_code = SafeHelper.GetSafestring(rw["iso_country_code"]);
            }

            if (rw.Table.Columns["entity_type"] != null)
            {
                result.entity_type = SafeHelper.GetSafestring(rw["entity_type"]);
            }

            if (rw.Table.Columns["company_status"] != null)
            {
                result.company_status = SafeHelper.GetSafestring(rw["company_status"]);
            }

            if (rw.Table.Columns["is_standalone_company"] != null)
            {
                result.is_standalone_company = SafeHelper.GetSafestring(rw["is_standalone_company"]);
            }

            if (rw.Table.Columns["branches_count"] != null)
            {
                result.branches_count = SafeHelper.GetSafestring(rw["branches_count"]);
            }

            if (rw.Table.Columns["subsidiaries_count"] != null)
            {
                result.subsidiaries_count = SafeHelper.GetSafestring(rw["subsidiaries_count"]);
            }

            if (rw.Table.Columns["parent_orb_num"] != null)
            {
                result.parent_orb_num = SafeHelper.GetSafestring(rw["parent_orb_num"]);
            }

            if (rw.Table.Columns["parent_name"] != null)
            {
                result.parent_name = SafeHelper.GetSafestring(rw["parent_name"]);
            }

            if (rw.Table.Columns["ultimate_parent_orb_num"] != null)
            {
                result.ultimate_parent_orb_num = SafeHelper.GetSafestring(rw["ultimate_parent_orb_num"]);
            }

            if (rw.Table.Columns["ultimate_parent_name"] != null)
            {
                result.ultimate_parent_name = SafeHelper.GetSafestring(rw["ultimate_parent_name"]);
            }

            if (rw.Table.Columns["ein0"] != null)
            {
                result.ein0 = SafeHelper.GetSafestring(rw["ein0"]);
            }

            if (rw.Table.Columns["ein1"] != null)
            {
                result.ein1 = SafeHelper.GetSafestring(rw["ein1"]);
            }

            if (rw.Table.Columns["fetch_url"] != null)
            {
                result.fetch_url = SafeHelper.GetSafestring(rw["fetch_url"]);
            }

            if (rw.Table.Columns["confidence_score"] != null)
            {
                result.confidence_score = SafeHelper.GetSafestring(rw["confidence_score"]);
            }

            if (rw.Table.Columns["matched_name"] != null)
            {
                result.matched_name = SafeHelper.GetSafestring(rw["matched_name"]);
            }

            if (rw.Table.Columns["matched_other_name"] != null)
            {
                result.matched_other_name = SafeHelper.GetSafestring(rw["matched_other_name"]);
            }

            if (rw.Table.Columns["matched_address1"] != null)
            {
                result.matched_address1 = SafeHelper.GetSafestring(rw["matched_address1"]);
            }

            if (rw.Table.Columns["matched_city"] != null)
            {
                result.matched_city = SafeHelper.GetSafestring(rw["matched_city"]);
            }

            if (rw.Table.Columns["matched_state"] != null)
            {
                result.matched_state = SafeHelper.GetSafestring(rw["matched_state"]);
            }

            if (rw.Table.Columns["matched_zip"] != null)
            {
                result.matched_zip = SafeHelper.GetSafestring(rw["matched_zip"]);
            }

            if (rw.Table.Columns["matched_country"] != null)
            {
                result.matched_country = SafeHelper.GetSafestring(rw["matched_country"]);
            }

            if (rw.Table.Columns["matched_phone"] != null)
            {
                result.matched_phone = SafeHelper.GetSafestring(rw["matched_phone"]);
            }

            if (rw.Table.Columns["matched_ein"] != null)
            {
                result.matched_ein = SafeHelper.GetSafestring(rw["matched_ein"]);
            }

            if (rw.Table.Columns["matched_website"] != null)
            {
                result.matched_website = SafeHelper.GetSafestring(rw["matched_website"]);
            }

            if (rw.Table.Columns["mg_name"] != null)
            {
                result.mg_name = SafeHelper.GetSafestring(rw["mg_name"]);
            }

            if (rw.Table.Columns["mg_street_num"] != null)
            {
                result.mg_street_num = SafeHelper.GetSafestring(rw["mg_street_num"]);
            }

            if (rw.Table.Columns["mg_street_name"] != null)
            {
                result.mg_street_name = SafeHelper.GetSafestring(rw["mg_street_name"]);
            }

            if (rw.Table.Columns["mg_city"] != null)
            {
                result.mg_city = SafeHelper.GetSafestring(rw["mg_city"]);
            }

            if (rw.Table.Columns["mg_state"] != null)
            {
                result.mg_state = SafeHelper.GetSafestring(rw["mg_state"]);
            }

            if (rw.Table.Columns["mg_postal_code"] != null)
            {
                result.mg_postal_code = SafeHelper.GetSafestring(rw["mg_postal_code"]);
            }

            if (rw.Table.Columns["mg_phone"] != null)
            {
                result.mg_phone = SafeHelper.GetSafestring(rw["mg_phone"]);
            }

            if (rw.Table.Columns["mg_webdomain"] != null)
            {
                result.mg_webdomain = SafeHelper.GetSafestring(rw["mg_webdomain"]);
            }

            if (rw.Table.Columns["mg_country"] != null)
            {
                result.mg_country = SafeHelper.GetSafestring(rw["mg_country"]);
            }

            if (rw.Table.Columns["mg_ein"] != null)
            {
                result.mg_ein = SafeHelper.GetSafestring(rw["mg_ein"]);
            }

            if (rw.Table.Columns["score_name"] != null)
            {
                result.score_name = SafeHelper.GetSafestring(rw["score_name"]);
            }

            if (rw.Table.Columns["score_streetname"] != null)
            {
                result.score_streetname = SafeHelper.GetSafestring(rw["score_streetname"]);
            }

            if (rw.Table.Columns["mdp_name"] != null)
            {
                result.mdp_name = SafeHelper.GetSafestring(rw["mdp_name"]);
            }

            if (rw.Table.Columns["mdp_webdomain"] != null)
            {
                result.mdp_webdomain = SafeHelper.GetSafestring(rw["mdp_webdomain"]);
            }

            if (rw.Table.Columns["MatchUrl"] != null)
            {
                result.MatchUrl = SafeHelper.GetSafestring(rw["MatchUrl"]);
            }

            return result;
        }


    }

}
