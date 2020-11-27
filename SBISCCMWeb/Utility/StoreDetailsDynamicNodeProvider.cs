using MvcSiteMapProvider;
using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchBusiness.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    #region "Report Code Not Use"
    //Power BI Report not used
    //public class StoreDetailsDynamicNodeProvider : DynamicNodeProviderBase
    //{
    //    public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)// Overide method to add dynamic Menu in site map through database
    //    {
    //        PowerBIClientApplicationReports oReport = CacheHelper.GetPowerBIApplicationAccess(Helper.hostName);
    //        var returnValue = new List<DynamicNode>();
    //        if (oReport != null && oReport.ClientReports != null && oReport.ClientReports.Any())
    //        {
    //            DynamicNode Tnode = new DynamicNode();
    //            foreach (var item in oReport.ClientReports)
    //            {
    //                Tnode = new DynamicNode();
    //                Tnode.Title = item.ReportType;
    //                Tnode.Action = "BaseReport";
    //                Tnode.Controller = "ReportsList";
    //                string ReportType = item.ReportType != null ? StringCipher.Encrypt(item.ReportType, General.passPhrase) : StringCipher.Encrypt("", General.passPhrase);
    //                Tnode.RouteValues.Add("ReportType", ReportType.Replace("+", Utility.urlseparator));
    //                returnValue.Add(Tnode);
    //            }
    //        }

    //        return returnValue;
    //    }
    //}
    #endregion
}