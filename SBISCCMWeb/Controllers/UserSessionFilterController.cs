using Microsoft.AspNet.Identity;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SBISCCMWeb.Controllers
{
    [Authorize]
    public class UserSessionFilterController : BaseController
    {
        UserSessionFilterEntity model = new UserSessionFilterEntity();
        // GET: UserSessionFilter
        public ActionResult Index()
        {
            TempData.Keep();
            return View();

        }

        #region "Session Filter"
        [HttpGet]
        public ActionResult popupUserFilter(string UserId)
        {
            // Open Session filter popup and set data to Popup
            UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetUserSessionFilterByUserId(Helper.oUser.UserId);
            if (model.UserId > 0)
            {
                model.OrderByColumn = model.OrderByColumn == null ? "SrcRecordId" : model.OrderByColumn;
            }
            else
            {
                model = new UserSessionFilterEntity();
                model.OrderByColumn = "SrcRecordId";
            }
            TempData.Keep();
            return PartialView("_popupUserFilter", model);
        }

        [HttpGet]
        public ActionResult popupUserFilterJson(string UserId)
        {
            UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
            model = fac.GetUserSessionFilterByUserId(Helper.oUser.UserId);
            if (model.UserId > 0)
            {
                model.OrderByColumn = model.OrderByColumn == null ? "SrcRecordId" : model.OrderByColumn;
            }
            else
            {
                model = new UserSessionFilterEntity();
                model.OrderByColumn = "SrcRecordId";
            }
            TempData.Keep();
            return Json(new { Data = model }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult popupUserFilter(UserSessionFilterEntity model)
        {
            // Save data to database for Session Filter.
            UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);

            int id = model.UserId;
            model.UserId = Helper.oUser.UserId;
            fac.InsertOrUpdateUserSessionFilter(model);
            ViewBag.Message = id > 0 ? "Data created successfully." : "Data updated successfully.";
            ViewBag.CloseAlert = "<script type='text/javascript'>$(document).ready(function(){parent.backToparent2();});</script>";
            TempData.Keep();
            return PartialView("_popupUserFilter", model);
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

        public ActionResult SaveImportFiler(string ImportProcess, string From)
        {
            if (!string.IsNullOrEmpty(ImportProcess))
            {
                ImportProcess = StringCipher.Decrypt(ImportProcess, General.passPhrase);
                UserSessionFacade fac = new UserSessionFacade(this.CurrentClient.ApplicationDBConnectionString);
                model = fac.GetUserSessionFilterByUserId(Helper.oUser.UserId);
                if (model.ImportProcess != ImportProcess)
                {
                    model.ImportProcess = ImportProcess;
                    fac.InsertOrUpdateUserSessionFilter(model);
                }
            }
            if (From == "MatchData")
            {
                return RedirectToAction("Index", "StewardshipPortal");
            }
            else
            {
                return RedirectToAction("Index", "BadInputData");
            }
        }
    }


}
