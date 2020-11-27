using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class IResearchInvestigationViewModel
    {
        public List<DashboardV2GetInvestigationStatistics> InvestigationStats { get; set; }
        public List<IResearchInvestigationEntity> lstResearchInvestigation { get; set; }
    }
}