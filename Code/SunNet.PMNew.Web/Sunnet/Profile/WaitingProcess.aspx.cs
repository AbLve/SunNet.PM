using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    public partial class WaitingProcess : BaseWebsitePage
    {
        SealsApplication app = new SealsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSeal.DataSource = app.GetList().FindAll(r => r.Status == Status.Active);
                ddlSeal.DataBind();
                ddlSeal.Items.Insert(0, new ListItem() { Value = "0", Text = "ALL" });

                BindRepeater();
            }
        }

        protected void aspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindRepeater();
        }

        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            aspNetPager1.CurrentPageIndex = 1;
            BindRepeater();
        }


        private void BindRepeater()
        {
            int recordCount;
            DateTime startDate;
            if (!DateTime.TryParse(txtStartDate.Text, out startDate)) //如果没有填时间，就当前时间减一个星期
                startDate = MinDate;

            DateTime endDate;
            if (!DateTime.TryParse(txtEndDate.Text, out endDate))
                endDate = MinDate;

            List<RequestStatus> status = new List<RequestStatus>();
            switch (UserInfo.Role)
            {
                case RolesEnum.ADMIN:
                    status.Add(RequestStatus.Submit);
                    break;
                case RolesEnum.PM:
                    status.Add(RequestStatus.Open);
                    status.Add(RequestStatus.Sealed);
                    break;
                default:
                    status.Add(RequestStatus.Approved);
                    break;
            }

            List<SealRequestsEntity> list =
                app.GetSealRequestsList(UserInfo.UserID, status, int.Parse(ddlSeal.SelectedValue), startDate, endDate
                , hidOrderBy.Value, hidOrderDirection.Value, aspNetPager1.CurrentPageIndex, aspNetPager1.PageSize, out recordCount);

            if (recordCount == 0)
            {
                trNoRecords.Visible = true;
                rptSealsRequest.DataSource = null;
                rptSealsRequest.DataBind();
            }
            else
            {
                trNoRecords.Visible = false;
                rptSealsRequest.DataSource = list;
                rptSealsRequest.DataBind();
            }
            aspNetPager1.RecordCount = recordCount;
        }

        protected string GetStautsHTML(object status)
        {
            RequestStatus requestStatus = (RequestStatus)status;
            string statusName = Enum.GetName(typeof(RequestStatus), status);
            if (requestStatus == RequestStatus.Denied)
            {
                return "<label style='color:red'>" + statusName + "<label>";
            }
            else
            {
                return "<label >" + statusName + "<label>";
            }
        }

    }
}
