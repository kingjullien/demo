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
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification, BrandingLicense]
    public class TicketController : BaseController
    {
        #region Get Ticket List
        //[Route("Ticket/{page?}/{sortby?}/{sortorder?}/{pagevalue?}")]
        public ActionResult Index(int? page, int? sortby, int? sortorder, int? pagevalue)
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

            #region Set Viewbag

            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortorder;
            ViewBag.pageno = currentPageIndex;
            ViewBag.pagevalue = pageSize;

            #endregion


            TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
            List<TicketEntity> model = new List<TicketEntity>();

            //Get Ticket List
            model = tac.GetTicketListByUser(Request.Url.Authority, Helper.UserName, sortParam, currentPageIndex, pageSize, out totalCount).ToList();

            //Get Paged Ticket List
            IPagedList<TicketEntity> pagedTicket = new StaticPagedList<TicketEntity>(model.ToList(), currentPageIndex, pageSize, totalCount);
            if (Request.IsAjaxRequest())
                return PartialView("_Index", pagedTicket);
            else
                return View("Index", pagedTicket);
        }

        #endregion

        #region Create Ticket
        [HttpGet, Authorize]
        [Route("Ticket/Create")]
        public ActionResult Create()
        {
            TicketEntity model = new TicketEntity();
            model.ClientInformation = Request.Url.Authority;
            model.ApplicationUser = Convert.ToString(!string.IsNullOrEmpty(SessionHelper.ClientName) ? SessionHelper.ClientName : CurrentClient.ClientName);
            model.EnteredBy = Helper.UserName;
            model.PrimaryEmailAddress = Helper.oUser.EmailAddress;

            #region remove files from blob storage on Page Load.

            if (!string.IsNullOrEmpty(SessionHelper.ImageName))
            {
                //List all file name
                string[] lstImages = SessionHelper.ImageName.ToString().TrimEnd(':').Split(':');
                for (int i = 0; i < lstImages.Count(); i++)
                {
                    string ImageName = lstImages[i].ToString();
                    if (!string.IsNullOrEmpty(ImageName))
                    {
                        //Delete file from blob storage
                        ImageHelper.DeleteBlob(ImageHelper.PictureType.TicketImage, ImageName);
                        SessionHelper.ImageName = SessionHelper.ImageName.Replace(ImageName, "");
                    }
                }
                SessionHelper.ImageName = string.Empty;
                model.Files = SessionHelper.ImageName;
            }
            #endregion

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(true), RequestFromSameDomain]
        [Route("Ticket/Create/{model?}")]
        public ActionResult Create(TicketEntity model)
        {
            ModelState.Remove("AssignedTo");
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(SessionHelper.ImageName))
                        model.Files = SessionHelper.ImageName.TrimEnd(':');

                    model.EnteredBy = Helper.UserName;
                    TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
                    //Save Ticket
                    int ticketId = tac.InsertUpdateTicketForClient(model);  // MP-846 Admin database cleanup and code cleanup.-CLIENT

                    if (ticketId > 0)
                    {
                        #region Send Notification on success
                        // When Create new ticket at time send mail user for creation of ticket.
                        TicketEntity objTicket = new TicketEntity();
                        objTicket = tac.GetTicketByIDByClients(ticketId);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                        string SupportEmail = ConfigurationManager.AppSettings["ApplicationEmail"];
                        string emailBody = string.Empty;
                        if (Helper.Branding == Branding.Matchbook.ToString())
                        {
                            emailBody += "Hi, " + model.EnteredBy + "<br/><br/>";
                            emailBody += MessageCollection.MatchbookTicketText + "<br/><br/>";
                            emailBody += "<table width='100%'>";
                            emailBody += "<tr><td style='width:11%;'>Title :</td><td>" + model.Title + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Priority :</td><td>" + objTicket.PriorityValue + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Status :</td><td>" + objTicket.CurrentStatusValue + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Description :</td><td>" + model.IssueDescription + "</td></tr>";
                            emailBody += "</table>";
                            Helper.SendMail(model.PrimaryEmailAddress, MessageCollection.MatchbookTicketCreated, emailBody, SupportEmail);
                        }
                        else if (Helper.Branding == Branding.DandB.ToString())
                        {
                            emailBody += "Hi, " + model.EnteredBy + "<br/><br/>";
                            emailBody += MessageCollection.DandBTicketText + "<br/><br/>";
                            emailBody += "<table width='100%'>";
                            emailBody += "<tr><td style='width:11%;'>Title :</td><td>" + model.Title + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Priority :</td><td>" + objTicket.PriorityValue + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Status :</td><td>" + objTicket.CurrentStatusValue + "</td></tr>";
                            emailBody += "<tr><td style='width:11%;'>Description :</td><td>" + model.IssueDescription + "</td></tr>";
                            emailBody += "</table>";
                            Helper.SendMail(model.PrimaryEmailAddress, MessageCollection.DandBTicketCreated, emailBody, SupportEmail);
                        }
                        #endregion
                    }
                    SessionHelper.TicketMessage = CommonMessagesLang.msgCommanInsertMessage;
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        #endregion

        #region Edit Ticket
        [HttpGet]
        [Route("Ticket/Edit")]
        public ActionResult Edit(string Parameters)
        {
            // Get Query string in Encrypted mode and decrypt Query string and set Parameters
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }

            int id = Convert.ToInt32(Parameters);
            TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
            TicketEntity model = new TicketEntity();
            TicketEntity objTicket = new TicketEntity();
            if (id > 0)
            {
                //Get ticket Details for given ticket Id
                model = tac.GetLastTicketAuditByTicketIdByClient(id);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                objTicket = tac.GetTicketByIDByClients(id);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                //Store Files in tempdata
                SessionHelper.ImageName = Convert.ToString(model.Files);
            }

            #region Ticket History

            List<TicketAuditEntity> ticketAuditList = new List<TicketAuditEntity>();

            //Get All Ticket Audit for given ticket Id
            ticketAuditList = tac.GetTicketAuditByTicketIdByClient(id);     // MP-846 Admin database cleanup and code cleanup.-CLIENT

            List<TicketHistory> ticketHistoryList = new List<TicketHistory>();
            if (ticketAuditList.Any())
            {
                #region History for Main Record

                TicketHistory ticketHistoryMain = new TicketHistory();
                List<ItemModification> itemModificationMainList = new List<ItemModification>();
                ticketHistoryMain.ChangedByName = ticketAuditList.First().ChangedByUser;

                #region Property Info

                Type elementNewMain = ticketAuditList.First().GetType();
                Type elementOldMain = objTicket.GetType();

                PropertyInfo[] propertyInfosNewMain = elementNewMain.GetProperties();
                PropertyInfo[] propertyInfosOldMain = elementOldMain.GetProperties();
                //Set the Value for the Priority for the Ticket
                for (int j = 0; j < propertyInfosNewMain.Length; j++)
                {
                    var fieldName = propertyInfosNewMain[j].Name;
                    if (fieldName == "StatusValue" || fieldName == "PriorityValue" || fieldName == "AssignedToName")
                    {
                        #region Set OldFieldName

                        string oldFieldName = string.Empty;
                        switch (fieldName)
                        {
                            case "StatusValue":
                                oldFieldName = "CurrentStatusValue";
                                break;
                            case "PriorityValue":
                                oldFieldName = "PriorityValue";
                                break;
                            case "AssignedToName":
                                oldFieldName = "AssignedToName";
                                break;
                            case "Notes":
                                oldFieldName = "IssueDescription";
                                break;
                        }

                        #endregion

                        if (fieldName == "StatusValue")
                            fieldName = "Status";
                        else if (fieldName == "PriorityValue")
                            fieldName = "Priority";
                        else if (fieldName == "AssignedToName")
                            fieldName = "AssignedTo";

                        var newValue = propertyInfosNewMain[j].GetValue(ticketAuditList.First());
                        var oldValue = elementOldMain.GetProperty(oldFieldName).GetValue(objTicket);
                        string strNewValue = newValue == null ? null : newValue.ToString();
                        string strOldValue = oldValue == null ? null : oldValue.ToString();
                        if (strNewValue != strOldValue)
                        {
                            ItemModification itemModification = new ItemModification();
                            itemModification.FieldName = fieldName;
                            itemModification.OldValue = strOldValue;
                            itemModification.NewValue = strNewValue;
                            itemModificationMainList.Add(itemModification);
                        }
                    }
                }

                #endregion

                ticketHistoryMain.itemModification = itemModificationMainList;
                ticketHistoryMain.ModificationDate = ticketAuditList.First().AuditDateTime;
                ticketHistoryMain.Note = ticketAuditList.First().Notes;
                ticketHistoryList.Add(ticketHistoryMain);

                #endregion

                for (int i = 0; i < ticketAuditList.Count - 1; i++)
                {
                    TicketHistory ticketHistory = new TicketHistory();
                    List<ItemModification> itemModificationList = new List<ItemModification>();
                    ticketHistory.ChangedByName = ticketAuditList[i + 1].ChangedByUser;

                    #region Property Info

                    Type elementNew = ticketAuditList[i + 1].GetType();
                    Type elementOld = ticketAuditList[i].GetType();

                    PropertyInfo[] propertyInfosNew = elementNew.GetProperties();
                    PropertyInfo[] propertyInfosOld = elementOld.GetProperties();

                    for (int j = 0; j < propertyInfosNew.Length; j++)
                    {
                        var fieldName = propertyInfosNew[j].Name;
                        if (fieldName == "StatusValue" || fieldName == "PriorityValue" || fieldName == "AssignedToName")
                        {
                            if (fieldName == "StatusValue")
                                fieldName = "Status";
                            else if (fieldName == "PriorityValue")
                                fieldName = "Priority";
                            else if (fieldName == "AssignedToName")
                                fieldName = "AssignedTo";

                            var newValue = propertyInfosNew[j].GetValue(ticketAuditList[i + 1]);
                            var oldValue = propertyInfosOld[j].GetValue(ticketAuditList[i]);
                            string strNewValue = newValue == null ? null : newValue.ToString();
                            string strOldValue = oldValue == null ? null : oldValue.ToString();

                            if (strNewValue != strOldValue)
                            {
                                ItemModification itemModification = new ItemModification();
                                itemModification.FieldName = fieldName;
                                itemModification.OldValue = strOldValue;
                                itemModification.NewValue = strNewValue;
                                itemModificationList.Add(itemModification);
                            }
                        }
                    }

                    #endregion

                    ticketHistory.itemModification = itemModificationList;
                    ticketHistory.ModificationDate = ticketAuditList[i + 1].AuditDateTime;
                    ticketHistory.Note = ticketAuditList[i + 1].Notes;
                    ticketHistoryList.Add(ticketHistory);
                }
            }

            SessionHelper.TicketHistory = JsonConvert.SerializeObject(ticketHistoryList);
            #endregion

            return View(model);
        }
        // Edit Specific Ticket and post the data.
        [HttpPost, ValidateInput(true), ValidateAntiForgeryToken, RequestFromSameDomain]
        [Route("Ticket/Edit/{model?}")]
        public ActionResult Edit(TicketEntity model)
        {
            ModelState.Remove("AssignedTo");
            if (ModelState.IsValid)
            {
                if (model != null)
                {
                    TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
                    TicketAuditEntity auditmodel = new TicketAuditEntity();

                    TicketEntity model1 = tac.GetLastTicketAuditByTicketIdByClient(model.Id);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                    auditmodel.AssignedTo = model.AssignedTo;
                    auditmodel.Priority = model.Priority;
                    auditmodel.Status = model1.CurrentStatus;// model.CurrentStatus;
                    auditmodel.Notes = model.Note;
                    auditmodel.TicketId = model.Id;
                    //Get Logged in User Name
                    auditmodel.ChangedByUser = Helper.UserName;
                    //Get Uploaded file name from tempdata
                    string Files = SessionHelper.ImageName;
                    //Save ticket data
                    tac.InsertUpdateTicketAuditForClient(auditmodel, model.PrimaryEmailAddress, model.PrimaryContactNumber, Files, model.Title, model.TicketType);   // MP-846 Admin database cleanup and code cleanup.-CLIENT
                    SessionHelper.TicketMessage = CommonMessagesLang.msgCommanUpdateMessage;
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Edit", new { Parameters = StringCipher.Encrypt(model.Id.ToString(), General.passPhrase) });
        }

        #endregion

        #region Upload and Remove File
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult UploadImage(HttpPostedFileBase file, int? TicketId)
        {
            List<string> lstFiles = new List<string>();
            if (CommonMethod.CheckFileType(".jpg,.jpeg,.png,.txt,.docx,.doc,.xlsx,.xls,.pdf", file.FileName.ToLower()))
            {
                if (file != null)
                {

                    FileInfo oFileInfo = new FileInfo(file.FileName);
                    string fileExtension = oFileInfo.Extension;
                    string TicketImage = System.DateTime.Now.Ticks + fileExtension.ToLower();
                    //Check for uploaded file is image or not

                    if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".jpeg" || fileExtension.ToLower() == ".png")
                    {
                        //Upload Image
                        ImageHelper.UploadImage(file, TicketImage, ImageHelper.PictureType.TicketImage);
                    }
                    else
                    {
                        //Upload Other file
                        ImageHelper.UploadNONImage(file, TicketImage, ImageHelper.PictureType.TicketImage);
                    }

                    //Set ImageName tempdata
                    if (string.IsNullOrEmpty(SessionHelper.ImageName))
                        SessionHelper.ImageName = TicketImage;
                    else
                        SessionHelper.ImageName = SessionHelper.ImageName + ":" + TicketImage;

                    string Files = SessionHelper.ImageName.TrimEnd(':');
                    for (int i = 0; i < Files.Split(':').Length; i++)
                    {
                        if (!string.IsNullOrEmpty(Files.Split(':')[i]))
                        {
                            //Add file in list if it exist on blob
                            if (ImageHelper.IsFileExists(Files.Split(':')[i].ToString(), ImageHelper.PictureType.TicketImage))
                                lstFiles.Add(Files.Split(':')[i].ToString());
                        }
                    }

                    if (TicketId != null)
                    {
                        //Update Files in ticket table
                        TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
                        TicketEntity objTicket = tac.GetTicketByIDByClients(TicketId.Value);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                        objTicket.Files = Files;
                        int result = tac.UpdateTicketFileForClients(objTicket);  // MP-846 Admin database cleanup and code cleanup.-CLIENT

                    }
                }
            }
            else
            {
                return new JsonResult { Data = CommonMessagesLang.msgWrongFormat };
            }

            IPagedList<string> pagedProducts = new StaticPagedList<string>(lstFiles, 1, 10000, 0);
            return PartialView("_UploadImage", pagedProducts);
        }
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult RemoveImage(string ImageName, int? TicketId)
        {
            List<string> lstFiles = new List<string>();
            if (!string.IsNullOrEmpty(ImageName))
            {
                if (!string.IsNullOrEmpty(SessionHelper.ImageName))
                {
                    string FilesName = SessionHelper.ImageName.TrimEnd(':');
                    for (int i = 0; i < FilesName.Split(':').Length; i++)
                    {
                        if (FilesName.Split(':')[i].ToString() != ImageName)
                        {
                            //Add file in list if it exist on blob
                            if (!string.IsNullOrEmpty(FilesName.Split(':')[i]))
                            {
                                if (ImageHelper.IsFileExists(FilesName.Split(':')[i].ToString(), ImageHelper.PictureType.TicketImage))
                                    lstFiles.Add(FilesName.Split(':')[i].ToString());
                            }
                        }
                    }
                }
                //Delete file from blob storage
                ImageHelper.DeleteBlob(ImageHelper.PictureType.TicketImage, ImageName);
                SessionHelper.ImageName = SessionHelper.ImageName.Replace(ImageName, "").Replace("::", ":").TrimEnd(':');

                if (TicketId != null)
                {
                    //Update Files in ticket table
                    TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
                    TicketEntity objTicket = tac.GetTicketByIDByClients(TicketId.Value);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                    objTicket.Files = SessionHelper.ImageName.Trim(':');
                    int result = tac.UpdateTicketFileForClients(objTicket);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
                }
            }
            IPagedList<string> pagedProducts = new StaticPagedList<string>(lstFiles, 1, 10000, 0);
            return PartialView("_UploadImage", pagedProducts);
        }

        #endregion

        #region Other Methods

        public static List<TicketStatus> GetTicketStatus(string Code, string ConnectionString)
        {
            TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
            List<TicketStatus> lstStatus = new List<TicketStatus>();
            lstStatus = tac.GetAttributeTypeForTicketForClients(Code).ToList();  // MP-846 Admin database cleanup and code cleanup.-CLIENT
            if (Code == "101") //101 = PRIORITY_STATUS
            {
                lstStatus = lstStatus.OrderByDescending(x => x.Code).ToList();
            }
            else if (Code == "102") //101 = TICKET_STATUS
            {
                if (System.Web.HttpContext.Current.Request.Url.AbsolutePath.IndexOf("edit") < 0)
                    lstStatus = lstStatus.Where(x => x.Code == "102001").ToList(); //102001=Created
                else
                    lstStatus = lstStatus.Where(x => x.Code == "102001" || x.Code == "102005").ToList(); //102001=Created, 102005=Closed
            }
            else if (Code == "103" && System.Web.HttpContext.Current.Request.Url.AbsolutePath.IndexOf("edit") < 0)//103 = TICKET SOURCE
            {
                lstStatus = lstStatus.Where(x => x.Value == "Web Application").ToList();
            }
            return lstStatus;

        }

        //Close Ticket
        [HttpPost, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        [Route("Ticket/CloseTicket/{id?}/{pageNo?}/{pageSize?}")]
        public ActionResult CloseTicket(int id, int pageNo, int pageSize)
        {
            TicketEntity objTicket = new TicketEntity();
            TicketFacade tac = new TicketFacade(StringCipher.Decrypt(Helper.GetMasterConnctionstring(), General.passPhrase));
            objTicket = tac.GetTicketByIDByClients(id);  // MP-846 Admin database cleanup and code cleanup.-CLIENT
            TicketAuditEntity objAudit = new TicketAuditEntity();
            objAudit.Status = 102005; // 102005=Closed
            objAudit.AssignedTo = 0;
            objAudit.ChangedByUser = Helper.UserName;
            objAudit.Priority = objTicket.Priority;
            objAudit.Notes = objTicket.IssueDescription;
            objAudit.TicketId = id;
            tac.InsertUpdateTicketAuditForClient(objAudit, null, null, null, null, 0);      // MP-846 Admin database cleanup and code cleanup.-CLIENT
            int finalsortOrder = 12;
            int totalCount = 0;
            List<TicketEntity> model = new List<TicketEntity>();
            //List all ticket
            model = tac.GetTicketListByUser(Request.Url.Authority, Helper.UserName, finalsortOrder, pageNo, pageSize, out totalCount).ToList();
            IPagedList<TicketEntity> pagedTicket = new StaticPagedList<TicketEntity>(model.ToList(), pageNo, pageSize, totalCount);
            if (Request.IsAjaxRequest())
                return PartialView("_Index", pagedTicket);
            else
                return View("Index", pagedTicket);
        }

        #endregion

    }
}