using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Web.Codes;
namespace SunNet.PMNew.Web
{
    public partial class TicketList : BaseWebsitePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TicketsDataBind();
            }
        }

        private void TicketsDataBind()
        {
            List<TicketsEntity> list = null;
            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();
            TicketsApplication ticketAPP = new TicketsApplication();
            TicketsSearchConditionDTO DTO = new TicketsSearchConditionDTO();

            DTO.KeyWord = "";
            DTO.Status = Convert.ToString((int)TicketsState.Draft);
            DTO.TicketType = "";
            DTO.Project = "";
            DTO.AssignedUser = "";
            DTO.Company = "";
            DTO.Client = "";
            DTO.ClientPriority = "";
            // DTO.PriorityView = true;


            request.TicketSc = DTO;
            //list = ticketAPP.GetTicketListBySearchCondition(request);
            //this.rptTicketsList.DataSource = list;
            //this.rptTicketsList.DataBind();
        }
    }
}
