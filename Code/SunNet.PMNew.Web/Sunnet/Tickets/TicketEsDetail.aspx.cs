using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class TicketEsDetail : BaseWebsitePage
    {
        TicketsApplication TicketApp = new TicketsApplication();
        UserApplication userAPP = new UserApplication();
        string isFinal = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.hdIsFinal.Value = isFinal == "iushau02u340" ? "true" : "false";
            if (!IsPostBack)
            {
                isFinal = QS("isFinal");
                int tid = QS("tid", 0);
                if (tid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                DateBind(isFinal, tid);
                ShowActionForSaler(tid);
            }
        }

        private void DateBind(string isFinal, int tid)
        {

            List<TicketEsTime> list = TicketApp.GetListEs(tid);

            this.trNoTickets.Visible = list.Count <= 0 ? true : false;
            this.trNoTickets02.Visible = true;
            if (isFinal == "iushau02u340final")
            {
                List<TicketEsTime> listEsFinal = list.FindAll(x => x.IsPM == true);
                TicketEsTime model2 = new TicketEsTime();
                model2.TotalTimes = listEsFinal.Sum(x => x.TotalTimes);
                model2.DevAdjust = listEsFinal.Sum(x => x.DevAdjust);
                model2.DocTime = listEsFinal.Sum(x => x.DocTime);
                model2.GrapTime = listEsFinal.Sum(x => x.GrapTime);
                model2.QaAdjust = listEsFinal.Sum(x => x.QaAdjust);
                model2.TrainingTime = listEsFinal.Sum(x => x.TrainingTime);
                model2.Week = "Total:";
                listEsFinal.Add(model2);
                this.trNoTickets02.Visible = listEsFinal.Count <= 0 ? true : false;
                this.rptTicketsEsListFinal.DataSource = listEsFinal;
                this.rptTicketsEsListFinal.DataBind();
            }

            List<TicketEsTime> listEs = list.FindAll(x => x.IsPM == false);
            TicketEsTime model = new TicketEsTime();
            model.TotalTimes = listEs.Sum(x => x.TotalTimes);
            model.TotalTimes = listEs.Sum(x => x.TotalTimes);
            model.DevAdjust = listEs.Sum(x => x.DevAdjust);
            model.DocTime = listEs.Sum(x => x.DocTime);
            model.GrapTime = listEs.Sum(x => x.GrapTime);
            model.QaAdjust = listEs.Sum(x => x.QaAdjust);
            model.TrainingTime = listEs.Sum(x => x.TrainingTime);
            model.Week = "Total:";
            listEs.Add(model);

            this.rptTicketsEsList.DataSource = listEs;
            this.rptTicketsEsList.DataBind();
        }

        private void ShowActionForSaler(int tid)
        {
            GetTicketCreateByAndStatusResponse response = TicketApp.GetTicketCreateByAndStatus(tid);
            int status = response == null ? 0 : response.status;
            if (status == (int)TicketsState.Waiting_Sales_Confirm)
            {
                if (UserInfo.Role == RolesEnum.Sales)
                {
                    this.lilSalesStatusEsFail.Text = "<input  type='button' class='btnthree' id='btnApp' value='Estimation Approved' onclick=\"updateStatusConfirm('estApp',false);return false;\"/>";
                    this.lilSalesStatusWaitForDev.Text = "<input  type='button' class='btnthree' id='btnDeny' value='Estimation Deny' onclick=\"updateStatusConfirm('estDeny',false);return false;\"/>";
                }
            }
        }
        #region commen method

        protected string GetUserNameByCreateID(string cid)
        {
            string userName = userAPP.GetFirstName(Convert.ToInt32(cid));
            return userName.Length == 0 ? "" : userName;
        }

        #endregion
    }
}
