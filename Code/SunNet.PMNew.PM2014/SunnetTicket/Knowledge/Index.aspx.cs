using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.ShareModel.DTO;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.SunnetTicket.Knowledge
{
    public partial class Index : TicketPageHelper
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "ModifiedOn";
            }
        }

        protected override string DefaultDirection
        {
            get
            {
                return "Desc";
            }
        }

        private ShareApplication shareApp = new ShareApplication();
        UserApplication userApp = new UserApplication();

        private void InitUsers()
        {
            SearchUsersRequest request = new SearchUsersRequest(
               SearchUsersType.All, false, " FirstName ", " ASC ");
            request.IsSunnet = true;
            SearchUserResponse response = userApp.SearchUsers(request);
            ddlUser.DataSource = response.ResultList;
            ddlUser.DataBind(delegate(UsersEntity user, string status)
            {
                return user.Status == status;
            });
            ddlUser.SelectItem(QS("user"));
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                shareApp.GetShareTypes()
                    .BindDropdown<ShareTypeEntity>(ddlType, "Title", "ID", DefaulAllText, "", QS("type"));
                InitUsers();
                txtStartDate.Text = QS("start");
                txtEndDate.Text = QS("end");
                txtKeyWord.Text = QS("keyword");
                InitData();
            }
        }

        private SearchShareResponse GetResponse(bool ispageMode)
        {
            SearchShareRequest request = new SearchShareRequest(ispageMode, CurrentPageIndex, anpShare.PageSize, OrderBy,
                OrderDirection);
            request.CreatedBy = QS("user", 0);
            request.Type = QS("type", 0);
            request.Keyword = txtKeyWord.Text;
            DateTime dt = DateTime.MinValue;

            if (DateTime.TryParse(txtStartDate.Text, out dt))
                request.StartDate = dt;
            if (DateTime.TryParse(txtEndDate.Text, out dt))
                request.EndDate = dt;
            var response = shareApp.GetShares(request);
            return response;
        }

        private void InitData()
        {
            var response = GetResponse(true);
            if (response != null && response.Dataset != null && response.Dataset.Count > 0)
            {
                rptShare.DataSource = response.Dataset;
                rptShare.DataBind();

                trNoTickets.Visible = false;
                rptShare.Visible = true;

                anpShare.RecordCount = response.Count;
            }
            else
            {
                trNoTickets.Visible = true;
                rptShare.Visible = false;
            }
        }

        protected void iBtnDownload_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";
            Page.EnableViewState = false;
            string fileName = string.Format("Knowledge Share - {0}.xls", DateTime.Now.ToString("yyyy_MM_dd"));
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Write("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=utf-8\"><title>Knowledge share report</title></head><body><center>");

            var response = GetResponse(false);
            StringBuilder dataHtml = new StringBuilder("<table border='1' style='width:100%;'><caption>Knowledge share report</caption>");
            dataHtml.Append(@"<tr>
                                <th>Ticket ID</th>
                                <th>Share Type</th>
                                <th>Note</th>
                                <th>Created By</th>
                                <th>Updated</th>
                            </tr>");
            if (response.Dataset != null)
            {
                foreach (var item in response.Dataset)
                {
                    dataHtml.AppendFormat(@"<tr>
                                                <th>{0}</th>
                                                <th>{1}</th>
                                                <th>{2}</th>
                                                <th>{3}</th>
                                                <th>{4}</th>
                                            </tr>",
                            item.TicketID, item.TypeEntity.Title, item.Note, GetSunnetUsername(item.CreatedBy), item.ModifiedOn.ToString("MM/dd/yyyy"));
                }
            }
            dataHtml.Append("</table>");
            Response.Write(dataHtml.ToString());
            Response.Write("</center></body></html>");
            Response.End();
        }

        protected void rptShare_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var rptFiles = (Repeater)e.Item.FindControl("rptFiles");
            var files = ((ShareEntity)e.Item.DataItem).Files;
            rptFiles.DataSource = files;
            rptFiles.DataBind();
        }

    }
}