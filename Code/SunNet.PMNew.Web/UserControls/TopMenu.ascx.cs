using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class TopMenu : BaseAscx
    {
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
        public int RoleID
        {
            get
            {
                //return (int)ViewState["RoleID"];
                return UserInfo.RoleID;
            }
            set
            {
                ViewState["RoleID"] = value;
            }
        }
        public int CurrentIndex
        {
            get
            {
                if (!(null == ViewState["CurrentIndex"]))
                    return (int)ViewState["CurrentIndex"];
                return 0;
            }
            set
            {
                ViewState["CurrentIndex"] = value;
            }
        }
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            if (!IsPostBack)
            {
                InitControl();
            }
        }
        private void InitControl()
        {
            List<ModulesEntity> list = userApp.GetRoleModules(RoleID, true);
            List<ModulesEntity> listTop = list.FindAll(m => m.ParentID == ParentID && m.ShowInMenu);
            if (UserInfo.Role == RolesEnum.CLIENT) // if current role is client then change Clients modules' module name to Tickets
            {
                ModulesEntity modulesEntity = listTop.Find(r => r.ModuleTitle.Trim().ToLower() == "clients");
                if (modulesEntity != null)
                {
                    modulesEntity.ModuleTitle = "Tickets";
                }
            }
            rptTop.DataSource = listTop;
            rptTop.DataBind();
        }
    }
}