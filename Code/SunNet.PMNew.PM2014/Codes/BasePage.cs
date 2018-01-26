using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.UI;
using NPOI.HSSF.Record.Formula.Functions;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		4/16 10:46:06
 * Description:		已登录页面基类
 * Version History:	Created,4/16 10:46:06
 * 
 * 
 **************************************************************************/
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.TicketModel;
using System.Web.UI.WebControls;
using System.IO;

namespace SunNet.PMNew.PM2014.Codes
{
    /// <summary>
    /// 已登录页面基类
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  4/16 10:46
    public class BasePage : GlobalPage
    {
        readonly UserApplication _userApp;
        public BasePage(UserApplication userApp)
        {
            _userApp = userApp;
        }
        public BasePage()
            : this(new UserApplication())
        {

        }

        protected override void OnPreInit(EventArgs e)
        {
            ViewStateUserKey = Session.SessionID;
            CheckAuthority();
            if (!Context.Items.Contains("USERINFO"))
                Context.Items.Add("USERINFO", UserInfo);
            base.OnPreInit(e);
        }

        protected void CheckAuthority()
        {
            if (this.UserInfo == null)
            {
                Response.Write(string.Format(@"<script type='text/javascript'>window.top.location.href='/Login.aspx?returnurl={0}';</script>", ReturnUrl));
                Response.End();
                return;
            }

            bool canVisit = _userApp.RoleCanAccessPage(this.UserInfo.RoleID, Request.Url.AbsolutePath.ToLower());
            if ((this.UserInfo.Role == RolesEnum.Contactor || this.UserInfo.Role == RolesEnum.Supervisor) &&
                Request.Url.AbsolutePath.ToLower() == "/account/profile.aspx")
                Response.Redirect("/account/changepassword.aspx");
            if (!canVisit)
            {
                Response.Write(string.Format(@"<script type='text/javascript'>window.top.location.href='/Login.aspx?returnurl={0}';</script>", ReturnUrl));
                Response.End();
            }
        }

        protected bool CheckRoleCanAccessPage()
        {
            int roleID = UserInfo.RoleID;
            string page = Request.Url.AbsolutePath.ToLower();
            return _userApp.RoleCanAccessPage(roleID, page);
        }
        public bool CheckRoleCanAccessPage(string page)
        {
            int roleID = UserInfo.RoleID;
            return _userApp.RoleCanAccessPage(roleID, page);
        }
        public bool IsReadOnlyRole
        {
            get
            {
                List<RolesEnum> list = new List<RolesEnum>();
                list.Add(RolesEnum.DEV);
                list.Add(RolesEnum.Leader);
                list.Add(RolesEnum.QA);
                list.Add(RolesEnum.Contactor);
                if (list.Contains(UserInfo.Role))
                {
                    return true;
                }
                return false;
            }
        }

        public bool ISReadOnlyRole
        {
            get
            {
                List<RolesEnum> list = new List<RolesEnum>();
                list.Add(RolesEnum.DEV);
                list.Add(RolesEnum.Leader);
                list.Add(RolesEnum.QA);
                list.Add(RolesEnum.Contactor);
                if (list.Contains(UserInfo.Role))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsInternalUser
        {
            get
            {
                List<RolesEnum> list = new List<RolesEnum>();
                list.Add(RolesEnum.ADMIN);
                list.Add(RolesEnum.Leader);
                list.Add(RolesEnum.PM);
                list.Add(RolesEnum.DEV);
                list.Add(RolesEnum.QA);
                if (list.Contains(UserInfo.Role))
                {
                    return true;
                }
                return false;
            }
        }

        private string userNameFormat = "FN LN";

        /// <summary>
        /// 页面绑定用户名的属性名
        /// </summary>
        public string UserNameDisplayProp
        {
            get { return "FirstAndLastName"; }
        }

        private string GetUserName(UsersEntity user, string format)
        {
            return user.GetClientUserName(UserInfo, format);
        }
        /// <summary>
        /// Hide client name to SH use default format(First Name)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string GetClientUserName(object client)
        {
            int clientid = 0;
            if (int.TryParse(client.ToString(), out clientid) && clientid > 0)
            {
                return GetClientUserName(clientid, userNameFormat);
            }
            return "";
        }
        /// <summary>
        /// Hide client name to SH
        /// </summary>
        /// <param name="client"></param>
        /// <param name="format">LN=Last Name,FN=First Name</param>
        /// <returns></returns>
        public string GetClientUserName(object client, string format)
        {
            int clientid = 0;
            if (int.TryParse(client.ToString(), out clientid) && clientid > 0)
            {
                return GetClientUserName(clientid, format);
            }
            return "";
        }
        /// <summary>
        /// Hide client name to SH use default format(First Name)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string GetClientUserName(int client)
        {
            if (client < 1)
            {
                return "";
            }
            return GetClientUserName(client, userNameFormat);
        }
        /// <summary>
        /// Hide client name to SH
        /// </summary>
        /// <param name="client"></param>
        /// <param name="format">LN=Last Name,FN=First Name</param>
        /// <returns></returns>
        public string GetClientUserName(int client, string format)
        {
            if (client < 1)
            {
                return "";
            }
            UsersEntity user = _userApp.GetUser(client);
            return GetClientUserName(user, format);
        }
        /// <summary>
        /// Hide client name to SH use default format(Last Name,First Name)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string GetClientUserName(UsersEntity client)
        {
            return GetClientUserName(client, userNameFormat);
        }
        /// <summary>
        /// Hide client name to SH
        /// </summary>
        /// <param name="client">User to format</param>
        /// <param name="format">LN,FN | FN LN</param>
        /// <returns></returns>
        public string GetClientUserName(UsersEntity client, string format)
        {
            if (client == null)
            {
                return "";
            }
            if (client.Role == RolesEnum.CLIENT && UserInfo.Office.ToUpper().Equals("CN"))
            {
                return "Client";
            }
            return GetUserName(client, format);
        }

        public string GetSunnetUsername(int user)
        {
            UsersEntity userEntity = _userApp.GetUser(user);
            return userEntity.FirstAndLastName;
        }

        public DateTime MinDate
        {
            get { return DateTime.Parse("1753-1-1"); }
        }

        public List<ListItem> ConvertStateListToItemList(List<TicketsState> ticketsStates)
        {
            List<ListItem> listItems = new List<ListItem>();
            foreach (TicketsState item in ticketsStates)
            {
                listItems.Add(new ListItem() { Text = item.ToText(), Value = ((int)item).ToString() });
            }
            return listItems;
        }

        public List<ListItem> ConvertEnumtToListItem(Type type)
        {
            List<ListItem> result = new List<ListItem>();

            string[] names = Enum.GetNames(type);
            Array values = Enum.GetValues(type);
            for (int i = 0; i < names.Length; i++)
            {
                result.Add(new ListItem() { Text = names[i].Replace('_', ' '), Value = values.GetValue(i).ToString() });
            }
            result.Sort(new RoleEnumComparer());
            return result;
        }

        /// <summary>
        /// Adapter from RoleEnum to TicketUserType
        /// </summary>
        /// <param name="role">current user role</param>
        /// <returns></returns>
        protected TicketUsersType GetUserTypeByRoleID(string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                int roleID = Convert.ToInt32(role);
                if ((int)RolesEnum.QA == roleID)
                {
                    return TicketUsersType.QA;
                }
                else if ((int)RolesEnum.DEV == roleID || (int)RolesEnum.Leader == roleID)
                {
                    return TicketUsersType.Dev;
                }
                else if ((int)RolesEnum.PM == roleID)
                {
                    return TicketUsersType.PM;
                }
                else
                {
                    return TicketUsersType.Other;
                }
            }
            return TicketUsersType.Other;
        }

        public string GetEnumName(Type type, object obj)
        {
            int i = 0;
            string name = "";
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return "";
            }
            else
            {
                int.TryParse(obj.ToString(), out i);
                name = Enum.GetName(type, i);
                return name;
            }
        }

        protected List<string> restrictedExtention = new List<string>() { "php", "php3", "php5", "phtml", "asp", "aspx", "ascx", "jsp", "cfm", "cfc", "pl", "bat", "exe", "dll", "reg", "cgi" };

        protected bool IsValidFile(string fileName)
        {
            string sExtension = Path.GetExtension(fileName);
            if (restrictedExtention.Contains(sExtension.Replace(".", "")))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}