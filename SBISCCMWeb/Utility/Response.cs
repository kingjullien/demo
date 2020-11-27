using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class Response
    {
        public bool Success { get; set; }
        public string ResponseString { get; set; }
        public dynamic Object { get; set; }

        public Response()
        {
            Success = true;
            ResponseString = string.Empty;
        }

        public Response(bool success, string respString)
        {
            Success = success;
            ResponseString = respString;
        }

        public Response(bool success, string respString, dynamic obj)
        {
            Success = success;
            ResponseString = respString;
            Object = obj;
        }
        public Response(bool success, string respString, dynamic obj, dynamic objSelectedItem)
        {
            Success = success;
            ResponseString = respString;
            Object = obj;
        }
    }
}