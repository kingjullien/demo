using Microsoft.AspNet.Identity;
using SBISCCMWeb.LanguageResources;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [TwoStepVerification, AllowLicense, ValidateInput(true), DandBLicenseEnabled, AllowDataStewardshipLicense]
    public class FamilyTreeController : BaseController
    {
        #region Single View
        public ActionResult Index(int? id)
        {
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dtFamilyTree = fac.GetListFamilyTree();

            FamilyTreeParentModel model = new FamilyTreeParentModel();

            DataView view = new DataView(dtFamilyTree);
            DataTable distinctValues = view.ToTable(true, "FamilyTreeType");

            model.lstFamilyTreeType = new SelectList(distinctValues.AsDataView(), "FamilyTreeType", "FamilyTreeType");

            if (dtFamilyTree != null && dtFamilyTree.Rows.Count > 0)
            {
                if (id != null && id > 0)
                {
                    model.FamilyTreeId = Convert.ToInt32(id);
                }
                else
                {
                    model.FamilyTreeId = Convert.ToInt32(dtFamilyTree.Rows[0]["FamilyTreeId"]);
                }

                DataTable dtFamilyTreeById = fac.GetFamilyTreeById(model.FamilyTreeId.ToString());

                if (dtFamilyTreeById != null)
                {
                    model.FamilyTreeDetails = new FamilyTreeModel();
                    foreach (DataRow row in dtFamilyTreeById.Rows)
                    {
                        DataRow[] result = dtFamilyTree.Select("FamilyTreeType = '" + row["FamilyTreeType"].ToString() + "'");
                        model.lstFamilyTree = new SelectList(result.CopyToDataTable().AsDataView(), "FamilyTreeId", "FamilyTreeName");
                        model.FamilyTreeDetails.FamilyTreeId = row["FamilyTreeId"].ToString();
                        model.FamilyTreeDetails.FamilyTreeName = row["FamilyTreeName"].ToString();
                        model.FamilyTreeDetails.FamilyTreeType = row["FamilyTreeType"].ToString();
                        model.FamilyTreeType = model.FamilyTreeDetails.FamilyTreeType;
                        model.FamilyTreeDetails.Editable = (string.IsNullOrEmpty(row["Editable"].ToString()) ? false : Convert.ToBoolean(row["Editable"].ToString()));
                        model.FamilyTreeDetails.LockForEdit = (string.IsNullOrEmpty(row["LockForEdit"].ToString()) ? false : Convert.ToBoolean(row["LockForEdit"].ToString()));
                        if (!string.IsNullOrEmpty(row["LastRefreshedDate"].ToString()))
                        {
                            model.FamilyTreeDetails.LastRefreshedDate = Convert.ToDateTime(row["LastRefreshedDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["AlternateId"].ToString()))
                        {
                            model.FamilyTreeDetails.AlternateId = Convert.ToString(row["AlternateId"].ToString());
                        }
                        if (!string.IsNullOrEmpty(row["LastModifiedUserId"].ToString()))
                        {
                            model.FamilyTreeDetails.LastModifiedUserName = Convert.ToString(row["LastModifiedUserId"].ToString());
                        }
                    }
                }

                DataTable dt = fac.GetFamilyTree(model.FamilyTreeId);


                List<FamilyTreeDetailModel> treeLst = new List<FamilyTreeDetailModel>();
                treeLst = (from DataRow dr in dt.Rows
                           where dr["ParentFamilyTreeDetailId"].ToString() == string.Empty
                           select new FamilyTreeDetailModel()
                           {
                               DetailId = dr["DetailId"].ToString(),
                               FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                               NodeName = dr["NodeName"].ToString(),
                               ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                               NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                               NodeType = dr["NodeType"].ToString(),
                               cntChildren = 1 /* Recursively grab the children */

                           }).ToList();

                model.lstMenu = treeLst;
            }
            return View(model);

        }

        public ActionResult DetailFamilyTree(string Parameters)
        {
            int id;
            List<SBISCCMWeb.Models.FamilyTreeDetailModel> lst = new List<FamilyTreeDetailModel>();
            if (!string.IsNullOrEmpty(Parameters))
            {
                id = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));

                if (id > 0)
                {
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    DataTable dtFamilyTree = fac.GetListFamilyTree();
                    var TreeDetails = dtFamilyTree.Select("FamilyTreeId =" + id);
                    ViewBag.treeData = TreeDetails;
                    DataTable dt = fac.GetFamilyTree(Convert.ToInt32(id));

                    List<FamilyTreeDetailModel> treeLst = (from DataRow dr in dt.Rows
                               where dr["ParentFamilyTreeDetailId"].ToString() == string.Empty
                               select new FamilyTreeDetailModel()
                               {
                                   DetailId = dr["DetailId"].ToString(),
                                   FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                                   NodeName = dr["NodeName"].ToString(),
                                   ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                                   NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                                   NodeType = dr["NodeType"].ToString(),
                                   Children = GetChildren(dt, dr["FamilyTreeDetailId"].ToString()) /* Recursively grab the children */

                               }).ToList();

                    lst = treeLst;
                }
            }
            return PartialView("_DetailFamilyTreeView", lst);
        }

        private static List<FamilyTreeDetailModel> GetChildren(DataTable dt, string parentId)
        {
            return (from DataRow dr in dt.Rows
                    where dr["ParentFamilyTreeDetailId"].ToString() == parentId
                    select new FamilyTreeDetailModel()
                    {
                        DetailId = dr["DetailId"].ToString(),
                        FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                        NodeName = dr["NodeName"].ToString(),
                        ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                        NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                        NodeType = dr["NodeType"].ToString(),
                        Children = GetChildren(dt, dr["FamilyTreeDetailId"].ToString()) /* Recursively grab the children */

                    }).ToList();
        }

        public ActionResult GetCorporateLinkageDuns()
        {
            List<string> lstDuns = new List<string>();
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dtLinkage = fac.GetCorporateLinkageDuns();
            foreach (DataRow row in dtLinkage.Rows)
            {
                lstDuns.Add(Convert.ToString(row[0]));
            }

            return View("_CorporateLinkageDuns", lstDuns);
        }

        public ActionResult AddCorporateLinkageDuns(string Parameters)
        {
            string duns;
            if (!string.IsNullOrEmpty(Parameters))
            {
                duns = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);

                CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                string message = fac.PopulateCorporateLinkageDuns(duns);
                return new JsonResult { Data = new { Message = message, result = true } };
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult DeleteFamilytreeNode(string Parameters)
        {
            int sourceFamilyTreeId, sourceFamilyTreeDetailId;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    sourceFamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    sourceFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string message = fac.DeleteFamilyTreeNode(sourceFamilyTreeId, sourceFamilyTreeDetailId, Convert.ToInt32(User.Identity.GetUserId()));
                    if (message == string.Empty)
                    {
                        message = FamilyTreeLang.msgFamilyTreeNodeDeleted;
                    }
                    return new JsonResult { Data = new { Message = message, result = true } };
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddFamilyNode(string Parameters)
        {
            string sourceFamilyTreeDetailId = string.Empty;
            if (!string.IsNullOrEmpty(Parameters))
            {
                sourceFamilyTreeDetailId = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
            }
            return PartialView("_AddFamilyNode", sourceFamilyTreeDetailId);
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult AddFamilyTreeNode(string Parameters)
        {
            int FamilyTreeId;
            string DetailId, NodeName, NodeDisplayDetail, NodeType;
            int? ParentFamilyTreeDetailId = null;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    FamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    DetailId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                    NodeName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);
                    NodeDisplayDetail = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1);
                    NodeType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 4, 1);
                    if (!string.IsNullOrEmpty(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1)))
                    {
                        ParentFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 5, 1));
                    }

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string message = fac.AddFamilyTreeNode(FamilyTreeId, DetailId, NodeName, NodeDisplayDetail, NodeType, ParentFamilyTreeDetailId, Convert.ToInt32(User.Identity.GetUserId()));
                    if (message == string.Empty)
                    {
                        message = FamilyTreeLang.msgFamilyTreeCreated;
                    }
                    return new JsonResult { Data = new { Message = message, result = true } };
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddFamilyTree()
        {
            return PartialView("_AddFamilyTree");
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult AddFamilyTree(string Parameters)
        {
            string FamilyTreeName, FamilyTreeType, AlternateId;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    FamilyTreeName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                    FamilyTreeType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                    AlternateId = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string message = fac.AddFamilyTree(FamilyTreeName, FamilyTreeType, AlternateId, Convert.ToInt32(User.Identity.GetUserId()));
                    if (message == string.Empty)
                    {
                        message = FamilyTreeLang.msgFamilyTreeCreated;
                    }
                    return new JsonResult { Data = new { Message = message, result = true } };
                }
                else
                {
                    return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
                }
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
            }
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult DeleteFamilyTree(string Parameters)
        {
            int FamilyTreeId;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    FamilyTreeId = Convert.ToInt32(StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase));
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string message = fac.DeleteFamilyTree(FamilyTreeId, Convert.ToInt32(User.Identity.GetUserId()));
                    return new JsonResult { Data = new { Message = message, result = true } };
                }
                else
                {
                    return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
                }
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
            }
        }

        public ActionResult DuplicateFamilyTree()
        {
            return View("_DuplicateFamilyTree");
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult DuplicateFamilyTree(string Parameters)
        {
            int FamilyTreeId; string FamilyTreeName, FamilyTreeType;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    FamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    FamilyTreeName = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1);
                    FamilyTreeType = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1);

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    string message = fac.DuplicaeFamilyTree(FamilyTreeId, FamilyTreeName, FamilyTreeType, Convert.ToInt32(User.Identity.GetUserId()));
                    if (message == string.Empty)
                    {
                        message = FamilyTreeLang.msgDuplicateFamilyTreeCreated;
                    }
                    return new JsonResult { Data = new { Message = message, result = true } };
                }
                else
                {
                    return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
                }
            }
            catch (Exception)
            {
                return new JsonResult { Data = new { Message = CommonMessagesLang.msgCommanErrorMessage, result = false } };
            }
        }

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult MoveFamilyTree(string Parameters)
        {
            int sourceFamilyTreeId, sourceFamilyTreeDetailId, destinationFamilyTreeId, destinationFamilyTreeDetailId;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    sourceFamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    sourceFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                    destinationFamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                    destinationFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    fac.MoveFamilyTree(sourceFamilyTreeId, sourceFamilyTreeDetailId, destinationFamilyTreeId, destinationFamilyTreeDetailId, "Move", Convert.ToInt32(User.Identity.GetUserId()));
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CopyFamilyTree(string Parameters)
        {
            int sourceFamilyTreeId, sourceFamilyTreeDetailId, destinationFamilyTreeId, destinationFamilyTreeDetailId;
            try
            {
                if (!string.IsNullOrEmpty(Parameters))
                {
                    Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                    sourceFamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1));
                    sourceFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                    destinationFamilyTreeId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                    destinationFamilyTreeDetailId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    fac.MoveFamilyTree(sourceFamilyTreeId, sourceFamilyTreeDetailId, destinationFamilyTreeId, destinationFamilyTreeDetailId, "Copy", Convert.ToInt32(User.Identity.GetUserId()));
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Multiple View
        [HttpPost, RequestFromAjax, RequestFromSameDomain, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult ViewChange(string Parameters)
        {
            string isSingleView; int id, leftId, rightId;

            Response response = new Response();
            if (!string.IsNullOrEmpty(Parameters))
            {
                Parameters = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                isSingleView = Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 0, 1);
                id = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 1, 1));
                leftId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 2, 1));
                rightId = Convert.ToInt32(Utility.Utility.SplitParameters(Parameters, Utility.Utility.Colonseparator, 3, 1));

                if (isSingleView == "1")
                {
                    FamilyTreeParentModel model = new FamilyTreeParentModel();
                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    DataTable dtFamilyTree = fac.GetListFamilyTree();


                    DataView view = new DataView(dtFamilyTree);
                    DataTable distinctValues = view.ToTable(true, "FamilyTreeType");

                    model.lstFamilyTreeType = new SelectList(distinctValues.AsDataView(), "FamilyTreeType", "FamilyTreeType");

                    model.lstFamilyTree = new SelectList(dtFamilyTree.AsDataView(), "FamilyTreeId", "FamilyTreeName");
                    if (dtFamilyTree != null && dtFamilyTree.Rows.Count > 0)
                    {
                        if (id > 0)
                        {
                            model.FamilyTreeId = id;
                        }
                        else
                        {
                            model.FamilyTreeId = Convert.ToInt32(dtFamilyTree.Rows[0]["FamilyTreeId"]);
                        }

                        DataTable dtFamilyTreeById = fac.GetFamilyTreeById(model.FamilyTreeId.ToString());

                        if (dtFamilyTreeById != null)
                        {
                            model.FamilyTreeDetails = new FamilyTreeModel();
                            foreach (DataRow row in dtFamilyTreeById.Rows)
                            {
                                DataRow[] result = dtFamilyTree.Select("FamilyTreeType = '" + row["FamilyTreeType"].ToString() + "'");
                                model.lstFamilyTree = new SelectList(result.CopyToDataTable().AsDataView(), "FamilyTreeId", "FamilyTreeName");
                                model.FamilyTreeDetails.FamilyTreeId = row["FamilyTreeId"].ToString();
                                model.FamilyTreeDetails.FamilyTreeName = row["FamilyTreeName"].ToString();
                                model.FamilyTreeDetails.FamilyTreeType = row["FamilyTreeType"].ToString();
                                model.FamilyTreeType = model.FamilyTreeDetails.FamilyTreeType;
                                model.FamilyTreeDetails.Editable = (string.IsNullOrEmpty(row["Editable"].ToString()) ? false : Convert.ToBoolean(row["Editable"].ToString()));
                                model.FamilyTreeDetails.LockForEdit = (string.IsNullOrEmpty(row["LockForEdit"].ToString()) ? false : Convert.ToBoolean(row["LockForEdit"].ToString()));
                                if (!string.IsNullOrEmpty(row["LastRefreshedDate"].ToString()))
                                {
                                    model.FamilyTreeDetails.LastRefreshedDate = Convert.ToDateTime(row["LastRefreshedDate"].ToString());
                                }
                                if (!string.IsNullOrEmpty(row["AlternateId"].ToString()))
                                {
                                    model.FamilyTreeDetails.AlternateId = Convert.ToString(row["AlternateId"].ToString());
                                }
                                if (!string.IsNullOrEmpty(row["LastModifiedUserId"].ToString()))
                                {
                                    model.FamilyTreeDetails.LastModifiedUserName = Convert.ToString(row["LastModifiedUserId"].ToString());
                                }
                            }
                        }

                        DataTable dt = fac.GetFamilyTree(model.FamilyTreeId);


                        List<FamilyTreeDetailModel> treeLst = new List<FamilyTreeDetailModel>();
                        treeLst = (from DataRow dr in dt.Rows
                                   where dr["ParentFamilyTreeDetailId"].ToString() == string.Empty
                                   select new FamilyTreeDetailModel()
                                   {
                                       DetailId = dr["DetailId"].ToString(),
                                       FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                                       NodeName = dr["NodeName"].ToString(),
                                       ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                                       NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                                       NodeType = dr["NodeType"].ToString(),
                                       cntChildren = 1 //GetChildren(dt, dr["FamilyTreeDetailId"].ToString()) /* Recursively grab the children */

                                   }).ToList();

                        model.lstMenu = treeLst;
                    }
                    else
                    {
                        model = new FamilyTreeParentModel();
                    }

                    response.Success = true;
                    response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/FamilyTree/_DetailFamilyTreeView.cshtml", model);
                }
                else
                {
                    SideBySideModel model = new SideBySideModel();

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);

                    DataTable dtFamilyTree = fac.GetListFamilyTree();

                    if (dtFamilyTree != null && dtFamilyTree.Rows != null && dtFamilyTree.Rows.Count > 0)
                    {
                        if (dtFamilyTree.Select("LockForEdit = 1").Length > 0)
                        {
                            DataTable dtLefFamilyTree = dtFamilyTree.Select("LockForEdit = 1").CopyToDataTable();
                            #region Left View
                            model.LeftView = new FamilyTreeParentModel();
                            DataView viewLeft = new DataView(dtLefFamilyTree);
                            DataTable distinctValuesLeft = viewLeft.ToTable(true, "FamilyTreeType");

                            model.LeftView.lstFamilyTreeType = new SelectList(distinctValuesLeft.AsDataView(), "FamilyTreeType", "FamilyTreeType");
                            model.LeftView.lstFamilyTree = new SelectList(dtLefFamilyTree.AsDataView(), "FamilyTreeId", "FamilyTreeName");
                            if (dtLefFamilyTree != null && dtLefFamilyTree.Rows.Count > 0)
                            {
                                if (leftId > 0)
                                {
                                    model.LeftView.FamilyTreeId = leftId;
                                }

                                DataTable dtFamilyTreeById = fac.GetFamilyTreeById(model.LeftView.FamilyTreeId.ToString());

                                if (dtFamilyTreeById != null)
                                {
                                    model.LeftView.FamilyTreeDetails = new FamilyTreeModel();
                                    foreach (DataRow row in dtFamilyTreeById.Rows)
                                    {
                                        DataRow[] result = dtFamilyTree.Select("FamilyTreeType = '" + row["FamilyTreeType"].ToString() + "'");
                                        model.LeftView.lstFamilyTree = new SelectList(result.CopyToDataTable().AsDataView(), "FamilyTreeId", "FamilyTreeName");
                                        model.LeftView.FamilyTreeDetails.FamilyTreeId = row["FamilyTreeId"].ToString();
                                        model.LeftView.FamilyTreeDetails.FamilyTreeName = row["FamilyTreeName"].ToString();
                                        model.LeftView.FamilyTreeDetails.FamilyTreeType = row["FamilyTreeType"].ToString();
                                        model.LeftView.FamilyTreeType = model.LeftView.FamilyTreeDetails.FamilyTreeType;
                                        model.LeftView.FamilyTreeDetails.Editable = (string.IsNullOrEmpty(row["Editable"].ToString()) ? false : Convert.ToBoolean(row["Editable"].ToString()));
                                        model.LeftView.FamilyTreeDetails.LockForEdit = (string.IsNullOrEmpty(row["LockForEdit"].ToString()) ? false : Convert.ToBoolean(row["LockForEdit"].ToString()));
                                        if (!string.IsNullOrEmpty(row["LastRefreshedDate"].ToString()))
                                        {
                                            model.LeftView.FamilyTreeDetails.LastRefreshedDate = Convert.ToDateTime(row["LastRefreshedDate"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(row["AlternateId"].ToString()))
                                        {
                                            model.LeftView.FamilyTreeDetails.AlternateId = Convert.ToString(row["AlternateId"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(row["LastModifiedUserId"].ToString()))
                                        {
                                            model.LeftView.FamilyTreeDetails.LastModifiedUserName = Convert.ToString(row["LastModifiedUserId"].ToString());
                                        }
                                    }
                                }

                                DataTable dt = fac.GetFamilyTree(model.LeftView.FamilyTreeId);


                                List<FamilyTreeDetailModel> treeLst = new List<FamilyTreeDetailModel>();
                                treeLst = (from DataRow dr in dt.Rows
                                           where dr["ParentFamilyTreeDetailId"].ToString() == string.Empty
                                           select new FamilyTreeDetailModel()
                                           {
                                               DetailId = dr["DetailId"].ToString(),
                                               FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                                               NodeName = dr["NodeName"].ToString(),
                                               ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                                               NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                                               NodeType = dr["NodeType"].ToString(),
                                               cntChildren = 1//Children = GetChildren(dt, dr["FamilyTreeDetailId"].ToString()) /* Recursively grab the children */

                                           }).ToList();

                                model.LeftView.lstMenu = treeLst;

                            }
                            #endregion

                            #region Right View
                            model.RightView = new FamilyTreeParentModel();

                            DataView viewRight = new DataView(dtFamilyTree);
                            DataTable distinctValuesRight = viewRight.ToTable(true, "FamilyTreeType");

                            model.RightView.lstFamilyTreeType = new SelectList(distinctValuesRight.AsDataView(), "FamilyTreeType", "FamilyTreeType");
                            model.RightView.lstFamilyTree = new SelectList(dtFamilyTree.AsDataView(), "FamilyTreeId", "FamilyTreeName");
                            if (dtFamilyTree != null && dtFamilyTree.Rows.Count > 0)
                            {
                                if (rightId > 0)
                                {
                                    model.RightView.FamilyTreeId = rightId;
                                }

                                DataTable dtFamilyTreeById = fac.GetFamilyTreeById(model.RightView.FamilyTreeId.ToString());

                                if (dtFamilyTreeById != null)
                                {
                                    model.RightView.FamilyTreeDetails = new FamilyTreeModel();
                                    foreach (DataRow row in dtFamilyTreeById.Rows)
                                    {
                                        DataRow[] result = dtFamilyTree.Select("FamilyTreeType = '" + row["FamilyTreeType"].ToString() + "'");
                                        model.RightView.lstFamilyTree = new SelectList(result.CopyToDataTable().AsDataView(), "FamilyTreeId", "FamilyTreeName");
                                        model.RightView.FamilyTreeDetails.FamilyTreeId = row["FamilyTreeId"].ToString();
                                        model.RightView.FamilyTreeDetails.FamilyTreeName = row["FamilyTreeName"].ToString();
                                        model.RightView.FamilyTreeDetails.FamilyTreeType = row["FamilyTreeType"].ToString();
                                        model.RightView.FamilyTreeType = model.RightView.FamilyTreeDetails.FamilyTreeType;
                                        model.RightView.FamilyTreeDetails.Editable = (string.IsNullOrEmpty(row["Editable"].ToString()) ? false : Convert.ToBoolean(row["Editable"].ToString()));
                                        model.RightView.FamilyTreeDetails.LockForEdit = (string.IsNullOrEmpty(row["LockForEdit"].ToString()) ? false : Convert.ToBoolean(row["LockForEdit"].ToString()));
                                        if (!string.IsNullOrEmpty(row["LastRefreshedDate"].ToString()))
                                        {
                                            model.RightView.FamilyTreeDetails.LastRefreshedDate = Convert.ToDateTime(row["LastRefreshedDate"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(row["AlternateId"].ToString()))
                                        {
                                            model.RightView.FamilyTreeDetails.AlternateId = Convert.ToString(row["AlternateId"].ToString());
                                        }
                                        if (!string.IsNullOrEmpty(row["LastModifiedUserId"].ToString()))
                                        {
                                            model.RightView.FamilyTreeDetails.LastModifiedUserName = Convert.ToString(row["LastModifiedUserId"].ToString());
                                        }
                                    }
                                }

                                DataTable dt = fac.GetFamilyTree(model.RightView.FamilyTreeId);


                                List<FamilyTreeDetailModel> treeLst = (from DataRow dr in dt.Rows
                                           where dr["ParentFamilyTreeDetailId"].ToString() == string.Empty
                                           select new FamilyTreeDetailModel()
                                           {
                                               DetailId = dr["DetailId"].ToString(),
                                               FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                                               NodeName = dr["NodeName"].ToString(),
                                               ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                                               NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                                               NodeType = dr["NodeType"].ToString(),
                                               cntChildren = 1//Children = GetChildren(dt, dr["FamilyTreeDetailId"].ToString()) /* Recursively grab the children */

                                           }).ToList();

                                model.RightView.lstMenu = treeLst;
                            }
                            #endregion
                        }
                        else
                        {
                            model = new SideBySideModel();
                            model.LeftView = new FamilyTreeParentModel();
                            model.RightView = new FamilyTreeParentModel();
                            model.LeftView.lstMenu = new List<FamilyTreeDetailModel>();
                            model.RightView.lstMenu = new List<FamilyTreeDetailModel>();
                        }
                    }
                    else
                    {
                        model = new SideBySideModel();
                        model.LeftView = new FamilyTreeParentModel();
                        model.RightView = new FamilyTreeParentModel();
                        model.LeftView.lstMenu = new List<FamilyTreeDetailModel>();
                        model.RightView.lstMenu = new List<FamilyTreeDetailModel>();
                    }
                    response.Success = true;
                    response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/FamilyTree/_SIdeBySide.cshtml", model);
                }
            }
            return Json(response);
        }
        #endregion

        [HttpPost, RequestFromSameDomain, RequestFromAjax, ValidateAntiForgeryTokenOnAllPosts]
        public ActionResult BindFamilyTree(string Parameters)
        {
            string type;
            Response response = new Response();
            try
            {
                type = StringCipher.Decrypt(Parameters.Replace(Utility.Utility.urlseparator, "+"), General.passPhrase);
                if (!string.IsNullOrEmpty(Parameters))
                {
                    var list = new List<KeyValuePair<string, string>>();

                    CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
                    DataTable dtFamilyTree = fac.GetListFamilyTree();
                    DataRow[] result = dtFamilyTree.Select("FamilyTreeType = '" + Server.UrlDecode(type) + "'");
                    foreach (DataRow row in result)
                    {
                        list.Add(new KeyValuePair<string, string>(Convert.ToString(row[0]), Convert.ToString(row[1])));
                    }
                    response.Success = true;
                    response.Object = list;
                }
            }
            catch (Exception)
            {
                response.Success = false;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetData(string id, string type)
        {
            Response response = new Response();
            CompanyFacade fac = new CompanyFacade(this.CurrentClient.ApplicationDBConnectionString, Helper.UserName);
            DataTable dt = fac.GetFamilyTreeChild(id);

            List<FamilyTreeDetailModel> treeLst = (from DataRow dr in dt.Rows
                       select new FamilyTreeDetailModel()
                       {
                           DetailId = dr["DetailId"].ToString(),
                           FamilyTreeDetailId = dr["FamilyTreeDetailId"].ToString(),
                           NodeName = dr["NodeName"].ToString(),
                           ParentFamilyTreeDetailId = dr["ParentFamilyTreeDetailId"].ToString(),
                           NodeDisplayDetail = dr["NodeDisplayDetail"].ToString(),
                           NodeType = dr["NodeType"].ToString(),
                           cntChildren = Convert.ToInt16(Convert.ToString(dr["Children"]))

                       }).ToList();

            if (string.IsNullOrEmpty(type))
            {
                response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/FamilyTree/PartialItem.cshtml", treeLst);
            }
            else if (type == "right")
            {
                response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/FamilyTree/_PartialRight.cshtml", treeLst);
            }
            else if (type == "left")
            {
                response.ResponseString = RenderViewAsString.RenderPartialViewToString(this, "~/Views/FamilyTree/_PartialLeft.cshtml", treeLst);
            }
            response.Success = true;
            return Json(response);
        }

    }
}