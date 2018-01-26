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
    public partial class EditRole : BasePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            if (!IsPostBack)
            {
                RolesEntity entity = userApp.GetRole(QS("id", 0));
                txtRoleName.Text = entity.RoleName;
                txtDesc.Text = entity.Description;
                ddlStatus.SelectedValue = entity.Status.ToString();
            }
        }

        private RolesEntity GetEntity()
        {
            RolesEntity model = userApp.GetRole(QS("id", 0));
            model.RoleName = txtRoleName.Text.NoHTML();
            model.Description = txtDesc.Text.NoHTML();
            model.Status = int.Parse(ddlStatus.SelectedValue);

            return model;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            RolesEntity model = GetEntity();

            if (userApp.UpdateRole(model))
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