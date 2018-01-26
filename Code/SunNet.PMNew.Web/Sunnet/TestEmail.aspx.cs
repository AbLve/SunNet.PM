using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet
{
    public partial class TestEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            EmailExecuter ex = new EmailExecuter();
            ex.SendDailyUSMails();
            ex.SendDailyCNMails();
        }
    }
}
