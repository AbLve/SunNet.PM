using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using NPOI.HSSF.UserModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class TicketReport : BaseWebsitePage
    {
        TicketsApplication tickApp;
        ProjectApplication projApp;
        protected FeedBackMessageHandler fbmHandler;
        protected void Page_Load(object sender, EventArgs e)
        {
            projApp = new ProjectApplication();
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            tickApp = new TicketsApplication();
            if (!IsPostBack)
            {
                InitSearchControls();


                int projectID = QS("pid", 0);
                if (projectID != 0)
                {
                    ddlProject.SelectedValue = projectID.ToString();
                    InitControl(projectID.ToString());
                }
                else
                {
                    InitControl(string.Empty);
                }
            }
        }
        private void InitSearchControls()
        {
            projApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ID", "All", "0");

            var statuses = new Dictionary<int, string>();
            foreach (int value in Enum.GetValues(typeof(ClientTicketState)))
            {
                if (value != (int)ClientTicketState.None)
                    statuses.Add(value, ((ClientTicketState)value).ToString().Replace("_", " "));
            }

            ddlStatus.DataSource = statuses;
            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("All", "-1"));
            ddlStatus.Items[0].Selected = true;
        }
        public string GetClientStatusNameBySatisfyStatus(object status, object ticketID)
        {
            return fbmHandler.GetClientStatusNameBySatisfyStatus(int.Parse(status.ToString()), int.Parse(ticketID.ToString()));
        }

        private SearchTicketsResponse GetResponse(bool isPageModel, string projectID)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(
                   SearchTicketsType.TicketsForReport,
                   string.Format(" {0} {1} ", hidOrderBy.Value, hidOrderDirection.Value),
                   isPageModel);
            request.CurrentPage = anpTicketReport.CurrentPageIndex;
            request.PageCount = anpTicketReport.PageSize;
            request.Keyword = txtKeywords.Text.Trim().NoHTML();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                request.CompanyID = UserInfo.CompanyID;
                request.UserID = UserInfo.UserID;
            }
            else
            {
                request.CompanyID = 0;
            }
            if (int.Parse(ddlStatus.SelectedValue) == (int)ClientTicketState.Waiting_Feedback)
            {
                request.SearchTicketID = true;
                request.TicketIDS = fbmHandler.FeedBackRequiredTicketIDs;
            }
            request.TicketType = ddlTicketType.SelectedItem.Text.Trim();
            request.ProjectID = int.Parse(projectID);
            request.Status = fbmHandler.GetSearchTicketStatuses(int.Parse(ddlStatus.SelectedValue));
            SearchTicketsResponse response = tickApp.SearchTickets(request);
            return response;
        }
        private void InitControl(string projectID)
        {
            SearchTicketsResponse response;
            if (!string.IsNullOrEmpty(projectID))
            {
                response = GetResponse(true, projectID);
            }
            else
            {

                response = GetResponse(true, ddlProject.SelectedValue);
            }
            if (response.IsError
                || response.ResultList == null
                || response.ResultList.Count == 0)
            {
                trNoTickets.Visible = true;
                rptTicketsReport.DataSource = null;
                rptTicketsReport.DataBind();
            }
            else
            {
                trNoTickets.Visible = false;
                rptTicketsReport.DataSource = response.ResultList;
                rptTicketsReport.DataBind();
                anpTicketReport.RecordCount = response.ResultCount;
            }
        }
        protected void anpTicketReport_PageChanged(object sender, EventArgs e)
        {
            InitControl(string.Empty);
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpTicketReport.CurrentPageIndex = 1;
            InitControl(string.Empty);
        }


        private List<int> GetProjectIDsFilter()
        {
            ProjectApplication projApp = new ProjectApplication();
            List<ProjectDetailDTO> listproj = projApp.GetUserProjects(UserInfo);
            List<int> list = new List<int>();
            foreach (ProjectsEntity item in listproj)
            {
                list.Add(item.ID);
            }
            return list;
        }

        protected void iBtnDownload_Click(object sender, ImageClickEventArgs e)
        {

            Response.ContentType = "application/vnd.ms-excel";
            string datetime = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
            string ticktype = ddlStatus.SelectedItem.Text;
            ticktype = ticktype == ddlStatus.Items[0].Text ? "ALL_Tickets_" : ticktype + "_Tickets_";
            string filename = HttpUtility.UrlDecode(ticktype + datetime + ".xls");
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //Set Sheet ColumnWidth
            sheet.SetColumnWidth(0, 30 * 256);
            sheet.SetColumnWidth(2, 50 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 20 * 256);
            sheet.SetColumnWidth(6, 20 * 256);
            sheet.SetColumnWidth(7, 20 * 256);
            sheet.SetColumnWidth(8, 100 * 256);

            sheet.DisplayGridlines = false;

            HSSFRow row_head = (HSSFRow)sheet.CreateRow(0);
            row_head.HeightInPoints = 20;

            row_head.CreateCell((short)0).SetCellValue("Project");
            row_head.CreateCell((short)1).SetCellValue("Type");
            row_head.CreateCell((short)2).SetCellValue("Ticket Code / Ticket Title");
            row_head.CreateCell((short)3).SetCellValue("Created Date");
            row_head.CreateCell((short)4).SetCellValue("Updated Date");
            row_head.CreateCell((short)5).SetCellValue("Status");
            row_head.CreateCell((short)6).SetCellValue("Priority");
            row_head.CreateCell((short)7).SetCellValue("Created By");
            row_head.CreateCell((short)8).SetCellValue("Description");


            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();

            //hStyle.BorderTop = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
            hStyle.BorderRight = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
            //hStyle.BorderLeft = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
            hStyle.BorderBottom = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;

            //Set head row background color
            hStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.TEAL.index;
            hStyle.FillPattern = NPOI.SS.UserModel.FillPatternType.SQUARES;
            hStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.TEAL.index;

            HSSFFont font = (HSSFFont)workbook.CreateFont();
            font.Color = NPOI.HSSF.Util.HSSFColor.WHITE.index;
            font.FontName = "Verdana";
            font.FontHeightInPoints = 12;

            hStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            hStyle.SetFont(font);

            foreach (var c in row_head.Cells)
            {
                c.CellStyle = hStyle;
            }

            List<int> projectIdList = GetProjectIDsFilter();
            //List<TicketsState> statusList = GetStautsFilterList();
            string keyword = txtKeywords.Text.Trim().NoHTML();
            Guid createdByUserId = Guid.Empty;

            SearchTicketsResponse response = GetResponse(false, ddlProject.SelectedValue);
            int i = 1;
            foreach (ExpandTicketsEntity ticket in response.ResultList)
            {
                HSSFRow rownumber = (HSSFRow)sheet.CreateRow(i);
                rownumber.CreateCell((short)0).SetCellValue(ticket.ProjectTitle);
                rownumber.CreateCell((short)1).SetCellValue(ticket.TicketType.ToString());
                rownumber.CreateCell((short)2).SetCellValue("[" + ticket.TicketCode + "]" + ticket.Title);
                rownumber.CreateCell((short)3).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.CreatedOn));
                rownumber.CreateCell((short)4).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.ModifiedOn));
                rownumber.CreateCell((short)5).SetCellValue(ticket.Status.ToString());
                rownumber.CreateCell((short)6).SetCellValue(ticket.Priority.ToString());
                rownumber.CreateCell((short)7).SetCellValue(string.Format("{0} {1}", ticket.FirstName, ticket.LastName));
                rownumber.CreateCell((short)8).SetCellValue(ticket.FullDescription);


                HSSFCellStyle rStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                rStyle.BorderRight = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                rStyle.BorderBottom = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
                rStyle.WrapText = true;
                rStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.CENTER;
                rStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.LEFT;
                foreach (var c in rownumber.Cells)
                {
                    c.CellStyle = rStyle;
                }
                i++;
            }
            workbook.Write(Response.OutputStream);
        }

        protected string ShowEditTicket(object ticketId, object status)
        {
            return string.Format("opentype='newtab' href='/Sunnet/Clients/TicketsDetail.aspx?is0hsisdse=54156&tid={0}')", ticketId);
        }
    }
}
