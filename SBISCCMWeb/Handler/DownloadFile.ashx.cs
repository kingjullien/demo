using SBISCCMWeb.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Handler
{
    /// <summary>
    /// Summary description for DownloadFile
    /// </summary>
    public class DownloadFile : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            string ImageName = request.QueryString["ImageName"];
            if (!string.IsNullOrEmpty(ImageName))
            {
                ImageHelper.DownloadBlob(ImageHelper.PictureType.TicketImage, ImageName);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}