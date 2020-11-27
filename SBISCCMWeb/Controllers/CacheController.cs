using SBISCCMWeb.Utility;
using System;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    public class CacheController : Controller
    {
        public ActionResult Clear(string pass)
        {
            if (string.Compare(pass, "2660FFCC-CDC9-473B-A307-0648DD60B85D", true) == 0)
            {
                if (CacheHelper.ClearCache(this.HttpContext))
                {
                    return Content("Cache is cleared");
                }
                else
                    return Content("Cache could not be cleared.");
            }
            else
                return Content("Invalid Password");
        }
    }
}