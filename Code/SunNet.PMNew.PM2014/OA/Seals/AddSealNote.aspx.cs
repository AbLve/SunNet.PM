using System;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.OA.Seals
{
    public partial class AddSealNote : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ((Pop)this.Master).Width = 540;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (QS("ID", 0) > 0)
            {
                SealNotesEntity entity = new SealNotesEntity();
                entity.Title = txtTitle.Text;
                entity.Description = txtDescription.Text.Replace("\r\n", "<br>");
                entity.SealRequestsID = QS("ID", 0);
                entity.CreateOn = DateTime.Now;
                entity.UserID = UserInfo.UserID;
                if (new App.SealsApplication().InsertSealNotes(entity) > 0)
                    Redirect(EmptyPopPageUrl, false, true);
                else
                    Redirect("error.", "AddSealNote.aspx?ID" + QS("ID"));
            }
            else
                ShowFailMessageToClient("unauthorized access.");
        }
    }
}