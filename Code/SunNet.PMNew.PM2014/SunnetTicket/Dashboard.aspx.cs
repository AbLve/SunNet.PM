using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Event;

namespace SunNet.PMNew.PM2014.Ticket.Sunnet
{
    public partial class Dashboard : TicketPageHelper
    {
        private static TicketsApplication ticketApp = new TicketsApplication();
        private static GlobalPage _globalPage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsInternalUser)
                {
                    Response.Redirect("/SunnetTicket/InternalDashboard.aspx");
                }
                var list = ticketApp.GetWorkingOnTickets(UserInfo.ID);
                if (list != null && list.Count > 0)
                {
                    var ticketlist= list.Distinct(new CompareTicket()).ToList();
                    rptTicketsList.DataSource = ticketlist;
                    rptTicketsList.DataBind();
                }
                else
                {
                    phlNoTicket.Visible = true;
                }
                _globalPage = this.Page as GlobalPage;
                List<UsersEntity> userList = new App.ProjectApplication().GetProjectUsersByUserId(UserInfo);
                userList = userList.Distinct(new CompareUser()).ToList();
                hiUserIds.Value = string.Join(",", userList.Select(r => r.UserID).ToArray());
            }
        }

        /// <summary>
        /// webservice 方法 进行获取相关的控件信息
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        [WebMethod]
        public static string RenderUserControl(int ticketId)
        {
            Page page = new Page();
            HtmlForm form=new HtmlForm();
            var ticketEntity = ticketApp.GetTickets(ticketId);
            UserControl ctl = (UserControl)page.LoadControl("~/UserControls/Ticket/FeedbackDashBoardList.ascx");
            Type feedfType = ctl.GetType();
            PropertyInfo isSunnet = feedfType.GetProperty("IsSunnet");
            FieldInfo ticketsEntityInfo = feedfType.GetField("TicketsEntityInfo");
            FieldInfo userInfo= feedfType.GetField("userInfo");
            isSunnet.SetValue(ctl, true, null);
            ticketsEntityInfo.SetValue(ctl, ticketEntity);
            userInfo.SetValue(ctl, _globalPage.UserInfo);
            page.Controls.Add(form);
            form.Controls.Add(ctl);
            ctl.EnableViewState = false;
            page.EnableEventValidation = false;
            page.DesignerInitialize();
            StringWriter writer = new StringWriter();
            HttpContext.Current.Server.Execute(page, writer, false);
            return writer.ToString();
        }

        class CompareTicket : IEqualityComparer<TicketsEntity>
        {
            public bool Equals(TicketsEntity x, TicketsEntity y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(TicketsEntity obj)
            {
                return obj.ID.GetHashCode();
            }
        }
    }
}