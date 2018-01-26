using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.TicketModel;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class EditPriority : BaseWebsitePage
    {
        TicketsApplication ticketApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            ticketApp = new TicketsApplication();
            if (!IsPostBack)
            {
                InitProjects();
                InitControls();
            }
        }
        private void InitProjects()
        {
            ProjectApplication projApp = new ProjectApplication();
            projApp.GetUserProjects(UserInfo).BindDropdown(ddlProjects, "Title", "ID", "Please select", "0");
        }
        private void InitControls()
        {

            if (ddlProjects.SelectedValue == "0")
            {
                trNoTickets.Visible = true;
            }
            else
            {
                int projectID = 0;
                if (int.TryParse(ddlProjects.SelectedValue, out projectID) && projectID > 0)
                {
                    SearchTicketsRequest request = new SearchTicketsRequest(SearchTicketsType.Priority,
                                                                    " OrderNum ASC ,Priority DESC,TicketTitle ASC ",
                                                                    false);
                    request.SheetDate = ObjectFactory.GetInstance<ISystemDateTime>().Now.Date;
                    request.ProjectID = projectID;
                    SearchTicketsResponse response = ticketApp.SearchTickets(request);
                   
                    if (response.ResultList == null || response.ResultList.Count <= 0)
                    {
                        rptTickets.Visible = false;
                        trNoTickets.Visible = true;
                    }
                    else
                    {
                        rptTickets.Visible = true;
                        rptTickets.DataSource = response.ResultList;
                        rptTickets.DataBind();
                        trNoTickets.Visible = false;
                    }
                }
                else
                {
                    rptTickets.Visible = false;
                    trNoTickets.Visible = true;
                }
            }
        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            
            InitControls();
        }
        private bool RemoveTicketsOrder(out int projectID)
        {
            int id = 0;
            projectID = 0;
            if (int.TryParse(ddlProjects.SelectedValue, out id) && id != 0)
            {
                projectID = id;
                return ticketApp.RemoveAllTicketsOrderByProject(id);
            }
            return false;
        }
        private void RecordMsg(List<BrokenRuleMessage> listmsgs, List<BrokenRuleMessage> listBrokenMsgs)
        {
            foreach (BrokenRuleMessage msg in listBrokenMsgs)
            {
                listmsgs.Add(msg);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string[] tickets = hidNewOrder.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int projectID = 0;

            List<BrokenRuleMessage> listMsgs = new List<BrokenRuleMessage>();

            if (tickets.Length > 0)
            {
                if (RemoveTicketsOrder(out projectID))
                {
                    for (int i = 0; i < tickets.Length; i++)
                    {
                        int id = 0;
                        if (int.TryParse(tickets[i], out id) && id != 0)
                        {
                            TicketsOrderEntity model = TicketsOrderFactory.CreateTicketsOrderEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                            model.ProjectID = projectID;
                            model.TicketID = id;
                            model.OrderNum = i + 1;

                            ticketApp.InsertTicketsOrder(model);
                            RecordMsg(listMsgs, ticketApp.BrokenRuleMessages);
                        }
                    }
                    if (listMsgs.Count > 0)
                    {
                        ShowFailMessageToClient(listMsgs);
                    }
                    else
                    {
                        InitControls();
                        ShowSuccessMessageToClient(false, false);
                    }
                }
                else
                {
                    ShowFailMessageToClient();
                }
            }
        }
    }
}
