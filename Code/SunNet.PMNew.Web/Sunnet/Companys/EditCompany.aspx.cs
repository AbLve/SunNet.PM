using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using StructureMap;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.IO;
using System.Drawing;

namespace SunNet.PMNew.Web.Sunnet.Companys
{
    public partial class EditCompany : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (QS("id", 0) > 0)
            {
                this.CompanyEdit1.CompanyID = QS("id", 0);
            }
            else
            {
                Response.Redirect("/Sunnet/Companys/ListCompany.aspx");
            }
        }
    }
}
