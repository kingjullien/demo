using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    class AcceptanceCriteriaAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();

        #region "OLD CODE"
        //public List<AdditionalAcceptanceEntity> Adapt(DataTable dt)
        //{
        //    List<AdditionalAcceptanceEntity> results = new List<AdditionalAcceptanceEntity>();
        //    foreach (DataRow rw in dt.Rows)
        //    {
        //        AdditionalAcceptanceEntity cust = new AdditionalAcceptanceEntity();
        //        cust = AdaptItem(rw);
        //        results.Add(cust);
        //    }
        //    return results;
        //}

        //public AdditionalAcceptanceEntity AdaptItem(DataRow rw)
        //{
        //    AdditionalAcceptanceEntity result = new AdditionalAcceptanceEntity();
        //    result.SettingId = SafeHelper.GetSafeint(rw["SecondaryAutoAcceptanceSettings"]);
        //    result.ConfidenceCode = SafeHelper.GetSafeint(rw["ConfidenceCode"]);
        //    result.MatchGrade = SafeHelper.GetSafestring(rw["MatchGrade"]);
        //    result.CompanyName = SafeHelper.GetSafestring(rw["MDPCompany"]);
        //    result.Street = SafeHelper.GetSafestring(rw["MDPStreetNo"]);
        //    result.StreetName = SafeHelper.GetSafestring(rw["MDPStreetName"]);
        //    result.City = SafeHelper.GetSafestring(rw["MDPCity"]);
        //    result.State = SafeHelper.GetSafestring(rw["MDPState"]);             
        //    result.SequenceNo = SafeHelper.GetSafeint(rw["SequenceNo"]);
        //    return result;
        //}

        #endregion


        public List<AutoAdditionalAcceptanceCriteriaEntity> Adapt(DataTable dt)
        {
            List<AutoAdditionalAcceptanceCriteriaEntity> results = new List<AutoAdditionalAcceptanceCriteriaEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                AutoAdditionalAcceptanceCriteriaEntity cust = new AutoAdditionalAcceptanceCriteriaEntity();
                cust = AdaptItem(rw);
                results.Add(cust);
            }
            return results;
        }

        public AutoAdditionalAcceptanceCriteriaEntity AdaptItem(DataRow rw)
        {
            AutoAdditionalAcceptanceCriteriaEntity result = new AutoAdditionalAcceptanceCriteriaEntity();
            //result.CriteriaId = SafeHelper.GetSafeint(rw["CriteriaId"]);
            //result.ConfidenceCode = SafeHelper.GetSafestring(rw["ConfidenceCode"]);
            //result.MatchGrade = SafeHelper.GetSafestring(rw["MatchGrade"]);
            //result.MDPCode = SafeHelper.GetSafestring(rw["MDPCode"]);
            //result.SequenceNo = SafeHelper.GetSafeint(rw["SequenceNo"]);
            //result.GroupId = SafeHelper.GetSafeint(rw["GroupId"]);
            //result.GroupName = SafeHelper.GetSafestring(rw["GroupName"]);
            //result.ExcludeFromAutoAccept = SafeHelper.GetSafebool(rw["ExcludeFromAutoAccept"]);
            //result.Tag = SafeHelper.GetSafestring(rw["Tags"]);

            result.CriteriaGroupId = SafeHelper.GetSafeint(rw["CriteriaGroupId"]);
            result.ConfidenceCode = SafeHelper.GetSafestring(rw["ConfidenceCodes"]);

            result.MG_Combined = SafeHelper.GetSafestring(rw["MG_Combined"]);
            result.MDP_Combined = SafeHelper.GetSafestring(rw["MDP_Combined"]);

            result.CompanyGrade = SafeHelper.GetSafestring(rw["MG_Company"]);
            result.StreetGrade = SafeHelper.GetSafestring(rw["MG_StreetNo"]);
            result.StreetNameGrade = SafeHelper.GetSafestring(rw["MG_StreetName"]);
            result.CityGrade = SafeHelper.GetSafestring(rw["MG_City"]);
            result.StateGrade = SafeHelper.GetSafestring(rw["MG_State"]);
            result.AddressGrade = SafeHelper.GetSafestring(rw["MG_POBox"]);
            result.PhoneGrade = SafeHelper.GetSafestring(rw["MG_Phone"]);
            result.ZipGrade = SafeHelper.GetSafestring(rw["MG_PostalCode"]);
            result.Density = SafeHelper.GetSafestring(rw["MG_Density"]);
            result.Uniqueness = SafeHelper.GetSafestring(rw["MG_Uniqueness"]);
            result.SIC = SafeHelper.GetSafestring(rw["MG_SIC"]);

            result.CompanyCode = SafeHelper.GetSafestring(rw["MDP_Company"]);
            result.StreetCode = SafeHelper.GetSafestring(rw["MDP_StreetNo"]);
            result.StreetNameCode = SafeHelper.GetSafestring(rw["MDP_StreetName"]);
            result.CityCode = SafeHelper.GetSafestring(rw["MDP_City"]);
            result.StateCode = SafeHelper.GetSafestring(rw["MDP_State"]);
            result.AddressCode = SafeHelper.GetSafestring(rw["MDP_POBox"]);
            result.PhoneCode = SafeHelper.GetSafestring(rw["MDP_Phone"]);
            result.ZipCode = SafeHelper.GetSafestring(rw["MDP_PostalCode"]);
            result.DensityCode = SafeHelper.GetSafestring(rw["MDP_Density"]);
            result.UniquenessCode = SafeHelper.GetSafestring(rw["MDP_Uniqueness"]);
            result.SICCode = SafeHelper.GetSafestring(rw["MDP_SIC"]);
            result.DUNSCode = SafeHelper.GetSafestring(rw["MDP_DUNS"]);
            result.NationalIDCode = SafeHelper.GetSafestring(rw["MDP_NationalID"]);
            result.URLCode = SafeHelper.GetSafestring(rw["MDP_URL"]);

            result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            result.ExcludeFromAutoAccept = SafeHelper.GetSafebool(rw["ExcludeFromAutoAccept"]);
            result.GroupId = SafeHelper.GetSafeint(rw["CountryGroupId"]);
            result.GroupName = SafeHelper.GetSafestring(rw["CountryGroupName"]);
            if (rw.Table.Columns["UserId"] != null)
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (rw.Table.Columns["UserName"] != null)
            {
                result.UserName = SafeHelper.GetSafestring(rw["UserName"]);
            }

            if (rw.Table.Columns["MatchGradeComponentCount"] != null)
            {
                result.MatchGradeComponentCount = SafeHelper.GetSafeint(rw["MatchGradeComponentCount"]);
            }

            if (rw.Table.Columns["CompanyScore"] != null)
            {
                result.CompanyScore = SafeHelper.GetSafeint(rw["CompanyScore"]);
            }
            result.MatchDataCriteria = SafeHelper.GetSafestring(rw["MatchDataCriteria"]);
            result.OperatingStatus = SafeHelper.GetSafestring(rw["OperatingStatus"]);
            result.BusinessType = SafeHelper.GetSafestring(rw["BusinessType"]);
            result.SingleCandidateMatchOnly = SafeHelper.GetSafebool(rw["SingleCandidateMatchOnly"]);

            return result;
        }
    }


    class AcceptanceCriteriaAdapterDetails
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<AutoAcceptanceCriteriaDetail> Adapt(DataTable dt)
        {
            List<AutoAcceptanceCriteriaDetail> results = new List<AutoAcceptanceCriteriaDetail>();
            foreach (DataRow rw in dt.Rows)
            {
                AutoAcceptanceCriteriaDetail cust = new AutoAcceptanceCriteriaDetail();
                cust = AdaptItemCriteriaDetail(rw);
                results.Add(cust);
            }
            return results;
        }
        public AutoAcceptanceCriteriaDetail AdaptItemCriteriaDetail(DataRow rw)
        {
            AutoAcceptanceCriteriaDetail result = new AutoAcceptanceCriteriaDetail();
            result.CriteriaId = SafeHelper.GetSafeint(rw["CriteriaId"]);
            result.ConfidenceCode = SafeHelper.GetSafeint(rw["ConfidenceCode"]);
            result.MatchGrade = SafeHelper.GetSafestring(rw["MatchGrade"]);
            result.MDPCode = SafeHelper.GetSafestring(rw["MDPCode"]);
            result.Tag = SafeHelper.GetSafestring(rw["Tag"]);
            result.CriteriaGroupId = SafeHelper.GetSafeint(rw["CriteriaGroupId"]);


            return result;
        }
    }
}
