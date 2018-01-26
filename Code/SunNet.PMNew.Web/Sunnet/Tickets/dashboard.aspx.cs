using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;

namespace SunNet.PMNew.Web.Sunnet.Tickets
{
    public partial class dashboard : BaseWebsitePage
    {
        #region declare
        TicketOrderApplication toApp = new TicketOrderApplication();
        ProjectApplication proApp = new ProjectApplication();
        TicketsApplication ticketApp = new TicketsApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();
        UserApplication userApp = new UserApplication();
        List<TicketsOrderEntity> listOrder;
        List<ProjectsEntity> list = new List<ProjectsEntity>();
        List<TicketsEntity> listTickets = new List<TicketsEntity>();
        #endregion

        #region FeedbackMessage

        FeedBackMessageHandler fbmHandler;
      
        protected string ShowAction(object ticketId)
        {
            return fbmHandler.ShowAction(ticketId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            AllowStatusList = AllowStatus();
            #region initial
            list = proApp.GetUserProjects(UserInfo, DateTime.Now.AddDays(-15));
            listOrder = toApp.GetListByProjectId(PidList());
            int userId = 0;
            if (UserInfo.Role == RolesEnum.Contactor)
                userId = UserInfo.UserID;
            listTickets = ticketApp.GetListTopTenTicketsByProjects(PidList(), AllowStatus(), userId);
            #endregion
            if (!IsPostBack)
            {
                BindDataList();
            }

            ((Main)this.Master).TopSelectedIndex = 2;
        }

        /// <summary>
        /// get pid string list
        /// </summary>
        /// <returns></returns>
        public string PidList()
        {
            string pidList = "";
            foreach (ProjectsEntity item in list)
            {
                if (item.ProjectID > 0)
                {
                    pidList += item.ProjectID + ",";
                }
            }
            return pidList.TrimEnd(',');
        }

        private void BindDataList()
        {
            if (list.Count <= 0)
            {
                ProjectDetailDTO entity = new ProjectDetailDTO();
                entity.ProjectID = 0;
                entity.Title = "No Project";
                list.Add(entity);
            }
            this.rptListProjects.DataSource = list;

            this.rptListProjects.DataBind();
        }

        #region allow shwo status
        int[] allowShowStatus = { (int)TicketsState.Submitted,       (int)TicketsState.Waiting_For_Estimation, 
                                  (int)TicketsState.Developing,      (int)TicketsState.Waiting_Sales_Confirm, 
                                  (int)TicketsState.PM_Reviewed,      (int)TicketsState.PM_Verify_Estimation, 
                                  (int)TicketsState.Testing_On_Local,  (int)TicketsState.Testing_On_Client, 
                                  (int)TicketsState.Ready_For_Review,(int)TicketsState.Estimation_Approved, 
                                  (int)TicketsState.Tested_Success_On_Local,  (int)TicketsState.Tested_Success_On_Client, 
                                  (int)TicketsState.Tested_Fail_On_Local, (int)TicketsState.Tested_Fail_On_Client, 
                                  (int)TicketsState.Not_Approved,     (int)TicketsState.PM_Deny};


        string AllowStatusList = "";

        private string AllowStatus()
        {
            string temp = "";
            foreach (int item in allowShowStatus)
            {
                temp += item + ",";
            }
            return temp.TrimEnd(',');
        }

        #endregion

        protected void rptListProjects_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int pid = ((ProjectsEntity)e.Item.DataItem).ProjectID;

                if (pid <= 0) return;

                Repeater rep = e.Item.FindControl("rptListTickets") as Repeater;
                List<TicketsEntity> list = listTickets.FindAll(x => x.ProjectID == pid);

                if (list == null || list.Count <= 0)
                {
                    ((System.Web.UI.HtmlControls.HtmlTableRow)e.Item.FindControl("trNoTickets")).Visible = true;
                }

                rep.DataSource = list;

                rep.DataBind();

            }
        }

        #region attention status
        int[] SalerAttentionStatus = { (int)TicketsState.Waiting_Sales_Confirm };
        int[] DevAttentionStatus = { (int)TicketsState.PM_Reviewed };
        int[] DevAttentionStatusWithEs = {(int)TicketsState.Estimation_Approved,(int)TicketsState.Waiting_For_Estimation,
                                          (int)TicketsState.Tested_Fail_On_Local,(int)TicketsState.Tested_Success_On_Local, 
                                          (int)TicketsState.Tested_Fail_On_Client  };
        int[] QaAttentionStatus = { (int)TicketsState.Waiting_For_Estimation, (int)TicketsState.Testing_On_Client, 
                                    (int)TicketsState.Testing_On_Local };
        int[] PmAttentionStatus = { (int)TicketsState.Submitted, (int)TicketsState.PM_Reviewed,
                                    (int)TicketsState.PM_Verify_Estimation, (int)TicketsState.Tested_Success_On_Client,
                                    (int)TicketsState.Tested_Success_On_Client,(int)TicketsState.Not_Approved};
        #endregion

        #region commen show

        public string ShowPriorityImgByDevDate(string date)
        {
            string imgString = "";

            DateTime Now = DateTime.Now.Date;

            TimeSpan Diff = Convert.ToDateTime(date).Subtract(Now);

            int DiffDate = Diff.Days;

            if (DiffDate <= 3 && DiffDate > 0)
            {
                imgString = "<img src='/icons/02.gif' />";
            }
            else if (DiffDate == 0)
            {
                imgString = "<img src='/icons/03.gif' />";
            }
            else if (Convert.ToDateTime(date) > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() && DiffDate < 0)
            {
                imgString = "<img src='/icons/01.gif' />";
            }

            return imgString;
        }

        public string ShowOrderNumberByTid(int tid, string priority)
        {
            TicketsOrderEntity model = listOrder.Find(x => x.TicketID == tid);
            bool existOrderNumber = model != null ? true : false;
            return existOrderNumber == true ? model.OrderNum.ToString() : priority;

        }

        protected string TrStyle(int index, int notBug)
        {
            if ((CovertDeleteState)notBug == CovertDeleteState.NotABug)
            {
                return "listrowHightLight";
            }
            else
            {
                return index % 2 == 0 ? "listrowone" : "listrowtwo";
            }
        }

        public string ShowLinkMore(string pid)
        {
            string LinkMore = string.Format(" <a href='ListTicket.aspx?pid={0}'>More...</a>", pid);

            return pid == "0" ? "" : LinkMore;
        }

        private bool NotPreChangRed(int tid)
        {
            List<int> list = ticketApp.GetEsUser(tid);
            if (null != list)
            {
                if (list.Contains(UserInfo.UserID))
                {
                    return false;
                }
            }
            return true;
        }

        protected string ChangeStatus(object status, int ticketId, decimal FinalTime, bool Es)
        {
            string tmp = string.Empty;
            try
            {
                string statusResult = fbmHandler.GetDashboardStatus(status, ticketId);

                if (!statusResult.Contains("span"))
                {
                    int statusToInt = Convert.ToInt32((TicketsState)Enum.Parse(typeof(TicketsState), statusResult));
                    switch (UserInfo.Role)
                    {
                        case RolesEnum.DEV:
                        case RolesEnum.Contactor:
                        case RolesEnum.Leader:
                            {
                                if (Es)
                                {
                                    if (DevAttentionStatusWithEs.Contains(statusToInt) || (statusToInt == (int)TicketsState.PM_Reviewed && FinalTime > 0))
                                    {
                                        if (statusToInt == (int)TicketsState.Waiting_For_Estimation)
                                        {
                                            if (!NotPreChangRed(ticketId))
                                            {
                                                tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                            }
                                        }
                                        else
                                        {
                                            tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                        }
                                    }
                                }
                                else
                                {
                                    if (DevAttentionStatus.Contains(statusToInt))
                                    {
                                        tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                    }
                                }
                                break;
                            }
                        case RolesEnum.QA:
                            {
                                if (QaAttentionStatus.Contains(statusToInt))
                                {
                                    if (statusToInt == (int)TicketsState.Waiting_For_Estimation)
                                    {
                                        if (!NotPreChangRed(ticketId))
                                        {
                                            tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                        }
                                    }
                                    else
                                    {
                                        tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                    }
                                }
                                break;
                            }
                        case RolesEnum.Sales:
                            {
                                if (SalerAttentionStatus.Contains(statusToInt))
                                {
                                    tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                }
                                break;
                            }
                        case RolesEnum.PM:
                            {
                                if (PmAttentionStatus.Contains(statusToInt))
                                {
                                    if (!(statusToInt == (int)TicketsState.PM_Reviewed && FinalTime > 0))
                                    {
                                        tmp = string.Format("<span style='color:red;'>{0}</span>", statusResult.Replace('_', ' '));
                                    }
                                }
                                break;
                            }
                        default:
                            {
                                tmp = statusResult.Replace('_', ' ');
                                break;
                            }
                    }
                }
                tmp = statusResult.Replace('_', ' ');
                tmp += "&nbsp;" + fbmHandler.FeedBackMessage(ticketId, UserInfo.Role);
                return tmp;
            }
            catch (Exception ex)
            {
                WebLogAgent.Write(ex);
                return "";
            }
        }
        #endregion
    }
}
