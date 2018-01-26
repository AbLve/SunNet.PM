using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.UserControls
{


    public partial class SunnetMenu : BaseAscx
    {
        public int waitingForCount { get; set; }
        protected SelectedSunnetMenu GetSelectedMenu(string module)
        {
            if (!SelectedMenu.ContainsKey(module))
                return SelectedSunnetMenu.None;
            return SelectedMenu[module];
        }

        private Dictionary<string, SelectedSunnetMenu> SelectedMenu
        {
            get
            {
                var urls = new Dictionary<string, SelectedSunnetMenu>();
                urls.Add("Tickets", SelectedSunnetMenu.Ticket);
                urls.Add("Companies", SelectedSunnetMenu.Admin);
                urls.Add("Projects", SelectedSunnetMenu.Admin);
                urls.Add("Reports", SelectedSunnetMenu.Report);
                urls.Add("Clients", SelectedSunnetMenu.ClientTicket);
                urls.Add("Documents", SelectedSunnetMenu.Document);
                urls.Add("Forums", SelectedSunnetMenu.Forum);
                urls.Add("OA", SelectedSunnetMenu.OA);
                urls.Add("Work Request", SelectedSunnetMenu.OA);
                urls.Add("Default page", SelectedSunnetMenu.Ticket);
                urls.Add("Events", SelectedSunnetMenu.Event);
                urls.Add("Profile", SelectedSunnetMenu.Profile);
                urls.Add("Admin", SelectedSunnetMenu.Admin);
                urls.Add("Dashboard", SelectedSunnetMenu.ClientDashboard);
                urls.Add("Invoice", SelectedSunnetMenu.Invoice);
                return urls;
            }
        }

        public int ParentID
        {
            get
            {
                return (int)ViewState["ParentID"];
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the current module.
        /// </summary>
        /// <value>
        /// The current module.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/28 11:51
        public SelectedSunnetMenu CurrentModule { get; set; }

        /// <summary>
        /// 设置导航的Target属性.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 14:35
        public string Target { get; set; }

        /// <summary>
        /// 获取每个目录菜单是否激活样式.
        /// </summary>
        /// <param name="module">The module.</param>
        /// <param name="pages">页面或者文件夹，页面必须是完整路径，文件夹必须从根路径开始并且以 / 开始和结尾，
        /// 最后一个参数可以填写激活样式,不写则默认为active：
        /// 例如：GetCurrentItemActiveClass("/Timesheet/","/Weekplan/","active")</param>
        /// <returns></returns>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/22 14:26
        protected string GetMenuClass(SelectedSunnetMenu module, params string[] pages)
        {
            if (CurrentModule == SelectedSunnetMenu.None)
            {
                return GetCurrentItemActiveClass(pages);
            }
            else
            {
                return CurrentModule == module ? "active" : "";
            }
        }

        UserApplication userApp = new UserApplication();
        private void InitControl()
        {
            List<ModulesEntity> list = userApp.GetRoleModules(UserInfo.RoleID, true);
            List<ModulesEntity> listTop = list.FindAll(m => m.ParentID == ParentID && m.ShowInMenu);
            listTop = listTop.FindAll(x => x.ModuleTitle.IndexOf("Companies", StringComparison.CurrentCultureIgnoreCase) < 0
                                 && x.ModuleTitle.IndexOf("Projects", StringComparison.CurrentCultureIgnoreCase) < 0
                );
            rptTop.DataSource = listTop;
            rptTop.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserInfo != null)
                {
                    InitControl();
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (UserInfo != null)
                {
                    SetMenuCount();
                }
            }
        }

        private void SetMenuCount()
        {
            List<RequestStatus> status = new List<RequestStatus>();
            status.Add(RequestStatus.Draft);
            status.Add(RequestStatus.Denied);
            status.Add(RequestStatus.Submitted);
            status.Add(RequestStatus.PendingApproval);
            status.Add(RequestStatus.Approved);
            status.Add(RequestStatus.PendingProcess);
            status.Add(RequestStatus.Processed);

            SealsApplication app = new SealsApplication();
            waitingForCount = app.GetSealRequestsWaitingCount(UserInfo.UserID, status);
        }

    }
}