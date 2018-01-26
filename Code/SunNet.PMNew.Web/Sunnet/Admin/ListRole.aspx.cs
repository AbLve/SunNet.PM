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

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class ListRole : BaseWebsitePage
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
