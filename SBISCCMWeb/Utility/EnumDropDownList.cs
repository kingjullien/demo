using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SBISCCMWeb.Utility
{
    public static class EnumDropDown
    {
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if ((attributes != null) && (attributes.Length > 0))
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor(htmlHelper, expression, null);
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            // If the enum is nullable, add an 'empty' item to the collection
            if (metadata.IsNullableValueType)
                items = SingleEmptyItem.Concat(items);

            return htmlHelper.DropDownListFor(expression, items, htmlAttributes);
        }

        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression) where TModel : class
        {
            return htmlHelper.EnumRadioButtonListFor(expression, null);
        }

        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes) where TModel : class
        {
            return htmlHelper.EnumRadioButtonListFor(expression, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString EnumRadioButtonListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, IDictionary<string, object> htmlAttributes) where TModel : class
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            Type enumType = GetNonNullableModelType(metadata);
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            if (values == null)
                return MvcHtmlString.Empty;

            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = GetEnumDescription(value),
                                                    Value = value.ToString(),
                                                    Selected = value.Equals(metadata.Model)
                                                };

            string content = string.Empty;
            var sb = new StringBuilder();
            foreach (var item in items)
            {
                content += RadioButton(htmlHelper, metadata.PropertyName, new SelectListItem { Text = item.Text, Selected = item.Selected, Value = item.Value.ToString() }, htmlAttributes);
            }

            return MvcHtmlString.Create(content);
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, SelectListItem listItem,
                             IDictionary<string, object> htmlAttributes)
        {

            //TODO dit kan beter met htmlHelper.RadioButtonFor(expression, item.Value, new { id = id, @class = htmlAttributes["class"] }).ToHtmlString();

            var id = new StringBuilder();

            id.Append(name)
              .Append("_")
              .Append(listItem.Value);

            var sb = new StringBuilder();

            var label = new TagBuilder("label");

            label.MergeAttribute("class", "radio");

            var radio = new TagBuilder("input");

            radio.MergeAttribute("type", "radio");
            radio.MergeAttribute("value", listItem.Value);
            radio.MergeAttribute("id", id.ToString());
            radio.MergeAttribute("name", name);
            radio.MergeAttributes(htmlAttributes);

            if (listItem.Selected)
            {
                radio.MergeAttribute("checked", "checked");
            }

            label.InnerHtml = radio.ToString() + listItem.Text;

            return label.ToString();
        }

        public static TProperty GetValue<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            TModel model = htmlHelper.ViewData.Model;
            if (model == null)
            {
                return default(TProperty);
            }
            Func<TModel, TProperty> func = expression.Compile();
            return func(model);
        }
    }

    //Created a  common enum ProviderType of both ProvidersMenu and ExportType
    public enum ProviderType
    {
        [Description("D&B Provider")]
        DandB,
        [Description("Orb Provider")]
        OI
    }
    //Created a  common enum Branding for both Matchbook and and DandB
    public enum Branding
    {
        [Description("Dun & Bradstreet")]
        DandB,
        [Description("Matchbook")]
        Matchbook
    }

    //Created a common enum PendoEvents for passing event name in Pendo API
    public enum PendoEvents
    {
        [Description("Search Data")]
        SearchData
    }
    public enum ThirdPartyProvider
    {
        DNB,
        ORB,
        GOOG,
        DESCARTES
    }
    public enum ThirdPartyCode
    {
        DNB_BUILD_A_LIST,
        DNB_INVESTIGATIONS,
        DNB_SINGLE_ENTITY_SEARCH,
        DNB_TYPEAHEAD_SEARCH,
        GOOGLE,
        ORB_BUILD_A_LIST,
        ORB_SINGLE_ENTITY_SEARCH,
    }
    public enum ThirdPartyProperties
    {
        ThirdPartyProvider, Code, APICredential, AuthToken, APIType
    }

    public enum ImportProcessStatus
    {
        [Description("Not Processed")]
        NotProcessed,
        Processing,
        Processed,
        Exporting,
        Exported,
        Archiving,
        Archived,
        [Description("Awaiting Approval")]
        AwaitingApproval
    }

    public enum ResearchRequestType
    {
        Mini,
        Targeted
    }

    public enum CVRefTypeCode
    {
        USER_STATUS = 101,
        USER_TYPE = 102,
        USER_LAYOUT_PREFERENCE = 103,
        ATTRIBUTE_DATA_TYPE = 104,
        LOGIN_STATUS = 105,
        THIRD_PARTY_PROVIDER = 106,
        DNB_API_TYPE = 107,
        QUEUE = 201,
        ETL_TYPE = 501,
        TAG_TYPE = 601
    }

}