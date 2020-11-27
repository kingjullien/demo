using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class PreviewAutoAcceptanceRuleAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        public List<PreviewAutoAcceptanceRuleEntity> Adapt(DataTable dt)
        {
            List<PreviewAutoAcceptanceRuleEntity> results = new List<PreviewAutoAcceptanceRuleEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                PreviewAutoAcceptanceRuleEntity productDataEntity = new PreviewAutoAcceptanceRuleEntity();
                productDataEntity = AdaptItem(rw, dt);
                results.Add(productDataEntity);
            }
            return results;
        }
        public PreviewAutoAcceptanceRuleEntity AdaptItem(DataRow rw, DataTable dt)
        {
            PreviewAutoAcceptanceRuleEntity result = new PreviewAutoAcceptanceRuleEntity();
            if (dt.Columns.Contains("RowId"))
            {
                result.RowId = SafeHelper.GetSafeint(rw["RowId"]);
            }

            if (dt.Columns.Contains("InputId"))
            {
                result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            }

            if (dt.Columns.Contains("SrcRecordId"))
            {
                result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            }

            if (dt.Columns.Contains("CompanyName"))
            {
                result.CompanyName = SafeHelper.GetSafestring(rw["CompanyName"]);
            }

            if (dt.Columns.Contains("Address"))
            {
                result.Address = SafeHelper.GetSafestring(rw["Address"]);
            }

            if (dt.Columns.Contains("City"))
            {
                result.City = SafeHelper.GetSafestring(rw["City"]);
            }

            if (dt.Columns.Contains("State"))
            {
                result.State = SafeHelper.GetSafestring(rw["State"]);
            }

            if (dt.Columns.Contains("PostalCode"))
            {
                result.PostalCode = SafeHelper.GetSafestring(rw["PostalCode"]);
            }

            if (dt.Columns.Contains("CountryISOAlpha2Code"))
            {
                result.CountryISOAlpha2Code = SafeHelper.GetSafestring(rw["CountryISOAlpha2Code"]);
            }

            if (dt.Columns.Contains("PhoneNbr"))
            {
                result.PhoneNbr = SafeHelper.GetSafestring(rw["PhoneNbr"]);
            }

            if (dt.Columns.Contains("TransactionTimestamp"))
            {
                result.TransactionTimestamp = SafeHelper.GetSafeDateTime(rw["TransactionTimestamp"]);
            }

            if (dt.Columns.Contains("DnBDUNSNumber"))
            {
                result.DnBDUNSNumber = SafeHelper.GetSafestring(rw["DnBDUNSNumber"]);
            }

            if (dt.Columns.Contains("DnBOrganizationName"))
            {
                result.DnBOrganizationName = SafeHelper.GetSafestring(rw["DnBOrganizationName"]);
            }

            if (dt.Columns.Contains("DnBTradeStyleName"))
            {
                result.DnBTradeStyleName = SafeHelper.GetSafestring(rw["DnBTradeStyleName"]);
            }

            if (dt.Columns.Contains("DnBSeniorPrincipalName"))
            {
                result.DnBSeniorPrincipalName = SafeHelper.GetSafestring(rw["DnBSeniorPrincipalName"]);
            }

            if (dt.Columns.Contains("DnBStreetAddressLine"))
            {
                result.DnBStreetAddressLine = SafeHelper.GetSafestring(rw["DnBStreetAddressLine"]);
            }

            if (dt.Columns.Contains("DnBPrimaryTownName"))
            {
                result.DnBPrimaryTownName = SafeHelper.GetSafestring(rw["DnBPrimaryTownName"]);
            }

            if (dt.Columns.Contains("DnBCountryISOAlpha2Code"))
            {
                result.DnBCountryISOAlpha2Code = SafeHelper.GetSafestring(rw["DnBCountryISOAlpha2Code"]);
            }

            if (dt.Columns.Contains("DnBPostalCode"))
            {
                result.DnBPostalCode = SafeHelper.GetSafestring(rw["DnBPostalCode"]);
            }

            if (dt.Columns.Contains("DnBTelephoneNumber"))
            {
                result.DnBTelephoneNumber = SafeHelper.GetSafestring(rw["DnBTelephoneNumber"]);
            }

            if (dt.Columns.Contains("DnBOperatingStatus"))
            {
                result.DnBOperatingStatus = SafeHelper.GetSafestring(rw["DnBOperatingStatus"]);
            }

            if (dt.Columns.Contains("DnBFamilyTreeMemberRole"))
            {
                result.DnBFamilyTreeMemberRole = SafeHelper.GetSafestring(rw["DnBFamilyTreeMemberRole"]);
            }

            if (dt.Columns.Contains("DnBStandaloneOrganization"))
            {
                result.DnBStandaloneOrganization = SafeHelper.GetSafebool(rw["DnBStandaloneOrganization"]);
            }

            if (dt.Columns.Contains("DnBConfidenceCode"))
            {
                result.DnBConfidenceCode = SafeHelper.GetSafeint(rw["DnBConfidenceCode"]);
            }

            if (dt.Columns.Contains("DnBMatchGradeText"))
            {
                result.DnBMatchGradeText = SafeHelper.GetSafestring(rw["DnBMatchGradeText"]);
            }

            if (dt.Columns.Contains("DnBMatchDataProfileText"))
            {
                result.DnBMatchDataProfileText = SafeHelper.GetSafestring(rw["DnBMatchDataProfileText"]);
            }

            if (dt.Columns.Contains("DnBMatchDataProfileComponentCount"))
            {
                result.DnBMatchDataProfileComponentCount = SafeHelper.GetSafeint(rw["DnBMatchDataProfileComponentCount"]);
            }

            if (dt.Columns.Contains("DnBDisplaySequence"))
            {
                result.DnBDisplaySequence = SafeHelper.GetSafeint(rw["DnBDisplaySequence"]);
            }

            if (dt.Columns.Contains("ExcludedCandidate"))
            {
                result.ExcludedCandidate = SafeHelper.GetSafestring(rw["ExcludedCandidate"]);
            }

            return result;
        }
    }
}
