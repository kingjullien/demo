using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class BaseModel
    {
        public ClientApplicationData CurrentClient { get; set; }
    }
    public class ClientApplicationData
    {
        public string ApplicationDBConnectionStringHash { get; set; }
        public string ClientName { get; set; }
        public string ClientLogo { get; set; }
        public int ApplicationId { get; set; }
        public string ApplicationDBConnectionString { get; set; }

    }
    #region "Report Code Not Use"
    //Power BI Report not used
    //public class PowerBIClientApplicationReports
    //{
    //    public int ApplicationId { get; set; }
    //    public string PowerBICollectionName { get; set; }
    //    public string PowerBIAccessKey { get; set; }
    //    public string PowerBIWorkspaceId { get; set; }

    //    public List<ClientApplicationReports> ClientReports { get; set;}
    //}
    //public class ClientApplicationReports
    //{
    //    public string ReportType { get; set; }
    //    public string ReportId { get; set; }
    //}
    #endregion
}