using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StructureMap;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class AddUser : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserEdit1.IsAdd = true;
            UserEdit1.IsSunnet = true;
        }

    }
}
