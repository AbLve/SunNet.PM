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
    public class UniValidate : IUniValidate
    {
        /// <summary>Event,The server validation and produce client validation javascript code.</summary>
        public Func<HttpRequestBase, bool> ValidE;
        private string _inputName;
        public bool _isNew = true;
        public bool _checked = true;
        public Dictionary<string, string[]> _jsList = new Dictionary<string, string[]>();
        public string _errStr;
        public UniValidate(string inputName)
        {
            _inputName = inputName;
        }

        /// <summary>Add JSON childden node.</summary>
        /// <param name="type">Validation type.</param>
        /// <param name="typeValue">Value of type.</param>
        /// <param name="message">Error message.</param>
        private void AddJs(string type, string typeValue, string message)
        {
            if (!_jsList.ContainsKey(type))
            {
                _jsList.Add(type, new string[] { typeValue, message });
            }
        }

        /// <summary>Add server error message</summary>
        private void AddErrStr(string message)
        {
            _errStr += message;
            _checked = _checked && false;
        }

        /// <summary>Finally must call.</summary>
        public MvcHtmlString ToString()
        {
            //return new MvcHtmlString("<p id='mes" + _inputName  + "' class='validMes' style='display:none'></p>");
            return new MvcHtmlString("");
        }
        /// <summary>And both sides double quotation marks.</summary>
        public string AddMarks(string message)
        {
            return "\"" + message + "\"";
        }

        #region All the validation method
        /// <summary>Not Empty</summary>
        public IUniValidate NotEmpty(string message)
        {
            if (_isNew)
            {
                AddJs("required", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] == null || request[_inputName].Length == 0) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for Email format.</summary>
        public IUniValidate EmailAddress(string message)
        {
            if (_isNew)
            {
                AddJs("email", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsEmail()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for Email format.</summary>
        public IUniValidate EmailAddresses(string message)
        {
            if (_isNew)
            {
                AddJs("emails", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsEmail()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Regular verification.</summary>
        public IUniValidate Regex(string regex, string message)
        {
            if (_isNew)
            {
                AddJs("regex", AddMarks(regex), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsMatchRegex(regex)) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Chinese</summary>
        public IUniValidate AllChinese(string message)
        {
            if (_isNew)
            {
                AddJs("allChinese", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsAllChinese()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>And the other a control value is the same.</summary>
        public IUniValidate EqualTo(string equalElement, string message)
        {
            if (_isNew)
            {
                AddJs("equalTo", AddMarks("#" + equalElement), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName] != request[equalElement]) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Can't with another control equal.</summary>
        public IUniValidate NotEqualTo(string equalElement, string message)
        {
            if (_isNew)
            {
                AddJs("notequalTo", AddMarks("#" + equalElement), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName] == request[equalElement]) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>More than another control value.</summary>
        public IUniValidate MaxEqualTo(string equalElement, string message)
        {
            if (_isNew)
            {
                AddJs("maxEqualTo", AddMarks("#" + equalElement), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToDouble() > request[equalElement].ToDouble()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Digital not equal to zero.</summary>
        public IUniValidate NotEqualToValue(string equalElement, string message)
        {
            if (_isNew)
            {
                AddJs("notequalTovalue", AddMarks(equalElement), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName] == request[equalElement]) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Must not be smaller than the minimum value.</summary>
        public IUniValidate Min(string num, string message)
        {
            if (_isNew)
            {
                double value = 0;
                if (double.TryParse(num, out value))
                {
                    AddJs("min", num.ToString(), message);
                }
                else
                {
                    AddJs("min", AddMarks(num.ToString()), message);
                }
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Is not greater than the maximum.</summary>
        public IUniValidate Max(string num, string message)
        {
            if (_isNew)
            {
                AddJs("max", num.ToString(), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Length can not less than.</summary>
        public IUniValidate MinLength(int num, string message)
        {
            if (_isNew)
            {
                AddJs("minlength", num.ToString(), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].Length < num) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Length is not greater than.</summary>
        public IUniValidate MaxLength(int num, string message)
        {
            if (_isNew)
            {
                AddJs("maxlength", num.ToString(), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].Length > num) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Validation length at the specified interval.</summary>
        public IUniValidate Length(int min, int max, string message)
        {
            if (_isNew)
            {
                AddJs("rangelength", string.Format("[{0},{1}]", min, max), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && (request[_inputName].Length > max || request[_inputName].Length < min)) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Remote validation USES the way of post according to the remote return Boolen value verification.(true: it means can submit, don't display error message. false: said can't submit, display error message.)</summary>
        public IUniValidate Ajax(string url, string message, string sendDataControl)
        {
            if (_isNew)
            {
                AddJs("remote", "{url:" + AddMarks(url) + ", type:\"post\", sendDataControl:\"" + sendDataControl + "\"}", message);
            }
            return this;
        }
        /// <summary>Date verification.</summary>
        public IUniValidate Date(string message)
        {
            if (_isNew)
            {
                AddJs("date", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsDateTime()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Site address validation.</summary>
        public IUniValidate Url(string message)
        {
            if (_isNew)
            {
                AddJs("url", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsURLAddress()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Decimal type digital verification.</summary>
        public IUniValidate Decimal(string message)
        {
            if (_isNew)
            {
                AddJs("isdecimal", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsDecimal()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for positive integer.</summary>
        public IUniValidate IsInt(string message)
        {
            if (_isNew)
            {
                AddJs("isInt", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsInt()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether the detection for IP address</summary>
        public IUniValidate IsIP(string message)
        {
            if (_isNew)
            {
                AddJs("isIP", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsIP()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Does not contain Html code</summary>
        public IUniValidate NoHtml(string message)
        {
            if (_isNew)
            {
                AddJs("noHtml", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].IsHasHtml()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Can't contain Chinese or full Angle character verification</summary>
        public IUniValidate NoChinese(string message)
        {
            if (_isNew)
            {
                AddJs("noChinese", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].IsHasChinese()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        public IUniValidate CheckPWdNotNullOrEmptyLength(string message)
        {
            if (_isNew)
            {
                AddJs("checkPwdLeng", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>To judge a standard character if contains non-standard error character.</summary>
        public IUniValidate IsLegalXmlString(string message)
        {
            if (_isNew)
            {
                AddJs("IsLegalXmlString", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && !request[_inputName].IsLegalXmlChar()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Don't match the regular.</summary>
        public IUniValidate NotRegex(string regex, string message)
        {
            if (_isNew)
            {
                AddJs("notRegex", AddMarks(regex), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].IsMatchRegex(regex)) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Char maximum length.</summary>
        public IUniValidate CharCodeLength(int num, string message)
        {
            if (_isNew)
            {
                AddJs("charCodeLength", num.ToString(), message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].CharCodeLength() > num) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for mobile phone format.</summary>
        public IUniValidate IsMobile(string message)
        {
            if (_isNew)
            {
                AddJs("mobile", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].IsMobilePhone()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for fixed-line format.</summary>
        public IUniValidate IsTel(string message)
        {
            if (_isNew)
            {
                AddJs("tel", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].IsTelephone()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for postal code format.</summary>
        public IUniValidate IsPostcode(string message)
        {
            if (_isNew)
            {
                AddJs("postcode", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].IsPostcode()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for QQ number, QQ number from the beginning of 10000.</summary>
        public IUniValidate IsQQ(string message)
        {
            if (_isNew)
            {
                AddJs("qq", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].IsQQ()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        /// <summary>Whether for user name format, 4 to 16 characters between.</summary>
        public IUniValidate IsUserName(string message)
        {
            if (_isNew)
            {
                AddJs("username", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].IsUserName()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }

        /// <summary>DropDownList judge whether a selected (default first value value is 1 used to indicate the user to select).</summary>
        public IUniValidate SelectNone(string message)
        {
            if (_isNew)
            {
                AddJs("selectnone", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].SelectNone()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }

        /// <summary>CheckBoxList judge whether a selected.</summary>
        public IUniValidate atLeastOneChecked(string message)
        {
            if (_isNew)
            {
                AddJs("atleastonechecked", "true", message);
                ValidE += (HttpRequestBase request) =>
                {
                    if (request[_inputName] != null && request[_inputName].ToString() != "" && !request[_inputName].atLeastOneChecked()) AddErrStr(message);
                    return _checked;
                };
            }
            return this;
        }
        #endregion
    }
}
