using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Helpers;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		4/16 10:48:16
 * Description:		所有页面基类
 * Version History:	Created,4/16 10:48:16
 * 
 * 
 **************************************************************************/



namespace SunNet.PMNew.PM2014.Codes
{
    /// <summary>
    /// 所有页面基类
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  4/16 10:48
    public class GlobalPage : Page
    {
        private UserApplication _userApp;
        public GlobalPage(UserApplication userApp)
        {
            _userApp = userApp;
        }

        public GlobalPage()
            : this(new UserApplication())
        {

        }

        private UsersEntity _user;
        /// <summary>
        /// 基于全局页面获取用户属性，有可能为空
        /// </summary>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/20 01:48
        public UsersEntity UserInfo
        {
            get
            {
                if (_user == null)
                {
                    IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                    string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                    if (string.IsNullOrEmpty(userID))
                    {
                        return null;
                    }
                    int id = int.Parse(userID);
                    _user = _userApp.GetUser(id);
                    if (_user.UserType!= "SUNNET")
                    {
                        ResumeCookie();
                    }
                }
                return _user;
            }
        }
        private void ResumeCookie()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);

            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("FirstName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LastName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("CompanyID"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("UserType"), 30);
            UtilFactory.Helpers.CookieHelper.ResumeExpire(encrypt.Encrypt("ExpireTime"), 30);
        }

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

            if (user.UserType== "SUNNET")
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LoginUserID"), encrypt.Encrypt(user.ID.ToString()),true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_UserName_"), encrypt.Encrypt(user.UserName), true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("FirstName"), encrypt.Encrypt(user.FirstName), true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LastName"), encrypt.Encrypt(user.LastName), true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("CompanyID"), encrypt.Encrypt(user.CompanyID + ""), true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("UserType"), encrypt.Encrypt(user.UserType), true);
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("ExpireTime"), "", true);
                if (remember)
                {
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), encrypt.Encrypt(password),
                        DateTime.Now.AddDays(7));
                }
                else
                {
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), string.Empty,
                        DateTime.Now.AddSeconds(1));
                }
            }
            else
            {
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LoginUserID"), encrypt.Encrypt(user.ID.ToString()),
                DateTime.Now.AddMinutes(60));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_UserName_"), encrypt.Encrypt(user.UserName),
                    DateTime.Now.AddDays(7));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("FirstName"), encrypt.Encrypt(user.FirstName),
                    DateTime.Now.AddMinutes(60));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("LastName"), encrypt.Encrypt(user.LastName),
                    DateTime.Now.AddMinutes(60));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("CompanyID"), encrypt.Encrypt(user.CompanyID + ""),
                    DateTime.Now.AddMinutes(60));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("UserType"), encrypt.Encrypt(user.UserType),
                    DateTime.Now.AddMinutes(60));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("ExpireTime"), DateTime.Now.ToUniversalTime().AddMinutes(30).ToString(), DateTime.Now.AddMinutes(30));
                UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("UtcTimeStamp"), (DateTime.UtcNow.AddMinutes(30) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds.ToString(), DateTime.Now.AddMinutes(30));
                if (remember)
                {
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), encrypt.Encrypt(password),
                        DateTime.Now.AddDays(7));
                }
                else
                {
                    UtilFactory.Helpers.CookieHelper.Add(encrypt.Encrypt("Login_Password_"), string.Empty,
                        DateTime.Now.AddSeconds(1));
                }
            }
            

            IdentityContext.UserID = user.ID;
            IdentityContext.CompanyID = user.CompanyID;

        }

        #region ShowMessage

        protected void Alert(string message, string url)
        {
            Response.Write(
                string.Format(@"<script type='text/javascript'>alert('{0}');window.location.href='{1}'</script>",
                    message, url));
            Response.End();
            return;
        }

        public void Redirect(string msg, string url)
        {
            string rmsg =
                string.Format(
                    "<script type=\"text/javascript\">alert(\"{0}\");window.location.href = \"{1}\";</script>",
                    msg, url);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "register", rmsg);
        }

        /// <summary>
        /// 父窗口跳转到指定页面
        /// </summary>
        public void ParentToUrl(string parentReloadUrl)
        {

            string rmsg = @"<script type='text/javascript'>$(function() {sucessCall('" + parentReloadUrl +"')});</script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "register", rmsg);
        }

        /// <summary>
        /// 弹出层保存成功之后，跳转到空页面关闭窗口.
        /// </summary>
        /// <value>
        /// The empty pop page URL.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/30 17:49
        public string EmptyPopPageUrl
        {
            get { return "/PopEmpty.aspx?parentmodal=" + QS("parentmodal"); }
        }

        /// <summary>
        /// 错误页面地址.
        /// </summary>
        /// <value>
        /// The error page URL.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  5/7 14:46
        public string ErrorPageUrl
        {
            get { return "/Error/error.html?sourceurl=" + Request.Url.ToString(); }
        }

        /// <summary>
        /// 导航到URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/18 10:25
        public void Redirect(string url)
        {
            Redirect(url, false, false);
        }

        /// <summary>
        /// 导航到URL并显示成功消息.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="close">if set to <c>true</c> [close].</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/18 10:23
        public void Redirect(string url, bool success, bool close = false)
        {
            Redirect(url, success, "", close);
        }

        /// <summary>
        /// 导航到URL并显示成功消息.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="anchor">The href anchor.</param>
        /// <param name="close">if set to <c>true</c> [close].</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/18 10:23
        public void Redirect(string url, bool success, string anchor, bool close = false)
        {
            if (success && url.ToLower().IndexOf("success=1") < 0)
            {
                if (url.IndexOf("?") < 0)
                    url = url + "?success=1";
                else
                    url = url + "&success=1";
            }
            if (close && url.ToLower().IndexOf("close=1") < 0)
            {
                if (url.IndexOf("?") < 0)
                    url = url + "?close=1";
                else
                    url = url + "&close=1";
            }
            if (!string.IsNullOrEmpty(anchor))
                url = url + "#" + anchor;
            Response.Redirect(url);
        }

        /// <summary>
        /// 返回到returnurl传入的页面
        /// </summary>
        /// <param name="urlIfNoReturnUrl">没有returnurl的跳转地址，如果为空并且returnurl也为空则将跳转到登录之后的页面</param>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/25 16:26
        public void RedirectBack(string urlIfNoReturnUrl)
        {
            string url = QS("returnurl");
            if (string.IsNullOrEmpty(url))
                url = urlIfNoReturnUrl;
            if (string.IsNullOrEmpty(url))
                url = "/Default.aspx";
            url = Server.UrlDecode(url);
            Response.Redirect(url);
        }

        /// <summary>
        /// 显示消息到浏览器
        /// </summary>
        /// <param name="content">消息内容.</param>
        /// <param name="level">消息类型 success:0,info:1,warning:2,danger:3</param>
        /// <param name="reLoadParentWindow">if set to <c>true</c> [re load parent window].</param>
        /// <param name="closeWindow">if set to <c>true</c> [close window].</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public void ShowMessageToClient(string content, int level, bool reLoadParentWindow, bool closeWindow,
            string width = null, string height = null)
        {
            string reload = reLoadParentWindow ? "true" : "false";
            string close = closeWindow ? "true" : "false";
            var levels = new Dictionary<int, string> { { 0, "success" }, { 1, "info" }, { 2, "warning" }, { 3, "danger" } };

            Page.ClientScript.RegisterClientScriptBlock(
                this.GetType(),
                Request.Url.AbsolutePath,
                string.Format(@"<script type='text/javascript'>jQuery(function(){6}
                                    ShowMessage('{0}', '{1}', {2},{3},{4},{5})
                                    {7});</script>",
                    content.Replace("'", "\\'"), levels[level], reload, close, width ?? "null", height ?? "null", "{",
                    "}"));
        }

        public void ShowSuccessMessageToClient()
        {
            ShowSuccessMessageToClient(true, true);
        }

        public void ShowSuccessMessageToClient(bool reLoadParentWindow, bool closeWindow)
        {
            ShowMessageToClient("Operation successful.", 0, reLoadParentWindow, closeWindow);
        }

        public void ShowSuccessMessageToClient(string message, bool reLoadParentWindow, bool closeWindow,
            string width = null, string height = null)
        {
            ShowMessageToClient(message, 0, reLoadParentWindow, closeWindow, width, height);
        }

        public void ShowFailMessageToClient()
        {
            ShowMessageToClient("Operation failed.", 3, false, false);
        }

        public void ShowFailMessageToClient(string msg, string width = null, string height = null)
        {
            ShowMessageToClient(msg, 3, false, false, width, height);
        }

        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList)
        {
            ShowMessageToClient(FormatMessages(msgList), 3, false, false);
        }

        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList, bool reloadParentWindow, bool closeWindow)
        {
            ShowMessageToClient(FormatMessages(msgList), 3, reloadParentWindow, closeWindow);
        }

        public void ShowFailMessageToClient(List<BrokenRuleMessage> msgList, bool closeWindow)
        {
            ShowMessageToClient(FormatMessages(msgList), 3, false, closeWindow);
        }

        public void ShowArgumentErrorMessageToClient()
        {
            ShowMessageToClient("Arguments Error.", 3, false, true);
        }

        protected string FormatMessages(List<BrokenRuleMessage> list, string title = "Operation failed.")
        {
            StringBuilder sbMsgs = new StringBuilder();
            sbMsgs.AppendFormat("{0}<br/>", title);

            foreach (BrokenRuleMessage msg in list)
            {
                sbMsgs.Append(msg.Message);
                sbMsgs.Append("<br/>");
            }
            return sbMsgs.ToString();
        }

        #endregion

        #region QueryString

        /// <summary>
        /// 从URL获取当前页码.
        /// </summary>
        /// <value>
        /// The index of the current page.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 15:44
        protected int CurrentPageIndex
        {
            get { return QS("page", 1); }
        }

        protected virtual string DefaultOrderBy
        {
            get { throw new NotImplementedException("Please set default order property for every page."); }
        }

        protected virtual string DefaultDirection
        {
            get { return "asc"; }
        }

        /// <summary>
        /// Gets the order by.
        /// </summary>
        /// <value>
        /// The order by.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 14:03
        protected string OrderBy
        {
            get { return QS("order", DefaultOrderBy); }
        }

        /// <summary>
        /// Gets the order direction.
        /// </summary>
        /// <value>
        /// The order direction.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 14:03
        protected virtual string OrderDirection
        {
            get { return QS("sort", DefaultDirection); }
        }

        protected string QS(string key)
        {
            return Request.QueryString[key] + "";
        }

        protected string QS(string key, string value)
        {
            return string.IsNullOrEmpty(Request.QueryString[key]) ? value : Request.QueryString[key];
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

        /// <summary>
        /// 传递给下一页面供返回到上一页使用.
        /// </summary>
        /// <value>
        /// The return URL.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/25 16:08
        public string ReturnUrl
        {
            get
            {
                var p = this.Master as Pop;
                if (p != null)
                    return "";
                string url = QS("returnurl");
                if (!string.IsNullOrEmpty(url))
                    return Server.UrlEncode(url);
                return Server.UrlEncode(Request.RawUrl);
            }
        }

        public string ReturnUrlOfCurrentPage
        {
            get
            {
                return Server.UrlEncode(Request.RawUrl);
            }
        }
        #endregion

        #region 公用值
        /// <summary>
        /// Gets the defaul select text.
        /// </summary>
        /// <value>
        /// The defaul select text.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 15:14
        protected string DefaulSelectText { get { return "Please select..."; } }
        /// <summary>
        /// Gets the defaul all text.
        /// </summary>
        /// <value>
        /// The defaul all text.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 15:14
        protected string DefaulAllText { get { return "ALL"; } }
        #endregion

        #region commen method

        //show to client status
        public List<ClientTicketState> ClientDisAllowShowStatus()
        {
            return new List<ClientTicketState>()
            {
                ClientTicketState.Draft,
                ClientTicketState.Cancelled,
                ClientTicketState.Denied,
                ClientTicketState.Completed
            };
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
                                           (int)TicketsState.Waiting_Confirm,(int)TicketsState.Estimation_Approved
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
            int[] UnderEstimationStatus = { (int)TicketsState.PM_Verify_Estimation, (int)TicketsState.Waiting_Confirm,
                                            (int)TicketsState.Waiting_For_Estimation,(int)TicketsState.Estimation_Approved};
            return UnderEstimationStatus;
        }

        public int[] UnderWaitProcessStatus()
        {
            int[] ClientAllowShowWaitProcessStatus = { (int)ClientTicketState.Ready_For_Review, (int)ClientTicketState.Waiting_Client_Feedback };

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
        #endregion


        public bool SourceIsWinform
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                return encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("source"))).Equals("winform");
            }
        }
    }
}