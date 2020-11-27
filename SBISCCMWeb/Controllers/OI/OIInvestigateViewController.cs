using Microsoft.AspNet.Identity;
using PagedList;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers.OI
{
    [Authorize(Roles = "ADMINISTRATOR"), TwoStepVerification, AllowLicense, ValidateInput(true), OrbLicenseEnabled]
    public class OIInvestigateViewController : BaseController
    {
        // GET: OIInvestigateView
        //public ActionResult Index()
        //{
        //    return View();
        //}

        #region "Investigation Report"
        // Open Investigation report and display the content
        [Route("OIInvestigateView/InvestigationReport/{page?}/{sortby?}/{sortorder?}/{pagevalue?}")]
        public ActionResult InvestigationReport(int? page, int? sortby, int? sortorder, int? pagevalue, bool Filter = false, string SearchText = null, string strType = null)
        {
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            int UserId = Convert.ToInt32(User.Identity.GetUserId());

            InvestigateFacade fac = new InvestigateFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<InvestigateViewEntity> lstInvestigation = new List<InvestigateViewEntity>();
            lstInvestigation = fac.GetCompanyInvestigationPaging(sortParam, currentPageIndex, pageSize, out totalCount);

            IPagedList<InvestigateViewEntity> pagedInvestigation = new StaticPagedList<InvestigateViewEntity>(lstInvestigation.ToList(), currentPageIndex, pageSize, totalCount);
            if (Request.IsAjaxRequest())
                return PartialView("~/Views/OI/OIInvestigateView/_InvestigationReport.cshtml", pagedInvestigation);
            else
                return View("~/Views/OI/OIInvestigateView/InvestigationReport.cshtml", pagedInvestigation);
        }
        #endregion

    }
}