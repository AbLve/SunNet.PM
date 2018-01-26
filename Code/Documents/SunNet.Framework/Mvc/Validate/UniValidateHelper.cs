using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using SF.Framework.Mvc.Extension;

namespace SF.Framework.Mvc.Validate
{
    public class UniValidateHelper
    {
        private static Dictionary<string, Dictionary<string, UniValidate>> all_valids = new Dictionary<string, Dictionary<string, UniValidate>>();
        private static Dictionary<string, string> all_Js = new Dictionary<string, string>();
        private string _pageName;
        private string _js;
        Dictionary<string, UniValidate> valids;
        public string ErrStr = "";

        private void Init(string pageName, bool refresh)
        {
            _pageName = pageName.ToLower();
            if (refresh)
            {
                all_valids.Remove(_pageName);
                all_Js.Remove(_pageName);
            }
            if (all_valids.ContainsKey(_pageName))
            {
                valids = all_valids[_pageName];
                if (all_Js.ContainsKey(_pageName)) _js = all_Js[_pageName];
            }
            else
            {
                #region about lock
                //lock (all_valids)
                //Don't need it and The Times less, if required strictly, to add, as follows:

                lock (all_valids)
                {
                    if (all_valids.ContainsKey(_pageName))
                    {
                        valids = all_valids[_pageName];
                        if (all_Js.ContainsKey(_pageName)) _js = all_Js[_pageName];
                    }
                    else
                    {
                        valids = new Dictionary<string, UniValidate>();
                        all_valids.Add(_pageName, valids);
                    }
                }
                #endregion
                //valids = new Dictionary<string, UniValidate>();
                //all_valids.Add(_pageName, valids);
            }
        }

        /// <summary> Validation auxiliary class. </summary>
        /// <param name="pageName">As the Key of cache to submit the form shall prevail, address to rewrite or form more use. </param>
        public UniValidateHelper(string pageName, bool refresh = false)
        {
            Init(pageName, refresh);
        }
        /// <summary> Validation auxiliary class. </summary>
        /// <param name="routaData">Routing ViewContext. RouteData, used to produce page names such as: Account/LogOn</param>
        public UniValidateHelper(RouteData routaData, bool refresh = false)
        {
            Init(GetPageName(routaData), refresh);
        }
        /// <summary>Began to control verification.</summary>
        /// <param name="ctrName">To verify the ID of the control that</param>
        public IUniValidate Bind(string ctrName)
        {
            if (valids.ContainsKey(ctrName))
            {
                valids[ctrName]._isNew = false;
                return valids[ctrName];
            }
            else
            {
                lock (valids)
                {
                    if (!valids.ContainsKey(ctrName))
                    {
                        UniValidate vd = new UniValidate(ctrName);
                        valids.Add(ctrName, vd);
                        return vd;
                    }
                    else
                    {
                        return valids[ctrName];
                    }
                }
            }
        }
        /// <summary>Began to control verification.</summary>
        /// <param name="expression">To validate the control attribute name:() => Model.AttrName </param>
        public IUniValidate BindFor<T>(System.Linq.Expressions.Expression<Func<T>> expression)
        {
            return Bind(((System.Linq.Expressions.MemberExpression)expression.Body).Member.Name);
        }
        /// <summary>Clear all cache.</summary>
        public void ClearAll()
        {
            all_valids.Clear();
        }
        /// <summary>Verify all, Controllers in the call.</summary>
        /// <param name="controller">Controller, general for this</param>
        /// <param name="pageName">When display page and submit page medium not the same time, write and submit the page display address, not submitted to address such as: Account/LogOn. </param>
        public bool CheckAll(Controller controller)
        {
            bool res = valids.Count > 0;
            //a lot of cache refresh will appear: set has been amended; May not be able to execute enumeration operation mistake, no lock because should not appear a lot of refresh buffer operation, if required strictly, to add.
            foreach (var item in valids)
            {
                lock (item.Value)
                {
                    item.Value._checked = true;
                    item.Value._errStr = "";
                    if (item.Value.ValidE != null) res = item.Value.ValidE(controller.Request) && res;
                    ErrStr += item.Value._errStr;
                }
            }
            controller.ViewData["ErrStr"] = ErrStr;//Asp.net no contr.ViewBag
            return res;
        }
        /// <summary>The last call, can put in the motherboard.</summary>
        public MvcHtmlString ValidEnd()
        {
            if (_js == null)
            {
                StringBuilder rulesJs = new StringBuilder();
                StringBuilder messagesJs = new StringBuilder();
                foreach (var item in valids)
                {
                    rulesJs.Append(",\"" + item.Key + "\": {");
                    messagesJs.Append(",\"" + item.Key + "\": {");
                    foreach (var jsItem in item.Value._jsList)
                    {
                        rulesJs.Append(jsItem.Key + ":" + jsItem.Value[0] + ",");
                        messagesJs.Append(jsItem.Key + ":\"" + jsItem.Value[1] + "\",");
                    }
                    if (rulesJs.ToString().LastIndexOf(',') != -1)
                        rulesJs.Remove(rulesJs.Length - 1, 1);
                    if (messagesJs.ToString().LastIndexOf(',') != -1)
                        messagesJs.Remove(messagesJs.Length - 1, 1);
                    rulesJs.Append("}\r\n");
                    messagesJs.Append("}\r\n");
                }
                _js = "$(function(){"
                    + "_Options" + FormID + " = {\r\n"
                    + "rules: {\r\n" + rulesJs.ToString().TrimStart(',') + "}\r\n"
                    + ",messages:{\r\n" + messagesJs.ToString().TrimStart(',') + "}\r\n"
                    + ", errorClass: \"field-validation-error\", errorElement: \"span\""
                    + (OnKeyupCheck ? "" : ", onkeyup: false\r\n")
                    + (FocusCleanup ? ", focusCleanup: true\r\n" : "")
                    + (Onfocusout ? ", onfocusout: true\r\n" : "")
                    + (ShowErrorTo == null ? showErrorsHide() : showErrorsToOne(ShowErrorTo))
                    + ", success: 'clear'"
                    + "\r\n"
                    + "}; \r\n"
                    + "$('#" + FormID + "').data('" + FormID + "',_Options" + FormID + ");"
                    + "});\r\n";
                //Don't need it and The Times less, if required strictly, to add
                if (!all_Js.ContainsKey(_pageName)) all_Js.Add(_pageName, _js);
            }
            return new MvcHtmlString("<script type=\"text/javascript\" defer=\"defer\">\r\n"
                + _js + "</script>");
        }
        /// <summary>Get page name</summary>
        /// <param name="rData">Route</param>
        public static string GetPageName(RouteData rData)
        {
            return rData.Values["controller"] + "/" + rData.Values["action"];
        }

        #region To set properties, attribute the revised need to refresh the cache
        string showErrorsHide()
        {
            return ", showErrors: function (errorMap, errorList) {\r\n"
                    + "     this.defaultShowErrors();\r\n"
                    + "         if (errorList.length > 0) {\r\n"
                    + "             for (key in errorMap) {\r\n"
                //+ "                 $('#' + key).parent().find('.field-validation-error').hide()\r\n"
                    + "             };\r\n"
                    + "         }\r\n"
                    + "   }\r\n";
        }
        string showErrorsToOne(string elementId)
        {
            return ", showErrors: function showErrorInDiv(errorMap, errorList) {\r\n"
                    + "     $('.combo-value').parent().removeClass('field-validation-error-foreasyui');\r\n"
                    + "     $('.combo-value').prev().prev().removeClass('field-validation-error-foreasyui-input');\r\n"
                    + "     $('input[type=text],textarea,select').removeClass('field-validation-error');\r\n"
                    + "     $('#" + elementId + "').show();\r\n"
                    + "     if (this.errorList.length > 0) {\r\n"
                    + "         var showHTML = '';\r\n"
                    + "         for(var i = 0 ; i < this.errorList.length; i++){\r\n"
                    + "             if ($(errorList[i].element).hasClass('combo-value')) {\r\n"
                    + "                 $($(errorList[i].element).parent()).addClass('field-validation-error-foreasyui');\r\n"
                    + "                 $($(errorList[i].element).prev().prev()).addClass('field-validation-error-foreasyui-input');\r\n"
                    + "             }\r\n"
                    + "             $(errorList[i].element).addClass('field-validation-error');\r\n"
                    + "             showHTML += '<span class=\"field-validation-error\" for=\"' + $(errorList[i].element).attr('name') + '\" generated=\"true\">' + errorList[i].message + '</span>';\r\n"
                    + "         }\r\n"
                    + "         $('#" + elementId + "').html(showHTML);\r\n"
                    + "     }\r\n"
                    + "     else {\r\n"
                    + "         $('#" + elementId + "').hide();\r\n"
                    + "         $('#" + elementId + "').html('&nbsp;');\r\n"
                    + "     }\r\n"
                    + "   }\r\n";
        }

        public string FormID { get; set; }
        /// <summary>The default false: only submit to test and verify, true: as long as lose focus and validation.</summary>
        public bool Onfocusout { get; set; }
        /// <summary>The default true: key bounce whether detection.</summary>
        public bool OnKeyupCheck { get; set; }
        /// <summary>Get clear focus error.</summary>
        public bool FocusCleanup { get; set; }
        /// <summary>The error information display in the location specified.</summary>
        public string ShowErrorTo { get; set; }
        #endregion
    }
}
