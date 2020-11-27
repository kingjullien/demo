using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Web.Mvc;


namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DandBController : BaseController
    {
        // GET: DandB
        public ActionResult Index()
        {
            SessionHelper.lstTempMonirtoring = string.Empty;
            SessionHelper.lstMonirtoringTemp = string.Empty;
            SessionHelper.objMTM = string.Empty;
            return View();
        }

        #region "Reset System Data"
        [HttpGet]
        public ActionResult ResetSystemData(bool isResetConfig)
        {
            ViewBag.isResetConfig = isResetConfig;
            return View();
        }
        public JsonResult ResetAllSystemData(bool isResetConfig)
        {
            try
            {
                SettingFacade fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
                fac.ResetSystemData();
                return new JsonResult { Data = DandBSettingLang.msgResetDataSuccessfully };
            }
            catch (Exception)
            {
                return new JsonResult { Data = DandBSettingLang.msgResetDataUnsuccessfully };
            }
        }
        #endregion

        [HttpPost]
        // Clears the session value - Set session value null
        public ActionResult UpdateSessionValue()
        {
            Session["Message"] = null;
            return Json(new { status = "success" });
        }



    }
}
