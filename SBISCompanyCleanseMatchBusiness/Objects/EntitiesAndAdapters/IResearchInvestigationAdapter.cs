using Newtonsoft.Json;
using SBISCompanyCleanseMatchBusiness.Objects.Helpers;
using System.Collections.Generic;
using System.Data;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class IResearchInvestigationAdapter
    {
        private DatatypeHelpers SafeHelper = new DatatypeHelpers();
        public List<IResearchInvestigationEntity> Adapt(DataTable dt)
        {
            List<IResearchInvestigationEntity> results = new List<IResearchInvestigationEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                IResearchInvestigationEntity matchCode = new IResearchInvestigationEntity();
                matchCode = AdaptItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public List<iResearchEntityTargetedEntity> AdaptItems(DataTable dt)
        {
            List<iResearchEntityTargetedEntity> results = new List<iResearchEntityTargetedEntity>();
            foreach (DataRow rw in dt.Rows)
            {
                iResearchEntityTargetedEntity cust = new iResearchEntityTargetedEntity();
                cust = AdaptItemResearchSubTypes(rw);
                results.Add(cust);
            }
            return results;
        }
        public IResearchInvestigationEntity AdaptItem(DataRow rw, DataTable dt)
        {
            IResearchInvestigationEntity result = new IResearchInvestigationEntity();

            if (dt.Columns.Contains("ResearchRequestId"))
            {
                result.ResearchRequestId = SafeHelper.GetSafeint(rw["ResearchRequestId"]);
            }

            if (dt.Columns.Contains("RequestType"))
            {
                result.RequestType = SafeHelper.GetSafestring(rw["RequestType"]);
            }

            if (dt.Columns.Contains("UserId"))
            {
                result.UserId = SafeHelper.GetSafeint(rw["UserId"]);
            }

            if (dt.Columns.Contains("InputId"))
            {
                result.InputId = SafeHelper.GetSafeint(rw["InputId"]);
            }

            if (dt.Columns.Contains("CaseId"))
            {
                result.CaseId = SafeHelper.GetSafeint(rw["CaseId"]);
            }

            if (dt.Columns.Contains("SrcRecordId"))
            {
                result.SrcRecordId = SafeHelper.GetSafestring(rw["SrcRecordId"]);
            }

            if (dt.Columns.Contains("Tags"))
            {
                result.Tags = SafeHelper.GetSafestring(rw["Tags"]);
            }

            if (dt.Columns.Contains("RequestBody"))
            {
                result.RequestBody = SafeHelper.GetSafestring(rw["RequestBody"]);
                result.RequestBodylst = JsonConvert.DeserializeObject<ResearchInvestigationRequestEntity>(result.RequestBody);
            }
            if (dt.Columns.Contains("RequestDateTime"))
            {
                result.RequestDateTime = SafeHelper.GetSafeDateTime(rw["RequestDateTime"]);
            }

            if (dt.Columns.Contains("LastestStatusDateTime"))
            {
                result.LastestStatusDateTime = SafeHelper.GetSafeDateTime(rw["LastestStatusDateTime"]);
            }

            if (dt.Columns.Contains("RequestResponseJSON"))
            {
                result.RequestResponseJSON = SafeHelper.GetSafestring(rw["RequestResponseJSON"]);
                result.RequestResponseJSONlst = JsonConvert.DeserializeObject<ResearchInvestigationResponseEntity>(result.RequestResponseJSON);
            }
            if (dt.Columns.Contains("CaseStatus"))
            {
                result.CaseStatus = SafeHelper.GetSafestring(rw["CaseStatus"]);
            }

            if (dt.Columns.Contains("ResolutionDUNS"))
            {
                result.ResolutionDUNS = SafeHelper.GetSafestring(rw["ResolutionDUNS"]);
            }

            if (dt.Columns.Contains("CaseResolution"))
            {
                result.CaseResolution = SafeHelper.GetSafestring(rw["CaseResolution"]);
            }

            if (dt.Columns.Contains("CaseSubResolution"))
            {
                result.CaseSubResolution = SafeHelper.GetSafestring(rw["CaseSubResolution"]);
            }

            return result;
        }
        public iResearchEntityTargetedEntity AdaptItemResearchSubTypes(DataRow rw)
        {
            iResearchEntityTargetedEntity result = new iResearchEntityTargetedEntity();

            if (rw.Table.Columns["categoryID"] != null)
            {
                result.categoryID = SafeHelper.GetSafeint(rw["categoryID"]);
            }

            if (rw.Table.Columns["categoryName"] != null)
            {
                result.categoryName = SafeHelper.GetSafestring(rw["categoryName"]);
            }

            if (rw.Table.Columns["code"] != null)
            {
                result.code = SafeHelper.GetSafestring(rw["code"]);
            }

            if (rw.Table.Columns["description"] != null)
            {
                result.description = SafeHelper.GetSafestring(rw["description"]);
            }

            return result;
        }

        public List<DashboardV2GetInvestigationStatistics> AdaptInvestigationStatistics(DataTable dt)
        {
            List<DashboardV2GetInvestigationStatistics> results = new List<DashboardV2GetInvestigationStatistics>();
            foreach (DataRow rw in dt.Rows)
            {
                DashboardV2GetInvestigationStatistics matchCode = new DashboardV2GetInvestigationStatistics();
                matchCode = AdaptInvestigationStatisticsItem(rw, dt);
                results.Add(matchCode);
            }
            return results;
        }

        public DashboardV2GetInvestigationStatistics AdaptInvestigationStatisticsItem(DataRow rw, DataTable dt)
        {
            DashboardV2GetInvestigationStatistics result = new DashboardV2GetInvestigationStatistics();
            if (dt.Columns.Contains("RequestType"))
            {
                result.RequestType = SafeHelper.GetSafestring(rw["RequestType"]);
            }

            if (dt.Columns.Contains("NbrOpenedCases"))
            {
                result.NbrOpenedCases = SafeHelper.GetSafeint(rw["NbrOpenedCases"]);
            }

            if (dt.Columns.Contains("NbrClosedCases"))
            {
                result.NbrClosedCases = SafeHelper.GetSafeint(rw["NbrClosedCases"]);
            }

            if (dt.Columns.Contains("NbrFailedCases"))
            {
                result.NbrFailedCases = SafeHelper.GetSafeint(rw["NbrFailedCases"]);
            }

            if (dt.Columns.Contains("NbrCaseOpenedLastWeek"))
            {
                result.NbrCaseOpenedLastWeek = SafeHelper.GetSafeint(rw["NbrCaseOpenedLastWeek"]);
            }

            if (dt.Columns.Contains("AverageResolutionTime_Minutes"))
            {
                result.AverageResolutionTime_Minutes = SafeHelper.GetSafeint(rw["AverageResolutionTime_Minutes"]);
            }

            return result;
        }
    }
}
