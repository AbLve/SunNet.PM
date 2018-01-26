using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;

namespace SF.Framework.Mvc.Extension
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList);
        }

        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList);
        }

        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, htmlAttributes);
        }
        public static MvcHtmlString YesOrNoBoolFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "False" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "True" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, htmlAttributes);
        }

        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optionLabel, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, optionLabel);
        }

        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optionLabel, IDictionary<string, object> htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString YesOrNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string optionLabel, object htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownListFor<TModel, TProperty>(htmlHelper, expression, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString YesOrNo(this HtmlHelper htmlHelper, string name, IDictionary<string, object> htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownList(htmlHelper, name, selectList);
        }

        public static MvcHtmlString YesOrNo(this HtmlHelper htmlHelper, string name, object htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, htmlAttributes);
        }

        public static MvcHtmlString YesOrNo(this HtmlHelper htmlHelper, string name, string optionLabel, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel);
        }

        public static MvcHtmlString YesOrNo(this HtmlHelper htmlHelper, string name, string optionLabel, IDictionary<string, object> htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString YesOrNo(this HtmlHelper htmlHelper, string name, string optionLabel, object htmlAttributes, string defaultText)
        {
            IList<SelectListItem> selectList = new List<SelectListItem>();
            if (defaultText != "")
            {
                selectList.Add(new SelectListItem { Selected = true, Text = defaultText, Value = "-1" });
            }
            selectList.Add(new SelectListItem { Text = "No", Value = "0" });
            selectList.Add(new SelectListItem { Text = "Yes", Value = "1" });
            return SelectExtensions.DropDownList(htmlHelper, name, selectList, optionLabel, htmlAttributes);
        }

        public static MvcHtmlString RadioButtonList(this HtmlHelper htmlHelper, string name, string codeCategory, string direction, RepeatDirection repeatDirection = RepeatDirection.Horizontal, IDictionary<string, object> htmlAttributes = null)
        {
            var codes = CodeManager.GetCodes(codeCategory);
            return GenerateHtml(name, codes, direction, repeatDirection, htmlAttributes, null);
        }

        public static MvcHtmlString RadioButtonListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string codeCategory, string direction, RepeatDirection repeatDirection = RepeatDirection.Horizontal, IDictionary<string, object> htmlAttributes = null)
        {
            var codes = CodeManager.GetCodes(codeCategory);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            var attributes = htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata);
            foreach (var item in attributes)
            {
                htmlAttributes.Add(item);
            }
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string stateValue = (string)metadata.Model;
            return GenerateHtml(fullHtmlFieldName, codes, direction, repeatDirection, htmlAttributes, stateValue);
        }

        private static MvcHtmlString GenerateHtml(string name, Collection<CodeDescription> codes, string direction, RepeatDirection repeatDirection, IDictionary<string, object> htmlAttributes, string stateValue = null)
        {
            TagBuilder table = new TagBuilder("table");
            int i = 0;
            if (repeatDirection == RepeatDirection.Horizontal)
            {
                TagBuilder tr = new TagBuilder("tr");
                foreach (var code in codes)
                {
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");
                    td.InnerHtml = GenerateRadioHtml(name, id, code.Description, code.Code, direction, (stateValue != null && stateValue == code.Code), htmlAttributes);
                    tr.InnerHtml += td.ToString();
                }
                table.InnerHtml = tr.ToString();
            }
            else
            {
                foreach (var code in codes)
                {
                    TagBuilder tr = new TagBuilder("tr");
                    i++;
                    string id = string.Format("{0}_{1}", name, i);
                    TagBuilder td = new TagBuilder("td");
                    td.InnerHtml = GenerateRadioHtml(name, id, code.Description, code.Code, direction, (stateValue != null && stateValue == code.Code), htmlAttributes);
                    tr.InnerHtml = td.ToString();
                    table.InnerHtml += tr.ToString();
                }
            }
            return new MvcHtmlString(table.ToString());
        }

        private static string GenerateRadioHtml(string name, string id, string labelText, string value, string direction, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();
            TagBuilder label = new TagBuilder("label");
            label.MergeAttribute("for", id);
            label.SetInnerText(labelText);
            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("name", name);
            input.MergeAttribute("type", "radio");
            input.MergeAttribute("value", value);
            input.MergeAttributes(htmlAttributes);
            if (isChecked)
            {
                input.MergeAttribute("checked", "checked");
            }
            if (direction == "R")
            {
                sb.AppendLine(input.ToString());
                sb.AppendLine(label.ToString());
            }
            else
            {
                sb.AppendLine(label.ToString());
                sb.AppendLine(input.ToString());
            }
            return sb.ToString();
        }

        public static MvcHtmlString FileFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IDictionary<string, object> htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText(expression);
            var attributes = htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata);
            foreach (var item in attributes)
            {
                htmlAttributes.Add(item);
            }
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string stateValue = (string)metadata.Model;
            return new MvcHtmlString(GenerateFileHtml(fullHtmlFieldName, fullHtmlFieldName, htmlAttributes));
        }

        private static string GenerateFileHtml(string name, string id, IDictionary<string, object> htmlAttributes)
        {
            StringBuilder sb = new StringBuilder();
            TagBuilder input = new TagBuilder("input");
            input.GenerateId(id);
            input.MergeAttribute("name", name);
            input.MergeAttribute("type", "file");
            input.MergeAttributes(htmlAttributes);
            sb.AppendLine(input.ToString());
            return sb.ToString();
        }
    }
}
