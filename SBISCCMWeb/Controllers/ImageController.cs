using SBISCCMWeb.Models;
using SBISCCMWeb.Utility;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Controllers
{
    public class ImageController : Controller
    {

        // Taken this methoad from home controller to here to reduce db calls because from home controller it was calling initialize method of base controller everytime and there was a DB Code.
        [Authorize, ValidateAccount, TwoStepVerification, DandBLicenseEnabled]
        [OutputCache(Duration = 3600, VaryByParam = "imagepath")]
        public ActionResult GetImage(string imagepath)
        {
            string noImagePath = Server.MapPath("/Images/no-image.jpg");
            string path = Server.MapPath(HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath)));
            if (!string.IsNullOrEmpty(path))
            {
                // if the user is not authenticated and direct past the url at that time we check and if not authenticate than we redirect to login page.
                if (User.Identity.IsAuthenticated)
                {
                    if (System.IO.File.Exists(path))
                    {
                        return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
                    }
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return new FileStreamResult(new FileStream(noImagePath, FileMode.Open, FileAccess.Read), "image/jpeg");
        }

        // Taken this methoad from AccountController to here to reduce db calls because from AccountController it was calling initialize method of baseController everytime and there was a DB Code.
        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "imagepath")]
        public ActionResult GetAnonymousImage(string imagepath)
        {
            string noImagePath = Server.MapPath("/Images/no-image.jpg");
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Login");
            }
            string path = Server.MapPath(HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath)));
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            {
                return new FileStreamResult(new FileStream(path, FileMode.Open, FileAccess.Read), "image/jpeg");
            }
            return new FileStreamResult(new FileStream(noImagePath, FileMode.Open, FileAccess.Read), "image/jpeg");
        }

        // This is specific function for the get image from the encrypted path and also manage url encode and if the path is exists that provide specific image.
        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "imagepath")]
        public ActionResult GetAnonymousBlobImage(string imagepath)
        {
            if (Request.UrlReferrer == null)
            {
                return RedirectToAction("Login");
            }
            string noImagePath = Server.MapPath("/Images/no-image.jpg");
            string path = HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath));
            if (!string.IsNullOrEmpty(path))
            {
                string filepath = GetImages(path);
                if (System.IO.File.Exists(filepath))
                {
                    FileStreamResult objFileStreamResult = new FileStreamResult(new FileStream(filepath, FileMode.Open), "image/jpeg");
                    return objFileStreamResult;
                }
                else
                {
                    return new FileStreamResult(new FileStream(noImagePath, FileMode.Open), "image/jpeg");
                }
            }
            else
            {
                return new FileStreamResult(new FileStream(noImagePath, FileMode.Open), "image/jpeg");
            }
        }

        [Authorize, ValidateAccount, TwoStepVerification, DandBLicenseEnabled, RequestFromSameDomain]
        [OutputCache(Duration = 3600, VaryByParam = "imagepath")]
        public ActionResult GetBlobImage(string imagepath)
        {
            string noImagePath = Server.MapPath("/Images/no-image.jpg");
            string path = HttpUtility.UrlDecode(SBISCCMWeb.Utility.Utility.GetDecryptedString(imagepath));
            if (!string.IsNullOrEmpty(path))
            {
                string filepath = GetImages(path);
                if (System.IO.File.Exists(filepath))
                {
                    FileStreamResult objFileStreamResult = new FileStreamResult(new FileStream(filepath, FileMode.Open), "image/jpeg");
                    return objFileStreamResult;
                }
                else
                {
                    return new FileStreamResult(new FileStream(noImagePath, FileMode.Open), "image/jpeg");
                }
            }
            else
            {
                return new FileStreamResult(new FileStream(noImagePath, FileMode.Open), "image/jpeg");
            }
        }
        private string GetImages(string filePath)
        {
            string testFileName;
            try
            {
                byte[] result = null;
                byte[] buffer = new byte[4097];

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(filePath);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream str = response.GetResponseStream();
                MemoryStream memoryStream = new MemoryStream();
                int count = 0;
                do
                {
                    count = str.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, count);

                    if (count == 0)
                    {
                        break;
                    }
                } while (true);

                result = memoryStream.ToArray();
                string imgPath = Server.MapPath("/Images/ClientLogos");
                if (!System.IO.Directory.Exists(imgPath))
                {
                    System.IO.Directory.CreateDirectory(imgPath);
                }
                testFileName = imgPath + Helper.ClientLogo;
                System.IO.File.WriteAllBytes(testFileName, result);
                return testFileName;
            }
            catch
            {
                return null;
            }
        }
    }
}
