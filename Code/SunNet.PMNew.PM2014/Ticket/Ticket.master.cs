using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Ticket : System.Web.UI.MasterPage
    {
        public GlobalPage GlobalPage
        {
            get
            {
                var page = this.Page as GlobalPage;
                return page ?? null;
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}