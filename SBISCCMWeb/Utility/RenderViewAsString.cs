using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Utility
{
    public static class RenderViewAsString
    {
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    //  ViewEngines.Engines.FindView
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    return sw.GetStringBuilder().ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}