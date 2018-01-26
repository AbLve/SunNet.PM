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
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class AddRole : BaseWebsitePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
        }

        private RolesEntity GetEntity()
        {
            RolesEntity model = UsersFactory.CreateRolesEntity(UserInfo.UserID, ObjectFactory.GetInstance<ISystemDateTime>());

            model.RoleName = txtRoleName.Text.NoHTML();
            model.Description = txtDesc.Text.NoHTML();
            model.Status = int.Parse(ddlStatus.SelectedValue);

            return model;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            RolesEntity model = GetEntity();

            int id = userApp.AddRole(model);
            if (id > 0)
            {
                this.ShowSuccessMessageToClient();
            }
            else
            {
                this.ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }
        }

    }
}
