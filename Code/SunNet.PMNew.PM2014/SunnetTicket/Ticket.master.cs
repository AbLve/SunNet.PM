using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.SunnetTicket
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

        /// <summary>
        /// Gets or sets the current module.
        /// </summary>
        /// <value>
        /// The current module.
        /// </value>
        /// Author  :  Jack Zhang (JACKZ)
        /// Date    :  4/28 11:51
        public SelectedSunnetMenu CurrentModule
        {
            set { this.SunnetMenu1.CurrentModule = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}