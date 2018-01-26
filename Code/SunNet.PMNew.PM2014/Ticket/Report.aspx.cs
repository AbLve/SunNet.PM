using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.PM2014.Ticket
{
    public partial class Report : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ShowNotification";
            }
        }
        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }

        ProjectApplication proApp = new ProjectApplication();
        TicketsApplication ticketAPP = new TicketsApplication();

        #region initial data bind
        private SearchTicketsResponse GetResponse(bool isPageModel)
        {
            SearchTicketsRequest request = new SearchTicketsRequest(
                   SearchTicketsType.TicketsForReport,
                   string.Format(" {0} {1} ", OrderBy, OrderDirection),
                   isPageModel);
            request.CurrentPage = CurrentPageIndex;
            request.PageCount = anpReport.PageSize;
            request.Keyword = txtKeyWord.Text.Trim().NoHTML();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                request.CompanyID = UserInfo.CompanyID;
                request.UserID = UserInfo.UserID;
            }
            else
            {
                request.CompanyID = 0;
            }
            
            request.TicketType = ddlTicketType.SelectedItem.Text.Trim();
            request.ProjectID = int.Parse(ddlProject.SelectedValue);
            request.Status = fbmHandler.GetSearchTicketStatuses(int.Parse(ddlStatus.SelectedValue));
            SearchTicketsResponse response = ticketAPP.SearchTickets(request);
            return response;
        }

        private void FillSearchDto()
        {
            proApp.GetUserProjects(UserInfo).BindDropdown<ProjectDetailDTO>(ddlProject, "Title", "ProjectID", this.DefaulAllText, "0",
                    QS("project"));
            GetTicketSatus().BindDropdown<ListItem>(ddlStatus, "Text", "Value", DefaulAllText, "-1", QS("status"));
            txtKeyWord.Text = QS("keyword");
            ddlTicketType.SelectItem(QS("tickettype"));
        }

        private void TicketsDataBind()
        {
            SearchTicketsResponse response = GetResponse(true);
            if (response.IsError
                || response.ResultList == null
                || response.ResultList.Count == 0)
            {
                trNoTickets.Visible = true;
                rptTicketsList.Visible = false;
            }
            else
            {
                trNoTickets.Visible = false;
                rptTicketsList.Visible = true;

                rptTicketsList.DataSource = response.ResultList;
                rptTicketsList.DataBind();
                anpReport.RecordCount = response.ResultCount;
            }
        }

        private List<ListItem> GetTicketSatus()
        {
            var list = new List<ListItem>();
            foreach (int value in Enum.GetValues(typeof(ClientTicketState)))
            {
                if (value != (int)ClientTicketState.None)
                {
                    list.Add(new ListItem(value.ToString().ToEnum<ClientTicketState>().ToText(), value.ToString()));
                }
            }
            return list;
        }

        protected string GetAction(int ID, TicketsState ticketsState)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendFormat("<a href=\"Detail.aspx?tid={0}&returnurl=" + ReturnUrl + "\" target='_blank' ticketId='{0}'><img src=\"/Images/icons/view.png\" title=\"View\" id='imageOpen{0}'></a>"
                ,ID);
            if (ticketsState == TicketsState.Ready_For_Review)
            {
                stringBuilder.Append("&nbsp;<a href=\"Approve.aspx?tid=" + ID.ToString()
                    + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/approve.png\" title=\"Approve\"></a>");
                stringBuilder.Append("&nbsp;<a href=\"Deny.aspx?tid=" + ID.ToString()
                    + "\" data-target=\"#modalsmall\" data-toggle=\"modal\"><img src=\"/Images/icons/deny.png\" title=\"Deny\"></a>");
            }
            return stringBuilder.ToString();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillSearchDto();
                TicketsDataBind();
            }
        }

        private void ExportExcel()
        {
            Response.ContentType = "application/vnd.ms-excel";
            string datetime = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
            string projectName = "ALL";
            projectName = ddlProject.SelectedValue == "0" ? projectName : ddlProject.SelectedItem.Text;

            string ticketType = "ALL";
            ticketType = ddlTicketType.SelectedValue == "-1" ? ticketType : ddlTicketType.SelectedItem.Text;

            string ticketStatu = "ALL";
            ticketStatu = ddlStatus.SelectedValue == "-1" ? ticketStatu : ddlStatus.SelectedItem.Text;

            string filename = string.Empty;
            if (projectName == "ALL" && ticketType == "ALL" && ticketStatu == "ALL")
                filename = string.Format("ALL_{0}.xls", datetime);
            else
            {
                projectName = projectName == "ALL" ? "AllProject" : projectName;
                ticketType = ticketType == "ALL" ? "ALLType" : ticketType;
                ticketStatu = ticketStatu == "ALL" ? "ALLStatus" : ticketStatu;
                filename = string.Format("{0}_{1}_{2}.xls", projectName, ticketStatu, ticketType);
            }

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

            SearchTicketsResponse response = GetResponse(false);
            int i = 1;
            foreach (ExpandTicketsEntity ticket in response.ResultList)
            {
                HSSFRow rownumber = (HSSFRow)sheet.CreateRow(i);
                rownumber.CreateCell((short)0).SetCellValue(ticket.ProjectTitle);
                rownumber.CreateCell((short)1).SetCellValue(ticket.TicketType.ToString());
                rownumber.CreateCell((short)2).SetCellValue(ticket.TicketID + " " + ticket.Title);
                rownumber.CreateCell((short)3).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.CreatedOn));
                rownumber.CreateCell((short)4).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.ModifiedOn));
                rownumber.CreateCell((short)5).SetCellValue(
                    GetClientStatusNameBySatisfyStatus((int)ticket.Status, ticket.TicketID, false));
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }
    }
}