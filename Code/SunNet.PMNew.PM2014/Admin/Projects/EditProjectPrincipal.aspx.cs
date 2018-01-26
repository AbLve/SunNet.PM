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
    public partial class EditProjectPrincipal : BasePage
    {
        ProjectApplication app;
        protected void Page_Load(object sender, EventArgs e)
        {
            app = new ProjectApplication();
            ((Pop)this.Master).Width = 540;
            if (!IsPostBack)
            {
                int id = QS("ID", 0);

                if (id == 0)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                ProjectPrincipalEntity principalEntity = app.GetProjectPrincipalInfo(id);
                if (principalEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }
                else
                {
                    this.txtModule.Value = principalEntity.Module;
                    this.txtPM.Value = principalEntity.PM;
                    this.txtDEV.Value = principalEntity.DEV;
                    this.txtQA.Value = principalEntity.QA;
                }
            }
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
                ProjectPrincipalEntity principalEntity = app.GetProjectPrincipalInfo(id);
                if (principalEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }

                principalEntity.Module = this.txtModule.Value;
                principalEntity.PM = this.txtPM.Value;
                principalEntity.DEV = this.txtDEV.Value;
                principalEntity.QA = this.txtQA.Value;
                if (app.UpdateProjectPrincipal(principalEntity))
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