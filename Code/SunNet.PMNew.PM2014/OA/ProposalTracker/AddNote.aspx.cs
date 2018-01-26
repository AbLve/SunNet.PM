using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class AddNote : BasePage
    {
        ProposalTrackerApplication app = new ProposalTrackerApplication();

        protected void Page_Load(object sender, EventArgs e)
        {          
            ((Pop)this.Master).Width = 540;
            if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            if (QS("ID", 0) == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int proposalTrackerId = QS("ID", 0);
            ProposalTrackerNoteEntity model = new ProposalTrackerNoteEntity();
            model.ProposalTrackerID = proposalTrackerId;
            model.Title = txtTitle.Text.Trim();
            model.Description = txtDescription.Text.Trim();
            model.ModifyOn = DateTime.Now;
            model.ModifyBy = UserInfo.UserID;
            if (app.AddNote(model) > 0)
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