using System;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class IResearchInvestigationEntity
    {
        public int ResearchRequestId { get; set; }
        public string RequestType { get; set; }
        public int UserId { get; set; }
        public int CaseId { get; set; }
        public int InputId { get; set; }
        public string SrcRecordId { get; set; }
        public string Tags { get; set; }
        public string RequestBody { get; set; }
        public DateTime RequestDateTime { get; set; }
        public DateTime LastestStatusDateTime { get; set; }
        public string RequestResponseJSON { get; set; }
        public string CaseStatus { get; set; }
        public string ResolutionDUNS { get; set; }
        public string CaseResolution { get; set; }
        public string CaseSubResolution { get; set; }
        public ResearchInvestigationRequestEntity RequestBodylst { get; set; }
        public ResearchInvestigationResponseEntity RequestResponseJSONlst { get; set; }
    }

    public class DashboardV2GetInvestigationStatistics
    {
        public string RequestType { get; set; }
        public int NbrOpenedCases { get; set; }
        public int NbrClosedCases { get; set; }
        public int NbrFailedCases { get; set; }
        public int NbrCaseOpenedLastWeek { get; set; }
        public int AverageResolutionTime_Minutes { get; set; }
    }
}
