using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin.Projects
{
    public partial class AddProjectPrincipal : BasePage
    {
        ProjectApplication app;
        protected void Page_Load(object sender, EventArgs e)
        {
            app = new ProjectApplication();
            ((Pop)this.Master).Width = 540;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = QS("ID", 0);

            if (id == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            else
            {
                ProjectsEntity projectEntity = new ProjectApplication().Get(id);
                if (projectEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }

                ProjectPrincipalEntity entity = new ProjectPrincipalEntity();
                entity.Module = this.txtModule.Value;
                entity.PM = this.txtPM.Value;
                entity.DEV = this.txtDEV.Value;
                entity.QA = this.txtQA.Value;
                entity.ProjectID = projectEntity.ProjectID;
                if (app.AddPrincipal(entity) > 0)
                {
                    Redirect(EmptyPopPageUrl, false, true);
                }
                else
                {
                    ShowFailMessageToClient(app.BrokenRuleMessages);
                }
            }
        }
    }
}