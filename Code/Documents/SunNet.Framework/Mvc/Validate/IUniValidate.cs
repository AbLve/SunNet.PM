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
    public interface IUniValidate
    {
        #region Validate Interface
        /// <summary>Must use at last.</summary>
        MvcHtmlString ToString();
        /// <summary>Not Empty.</summary>
        IUniValidate NotEmpty(string message = "This field cannot be empty.");
        /// <summary>Whether for Email format.</summary>
        IUniValidate EmailAddress(string message = "Please input a well-formatted email address.");
        /// <summary>Whether for Email list format.</summary>
        IUniValidate EmailAddresses(string message = "Please input the well-formatted email address list.");
        /// <summary>Regular expression format</summary>
        IUniValidate Regex(string regex, string message = "Incorrect.");
        ///// <summary>It's Chinese</summary>
        //IUniValidate AllChinese(string message = "It must be Chinese!");
        /// <summary>Equal to.</summary>
        IUniValidate EqualTo(string equalElement, string message = "It is unequal.");
        /// <summary>Not equal to other form element value.</summary>
        IUniValidate NotEqualTo(string equalElement, string message = "It is equal! It must be unequal.");
        /// <summary>Number can't be 0.</summary>
        IUniValidate NotEqualToValue(string equalElement, string message = "It is equal! It must be unequal.");
        /// <summary>More than another control value</summary>
        IUniValidate MaxEqualTo(string equalElement, string message = "Please input data not to exceed the maximum.");
        /// <summary>Must not be smaller than the minimum value.</summary>
        IUniValidate Min(string num, string message = "This field cannot be smaller than the minimum value.");
        /// <summary>Is not greater than the maximum.</summary>
        IUniValidate Max(string num, string message = "This field cannot be greater than the maximum value.");
        /// <summary>Length can not less than.</summary>
        IUniValidate MinLength(int num, string message = "The length of the data is short.");
        /// <summary>Length is not greater than.</summary>
        IUniValidate MaxLength(int num, string message = "The length of the data is too long.");
        /// <summary>Validation length at the specified interval.</summary>
        IUniValidate Length(int min, int max, string message = "The length is incorrect.");

        /// <summary>
        /// Remote validate, use post method, According to the remote return Boolen value verification.
        /// true:Said can submit, don't show the error message.
        /// false:Said can't submit, display error message.
        /// </summary>
        /// <param name="url">Post url</param>
        /// <param name="message">Tip</param>
        /// <param name="sendDataControl">Send the data in the control,If it is more control, use commas.</param>
        /// <returns></returns>
        IUniValidate Ajax(string url, string message, string sendDataControl = "");

        /// <summary>Date Validation.</summary>
        IUniValidate Date(string message = "Date is incorrect.");
        /// <summary>Site address validation.</summary>
        IUniValidate Url(string message = "The url is incorrect.");
        /// <summary>Decimal type digital verification.</summary>
        IUniValidate Decimal(string message = "Invalid data. It must be decimal.");
        /// <summary>If it is integer.</summary>
        IUniValidate IsInt(string message = "Invalid data. It must be integer.");
        /// <summary>Whether the detection for IP address.</summary>
        IUniValidate IsIP(string message = "IP is incorrect.");
        /// <summary>Does not contain Html code.</summary>
        IUniValidate NoHtml(string message = "Format of HTML is incorrect.");
        /// <summary>Can't contain Chinese or full Angle character verification.</summary>
        IUniValidate NoChinese(string message = "Cannot input HTML code.");
        /// <summary>To judge a standard character if contains non-standard error character</summary>
        IUniValidate IsLegalXmlString(string message = "Format of XML is incorrect.");
        /// <summary>Don't match the regular.</summary>
        IUniValidate NotRegex(string regex, string message);
        /// <summary>Char maximum length.</summary>
        IUniValidate CharCodeLength(int num, string message = "Length is incorrect! It must be char type.");
        ///// <summary>Whether for mobile phone format.</summary>
        //IUniValidate IsMobile(string message = "");
        ///// <summary>Whether for fixed-line format.</summary>
        //IUniValidate IsTel(string message = "");
        /// <summary>Whether for postal code format.</summary>
        IUniValidate IsPostcode(string message = "Please input the correct format of postcode.");
        ///// <summary>Whether for QQ number, QQ number from the beginning of 10000.</summary>
        //IUniValidate IsQQ(string message = "");
        /// <summary>Whether for user name format, 4 to 16 characters between.</summary>
        IUniValidate IsUserName(string message = "User Name format error.");

        /// <summary>DropDownList judge whether a selected (Default first value value is 1 used to indicate the user to select).</summary>
        IUniValidate SelectNone(string message = "Please select at least one.");
        /// <summary>CheckBoxList judge whether a selected.</summary>
        IUniValidate atLeastOneChecked(string message = "Please select at least one.");

        IUniValidate CheckPWdNotNullOrEmptyLength(string message = "The password must be 8 to 16 characters long.");
        #endregion
    }
}
