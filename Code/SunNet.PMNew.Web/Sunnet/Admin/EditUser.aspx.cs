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
    public partial class EditUser : BaseWebsitePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                userApp = new UserApplication();
                //UserEdit1.BaseWebsitePage = this;
                int id = QS("id", 0);
                if (id == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                UsersEntity model = userApp.GetUser(id);
                UserEdit1.IsAdd = false;
                UserEdit1.IsSunnet = !(model.Role == RolesEnum.CLIENT);
                UserEdit1.UserToEdit = model;
            }
            catch
            {
                this.ShowArgumentErrorMessageToClient();
            }
        }
    }
}
