using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.PM2014.Codes;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.SealModel.Enum;

namespace SunNet.PMNew.PM2014.OA.Seals
{
    public partial class WaitingProcessSeal : BasePage
    {
        protected override string DefaultOrderBy
        {
            get
            {
                return "RequestedDate";
            }
        }

        protected override string DefaultDirection
        {
            get { return "DESC"; }
        }


        SealsApplication app = new SealsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlType.SelectedValue = QS("type");
                ddlSeal.DataSource = app.GetList().FindAll(r => r.Status == Status.Active);
                ddlSeal.DataBind();
                ddlSeal.Items.Insert(0, new ListItem() { Value = "0", Text = "ALL" });
                ddlSeal.SelectedValue = QS("seal");
                txtKeyword.Text = QS("keyword");
                BindRepeater();
            }
        }


        private void BindRepeater()
        {
            int recordCount;
            DateTime startDate;
            if (!DateTime.TryParse(QS("start"), out startDate))
                startDate = MinDate;
            else
            {
                txtStartDate.Text = startDate.ToString("MM/dd/yyyy");
            }

            DateTime endDate;
            if (!DateTime.TryParse(QS("end"), out endDate))
                endDate = MinDate;
            else
            {
                if (endDate >= startDate)
                {
                    txtEndDate.Text = endDate.ToString("MM/dd/yyyy");
                }
            }


            List<RequestStatus> status = new List<RequestStatus>();
            status.Add(RequestStatus.Draft);
            status.Add(RequestStatus.Denied);
            status.Add(RequestStatus.Submitted);
            status.Add(RequestStatus.PendingApproval);
            status.Add(RequestStatus.Approved);
            status.Add(RequestStatus.PendingProcess);
            status.Add(RequestStatus.Processed);

            //switch (UserInfo.Role)
            //{
            //    case RolesEnum.ADMIN:
            //        status.Add(RequestStatus.Submitted);
            //        break;
            //    case RolesEnum.Sales:
            //        status.Add(RequestStatus.Submitted);
            //        break;
            //    case RolesEnum.PM:
            //        status.Add(RequestStatus.Draft);
            //        status.Add(RequestStatus.Processed);
            //        break;
            //    default:
            //        status.Add(RequestStatus.Approved);
            //        break;
            //}

            List<SealRequestsEntity> list =
                app.GetSealRequestsWaitingList(UserInfo.UserID, QS("keyword"), int.Parse(ddlType.SelectedValue), status, int.Parse(ddlSeal.SelectedValue), startDate, endDate
                , OrderBy, OrderDirection, CurrentPageIndex, anpWaitting.PageSize, out recordCount);

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
            anpWaitting.RecordCount = recordCount;
        }

        protected string GetStautsHTML(object status)
        {
            RequestStatus requestStatus = (RequestStatus)status;
            string statusName = requestStatus.RequestStatusToText();
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