using Microsoft.AspNet.Identity;
using Pjax.Mvc5;
using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using SBISCompanyCleanseMatchBusiness.Objects.EntitiesAndAdapters;
using SBISCompanyCleanseMatchFacade.Objects;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SBISCCMWeb.Controllers
{
    [CheckLicenseStatus, EULAValidation]
    public class BaseController : Controller, IPjax
    {
        public bool IsPjaxRequest { get; set; }
        public string PjaxVersion { get; set; }
        private BaseModel _oBaseModel = new BaseModel();
        public BaseModel oBaseModel { get { return _oBaseModel; } }

        public void SetBaseModel(BaseModel oBaseModel)
        {
            if (oBaseModel != null)
                _oBaseModel = oBaseModel;
        }

        public ClientApplicationData CurrentClient
        {
            get
            {
                if (null != _oBaseModel)
                    return _oBaseModel.CurrentClient;
                else
                    return null;
            }
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            WebHelper _webHelper = new WebHelper(requestContext.HttpContext);

            //determine the current store by HTTP_HOST
            var host = _webHelper.ServerVariables("HTTP_HOST");
            Helper.hostName = host;
            _oBaseModel = CacheHelper.Get<BaseModel>(host);
            if (_oBaseModel == null)
                _oBaseModel = CacheHelper.GetBaseModel(host, requestContext.HttpContext);

            try
            {
                // if User information is not found than need to fill current user information
                if (User != null && User.Identity.IsAuthenticated)
                {
                    if (Helper.oUser == null)
                    {
                        SettingFacade sfac = new SettingFacade(oBaseModel.CurrentClient.ApplicationDBConnectionString);
                        Helper.oUser = sfac.GetUserDetailsById(Convert.ToInt32(User.Identity.GetUserId()));
                    }
                    Helper.Enable2StepUpdate = Helper.oUser.Enable2StepUpdate;
                    Helper.IsApprover = Helper.oUser.IsApprover;
                    Helper.UserType = Helper.oUser.UserType;
                    Helper.UserName = User.Identity.GetUserName();
                }
                if (_oBaseModel == null)
                {
                    requestContext.HttpContext.Items["NoUser"] = true;
                    return;
                }
            }
            catch (Exception)
            {
                Helper.IsUserAllreadyLogin = true;
            }
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (Request.Url != null)
                {
                    var queryUrl = Request.Url.Query;
                    if (queryUrl.Contains("%26"))
                    {
                        var routeValueDictionary = new RouteValueDictionary { { "controller", "Error" }, { "action", "InternalError" } };
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValueDictionary));
                    }
                }
            }
            catch (HttpRequestValidationException)
            {
                var routeValueDictionary = new RouteValueDictionary { { "controller", "Error" }, { "action", "InternalError" } };
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValueDictionary));
            }
            if (filterContext != null && filterContext.HttpContext != null && filterContext.HttpContext.Items["NoUser"] != null && (bool)filterContext.HttpContext.Items["NoUser"])
            {
                //Return NoUser.html Page when Database not contain ApplicationSubdomain
                filterContext.Result = new RedirectResult("~/NoUser.html");
            }

        }

        protected override void OnResultExecuting(ResultExecutingContext resultExecutingContext)
        {
            var viewResult = resultExecutingContext.Result as ViewResult;
            if (viewResult != null)
            {
                viewResult.ViewBag.LayoutModel = oBaseModel;
            }
            else
            {
                var partialViewResult = resultExecutingContext.Result as PartialViewResult;
                if (partialViewResult != null && partialViewResult.ViewBag.LayoutModel == null)
                    partialViewResult.ViewBag.LayoutModel = oBaseModel;
            }

            base.OnResultExecuting(resultExecutingContext);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}