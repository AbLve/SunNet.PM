using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.Admin.Roles
{
    public partial class AddRole : BasePage
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
                Redirect(EmptyPopPageUrl, false, true);
            }
            else
            {
                this.ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }
        }

    }
}
