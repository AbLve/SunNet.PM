using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin.Roles
{
    public partial class Roles : BasePage
    {
        UserApplication userApp = new UserApplication();

        public List<string> RoleStatus
        {
            get
            {
                return new string[] { "Inactive", "Active" }.ToList<string>();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            List<RolesEntity> list = userApp.GetAllRoles();
            rptRoles.DataSource = list;
            rptRoles.DataBind();
        }
    }
}