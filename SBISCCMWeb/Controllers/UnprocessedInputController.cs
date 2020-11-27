using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    public class UnprocessedInputController : BaseController
    {
        // GET: UnprocessedInput
        public ActionResult Index(int? pageSize, int? pageNumber, string importProcess)
        {
            pageNumber = pageNumber.HasValue ? pageNumber.Value : 1;
            pageSize = pageSize.HasValue ? pageSize.Value : 50;

            int totalCount = 0;
            UnprocessedInputFacade fac = new UnprocessedInputFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<UnprocessedInputEntity> lstUnprocesseds = fac.GetUnprocessedInputRecords(importProcess, pageSize.Value, pageNumber.Value, out totalCount);

            UnprocessedInputViewModel viewModel = new UnprocessedInputViewModel();
            viewModel.pglstUnprocesseds = new StaticPagedList<UnprocessedInputEntity>(lstUnprocesseds, pageNumber.Value, pageSize.Value, totalCount);
            viewModel.pageSize = pageSize.Value;
            viewModel.pageNumber = pageNumber.Value;
            viewModel.importProcess = importProcess;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_index", viewModel);
            }
            return View(viewModel);
        }
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteUnprocessedInputRecords(string parameters)
        {
            try
            {
                string importProcess = string.Empty;
                if (!string.IsNullOrEmpty(parameters))
                {
                    importProcess = StringCipher.Decrypt(parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase).Replace("&#58&#58", "::");
                }
                UnprocessedInputFacade fac = new UnprocessedInputFacade(this.CurrentClient.ApplicationDBConnectionString);
                bool result = fac.DeleteUnprocessedInputRecords(importProcess);
                if (result)
                    return Json(new { result = result, message = DandBSettingLang.msgCommonDeleteMessage }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = result, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult MoveUnprocessedInputRecordsToBID(string parameters)
        {
            try
            {
                string importProcess = string.Empty;
                if (!string.IsNullOrEmpty(parameters))
                {
                    importProcess = StringCipher.Decrypt(parameters.Replace(Utility.Utility.urlseparator, "+").Replace("&#58&#58", "::"), General.passPhrase);
                }
                UnprocessedInputFacade fac = new UnprocessedInputFacade(this.CurrentClient.ApplicationDBConnectionString);
                bool result = fac.MoveUnprocessedInputRecordsToBID(parameters);
                if (result)
                    return Json(new { result = result, message = DandBSettingLang.msgCommonMovedMessage }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = result, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ExportUnprocessedRecords()
        {
            int totalCount = 0;
            UnprocessedInputFacade fac = new UnprocessedInputFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<UnprocessedInputEntity> lstUnprocesseds = fac.GetUnprocessedInputRecords("", 50, -1, out totalCount);
            DataTable dtUnprocesseds = CommonMethod.ToDataTable(lstUnprocesseds);

            string fileName = "Input Records_" + DateTime.Now.Ticks.ToString() + ".xlsx";
            string SheetName = "Input Records";
            byte[] response = CommonExportMethods.ExportExcelFile(dtUnprocesseds, fileName, SheetName);
            return File(response, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}