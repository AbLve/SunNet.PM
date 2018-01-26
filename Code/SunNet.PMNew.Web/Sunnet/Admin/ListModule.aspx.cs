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
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class ListModule : BaseWebsitePage
    {
        UserApplication userApp = new UserApplication();
        int recordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int parentID = QS("parent", 0);
                int selected = QS("selected", 0);

                List<ModulesEntity> list = userApp.GetAllModules(parentID, 1, 1000, out recordCount);

                ddlParentModule.DataTextField = "ModuleTitle";
                ddlParentModule.DataValueField = "ModuleID";
                ddlParentModule.DataSource = list;
                ddlParentModule.DataBind();

                if (selected > 0)
                {
                    ddlParentModule.SelectedValue = selected.ToString();
                }

                BindControl();

                ltlMenu.Text = GetPath(parentID);
            }
        }
        private string template = "<a href='ListModule.aspx?parent={parent}&selected={selected}'>{Text}</a>";
        private string GetPath(int currentParent)
        {
            string parentHtml = string.Empty;
            ModulesEntity module = userApp.GetModule(currentParent);
            if (module == null)
                return "";
            parentHtml = template.Replace("{parent}", module.ParentID.ToString())
                                .Replace("{Text}", module.ModuleTitle)
                                .Replace("{selected}", module.ID.ToString());
            if (module.ParentID != 0)
            {
                return GetPath(module.ParentID) + "&nbsp;&nbsp;>&nbsp;&nbsp;" + parentHtml;
            }
            return parentHtml;
        }
        protected void BindControl()
        {
            if (ddlParentModule.Items.Count > 0)
            {
                int page = 1;
                page = anpModules.CurrentPageIndex;
                int parentID = int.Parse(ddlParentModule.SelectedValue);
                List<ModulesEntity> list = userApp.GetAllModules(parentID, page, anpModules.PageSize, out recordCount);

                rptModules.DataSource = list;
                rptModules.DataBind();

                anpModules.RecordCount = recordCount;
                ltlTotal.Text = recordCount.ToString();
            }
        }

        protected void ibtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpModules.CurrentPageIndex = 1;
            BindControl();
        }
    }
}
