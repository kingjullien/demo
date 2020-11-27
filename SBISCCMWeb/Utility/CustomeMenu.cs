using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBISCCMWeb.Utility
{
    public static class CustomeMenu
    {
        //use for set Active and Deactivate menu
        public static IHtmlString RouteIf(this HtmlHelper helper, string Controllervalue, string attribute, string Actionvalue)
        {
            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();
            var currentAction =
                (helper.ViewContext.RequestContext.RouteData.Values["action"] ?? string.Empty).ToString().UnDash();

            var hasController = Controllervalue.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            var hasAction = Actionvalue != null ? Actionvalue.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase) : false; ;
            return hasAction && hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }

        public static IHtmlString RouteIfParent(this HtmlHelper helper, string Controllervalue, string attribute)
        {
            var currentController =
                (helper.ViewContext.RequestContext.RouteData.Values["controller"] ?? string.Empty).ToString().UnDash();

            var hasController = Controllervalue.Equals(currentController, StringComparison.InvariantCultureIgnoreCase);
            return hasController ? new HtmlString(attribute) : new HtmlString(string.Empty);
        }
    }
    public static class StringExtensions
    {
        /// <summary>
        ///     Removes dashes ("-") from the given object value represented as a string and returns an empty string ("")
        ///     when the instance type could not be represented as a string.
        ///     <para>
        ///         Note: This will return the type name of given isntance if the runtime type of the given isntance is not a
        ///         string!
        ///     </para>
        /// </summary>
        /// <param name="value">The object instance to undash when represented as its string value.</param>
        /// <returns></returns>
        public static string UnDash(this object value)
        {
            return ((value as string) ?? string.Empty).UnDash();
        }

        /// <summary>
        ///     Removes dashes ("-") from the given string value.
        /// </summary>
        /// <param name="value">The string value that optionally contains dashes.</param>
        /// <returns></returns>
        public static string UnDash(this string value)
        {
            return (value ?? string.Empty).Replace("-", string.Empty);
        }
    }
}