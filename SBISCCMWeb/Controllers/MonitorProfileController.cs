using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBISCCMWeb.Models;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCCMWeb.Utility;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using PagedList;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, AllowLicense]
    public class MonitorProfileController : BaseController
    {
        #region Initialize veriable
        public string UserName, Password;
        int DNB_USERNAME, DNB_PASSWORD, SyncMonitoringProfileCount, SyncNotificationCount, SyncUserPreferenceCount, SyncCurrentMonitoringProfileCount, SyncCurrentNotificationCount, SyncCurrentUserPreferenceCount;

        #endregion

        #region MonitorProfile

        #region List of Monitoring Profile
        public ActionResult Index()
        {
            // Reinitialize all temp data 
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData["lstMonirtoringTemp"] = new List<TempMonitoring>();
            TempData["objTM"] = new TempMonitoring();
            TempData["objMTM"] = new TempMonitoring();
            TempData.Keep();

            return View();
        }
        // List of the Monitoring Profiles and manage pagination for the Monitoring Profiles 
        public ActionResult IndexMonitoringProfile(int? page, int? sortby, int? sortorder, int? pagevalue)
            {
            #region paging nation
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion

            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);

            List<MonitoringProfileEntity> objMPE = new List<MonitoringProfileEntity>();
            MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            // Get the records according to the page size and value from the database and fill in the list
            objMPE = Mpfac.GetMonitoringProfile(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();
            TempData.Keep();

            IPagedList<MonitoringProfileEntity> pagedMonitorProfile = new StaticPagedList<MonitoringProfileEntity>(objMPE.ToList(), currentPageIndex, pageSize, totalCount);
            return PartialView("IndexMonitoringProfile", pagedMonitorProfile);
        }
        public ActionResult _index()
        {
            return View();
        }
        #endregion

        #region Sync Monitoring Profile
        //Sync the current database with Dnb api and get the data from the api and update into database.
        public ActionResult GetProfileList()
        {
            MonitorProfileFacade MPfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<MonitoringProfileEntity> lstMPExits = new List<MonitoringProfileEntity>();
            lstMPExits = MPfac.GetAllMonitoringProfile();
            List<MonitoringProfileEntity> lstMPE = new List<MonitoringProfileEntity>();
            string endPoint = string.Format(DnBApi.SyncMonitoringProfileList, "1000", SyncCurrentMonitoringProfileCount);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);
            try
            {
                // Make Api call 
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    ListMonitoring objResponse = JsonConvert.DeserializeObject<ListMonitoring>(result);
                    // To check if the response of the Api is not null and also check the Contain of the response.
                    if (objResponse != null && objResponse.ListMonitoringProfileResponse != null && objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail != null)
                    {
                        if (objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.MonitoringProfileDetail != null && objResponse.ListMonitoringProfileResponse.ListMonitoringProfileResponseDetail.MonitoringProfileDetail.Count > 0)
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
                                MPfac.InsertUpdateMonitoringProfile(objMPE);
                                //MPfac.UpdateMonitorProfile(objMPE);
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
                        CommonMethod objCommon = new CommonMethod();
                        objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.ListMonitoringProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.ListMonitoringProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.ListMonitoringProfileResponse.TransactionResult.SeverityText, objResponse.ListMonitoringProfileResponse.TransactionResult.ResultID, objResponse.ListMonitoringProfileResponse.TransactionResult.ResultText, null, 0, null);
                    }
                    //// sync with database 
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    TempData["ResponseErroeMessage"] = MessageCollection.CommanErrorMessage;
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        ListMonitoring objResponse = serializer.Deserialize<ListMonitoring>(result);
                        if (objResponse != null && objResponse.ListMonitoringProfileResponse != null && objResponse.ListMonitoringProfileResponse.TransactionResult != null)
                            TempData["ResponseErroeMessage"] = objResponse.ListMonitoringProfileResponse.TransactionResult.ResultText;
                    }
                    return RedirectToAction("IndexMonitoringProfile", "MonitorProfile");
                }
            }
            if (SyncCurrentMonitoringProfileCount >= SyncMonitoringProfileCount)
            {
                return RedirectToAction("IndexMonitoringProfile", "MonitorProfile");
            }
            else
            {
                return GetProfileList();
            }
        }
        #endregion
        #region Create Monitor Profile
        [HttpGet]
        public ActionResult CreateMonitorProfile(string Parameters, string temp)
        {
            MonitoringProfileEntity objMPE = new MonitoringProfileEntity();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;

            Helper.ProfileId = 0;
            ViewBag.EditMonitorId = 0;
            ViewBag.IsConditionExists = false;
            #region Fetch data 
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                ViewBag.EditMonitorId = Convert.ToInt32(Parameters);
                MonitorProfileFacade mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                objMPE = mpfac.GetMonitorProfileByID(Convert.ToInt32(Parameters));
                Helper.ProfileId = objMPE.MonitoringProfileID;
                int totalCount = 0;
                lstMonitoringElemetsEntity = mpfac.GetMonitoringElementConditionsByProfileId(objMPE.MonitoringProfileID);
                TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
                if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
                {
                    ViewBag.EditMonitorId = objMPE.MonitoringProfileID;
                    ViewBag.IsConditionExists = true;

                }
            }
            #endregion
            #region Reset all list
            //lstMonitoringProfileElements = new List<MonitoingProfileElementsConditon>();
            TempData["lstBusinessElementssConditon"] = new List<BusinessElementssConditonList>();
            TempData["lstME"] = new List<MonitoringElements>();
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData["lstMonirtoringTemp"] = new List<TempMonitoring>();
            TempData["objTM"] = new TempMonitoring();
            TempMonitoring objMTM = new TempMonitoring();
            TempData["strCondition"] = "";
            TempData.Keep();
            #endregion
            return View(objMPE);
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CreateMonitorProfile(string Parameters)
        {
            MonitoringProfileEntity objMP = new MonitoringProfileEntity();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            string strResion = IsValidProfileName(objMP.ProfileName, objMP.ProductCode, objMP.MonitoringLevel);
            TempData.Keep();
            if (!string.IsNullOrEmpty(strResion))
            {
                if (strResion == "Name")
                {
                    return new JsonResult { Data = MessageCollection.DuplicateNameMonitoringProfile.ToString() };
                }
                else if (strResion == "ProfileCode")
                {
                    return new JsonResult { Data = MessageCollection.DuplicateProductMonitoringProfile.ToString() };
                }
                else if (strResion == "Elements")
                {
                    return new JsonResult { Data = MessageCollection.DuplicateElementsMonitoringProfile.ToString() };
                }
            }
            string endPoint = DnBApi.MonitoringProfileUrl;

            CommonMethod objCommon = new CommonMethod();
            objMP.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            if (objMP.MonitoringProfileID == 0)
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);
                MonitoringRequest root = CreateObject(objMP);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            // Insert Api logs
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.CreateMonitoringProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.CreateMonitoringProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.CreateMonitoringProfileResponse.TransactionResult.SeverityText, objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultID, objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText, null, 0, null);
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
                            MonitoringResponse objResponse = JsonConvert.DeserializeObject<MonitoringResponse>(result);
                            if (objResponse != null && objResponse.CreateMonitoringProfileResponse != null && objResponse.CreateMonitoringProfileResponse.TransactionResult != null)
                                return new JsonResult { Data = objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText };
                            else
                                return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                        }
                        else
                            return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                    }
                }
                TempData.Keep();
                return new JsonResult { Data = MessageCollection.InsertMonitoringProfile.ToString() };
            }
            else
            {
                endPoint += "/" + objMP.MonitoringProfileID;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

                UpdateMonitoringRequest root = UpdateRequstObject(objMP);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            // Insert Api logs
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdateMonitoringProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdateMonitoringProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdateMonitoringProfileResponse.TransactionResult.SeverityText, objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultID, objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText, null, 0, null);
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
                            MonitoringResponse objResponse = JsonConvert.DeserializeObject<MonitoringResponse>(result);
                            if (objResponse != null && objResponse.CreateMonitoringProfileResponse != null && objResponse.CreateMonitoringProfileResponse.TransactionResult != null && objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText != null)
                                return new JsonResult { Data = objResponse.CreateMonitoringProfileResponse.TransactionResult.ResultText };
                            else
                                return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                        }
                        else
                            return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                    }
                }
                TempData.Keep();
                return new JsonResult { Data = MessageCollection.UpdateMonitoringProfile.ToString() };
            }

        }

        public MonitoringRequest CreateObject(MonitoringProfileEntity objMPE)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            TempData.Keep();
            return objRequest;
        }

        public UpdateMonitoringRequest UpdateRequstObject(MonitoringProfileEntity objMPE)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            TempData.Keep();
            return objRequest;
        }
        #endregion
        #endregion

        #region Delete Monitoring Profile
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteProfile(string Parameters)
        {
            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    try
                    {
                        TempData.Keep();
                        if (CheckMonitoringProfileUsed(Convert.ToInt32(Parameters)))
                        {
                            return new JsonResult { Data = MessageCollection.UsedMonitoringProfile.ToString() };
                        }
                        DeleteMonitoringProfile(Parameters);

                    }
                    catch (WebException ex)
                    {
                        TempData.Keep();
                        using (var stream = ex.Response.GetResponseStream())
                        using (var streamReader = new StreamReader(stream))
                        {
                            var result = streamReader.ReadToEnd();
                            if (result != null)
                            {
                                var serializer = new JavaScriptSerializer();
                                UpdateMonitoringResponse objResponse = serializer.Deserialize<UpdateMonitoringResponse>(result);
                                if (objResponse != null && objResponse.UpdateMonitoringProfileResponse != null && objResponse.UpdateMonitoringProfileResponse.TransactionResult != null)
                                    return new JsonResult { Data = objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText };
                            }
                            return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                        }
                    }
                }
            }

            return new JsonResult { Data = MessageCollection.DeleteMonitoringProfile.ToString() };
        }
        private void DeleteMonitoringProfile(string ProfileId)
        {
            MonitorProfileFacade mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);

            string endPoint = DnBApi.MonitoringProfileUrl + "/" + ProfileId; // objMPE.MonitoringProfileID;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PUT";
            httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

            UpdateMonitoringRequest root = DeleteRequestObject();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                    CommonMethod objCommon = new CommonMethod();
                    if (objResponse != null)
                    {
                        objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdateMonitoringProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdateMonitoringProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdateMonitoringProfileResponse.TransactionResult.SeverityText, objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultID, objResponse.UpdateMonitoringProfileResponse.TransactionResult.ResultText, null, 0, null);
                    }
                }
            }
            catch (WebException webEx)
            { throw; }
            TempData.Keep();
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
            TempData.Keep();
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
                if (TempData["lstMonitoringElemetsEntity"] != null)
                {
                    lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
                }
                MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                if (lstMonitoringElemetsEntity.Count == 0)
                {
                    lstMonitoringElemetsEntity = Mpfac.GetMonitoringElementConditionsByProfileId(Helper.ProfileId).ToList();
                    TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
                }
            }
            TempData.Keep();
            return new JsonResult { Data = "Success" };
        }
        public ActionResult BusinessElement(int? page, int? sortby, int? sortorder, int? pagevalue, int? ProfileId)
        {
            // paging nation
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;

            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;

            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);
            ProfileId = Helper.ProfileId > 0 ? Helper.ProfileId : 0;
            //Get the list of Business Element 
            List<MonitoringElementConditionsEntity> lstTemp = new List<MonitoringElementConditionsEntity>();
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            TempData["lstBusinessElementssConditon"] = new List<BusinessElementssConditonList>();
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData.Keep();
            if (Request.IsAjaxRequest())
                return View("BusinessElement", pagedElementCondition);
            else
                return View("BusinessElement", pagedElementCondition);
        }

        #region Create Business Element
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult SaveBusinessElement(string Parameters)
        {
            //Save Business Element
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            if (TempData["lstBusinessElementssConditon"] != null)
            {
                lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();
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
                    TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
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
                TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
            }
            TempData.Keep();
            lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            return new JsonResult { Data = "success" };
        }
        #endregion

        #region Delete Business Elements
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult DeleteMainConditon(string Parameters)
        {
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
            }
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (Parameters != null && Convert.ToInt32(Parameters) > 0)
                {
                    if (lstMonitoringElemetsEntity != null && lstMonitoringElemetsEntity.Any())
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
            }
            TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
            TempData.Keep();
            if (Request.IsAjaxRequest())
            {
                return new JsonResult { Data = "Success" };
            }

            return new JsonResult { Data = "Fail" };
        }
        #endregion
        #endregion

        #region Business Condition List
        public ActionResult AddBusinessElement(string Parameters)
        {
            TempData["ConditionCount"] = 0;
            TempData["strCondition"] = "";
            //Get Query string in Encrypted mode and decrypt Query string and set Parameters
            TempData["BusinessCondionUpdateId"] = "";
            List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
            }
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (TempData["lstBusinessElementssConditon"] != null)
            {
                lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();
            }
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (TempData["lstTempMonirtoring"] != null)
            {
                lstTempMonirtoring = (TempData["lstTempMonirtoring"] as List<TempMonitoring>).Copy();
            }
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                TempData["BusinessCondionUpdateId"] = Parameters;

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
                                    TempData["strCondition"] = Convert.ToString(lstBusiness[0].strCondition);
                                    item.JsonCondition = jsonSerialiser.Serialize(lstBusinessElementssConditon);
                                    Helper.strCondition = Convert.ToString(TempData["strCondition"]);
                                    if (lstTempMonirtoring != null && lstTempMonirtoring.Any())
                                    {
                                        foreach (var Monitoring in lstTempMonirtoring)
                                        {
                                            if (Monitoring.objM.lstCondition != null && Monitoring.objM.lstCondition.Any())
                                            {
                                                TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) + Monitoring.objM.lstCondition.Count();
                                            }
                                            else
                                            {
                                                TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) + 1;
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
                TempData["lstTempMonirtoring"] = lstTempMonirtoring;
            }
            TempData["lstTempMonirtoring"] = lstTempMonirtoring;
            TempData["lstBusinessElementssConditon"] = lstBusinessElementssConditon;
            TempData.Keep();
            TempData.Remove("ConditionListUpdatesId");
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
            if (TempData["lstBusinessElementssConditon"] != null)
            {
                lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();
            }
            int DeletedId = Convert.ToInt32(Parameters);
            for (int i = 0; i < lstBusinessElementssConditon.Count; i++)
            {
                if (lstBusinessElementssConditon[i].ElementsConditonListId == Convert.ToInt32(Convert.ToInt32(DeletedId)))
                {
                    lstBusinessElementssConditon.RemoveAt(i);
                }
            }
            TempData["lstBusinessElementssConditon"] = lstBusinessElementssConditon;
            TempData.Keep();
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
            if (TempData["lstTempMonirtoring"] != null)
            {
                lstTempMonirtoring = (TempData["lstTempMonirtoring"] as List<TempMonitoring>).Copy();
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
                                    TempData["lstTempMonirtoring"] = lstTempMonirtoring;
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
                                TempData["lstTempMonirtoring"] = lstTempMonirtoring;
                            }
                        }
                        else
                        {
                            TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) + 1;
                            AddBlankRowinConditons(objMPEC.Conditon);
                        }

                        break;
                    case "Update":
                        for (int i = 0; i <= lstTempMonirtoring.Count - 1; i++)
                        {
                            UpdateGroupElement(lstTempMonirtoring[i], TempGrpId, objMPEC);
                            TempData["lstTempMonirtoring"] = lstTempMonirtoring;
                        }
                        break;
                    case "delete":
                        List<TempMonitoring> lstMonirtoringTemp = new List<TempMonitoring>();
                        TempData["lstMonirtoringTemp "] = lstMonirtoringTemp;
                        DeleteElementGroupNew(lstTempMonirtoring, TempGrpId);
                        TempData["lstTempMonirtoring"] = lstTempMonirtoring;
                        break;
                }
            }

            if (!string.IsNullOrEmpty(ConditionListUpdatesId))
            {
                ConditionListUpdatesId = StringCipher.Decrypt(ConditionListUpdatesId.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
                if (TempData["lstBusinessElementssConditon"] != null)
                {
                    lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();
                }
                lstTempMonirtoring = new List<TempMonitoring>();
                for (int i = 0; i < lstBusinessElementssConditon.Count; i++)
                {
                    if (lstBusinessElementssConditon[i].ElementsConditonListId == Convert.ToInt32(Convert.ToInt32(ConditionListUpdatesId)))
                    {
                        lstTempMonirtoring = lstBusinessElementssConditon[i].lstBusinessElements;
                    }
                }
                TempData["ConditionListUpdatesId"] = ConditionListUpdatesId;
                TempData["lstTempMonirtoring"] = lstTempMonirtoring;
                TempData["strCondition"] = "";
                CreateStringForCondition(lstTempMonirtoring);

                return View("AddElementCondition", lstTempMonirtoring);
            }
            if (TempData["lstTempMonirtoring"] != null)
            {
                lstTempMonirtoring = (TempData["lstTempMonirtoring"] as List<TempMonitoring>).Copy();
            }
            TempData["strCondition"] = "";
            Helper.strCondition = "";
            CreateStringForCondition(lstTempMonirtoring);
            Helper.strCondition = Convert.ToString(TempData["strCondition"]).Replace("(And", "And(").Replace("(OR", "OR(");
            TempData.Keep();
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
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
            }
            List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
            if (TempData["lstBusinessElementssConditon"] != null)
            {
                lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();
            }
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (TempData["lstTempMonirtoring"] != null)
            {
                lstTempMonirtoring = (TempData["lstTempMonirtoring"] as List<TempMonitoring>).Copy();
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
            TempData["strCondition"] = "";
            CreateStringForCondition(lstTempMonirtoring);
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["strCondition"])))
            {
                objBECL.strCondition = Convert.ToString(TempData["strCondition"]).Replace("(And", "And(").Replace("(OR", "OR(");
            }
            if (!string.IsNullOrEmpty(BusinessCondionUpdateId) && Convert.ToInt32(BusinessCondionUpdateId) > 0 && lstBusinessElementssConditon.Count > 0)
            {
                BusinessElementssConditonList objTBECL = lstBusinessElementssConditon.Where(x => x.ElementsConditonListId == Convert.ToInt32(BusinessCondionUpdateId)).FirstOrDefault();
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
            TempData["lstBusinessElementssConditon"] = lstBusinessElementssConditon;
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();

            #region Save for display in business elements listing
            objMECE.ElementName = ParentElements;
            objMECE.ChangeCondition = ParentCondition;
            objMECE.MonitoringConditionID = 0;
            objMECE.ProductElementID = Convert.ToInt32(ProductElementId);
            MonitorProfileFacade mfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            objMECE.ElementPCMXPath = Convert.ToString(mfac.GetProductElementData(Helper.ProductId).Where(x => x.ElementName == objMECE.ElementName).Select(x => x.ElementPCMXPath).FirstOrDefault());
            var jsonSerialiser = new JavaScriptSerializer();
            objMECE.JsonCondition = jsonSerialiser.Serialize(lstBusinessElementssConditon);
            objMECE.Condition = Convert.ToString(TempData["strCondition"]).Replace("(And", "And(").Replace("(OR", "OR(");
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

            TempData["lstMonitoringElemetsEntity"] = lstMonitoringElemetsEntity;
            TempData["lstBusinessElementssConditon"] = new List<BusinessElementssConditonList>();
            #endregion
            TempData.Keep();
            return new JsonResult { Data = "success" };
        }
        [HttpGet,RequestFromAjax,RequestFromSameDomain]
        public JsonResult EmptyCondition()
        {
            TempData["ConditionCount"] = 0;
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
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
                    List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity = new List<MonitoringElementConditionsEntity>();
                    lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
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
            TempData["ConditionCount"] = 0;
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData["lstBusinessElementssConditon"] = new List<BusinessElementssConditonList>();
            TempData["lstMonitoringElemetsEntity"] = new List<MonitoringElementConditionsEntity>();
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData["lstMonirtoringTemp"] = new List<TempMonitoring>();
            TempData["objTM"] = new TempMonitoring();
            TempMonitoring objMTM = new TempMonitoring();
            TempData["strCondition"] = "";
            return new JsonResult { Data = "success" };
        }
        #endregion

        #region Add blank Row in Element Conditions
        public void AddBlankRowinConditons(string Condition)
        {
            // Single node add in list for the Condition. It may be "And" or "OR"
            MonitoingProfileElementsConditon objMPEC = new MonitoingProfileElementsConditon();
            List<TempMonitoring> lstTempMonirtoring = new List<TempMonitoring>();
            if (TempData["lstTempMonirtoring"] != null)
            {
                lstTempMonirtoring = (TempData["lstTempMonirtoring"] as List<TempMonitoring>).Copy();
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
            TempData["lstTempMonirtoring"] = lstTempMonirtoring;
            TempData.Keep();
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
                        TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) + 1;
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

            Outer:
            var strValue = "";

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
            Outer:
            var strValue = "";
        }
        public void DeleteElementGroup(TempMonitoring objTemp, string TempGrpId)
        {
            List<TempMonitoring> lstMonirtoringTemp = new List<TempMonitoring>();
            if (TempData["lstMonirtoringTemp"] != null)
            {
                lstMonirtoringTemp = (TempData["lstMonirtoringTemp"] as List<TempMonitoring>).Copy();
            }
            TempMonitoring objMTM = new TempMonitoring();
            if (TempData["objMTM"] != null)
            {
                objMTM = (TempData["objMTM"] as TempMonitoring).Copy();
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
                    TempData["lstMonirtoringTemp"] = lstMonirtoringTemp;
                    TempData["objMTM"] = objMTM;

                    objTemp = objTClone.Clone().Copy();
                    if (objTemp.objM.TempConditionId != TempGrpId)
                    {
                        if (objTemp.objM.lstCondition != null && objTemp.objM.lstCondition.Count > 0)
                        {
                            for (int j = 0; j <= objTemp.objM.lstCondition.Count - 1; j++)
                            {
                                DeleteElementGroup(objTemp.objM.lstCondition[j], TempGrpId);
                            }
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
                            TempData["objMTM"] = objMTM;
                        }
                        else
                        {
                            lstMonirtoringTemp.Add(objTemp);
                            TempData["lstMonirtoringTemp"] = lstMonirtoringTemp;
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
                        TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) - lstTemp[i].objM.lstCondition.Count();
                    }
                    else
                    {
                        TempData["ConditionCount"] = Convert.ToInt32(TempData["ConditionCount"]) - 1;
                    }

                    if (TempData["lstBusinessElementssConditon"] != null)
                    {
                        List<BusinessElementssConditonList> lstBusinessElementssConditon = new List<BusinessElementssConditonList>();
                        lstBusinessElementssConditon = (TempData["lstBusinessElementssConditon"] as List<BusinessElementssConditonList>).Copy();

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
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["strCondition"])))
            {
                strCondition = Convert.ToString(TempData["strCondition"]);
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
                                TempData["strCondition"] = strCondition;
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
                                TempData["strCondition"] = strCondition;
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
                            TempData["strCondition"] = strCondition;
                            CreateStringForCondition(lstTemp[i].objM.lstCondition);
                            if (!string.IsNullOrEmpty(Convert.ToString(TempData["strCondition"])))
                            {
                                strCondition = Convert.ToString(TempData["strCondition"]);
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
                            TempData["strCondition"] = strCondition;
                        }
                    }
                }
            }
        }

        public void CreateStringForMaterCondition(List<TempMonitoring> lstTemp)
        {
            string strCondition = "";
            if (!string.IsNullOrEmpty(Convert.ToString(TempData["strCondition"])))
            {
                strCondition = Convert.ToString(TempData["strCondition"]);
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
                                TempData["strCondition"] = strCondition;
                            }
                            else
                            {
                                strCondition += (lstTemp[i].objM.Conditon != null ? lstTemp[i].objM.Conditon : "") + "@#$" + "(" + lstTemp[i].objM.Element + "@#$" + lstTemp[i].objM.ConditonOpetator + "@#$" + lstTemp[i].objM.ConditonValue + " )";
                                TempData["strCondition"] = strCondition;
                            }

                        }
                        else if (lstTemp[i].objM != null && lstTemp[i].objM.lstCondition != null && lstTemp[i].objM.lstCondition.Any())
                        {
                            lstTemp[i].objM.lstCondition[0].objM.Conditon = null;
                            strCondition += "(" + (lstTemp[i].objM.Conditon == "AndGroup" ? "And" : "OR");
                            TempData["strCondition"] = strCondition;
                            CreateStringForMaterCondition(lstTemp[i].objM.lstCondition);
                            if (!string.IsNullOrEmpty(Convert.ToString(TempData["strCondition"])))
                            {
                                strCondition = Convert.ToString(TempData["strCondition"]);
                            }
                            strCondition += ")";
                            TempData["strCondition"] = strCondition;
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
            List<MonitoringProductEntity> lstProductElement = new List<MonitoringProductEntity>();
            MonitorProfileFacade mfac = new MonitorProfileFacade(ConnectionString);
            lstProductElement = mfac.getProductList();
            return lstProductElement;
        }
        //Gel all Products Element Detail for DropDown List in Business Condition view
        public List<MonitoringProductElementEntity> GetAllProductElement(List<MonitoringElementConditionsEntity> lstMonitoringElemetsEntity, string ProductElementID, string ConnectionString)
        {
            // for Dropdown in add element condition page set all Element product wise.
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
            }
            List<MonitoringProductElementEntity> lstProductElement = new List<MonitoringProductElementEntity>();
            MonitorProfileFacade mfac = new MonitorProfileFacade(ConnectionString);
            lstProductElement = mfac.GetProductElementData(Helper.ProductId);
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
            TempData.Keep();
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
        [RequestFromSameDomain,RequestFromAjax]
        public JsonResult CheckBusinessCondition(int ParentElemets, string ParentCodition, string BusinessCondionUpdateId)
        {
            TempData["lstTempMonirtoring"] = null;
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
                TempData["lstTempMonirtoring"] = null;
                TempData["ConditionCount"] = null;
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
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueEquals", Text = "ValueEquals" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueBecomesNull", Text = "ValueBecomesNull" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueBecomesNotNull", Text = "ValueBecomesNotNull" });
                    lstAllBussnessElements.Add(new SelectListItem { Value = "MultiCondition", Text = "MultiCondition" });
                }
                else if (Condition.ToLower() == "numeric" && objMPE.MontoringType.ToLower() == "multichange")
                {
                    Helper.ElementType = "numeric";
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueIncreaseByPercentage", Text = "ValueIncreaseByPercentage" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueDecreaseByPercentage", Text = "ValueDecreaseByPercentage" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueChangeByPercentage", Text = "ValueChangeByPercentage" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueIncreaseBy", Text = "ValueIncreaseBy" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueDecreaseBy", Text = "ValueDecreaseBy" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueChangeBy", Text = "ValueChangeBy" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueGoesBeyond", Text = "ValueGoesBeyond" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueGoesBelow", Text = "ValueGoesBelow" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueEquals", Text = "ValueEquals" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueBecomesNull", Text = "ValueBecomesNull" });
                    //lstAllBussnessElements.Add(new SelectListItem { Value = "ValueBecomesNotNull", Text = "ValueBecomesNotNull" });
                    lstAllBussnessElements.Add(new SelectListItem { Value = "MultiCondition", Text = "MultiCondition" });
                }
                TempData.Keep();
            }
            return new JsonResult { Data = new { lstAllBussnessElements = lstAllBussnessElements, ElementType = Helper.ElementType } };
        }
        #endregion

        #region Cancel Monitoring Profile
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public JsonResult CancelMonitoringProfile()
        {
            //Clear Data and set Default value for all.
            TempData["lstBusinessElementssConditon"] = new List<BusinessElementssConditonList>();
            TempData["lstME"] = new List<MonitoringElements>();
            TempData["lstTempMonirtoring"] = new List<TempMonitoring>();
            TempData["lstMonirtoringTemp"] = new List<TempMonitoring>();
            TempMonitoring objTM = new TempMonitoring();
            TempData["objMTM"] = new TempMonitoring();
            TempData["strCondition"] = "";
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
            if (TempData["lstMonitoringElemetsEntity"] != null)
            {
                lstMonitoringElemetsEntity = (TempData["lstMonitoringElemetsEntity"] as List<MonitoringElementConditionsEntity>).Copy();
            }
            MonitorProfileFacade Mpfac = new MonitorProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<MonitoringProfileEntity> lstMPE = new List<MonitoringProfileEntity>();
            lstMPE = Mpfac.GetAllMonitoringProfile();
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
                            if (lstTME != null && lstTME.Any())
                            {
                                if (lstME.Count == lstTME.Count)
                                {
                                    foreach (var elements in lstTME)
                                    {
                                        foreach (var child in lstME)
                                        {
                                            if (elements.ElementName == child.ElementName)
                                            {
                                                if (elements.ChangeCondition == child.ChangeCondition)
                                                {
                                                    count += 1;
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                            if (count > 0)
                            {
                                if (count == lstME.Count)
                                {
                                    strResion = "Elements";
                                    return strResion;
                                }
                            }
                        }
                    }
                }

            }
            TempData.Keep();
            return strResion;
        }

        public bool CheckMonitoringProfileUsed(int ProfileId)
        {
            bool IsUsed = false;
            DUNSregistrationFacade Dfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            return IsUsed = Dfac.CheckMonitoringProfileUsed(ProfileId);
        }
        #endregion

        #region User Prefrence

        #region List User Preference
        public ActionResult IndexUserPreference(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region paging nation
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;
            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;
            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion
            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion
            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);


            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<UserPreferenceEntity> lstuserPrefrnc = new List<UserPreferenceEntity>();
            lstuserPrefrnc = UPFac.GetUserPreference(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();
            IPagedList<UserPreferenceEntity> pagedUserPreference = new StaticPagedList<UserPreferenceEntity>(lstuserPrefrnc, currentPageIndex, pageSize, totalCount);
            return PartialView("IndexUserPreference", pagedUserPreference);
        }
        #endregion

        #region Snyc User Preference
        public ActionResult GetUserPrefrenceList()
        {
            string endPoint = string.Format(DnBApi.UserPrefrenceLis, "1000", SyncCurrentUserPreferenceCount);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);
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
                        if (!string.IsNullOrEmpty(Convert.ToString(TempData["lstPreference"])))
                        {
                            lstPreference = (TempData["lstPreference"] as List<string>).Copy();
                        }
                        foreach (var item in objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.PreferenceDetail)
                        {
                            lstPreference.Add(item.PreferenceName);
                            UserPreferenceEntity objUserPreferenceExists = lstUserPreference.Where(x => x.PreferenceName == item.PreferenceName).FirstOrDefault();
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
                            objUserPreference.IsDeleted = item.PreferenceStatusText == "Active" ? false : true;
                            UPFac.InsertUpdateUserPreference(objUserPreference);
                        }
                        // Insert Api logs
                        CommonMethod objCommon = new CommonMethod();
                        objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.ListPreferenceResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.ListPreferenceResponse.TransactionDetail.TransactionTimestamp), objResponse.ListPreferenceResponse.TransactionResult.SeverityText, objResponse.ListPreferenceResponse.TransactionResult.ResultID, objResponse.ListPreferenceResponse.TransactionResult.ResultText, null, 0, null);
                        lstUserPreference = UPFac.GetAllUserPreference();
                        //List<string> APIPreferenceDetail = objResponse.ListPreferenceResponse.ListPreferenceResponseDetail.PreferenceDetail.Select(i => i.PreferenceName).Distinct().OrderByDescending(s => s).ToList();
                        List<string> APIPreferenceDetail = lstPreference.Distinct().OrderByDescending(s => s).ToList();
                        List<string> PreferenceDetail = lstUserPreference.Select(i => i.PreferenceName).Distinct().OrderByDescending(s => s).ToList();
                        var DiffUserPreference = PreferenceDetail.Except(APIPreferenceDetail);
                        if (DiffUserPreference != null)
                        {
                            foreach (var item in DiffUserPreference)
                            {
                                UserPreferenceEntity objUserPreferenceExists = lstUserPreference.Where(x => x.PreferenceName == item).FirstOrDefault();
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
                    TempData["ResponseErroeMessage"] = MessageCollection.CommanErrorMessage;
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        ListPreference objResponse = serializer.Deserialize<ListPreference>(result);
                        if (objResponse != null && objResponse.ListPreferenceResponse != null && objResponse.ListPreferenceResponse.TransactionResult != null)
                            TempData["ResponseErroeMessage"] = objResponse.ListPreferenceResponse.TransactionResult.ResultText;
                    }
                    return RedirectToAction("IndexUserPreference", "MonitorProfile");
                }
            }
            if (SyncCurrentUserPreferenceCount >= SyncUserPreferenceCount)
            {
                return RedirectToAction("IndexUserPreference", "MonitorProfile");
            }
            else
            {
                TempData["lstPreference"] = lstPreference;
                return GetUserPrefrenceList();
            }
        }
        #endregion

        #region Insert Update User Preference
        public ActionResult CreateUserPrefrence(string Parameters)
        {
            UserPreferenceEntity obj = new UserPreferenceEntity();
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            obj.PreferenceValue = "dnbnotifications@matchbookservices.com";
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = UPFac.GetUserPreferenceById(Convert.ToInt32(Parameters));
            }

            return View(obj);
        }
        [HttpPost, RequestFromSameDomain]
        public ActionResult CreateUserPrefrence(UserPreferenceEntity obj, string btnUserPreference)
        {

            obj.PreferenceType = "Email";
            obj.ApplicationAreaName = "Monitoring";
            int PreferenceId = 0;
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (CheckUserPreferenceName(obj.PreferenceName.Trim(), obj.PreferenceID))
            {
                ViewBag.Message = MessageCollection.DuplicateNameUserPreference;
                return View(obj);
            }
            obj.CreatedBy = Convert.ToInt32(User.Identity.GetUserId());
            obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            #region Api call
            string endPoint = DnBApi.UserpreferenceUrl;
            CommonMethod objCommon = new CommonMethod();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);
            if (obj.PreferenceID == 0)
            {
                UserPreferenceRequest root = CreatePrefObject(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            PreferenceId = UPFac.InsertUpdateUserPreference(obj);
                            // Insert Api logs
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.CreatePreferenceResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.CreatePreferenceResponse.TransactionDetail.TransactionTimestamp), objResponse.CreatePreferenceResponse.TransactionResult.SeverityText, objResponse.CreatePreferenceResponse.TransactionResult.ResultID, objResponse.CreatePreferenceResponse.TransactionResult.ResultText, null, 0, null);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = MessageCollection.CommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            UserPreferenceReponse objResponse = JsonConvert.DeserializeObject<UserPreferenceReponse>(result);
                            if (objResponse != null && objResponse.CreatePreferenceResponse != null && objResponse.CreatePreferenceResponse.TransactionResult != null && objResponse.CreatePreferenceResponse.TransactionResult.ResultText != null)
                            {
                                ViewBag.Message = objResponse.CreatePreferenceResponse.TransactionResult.ResultText;
                            }
                            else
                            {
                                ViewBag.Message = MessageCollection.CommanErrorMessage;
                            }
                        }
                        else
                        {
                            ViewBag.Message = MessageCollection.CommanErrorMessage;
                        }
                        return View(obj);
                    }
                }
            }
            else
            {
                UserPreferenceUpdateRequest root = UpdatePrefObject(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            PreferenceId = UPFac.InsertUpdateUserPreference(obj);
                            // Insert Api logs
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdatePreferenceResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdatePreferenceResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdatePreferenceResponse.TransactionResult.SeverityText, objResponse.UpdatePreferenceResponse.TransactionResult.ResultID, objResponse.UpdatePreferenceResponse.TransactionResult.ResultText, null, 0, null);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = MessageCollection.CommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            UserPreferenceReponse objResponse = JsonConvert.DeserializeObject<UserPreferenceReponse>(result);
                            if (objResponse != null && objResponse.CreatePreferenceResponse != null && objResponse.CreatePreferenceResponse.TransactionResult != null && objResponse.CreatePreferenceResponse.TransactionResult.ResultText != null)
                            { ViewBag.Message = objResponse.CreatePreferenceResponse.TransactionResult.ResultText; }
                            else
                            {
                                ViewBag.Message = MessageCollection.CommanErrorMessage;
                            }
                        }
                        else
                        {
                            ViewBag.Message = MessageCollection.CommanErrorMessage;
                        }
                        return View(obj);
                    }
                }
            }
            #endregion
            if (PreferenceId > 0)
            {
                switch (btnUserPreference)
                {
                    case "Create User Preference":
                        ViewBag.Message = MessageCollection.InertUserPreference;
                        break;
                    case "Update User Preference":
                        ViewBag.Message = MessageCollection.UpdateUserPreference;
                        break;
                }
            }
            return View(obj);
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
            //objTrans.ApplicationTransactionID = "Pref_"+ GetDigits();
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
                UserPreferenceEntity obj = new UserPreferenceEntity();
                obj = UPFac.GetUserPreferenceById(Convert.ToInt32(Convert.ToInt32(id)));
                if (CheckUserPreferenceUsed(obj.PreferenceName))
                {
                    return new JsonResult { Data = MessageCollection.UsedUserPreference };
                }
                #region Api call
                string endPoint = DnBApi.UserpreferenceUrl;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

                UserPreferenceUpdateRequest root = DeletePrefRequestObject(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            UPFac.InsertUpdateUserPreference(obj);
                            // Insert Api logs
                            CommonMethod objCommon = new CommonMethod();
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdatePreferenceResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdatePreferenceResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdatePreferenceResponse.TransactionResult.SeverityText, objResponse.UpdatePreferenceResponse.TransactionResult.ResultID, objResponse.UpdatePreferenceResponse.TransactionResult.ResultText, null, 0, null);
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
                                return new JsonResult { Data = objResponse.UpdatePreferenceResponse.TransactionResult.ResultText };
                        }
                        return new JsonResult { Data = MessageCollection.CommanErrorMessage.ToString() };
                    }
                }
            }
            else
            {
                return new JsonResult { Data = MessageCollection.CommanErrorMessage.ToString() };
            }
            #endregion
            return new JsonResult { Data = "Success" };
        }
        private UserPreferenceUpdateRequest DeletePrefRequestObject(UserPreferenceEntity objUserPreference)
        {
            UserPreferenceUpdateRequest objRequest = new UserPreferenceUpdateRequest();
            UpdatePreferenceRequest objUpdatePrefRequest = new UpdatePreferenceRequest();
            objRequest.UpdatePreferenceRequest = objUpdatePrefRequest;
            objUpdatePrefRequest.xmlns = DnBApi.UserPreferenceService;

            TransactionDetail objTrans = new TransactionDetail();
            //objTrans.ApplicationTransactionID = "Pref_" + GetDigits();
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
            bool IsUsed = false;
            UserPreferenceFacade Ufac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            return IsUsed = Ufac.CheckUserPreferenceUsed(UserPreference);
        }
        #endregion

        #region Others
        public bool CheckUserPreferenceName(string UserPreferenceName, int Id)
        {
            bool IsExits = false;
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
            return IsExits = UPFac.CheckUserPreferenceName(Id, UserPreferenceName);
        }

        #endregion

        #endregion

        #region Notification Profile
        #region List Notification
        public ActionResult IndexNotificationProfile(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region paging
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;
            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;
            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion

            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion

            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);

            NotificationProfileFacade NPfac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<NotificationProfileEntity> lstNotificationProfile = new List<NotificationProfileEntity>();
            lstNotificationProfile = NPfac.GetNotificationProfile(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();
            IPagedList<NotificationProfileEntity> pagedNotificationProfile = new StaticPagedList<NotificationProfileEntity>(lstNotificationProfile, currentPageIndex, pageSize, totalCount);
            return PartialView("IndexNotificationProfile", pagedNotificationProfile);
        }
        #endregion

        #region Sync Notification
        public ActionResult GetNotificationList()
        {
            string endPoint = string.Format(DnBApi.NotificationList, "1000", SyncCurrentMonitoringProfileCount); // put in common file

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);
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
                        List<NotificationProfileEntity> lstNotification = NFac.GetAllNotificationProfile();
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
                            objNotification.IsDeleted = item.NotificationProfileStatusText == "Active" ? false : true;
                            NFac.InsertNotificationProfile(objNotification);
                        }
                        // Insert Api logs
                        CommonMethod objCommon = new CommonMethod();
                        objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.ListNotificationProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.ListNotificationProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.ListNotificationProfileResponse.TransactionResult.SeverityText, objResponse.ListNotificationProfileResponse.TransactionResult.ResultID, objResponse.ListNotificationProfileResponse.TransactionResult.ResultText, null, 0, null);
                    }
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var streamReader = new StreamReader(stream))
                {
                    var result = streamReader.ReadToEnd();
                    TempData["ResponseErroeMessage"] = MessageCollection.CommanErrorMessage;
                    if (result != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        ListNotification objResponse = serializer.Deserialize<ListNotification>(result);
                        if (objResponse != null && objResponse.ListNotificationProfileResponse != null && objResponse.ListNotificationProfileResponse.TransactionResult != null)
                            TempData["ResponseErroeMessage"] = objResponse.ListNotificationProfileResponse.TransactionResult.ResultText;
                    }
                    return RedirectToAction("IndexNotificationProfile", "MonitorProfile");
                }
            }
            if (SyncCurrentNotificationCount >= SyncNotificationCount)
            {
                return RedirectToAction("IndexNotificationProfile", "MonitorProfile");
            }
            else
            {
                return GetNotificationList();
            }
        }
        #endregion

        #region Insert Update Notification
        public ActionResult CreateNotificationProfile(string Parameters)
        {
            NotificationProfileEntity obj = new NotificationProfileEntity();
            NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = NPFac.GetNotificationProfileById(Convert.ToInt32(Parameters));
            }
            ViewBag.Frequency = GetFrequency();
            ViewBag.UserPreferences = GetUserPreferenceName();
            return View(obj);
        }

        [HttpPost, ValidateAntiForgeryToken, RequestFromSameDomain]
        public ActionResult CreateNotificationProfile(NotificationProfileEntity obj, string btnNotificationProfile)
        {
            int ProfileId = 0;
            obj.DeliveryMode = "Email";
            obj.DeliveryFormat = "XML";
            obj.CreatedBy = Convert.ToInt32(User.Identity.GetUserId());
            obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
            NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (CheckNotification(obj.NotificationProfileName.Trim(), obj.NotificationProfileID))
            {
                ViewBag.Message = MessageCollection.DuplicateNotification;
                ViewBag.Frequency = GetFrequency();
                ViewBag.UserPreferences = GetUserPreferenceName();
                return View(obj);
            }

            #region Api Call

            if (obj.NotificationProfileID == 0)
            {
                string applicationID = "NOT_001";
                // Actual call
                string endPoint = DnBApi.NotificationUrl;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

                NotificationRequest root = CreateNotificationObject(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            ProfileId = NPFac.InsertNotificationProfile(obj);
                            // Insert Api logs
                            CommonMethod objCommon = new CommonMethod();
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.CreateNotificationProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.CreateNotificationProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.CreateNotificationProfileResponse.TransactionResult.SeverityText, objResponse.CreateNotificationProfileResponse.TransactionResult.ResultID, objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText, null, 0, null);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = MessageCollection.CommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationResponse objResponse = serializer.Deserialize<NotificationResponse>(result);
                            if (objResponse != null && objResponse.CreateNotificationProfileResponse != null && objResponse.CreateNotificationProfileResponse.TransactionResult != null && objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText != null)
                            {
                                ViewBag.Message = objResponse.CreateNotificationProfileResponse.TransactionResult.ResultText;
                            }
                            else
                            {
                                ViewBag.Message = MessageCollection.CommanErrorMessage;
                            }
                        }
                        else
                        {
                            ViewBag.Message = MessageCollection.CommanErrorMessage;
                        }
                    }
                }
            }
            else
            {
                string endPoint = DnBApi.NotificationUrl + "/" + obj.NotificationProfileID;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

                NotificationUpdateRequest root = UpdateNotificationObjct(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            ProfileId = NPFac.InsertNotificationProfile(obj);
                            // Insert Api logs
                            CommonMethod objCommon = new CommonMethod();
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdateNotificationProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdateNotificationProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdateNotificationProfileResponse.TransactionResult.SeverityText, objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultID, objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText, null, 0, null);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        ViewBag.Message = MessageCollection.CommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                            if (objResponse != null && objResponse.UpdateNotificationProfileResponse != null && objResponse.UpdateNotificationProfileResponse.TransactionResult != null && objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText != null)
                            {
                                ViewBag.Message = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText;
                            }
                            else
                            {
                                ViewBag.Message = MessageCollection.CommanErrorMessage;
                            }
                        }
                        else
                        {
                            ViewBag.Message = MessageCollection.CommanErrorMessage;
                        }
                    }
                }
            }
            #endregion
            if (ProfileId > 0)
            {
                switch (btnNotificationProfile)
                {
                    case "Create Notification Profile":
                        ViewBag.Message = MessageCollection.InsertNotification;
                        break;
                    case "Update Notification Profile":
                        ViewBag.Message = MessageCollection.UpdateNotification;
                        break;
                }
            }

            ViewBag.Frequency = GetFrequency();
            ViewBag.UserPreferences = GetUserPreferenceName();
            return View(obj);
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
            //objDetail.ApplicationTransactionID = "Not_"+GetDigits();

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
                    return new JsonResult { Data = MessageCollection.UsedNotification.ToString() };
                }
                NotificationProfileFacade NPFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
                NotificationProfileEntity obj = new NotificationProfileEntity();
                obj = NPFac.GetNotificationProfileById(Convert.ToInt32(id));
                obj.RequestDateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:MM:ss", CultureInfo.CreateSpecificCulture("en-US")));
                #region Api Call
                string endPoint = DnBApi.NotificationUrl + "/" + obj.NotificationProfileID;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(endPoint);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                httpWebRequest.Headers.Add("Authorization", Helper.ApiToken);

                NotificationUpdateRequest root = DeleteNotificationObject(obj);
                DataContractJsonSerializer ser = new DataContractJsonSerializer(root.GetType());
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
                            NPFac.InsertNotificationProfile(obj);
                            // Insert Api logs
                            CommonMethod objCommon = new CommonMethod();
                            objCommon.InsertAPILogs(this.CurrentClient.ApplicationDBConnectionString, objResponse.UpdateNotificationProfileResponse.TransactionDetail.ServiceTransactionID, Convert.ToDateTime(objResponse.UpdateNotificationProfileResponse.TransactionDetail.TransactionTimestamp), objResponse.UpdateNotificationProfileResponse.TransactionResult.SeverityText, objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultID, objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText, null, 0, null);
                        }
                    }
                }
                catch (WebException ex)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        var result = streamReader.ReadToEnd();
                        string message = MessageCollection.CommanErrorMessage;
                        if (result != null)
                        {
                            var serializer = new JavaScriptSerializer();
                            NotificationUpdateResponse objResponse = serializer.Deserialize<NotificationUpdateResponse>(result);
                            if (objResponse != null && objResponse.UpdateNotificationProfileResponse != null && objResponse.UpdateNotificationProfileResponse.TransactionResult != null)
                                return new JsonResult { Data = objResponse.UpdateNotificationProfileResponse.TransactionResult.ResultText };
                        }
                        return new JsonResult { Data = MessageCollection.CommanErrorMessage };
                    }
                }
                #endregion
                NPFac.DeleteNotificationProfile(Convert.ToInt32(id));
                return new JsonResult { Data = "Success" };
            }
            else
            {
                return new JsonResult { Data = "fail" };
            }
        }

        public NotificationUpdateRequest DeleteNotificationObject(NotificationProfileEntity objNotification)
        {
            NotificationUpdateRequest objRequest = new NotificationUpdateRequest();
            MonUpdateNotificationProfileRequest objNotiRequest = new MonUpdateNotificationProfileRequest();
            objRequest.MonUpdateNotificationProfileRequest = objNotiRequest;

            objNotiRequest.xmlnsmon = DnBApi.MonitoingService;

            TransactionDetail objDetail = new TransactionDetail();
            //objDetail.ApplicationTransactionID = "Not_"+ GetDigits();

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
        public List<SelectListItem> GetUserPreferenceName()
        {
            List<SelectListItem> lstUserPreference = new List<SelectListItem>();
            UserPreferenceFacade UPFac = new UserPreferenceFacade(this.CurrentClient.ApplicationDBConnectionString);
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

        public List<SelectListItem> GetFrequency()
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
            bool IsExits = false;
            NotificationProfileFacade NFac = new NotificationProfileFacade(this.CurrentClient.ApplicationDBConnectionString);
            return IsExits = NFac.CheckNotificationName(Id, ProfileName);
        }

        public bool CheckNotificationProfileUsed(int ProfileId)
        {
            bool IsUsed = false;
            DUNSregistrationFacade Dfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            return IsUsed = Dfac.CheckNotificationProfileUsed(ProfileId);
        }
        #endregion

        #endregion

        #region Monitoring Registration
        #region List Monitoring registration
        public ActionResult IndexMonitoringRegistration(int? page, int? sortby, int? sortorder, int? pagevalue)
        {
            #region paging
            int pageNumber = (page ?? 1);
            if (!(sortby.HasValue && sortby.Value > 0))
                sortby = 1;
            if (!(sortorder.HasValue && sortorder.Value > 0))
                sortorder = 2;
            int sortParam = int.Parse(sortby.ToString() + sortorder.ToString());
            int totalCount = 0;
            int currentPageIndex = page.HasValue ? page.Value : 1;
            int pageSize = pagevalue.HasValue ? pagevalue.Value : 10;
            #endregion

            #region Set Viewbag
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;
            TempData["pageno"] = currentPageIndex;
            TempData["pagevalue"] = pageSize;
            #endregion

            string finalsortOrder = Convert.ToString(sortby) + Convert.ToString(sortorder);

            DUNSregistrationFacade Drfac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<DUNSregistrationEntity> lstMonitoringRegistrations = new List<DUNSregistrationEntity>();
            lstMonitoringRegistrations = Drfac.GetDUNSregistration(Convert.ToInt32(finalsortOrder), currentPageIndex, pageSize, out totalCount).ToList();
            IPagedList<DUNSregistrationEntity> pagedMonitoringRegistrations = new StaticPagedList<DUNSregistrationEntity>(lstMonitoringRegistrations, currentPageIndex, pageSize, totalCount);
            return PartialView("IndexMonitoringRegistration", pagedMonitoringRegistrations);
        }
        #endregion


        #region Insert Update Monitoring Registration
        public ActionResult CreateMonitoringRegistration(string Parameters)
        {
            DUNSregistrationEntity obj = new DUNSregistrationEntity();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                obj = DrFac.GetDUNSregistrationById(Convert.ToInt32(Parameters));
            }

            if (obj.MonitoringRegistrationId > 0)
            {
                ViewBag.MonitoringProfile = new SelectList(GetMonitioringProfileNames(), "Value", "Text", obj.MonitoringProfileId);
                ViewBag.NotificationProfile = new SelectList(GetNotificationProfileNames(), "Value", "Text", obj.NotificationProfileId);
            }
            else
            {
                ViewBag.MonitoringProfile = new SelectList(GetMonitioringProfileNames(), "Value", "Text", "-1");
                ViewBag.NotificationProfile = new SelectList(GetNotificationProfileNames(), "Value", "Text", "-1");
            }
            return View(obj);
        }

        [HttpPost, RequestFromSameDomain]
        public ActionResult CreateMonitoringRegistration(DUNSregistrationEntity obj, string btnMonitorRegistration, string MonitoringProfileId, string NotificationProfileId)
        {
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            obj.MonitoringProfileId = Convert.ToInt32(MonitoringProfileId);
            obj.NotificationProfileId = Convert.ToInt32(NotificationProfileId);
            obj.SubjectCategory = obj.SubjectCategory == null ? "" : obj.SubjectCategory;
            obj.CustomerReferenceText = obj.CustomerReferenceText == null ? "" : obj.CustomerReferenceText;
            obj.BillingEndorsementText = obj.BillingEndorsementText == null ? "" : obj.BillingEndorsementText;
            obj.Tags = obj.Tags == null ? "" : obj.Tags;
            int Id = DrFac.InsertDUNSregistration(obj);

            if (Id > 0)
            {
                switch (btnMonitorRegistration)
                {
                    case "Create DUNS Registration":
                        ViewBag.Message = MessageCollection.InsertDUNSRegistration;
                        break;
                    case "Update DUNS Registration":
                        ViewBag.Message = MessageCollection.UpdateDUNSRegistration;
                        break;
                }
            }
            ViewBag.MonitoringProfile = new SelectList(GetMonitioringProfileNames(), "Value", "Text", obj.MonitoringProfileId);
            ViewBag.NotificationProfile = new SelectList(GetNotificationProfileNames(), "Value", "Text", obj.NotificationProfileId);
            return View(obj);
        }
        #endregion

        public List<SelectListItem> GetMonitioringProfileNames()
        {
            List<SelectListItem> lstMonitoringProfileNames = new List<SelectListItem>();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
            List<MonitoringProfileEntity> lstMonitoringProfileEntity = DrFac.GetAllMonitoringProfileNames();
            lstMonitoringProfileNames.Add(new SelectListItem { Value = "-1", Text = "Select a Monitoring Profile" });
            foreach (var item in lstMonitoringProfileEntity)
            {
                lstMonitoringProfileNames.Add(new SelectListItem { Value = Convert.ToString(item.MonitoringProfileID), Text = item.ProfileName });
            }
            return lstMonitoringProfileNames;
        }

        public List<SelectListItem> GetNotificationProfileNames()
        {
            List<SelectListItem> lstNotificationProfileNames = new List<SelectListItem>();
            DUNSregistrationFacade DrFac = new DUNSregistrationFacade(this.CurrentClient.ApplicationDBConnectionString);
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
                    }
                    catch
                    {
                        return new JsonResult { Data = MessageCollection.CommanErrorMessage.ToString() };
                    }
                    return new JsonResult { Data = "Success" };
                }
            }
            return new JsonResult { Data = MessageCollection.CommanErrorMessage.ToString() };
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