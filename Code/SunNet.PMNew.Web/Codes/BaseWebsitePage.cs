using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;

using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Core.UI;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.SchedulesModel;

namespace SunNet.PMNew.Web.Codes
{
    public class BaseWebsitePage : GlobalPageBase
    {
        UserApplication userApp = new UserApplication();
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
                Response.Write(@"<script type='text/javascript'>if(self.dialogArguments){window.returnValue=-4; window.opener = self;window.close();window.opener.location.replace('/Login.aspx');}else{window.location.href='/Login.aspx';}</script>");

                Response.End();
                return;
            }
            bool canVisit = userApp.RoleCanAccessPage(this.UserInfo.RoleID, Request.Url.AbsolutePath.ToLower());
            if ((this.UserInfo.Role == RolesEnum.Contactor || this.UserInfo.Role == RolesEnum.Supervisor) &&
                Request.Url.AbsolutePath.ToLower() == "/sunnet/profile/profile.aspx")
                Response.Redirect("ChangePassword.aspx");
            if (!canVisit)
            {
                Response.Redirect("/NoAccess.html?sourceUrl=" + Request.Url.ToString());
            }
        }
        protected bool CheckRoleCanAccessPage()
        {
            int roleID = UserInfo.RoleID;
            string page = Request.Url.AbsolutePath.ToLower();
            return userApp.RoleCanAccessPage(roleID, page);
        }
        protected bool CheckRoleCanAccessPage(string page)
        {
            int roleID = UserInfo.RoleID;
            return userApp.RoleCanAccessPage(roleID, page);
        }
        public bool ISReadOnlyRole
        {
            get
            {
                List<RolesEnum> list = new List<RolesEnum>();
                list.Add(RolesEnum.DEV);
                list.Add(RolesEnum.Leader);
                list.Add(RolesEnum.QA);
                list.Add(RolesEnum.PM);
                list.Add(RolesEnum.Contactor);
                if (list.Contains(UserInfo.Role))
                {
                    return true;
                }
                return false;
            }
        }
        public bool ISCreateDirectoryRoles
        {
            get
            {
                List<RolesEnum> list = new List<RolesEnum>();
                list.Add(RolesEnum.ADMIN);
                list.Add(RolesEnum.PM);
                if (list.Contains(UserInfo.Role))
                {
                    return true;
                }
                return false;
            }
        }

        public UsersEntity UserInfo
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(userID))
                {
                    return null;
                }
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
                int id = int.Parse(userID);
                UsersEntity model = userApp.GetUser(id);
                ResumeCookie();
                return model;
            }
        }


        private void ResumeCookie()
        {
            IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);

            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("FirstName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LastName"), 30);
            UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("CompanyID"), 30);
        }

        private string GetUserName(string firstname, string lastname, string format)
        {
            return string.Format("<span title='{1}'>{0}</span>", format.ToUpper().Replace("FN", firstname).Replace("LN", lastname), string.Format("{0},{1}", lastname, firstname));
        }
        private string GetUserName(UsersEntity user, string format)
        {
            return GetUserName(user.FirstName, user.LastName, format);
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
                return GetClientUserName(clientid, "FN");
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
            return GetClientUserName(client, "FN");
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
            UserApplication userApp = new UserApplication();
            UsersEntity user = userApp.GetUser(client);
            return GetClientUserName(user, format);
        }
        /// <summary>
        /// Hide client name to SH use default format(Last Name,First Name)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public string GetClientUserName(UsersEntity client)
        {
            return GetClientUserName(client, "LN,FN");
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

        public DateTime MinDate
        {
            get { return DateTime.Parse("1753-1-1"); }
        }

        protected string SerializeScheduleList(List<SchedulesEntity> scheduleEntities)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            string planDay = string.Empty;
            foreach (SchedulesEntity schedulesEntity in scheduleEntities)
            {
                planDay = schedulesEntity.PlanDate.ToString("MM/dd/yyyy");
                stringBuilder.Append("{");
                stringBuilder.AppendFormat("id:\"{0}\",name:\"{1}\",startDate:\"{2}\",endDate:\"{3}\""
                    , schedulesEntity.ID, schedulesEntity.Title, planDay + " " + schedulesEntity.StartTime
                    , planDay + " " + schedulesEntity.EndTime);
                stringBuilder.Append("},");
            }
            return stringBuilder.ToString().TrimEnd(',') + "]";
        }
    }
}
