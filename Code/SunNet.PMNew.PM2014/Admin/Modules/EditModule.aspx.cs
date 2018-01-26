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
using StructureMap;

namespace SunNet.PMNew.PM2014.Admin.Modules
{
    public partial class EditModule : BasePage
    {
        UserApplication userApp = new UserApplication();
        int recordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int parentID = 0;
                int selected = 0;
                if (string.IsNullOrEmpty(QS("selected"))
                    || string.IsNullOrEmpty(QS("parent"))
                    || QS("selected", 0) == 0
                    || QS("parent", 0) == 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                else
                {
                    parentID = QS("parent", 0);
                    selected = QS("selected", 0);

                    ModulesEntity parent = userApp.GetModule(parentID);
                    parentID = parent.ParentID;
                }

                ModulesEntity model = userApp.GetModule(selected);

                List<ModulesEntity> listparent = userApp.GetAllModules(parentID, 1, 1000, out recordCount);
                foreach (ModulesEntity m in listparent)
                {
                    ListItem item = new ListItem(string.Format("{0} [{1}]", m.ModuleTitle, m.ModulePath), m.ID.ToString());
                    ddlParentModule.Items.Add(item);
                }
                InitContorls(model);
                Page.Title = string.Format("Edit Module({0},{1})", model.ModuleTitle, model.ModulePath);
            }
        }
        protected void InitContorls(ModulesEntity model)
        {
            ddlParentModule.SelectedValue = model.ParentID.ToString();

            this.txtModulePath.Text = model.ModulePath;
            this.txtModuleTitle.Text = model.ModuleTitle;
            this.txtDefaultPage.Text = model.DefaultPage;
            this.txtClickFunction.Text = model.ClickFunctioin;
            this.txtOrders.Text = model.Orders.ToString();
            this.chkShow.Checked = model.ShowInMenu;
        }
        private ModulesEntity GetEntity()
        {
            ModulesEntity model = userApp.GetModule(QS("selected", 0));

            model.ModuleTitle = txtModuleTitle.Text.Trim().NoHTML();
            model.ModulePath = txtModulePath.Text.Trim();
            model.DefaultPage = txtDefaultPage.Text.Trim();
            model.ClickFunctioin = txtClickFunction.Text.Trim();
            model.ShowInMenu = chkShow.Checked;
            model.PageOrModule = txtModulePath.Text.IndexOf(".") > 0 ? 0 : 1;
            model.Orders = int.Parse(txtOrders.Text.Trim());
            model.ParentID = int.Parse(ddlParentModule.SelectedValue);

            return model;

        }
        private bool CheckInput()
        {
            int result;
            if (!int.TryParse(txtOrders.Text.Trim(), out result))
            {
                return false;
            }
            if (ddlParentModule.Items.Count <= 0 || !(int.TryParse(ddlParentModule.SelectedValue, out result)))
            {
                return false;
            }
            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("AddModule");
            if (!IsValid)
                return;
            if (!CheckInput())
            {
                this.ShowMessageToClient("Input Error,please select a parent module and you must input a number for Priority field",
                    2, false, false);
                return;
            }
            ModulesEntity model = GetEntity();
            if (userApp.UpdateModule(model, UserInfo.RoleID))
                Redirect(QS("returnurl"));
            else
                this.ShowFailMessageToClient(userApp.BrokenRuleMessages);

        }
    }
}
