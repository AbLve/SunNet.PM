using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Extensions;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;


namespace SunNet.PMNew.PM2014.Report
{
    public partial class TicketsExport : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "TicketID";
            }
        }

        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }

        #region declare

        string pid = "";
        int page = 1;
        int recordCount;
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication(); 

        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();

        protected DateTime StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtStartDate.Text))
                {
                    return new DateTime(1753, 1, 1);
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(txtStartDate.Text, out dt))
                    {
                        return dt;
                    }
                    return new DateTime(1753, 1, 1);
                }
            }

        }
        protected DateTime EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(txtEndDate.Text))
                {
                    return new DateTime(2200, 1, 1);
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParse(txtEndDate.Text, out dt))
                    {
                        return dt;
                    }
                    return new DateTime(2200, 1, 1);
                }
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            listPorject = proApp.GetUserProjects(UserInfo);
          
            if (!IsPostBack)
            {
                pid = QS("pid");
            
                FillSearchDto();
              
                TicketsDataBind();

                if (!string.IsNullOrEmpty(pid))
                {
                    if (!CheckSecurity(Convert.ToInt32(pid)))
                    {
                        Response.Redirect("~/SunnetTicket/dashboard.aspx");
                        return;
                    }
                }
            }
        }

        
 

        private void InitProjectTitleBind(string selected)
        {
            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind((ProjectDetailDTO project, string status) => project.Status.ToString() == status);
            this.ddlProject.SelectItem(selected);
        }

        private void FillSearchDto()
        {
            txtKeyWord.Text = QS("keyword");
            ddlTicketType.SelectItem(QS("tickettype"));
            InitProjectTitleBind(QS("project"));
            txtStartDate.Text = QS("startdate");
            txtEndDate.Text = QS("enddate");
        }

        private void TicketsDataBind()
        {
            var project = 0;
            int.TryParse(ddlProject.SelectedValue, out project);
            var typeValue = -1;
            var type = TicketsType.None;
            if (int.TryParse(ddlTicketType.SelectedValue, out typeValue) && typeValue >= 0)
                type = (TicketsType)typeValue;

            List<TicketsEntity> list = ticketAPP.TickectsExport(UserInfo, txtKeyWord.Text, project, type, StartDate, EndDate,
                 CurrentPageIndex, anpOngoing.PageSize, OrderBy, OrderDirection, out recordCount);

            if (null == list || list.Count <= 0)
            {
                this.trNoTickets.Visible = true;
            }
            else
            {
                this.trNoTickets.Visible = false;
            }
            this.rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();
            anpOngoing.RecordCount = recordCount;
        }
        private void DataListForExcel()
        {
            var project = 0;
            int.TryParse(ddlProject.SelectedValue, out project);
            var typeValue = -1;
            var type = TicketsType.None;
            if (int.TryParse(ddlTicketType.SelectedValue, out typeValue) && typeValue >= 0)
                type = (TicketsType)typeValue;

            List<TicketsEntity> list = ticketAPP.TickectsExport(UserInfo, txtKeyWord.Text, project, type, StartDate, EndDate,
                 1, int.MaxValue, OrderBy, OrderDirection, out recordCount);

            ExportExcel(list);
        }

        private bool CheckSecurity(int pid)
        {
            List<ProjectDetailDTO> list = new List<ProjectDetailDTO>();
            list = proApp.GetUserProjects(UserInfo);
            list = list.FindAll(x => x.ProjectID == pid);
            return null != list && list.Count > 0;
        }

       
        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            DataListForExcel();
            
        }

        private void ExportExcel(List<TicketsEntity> list)
        {
            Response.ContentType = "application/vnd.ms-excel";
            string datetime = DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "");
            string projectName = "ALL";
            projectName = ddlProject.SelectedValue == "0" ? projectName : ddlProject.SelectedItem.Text;

            string ticketType = "ALL";
            ticketType = ddlTicketType.SelectedValue == "-1" ? ticketType : ddlTicketType.SelectedItem.Text;
             

            string filename = string.Empty;
            if (projectName == "ALL" && ticketType == "ALL" )
                filename = string.Format("ALL_{0}.xls", datetime);
            else
            {
                projectName = projectName == "ALL" ? "AllProject" : projectName;
                ticketType = ticketType == "ALL" ? "ALLType" : ticketType; 
                filename = string.Format("{0}_{1}.xls", projectName,  ticketType);
            }

            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);

            HSSFWorkbook workbook = new HSSFWorkbook();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();

            //Set Sheet ColumnWidth
            sheet.SetColumnWidth(0, 30 * 256);
            sheet.SetColumnWidth(1, 20 * 256);
            sheet.SetColumnWidth(2, 50 * 256);
            sheet.SetColumnWidth(3, 20 * 256);
            sheet.SetColumnWidth(4, 20 * 256);
            sheet.SetColumnWidth(5, 25 * 256);
            sheet.SetColumnWidth(6, 25 * 256);
            sheet.DisplayGridlines = false;

            HSSFRow row_head = (HSSFRow)sheet.CreateRow(0);
            row_head.HeightInPoints = 20;

            row_head.CreateCell((short)0).SetCellValue("Project");
            row_head.CreateCell((short)1).SetCellValue("Ticket ID");
            row_head.CreateCell((short)2).SetCellValue("Title");
            row_head.CreateCell((short)3).SetCellValue("Type"); 
            row_head.CreateCell((short)4).SetCellValue("Status");
            row_head.CreateCell((short)5).SetCellValue("Created On");
            row_head.CreateCell((short)6).SetCellValue("Updated On");  


            HSSFCellStyle hStyle = (HSSFCellStyle)workbook.CreateCellStyle();

            hStyle.BorderRight = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;
            hStyle.BorderBottom = (NPOI.SS.UserModel.CellBorderType)NPOI.SS.UserModel.BorderStyle.THIN;

            //Set head row background color
       

            HSSFFont font = (HSSFFont)workbook.CreateFont(); 
            font.FontName = "Verdana";
            font.FontHeightInPoints = 12;

            hStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.CENTER;
            hStyle.SetFont(font);

            foreach (var c in row_head.Cells)
            {
                c.CellStyle = hStyle;
            }
            int i = 1;
            foreach (TicketsEntity ticket in list)
            {
                HSSFRow rownumber = (HSSFRow)sheet.CreateRow(i);
                rownumber.CreateCell((short)0).SetCellValue(ticket.ProjectTitle);
                rownumber.CreateCell((short)1).SetCellValue(ticket.TicketID.ToString());//TicketType
                rownumber.CreateCell((short)2).SetCellValue(ticket.Title);
                rownumber.CreateCell((short)3).SetCellValue(ticket.TicketType.ToString());
                rownumber.CreateCell((short)4).SetCellValue(GetClientStatusNameBySatisfyStatus((int)ticket.Status, ticket.TicketID, false)); 
                rownumber.CreateCell((short)5).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.CreatedOn));
                rownumber.CreateCell((short)6).SetCellValue(string.Format("{0:M/d/yyyy}", ticket.ModifiedOn)); 

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
    }
}