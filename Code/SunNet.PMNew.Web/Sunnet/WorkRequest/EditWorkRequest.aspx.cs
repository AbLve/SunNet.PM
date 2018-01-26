using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.WorkRequestModel;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class EditWorkRequest : BaseWebsitePage
    {
        WorkRequestApplication wrApp = new WorkRequestApplication();
        FileApplication fileApp = new FileApplication();
        UserApplication userApp = new UserApplication();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                
                int wid = QS("id", 0);
                if (wid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                else
                {

                    AddWorkRequest1.WorkRequestID = wid.ToString();
                    RelationTicketsList1.WorkRequestID = wid.ToString();
                    WorkRequestEntity entity = wrApp.Get(wid);

                    if (entity != null)
                    {
                        AddWorkRequest1.wrEntity = entity;
                        RelationTicketsList1.ProjectID = entity.ProjectID.ToString();
                        AddWorkRequest1.IsAdd = false;
                    }
                    else
                    {
                        this.ShowArgumentErrorMessageToClient();
                        return;
                    }
                    RelationTicketsList1.ProjectID = entity.ProjectID.ToString();
                    BindDocuments(wid);
                    BindNotes(wid);
                }
            }
        }

        private void BindDocuments(int wid)
        {
            rptDocuments.DataSource = fileApp.GetFileListBySourceId(wid, SunNet.PMNew.Entity.FileModel.FileSourceType.WorkRequest);
            rptDocuments.DataBind();
        }

        private void BindNotes(int wid)
        {
            rptNotes.DataSource = wrApp.GetWorkRequestNotes(wid);
            rptNotes.DataBind();
        }


        protected void rptNote_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Literal lid = e.Item.FindControl("ltlCreatedByID") as Literal;
                Literal lname = e.Item.FindControl("ltlCreatedByName") as Literal;
                lname.Text = userApp.GetUser(Convert.ToInt32(lid.Text)).FirstName;

            }
        }
    }
}
