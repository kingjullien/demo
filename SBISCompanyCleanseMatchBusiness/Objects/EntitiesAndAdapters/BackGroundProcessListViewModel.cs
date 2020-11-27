using System.Collections.Generic;

namespace SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters
{
    public class BackGroundProcessListViewModel
    {
        public List<DashboardBackgroundProcessStatsEntity> StatsList { get; set; }
        public List<DashboardBackgroundProcessEntity> ProcessList { get; set; }
    }
}
