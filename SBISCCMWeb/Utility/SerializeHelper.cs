using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBISCCMWeb.Utility
{
    public class SerializeHelper
    {
        public static string SerializeObject<T>(T obj)
        {
            return (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(obj);
        }

        public static T DeserializeString<T>(string strInput)
        {
            T objT = (T)((new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize(strInput, typeof(T)));
            return (T)(object)objT;
        }

    }
}