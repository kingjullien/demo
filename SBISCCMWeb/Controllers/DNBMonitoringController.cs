using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PagedList;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled]
    public class DNBMonitoringController : BaseController
    {
        // GET: DNBMonitoring
        int SyncMonitoringProfileCount, SyncNotificationCount, SyncUserPreferenceCount, SyncCurrentMonitoringProfileCount, SyncCurrentNotificationCount, SyncCurrentUserPreferenceCount;
        public ActionResult Index()
        {
            return View();
        }
        [Route("DNB/MonitoringProfile")]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult IndexMonitorProfile()
        {
            if (Request.Headers["X-PJAX"] == "true")
            {
                return View();
            }
            else
            {
                ViewBag.SelectedTab = "Monitoring Direct 2.0";
                ViewBag.SelectedIndividualTab = "Monitoring Profile";
                return View("~/Views/DandB/Index.cshtml");
            }
        }

        public ActionResult IndexLoadMornitoringTabs(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Helper.DefaultMornitoring20Credential = Convert.ToInt32(Parameters);
            }
            return View("IndexMonitoringTabs");
        }

        #region "Monitoring"
        public ActionResult IndexMonitoringProfile()
        {
            MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Get the records according to the page size and value from the database and fill in the list
            List<MonitoringProfileEntity> objMPE = Mpfac.GetMonitoringProfile(Helper.DefaultMornitoring20Credential);
            return PartialView(objMPE);
        }

        [HttpGet]
        public ActionResult CreateMonitorProfile(string Parameters, string temp)
        {
            MonitoringProfileEntity objMPE = new MonitoringProfileEntity();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);

            Helper.ProfileId = 0;
            ViewBag.EditMonitorId = 0;
            ViewBag.IsConditionExists = false;
            #region Fetch data 
            int CreateMonitoringProfileId = 0;

            if (!string.IsNullOrEmpty(Parameters))
            {
                CreateMonitoringProfileId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
            }
            if (CreateMonitoringProfileId > 0)
            {
                MonitorProfileFacade mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                objMPE = mpfac.GetMonitorProfileByID(CreateMonitoringProfileId);
                Helper.ProfileId = objMPE.MonitoringProfileID;
                lstMonitoringElemetsEntity = mpfac.GetMonitoringElementConditionsByProfileId(objMPE.MonitoringProfileID);
                SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
                if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                {
                    ViewBag.EditMonitorId = objMPE.MonitoringProfileID;
                    ViewBag.IsConditionExists = true;
                }
            }
            else
            {
                Helper.ProfileId = 0;
                SessionHelper.lstMonitoringElemetsEntity = string.Empty;
                ViewBag.EditMonitorId = 0;

            }


            #endregion
            #region Reset all list
            SessionHelper.lstBusinessElementssConditon = string.Empty;
            SessionHelper.lstTempMonirtoring = string.Empty;
            SessionHelper.lstMonirtoringTemp = string.Empty;
            SessionHelper.DandB_strCondition = string.Empty;
            #endregion
            return PartialView(objMPE);
        }

        #region Sync Monitoring Profile
        //Sync the current database with Dnb api and get the data from the api and update into database.
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetProfileList()
        {

            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
            if (Direct20Auth != null)
            {
                MonitorProfileFacade MPfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                string endPoint = string.Format(DnBApi.SyncMonitoringProfileList, "1000", SyncCurrentMonitoringProfileCount);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);
                try
                {
                    // Make Api call 
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        ListMonitoring objResponse = JsonConvert.DeserializeObject<ListMonitoring>(result);
                        // To check if the response of the Api is not null and also check the Contain of the response.
                        if (objResponse != null && objResponse.ListMonitoringProfileResponse != null &&
                            objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail != null &&
                            objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.MonitoringProfileDetail != null &&
                            objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.MonitoringProfileDetail.Count > 0
                            )
                        {
                            SyncMonitoringProfileCount = objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.CandidateMatchedQuantity;
                            SyncCurrentMonitoringProfileCount += objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.CandidateReturnedQuantity;
                            // Get the list from the api and make loop for to insert into database for the Monitoring Profile. also check if the monitoring profile is already exists than update the profile into database.
                            foreach (var item in objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.MonitoringProfileDetail)
                            {
                                // Update the response into database
                                MonitoringProfileEntity objMPE = new MonitoringProfileEntity();
                                objMPE.ProfileName = item.MonitoringProfileName;
                                objMPE.ProfileDescription = item.MonitoringProfileDescription;
                                objMPE.MonitoringLevel = item.MonitoringLevel;
                                objMPE.ProductID = Convert.ToInt32(MPfac.getProductList().Where(x => x.ProductCode == item.DNBProductID).Select(x => x.ProductID).FirstOrDefault());
                                objMPE.ProductCode = item.DNBProductID;
                                objMPE.MonitoringProfileID = item.MonitoringProfileID;
                                objMPE.ApplicationTransactionID = Convert.ToInt32(objResponse.ListMonitoringProfileResponse.TransactionDetail.ApplicationTransactionID);
                                objMPE.TransactionTimestamp = Convert.ToDateTime(objResponse.ListMonitoringProfileResponse.TransactionDetail.TransactionTimestamp);
                                objMPE.CustomerReferenceText = "";
                                objMPE.ResultID = objResponse.ListMonitoringProfileResponse.TransactionResult.ResultID;
                                objMPE.SeverityText = objResponse.ListMonitoringProfileResponse.TransactionResult.SeverityText;
                                objMPE.ResultText = objResponse.ListMonitoringProfileResponse.TransactionResult.ResultText;
                                objMPE.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId());
                                objMPE.RequestDateTime = Convert.ToDateTime(objResponse.ListMonitoringProfileResponse.TransactionDetail.TransactionTimestamp);
                                objMPE.ResponseDateTime = Convert.ToDateTime(objResponse.ListMonitoringProfileResponse.TransactionDetail.TransactionTimestamp);
                                // if Monitoring Profile is Cancelled it will be deleted(soft delete)
                                if (item.MonitoringProfileStatusText.ToLower() == "cancelled")
                                {
                                    objMPE.IsDeleted = true;
                                }
                                else
                                {
                                    objMPE.IsDeleted = false;
                                }
                                // Insert Monitoring Profile into database
                                objMPE.CredentialId = Helper.DefaultMornitoring20Credential;
                                MPfac.InsertUpdateMonitoringProfile(objMPE);
                                if (item.MonitoringElementDetail != null && item.MonitoringElementDetail.MonitoringElement.Count > 0)
                                {
                                    //Save Business Elements 
                                    List<MonitoringElementConditionsEntity> lstElementsCondtion = MPfac.GetMonitoringElementConditionsByProfileId(objMPE.MonitoringProfileID).ToList();
                                    // insert Element and  condition into database for there Specific Monitoring Profile.
                                    foreach (var element in item.MonitoringElementDetail.MonitoringElement)
                                    {
                                        MonitoringElementConditionsEntity objMECE = new MonitoringElementConditionsEntity();
                                        objMECE.ProfileID = objMPE.MonitoringProfileID;
                                        objMECE.ProductElementID = Convert.ToInt32(MPfac.GetProductElementData(objMPE.ProductID).Where(x => x.ElementPCMXPath.ToLower() == element.PCMElementXPATHText.ToLower()).Select(x => x.ProductElementID).FirstOrDefault());
                                        objMECE.ChangeCondition = "";
                                        foreach (var el in element.MonitoringChanges)
                                        {
                                            objMECE.ChangeCondition = el.ChangeCondition;
                                            objMECE.Condition = el.ChangeValue != null ? el.ChangeValue : "";
                                            // Get the Monitoring Condition Id from the Database through the Product ElementId and ChangeCondition
                                            int MonitoringConditionID = lstElementsCondtion.Where(x => x.ProductElementID == objMECE.ProductElementID && x.ChangeCondition == objMECE.ChangeCondition).Select(x => x.MonitoringConditionID).FirstOrDefault();
                                            if (MonitoringConditionID > 0)
                                            {
                                                objMECE.MonitoringConditionID = MonitoringConditionID;
                                            }
                                            MPfac.InsertMonitorProfileElementCondition(objMECE);
                                        }
                                    }
                                }
                            }
                        }
                        // sync with database 
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            ListMonitoring objResponse = serializer.Deserialize<ListMonitoring>(result);
                            if (objResponse != null && objResponse.ListMonitoringProfileResponse != null && objResponse.ListMonitoringProfileResponse.TransactionResult != null)
                            {
                                return Json(new { result = false, message = objResponse.ListMonitoringProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(new { result = true, message = CommonMessagesLang.msgSuccess }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (SyncCurrentMonitoringProfileCount >= SyncMonitoringProfileCount)
                {
                    return Json(new { result = true, message = CommonMessagesLang.msgSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return GetProfileList();
                }
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgSetDefaultCredentialForMonitoring }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Create Monitor Profile

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CreateMonitorProfile(string Parameters)
        {
            MonitoringProfileEntity objMP = new MonitoringProfileEntity();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                objMP.ProfileName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1).Trim();
                objMP.ProfileDescription = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                objMP.ProductID = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1) == "" ? "0" : Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                objMP.MonitoringLevel = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                objMP.ProductCode = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                objMP.MonitoringProfileID = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1) == "" ? "0" : Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));
                objMP.CustomerReferenceText = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
            }
            //Save the detail of Monitor Profile
            objMP.CreatedBy = Convert.ToInt32(User.Identity.GetUserId());
            MonitorProfileFacade MPfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            //Save Business Elements 
            List<MonitoringElementConditionsEntity> lstElementsConditions = MPfac.GetMonitoringElementConditionsByProfileId(objMP.MonitoringProfileID).ToList();
            string endPoint = DnBApi.MonitoringProfileUrl;
            //API Statistics table(MP-131)
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
            objMP.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            if (objMP.MonitoringProfileID == 0)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth?.AuthToken);
                MonitoringRequest root = CreateObject(objMP);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                data = data.Replace("xmljson", "@xmlns$mon").Replace("CreateMonitoringProfileRequestMain", "mon:CreateMonitoringProfileRequest");

                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        MonitoringResponse objResponse = serializer.Deserialize<MonitoringResponse>(result);
                        if (objResponse != null)
                        {
                            //Update Response In current profile
                            objMP.ApplicationTransactionID = Convert.ToInt32(objResponse.CreateMonitoringProfileResponse.TransactionDetail.ApplicationTransactionID);
                            objMP.TransactionTimestamp = Convert.ToDateTime(objResponse.CreateMonitoringProfileResponse.TransactionDetail.TransactionTimestamp);
                            objMP.ResultID = objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultID;
                            objMP.SeverityText = objResponse.CreateMonitoringProfileResponse.TransactionResult.SeverityText;
                            objMP.ResultText = objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText;
                            objMP.MonitoringProfileID = objResponse.CreateMonitoringProfileResponse.CreateMonitoringProfileResponseDetail.MonitoringProfileDetail.MonitoringProfileID;
                            objMP.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId());
                            objMP.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            objMP.CredentialId = Helper.DefaultMornitoring20Credential;
                            Helper.ProfileId = MPfac.InsertUpdateMonitoringProfile(objMP);
                            if (lstElementsConditions != null && lstElementsConditions.Any())
                            {
                                foreach (var item in lstElementsConditions)
                                {
                                    MPfac.DeleteMonitoringElementConditions(item.MonitoringConditionID);
                                }
                            }
                            foreach (var item in lstMonitoringElemetsEntity)
                            {
                                item.ProfileID = Helper.ProfileId;
                                MPfac.InsertMonitorProfileElementCondition(item);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            MonitoringResponse objResponse = JsonConvert.DeserializeObject<MonitoringResponse>(result);
                            if (objResponse != null && objResponse.CreateMonitoringProfileResponse != null && objResponse.CreateMonitoringProfileResponse.TransactionResult != null)
                            {
                                return Json(new { result = false, message = objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { result = true, message = DandBSettingLang.msgInsertMonitoringProfile }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                endPoint += "/" + objMP.MonitoringProfileID;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth?.AuthToken);

                UpdateMonitoringRequest root = UpdateRequstObject(objMP);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                data = data.Replace("xmljson", "@xmlns$mon").Replace("MonUpdateMonitoringProfileRequest", "mon:UpdateMonitoringProfileRequest");

                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        UpdateMonitoringResponse objResponse = serializer.Deserialize<UpdateMonitoringResponse>(result);
                        if (objResponse != null)
                        {
                            //Update Response In current profile
                            objMP.ApplicationTransactionID = Convert.ToInt32(objResponse.UpdateMonitoringProfileResponse.TransactionDetail.ApplicationTransactionID);
                            objMP.TransactionTimestamp = Convert.ToDateTime(objResponse.UpdateMonitoringProfileResponse.TransactionDetail.TransactionTimestamp);
                            if (objResponse.UpdateMonitoringProfileResponse.UpdateMonitoringProfileResponseDetail.MonitoringProfileDetail.InquiryReferenceText != null && objResponse.UpdateMonitoringProfileResponse.UpdateMonitoringProfileResponseDetail.MonitoringProfileDetail.InquiryReferenceText.CustomerReferenceText != null && objResponse.UpdateMonitoringProfileResponse.UpdateMonitoringProfileResponseDetail.MonitoringProfileDetail.InquiryReferenceText.CustomerReferenceText.Any())
                            {
                                objMP.CustomerReferenceText = Convert.ToString(objResponse.UpdateMonitoringProfileResponse.UpdateMonitoringProfileResponseDetail.MonitoringProfileDetail.InquiryReferenceText.CustomerReferenceText[0]);
                            }
                            else
                            {
                                objMP.CustomerReferenceText = "";
                            }

                            objMP.ResultID = objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultID;
                            objMP.SeverityText = objResponse.UpdateMonitoringProfileResponse.TransactionResult.SeverityText;
                            objMP.ResultText = objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText;
                            objMP.MonitoringProfileID = objResponse.UpdateMonitoringProfileResponse.UpdateMonitoringProfileResponseDetail.MonitoringProfileDetail.MonitoringProfileID;
                            objMP.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId());
                            objMP.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            objMP.CredentialId = Helper.DefaultMornitoring20Credential;
                            Helper.ProfileId = MPfac.InsertUpdateMonitoringProfile(objMP);
                            if (lstElementsConditions != null && lstElementsConditions.Any())
                            {
                                foreach (var item in lstElementsConditions)
                                {
                                    MPfac.DeleteMonitoringElementConditions(item.MonitoringConditionID);
                                }
                            }
                            foreach (var item in lstMonitoringElemetsEntity)
                            {
                                item.ProfileID = Helper.ProfileId;
                                MPfac.InsertMonitorProfileElementCondition(item);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            UpdateMonitoringResponse objResponse = JsonConvert.DeserializeObject<UpdateMonitoringResponse>(result);
                            if (objResponse != null && objResponse.UpdateMonitoringProfileResponse != null && objResponse.UpdateMonitoringProfileResponse.TransactionResult != null && objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText != null)
                            {
                                return Json(new { result = false, message = objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                return Json(new { result = true, message = DandBSettingLang.msgUpdateMonitoringProfile }, JsonRequestBehavior.AllowGet);
            }

        }

        public MonitoringRequest CreateObject(MonitoringProfileEntity objMPE)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            MonitoringRequest objRequest = new MonitoringRequest();
            MonCreateMonitoringProfileRequest requestObject = new MonCreateMonitoringProfileRequest();

            objRequest.CreateMonitoringProfileRequestMain = requestObject;

            CreateMonitoringProfileRequestDetail objRequestDetail = new CreateMonitoringProfileRequestDetail();
            requestObject.CreateMonitoringProfileRequestDetail = objRequestDetail;

            TransactionDetail objDetail = new TransactionDetail();
            objDetail.ApplicationTransactionID = GetDigits();

            requestObject.TransactionDetail = objDetail;
            requestObject.TransactionDetail.TransactionTimestamp = Convert.ToString(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));// "2017-01-06T09:29:10Z";
            requestObject.xmljson = DnBApi.MonitoingService;

            MonitoringProfileSpecification profileSpecification = new MonitoringProfileSpecification();
            objRequestDetail.MonitoringProfileSpecification = profileSpecification;
            InquiryReferenceText inqRefText = new InquiryReferenceText();
            profileSpecification.DNBProductID = Convert.ToString(objMPE.ProductCode);// selection from product dropdown.
            profileSpecification.MonitoringProfileName = objMPE.ProfileName;// enter profileName.
            profileSpecification.MonitoringProfileDescription = objMPE.ProfileDescription;// enter ProfileDesc,
            profileSpecification.MonitoringLevel = objMPE.MonitoringLevel;// Select from monitoring level.
                                                                          // add monitoring element only when we have Element added
            if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
            {
                #region "Monitoring Element"
                MonitoringElementDetail objElementDetail = new MonitoringElementDetail();
                profileSpecification.MonitoringElementDetail = objElementDetail;
                List<MonitoringElement> objElementList = new List<MonitoringElement>(); // loop through all the elements.
                List<MonitoringElementConditionsEntity> lstElemetsEntity = lstMonitoringElemetsEntity.GroupBy(x => x.ElementName).Select(g => g.First()).ToList();

                foreach (var item in lstElemetsEntity)
                {
                    MonitoringElement objElement = new MonitoringElement();
                    objElement.PCMElementXPATHText = item.ElementPCMXPath;
                    List<MonitoringChanges> lstChanges = new List<MonitoringChanges>();
                    foreach (var changes in lstMonitoringElemetsEntity.Where(x => x.ElementName == item.ElementName))
                    {
                        MonitoringChanges objChanges = new MonitoringChanges();
                        objChanges.ChangeCondition = changes.ChangeCondition;
                        objChanges.ChangeValue = !string.IsNullOrEmpty(changes.Condition) ? changes.Condition : null;
                        lstChanges.Add(objChanges);
                    }
                    objElement.MonitoringChanges = lstChanges;
                    objElementList.Add(objElement);

                    objElementDetail.MonitoringElement = objElementList;
                }
                #endregion
            }
            else
            {
                profileSpecification.MonitoringElementDetail = null;
            }
            if (string.IsNullOrEmpty(objMPE.CustomerReferenceText))
            {
                inqRefText = null;
            }
            else
            {
                inqRefText.CustomerReferenceText = new List<string>() { objMPE.CustomerReferenceText };
            }
            objRequestDetail.InquiryReferenceText = inqRefText;
            return objRequest;
        }

        public UpdateMonitoringRequest UpdateRequstObject(MonitoringProfileEntity objMPE)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            UpdateMonitoringRequest objRequest = new UpdateMonitoringRequest();
            MonUpdateMonitoringProfileRequest requestObject = new MonUpdateMonitoringProfileRequest();

            objRequest.MonUpdateMonitoringProfileRequest = requestObject;

            UpdateMonitoringProfileRequestDetail objRequestDetail = new UpdateMonitoringProfileRequestDetail();
            requestObject.UpdateMonitoringProfileRequestDetail = objRequestDetail;

            TransactionDetail objDetail = new TransactionDetail();

            requestObject.TransactionDetail = objDetail;
            requestObject.TransactionDetail.TransactionTimestamp = Convert.ToString(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            requestObject.xmljson = DnBApi.MonitoingService;

            MonitoringProfileUpdateSpecification profileSpecification = new MonitoringProfileUpdateSpecification();
            objRequestDetail.MonitoringProfileUpdateSpecification = profileSpecification;
            profileSpecification.MonitoringProfileName = objMPE.ProfileName; // enter profileName.
            profileSpecification.ServiceVersionNumber = "5.3";

            // add monitoring element only when we have Element added
            #region "Monitoring Element"

            MonitoringElementDetail objElementDetail = new MonitoringElementDetail();
            profileSpecification.MonitoringElementDetail = objElementDetail;
            List<MonitoringElement> objElementList = new List<MonitoringElement>();
            List<MonitoringElementConditionsEntity> lstElemetsEntity = lstMonitoringElemetsEntity.GroupBy(x => x.ElementName).Select(g => g.First()).ToList();
            foreach (var item in lstElemetsEntity)
            {
                MonitoringElement objME = new MonitoringElement();
                objME.PCMElementXPATHText = item.ElementPCMXPath;
                List<MonitoringChanges> lstChanges = new List<MonitoringChanges>();
                foreach (var changes in lstMonitoringElemetsEntity.Where(x => x.ElementName == item.ElementName))
                {
                    MonitoringChanges objChange = new MonitoringChanges();
                    objChange.ChangeCondition = changes.ChangeCondition;
                    objChange.ChangeValue = !string.IsNullOrEmpty(changes.Condition) ? changes.Condition : null;
                    lstChanges.Add(objChange);
                }
                objME.MonitoringChanges = lstChanges;
                objElementList.Add(objME);
            }
            objElementDetail.MonitoringElement = objElementList;
            InquiryReferenceText inqRefText = new InquiryReferenceText();

            if (string.IsNullOrEmpty(objMPE.CustomerReferenceText))
            {
                inqRefText = null;
            }
            else
            {
                inqRefText.CustomerReferenceText = new List<string>() { objMPE.CustomerReferenceText };
            }
            profileSpecification.InquiryReferenceText = inqRefText;
            if (objRequest.MonUpdateMonitoringProfileRequest.UpdateMonitoringProfileRequestDetail.MonitoringProfileUpdateSpecification.MonitoringElementDetail.MonitoringElement.Count == 0)
            {
                objRequest.MonUpdateMonitoringProfileRequest.UpdateMonitoringProfileRequestDetail.MonitoringProfileUpdateSpecification.MonitoringElementDetail = null;
            }
            #endregion
            return objRequest;
        }

        #endregion

        #region Delete Monitoring Profile
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteProfile(string Parameters)
        {
            if (Request.IsAjaxRequest() && !string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                try
                {
                    if (CheckMonitoringProfileUsed(Convert.ToInt32(Parameters)))
                    {
                        return Json(new { result = false, message = DandBSettingLang.msgUsedMonitoringProfile }, JsonRequestBehavior.AllowGet);
                    }
                    DeleteMonitoringProfile(Parameters);
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            UpdateMonitoringResponse objResponse = serializer.Deserialize<UpdateMonitoringResponse>(result);
                            if (objResponse != null && objResponse.UpdateMonitoringProfileResponse != null && objResponse.UpdateMonitoringProfileResponse.TransactionResult != null)
                            {
                                return Json(new { result = false, message = objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { result = true, message = DandBSettingLang.msgDeleteMonitoringProfile }, JsonRequestBehavior.AllowGet);
        }

        private void DeleteMonitoringProfile(string ProfileId)
        {


            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
            MonitorProfileFacade mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            string endPoint = DnBApi.MonitoringProfileUrl + "/" + ProfileId; // objMPE.MonitoringProfileID;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", Direct20Auth?.AuthToken);

            UpdateMonitoringRequest root = DeleteRequestObject();
            StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

            string data = JsonConvert.SerializeObject(root,
                              Newtonsoft.Json.Formatting.None,
                              new JsonSerializerSettings
                              {
                                  NullValueHandling = NullValueHandling.Ignore
                              });
            data = data.Replace("xmljson", "@xmlns$mon").Replace("MonUpdateMonitoringProfileRequest", "mon:UpdateMonitoringProfileRequest");

            writer.Write(data);
            writer.Close();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var serializer = new JavaScriptSerializer();
                    UpdateMonitoringResponse objResponse = serializer.Deserialize<UpdateMonitoringResponse>(result);
                    mpfac.DeleteMonitoringProfile(Convert.ToInt32(ProfileId));
                }
            }
            catch (WebException)
            { throw; }
        }

        public UpdateMonitoringRequest DeleteRequestObject()
        {
            UpdateMonitoringRequest objRequest = new UpdateMonitoringRequest();
            MonUpdateMonitoringProfileRequest requestObject = new MonUpdateMonitoringProfileRequest();

            objRequest.MonUpdateMonitoringProfileRequest = requestObject;
            requestObject.xmljson = DnBApi.MonitoingService;

            UpdateMonitoringProfileRequestDetail objRequestDetail = new UpdateMonitoringProfileRequestDetail();
            requestObject.UpdateMonitoringProfileRequestDetail = objRequestDetail;

            MonitoringProfileUpdateSpecification profileSpecification = new MonitoringProfileUpdateSpecification();
            objRequestDetail.MonitoringProfileUpdateSpecification = profileSpecification;
            profileSpecification.MonitoringProfileStatusText = "Cancelled";
            InquiryReferenceText inqRefText = new InquiryReferenceText();
            profileSpecification.InquiryReferenceText = inqRefText;

            inqRefText.CustomerReferenceText = new List<string>() { "DCP_PREM", "Test from Rushabh Mehta" };
            return objRequest;
        }
        #endregion

        #region create Business Element Condition
        [RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult SetMonitoringData(string Parameters, string ProductCode, string BusinessLevel)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ProductCode = StringCipher.Decrypt(ProductCode.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                BusinessLevel = StringCipher.Decrypt(BusinessLevel.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                Helper.ProductId = Convert.ToInt32(Parameters);
                Helper.ProductCode = ProductCode;
                Helper.MonitoringLevel = BusinessLevel;
                List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
                if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
                {
                    lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
                }
                MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                if (lstMonitoringElemetsEntity.Count == 0)
                {
                    lstMonitoringElemetsEntity = Mpfac.GetMonitoringElementConditionsByProfileId(Helper.ProfileId).ToList();
                    SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
                }
            }
            return Json(new { result = true, message = CommonMessagesLang.msgSuccess }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult BusinessElement(int? page, int? sortby, int? sortorder, int? pagevalue, int? ProfileId, string Parameters = null)
        {
            bool IsFromMainPage = false;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                IsFromMainPage = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1).Trim());
            }
            // paging nation
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            #endregion
            //Get the list of Business Element 
            List<MonitoringElementConditionsEntity> lstTemp = new List<MonitoringElementConditionsEntity>();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            int skiprecord = Convert.ToInt32(pageSize * (pageNumber - 1));
            if (pageNumber == 1)
            {
                lstTemp = lstMonitoringElemetsEntity.Take(pageSize).ToList();
            }
            else
            {
                lstTemp = lstMonitoringElemetsEntity.Skip(skiprecord).Take(pageSize).ToList();
            }

            IPagedList<MonitoringElementConditionsEntity> pagedElementCondition = new StaticPagedList<MonitoringElementConditionsEntity>(lstTemp.ToList(), currentPageIndex, pageSize, lstMonitoringElemetsEntity.Count);
            SessionHelper.lstBusinessElementssConditon = string.Empty;
            SessionHelper.lstTempMonirtoring = string.Empty;
            if (IsFromMainPage)
            {
                return PartialView("BusinessElement", pagedElementCondition);
            }
            else
            {
                return PartialView("_businessElement", pagedElementCondition);
            }

        }

        #region Create Business Element
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult SaveBusinessElement(string Parameters)
        {
            //Save Business Element
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            MonitoringElementConditionsEntity objMECE = new MonitoringElementConditionsEntity();
            MonitorProfileFacade mfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            string BusinessCondionUpdateId = null;
            string SingleCondition = null;
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                objMECE.ElementName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                SingleCondition = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                BusinessCondionUpdateId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                objMECE.ProductElementID = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1) == "" ? "0" : Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));
            }
            objMECE.ElementPCMXPath = Convert.ToString(mfac.GetProductElementData(Helper.ProductId).Where(x => x.ElementName == objMECE.ElementName).Select(x => x.ElementPCMXPath).FirstOrDefault());
            if (!string.IsNullOrEmpty(BusinessCondionUpdateId))
            {
                objMECE.MonitoringConditionID = Convert.ToInt32(BusinessCondionUpdateId);
            }
            objMECE.ProfileID = Helper.ProfileId;
            objMECE.ChangeCondition = SingleCondition;
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (!string.IsNullOrEmpty(SessionHelper.lstBusinessElementssConditon))
            {
                lstBusinessElementssConditon = JsonConvert.DeserializeObject<List<BusinessElementssConditonList>>(SessionHelper.lstBusinessElementssConditon);
            }
            var jsonSerialiser = new JavaScriptSerializer();
            var strBusinessEleemts = jsonSerialiser.Serialize(lstBusinessElementssConditon);
            objMECE.JsonCondition = strBusinessEleemts;
            objMECE.Condition = objMECE.Condition != null ? objMECE.Condition : "";
            if (!string.IsNullOrEmpty(BusinessCondionUpdateId) && Convert.ToInt32(BusinessCondionUpdateId) > 0)
            {
                if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                {
                    foreach (var item in lstMonitoringElemetsEntity)
                    {
                        if (item.TempConditionId == Convert.ToInt32(BusinessCondionUpdateId))
                        {
                            item.ElementName = objMECE.ElementName;
                            item.ElementPCMXPath = objMECE.ElementPCMXPath;
                            item.ChangeCondition = objMECE.ChangeCondition;
                            item.ProductElementID = objMECE.ProductElementID;
                            item.Condition = objMECE.Condition;
                            item.JsonCondition = !string.IsNullOrEmpty(objMECE.Condition) ? objMECE.JsonCondition : "";
                        }
                    }
                    SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
                }
            }
            else
            {
                if (lstMonitoringElemetsEntity.Count > 0)
                {
                    objMECE.TempConditionId = lstMonitoringElemetsEntity.OrderByDescending(x => x.TempConditionId).Select(x => x.TempConditionId).FirstOrDefault() + 1;
                }
                else
                {
                    objMECE.TempConditionId = 1;
                }
                lstMonitoringElemetsEntity.Add(objMECE);
                SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
            }
            return new JsonResult { Data = CommonMessagesLang.msgSuccess };
        }
        #endregion

        #region Delete Business Elements
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteMainConditon(string Parameters)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (Parameters != null && Convert.ToInt32(Parameters) > 0 && lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                {
                    for (int i = 0; i <= lstMonitoringElemetsEntity.Count - 1; i++)
                    {
                        if (lstMonitoringElemetsEntity[i].TempConditionId == Convert.ToInt32(Parameters))
                        {
                            lstMonitoringElemetsEntity.RemoveAt(i);
                        }
                    }
                }
            }
            SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
            if (Request.IsAjaxRequest())
            {
                return Json(new { result = true, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, Message = "fail" }, JsonRequestBehavior.AllowGet);

        }
        #endregion
        #endregion

        #region Business Condition List
        public ActionResult AddBusinessElement(string Parameters)
        {
            SessionHelper.ConditionCount = "0";
            SessionHelper.DandB_strCondition = string.Empty;
            //Get Query string in Encrypted mode and decrypt Query string and set Parameters
            SessionHelper.BusinessCondionUpdateId = "";
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (!string.IsNullOrEmpty(SessionHelper.lstBusinessElementssConditon))
            {
                lstBusinessElementssConditon = JsonConvert.DeserializeObject<List<BusinessElementssConditonList>>(SessionHelper.lstBusinessElementssConditon);
            }
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (!string.IsNullOrEmpty(SessionHelper.lstTempMonirtoring))
            {
                lstTempMonirtoring = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstTempMonirtoring);
            }
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                SessionHelper.BusinessCondionUpdateId = Parameters;

                if (lstBusinessElementssConditon.Count == 0 && lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Count > 0)
                {
                    foreach (var item in lstMonitoringElemetsEntity)
                    {
                        if (item.TempConditionId == Convert.ToInt32(Parameters))
                        {
                            ViewBag.ProductElementID = item.ProductElementID;
                            Helper.ProductElementID = item.ProductElementID;
                            ViewBag.SingleCondition = item.ChangeCondition;
                            string conditionstring = item.JsonCondition;
                            if (lstBusinessElementssConditon == null)
                            {
                                lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
                            }
                            var jsonSerialiser = new JavaScriptSerializer();
                            if (conditionstring != null)
                            {
                                lstBusinessElementssConditon = jsonSerialiser.Deserialize<List<BusinessElementssConditonList>>(conditionstring);
                                if (lstBusinessElementssConditon != null && lstBusinessElementssConditon.Count > 0)
                                {
                                    int count = lstBusinessElementssConditon.Count - 1;
                                    List<BusinessElementssConditonList> lstBusiness = new List<BusinessElementssConditonList>();
                                    lstBusiness.Add(lstBusinessElementssConditon[count]);
                                    lstBusinessElementssConditon = lstBusiness;
                                    lstTempMonirtoring = lstBusiness[0].lstBusinessElements;
                                    SessionHelper.DandB_strCondition = lstBusiness[0].strCondition;
                                    item.JsonCondition = jsonSerialiser.Serialize(lstBusinessElementssConditon);
                                    Helper.strCondition = SessionHelper.DandB_strCondition;
                                    if (lstTempMonirtoring != null && lstTempMonirtoring.Any())
                                    {
                                        foreach (var Monitoring in lstTempMonirtoring)
                                        {
                                            if (Monitoring.objM.lstCondition != null && Monitoring.objM.lstCondition.Any())
                                            {
                                                SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) + Monitoring.objM.lstCondition.Count);
                                            }
                                            else
                                            {
                                                SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) + 1);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lstTempMonirtoring = new List<TempMonitoring>();
                                }
                            }
                        }
                    }
                }
            }
            if (lstTempMonirtoring != null && lstTempMonirtoring.Count > 0)
            {
                int TempCount = 0;
                foreach (var item in lstTempMonirtoring)
                {
                    if (item.objM != null)
                    {
                        item.objM.TempConditionId = Convert.ToString(TempCount + 1);
                        TempCount += 1;
                    }

                }
                SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
            }
            SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
            SessionHelper.lstBusinessElementssConditon = JsonConvert.SerializeObject(lstBusinessElementssConditon);
            Session.Remove("ConditionListUpdatesId");
            return View(lstTempMonirtoring);
        }
        //Delete condition from  Business Condition List
        public ActionResult DeleteBusinessCondition(string Parameters)
        {

            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (!string.IsNullOrEmpty(SessionHelper.lstBusinessElementssConditon))
            {
                lstBusinessElementssConditon = JsonConvert.DeserializeObject<List<BusinessElementssConditonList>>(SessionHelper.lstBusinessElementssConditon);
            }
            int DeletedId = Convert.ToInt32(Parameters);
            for (int i = 0; i < lstBusinessElementssConditon.Count; i++)
            {
                if (lstBusinessElementssConditon[i].ElementsConditonListId == Convert.ToInt32(Convert.ToInt32(DeletedId)))
                {
                    lstBusinessElementssConditon.RemoveAt(i);
                }
            }
            SessionHelper.lstBusinessElementssConditon = JsonConvert.SerializeObject(lstBusinessElementssConditon);
            return PartialView("_businessElements", lstBusinessElementssConditon);
        }
        #endregion

        #region Element Condition List
        [HttpGet]
        public ActionResult AddElementCondition(string Parameters, string ConditionListUpdatesId)
        {
            MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
            string btnSubmit = "", deletedId = "", GrpCondition = ""; string TempGrpId = "0";
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (!string.IsNullOrEmpty(SessionHelper.lstTempMonirtoring))
            {
                lstTempMonirtoring = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstTempMonirtoring);
            }
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                objMPEC.Conditon = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                objMPEC.Element = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                objMPEC.ConditonOpetator = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                objMPEC.ConditonValue = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                btnSubmit = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                deletedId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1);
                TempGrpId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 6, 1);
                GrpCondition = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 7, 1);
                bool IsCreate = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1) == "" ? "false" : Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 8, 1));
                bool IsGroup = Convert.ToBoolean(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1) == "" ? "false" : Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 9, 1));

                switch (btnSubmit)
                {
                    case "Add":
                        if (objMPEC.Conditon == MonitoringType.Group.ToString())
                        {
                            if (TempGrpId != "0")
                            {
                                for (int i = 0; i <= lstTempMonirtoring.Count - 1; i++)
                                {
                                    AddGroupElementRecursive(lstTempMonirtoring[i], TempGrpId, IsCreate, IsGroup, GrpCondition);
                                    SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
                                }
                            }
                            else
                            {
                                MonitoingProfileElementsConditon objTMPEC = new MonitoingProfileElementsConditon();
                                string count = Convert.ToString(lstTempMonirtoring.Count + 1);
                                TempMonitoring objTM = new TempMonitoring();
                                objTM.objM = objTMPEC;
                                objTM.objM.TempConditionId = count;
                                objTM.objM.Conditon = GrpCondition.ToString();
                                objTM.objM.lstCondition = new List<TempMonitoring>();
                                lstTempMonirtoring.Add(objTM);
                                SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
                            }
                        }
                        else
                        {
                            SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) + 1);
                            AddBlankRowinConditons(objMPEC.Conditon);
                        }

                        break;
                    case "Update":
                        for (int i = 0; i <= lstTempMonirtoring.Count - 1; i++)
                        {
                            UpdateGroupElement(lstTempMonirtoring[i], TempGrpId, objMPEC);
                            SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
                        }
                        break;
                    case "delete":
                        List<TempMonitoring> lstMonirtoringTemp = new List<TempMonitoring>();
                        SessionHelper.lstMonirtoringTemp = JsonConvert.SerializeObject(lstMonirtoringTemp);
                        DeleteElementGroupNew(lstTempMonirtoring, TempGrpId);
                        SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
                        break;
                }
            }

            if (!string.IsNullOrEmpty(ConditionListUpdatesId))
            {
                ConditionListUpdatesId = StringCipher.Decrypt(ConditionListUpdatesId.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
                if (!string.IsNullOrEmpty(SessionHelper.lstBusinessElementssConditon))
                {
                    lstBusinessElementssConditon = JsonConvert.DeserializeObject<List<BusinessElementssConditonList>>(SessionHelper.lstBusinessElementssConditon);
                }
                lstTempMonirtoring = new List<TempMonitoring>();
                for (int i = 0; i < lstBusinessElementssConditon.Count; i++)
                {
                    if (lstBusinessElementssConditon[i].ElementsConditonListId == Convert.ToInt32(Convert.ToInt32(ConditionListUpdatesId)))
                    {
                        lstTempMonirtoring = lstBusinessElementssConditon[i].lstBusinessElements;
                    }
                }
                SessionHelper.ConditionListUpdatesId = ConditionListUpdatesId;
                SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
                SessionHelper.DandB_strCondition = string.Empty;
                CreateStringForCondition(lstTempMonirtoring);

                return View("AddElementCondition", lstTempMonirtoring);
            }
            if (!string.IsNullOrEmpty(SessionHelper.lstTempMonirtoring))
            {
                lstTempMonirtoring = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstTempMonirtoring);
            }
            SessionHelper.DandB_strCondition = string.Empty;
            Helper.strCondition = "";
            CreateStringForCondition(lstTempMonirtoring);
            Helper.strCondition = SessionHelper.DandB_strCondition.Replace("(And", "And(").Replace("(OR", "OR(");
            if (Request.IsAjaxRequest())
            {
                return PartialView("_conditonListView", lstTempMonirtoring);
            }
            else
            {
                return View(lstTempMonirtoring);
            }
        }
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult AddElementCondition(string BusinessCondionUpdateId, string ParentElements, string ParentCondition, string ProductElementId)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (!string.IsNullOrEmpty(SessionHelper.lstBusinessElementssConditon))
            {
                lstBusinessElementssConditon = JsonConvert.DeserializeObject<List<BusinessElementssConditonList>>(SessionHelper.lstBusinessElementssConditon);
            }
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (!string.IsNullOrEmpty(SessionHelper.lstTempMonirtoring))
            {
                lstTempMonirtoring = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstTempMonirtoring);
            }
            BusinessElementssConditonList objBECL = new BusinessElementssConditonList();
            MonitoringElementConditionsEntity objMECE = new MonitoringElementConditionsEntity();
            if (!string.IsNullOrEmpty(ParentElements))
                ParentElements = StringCipher.Decrypt(ParentElements.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

            if (!string.IsNullOrEmpty(ParentCondition))
                ParentCondition = StringCipher.Decrypt(ParentCondition.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

            if (!string.IsNullOrEmpty(ProductElementId))
                ProductElementId = StringCipher.Decrypt(ProductElementId.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

            if (!string.IsNullOrEmpty(BusinessCondionUpdateId))
                BusinessCondionUpdateId = StringCipher.Decrypt(BusinessCondionUpdateId.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

            if (lstBusinessElementssConditon != null)
            {
                if (lstBusinessElementssConditon.Count > 0)
                {
                    foreach (var item in lstBusinessElementssConditon)
                    {
                        objBECL.ElementsConditonListId = item.ElementsConditonListId + 1;
                    }
                }
                else
                {
                    objBECL.ElementsConditonListId = 1;
                }
            }

            objBECL.lstBusinessElements = lstTempMonirtoring;
            SessionHelper.DandB_strCondition = string.Empty;
            CreateStringForCondition(lstTempMonirtoring);
            if (!string.IsNullOrEmpty(SessionHelper.DandB_strCondition))
            {
                objBECL.strCondition = SessionHelper.DandB_strCondition.Replace("(And", "And(").Replace("(OR", "OR(");
            }
            if (!string.IsNullOrEmpty(BusinessCondionUpdateId) && Convert.ToInt32(BusinessCondionUpdateId) > 0 && lstBusinessElementssConditon.Count > 0)
            {
                BusinessElementssConditonList objTBECL = lstBusinessElementssConditon.FirstOrDefault(x => x.ElementsConditonListId == Convert.ToInt32(BusinessCondionUpdateId));
                if (objTBECL != null)
                {
                    foreach (var item in lstBusinessElementssConditon)
                    {
                        if (item.ElementsConditonListId == Convert.ToInt32(BusinessCondionUpdateId))
                        {
                            item.lstBusinessElements = objBECL.lstBusinessElements;
                            item.ElementsConditonList = objBECL.ElementsConditonList;
                            item.strCondition = objBECL.strCondition;
                        }
                    }
                }
                else
                {
                    lstBusinessElementssConditon.Add(objBECL);
                }
            }
            else
            {
                lstBusinessElementssConditon.Add(objBECL);
            }
            SessionHelper.lstBusinessElementssConditon = JsonConvert.SerializeObject(lstBusinessElementssConditon);
            SessionHelper.lstTempMonirtoring = string.Empty;

            #region Save for display in business elements listing
            objMECE.ElementName = ParentElements;
            objMECE.ChangeCondition = ParentCondition;
            objMECE.MonitoringConditionID = 0;
            objMECE.ProductElementID = Convert.ToInt32(ProductElementId);
            MonitorProfileFacade mfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            objMECE.ElementPCMXPath = Convert.ToString(mfac.GetProductElementData(Helper.ProductId).Where(x => x.ElementName == objMECE.ElementName).Select(x => x.ElementPCMXPath).FirstOrDefault());
            var jsonSerialiser = new JavaScriptSerializer();
            objMECE.JsonCondition = jsonSerialiser.Serialize(lstBusinessElementssConditon);
            objMECE.Condition = SessionHelper.DandB_strCondition.Replace("(And", "And(").Replace("(OR", "OR(");
            if (!string.IsNullOrEmpty(BusinessCondionUpdateId) && Convert.ToInt32(BusinessCondionUpdateId) > 0)
            {
                if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                {
                    foreach (var item in lstMonitoringElemetsEntity)
                    {
                        if (item.TempConditionId == Convert.ToInt32(BusinessCondionUpdateId))
                        {
                            item.ElementName = objMECE.ElementName;
                            item.ElementPCMXPath = objMECE.ElementPCMXPath;
                            item.ChangeCondition = objMECE.ChangeCondition;
                            item.ProductElementID = objMECE.ProductElementID;
                            item.Condition = objMECE.Condition;
                            item.JsonCondition = objMECE.JsonCondition;
                        }
                    }
                }
            }
            else
            {
                if (lstMonitoringElemetsEntity.Count > 0)
                {
                    objMECE.TempConditionId = lstMonitoringElemetsEntity[lstMonitoringElemetsEntity.Count - 1].TempConditionId + 1;
                }
                else
                {
                    objMECE.TempConditionId = 1;
                }
                lstMonitoringElemetsEntity.Add(objMECE);
            }
            SessionHelper.lstMonitoringElemetsEntity = JsonConvert.SerializeObject(lstMonitoringElemetsEntity);
            SessionHelper.lstBusinessElementssConditon = string.Empty;
            #endregion
            return new JsonResult { Data = "success" };
        }
        [HttpGet, RequestFromAjax, RequestFromSameDomain]
        public JsonResult EmptyCondition()
        {
            SessionHelper.ConditionCount = "0";
            SessionHelper.lstTempMonirtoring = string.Empty;
            return new JsonResult { Data = "success" };
        }

        [HttpGet, RequestFromAjax, RequestFromSameDomain]
        public JsonResult RemoveCondition(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (!string.IsNullOrEmpty(Parameters) && Convert.ToInt32(Parameters) > 0)
                {
                    List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
                    if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                    {
                        MonitorProfileFacade MPfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                        foreach (var item in lstMonitoringElemetsEntity)
                        {
                            MPfac.DeleteMonitoringElementConditions(item.MonitoringConditionID);
                        }
                    }
                }
            }
            SessionHelper.ConditionCount = "0";
            SessionHelper.lstTempMonirtoring = string.Empty;
            SessionHelper.lstBusinessElementssConditon = string.Empty;
            SessionHelper.lstMonitoringElemetsEntity = string.Empty;
            SessionHelper.lstTempMonirtoring = string.Empty;
            SessionHelper.lstMonirtoringTemp = string.Empty;
            SessionHelper.DandB_strCondition = string.Empty;
            return new JsonResult { Data = "success" };
        }
        #endregion

        #region Add blank Row in Element Conditions
        public void AddBlankRowinConditons(string Condition)
        {
            // Single node add in list for the Condition. It may be "And" or "OR"
            MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (!string.IsNullOrEmpty(SessionHelper.lstTempMonirtoring))
            {
                lstTempMonirtoring = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstTempMonirtoring);
            }
            int lastcount = 0;
            if (lstTempMonirtoring != null && lstTempMonirtoring.Any())
            {
                lastcount = Convert.ToInt32(lstTempMonirtoring[lstTempMonirtoring.Count - 1].objM.TempConditionId);
            }
            objMPEC.TempConditionId = Convert.ToString(lastcount + 1);
            objMPEC.Conditon = Condition;
            objMPEC.Element = "element";
            objMPEC.ConditonOpetator = "ValueEquals";
            objMPEC.ConditonValue = "";
            TempMonitoring objTM = new TempMonitoring();
            objTM.objM = objMPEC;
            objTM.objM.lstCondition = null;
            lstTempMonirtoring.Add(objTM);
            SessionHelper.lstTempMonirtoring = JsonConvert.SerializeObject(lstTempMonirtoring);
        }
        // Add Recursive Element in List and Add Group in List and also user can add n level or group and Condition in List.
        public void AddGroupElementRecursive(TempMonitoring objTemp, string TempGrpId, bool IsCreate, bool IsGroup, string GrpCondition)
        {
            if (IsCreate)
            {
                if (objTemp.objM.TempConditionId == TempGrpId) // To Check current active node id in the list 
                {
                    if (objTemp.objM == null)
                        objTemp.objM = new MonitoingProfileElementsConditon();

                    if (IsGroup) // if user want to add Group in list than check is it Group or node.
                    {
                        if (objTemp.objM.lstCondition == null)   // if last node of list in empty or null than create and add the node 
                        {
                            objTemp.objM.lstCondition = new List<TempMonitoring>();

                            MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
                            int number = 0;
                            if (objTemp.objM.lstCondition.Count == 0)
                            {
                                number = 0;
                            }
                            else
                            {
                                string[] val = objTemp.objM.lstCondition[objTemp.objM.lstCondition.Count - 1].objM.TempConditionId.Split(new string[] { "@@@" }, StringSplitOptions.None);
                                number = Convert.ToInt32(val[val.Count() - 1]);
                            }
                            string count = TempGrpId + "@@@" + Convert.ToString(number + 1);
                            TempMonitoring objTM = new TempMonitoring();
                            objTM.objM = objMPEC;
                            objTM.objM.Conditon = GrpCondition.ToString();// MonitoringType.Group.ToString();
                            if (objTemp.objM.lstCondition == null)
                                objTemp.objM.lstCondition = new List<TempMonitoring>();
                            goto Outer;
                        }
                        else
                        {
                            int number = 0;
                            if (objTemp.objM.lstCondition.Count == 0)
                            {
                                number = 0;
                            }
                            else
                            {
                                string[] val = objTemp.objM.lstCondition[objTemp.objM.lstCondition.Count - 1].objM.TempConditionId.Split(new string[] { "@@@" }, StringSplitOptions.None);
                                number = Convert.ToInt32(val[val.Count() - 1]);
                            }
                            MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
                            string count = TempGrpId + "@@@" + Convert.ToString(number + 1);
                            TempMonitoring objTM = new TempMonitoring();
                            objTM.objM = objMPEC;
                            objTM.objM.Conditon = GrpCondition.ToString(); // MonitoringType.Group.ToString();
                            objTM.objM.lstCondition = new List<TempMonitoring>();
                            objTM.objM.TempConditionId = count;
                            objTemp.objM.lstCondition.Add(objTM);
                            goto Outer;
                        }
                    }
                    else
                    {
                        int number = 0;
                        if (objTemp.objM.lstCondition == null || objTemp.objM.lstCondition.Count == 0)
                        {
                            number = 0;
                        }
                        else
                        {
                            string[] val = objTemp.objM.lstCondition[objTemp.objM.lstCondition.Count - 1].objM.TempConditionId.Split(new string[] { "@@@" }, StringSplitOptions.None);
                            number = Convert.ToInt32(val[val.Count() - 1]);
                        }
                        MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
                        string count = TempGrpId + "@@@" + Convert.ToString(number + 1);
                        objMPEC.Conditon = GrpCondition.ToString();
                        objMPEC.Element = "element";
                        objMPEC.ConditonOpetator = "ValueEquals";
                        objMPEC.ConditonValue = "";
                        objMPEC.TempConditionId = count;
                        TempMonitoring objTM = new TempMonitoring();
                        objTM.objM = objMPEC;
                        objTM.objM.lstCondition = null;
                        if (objTemp.objM.lstCondition == null)
                            objTemp.objM.lstCondition = new List<TempMonitoring>();

                        objTemp.objM.lstCondition.Add(objTM);
                        SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) + 1);
                        goto Outer;
                    }
                }
                else
                {
                    if (objTemp.objM.lstCondition != null && objTemp.objM.lstCondition.Count > 0)
                    {
                        for (int k = 0; k <= objTemp.objM.lstCondition.Count - 1; k++)
                        {
                            AddGroupElementRecursive(objTemp.objM.lstCondition[k], TempGrpId, IsCreate, IsGroup, GrpCondition);
                        }
                    }
                }

            }
        Outer:;
        }

        public void UpdateGroupElement(TempMonitoring objTemp, string TempGrpId, MonitoingProfileElementsConditon objMPEC)
        {
            if (objTemp.objM.TempConditionId == TempGrpId)
            {
                objTemp.objM.ConditonOpetator = objMPEC.ConditonOpetator;
                objTemp.objM.Element = objMPEC.Element;
                objTemp.objM.ConditonValue = objMPEC.ConditonValue;
                goto Outer;
            }
            else
            {
                if (objTemp.objM.lstCondition != null && objTemp.objM.lstCondition.Count > 0)
                {
                    for (int j = 0; j <= objTemp.objM.lstCondition.Count - 1; j++)
                    {
                        UpdateGroupElement(objTemp.objM.lstCondition[j], TempGrpId, objMPEC);
                    }
                }
            }
        Outer:;
        }
        public void DeleteElementGroup(TempMonitoring objTemp, string TempGrpId)
        {
            List<TempMonitoring> lstMonirtoringTemp = new List<TempMonitoring>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonirtoringTemp))
            {
                lstMonirtoringTemp = JsonConvert.DeserializeObject<List<TempMonitoring>>(SessionHelper.lstMonirtoringTemp);
            }
            TempMonitoring objMTM = new TempMonitoring();
            if (!string.IsNullOrEmpty(SessionHelper.objMTM))
            {
                objMTM = JsonConvert.DeserializeObject<TempMonitoring>(SessionHelper.objMTM);
            }
            if (objTemp.objM != null)
            {
                if (objTemp.objM.Conditon == MonitoringType.Group.ToString())
                {
                    TempMonitoring objTClone = objTemp.Clone().Copy();

                    objMTM.objM = new MonitoingProfileElementsConditon();
                    objMTM.objM.Conditon = MonitoringType.Group.ToString();
                    objMTM.objM.ConditonOpetator = null;
                    objMTM.objM.ConditonValue = null;
                    objMTM.objM.Element = null;
                    objMTM.objM.TempConditionId = Convert.ToString(lstMonirtoringTemp.Count + 1);
                    objMTM.objM.lstCondition = new List<TempMonitoring>();
                    lstMonirtoringTemp.Add(objMTM);
                    SessionHelper.lstMonirtoringTemp = JsonConvert.SerializeObject(lstMonirtoringTemp);
                    SessionHelper.objMTM = JsonConvert.SerializeObject(objMTM);

                    objTemp = objTClone.Clone().Copy();
                    if (objTemp.objM.TempConditionId != TempGrpId && objTemp.objM.lstCondition != null && objTemp.objM.lstCondition.Count > 0)
                    {
                        for (int j = 0; j <= objTemp.objM.lstCondition.Count - 1; j++)
                        {
                            DeleteElementGroup(objTemp.objM.lstCondition[j], TempGrpId);
                        }
                    }
                }
                else
                {
                    if (objTemp.objM.TempConditionId != TempGrpId)
                    {
                        if (objTemp.objM.TempConditionId.Contains("@@@"))
                        {
                            objMTM.objM.lstCondition.Add(objTemp);
                            SessionHelper.objMTM = JsonConvert.SerializeObject(objMTM);
                        }
                        else
                        {
                            lstMonirtoringTemp.Add(objTemp);
                            SessionHelper.lstMonirtoringTemp = JsonConvert.SerializeObject(lstMonirtoringTemp);
                        }
                    }
                }

            }
        }

        public void DeleteElementGroupNew(List<TempMonitoring> lstTemp, string TempGrpId)
        {
            for (int i = 0; i < lstTemp.Count; i++)
            {
                if (lstTemp[i].objM != null && lstTemp[i].objM.TempConditionId == TempGrpId)
                {
                    if (lstTemp[i].objM.lstCondition != null && lstTemp[i].objM.lstCondition.Any())
                    {
                        SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) - lstTemp[i].objM.lstCondition.Count);
                    }
                    else
                    {
                        SessionHelper.ConditionCount = Convert.ToString(Convert.ToInt32(SessionHelper.ConditionCount) - 1);
                    }
                    lstTemp.RemoveAt(i);
                    break;
                }
                else if (lstTemp[i].objM != null && lstTemp[i].objM.lstCondition != null && lstTemp[i].objM.lstCondition.Any())
                {
                    DeleteElementGroupNew(lstTemp[i].objM.lstCondition, TempGrpId);
                }
            }
        }
        public void CreateStringForCondition(List<TempMonitoring> lstTemp)
        {
            string strCondition = "";
            if (!string.IsNullOrEmpty(SessionHelper.DandB_strCondition))
            {
                strCondition = SessionHelper.DandB_strCondition;
            }
            if (lstTemp != null && lstTemp.Count > 0)
            {
                for (int i = 0; i < lstTemp.Count; i++)
                {
                    if (lstTemp[i].objM != null)
                    {
                        if (lstTemp[i].objM.Conditon != MonitoringType.Group.ToString() && lstTemp[i].objM.Conditon != MonitoringType.AndGroup.ToString() && lstTemp[i].objM.Conditon != MonitoringType.ORGroup.ToString())
                        {
                            if (string.IsNullOrEmpty(strCondition))
                            {
                                if (Helper.ElementType.ToLower() == "string")
                                {
                                    strCondition += "(" + lstTemp[i].objM.Element + " " + lstTemp[i].objM.ConditonOpetator + " '" + lstTemp[i].objM.ConditonValue + "' ) ";
                                }
                                else
                                {
                                    strCondition += "(" + lstTemp[i].objM.Element + " " + lstTemp[i].objM.ConditonOpetator + " " + lstTemp[i].objM.ConditonValue + " ) ";
                                }
                                if (!string.IsNullOrEmpty(strCondition) && strCondition.Length > 5)
                                {
                                    if (strCondition.Substring(0, 3) == "(And")
                                    {
                                        strCondition = strCondition.Substring(3, strCondition.Length - 3);
                                    }
                                    else if (strCondition.Substring(0, 3) == "(OR")
                                    {
                                        strCondition = strCondition.Substring(2, strCondition.Length - 2);
                                    }
                                }
                                SessionHelper.DandB_strCondition = strCondition;
                            }
                            else
                            {
                                if (Helper.ElementType.ToLower() == "string")
                                {
                                    strCondition += (lstTemp[i].objM.Conditon != null ? lstTemp[i].objM.Conditon : "") + " " + "(" + lstTemp[i].objM.Element + " " + lstTemp[i].objM.ConditonOpetator + " '" + lstTemp[i].objM.ConditonValue + "' ) ";
                                }
                                else
                                {
                                    strCondition += (lstTemp[i].objM.Conditon != null ? lstTemp[i].objM.Conditon : "") + " " + "(" + lstTemp[i].objM.Element + " " + lstTemp[i].objM.ConditonOpetator + " " + lstTemp[i].objM.ConditonValue + " ) ";
                                }
                                if (!string.IsNullOrEmpty(strCondition) && strCondition.Length > 5)
                                {
                                    if (strCondition.Substring(0, 4) == "(And")
                                    {
                                        strCondition = "(" + strCondition.Substring(4, strCondition.Length - 4);
                                    }
                                    else if (strCondition.Substring(0, 3) == "(OR")
                                    {
                                        strCondition = "(" + strCondition.Substring(3, strCondition.Length - 3);
                                    }
                                }
                                SessionHelper.DandB_strCondition = strCondition;
                            }

                        }
                        else if (lstTemp[i].objM != null && lstTemp[i].objM.lstCondition != null && lstTemp[i].objM.lstCondition.Any())
                        {
                            lstTemp[i].objM.lstCondition[0].objM.Conditon = null;
                            strCondition += "(" + (lstTemp[i].objM.Conditon == "AndGroup" ? "And" : "OR");
                            if (!string.IsNullOrEmpty(strCondition) && strCondition.Length > 5)
                            {
                                if (strCondition.Substring(0, 4) == "(And")
                                {
                                    strCondition = "(" + strCondition.Substring(4, strCondition.Length - 4);
                                }
                                else if (strCondition.Substring(0, 3) == "(OR")
                                {
                                    strCondition = "(" + strCondition.Substring(3, strCondition.Length - 3);
                                }
                            }
                            SessionHelper.DandB_strCondition = strCondition;
                            CreateStringForCondition(lstTemp[i].objM.lstCondition);
                            if (!string.IsNullOrEmpty(SessionHelper.DandB_strCondition))
                            {
                                strCondition = SessionHelper.DandB_strCondition;
                            }
                            strCondition += ") ";
                            if (!string.IsNullOrEmpty(strCondition) && strCondition.Length > 5)
                            {
                                if (strCondition.Substring(0, 4) == "(And")
                                {
                                    strCondition = "(" + strCondition.Substring(4, strCondition.Length - 4);
                                }
                                else if (strCondition.Substring(0, 3) == "(OR")
                                {
                                    strCondition = "(" + strCondition.Substring(3, strCondition.Length - 3);
                                }
                            }
                            SessionHelper.DandB_strCondition = strCondition;
                        }
                    }
                }
            }
        }
        public void CreateStringForMaterCondition(List<TempMonitoring> lstTemp)
        {
            string strCondition = "";
            if (!string.IsNullOrEmpty(SessionHelper.DandB_strCondition))
            {
                strCondition = SessionHelper.DandB_strCondition;
            }
            if (lstTemp != null && lstTemp.Count > 0)
            {
                for (int i = 0; i < lstTemp.Count; i++)
                {
                    if (lstTemp[i].objM != null)
                    {
                        if (lstTemp[i].objM.Conditon != MonitoringType.Group.ToString() && lstTemp[i].objM.Conditon != MonitoringType.AndGroup.ToString() && lstTemp[i].objM.Conditon != MonitoringType.ORGroup.ToString())
                        {
                            if (string.IsNullOrEmpty(strCondition))
                            {
                                strCondition += "(" + lstTemp[i].objM.Element + "@#$" + lstTemp[i].objM.ConditonOpetator + "@#$" + lstTemp[i].objM.ConditonValue + " )";
                                SessionHelper.DandB_strCondition = strCondition;
                            }
                            else
                            {
                                strCondition += (lstTemp[i].objM.Conditon != null ? lstTemp[i].objM.Conditon : "") + "@#$" + "(" + lstTemp[i].objM.Element + "@#$" + lstTemp[i].objM.ConditonOpetator + "@#$" + lstTemp[i].objM.ConditonValue + " )";
                                SessionHelper.DandB_strCondition = strCondition;
                            }

                        }
                        else if (lstTemp[i].objM != null && lstTemp[i].objM.lstCondition != null && lstTemp[i].objM.lstCondition.Any())
                        {
                            lstTemp[i].objM.lstCondition[0].objM.Conditon = null;
                            strCondition += "(" + (lstTemp[i].objM.Conditon == "AndGroup" ? "And" : "OR");
                            SessionHelper.DandB_strCondition = strCondition;
                            CreateStringForMaterCondition(lstTemp[i].objM.lstCondition);
                            if (!string.IsNullOrEmpty(SessionHelper.DandB_strCondition))
                            {
                                strCondition = SessionHelper.DandB_strCondition;
                            }
                            strCondition += ")";
                            SessionHelper.DandB_strCondition = strCondition;
                        }
                    }
                }
            }
        }
        #endregion

        #region Get data for DropDown
        //Gel all Products Detail for DropDown List in Business view
        public static List<MonitoringProductEntity> GetAllProductData(string ConnectionString)
        {
            MonitorProfileFacade mfac = new MonitorProfileFacade(ConnectionString);
            List<MonitoringProductEntity> lstProductElement = mfac.getProductList();
            return lstProductElement;
        }
        //Gel all Products Element Detail for DropDown List in Business Condition view
        public List<MonitoringProductElementEntity> GetAllProductElement(List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity, string ProductElementID, string ConnectionString)
        {
            // for Dropdown in add element condition page set all Element product wise.
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            MonitorProfileFacade mfac = new MonitorProfileFacade(ConnectionString);
            List<MonitoringProductElementEntity> lstProductElement = mfac.GetProductElementData(Helper.ProductId);
            // Also check if Element is use for one time in current monitoring profile than remove form the list.
            if (lstProductElement != null && lstProductElement.Any() && lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
            {
                for (int i = 0; i < lstProductElement.Count; i++)
                {
                    foreach (var item in lstMonitoringElemetsEntity)
                    {
                        if (item.ElementName == lstProductElement[i].ElementName)
                        {
                            if (Convert.ToInt32(ProductElementID) == 0)
                            {
                                lstProductElement.RemoveAt(i);
                            }
                            else if (lstProductElement[i].ProductElementID != Convert.ToInt32(ProductElementID))
                            {
                                lstProductElement.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            return lstProductElement;
        }

        public static List<int> GetPageSize()
        {
            // Get Page size of Monitoring Profile to display on index or listing page.
            List<int> lstsize = new List<int>();
            lstsize.Add(10);
            lstsize.Add(20);
            lstsize.Add(30);
            return lstsize;
        }
        [RequestFromSameDomain, RequestFromAjax]
        public JsonResult CheckBusinessCondition(int ParentElemets, string ParentCodition, string BusinessCondionUpdateId)
        {
            SessionHelper.lstTempMonirtoring = string.Empty;
            return new JsonResult { Data = "Success" };
        }
        [RequestFromSameDomain, RequestFromAjax]
        public JsonResult BussnessConditions(string Parameters)
        {
            int ProductElementId = 0; string SetId = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ProductElementId = String.IsNullOrEmpty(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1)) ? 0 : Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                SetId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
            }

            // Select Element and accursing to element set condition for dropdown
            if (!string.IsNullOrEmpty(SetId) && Convert.ToInt32(SetId) > 0)
            {
                SessionHelper.lstTempMonirtoring = string.Empty;
                SessionHelper.ConditionCount = "0";
            }
            List<SelectListItem> lstAllBussnessElements = new List<SelectListItem>();

            lstAllBussnessElements.Add(new SelectListItem { Value = "-1", Text = "Select Condition" });
            if (ProductElementId > 0)
            {
                MonitorProfileFacade mfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                MonitoringProductElementEntity objMPE = mfac.GetProductElementByID(ProductElementId);
                string Condition = objMPE.ElementType;
                lstAllBussnessElements.Add(new SelectListItem { Value = "AnyChange", Text = "AnyChange" });
                if (Condition.ToLower() == "string" && objMPE.MontoringType.ToLower() == "multichange")
                {
                    Helper.ElementType = "string";
                    lstAllBussnessElements.Add(new SelectListItem { Value = "MultiCondition", Text = "MultiCondition" });
                }
                else if (Condition.ToLower() == "numeric" && objMPE.MontoringType.ToLower() == "multichange")
                {
                    Helper.ElementType = "numeric";
                    lstAllBussnessElements.Add(new SelectListItem { Value = "MultiCondition", Text = "MultiCondition" });
                }
            }
            return new JsonResult { Data = new { lstAllBussnessElements = lstAllBussnessElements, ElementType = Helper.ElementType } };
        }
        #endregion

        #region Cancel Monitoring Profile
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CancelMonitoringProfile()
        {
            //Clear Data and set Default value for all.
            SessionHelper.lstMonitoringElemetsEntity = string.Empty;
            SessionHelper.lstBusinessElementssConditon = string.Empty;
            SessionHelper.lstTempMonirtoring = string.Empty;
            SessionHelper.lstMonirtoringTemp = string.Empty;
            SessionHelper.DandB_strCondition = string.Empty;
            SessionHelper.objMTM = string.Empty;
            Helper.ProfileId = 0;
            Helper.ProductId = 0;
            Helper.ProductCode = string.Empty;
            return new JsonResult { Data = "success" };
        }
        #endregion

        [HttpPost, RequestFromSameDomain]
        public string IsValidProfileName(string ProfileName, string ProductCode, string MonitoringLevel)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (!string.IsNullOrEmpty(SessionHelper.lstMonitoringElemetsEntity))
            {
                lstMonitoringElemetsEntity = JsonConvert.DeserializeObject<List<MonitoringElementConditionsEntity>>(SessionHelper.lstMonitoringElemetsEntity);
            }
            MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<MonitoringProfileEntity> lstMPE = Mpfac.GetAllMonitoringProfile(Helper.DefaultMornitoring20Credential);
            string strResion = "";
            string strProfileName = "";

            if (lstMPE != null && lstMPE.Count > 0)
            {
                if (Helper.ProfileId > 0)
                {
                    strProfileName = lstMPE.Where(x => x.ProfileName.ToLower() == ProfileName.ToLower() && x.MonitoringProfileID != Helper.ProfileId).Select(x => x.ProfileName).FirstOrDefault();
                }
                else
                {
                    strProfileName = lstMPE.Where(x => x.ProfileName.ToLower() == ProfileName.ToLower()).Select(x => x.ProfileName).FirstOrDefault();
                }

                int ProfileId = lstMPE.Where(x => x.ProductCode == ProductCode && x.MonitoringLevel == MonitoringLevel).Select(x => x.MonitoringProfileID).FirstOrDefault();
                if (!string.IsNullOrEmpty(strProfileName))
                {
                    strResion = "Name";
                    return strResion;
                }
                if (ProfileId > 0)
                {
                    if (lstMonitoringElemetsEntity.Count == 0)
                    {
                        strResion = "ProfileCode";
                        return strResion;
                    }
                    else
                    {
                        List<MonitoringElementConditionsEntity> lstME = new List<MonitoringElementConditionsEntity>();
                        if (Helper.ProfileId > 0)
                        {

                            lstME = lstMonitoringElemetsEntity.Where(x => x.ProfileID != Helper.ProfileId).GroupBy(x => x.ElementName).Select(g => g.First()).ToList();
                        }
                        else
                        {
                            lstME = lstMonitoringElemetsEntity.GroupBy(x => x.ElementName).Select(g => g.First()).ToList();
                        }
                        foreach (var item in lstMPE)
                        {
                            List<MonitoringElementConditionsEntity> lstTME = Mpfac.GetMonitoringElementConditionsByProfileId(item.MonitoringProfileID).GroupBy(x => x.ElementName).Select(g => g.First()).ToList();
                            int count = 0;
                            if (lstTME != null && lstTME.Any() && lstME.Count == lstTME.Count)
                            {
                                foreach (var elements in lstTME)
                                {
                                    foreach (var child in lstME)
                                    {
                                        if (elements.ElementName == child.ElementName && elements.ChangeCondition == child.ChangeCondition)
                                        {
                                            count += 1;
                                        }

                                    }
                                }
                            }
                            if (count > 0 && count == lstME.Count)
                            {
                                strResion = "Elements";
                                return strResion;
                            }
                        }
                    }
                }

            }
            return strResion;
        }
        public bool CheckMonitoringProfileUsed(int ProfileId)
        {
            DUNSregistrationFacade Dfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            return Dfac.CheckMonitoringProfileUsed(ProfileId);
        }
        #endregion

        #region User Prefrence

        #region List User Preference
        public ActionResult IndexUserPreference()
        {
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<UserPreferenceEntity> lstuserPrefrnc = new List<UserPreferenceEntity>();
            lstuserPrefrnc = UPFac.GetUserPreference(Helper.DefaultMornitoring20Credential);
            
            UserPreferenceEntity obj = new UserPreferenceEntity();
            string indexUserPrefrnc = string.Empty;
            string upsertUserPrefrnc = string.Empty;
            if (lstuserPrefrnc != null && lstuserPrefrnc.Count > 0)
            {
                obj = lstuserPrefrnc.FirstOrDefault();
                obj.PreferenceValue = "dnbnotifications@matchbookservices.com";
                indexUserPrefrnc = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexUserPreference.cshtml", lstuserPrefrnc);
                upsertUserPrefrnc = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateUserPreference.cshtml", obj);
            }
            else
            {
                indexUserPrefrnc = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexUserPreference.cshtml", null);
                upsertUserPrefrnc = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateUserPreference.cshtml", obj);
            }
            return Json(new { result = true, indexUserPrefrnc = indexUserPrefrnc, upsertUserPrefrnc = upsertUserPrefrnc }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Snyc User Preference
        [HttpPost, RequestFromSameDomain, RequestFromAjax]
        public JsonResult GetUserPrefrenceList()
        {
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
            if (Direct20Auth != null)
            {
                string endPoint = string.Format(DnBApi.UserPrefrenceLis, "1000", SyncCurrentUserPreferenceCount);
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);
                List<string> lstPreference = new List<string>();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        string finalResult = result.Replace("\"$\"", "\"_param\"");
                        ListPreference objResponse = JsonConvert.DeserializeObject<ListPreference>(finalResult);
                        if (objResponse != null && objResponse.ListPreferenceResponse != null && objResponse.ListPreferenceResponse.ListPreferenceResponseDetail != null && objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.PreferenceDetail != null && objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.PreferenceDetail.Any())
                        {
                            SyncUserPreferenceCount = objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.CandidateMatchedQuantity;
                            SyncCurrentUserPreferenceCount += objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.CandidateReturnedQuantity;
                            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
                            List<UserPreferenceEntity> lstUserPreference = UPFac.GetAllUserPreference();
                            if (!string.IsNullOrEmpty(SessionHelper.lstPreference))
                            {
                                lstPreference = JsonConvert.DeserializeObject<List<string>>(SessionHelper.lstPreference);
                            }
                            foreach (var item in objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.PreferenceDetail)
                            {
                                lstPreference.Add(item.PreferenceName);
                                UserPreferenceEntity objUserPreferenceExists = lstUserPreference.FirstOrDefault(x => x.PreferenceName == item.PreferenceName);
                                UserPreferenceEntity objUserPreference = new UserPreferenceEntity();
                                if (objUserPreferenceExists != null && objUserPreferenceExists.PreferenceID > 0)
                                {
                                    objUserPreference.PreferenceID = objUserPreferenceExists.PreferenceID;
                                }
                                objUserPreference.PreferenceName = item.PreferenceName;
                                objUserPreference.PreferenceDescription = item.PreferenceDescription;
                                objUserPreference.PreferenceType = item.PreferenceType;
                                objUserPreference.PreferenceValue = item.PreferenceValueText;
                                objUserPreference.DefaultPreference = Convert.ToBoolean(item.DefaultPreference);
                                objUserPreference.ApplicationAreaName = item.ApplicationAreaName;
                                objUserPreference.ResultID = objResponse.ListPreferenceResponse.TransactionResult.ResultID;
                                objUserPreference.SeverityText = objResponse.ListPreferenceResponse.TransactionResult.SeverityText;
                                objUserPreference.ResultText = objResponse.ListPreferenceResponse.TransactionResult.ResultText;
                                objUserPreference.RequestDateTime = Convert.ToDateTime(objResponse.ListPreferenceResponse.TransactionDetail.TransactionTimestamp);
                                objUserPreference.ResponseDateTime = Convert.ToDateTime(objResponse.ListPreferenceResponse.TransactionDetail.TransactionTimestamp);

                                objUserPreference.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId().ToString());
                                objUserPreference.CreateDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                                objUserPreference.ModifiedDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                                objUserPreference.IsDeleted = item.PreferenceStatusText == "Active";
                                objUserPreference.CredentialId = Helper.DefaultMornitoring20Credential;
                                UPFac.InsertUpdateUserPreference(objUserPreference);
                            }

                            lstUserPreference = UPFac.GetAllUserPreference();
                            List<string> APIPreferenceDetail = lstPreference.Distinct().OrderByDescending(s => s).ToList();
                            List<string> PreferenceDetail = lstUserPreference.Select(i => i.PreferenceName).Distinct().OrderByDescending(s => s).ToList();
                            var DiffUserPreference = PreferenceDetail.Except(APIPreferenceDetail);
                            if (DiffUserPreference != null)
                            {
                                foreach (var item in DiffUserPreference)
                                {
                                    UserPreferenceEntity objUserPreferenceExists = lstUserPreference.FirstOrDefault(x => x.PreferenceName == item);
                                    objUserPreferenceExists.IsDeleted = true;
                                    UPFac.InsertUpdateUserPreference(objUserPreferenceExists);// Maintain flag if any unwanted records comes.
                                }
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        SessionHelper.DandB_ResponseErroeMessage = CommonMessagesLang.msgCommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            ListPreference objResponse = serializer.Deserialize<ListPreference>(result);
                            if (objResponse != null && objResponse.ListPreferenceResponse != null && objResponse.ListPreferenceResponse.TransactionResult != null)
                                SessionHelper.DandB_ResponseErroeMessage = objResponse.ListPreferenceResponse.TransactionResult.ResultText;
                        }
                        return new JsonResult { Data = "success" };
                    }
                }
                if (SyncCurrentUserPreferenceCount >= SyncUserPreferenceCount)
                {
                    return new JsonResult { Data = "success" };
                }
                else
                {
                    SessionHelper.lstPreference = JsonConvert.SerializeObject(lstPreference);
                    return GetUserPrefrenceList();
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSetDefaultCredentialForMonitoring };
            }
        }
        #endregion

        #region Insert Update User Preference
        public ActionResult AddUpdateUserPreference(string Parameters)
        {
            UserPreferenceEntity obj = new UserPreferenceEntity();
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            obj.PreferenceValue = "dnbnotifications@matchbookservices.com";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = UPFac.GetUserPreferenceById(Convert.ToInt32(Parameters));
            }
            return PartialView(obj);
        }
        [HttpPost, RequestFromSameDomain, ValidateInput(true), ValidateAntiForgeryToken]
        public ActionResult AddUpdateUserPreference(UserPreferenceEntity obj, string btnUserPreference)
        {
            obj.PreferenceType = "Email";
            obj.ApplicationAreaName = "Monitoring";
            int PreferenceId = 0;
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (CheckUserPreferenceName(obj.PreferenceName.Trim(), obj.PreferenceID))
            {
                return Json(new { result = false, message = DandBSettingLang.msgDuplicateNameUserPreference }, JsonRequestBehavior.AllowGet);
            }
            obj.CreatedBy = Convert.ToInt32(User.Identity.GetUserId());
            obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            #region Api call
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);

            string endPoint = DnBApi.UserpreferenceUrl;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", Direct20Auth?.AuthToken);
            if (obj.PreferenceID == 0)
            {
                UserPreferenceRequest root = CreatePrefObject(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                data = data.Replace("xmlnsuser", "@xmlns$user").Replace("CreatePreferenceRequestUser", "user:CreatePreferenceRequest").Replace("TransactionDetailUser", "user:TransactionDetail").Replace("ApplicationTransactionID", "user:ApplicationTransactionID").Replace("PreferenceRequestDetailUser", "user:PreferenceRequestDetail").Replace("PreferenceSpecificationUser", "user:PreferenceSpecification").Replace("PreferenceTypeUser", "user:PreferenceType").Replace("ApplicationAreaNameUser", "user:ApplicationAreaName").Replace("PreferenceNameUser", "user:PreferenceName").Replace("PreferenceDescriptionUser", "user:PreferenceDescription").Replace("PreferenceValueTextUser", "user:PreferenceValueText").Replace("DefaultPreferenceUser", "user:DefaultPreference");
                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        UserPreferenceReponse objResponse = serializer.Deserialize<UserPreferenceReponse>(result);
                        if (objResponse != null)
                        {
                            obj.ResultID = objResponse.CreatePreferenceResponse.TransactionResult.ResultID;
                            obj.SeverityText = objResponse.CreatePreferenceResponse.TransactionResult.SeverityText;
                            obj.ResultText = objResponse.CreatePreferenceResponse.TransactionResult.ResultText;
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.IsDeleted = false;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            PreferenceId = UPFac.InsertUpdateUserPreference(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            UserPreferenceReponse objResponse = JsonConvert.DeserializeObject<UserPreferenceReponse>(result);
                            if (objResponse != null && objResponse.CreatePreferenceResponse != null && objResponse.CreatePreferenceResponse.TransactionResult != null && objResponse.CreatePreferenceResponse.TransactionResult.ResultText != null)
                            {
                                return Json(new { result = true, message = objResponse.CreatePreferenceResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                UserPreferenceUpdateRequest root = UpdatePrefObject(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                data = data.Replace("xmlns", "@xmlns");
                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        UserPreferenceUpdateResponse objResponse = serializer.Deserialize<UserPreferenceUpdateResponse>(result);
                        if (objResponse != null)
                        {
                            obj.ResultID = objResponse.UpdatePreferenceResponse.TransactionResult.ResultID;
                            obj.SeverityText = objResponse.UpdatePreferenceResponse.TransactionResult.SeverityText;
                            obj.ResultText = objResponse.UpdatePreferenceResponse.TransactionResult.ResultText;
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.IsDeleted = false;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            PreferenceId = UPFac.InsertUpdateUserPreference(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            UserPreferenceReponse objResponse = JsonConvert.DeserializeObject<UserPreferenceReponse>(result);
                            if (objResponse != null && objResponse.CreatePreferenceResponse != null && objResponse.CreatePreferenceResponse.TransactionResult != null && objResponse.CreatePreferenceResponse.TransactionResult.ResultText != null)
                            {
                                return Json(new { result = false, message = objResponse.CreatePreferenceResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }

                    }
                }
            }
            #endregion
            if (PreferenceId > 0)
            {
                return Json(new { result = true, message = obj.PreferenceID > 0 ? DandBSettingLang.msgUpdateUserPreference : DandBSettingLang.msgInertUserPreference }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        private UserPreferenceRequest CreatePrefObject(UserPreferenceEntity objUserPreference)
        {
            UserPreferenceRequest objRequest = new UserPreferenceRequest();
            UserCreatePreferenceRequest objPrefRequest = new UserCreatePreferenceRequest();
            objRequest.CreatePreferenceRequestUser = objPrefRequest;

            objPrefRequest.xmlnsuser = DnBApi.UserPreferenceService;

            TransactionDetail objDetail = new TransactionDetail();
            objDetail.ApplicationTransactionID = "Pref_" + GetDigits();

            objPrefRequest.TransactionDetailUser = objDetail;

            UserPreferenceRequestDetail objRequestPrefDetail = new UserPreferenceRequestDetail();
            objPrefRequest.PreferenceRequestDetailUser = objRequestPrefDetail;

            UserPreferenceSpecification objPrefSpecification = new UserPreferenceSpecification();
            objRequestPrefDetail.PreferenceSpecificationUser = objPrefSpecification;

            objPrefSpecification.ApplicationAreaNameUser = objUserPreference.ApplicationAreaName;
            objPrefSpecification.DefaultPreferenceUser = Convert.ToString(objUserPreference.DefaultPreference).ToLower();
            objPrefSpecification.PreferenceDescriptionUser = objUserPreference.PreferenceDescription;
            objPrefSpecification.PreferenceNameUser = objUserPreference.PreferenceName.Trim();
            objPrefSpecification.PreferenceTypeUser = objUserPreference.PreferenceType;  // hardcode for now
            objPrefSpecification.PreferenceValueTextUser = objUserPreference.PreferenceValue;

            return objRequest;
        }
        private UserPreferenceUpdateRequest UpdatePrefObject(UserPreferenceEntity objUserPreference)
        {
            UserPreferenceUpdateRequest objRequest = new UserPreferenceUpdateRequest();
            UpdatePreferenceRequest objUpdatePrefRequest = new UpdatePreferenceRequest();
            objRequest.UpdatePreferenceRequest = objUpdatePrefRequest;
            objUpdatePrefRequest.xmlns = DnBApi.UserPreferenceService;

            TransactionDetail objTrans = new TransactionDetail();
            objUpdatePrefRequest.TransactionDetail = objTrans;

            UpdatePreferenceRequestDetail objReqDetail = new UpdatePreferenceRequestDetail();
            objUpdatePrefRequest.UpdatePreferenceRequestDetail = objReqDetail;

            PreferenceDetail objDetail = new PreferenceDetail();
            objDetail.ApplicationAreaName = objUserPreference.ApplicationAreaName;
            objDetail.PreferenceType = objUserPreference.PreferenceType;
            objDetail.PreferenceName = objUserPreference.PreferenceOldName.Trim();
            objReqDetail.PreferenceDetail = objDetail;

            PreferenceUpdateSpecification objPrefSpecification = new PreferenceUpdateSpecification();
            objPrefSpecification.ApplicationAreaName = objUserPreference.ApplicationAreaName; // hardcode for now
            objPrefSpecification.DefaultPreference = Convert.ToString(objUserPreference.DefaultPreference).ToLower();
            objPrefSpecification.PreferenceName = objUserPreference.PreferenceName.Trim();
            objPrefSpecification.PreferenceValueText = objUserPreference.PreferenceValue;
            objPrefSpecification.PreferenceStatusText = "Active";
            objReqDetail.PreferenceUpdateSpecification = objPrefSpecification;

            return objRequest;
        }
        #endregion

        #region Delete User Preference
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteUserPrefrence(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {
                int id = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
                UserPreferenceEntity obj = UPFac.GetUserPreferenceById(Convert.ToInt32(Convert.ToInt32(id)));
                if (CheckUserPreferenceUsed(obj.PreferenceName))
                {
                    return new JsonResult { Data = DandBSettingLang.msgUsedUserPreference };
                }
                ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
                ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
                #region Api call
                string endPoint = DnBApi.UserpreferenceUrl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);

                UserPreferenceUpdateRequest root = DeletePrefRequestObject(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                data = data.Replace("xmlns", "@xmlns");

                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        UserPreferenceUpdateResponse objResponse = serializer.Deserialize<UserPreferenceUpdateResponse>(result);
                        if (objResponse != null)
                        {
                            obj.ResultID = objResponse.UpdatePreferenceResponse.TransactionResult.ResultID;
                            obj.SeverityText = objResponse.UpdatePreferenceResponse.TransactionResult.SeverityText;
                            obj.ResultText = objResponse.UpdatePreferenceResponse.TransactionResult.ResultText;
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.IsDeleted = true;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            UPFac.InsertUpdateUserPreference(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            UserPreferenceUpdateResponse objResponse = serializer.Deserialize<UserPreferenceUpdateResponse>(result);
                            if (objResponse != null && objResponse.UpdatePreferenceResponse != null && objResponse.UpdatePreferenceResponse.TransactionResult != null)
                            {
                                return Json(new { result = false, message = objResponse.UpdatePreferenceResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
        }
        private UserPreferenceUpdateRequest DeletePrefRequestObject(UserPreferenceEntity objUserPreference)
        {
            UserPreferenceUpdateRequest objRequest = new UserPreferenceUpdateRequest();
            UpdatePreferenceRequest objUpdatePrefRequest = new UpdatePreferenceRequest();
            objRequest.UpdatePreferenceRequest = objUpdatePrefRequest;
            objUpdatePrefRequest.xmlns = DnBApi.UserPreferenceService;

            TransactionDetail objTrans = new TransactionDetail();
            objUpdatePrefRequest.TransactionDetail = objTrans;

            UpdatePreferenceRequestDetail objReqDetail = new UpdatePreferenceRequestDetail();
            objUpdatePrefRequest.UpdatePreferenceRequestDetail = objReqDetail;

            PreferenceDetail objDetail = new PreferenceDetail();
            objDetail.ApplicationAreaName = objUserPreference.ApplicationAreaName;
            objDetail.PreferenceType = objUserPreference.PreferenceType;
            objDetail.PreferenceName = objUserPreference.PreferenceName;
            objReqDetail.PreferenceDetail = objDetail;

            PreferenceUpdateSpecification objPrefSpecification = new PreferenceUpdateSpecification();
            objPrefSpecification.ApplicationAreaName = objUserPreference.ApplicationAreaName; // hardcode for now
            objPrefSpecification.DefaultPreference = Convert.ToString(objUserPreference.DefaultPreference).ToLower();
            objPrefSpecification.PreferenceName = objUserPreference.PreferenceName;
            objPrefSpecification.PreferenceValueText = objUserPreference.PreferenceValue;
            objPrefSpecification.PreferenceStatusText = "Cancelled";
            objReqDetail.PreferenceUpdateSpecification = objPrefSpecification;
            return objRequest;
        }
        public bool CheckUserPreferenceUsed(string UserPreference)
        {
            UserPreferenceFacade Ufac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            return Ufac.CheckUserPreferenceUsed(UserPreference);
        }
        #endregion

        #region Others
        public bool CheckUserPreferenceName(string UserPreferenceName, int Id)
        {
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            return UPFac.CheckUserPreferenceName(Id, UserPreferenceName);
        }
        #endregion

        #endregion

        #region Notification Profile
        #region List Notification
        public ActionResult IndexNotificationProfile()
        {
            NotificationProfileFacade NPfac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<NotificationProfileEntity> lstNotificationProfile = new List<NotificationProfileEntity>();
            lstNotificationProfile = NPfac.GetNotificationProfile(Helper.DefaultMornitoring20Credential);

            NotificationProfileEntity obj = new NotificationProfileEntity();
            string indexNotificationProfile = string.Empty;
            string upsertNotificationProfile = string.Empty;
            ViewBag.Frequency = GetFrequency();
            ViewBag.ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            ViewBag.UserPreferences = GetUserPreferenceName(this.CurrentClient.ApplicationDBConnectionString);
            if (lstNotificationProfile != null && lstNotificationProfile.Count > 0)
            {
                obj = lstNotificationProfile.FirstOrDefault();
                obj = obj == null ? new NotificationProfileEntity() : obj;
                indexNotificationProfile = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexNotificationProfile.cshtml", lstNotificationProfile);
                upsertNotificationProfile = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateNotificationProfile.cshtml", obj);
            }
            else
            {
                indexNotificationProfile = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexNotificationProfile.cshtml", null);
                upsertNotificationProfile = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateNotificationProfile.cshtml", obj);
            }
            return Json(new { result = true, indexNotificationProfile = indexNotificationProfile, upsertNotificationProfile = upsertNotificationProfile }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sync Notification
        [HttpPost, RequestFromAjax, RequestFromSameDomain]
        public JsonResult GetNotificationList()
        {
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
            if (Direct20Auth != null)
            {
                string endPoint = string.Format(DnBApi.NotificationList, "1000", SyncCurrentMonitoringProfileCount); // put in common file
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        string finalResult = result.Replace("\"$\"", "\"_param\"");
                        ListNotification objResponse = JsonConvert.DeserializeObject<ListNotification>(finalResult);
                        if (objResponse != null && objResponse.ListNotificationProfileResponse != null && objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail != null && objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail.NotificationProfileDetail != null && objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail.NotificationProfileDetail.Any())
                        {
                            SyncNotificationCount = objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail.CandidateMatchedQuantity;
                            SyncCurrentNotificationCount += objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail.CandidateReturnedQuantity;
                            NotificationProfileFacade NFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                            foreach (var item in objResponse.ListNotificationProfileResponse.ListNotificationProfileResponseDetail.NotificationProfileDetail)
                            {
                                NotificationProfileEntity objNotification = new NotificationProfileEntity();
                                objNotification.NotificationProfileName = item.NotificationProfileName;
                                objNotification.NotificationProfileDescription = item.NotificationProfileDescription;
                                objNotification.DeliveryMode = item.DeliveryMode;
                                objNotification.DeliveryChannelUserPreferenceName = item.DeliveryChannelUserPreferenceName;
                                objNotification.DeliveryFrequency = item.DeliveryFrequency;
                                objNotification.DeliveryFormat = item.DeliveryFormat;
                                objNotification.StopDeliveryIndicator = Convert.ToBoolean(item.StopDeliveryIndicator);
                                objNotification.CompressedProductIndicator = false;
                                objNotification.InquiryReferenceText = Convert.ToString(item.InquiryReferenceText);
                                objNotification.ResultID = objResponse.ListNotificationProfileResponse.TransactionResult.ResultID;
                                objNotification.SeverityText = objResponse.ListNotificationProfileResponse.TransactionResult.SeverityText;
                                objNotification.ResultText = objResponse.ListNotificationProfileResponse.TransactionResult.ResultText;
                                objNotification.NotificationProfileID = Convert.ToInt32(item.NotificationProfileID);
                                objNotification.RequestDateTime = Convert.ToDateTime(objResponse.ListNotificationProfileResponse.TransactionDetail.TransactionTimestamp);
                                objNotification.ResponseDateTime = Convert.ToDateTime(objResponse.ListNotificationProfileResponse.TransactionDetail.TransactionTimestamp);
                                objNotification.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId().ToString());
                                objNotification.CreatedDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                                objNotification.ModifiedDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                                objNotification.IsDeleted = item.NotificationProfileStatusText == "Active";
                                objNotification.CredentialId = Helper.DefaultMornitoring20Credential;
                                NFac.InsertNotificationProfile(objNotification);
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        SessionHelper.DandB_ResponseErroeMessage = CommonMessagesLang.msgCommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            ListNotification objResponse = serializer.Deserialize<ListNotification>(result);
                            if (objResponse != null && objResponse.ListNotificationProfileResponse != null && objResponse.ListNotificationProfileResponse.TransactionResult != null)
                                SessionHelper.DandB_ResponseErroeMessage = objResponse.ListNotificationProfileResponse.TransactionResult.ResultText;
                        }
                        return new JsonResult { Data = "success" };
                    }
                }
                if (SyncCurrentNotificationCount >= SyncNotificationCount)
                {
                    return new JsonResult { Data = "success" };
                }
                else
                {
                    return GetNotificationList();
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgSetDefaultCredentialForMonitoring };
            }
        }
        #endregion

        #region Insert Update Notification
        public ActionResult AddUpdateNotificationProfile(string Parameters)
        {
            NotificationProfileEntity obj = new NotificationProfileEntity();
            NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = NPFac.GetNotificationProfileById(Convert.ToInt32(Parameters));
            }
            ViewBag.Frequency = GetFrequency();
            ViewBag.ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            ViewBag.UserPreferences = GetUserPreferenceName(this.CurrentClient.ApplicationDBConnectionString);
            return PartialView("_addUpdateNotificationProfile", obj);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult AddUpdateNotificationProfile(NotificationProfileEntity obj, string btnNotificationProfile)
        {
            int ProfileId = 0;
            obj.DeliveryMode = "Email";
            obj.DeliveryFormat = "XML";
            obj.CreatedBy = Convert.ToInt32(User.Identity.GetUserId());
            obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (CheckNotification(obj.NotificationProfileName.Trim(), obj.NotificationProfileID))
            {
                return Json(new { result = false, message = DandBSettingLang.msgDuplicateNotification }, JsonRequestBehavior.AllowGet);
            }

            #region Api Call
            ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
            ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);


            if (obj.NotificationProfileID == 0)
            {
                // Actual call
                string endPoint = DnBApi.NotificationUrl;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);

                NotificationRequest root = CreateNotificationObject(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                   Newtonsoft.Json.Formatting.None,
                                   new JsonSerializerSettings
                                   {
                                       NullValueHandling = NullValueHandling.Ignore
                                   });

                data = data.Replace("xmlnsmon", "@xmlns$mon").Replace("MonCreateNotificationProfileRequest", "mon:CreateNotificationProfileRequest");

                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        NotificationResponse objResponse = serializer.Deserialize<NotificationResponse>(result);
                        if (objResponse != null)
                        {
                            obj.InquiryReferenceText = objResponse.CreateNotificationProfileResponse.CreateNotificationProfileResponseDetail.InquiryReferenceText.CustomerReferenceText[0];
                            obj.ResultID = objResponse.CreateNotificationProfileResponse.TransactionResult.ResultID;
                            obj.ResultText = objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText;
                            obj.SeverityText = objResponse.CreateNotificationProfileResponse.TransactionResult.SeverityText;
                            obj.NotificationProfileID = Convert.ToInt32(objResponse.CreateNotificationProfileResponse.CreateNotificationProfileResponseDetail.NotificationProfileDetail.NotificationProfileID);
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.IsDeleted = false;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            ProfileId = NPFac.InsertNotificationProfile(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = CommonMessagesLang.msgCommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationResponse objResponse = serializer.Deserialize<NotificationResponse>(result);
                            if (objResponse != null && objResponse.CreateNotificationProfileResponse != null && objResponse.CreateNotificationProfileResponse.TransactionResult != null && objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText != null)
                            {
                                return Json(new { result = false, message = objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            else
            {
                string endPoint = DnBApi.NotificationUrl + "/" + obj.NotificationProfileID;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);

                NotificationUpdateRequest root = UpdateNotificationObjct(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });

                data = data.Replace("xmlnsmon", "@xmlns$mon").Replace("MonUpdateNotificationProfileRequest", "mon:UpdateNotificationProfileRequest");
                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                        if (objResponse != null)
                        {
                            obj.InquiryReferenceText = objResponse.UpdateNotificationProfileResponse.UpdateNotificationProfileResponseDetail.InquiryReferenceText.CustomerReferenceText[0];
                            obj.ResultID = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultID;
                            obj.ResultText = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText;
                            obj.SeverityText = objResponse.UpdateNotificationProfileResponse.TransactionResult.SeverityText;
                            obj.NotificationProfileID = Convert.ToInt32(objResponse.UpdateNotificationProfileResponse.UpdateNotificationProfileResponseDetail.NotificationProfileDetail.NotificationProfileID);
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.ModifiedBy = Convert.ToInt32(User.Identity.GetUserId());
                            obj.IsDeleted = false;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            ProfileId = NPFac.InsertNotificationProfile(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = CommonMessagesLang.msgCommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                            if (objResponse != null && objResponse.UpdateNotificationProfileResponse != null && objResponse.UpdateNotificationProfileResponse.TransactionResult != null && objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText != null)
                            {
                                return Json(new { result = false, message = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            #endregion
            if (ProfileId > 0)
            {
                return Json(new { result = true, message = obj.NotificationProfileID > 0 ? DandBSettingLang.msgUpdateNotification : DandBSettingLang.msgInsertNotification }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
        }

        private NotificationRequest CreateNotificationObject(NotificationProfileEntity objNotification)
        {
            NotificationRequest objRequest = new NotificationRequest();
            MonCreateNotificationProfileRequest objNotiRequest = new MonCreateNotificationProfileRequest();
            objRequest.MonCreateNotificationProfileRequest = objNotiRequest;

            objNotiRequest.xmlnsmon = DnBApi.MonitoingService;

            TransactionDetail objDetail = new TransactionDetail();
            objDetail.ApplicationTransactionID = "Not_" + GetDigits();
            objNotiRequest.TransactionDetail = objDetail;

            CreateNotificationProfileRequestDetail objRequestNotificationDetail = new CreateNotificationProfileRequestDetail();
            objNotiRequest.CreateNotificationProfileRequestDetail = objRequestNotificationDetail;

            NotificationProfileSpecification objNotificationProfileSpecification = new NotificationProfileSpecification();
            objRequestNotificationDetail.NotificationProfileSpecification = objNotificationProfileSpecification;

            objNotificationProfileSpecification.NotificationProfileName = objNotification.NotificationProfileName.Trim();
            objNotificationProfileSpecification.NotificationProfileDescription = objNotification.NotificationProfileDescription;
            objNotificationProfileSpecification.DeliveryChannelUserPreferenceName = objNotification.DeliveryChannelUserPreferenceName; // this is from USerPref table
            objNotificationProfileSpecification.DeliveryFormat = objNotification.DeliveryFormat;
            objNotificationProfileSpecification.DeliveryMode = objNotification.DeliveryMode;
            objNotificationProfileSpecification.DeliveryFrequency = objNotification.DeliveryFrequency;
            objNotificationProfileSpecification.StopDeliveryIndicator = Convert.ToString(objNotification.StopDeliveryIndicator).ToLower();
            if (!string.IsNullOrEmpty(objNotification.InquiryReferenceText))
            {
                objRequestNotificationDetail.InquiryReferenceText = new InquiryReferenceText { CustomerReferenceText = new List<string> { objNotification.InquiryReferenceText } };
            }
            else { objRequestNotificationDetail.InquiryReferenceText = null; }


            return objRequest;
        }

        private NotificationUpdateRequest UpdateNotificationObjct(NotificationProfileEntity objNotification)
        {
            NotificationUpdateRequest objRequest = new NotificationUpdateRequest();
            MonUpdateNotificationProfileRequest objNotiRequest = new MonUpdateNotificationProfileRequest();
            objRequest.MonUpdateNotificationProfileRequest = objNotiRequest;

            objNotiRequest.xmlnsmon = DnBApi.MonitoingService;

            TransactionDetail objDetail = new TransactionDetail();

            objNotiRequest.TransactionDetail = objDetail;

            UpdateNotificationProfileRequestDetail objRequestNotificationDetail = new UpdateNotificationProfileRequestDetail();
            objNotiRequest.UpdateNotificationProfileRequestDetail = objRequestNotificationDetail;

            NotificationProfileDetail objNotificationProfileDetail = new NotificationProfileDetail();
            objRequestNotificationDetail.NotificationProfileDetail = objNotificationProfileDetail;

            objNotificationProfileDetail.NotificationProfileName = objNotification.NotificationProfileName.Trim();
            objNotificationProfileDetail.DeliveryChannelUserPreferenceName = objNotification.DeliveryChannelUserPreferenceName;
            objNotificationProfileDetail.DeliveryFormat = objNotification.DeliveryFormat;
            objNotificationProfileDetail.DeliveryMode = objNotification.DeliveryMode;
            objNotificationProfileDetail.DeliveryFrequency = objNotification.DeliveryFrequency;
            objNotificationProfileDetail.StopDeliveryIndicator = objNotification.StopDeliveryIndicator;
            objNotificationProfileDetail.NotificationProfileStatusText = "Active";
            if (!string.IsNullOrEmpty(objNotification.InquiryReferenceText))
            {
                objRequestNotificationDetail.InquiryReferenceText = new InquiryReferenceText { CustomerReferenceText = new List<string> { objNotification.InquiryReferenceText } }; new List<string>() { };
            }
            else { objRequestNotificationDetail.InquiryReferenceText = null; }
            return objRequest;
        }
        #endregion

        #region Delete Notification
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteNotificationProfile(string Parameters)
        {
            if (!string.IsNullOrEmpty(Parameters))
            {

                int id = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                if (CheckNotificationProfileUsed(id))
                {
                    return Json(new { result = false, message = DandBSettingLang.msgUsedNotification }, JsonRequestBehavior.AllowGet);
                }
                NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                NotificationProfileEntity obj = NPFac.GetNotificationProfileById(Convert.ToInt32(id));
                obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                ThirdPartyAPICredentialsFacade thirdAPIFac = new ThirdPartyAPICredentialsFacade(this.CurrentClient.ApplicationDBConnectionString);
                List<ThirdPartyAPICredentialsEntity> lstAuth = thirdAPIFac.GetThirdPartyAPICredentials(ThirdPartyProvider.DNB.ToString());
                ThirdPartyAPICredentialsEntity Direct20Auth = lstAuth.FirstOrDefault(x => x.APIType.ToLower() == ApiLayerType.Direct20.ToString().ToLower() && x.CredentialId == Helper.DefaultMornitoring20Credential);
                #region Api Call
                string endPoint = DnBApi.NotificationUrl + "/" + obj.NotificationProfileID;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Direct20Auth.AuthToken);

                NotificationUpdateRequest root = DeleteNotificationObject(obj);
                StreamWriter writer = new StreamWriter(httpWebRequest.GetRequestStream());

                string data = JsonConvert.SerializeObject(root,
                                  Newtonsoft.Json.Formatting.None,
                                  new JsonSerializerSettings
                                  {
                                      NullValueHandling = NullValueHandling.Ignore
                                  });
                data = data.Replace("xmlnsmon", "@xmlns$mon").Replace("MonUpdateNotificationProfileRequest", "mon:UpdateNotificationProfileRequest");
                writer.Write(data);
                writer.Close();
                try
                {
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        var serializer = new JavaScriptSerializer();
                        NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                        if (objResponse != null)
                        {
                            obj.InquiryReferenceText = objResponse.UpdateNotificationProfileResponse.UpdateNotificationProfileResponseDetail.InquiryReferenceText.CustomerReferenceText[0];
                            obj.ResultID = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultID;
                            obj.ResultText = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText;
                            obj.SeverityText = objResponse.UpdateNotificationProfileResponse.TransactionResult.SeverityText;
                            obj.NotificationProfileID = Convert.ToInt32(objResponse.UpdateNotificationProfileResponse.UpdateNotificationProfileResponseDetail.NotificationProfileDetail.NotificationProfileID);
                            obj.ResponseDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                            obj.IsDeleted = true;
                            obj.CredentialId = Helper.DefaultMornitoring20Credential;
                            NPFac.InsertNotificationProfile(obj);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                            if (objResponse != null && objResponse.UpdateNotificationProfileResponse != null && objResponse.UpdateNotificationProfileResponse.TransactionResult != null)
                            {
                                return Json(new { result = false, message = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                #endregion
                NPFac.DeleteNotificationProfile(Convert.ToInt32(id));

                return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        public NotificationUpdateRequest DeleteNotificationObject(NotificationProfileEntity objNotification)
        {
            NotificationUpdateRequest objRequest = new NotificationUpdateRequest();
            MonUpdateNotificationProfileRequest objNotiRequest = new MonUpdateNotificationProfileRequest();
            objRequest.MonUpdateNotificationProfileRequest = objNotiRequest;

            objNotiRequest.xmlnsmon = DnBApi.MonitoingService;

            TransactionDetail objDetail = new TransactionDetail();

            objNotiRequest.TransactionDetail = objDetail;

            UpdateNotificationProfileRequestDetail objRequestNotificationDetail = new UpdateNotificationProfileRequestDetail();
            objNotiRequest.UpdateNotificationProfileRequestDetail = objRequestNotificationDetail;

            NotificationProfileDetail objNotificationProfileDetail = new NotificationProfileDetail();
            objRequestNotificationDetail.NotificationProfileDetail = objNotificationProfileDetail;

            objNotificationProfileDetail.NotificationProfileName = objNotification.NotificationProfileName;
            objNotificationProfileDetail.DeliveryChannelUserPreferenceName = objNotification.DeliveryChannelUserPreferenceName;
            objNotificationProfileDetail.DeliveryFormat = objNotification.DeliveryFormat;
            objNotificationProfileDetail.DeliveryMode = objNotification.DeliveryMode;
            objNotificationProfileDetail.DeliveryFrequency = objNotification.DeliveryFrequency;
            objNotificationProfileDetail.NotificationProfileStatusText = "Cancelled";
            if (!string.IsNullOrEmpty(objNotification.InquiryReferenceText))
            {
                objRequestNotificationDetail.InquiryReferenceText = new InquiryReferenceText { CustomerReferenceText = new List<string> { objNotification.InquiryReferenceText } };
            }
            else { objRequestNotificationDetail.InquiryReferenceText = null; }

            return objRequest;
        }
        #endregion

        #region Others
        public static List<SelectListItem> GetUserPreferenceName(string ConnectionString = null)
        {
            List<SelectListItem> lstUserPreference = new List<SelectListItem>();
            UserPreferenceFacade UPFac = new UserPreferenceFacade(ConnectionString);
            List<UserPreferenceEntity> lstPreferences = UPFac.GetActiveUserPreference();
            if (lstPreferences == null)
                lstPreferences = new List<UserPreferenceEntity>();

            lstUserPreference.Add(new SelectListItem { Value = "", Text = "Select User Preference" });
            foreach (var item in lstPreferences)
            {
                lstUserPreference.Add(new SelectListItem() { Text = item.PreferenceName, Value = item.PreferenceName });
            }
            return lstUserPreference;
        }

        public static List<SelectListItem> GetFrequency()
        {
            List<SelectListItem> lstFrequencies = new List<SelectListItem>();
            lstFrequencies.Add(new SelectListItem { Value = "-1", Text = "Select Frequency" });
            lstFrequencies.Add(new SelectListItem { Value = "Immediate", Text = "Immediate" });
            lstFrequencies.Add(new SelectListItem { Value = "Hourly", Text = "Hourly" });
            lstFrequencies.Add(new SelectListItem { Value = "Daily", Text = "Daily" });
            lstFrequencies.Add(new SelectListItem { Value = "Weekly", Text = "Weekly" });
            return lstFrequencies;
        }

        public bool CheckNotification(string ProfileName, int Id)
        {
            NotificationProfileFacade NFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            return NFac.CheckNotificationName(Id, ProfileName);
        }

        public bool CheckNotificationProfileUsed(int ProfileId)
        {
            DUNSregistrationFacade Dfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            return Dfac.CheckNotificationProfileUsed(ProfileId);
        }
        #endregion

        #endregion

        #region "DUNS Registration Setup"
        #region List Monitoring registration
        public ActionResult IndexMonitoringRegistration()
        {
            DUNSregistrationFacade Drfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DUNSregistrationEntity> lstMonitoringRegistrations = new List<DUNSregistrationEntity>();
            lstMonitoringRegistrations = Drfac.GetDUNSregistration(Helper.DefaultMornitoring20Credential);
            DUNSregistrationEntity obj = new DUNSregistrationEntity();
            string indexMonitoringRegistrations = string.Empty;
            string upsertMonitoringRegistrations = string.Empty;
            ViewBag.ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            if (lstMonitoringRegistrations != null && lstMonitoringRegistrations.Count > 0)
            {
                obj = lstMonitoringRegistrations.FirstOrDefault();
                obj = obj == null ? new DUNSregistrationEntity() : obj;
                indexMonitoringRegistrations = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexMonitoringRegistration.cshtml", lstMonitoringRegistrations);
                upsertMonitoringRegistrations = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateDUNSregistration.cshtml", obj);
            }
            else
            {
                indexMonitoringRegistrations = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/IndexMonitoringRegistration.cshtml", null);
                upsertMonitoringRegistrations = RenderViewAsString.RenderPartialViewToString(this, "~/Views/DNBMonitoring/AddUpdateDUNSregistration.cshtml", obj);
            }
            return Json(new { result = true, indexMonitoringRegistrations = indexMonitoringRegistrations, upsertMonitoringRegistrations = upsertMonitoringRegistrations }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Insert Update Monitoring Registration
        public ActionResult AddUpdateDUNSregistration(string Parameters)
        {
            DUNSregistrationEntity obj = new DUNSregistrationEntity();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = DrFac.GetDUNSregistrationById(Convert.ToInt32(Parameters));
            }
            ViewBag.ConnectionString = this.CurrentClient.ApplicationDBConnectionString;
            return View(obj);
        }
        [HttpPost, RequestFromSameDomain, ValidateInput(true)]
        public ActionResult AddUpdateDUNSregistration(DUNSregistrationEntity obj, string btnMonitorRegistration, string MonitoringProfileId, string NotificationProfileId)
        {
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            obj.MonitoringProfileId = Convert.ToInt32(MonitoringProfileId);
            obj.NotificationProfileId = Convert.ToInt32(NotificationProfileId);
            obj.SubjectCategory = obj.SubjectCategory == null ? "" : obj.SubjectCategory;
            obj.CustomerReferenceText = obj.CustomerReferenceText == null ? "" : obj.CustomerReferenceText;
            obj.BillingEndorsementText = obj.BillingEndorsementText == null ? "" : obj.BillingEndorsementText;
            obj.Tags = obj.Tags == null ? "" : obj.Tags.TrimStart(',').TrimEnd(',');
            obj.CredentialId = Helper.DefaultMornitoring20Credential;
            int Id = DrFac.InsertDUNSregistration(obj);
            if (Id > 0)
            {
                return Json(new { result = true, message = obj.MonitoringRegistrationId > 0 ? DandBSettingLang.msgUpdateDUNSRegistration : DandBSettingLang.msgInsertDUNSRegistration }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        public static List<SelectListItem> GetMonitioringProfileNames(string ConnectionString)
        {
            List<SelectListItem> lstMonitoringProfileNames = new List<SelectListItem>();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(ConnectionString);
            List<MonitoringProfileEntity> lstMonitoringProfileEntity = DrFac.GetAllMonitoringProfileNames();
            lstMonitoringProfileNames.Add(new SelectListItem { Value = "-1", Text = "Select a Monitoring Profile" });
            foreach (var item in lstMonitoringProfileEntity)
            {
                lstMonitoringProfileNames.Add(new SelectListItem { Value = Convert.ToString(item.MonitoringProfileID), Text = item.ProfileName });
            }
            return lstMonitoringProfileNames;
        }
        public static List<SelectListItem> GetNotificationProfileNames(string ConnectionString)
        {
            List<SelectListItem> lstNotificationProfileNames = new List<SelectListItem>();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(ConnectionString);
            List<NotificationProfileEntity> lstNotificationProfileEntity = DrFac.GetAllNotificationProfileNames();
            lstNotificationProfileNames.Add(new SelectListItem { Value = "-1", Text = "Select a Notification Profile" });
            foreach (var item in lstNotificationProfileEntity)
            {
                lstNotificationProfileNames.Add(new SelectListItem { Value = Convert.ToString(item.NotificationProfileID), Text = item.NotificationProfileName });
            }
            return lstNotificationProfileNames;
        }
        #region Delete Monitoring Registration
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteMonitoringRegistration(string Parameters)
        {
            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    int id = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                    try
                    {
                        DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
                        DrFac.DeleteDUNSregistration(Convert.ToInt32(id));
                        return Json(new { result = true, message = CommonMessagesLang.msgCommanDeleteMessage }, JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false, message = CommonMessagesLang.msgInvadilState }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false, message = CommonMessagesLang.msgCommanErrorMessage }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Generate Random Number for applicationId 
        public string GetDigits()
        {
            var bytes = new byte[4];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 10000;
            return String.Format("{0:D4}", random);
        }
        #endregion
    }
}