using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Text.RegularExpressions;

using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.UI;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.Codes
{
    public class GlobalPageBase : Page
    {
        UserApplication userApp = new UserApplication();
        /// <summary>
        /// Login and set cookie
        /// </summary>
        /// <param name="user">Login user</param>
        /// <param name="password"></param>
        /// <param name="remember"></param>
        /// <param name="url">null mean default,a url to redirect</param>
        public void LoginSystem(UsersEntity user, string password, bool remember)
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
            string loginUseridEncrypt = encrypt.Encrypt("LoginUserID");
            string userIdEncrypt = encrypt.Encrypt(user.ID.ToString());

            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LoginUserID"), encrypt.Encrypt(user.ID.ToString()), DateTime.Now.AddMinutes(30));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_UserName_"), encrypt.Encrypt(user.UserName), DateTime.Now.AddDays(7));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("FirstName"), encrypt.Encrypt(user.FirstName), DateTime.Now.AddMinutes(30));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LastName"), encrypt.Encrypt(user.LastName), DateTime.Now.AddMinutes(30));
            UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("CompanyID"), encrypt.Encrypt(user.CompanyID + ""), DateTime.Now.AddMinutes(30));
            if (remember)
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), encrypt.Encrypt(password), DateTime.Now.AddDays(7));
            }
            else
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), string.Empty, DateTime.Now.AddSeconds(1));
            }

            IdentityContext.UserID = user.ID;
            IdentityContext.CompanyID = user.CompanyID;

        }

        #region ShowMessage
        protected void Alert(string message, string url)
        {
            Response.Write(string.Format(@"<script type='text/javascript'>alert('{0}');window.location.href='{1}'</script>", message, url));
            Response.End();
            return;
        }
        protected void Redirect(string msg, string url)
        {
            string rmsg = string.Format("<script type=\"text/javascript\">alert(\"{0}\");window.location.href = \"{1}\";</script>",
                                                             msg, url);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "register", rmsg);
        }

        protected void Redirect(string url)
        {
            string rmsg = string.Format("<script type=\"text/javascript\">window.location.href = \"{0}\";</script>",
                                                              url);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "register", rmsg);
        }

        protected void ShowMessageAndRedirect(string msg, string redirectUrl)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                                                            Request.Url.AbsolutePath,
                                                            string.Format(@"<script type='text/javascript'>ShowMessageAndRedirect('{0}', '{1}');</script>",
                                                            msg.Replace("'", "\\'"), redirectUrl));
        }


        /// <summary>
        /// ShowMessageToBrowser
        /// </summary>
        /// <param name="content"></param>
        /// <param name="level">message level infomation:0,warning:1,error:2</param>
        /// <param name="reLoadParentWindow"></param>
        /// <param name="closeWindow"></param>
        public void ShowMessageToClient(string content, int level, bool reLoadParentWindow, bool closeWindow, string width = null, string height = null)
        {
            string reload = reLoadParentWindow ? "true" : "false";
            string close = closeWindow ? "true" : "false";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(),
                                                            Request.Url.AbsolutePath,
                                                            string.Format(@"<script type='text/javascript'>ShowMessage('{0}', {1}, {2},{3},{4},{5});</script>",
                                                            content.Replace("'", "\\'"), level, reload, close, width == null ? "null" : width, height == null ? "null" : height));
        }

        public void ShowSuccessMessageToClient()
        {
            ShowSuccessMessageToClient(true, true);
        }

        public void ShowSuccessMessageToClient(bool reLoadParentWindow, bool closeWindow)
        {
            ShowMessageToClient("Operation successful.", 0, reLoadParentWindow, closeWindow);
        }

        public void ShowSuccessMessageToClient(string message, bool reLoadParentWindow, bool closeWindow, string width = null, string height = null)
        {
            ShowMessageToClient(message, 0, reLoadParentWindow, closeWindow, width, height);
        }

        public void ShowFailMessageToClient()
        {
            ShowMessageToClient("Operation failed.", 0, false, false);
        }
        public void ShowFailMessageToClient(string msg, string width = null, string height = null)
        {
            ShowMessageToClient(msg, 0, false, false, width, height);
        }
        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList)
        {
            ShowMessageToClient(FormatMessages(msgList), 0, false, false);
        }

        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList, bool reloadParentWindow, bool closeWindow)
        {
            ShowMessageToClient(FormatMessages(msgList), 0, reloadParentWindow, closeWindow);
        }

        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList, bool closeWindow)
        {
            ShowMessageToClient(FormatMessages(msgList), 0, false, closeWindow);
        }
        public void ShowArgumentErrorMessageToClient()
        {
            ShowMessageToClient("Arguments Error.", 2, false, true);
        }

        protected string FormatMessages(List<BrokenRuleMessage> list)
        {
            StringBuilder sbMsgs = new StringBuilder();
            foreach (BrokenRuleMessage msg in list)
            {
                sbMsgs.Append(msg.Message);
                sbMsgs.Append("<br/>");
            }
            return sbMsgs.ToString();
        }
        #endregion

        #region QueryString
        protected string QS(string key)
        {
            return Request.QueryString[key] + "";
        }

        protected int QS(string key, int v)
        {
            int result;
            if (int.TryParse(QS(key), out result))
                return result;
            return v;
        }

        protected int? QS(string key, int? v)
        {
            int result;
            if (int.TryParse(QS(key), out result))
                return result;
            return v;
        }

        protected DateTime QS(string key, DateTime v)
        {
            DateTime result;
            if (DateTime.TryParse(QS(key), out result))
                return result;
            return v;
        }

        protected string QF(string key)
        {
            return Request.Form[key] + "";
        }

        protected int QF(string key, int v)
        {
            int result;
            if (int.TryParse(QF(key), out result))
                return result;
            return v;
        }
        #endregion

        #region commen method

        //show to client status
        public int[] ClientDisAllowShowStatus()
        {
            int[] DisAllowShowToClientStatus = { (int)ClientTicketState.Draft, (int)ClientTicketState.Cancelled,
                                                 (int)ClientTicketState.Completed};
            return DisAllowShowToClientStatus;

        }

        public int[] ClientAllowShowWaitProcessStatus()
        {
            int[] ClientAllowShowWaitProcessStatus = { (int)TicketsState.Ready_For_Review };
            return ClientAllowShowWaitProcessStatus;

        }

        public int[] ClientAllowShowStatus()
        {
            //allow status
            int[] ClientAllowShowStatus = {(int)TicketsState.Submitted,          (int)TicketsState.PM_Reviewed,
                                           (int)TicketsState.Developing,         (int)TicketsState.Waiting_For_Estimation,
                                           (int)TicketsState.Testing_On_Client,    (int)TicketsState.Testing_On_Local ,
                                           (int)TicketsState.Tested_Fail_On_Client,   (int)TicketsState.Tested_Fail_On_Local,
                                           (int)TicketsState.Tested_Success_On_Client,    (int)TicketsState.Tested_Success_On_Local, 
                                           (int)TicketsState.PM_Deny,             (int)TicketsState.PM_Verify_Estimation ,
                                           (int)TicketsState.Not_Approved,        (int)TicketsState.Ready_For_Review,
                                           (int)TicketsState.Waiting_Sales_Confirm,(int)TicketsState.Estimation_Approved
                                };
            return ClientAllowShowStatus;
        }

        public int[] UnderDevelopingStatus()
        {
            //developing UnderDevelopingStatus
            int[] UnderDevelopingStatus = {(int)TicketsState.Testing_On_Client,   (int)TicketsState.Tested_Success_On_Client,
                                         (int)TicketsState.Tested_Fail_On_Client,  (int)TicketsState.Tested_Fail_On_Local,
                                         (int)TicketsState.Testing_On_Local,(int)TicketsState.Tested_Success_On_Local, 
                                         (int)TicketsState.Developing,    (int)TicketsState.PM_Deny,
                                         (int)TicketsState.PM_Reviewed   
                                      };
            return UnderDevelopingStatus;
        }

        public int[] UnderEstimationStatus()
        {
            int[] UnderEstimationStatus = { (int)TicketsState.PM_Verify_Estimation, (int)TicketsState.Waiting_Sales_Confirm,
                                            (int)TicketsState.Waiting_For_Estimation,(int)TicketsState.Estimation_Approved};
            return UnderEstimationStatus;
        }

        public int[] UnderWaitProcessStatus()
        {
            int[] ClientAllowShowWaitProcessStatus = { (int)ClientTicketState.Ready_For_Review, (int)ClientTicketState.Waiting_Feedback };

            return ClientAllowShowWaitProcessStatus;
        }

        public string ReturnTicketId(string keyWord)
        {
            if (keyWord.Length == 0) return "";
            string ticketId = string.Empty;
            Regex reg = new Regex("^[BbRr]{1}[0-9]{1,}$");
            MatchCollection matches = reg.Matches(keyWord, 0);
            if (matches != null && matches.Count == 1)
            {
                ticketId = keyWord.Remove(0, 1);
                return ticketId;
            }
            else
            {
                return keyWord;
            }
        }

        public void SetDefaultValueForDropdownList<T>(DropDownList ddl, List<T> list)
        {
            if (list.Count > 0)
            {
                ddl.DataSource = list;
                ddl.DataBind();
                ddl.Items.Insert(0, new ListItem("Please select...", ""));
                ddl.SelectedIndex = list.Count == 1 ? 1 : 0;
            }
        }

        public string ShowActionByFbMsg(string fbMsg)
        {
            if (string.IsNullOrEmpty(fbMsg))
            {
                return "";
            }
            return "class ='action'";
        }

        public List<ListItem> ConvertEnumtToListItem(Type type)
        {
            List<ListItem> result = new List<ListItem>();

            string[] names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new ListItem() { Text = names[i], Value = values.GetValue(i).ToString() });
            }
            result.Sort(new RoleEnumComparer());

            return result;
        }
        #endregion
    }
}
