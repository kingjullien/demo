using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Models.OI.CleanseMatch;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SBISCCMWeb.Controllers
{
    [Authorize, OrbLicenseEnabled, ValidateAccount, AllowLicense, TwoStepVerification]
    public class OISearchDataController : BaseController
    {
        // GET: OISearchData
        [Route("OI/Searchdata"), HttpGet]
        public ActionResult Index()
        {
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
            return View("~/Views/OI/OISearchData/Index.cshtml", model);
        }
        [Route("OI/Searchdata"), HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult Index(string CompanyName, string Address1, string Address2, string City, string State, string Country, string Zipcode, string Telephone)
        {
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            if (!string.IsNullOrEmpty(CompanyName) && !string.IsNullOrEmpty(Country))
            {
                string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
                string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
                string SubDomain = hostParts[0];
                model = APIUtility.GetOICleanseMatchResult(CompanyName, Address1, Address2, City, State, Country, Zipcode, Telephone, ConnectionString, SubDomain);
                if (model.oICleanseMatchOutputs == null && model.Error != null)
                {
                    model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                    ViewBag.ErrorMessage = model.Error.ToString();
                }
            }
            else
            {
                model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                ViewBag.ErrorMessage = DandBSettingLang.msgInvadilState;
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/OI/OISearchData/_Index.cshtml", model);
            }
            else
            {
                return PartialView("~/Views/OI/OISearchData/Index.cshtml", model);
            }
        }

        #region "Search By Alt. fields"
        // Options to Search By Alt. fields
        public ActionResult OISearchAltfields()
        {
            return View("~/Views/OI/OISearchData/OISearchAltfields.cshtml");
        }
        #region "Orb Number Search"
        // If searched by Orb Number
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public ActionResult OIAltSearchOrbNumber(string Parameters)
        {
            //OI - Search Data(Support Search by webdomain, orb number, email, ein)(MP - 577)
            string Country = string.Empty, OrbNum = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                OrbNum = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                Country = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }
            string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
            string SubDomain = hostParts[0];
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            model = APIUtility.GetOICleanseMatchResult("", "", "", "", "", Country, "", "", ConnectionString, SubDomain, null, "", OrbNum);
            if (model.oICleanseMatchOutputs == null && model.Error != null)
            {
                model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                ViewBag.ErrorMessage = model.Error.ToString();
            }
            return PartialView("~/Views/OI/OISearchData/_Index.cshtml", model);
        }
        #endregion

        #region "Add Company By ORB" 
        // Add company name on the search data records
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult FillMatchData(string orb_num, string MatchString)
        {
            ViewBag.OriginalSrcRecordId = orb_num;
            ViewBag.orb_num = "Orb-" + orb_num;

            OICleanseMatchViewModel OIMatch = new OICleanseMatchViewModel();
            if (!string.IsNullOrWhiteSpace(MatchString))
                OIMatch = SerializeHelper.DeserializeString<OICleanseMatchViewModel>(MatchString);
            ViewBag.MatchUrl = OIMatch.MatchUrl;
            ViewBag.ResponseJson = OIMatch.ResponseJson;
            ViewBag.ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            string ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/OI/OISearchData/AddCompany.cshtml", null);
            return Json(new { result = true, htmlData = ResponseString }, JsonRequestBehavior.AllowGet);
        }

        //Add company 
        [HttpPost, ValidateInput(true), RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult AddCompany(string SrcId, string orb_num, string Tag, string MatchURL, string ResponseJSON)
        {
            // On selecting Adding Company
            OICleanseMatchViewModel OIMatch = new OICleanseMatchViewModel();
            OICompanyMatchFacade company = new OICompanyMatchFacade(this.CurrentClient.ApplicationDBConnectionString);
            try
            {
                // Validate SrcId.
                company.ValidateCompanySrcId(SrcId);
                OICompanyEntity Company = new OICompanyEntity();
                if (!string.IsNullOrWhiteSpace(orb_num))
                {
                    OIMatch.MatchUrl = MatchURL;
                    OIMatch.ResponseJson = ResponseJSON;
                    Company.Tags = Tag;
                    Company.SrcRecordId = SrcId;
                    ViewBag.matchRecord = Company;
                    company.OIAddRecordAsNewCompany(MatchURL, ResponseJSON, orb_num, SrcId == null ? "123" : SrcId, Tag, Helper.oUser.UserId);
                }
            }
            catch (SqlException ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message };
            }
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        #endregion

        #region "Email Search"
        // If searched by Email address
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public ActionResult OIAltSearchEmail(string Parameters)
        {
            //OI - Search Data(Support Search by webdomain, orb number, email, ein)(MP - 577)
            string Country = string.Empty, Email = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Email = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            //Get SubDomain  
            string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
            string SubDomain = hostParts[0];
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            model = APIUtility.GetOICleanseMatchResult("", "", "", "", "", Country, "", "", ConnectionString, SubDomain, null, "", "", "", "", Email);
            if (model.oICleanseMatchOutputs == null && model.Error != null)
            {
                model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                ViewBag.ErrorMessage = model.Error.ToString();
            }
            return PartialView("~/Views/OI/OISearchData/_Index.cshtml", model);
        }
        #endregion
        #region "Domain Search"
        // If searched by Domain
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public ActionResult OIAltSearchDomain(string Parameters)
        {
            //OI - Search Data(Support Search by webdomain, orb number, email, ein)(MP - 577)
            string Country = string.Empty, Domain = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Domain = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
            string SubDomain = hostParts[0];
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            model = APIUtility.GetOICleanseMatchResult("", "", "", "", "", Country, "", "", ConnectionString, SubDomain, null, "", "", "", Domain, "");
            if (model.oICleanseMatchOutputs == null && model.Error != null)
            {
                model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                ViewBag.ErrorMessage = model.Error.ToString();
            }
            return PartialView("~/Views/OI/OISearchData/_Index.cshtml", model);
        }
        #endregion
        #region "EIN Search"
        // If searched by EIN
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public ActionResult OIAltSearchEIN(string Parameters)
        {
            //OI - Search Data(Support Search by webdomain, orb number, email, ein)(MP - 577)
            string Country = string.Empty, EIN = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                EIN = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
            }
            string ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            string[] hostParts = new System.Uri(Request.Url.AbsoluteUri).Host.Split('.');
            string SubDomain = hostParts[0];
            OICleanseMatchViewModel model = new OICleanseMatchViewModel();
            model = APIUtility.GetOICleanseMatchResult("", "", "", "", "", Country, "", "", ConnectionString, SubDomain, null, "", "", EIN, "", "");
            if (model.oICleanseMatchOutputs == null && model.Error != null)
            {
                model.oICleanseMatchOutputs = new List<OICleanseMatchOutput>();
                ViewBag.ErrorMessage = model.Error.ToString();
            }
            return PartialView("~/Views/OI/OISearchData/_Index.cshtml", model);
        }
        #endregion
        #endregion
    }
}