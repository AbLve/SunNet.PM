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
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class EditRole : BaseWebsitePage
    {
        UserApplication userApp = new UserApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = QS("id", 0);
                if (id == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }

                RolesEntity model = userApp.GetRole(id);
                InitControls(model);

            }
        }
        private void InitControls(RolesEntity role)
        {
            txtRoleName.Text = role.RoleName;
            txtDesc.Text = role.Description;
            ddlStatus.SelectedValue = role.Status.ToString();
        }
        private RolesEntity GetEntity()
        {
            int id = QS("id", 0);
            RolesEntity model = userApp.GetRole(id);
            model.RoleName = txtRoleName.Text.NoHTML();
            model.Description = txtDesc.Text.NoHTML();
            model.Status = int.Parse(ddlStatus.SelectedValue);

            return model;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("Add");
            if (!IsValid)
                return;

            RolesEntity model = GetEntity();
            if (userApp.UpdateRole(model))
            {
                this.ShowSuccessMessageToClient(false, true);
            }
            else
            {
                ShowFailMessageToClient(userApp.BrokenRuleMessages);
            }
        }
    }
}
