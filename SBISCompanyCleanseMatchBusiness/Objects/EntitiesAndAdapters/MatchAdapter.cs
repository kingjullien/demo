using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class MatchAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<MatchEntity> Adapt(DataTable dt)
        {
            List<MatchEntity> results = new List<MatchEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                MatchEntity cust = new MatchEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public MatchEntity AdaptItem(DataRow rw)
        {
            MatchEntity result = new MatchEntity();
            result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            result.DnBDUNSNumber = SafeHelper.GetSafestring(rw["DnBDUNSNumber"]);
            result.DnBOrganizationName = SafeHelper.GetSafestring(rw["DnBOrganizationName"]);
            result.DnBTradeStyleName = SafeHelper.GetSafestring(rw["DnBTradeStyleName"]);
            result.DnBSeniorPrincipalName = SafeHelper.GetSafestring(rw["DnBSeniorPrincipalName"]);
            result.DnBStreetAddressLine = SafeHelper.GetSafestring(rw["DnBStreetAddressLine"]);
            result.DnBPrimaryTownName = SafeHelper.GetSafestring(rw["DnBPrimaryTownName"]);
            result.DnBCountryISOAlpha2Code = SafeHelper.GetSafestring(rw["DnBCountryISOAlpha2Code"]);
            result.DnBPostalCode = SafeHelper.GetSafestring(rw["DnBPostalCode"]);
            result.DnBPostalCodeExtensionCode = SafeHelper.GetSafestring(rw["DnBPostalCodeExtensionCode"]);
            result.DnBTerritoryAbbreviatedName = SafeHelper.GetSafestring(rw["DnBTerritoryAbbreviatedName"]);
            result.DnBAddressUndeliverable = SafeHelper.GetSafestring(rw["DnBAddressUndeliverable"]);
            result.DnBTelephoneNumber = SafeHelper.GetSafestring(rw["DnBTelephoneNumber"]);
            result.DnBOperatingStatus = SafeHelper.GetSafestring(rw["DnBOperatingStatus"]);
            result.DnBFamilyTreeMemberRole = SafeHelper.GetSafestring(rw["DnBFamilyTreeMemberRole"]);
            result.DnBStandaloneOrganization = SafeHelper.GetSafestring(rw["DnBStandaloneOrganization"]);
            result.DnBConfidenceCode = SafeHelper.GetSafeint(rw["DnBConfidenceCode"]);
            result.DnBMatchGradeText = SafeHelper.GetSafestring(rw["DnBMatchGradeText"]);
            result.DnBMatchDataProfileText = SafeHelper.GetSafestring(rw["DnBMatchDataProfileText"]);
            result.DnBMatchDataProfileComponentCount = SafeHelper.GetSafeint(rw["DnBMatchDataProfileComponentCount"]);
            result.DnBDisplaySequence = SafeHelper.GetSafestring(rw["DnBDisplaySequence"]);
            result.TTCompanyName = SafeHelper.GetSafestring(rw["TTCompanyName"]);
            result.TTAddress = SafeHelper.GetSafestring(rw["TTAddress"]);
            result.TTCity = SafeHelper.GetSafestring(rw["TTCity"]);
            result.TTState = SafeHelper.GetSafestring(rw["TTState"]);
            result.TTPhoneNbr = SafeHelper.GetSafestring(rw["TTPhoneNbr"]);
            result.MGVCompanyName = SafeHelper.GetSafestring(rw["MGVCompanyName"]);
            result.MGVStreetNo = SafeHelper.GetSafestring(rw["MGVStreetNo"]);
            result.MGVStreetName = SafeHelper.GetSafestring(rw["MGVStreetName"]);
            result.MGVCity = SafeHelper.GetSafestring(rw["MGVCity"]);
            result.MGVState = SafeHelper.GetSafestring(rw["MGVState"]);
            result.MGVMailingAddress = SafeHelper.GetSafestring(rw["MGVPOBox"]);
            result.MGVTelephone = SafeHelper.GetSafestring(rw["MGVTelephone"]);
            result.MGVZipCode = SafeHelper.GetSafestring(rw["MGVZipCode"]);
            result.MGVDensity = SafeHelper.GetSafestring(rw["MGVDensity"]);
            result.MGVUniqueness = SafeHelper.GetSafestring(rw["MGVUniqueness"]);
            result.MGVSIC = SafeHelper.GetSafestring(rw["MGVSIC"]);
            result.MDPVCompanyName = SafeHelper.GetSafestring(rw["MDPVCompanyName"]);
            result.MDPVStreetNo = SafeHelper.GetSafestring(rw["MDPVStreetNo"]);
            result.MDPVStreetName = SafeHelper.GetSafestring(rw["MDPVStreetName"]);
            result.MDPVCity = SafeHelper.GetSafestring(rw["MDPVCity"]);
            result.MDPVState = SafeHelper.GetSafestring(rw["MDPVState"]);
            result.MDPVMailingAddress = SafeHelper.GetSafestring(rw["MDPVPOBox"]);
            result.MDPVTelephone = SafeHelper.GetSafestring(rw["MDPVTelephone"]);
            result.MDPVZipCode = SafeHelper.GetSafestring(rw["MDPVZipCode"]);
            result.MDPVDensity = SafeHelper.GetSafestring(rw["MDPVDensity"]);
            result.MDPVUniqueness = SafeHelper.GetSafestring(rw["MDPVUniqueness"]);
            result.MDPVSIC = SafeHelper.GetSafestring(rw["MDPVSIC"]);
            result.MDPVDUNS = SafeHelper.GetSafestring(rw["MDPVDUNS"]);
            result.MDPVNationalID = SafeHelper.GetSafestring(rw["MDPVNationalID"]);
            result.MDPVURL = SafeHelper.GetSafestring(rw["MDPVURL"]);
            result.MDPPhysicalAddress = SafeHelper.GetSafestring(rw["MDPPhysicalAddress"]);
            result.MDPPhone = SafeHelper.GetSafestring(rw["MDPTelephone"]);
            result.MDPCompanyName = SafeHelper.GetSafestring(rw["MDPCompanyName"]);
            result.MDPMailingAddress = SafeHelper.GetSafestring(rw["MDPPOBox"]);
            result.MGCompanyName = SafeHelper.GetSafestring(rw["MGCompanyName"]);
            result.MGStreetNo = SafeHelper.GetSafestring(rw["MGStreetNo"]);
            result.MGStreetName = SafeHelper.GetSafestring(rw["MGStreetName"]);
            result.MGCity = SafeHelper.GetSafestring(rw["MGCity"]);
            result.MGState = SafeHelper.GetSafestring(rw["MGState"]);
            result.MGTelephone = SafeHelper.GetSafestring(rw["MGTelephone"]);
            result.MGZipCode = SafeHelper.GetSafestring(rw["MGZipCode"]);
            result.MGDensity = SafeHelper.GetSafestring(rw["MGDensity"]);
            result.MGUniqueness = SafeHelper.GetSafestring(rw["MGUniqueness"]);
            result.MGSIC = SafeHelper.GetSafestring(rw["MGSIC"]);
            result.MGMailingAddress = SafeHelper.GetSafestring(rw["MGPOBox"]);
            result.IsSelected = SafeHelper.GetSafebool(rw["IsSelected"]);
            result.MatchDataCriteriaText = SafeHelper.GetSafestring(rw["MatchDataCriteriaText"]);
            if (rw.Table.Columns["InputId"] != null)
            {
                result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            }

            if (rw.Table.Columns["DnBMailingAddress"] != null)
            {
                result.DnBMailingAddress = SafeHelper.GetSafestring(rw["DnBMailingAddress"]);
            }

            if (rw.Table.Columns["DnBMailingAddressUndeliverable"] != null)
            {
                result.DnBMailingAddressUndeliverable = SafeHelper.GetSafebool(rw["DnBMailingAddressUndeliverable"]);
            }

            result.DnBMarketabilityIndicator = rw["DnBMarketabilityIndicator"] != DBNull.Value ? Convert.ToBoolean(rw["DnBMarketabilityIndicator"]) : new bool?();

            if (rw.Table.Columns["DnBTelephoneNumberUnreachableIndicator"] != null)
            {
                result.DnBTelephoneNumberUnreachableIndicator = SafeHelper.GetSafebool(rw["DnBTelephoneNumberUnreachableIndicator"]);
            }

            if (rw.Table.Columns["ScoreCompany"] != null)
            {
                result.ScoreCompany = SafeHelper.GetSafestring(rw["ScoreCompany"]);
            }

            if (rw.Table.Columns["RegistrationNumbers"] != null)
            {
                result.RegistrationNumbers = SafeHelper.GetSafestring(rw["RegistrationNumbers"]);
            }

            if (rw.Table.Columns["WebsiteURL"] != null)
            {
                result.WebsiteURL = SafeHelper.GetSafestring(rw["WebsiteURL"]);
            }

            return result;
        }
    }
}
