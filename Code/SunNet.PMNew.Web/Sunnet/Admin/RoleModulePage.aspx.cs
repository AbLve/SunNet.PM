using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using StructureMap;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class RoleModulePage : BaseWebsitePage
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
                RolesEntity role = userApp.GetRole(id);
                ltlRole.Text = string.Format("{0}[{1}]", role.RoleName, role.Description);
                InitControl(id);
            }
        }
        #region Template
        private string templateParent = @"<li><input type='checkbox' id='M{ID}'><label for='M{ID}'>{Text}</label>
                                               {Children}
                                        </li>";
        private string templateChild = @"<ul>
                                            <li>
                                                <input type='checkbox' id='M{ID}'><label for='M{ID}'>{Text}</label>
                                            </li>
                                        </ul>";
        private string templateP2C = @"<li>
                                           <input type='checkbox' id='M{ID}'><label for='M{ID}'>{Text}</label>
                                       </li> ";
        #endregion
        // Create Modules tree
        private string GetTree(List<ModulesEntity> list, int parentID)
        {
            StringBuilder strTree = new StringBuilder();

            ModulesEntity parent = list.First<ModulesEntity>(m => m.ID == parentID);
            if (null == parent)
            {
                return string.Empty;
            }

            var children = list.FindAll(m => m.ParentID == parent.ID);
            StringBuilder strChildren = new StringBuilder();
            if (children.Count<ModulesEntity>() > 0)
            {
                List<ModulesEntity> listChildren = children.ToList<ModulesEntity>();
                foreach (ModulesEntity model in listChildren)
                {
                    strChildren.Append(GetTree(list, model.ID));
                }
            }
            else
            {
                string child = templateChild.Replace("{ID}", parent.ID.ToString())
                                        .Replace("{ID}", parent.ID.ToString())
                                        .Replace("{Text}", parent.ModuleTitle);
                return child;
            }
            string _this = string.Empty;
            if (parent.IsPage)
            {
                _this = templateP2C.Replace("{ID}", parent.ID.ToString())
                    .Replace("{ID}", parent.ID.ToString())
                    .Replace("{Text}", parent.ModuleTitle);
            }
            else
            {
                _this = templateParent.Replace("{ID}", parent.ID.ToString())
                    .Replace("{ID}", parent.ID.ToString())
                    .Replace("{Text}", parent.ModuleTitle)
                    .Replace("{Children}", strChildren.ToString());
                _this = string.Format("<ul>{0}</ul>", _this);
            }
            strTree.Append(_this);
            return strTree.ToString();
        }

        // Get existsed modules in role
        private string GetSelectedModules(List<ModulesEntity> list)
        {
            StringBuilder strSelected = new StringBuilder();
            foreach (ModulesEntity model in list)
            {
                strSelected.AppendFormat("M{0},", model.ID);
            }
            return strSelected.ToString();
        }

        private void InitControl(int role)
        {
            List<ModulesEntity> listAll = userApp.GetRoleModules(0);
            List<ModulesEntity> listCurrent = userApp.GetRoleModules(role);

            ltlModules.Text = GetTree(listAll, 1);
            ltlModules2.Text = GetTree(listAll, 1);
            hidSelected.Value = GetSelectedModules(listCurrent);
        }
        List<BrokenRuleMessage> Messages;

        private void AddRoleModule(int role, int module)
        {
            RoleModulesEntity model = UsersFactory.CreateRoleModulesEntity(0, ObjectFactory.GetInstance<ISystemDateTime>());
            model.RoleID = role;
            model.ModuleID = module;

            if (!userApp.AddRoleModule(model))
            {
                if (this.Messages == null)
                    Messages = new List<BrokenRuleMessage>();
                foreach (BrokenRuleMessage message in userApp.BrokenRuleMessages)
                {
                    this.Messages.Add(message);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] selectedModules = hidSelected.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int id = QS("id", 0);

            userApp.RemoveAllModules(id);

            foreach (string module in selectedModules)
            {
                int mid = UtilFactory.Helpers.CommonHelper.ToInt(module.Replace("M", ""));
                AddRoleModule(id, mid);
            }
            if (this.Messages == null || this.Messages.Count <= 0)
            {
                this.ShowSuccessMessageToClient(false,true);
            }
            else
            {
                this.ShowFailMessageToClient(this.Messages);
            }
        }
    }
}
