using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Web.Sunnet.Projects
{
    public partial class AddPrincipal : BaseWebsitePage
    {
        ProjectApplication app;
        protected void Page_Load(object sender, EventArgs e)
        {
            app = new ProjectApplication();
            if (QS("id", 0) == 0)
            {
                return;
            }
            else
            {
                hdprojectId.Value = QS("id");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ProjectPrincipalEntity entity = new ProjectPrincipalEntity();
            entity.Module = this.txtModule.Value;
            entity.PM = this.txtPM.Value;
            entity.DEV = this.txtDEV.Value;
            entity.QA = this.txtQA.Value;
            entity.ProjectID = (int)QS("id", 0);
            if (app.AddPrincipal(entity) > 0)
            {
                ShowSuccessMessageToClient();
            }
            else
            {
                ShowFailMessageToClient(app.BrokenRuleMessages);
            }
        }
    }
}