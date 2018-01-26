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

namespace SunNet.PMNew.PM2014.Admin.Modules
{
    public partial class AddModule : BasePage
    {
        UserApplication userApp = new UserApplication();
        int recordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int parentID = QS("parent", -1);
                int selected = QS("selected", -1);
                if (selected < 0)
                {
                    this.ShowFailMessageToClient();
                    return;
                }
                if (parentID >= 0)
                {
                    List<ModulesEntity> list = userApp.GetAllModules(parentID, 1, 1000, out recordCount);
                    foreach (ModulesEntity model in list)
                    {
                        ListItem item = new ListItem(string.Format("{0} [{1}]", model.ModuleTitle, model.ModulePath), model.ID.ToString());
                        ddlParentModule.Items.Add(item);
                        if (model.ID == selected)
                        {
                            InitControls(model);
                        }
                    }

                    ddlParentModule.SelectedValue = selected.ToString();
                }
                else
                {
                    int id = UtilFactory.Helpers.CommonHelper.ToInt(Request.QueryString["selected"]);
                    ModulesEntity model = userApp.GetModule(id);
                    if (!(model == null))
                    {
                        ListItem item = new ListItem(string.Format("{0} [{1}]", model.ModuleTitle, model.ModulePath), model.ID.ToString());
                        ddlParentModule.Items.Add(item);
                        ddlParentModule.Enabled = false;
                    }
                    InitControls(model);
                }

            }
        }
        private void InitControls(ModulesEntity parent)
        {
            txtModulePath.Text = parent.ModulePath;
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
        private ModulesEntity GetEntity()
        {
            ModulesEntity model = UsersFactory.CreateModulesEntity(0, ObjectFactory.GetInstance<ISystemDateTime>());

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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckInput())
            {
                this.ShowMessageToClient("Input Error,please select a parent module and you must input a number for Priority field", 2, false, false);
                return;
            }
            ModulesEntity model = GetEntity();
            int id = userApp.AddModule(model);

            if (id > 0)
                Redirect(QS("returnurl"));
            else
                this.ShowFailMessageToClient(userApp.BrokenRuleMessages);
        }
    }
}
