using Microsoft.AspNet.Identity;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SBISCCMWeb.Controllers
{
    public class TagsController : BaseController
    {
        // GET: Tags
        #region Tags
        #region Add Tags and Validate Tags
        [HttpGet]
        public ActionResult AddTags(bool isAllowLOBTag = true)
        {
            if (!Helper.LicenseEnableTags)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.isAllowLOBTag = isAllowLOBTag;
            return View();
        }

        [HttpPost, RequestFromSameDomain]
        public JsonResult AddTags(TagsEntity objTags)
        {
            string Parameters = string.Empty;
            string strOption = "";
            if (objTags.TagId > 0 || (!string.IsNullOrEmpty(objTags.Tag?.Trim()) && !string.IsNullOrEmpty(objTags.TagTypeCode?.Trim())))
            {
                if (objTags.TagId > 0 || CommonMethod.isValidTagName(objTags.Tag) == true)
                {
                    TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
                    if (objTags.TagId > 0)
                    {
                        TagsEntity newObjTags = fac.GetTagByTagId(objTags.TagId);
                        newObjTags.LOBTag = objTags.LOBTag;
                        fac.UpdateTags(newObjTags);
                        return Json(new { result = true, message = CommonMessagesLang.msgCommanUpdateMessage, tagValue = objTags.Tag }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        string[] separatingStrings = { "@#$" };
                        string[] tagCode = objTags.TagTypeCode.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                        objTags.TagValue = objTags.Tag;
                        objTags.Tag = "[" + tagCode[1] + "::" + objTags.Tag + "]";
                        objTags.TagTypeCode = tagCode[0];
                        objTags.CreatedUserId = Convert.ToInt32(User.Identity.GetUserId());

                        if (!IsTasExists(objTags.Tag))// Validate tag is already exists or not
                        {
                            // Insert Tag into database.
                            fac.InsertTags(objTags, Helper.oUser.UserId);
                            return Json(new { result = true, message = CommonMessagesLang.msgCommanInsertMessage, tagValue = objTags.Tag }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { result = false, message = TagLang.msgTagAlreadyExists, tagValue = objTags.Tag }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgValidCharacters, tagValue = "" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage, tagValue = "" }, JsonRequestBehavior.AllowGet);
        }
        public bool IsTasExists(string tagValue)
        {
            // Validate tag is already exists or not
            bool IsExists = false;
            TagFacade fac = new TagFacade(this.CurrentClient.ApplicationDBConnectionString);
            int TagId = fac.GetAllTags("").Where(x => x.Tag.ToLower() == tagValue.ToLower()).Select(x => x.TagId).FirstOrDefault();
            if (TagId > 0)
            {
                IsExists = true;
            }
            return IsExists;
        }
        #endregion
        #endregion
    }
}