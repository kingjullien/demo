using SBISCCMWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Views.Base
{
    public abstract class ViewBaseWithLayoutModel : WebViewPage
    {
        public BaseModel LayoutModel { get { return (BaseModel)ViewBag.LayoutModel; } }
    }
    public abstract class ViewBaseWithLayoutModel<T> : WebViewPage<T>
    {
        public BaseModel LayoutModel { get { return (BaseModel)ViewBag.LayoutModel; } }
    }
}