using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SF.Framework.Core.BrokenMessage;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SF.Framework.Result
{
    public class FunctionResult : BrokenMessageEnabled, IBrokenMessageBinder
    {
        public FunctionResult()
        {
            this.BoolResult = true;
            this.IntResult = 0;
            this.StringResult = string.Empty;
        }
        public bool BoolResult { get; set; }
        public int IntResult { get; set; }
        public string StringResult { get; set; }
        public object ObjectResult { get; set; }

        /// <summary>
        /// First brokenrulemessage
        /// </summary>
        public string FirstBrokenRuleMessageHtml
        {
            get
            {
                if (this.BrokenRuleMessages.Count > 0)
                {
                    return string.Format("{0}:{1}", this.FirstBrokenRuleMessage.Key, this.FirstBrokenRuleMessage.Message);
                }
                return "";
            }
        }
        /// <summary>
        /// All brokenrulemessages
        /// </summary>
        public string BrokenRuleMessagesHtml
        {
            get
            {
                if (this.BrokenRuleMessages.Count > 0)
                {
                    StringBuilder html = new StringBuilder();
                    foreach (var item in this.BrokenRuleMessages)
                    {
                        html.AppendFormat("{0}:{1}", item.Key, item.Message);
                        html.Append("\r\n");
                    }
                    return html.ToString();
                }
                return "";
            }
        }

        /// <summary>
        /// Show Messages to page
        /// </summary>
        /// <param name="page"></param>
        public void BindBrokenMessages2WebPageBindableFieldValidators(System.Web.UI.Page page)
        {
            var container = page.Form.Controls[0];
            if (container == null || container.Controls.Count == 0)
                container = page.Form.Controls[1];
            HtmlContainerControl divError = (HtmlContainerControl)container.FindControl("divServerMsg");
            if (divError != null)
            {
                divError.Style.Remove("display");
                if (this.BrokenRuleMessages.Count == 0)
                    divError.Style.Add("display", "none");
                else
                    divError.Style.Add("display", "block");
            }
            Literal ltlClientErrorMsg = (Literal)container.FindControl("ltlClientErrorMsg");
            if (ltlClientErrorMsg != null)
                ltlClientErrorMsg.Visible = false;

            Literal ltlServerErrorMsg = (Literal)container.FindControl("ltlServerErrorMsg");
            if (ltlServerErrorMsg != null)
                ltlServerErrorMsg.Text = " There may be some errors among your field(s) in the form,please fix them and try again.<br/>";

            Literal ltlMsg = (Literal)container.FindControl("ltlServerMsgs");
            if (ltlMsg != null)
                ltlMsg.Text = this.BrokenRuleMessagesHtml;
        }

        public void BindBrokenMessages2WinformErrorProvider(System.Windows.Forms.ErrorProvider errorProvider, System.Windows.Forms.Control.ControlCollection controls)
        {
            throw new NotImplementedException();
        }

        public void BindBrokenMessages2JQuery(System.Web.UI.Page page)
        {
            throw new NotImplementedException();
        }
    }
}
