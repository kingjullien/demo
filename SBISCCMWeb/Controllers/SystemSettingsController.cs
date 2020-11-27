using Microsoft.AspNet.Identity;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    //[Authorize(Roles = "ADMINISTRATOR")]
    [Authorize(Roles = "NOACCESS"), TwoStepVerification, DandBLicenseEnabled]
    public class SystemSettingsController : BaseController
    {
        // GET: SystemSettings
        SettingFacade fac;
        public SystemSettingsController()
        {
            fac = new SettingFacade(this.CurrentClient.ApplicationDBConnectionString);
        }
        public ActionResult Index()
        {
            // To fill the System setting to view.
            SystemSettingsModel model = new SystemSettingsModel();
            LoadCleanseMatchSettings(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult Index(SystemSettingsModel model, string hdnDataSecuritySettings, string btnSave, bool isCustomSettings = false)
        {

            if (btnSave == "Save")
            {
                // Save the system setting.
                model.Settings = fac.GetSystemSettings();
                GetSettingIDs(model);
                model.sldPresets = Convert.ToInt32(Convert.ToDecimal(hdnDataSecuritySettings));
                if (model.isRetainGMArchive)
                    model.Settings[model.RETAIN_GOOD_MATCH_ARCHIVE].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_GOOD_MATCH_ARCHIVE].SettingValue = "0";
                if (model.isRetainLCMArchive)
                    model.Settings[model.RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE].SettingValue = "0";
                if (model.isRetainAuditArchive)
                    model.Settings[model.RETAIN_AUDIT_ARCHIVE].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_AUDIT_ARCHIVE].SettingValue = "0";
                if (model.isRetainStewAuditArchive)
                    model.Settings[model.RETAIN_STEWARDSHIP_AUDIT_ARCHIVE].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_STEWARDSHIP_AUDIT_ARCHIVE].SettingValue = "0";
                if (model.isRetainSrcCompanyInfo)
                    model.Settings[model.RETAIN_SRC_COMPANY_INFO].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_SRC_COMPANY_INFO].SettingValue = "0";
                if (model.isRetainOutputData)
                    model.Settings[model.RETAIN_OUTPUT_DATA].SettingValue = "1";
                else
                    model.Settings[model.RETAIN_OUTPUT_DATA].SettingValue = "0";
                if (model.isEncryptAll)
                    model.Settings[model.ENCRYPT_ALL_DATA].SettingValue = "1";
                else
                    model.Settings[model.ENCRYPT_ALL_DATA].SettingValue = "0";
                model.Settings[model.AUDIT_RETENTION_PERIOD_DAYS].SettingValue = model.ArchiveDurationDays;
                model.Settings[model.CUSTOM_SETTINGS].SettingValue = model.isCustomSettings.ToString();
                model.Settings[model.PRE_SET_VALUE].SettingValue = model.sldPresets.ToString();
                fac.UpdateCleanseMatchSettings(model.Settings);
                ViewBag.Message = "Settings updated successfully";
                return PartialView("_Index", model);
            }
            else
            {
                //Cancel to Save the system setting.
                return RedirectToAction("UpdatePresetSettings", new { hdnDataSecuritySettings = hdnDataSecuritySettings, isCustomSettings = isCustomSettings });
            }

        }
        public ActionResult UpdatePresetSettings(string hdnDataSecuritySettings, bool isCustomSettings = false)
        {
            SystemSettingsModel model = new SystemSettingsModel();
            model.sldPresets = Convert.ToInt32(Convert.ToDecimal(hdnDataSecuritySettings));
            model.isCustomSettings = isCustomSettings;
            switch (model.sldPresets.ToString())
            {
                case "1":
                    model.isRetainGMArchive = true;
                    model.isRetainLCMArchive = true;
                    model.isRetainAuditArchive = true;
                    model.isRetainStewAuditArchive = true;
                    model.isRetainSrcCompanyInfo = true;
                    model.isRetainOutputData = true;
                    model.isEncryptAll = false;
                    model.ArchiveDurationDays = "-1";
                    break;
                case "2":
                    model.isRetainGMArchive = true;
                    model.isRetainLCMArchive = true;
                    model.isRetainAuditArchive = true;
                    model.isRetainStewAuditArchive = true;
                    model.isRetainSrcCompanyInfo = true;
                    model.isRetainOutputData = false;
                    model.isEncryptAll = false;
                    model.ArchiveDurationDays = "365";
                    break;
                case "3":
                    model.isRetainGMArchive = false;
                    model.isRetainLCMArchive = false;
                    model.isRetainAuditArchive = true;
                    model.isRetainStewAuditArchive = false;
                    model.isRetainSrcCompanyInfo = false;
                    model.isRetainOutputData = false;
                    model.isEncryptAll = false;
                    model.ArchiveDurationDays = "180";
                    break;
                case "4":
                    model.isRetainGMArchive = false;
                    model.isRetainLCMArchive = false;
                    model.isRetainAuditArchive = false;
                    model.isRetainStewAuditArchive = false;
                    model.isRetainSrcCompanyInfo = false;
                    model.isRetainOutputData = false;
                    model.isEncryptAll = false;
                    model.ArchiveDurationDays = "0";
                    break;
                case "5":
                    model.isRetainGMArchive = false;
                    model.isRetainLCMArchive = false;
                    model.isRetainAuditArchive = false;
                    model.isRetainStewAuditArchive = false;
                    model.isRetainSrcCompanyInfo = false;
                    model.isRetainOutputData = false;
                    model.isEncryptAll = true;
                    model.ArchiveDurationDays = "0";
                    break;
            }
            return PartialView("_Index", model);
        }
        public void LoadCleanseMatchSettings(SystemSettingsModel model)
        {
            // To fill the System setting to view.
            model.Settings = fac.GetSystemSettings();
            GetSettingIDs(model);
            model.sldPresets = Convert.ToInt32(model.Settings[model.PRE_SET_VALUE].SettingValue);
            if (model.Settings[model.RETAIN_GOOD_MATCH_ARCHIVE].SettingValue == "1")
                model.isRetainGMArchive = true;
            else
                model.isRetainGMArchive = false;
            if (model.Settings[model.RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE].SettingValue == "1")
                model.isRetainLCMArchive = true;
            else
                model.isRetainLCMArchive = false;
            if (model.Settings[model.RETAIN_AUDIT_ARCHIVE].SettingValue == "1")
                model.isRetainAuditArchive = true;
            else
                model.isRetainAuditArchive = false;
            if (model.Settings[model.RETAIN_STEWARDSHIP_AUDIT_ARCHIVE].SettingValue == "1")
                model.isRetainStewAuditArchive = true;
            else
                model.isRetainStewAuditArchive = false;
            if (model.Settings[model.RETAIN_OUTPUT_DATA].SettingValue == "1")
                model.isRetainOutputData = true;
            else
                model.isRetainOutputData = false;
            if (model.Settings[model.RETAIN_SRC_COMPANY_INFO].SettingValue == "1")
                model.isRetainSrcCompanyInfo = true;
            else
                model.isRetainSrcCompanyInfo = false;
            if (model.Settings[model.ENCRYPT_ALL_DATA].SettingValue == "1")
                model.isEncryptAll = true;
            else
                model.isEncryptAll = false;
            model.ArchiveDurationDays = model.Settings[model.AUDIT_RETENTION_PERIOD_DAYS].SettingValue;
            model.isCustomSettings = Convert.ToBoolean(model.Settings[model.CUSTOM_SETTINGS].SettingValue);
        }
        private void GetSettingIDs(SystemSettingsModel model)
        {
            for (int i = 0; i < model.Settings.Count; i++)
            {
                string settingname = model.Settings[i].SettingName;
                switch (settingname)
                {
                    case "RETAIN_GOOD_MATCH_ARCHIVE":
                        model.RETAIN_GOOD_MATCH_ARCHIVE = i; break;
                    case "RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE":
                        model.RETAIN_LOW_CONFIDENCE_MATCH_ARCHIVE = i; break;
                    case "RETAIN_AUDIT_ARCHIVE":
                        model.RETAIN_AUDIT_ARCHIVE = i; break;
                    case "RETAIN_STEWARDSHIP_AUDIT_ARCHIVE":
                        model.RETAIN_STEWARDSHIP_AUDIT_ARCHIVE = i; break;
                    case "RETAIN_SRC_COMPANY_INFO":
                        model.RETAIN_SRC_COMPANY_INFO = i; break;
                    case "RETAIN_OUTPUT_DATA":
                        model.RETAIN_OUTPUT_DATA = i; break;
                    case "ENCRYPT_ALL_DATA":
                        model.ENCRYPT_ALL_DATA = i; break;
                    case "AUDIT_RETENTION_PERIOD_DAYS":
                        model.AUDIT_RETENTION_PERIOD_DAYS = i; break;
                    case "CUSTOM_SETTINGS":
                        model.CUSTOM_SETTINGS = i; break;
                    case "PRE_SET_VALUE":
                        model.PRE_SET_VALUE = i; break;
                    default:
                        break;
                }
            }
        }
    }
}