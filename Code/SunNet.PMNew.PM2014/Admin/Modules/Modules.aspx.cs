using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin.Modules
{
    public partial class Modules : BasePage
    {
        UserApplication userApp = new UserApplication();
        int recordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int parentID = QS("parent", 0);
                BindControl();

                if (QS("selected", 1) == 1)
                {
                    ltlMenu.Text = "&gt;&nbsp;&nbsp;<strong>Root</strong>";
                }
                else
                {
                    ModulesEntity module = userApp.GetModule(QS("selected", 1));
                    ltlMenu.Text = GetPath(parentID) + string.Format("&nbsp;&nbsp;>&nbsp;&nbsp;<strong>{0}</strong>", module.ModuleTitle);
                }
            }
        }


        private string template = "<a href='Modules.aspx?parent={parent}&selected={selected}'>{Text}</a>";
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
                return GetPath(module.ParentID) + "&nbsp;&nbsp;&gt;&nbsp;&nbsp;" + parentHtml;
            }
            return parentHtml;
        }

        protected void BindControl()
        {
            List<ModulesEntity> list = userApp.GetAllModules(QS("selected", 1), 1, 100, out recordCount);
            ltlModules.Text = string.Join(",", list.Select(x => x.ID));
            rptModules.DataSource = list;
            rptModules.DataBind();
        }
    }
}