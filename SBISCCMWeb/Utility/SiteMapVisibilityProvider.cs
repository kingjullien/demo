using MvcSiteMapProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace SBISCCMWeb.Utility
{

    public class SiteMapVisibilityProvider : SiteMapNodeVisibilityProviderBase
    {

        public override bool IsVisible(ISiteMapNode node, IDictionary<string, object> sourceMetadata)
        {
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "1")
            {
                if (Helper.IsApprover && Helper.Enable2StepUpdate)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "2")
            {
                if (!string.IsNullOrEmpty(Helper.UserType))
                {
                    if (Convert.ToString(Helper.UserType).ToLower() == "administrator")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "6")
            {
                if (!string.IsNullOrEmpty(Helper.UserType))
                {
                    if (Convert.ToString(Helper.UserType).ToLower() == "steward")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "7")
            {
                if (!string.IsNullOrEmpty(Helper.UserType))
                {
                    if (Convert.ToString(Helper.UserType).ToLower() == "steward")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "8")
            {
                if (!string.IsNullOrEmpty(Helper.UserType))
                {
                    if (Convert.ToString(Helper.UserType).ToLower() == "steward")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "10")
            {
                if (Helper.LicenseEnableMonitoring)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (node.Attributes.ContainsKey("pageid") && node.Attributes["pageid"].ToString() == "18")
            {
                if (!Helper.LicenseEnableMonitoring && !string.IsNullOrEmpty(Helper.UserType) && Convert.ToString(Helper.UserType).ToLower() == "steward")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;

        }
    }


}