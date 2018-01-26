using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
/**************************************************************************
 * Developer: 		Jack Zhang
 * Computer:		JACKZ
 * Domain:			Jackz
 * CreatedOn:		4/16 10:50:13
 * Description:		所有用户控件基类
 * Version History:	Created,4/16 10:50:13
 * 
 * 
 **************************************************************************/
using System.Web.UI;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework;

namespace SunNet.PMNew.PM2014.Codes
{
    /// <summary>
    /// 所有用户控件基类
    /// </summary>
    /// Author  :  Jack Zhang (JACKZ)
    /// Date    :  4/16 10:50
    public class BaseAscx : UserControl
    {
        private UserApplication userApp = new UserApplication();
        public GlobalPage GlobalPage
        {
            get
            {
                var page = this.Page as GlobalPage;
                return page ?? null;
            }
        }

        public BasePage BasePage
        {
            get
            {
                var page = this.Page as BasePage;
                return page ?? new BasePage();
            }
        }



        public UsersEntity UserInfo
        {
            get
            {
                if (GlobalPage != null) return GlobalPage.UserInfo;
                return null;
            }
        }

        protected string GetEnumName<T>(T value)
        {
            return Enum.GetName(typeof(T), value).Replace('_', ' ');
        }

        /// <summary>
        /// 获取当前页面是否激活样式.
        /// </summary>
        /// <param name="page">页面名称（从根目录开始）.</param>
        /// <param name="activeClass">激活的样式.</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 10:04
        protected string GetCurrentPageActiveClass(string page, string activeClass = "active")
        {
            return Request.Url.AbsolutePath.Equals(page, StringComparison.CurrentCultureIgnoreCase) ? activeClass : "";
        }
        /// <summary>
        /// 当前页面是否激活样式.
        /// </summary>
        /// <param name="pages">页面或者文件夹，页面必须是完整路径，文件夹必须从根路径开始并且以 / 开始和结尾，
        /// 最后一个参数可以填写激活样式,不写则默认为active：
        /// 例如：GetCurrentItemActiveClass("/Timesheet/","/Weekplan/","active")
        /// </param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/17 10:04
        protected virtual string GetCurrentItemActiveClass(params string[] pages)
        {
            if (pages == null)
                return "";
            string activeClass = "active";
            if (pages.Length > 0 && pages.Last().IndexOf("/") < 0 && pages.Last().IndexOf(".") < 0)
            {
                activeClass = pages.Last();
            }
            var all = pages.ToList().FindAll(x => x.IndexOf("/") >= 0 || x.IndexOf(".") >= 0).Select(x => x.ToLower()).ToList();
            if (all == null || all.Count == 0)
                return "";
            return all.Find(x => Request.Url.AbsolutePath.ToLower().StartsWith(x)) == null ? "" : activeClass;
        }

        protected TicketUsersType MapRoleToTicketUserType(RolesEnum role)
        {
            switch (role)
            {
                case RolesEnum.PM:
                    return TicketUsersType.PM;
                case RolesEnum.DEV:
                case RolesEnum.Leader:
                    return TicketUsersType.Dev;
                case RolesEnum.QA:
                    return TicketUsersType.QA;
                default: return TicketUsersType.Other;
            }
        }

        public virtual int ModuleID
        {
            get
            {
                throw new NotImplementedException("Please set ModuleID for left menu.");
            }
        }

        private List<ModulesEntity> _leftMenus;
        public List<ModulesEntity> LeftMenus
        {
            get
            {
                if (_leftMenus == null)
                {
                    _leftMenus = userApp.GetRoleModules(UserInfo.RoleID, true);
                    _leftMenus = _leftMenus.FindAll(x => x.ParentID == ModuleID && x.ShowInMenu);
                    var timesheetReportMenu = _leftMenus.Find(r => r.ID == 55);
                    var ticketComparisonReportMenu = _leftMenus.Find(r => r.ID == 51);
                    if (timesheetReportMenu != null)
                    {
                        if (Config.TimesheetReport.Contains(UserInfo.UserID) == false)
                        {
                            _leftMenus.Remove(timesheetReportMenu);
                            _leftMenus.Remove(ticketComparisonReportMenu);
                        }
                    }
                }
                return _leftMenus;
            }
        }
    }
}