using SBISCCMWeb.Models;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    [Authorize, TwoStepVerification]
    public class ErrorController : BaseController
    {
        // GET: Error/PageNotFound
        [HandleError]
        public ActionResult PageNotFound()
        {
            // Display Page not found page and handle 404 error
            return View();
        }
        [HandleError]
        public ActionResult InternalError()
        {
            // Display internal Error page and handle 500 error
            return View();
        }

    }
}