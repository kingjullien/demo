using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters.OI;
using SBISCompanyCleanseMatchFacade.Objects;
using SBISCompanyCleanseMatchFacade.Objects.OI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers.OI
{
    [Authorize]
    // DB Changes (MP-716)
    public class OIUserSessionFilterController : BaseController
    {
        OIUserSessionFilterEntity model = new OIUserSessionFilterEntity();
        // GET: UserSessionFilter
        public ActionResult Index()
        {
            return View();
        }

        #region "Session Filter"
        [HttpGet]
        public ActionResult popupUserFilter(string UserId)
        {
            // Open Session filter popup and set data to Popup
            OIUserSessionFacade fac = new OIUserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetUserSessionFilterByUserId(Helper.oUser.UserId);
            if (model.UserId > 0)
            {
                model.OrderByColumn = model.OrderByColumn == null ? "SrcRecordId" : model.OrderByColumn;
            }
            else
            {
                model = new OIUserSessionFilterEntity();
                model.OrderByColumn = "SrcRecordId";
            }
            return PartialView("~/Views/OI/OIUserSessionFilter/_popupUserFilter.cshtml", model);
        }
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult popupUserFilter(OIUserSessionFilterEntity model)
        {
            // Save data to database for Session Filter.
            OIUserSessionFacade fac = new OIUserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);

            int id = model.UserId;
            model.UserId = Helper.oUser.UserId;
            fac.InsertOrUpdateUserSessionFilter(model);
            return Json(new { result = true, message = id > 0 ? CommonMessagesLang.msgCommanUpdateMessage : CommonMessagesLang.msgCommanInsertMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Get Tags and Tags Type Code
        public static List<TagsEntity> GetAllTags(string ConnectionString)
        {
            // Get All tags from the database and fill the dropdown 
            List<TagsEntity> model = new List<TagsEntity>();
            TagFacade fac = new TagFacade(ConnectionString);
            model = fac.GetAllTags(Helper.oUser.LOBTag);
            return model;
        }



        public static SelectList GetTagTypeCode(string ConnectionString)
        {
            // Get All tags type code from the database and fill the dropdown 
            List<SelectListItem> lstAllFilter = new List<SelectListItem>();
            TagFacade fac = new TagFacade(ConnectionString);
            DataTable dt = fac.GetTagTypeCode();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAllFilter.Add(new SelectListItem { Value = dt.Rows[i]["TagTypeCode"].ToString() + "@#$" + dt.Rows[i]["Value"].ToString(), Text = dt.Rows[i]["Description"].ToString() });
            }

            return new SelectList(lstAllFilter, "Value", "Text");
        }

        #endregion
    }
}