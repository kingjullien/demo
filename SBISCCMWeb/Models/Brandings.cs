using SBISCCMWeb.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace SBISCCMWeb.Models
{
    public class Brandings
    {
        public static List<Brands> AvailableBranding = new List<Brands> {
            new Brands {
                BrandType = Branding.DandB.ToString()
            },
            new Brands {
                BrandType = Branding.Matchbook.ToString()
            },
        };
        public static bool IsBrandingAvailable(string brand)
        {
            return AvailableBranding.Where(a => a.BrandType.Equals(brand)).FirstOrDefault() != null ? true : false;
        }
        public static string GetDefaultBranding()
        {
            return AvailableBranding[0].BrandType;
        }
        public static string SetBranding(string brand)
        {
            try
            {
                if (!IsBrandingAvailable(brand)) brand = GetDefaultBranding();
                HttpCookie brandCookie = new HttpCookie("culture", brand);
                brandCookie.HttpOnly = true;
                brandCookie.Expires = DateTime.Now.AddHours(8);
                HttpContext.Current.Response.Cookies.Add(brandCookie);
            }
            catch (Exception) {
                //Empty catch block to stop from breaking
            }
            return brand;
        }
    }
    public class Brands
    {
        public string BrandType
        {
            get;
            set;
        }
    }
}