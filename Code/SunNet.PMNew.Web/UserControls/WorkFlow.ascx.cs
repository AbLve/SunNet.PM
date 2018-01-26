using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class WorkFlow : BaseAscx
    {
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApplication = new UserApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int tid = QS("tid", 0);
                if (tid > 0)
                {
                    DataBind(tid);
                }
            }
        }

        private void DataBind(int tid)
        {
            if (WorkFlowTicketEntity == null) return;

            int status = (int)WorkFlowTicketEntity.Status;
            bool isEstimate = WorkFlowTicketEntity.IsEstimates;

            #region task calc

            float TaskCount = ticketApp.GetTasksCountByID(tid, true);
            float completedTaskCount = ticketApp.GetCompletedTasksCountByID(tid, true);
            float compRate = 0;
            if (TaskCount == 0 && completedTaskCount == 0)
            {
                compRate = 0;
            }
            else
            {
                compRate = (completedTaskCount / TaskCount);
            }

            #endregion

            ShowDivColorByStatus(status, isEstimate);

            if (userApplication.GetUser(WorkFlowTicketEntity.CreatedBy).Role == Entity.UserModel.RolesEnum.Supervisor)
            {
                divWaitClientVerified.InnerText = "Supervisor Verified Change";
            }
            #region Estimation

            if (null != WorkFlowTicketEntity)
            {
                this.lilEsHours.Text = WorkFlowTicketEntity.FinalTime.ToString();

                if (status < (int)TicketsState.Estimation_Fail)
                {
                    this.lilSaleState.Text = "<span>Waiting</span>";
                }
                else if (status == (int)TicketsState.Waiting_Sales_Confirm)
                {
                    this.lilSaleState.Text = "<span style='color: Red;'>Waiting Sales Confirm</span>";
                }
                else if (status == (int)TicketsState.Estimation_Fail)
                {
                    this.lilSaleState.Text = "<span style='color: Red;font-size:12px;'>Fail</span>";
                }
                if (WorkFlowTicketEntity.IsEstimates)
                {
                    if (status >= (int)TicketsState.Estimation_Approved ||
                       (status == (int)TicketsState.PM_Reviewed && WorkFlowTicketEntity.FinalTime > 0))
                    {
                        this.lilSaleState.Text = "<span style='color: Red;'>Pass</span>";
                    }
                }
                else
                {
                    if (status >= (int)TicketsState.PM_Reviewed)
                    {
                        this.lilSaleState.Text = "<span style='color: Red;'>Pass</span>";
                    }
                }
            }

            #endregion

            #region task

            this.lilTaskCount.Text = TaskCount.ToString();
            this.lilCompCount.Text = completedTaskCount.ToString();
            if (status < (int)TicketsState.Developing)
            {
                this.lilTask.Text = compRate < 1 ? string.Format("<span>{0}</span>", compRate.ToString("0%")) : compRate.ToString("0%");
            }
            else
            {
                this.lilTask.Text = compRate < 1 ? string.Format("<span style='color: Red;'>{0}</span>", compRate.ToString("0%")) : compRate.ToString("0%");
            }
            #endregion
        }

        /// <summary>
        /// show div color by ticket status
        /// </summary>
        /// <param name="status"></param>
        private void ShowDivColorByStatus(int status, bool isEn)
        {

            if (status == (int)TicketsState.Submitted ||
                status == (int)TicketsState.Cancelled)
            {
                this.lilDevResultOnLocal.Text = "waiting";
                this.lilQaResultOnLocal.Text = "waiting";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";
                this.trIsPmRev.Visible = false;

                this.lilEstimation.Text = "<img src='/images/grey_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
            }
            else if (status < (int)TicketsState.Estimation_Fail &&
                status >= (int)TicketsState.PM_Reviewed)
            {
                if (isEn)
                {
                    this.divPMReviewed.Attributes["class"] = "greenBox";
                    this.divEstimation.Attributes["class"] = "orangeBox";

                    this.lilDevResultOnLocal.Text = "waiting";
                    this.lilQaResultOnLocal.Text = "waiting";
                    this.lilDevResultOnCS.Text = "waiting";
                    this.lilQaResultOnCS.Text = "waiting";

                    this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                    this.lilDeveloping.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                }
                else
                {
                    this.divPMReviewed.Attributes["class"] = "greenBox";
                    this.divEstimation.Attributes["class"] = "greenBox";
                    this.divDeveloping.Attributes["class"] = "lightyellow";

                    this.lilDevResultOnLocal.Text = "waiting";
                    this.lilQaResultOnLocal.Text = "waiting";
                    this.lilDevResultOnCS.Text = "waiting";
                    this.lilQaResultOnCS.Text = "waiting";

                    this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                    this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                    this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                    this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                }

            }
            else if (status == (int)TicketsState.Estimation_Fail)
            {

                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divEstimation.Attributes["class"] = "orangeBox";

                this.lilDevResultOnLocal.Text = "waiting";
                this.lilQaResultOnLocal.Text = "waiting";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";

                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
            }
            else if (status > (int)TicketsState.Estimation_Fail &&
                     status < (int)TicketsState.Developing)
            {

                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "lightyellow";

                this.lilDevResultOnLocal.Text = "waiting";
                this.lilQaResultOnLocal.Text = "waiting";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";

                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

            }
            else if (status == (int)TicketsState.Developing)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "waiting";
                this.lilQaResultOnLocal.Text = "waiting";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";

                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "orangeBox";
            }
            else if (status < (int)TicketsState.Tested_Fail_On_Local &&
                status > (int)TicketsState.Developing)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "Prepare";
                this.lilQaResultOnLocal.Text = "Testing";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";

                this.divPMReviewed.Attributes["class"] = "greenBox";//
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "orangeBox";
            }
            else if (status == (int)TicketsState.Tested_Fail_On_Local)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='Not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "Prepare";
                this.lilQaResultOnLocal.Text = "<span style='color: Red; font-size:12px;'>Fail</span>";
                this.lilDevResultOnCS.Text = "waiting";
                this.lilQaResultOnCS.Text = "waiting";

                this.divPMReviewed.Attributes["class"] = "greenBox";//
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "orangeBox";
            }
            else if (status < (int)TicketsState.Tested_Fail_On_Client &&
                status > (int)TicketsState.Tested_Fail_On_Local)
            {

                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";

                this.divPMReviewed.Attributes["class"] = "greenBox";//
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                if (status <= (int)TicketsState.Tested_Success_On_Local)
                {
                    this.lilDevResultOnCS.Text = "waiting";
                    this.lilQaResultOnCS.Text = "waiting";
                    this.divTsonClientServer.Attributes["class"] = "lightyellow";
                }
                else
                {
                    this.lilDevResultOnCS.Text = "Prepare";
                    this.lilQaResultOnCS.Text = "Testing";
                    this.divTsonClientServer.Attributes["class"] = "orangeBox";
                }

            }
            else if (status == (int)TicketsState.Tested_Fail_On_Client)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='not pass'>";
                this.lilPmAudit.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Prepare";
                this.lilQaResultOnCS.Text = "<span style='color: Red;font-size:12px;'>Fail</span>";

                this.divPMReviewed.Attributes["class"] = "greenBox";//
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "orangeBox";

            }
            else if (status <= (int)TicketsState.Tested_Success_On_Client &&
               status > (int)TicketsState.Tested_Fail_On_Client)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Pass";
                this.lilQaResultOnCS.Text = "Pass";

                this.divPMReviewed.Attributes["class"] = "greenBox";//
                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "greenBox";
                this.divPMAudit.Attributes["class"] = "orangeBox";
            }
            else if (status == (int)TicketsState.PM_Deny)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPublishPS.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/grey_arrow.gif' alt='Not pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Pass";
                this.lilQaResultOnCS.Text = "Pass";
                this.lilPmAuditStatus.Text = "<span style='color: Red; font-size:13px;'>PM Deny</span>";

                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "greenBox";
                this.divPMAudit.Attributes["class"] = "orangeBox";
            }
            else if (status <= (int)TicketsState.Ready_For_Review &&
                status > (int)TicketsState.PM_Deny)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPublishPS.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/green_arrow.gif' alt='Not pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Pass";
                this.lilQaResultOnCS.Text = "Pass";

                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divPMAudit.Attributes["class"] = "greenBox";
                this.divPublshPS.Attributes["class"] = "greenBox";
                this.divWaitClientVerified.Attributes["class"] = "orangeBox";
            }
            else if (status == (int)TicketsState.Not_Approved)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/grey_arrow.gif' alt='not pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPublishPS.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/green_arrow.gif' alt='pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Pass";
                this.lilQaResultOnCS.Text = "Pass";

                //spanComp.Visible = false;
                this.lilCompWord.Text = "<span style='color: Red;font-size: 18px;'>Result:Client Not Approved</span>";

                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divPMAudit.Attributes["class"] = "greenBox";
                this.divPublshPS.Attributes["class"] = "greenBox";
                this.divWaitClientVerified.Attributes["class"] = "lightyellow";
            }
            else if (status == (int)TicketsState.Completed)
            {
                this.lilEstimation.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilDeveloping.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilComplete.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilTsonClientServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPmAudit.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilPublishPS.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilWaitClientVerified.Text = "<img src='/images/green_arrow.gif' alt='pass'>";
                this.lilTsonLocalServer.Text = "<img src='/images/green_arrow.gif' alt='pass'>";

                this.lilDevResultOnLocal.Text = "Pass";
                this.lilQaResultOnLocal.Text = "Pass";
                this.lilDevResultOnCS.Text = "Pass";
                this.lilQaResultOnCS.Text = "Pass";

                this.divEstimation.Attributes["class"] = "greenBox";
                this.divDeveloping.Attributes["class"] = "greenBox";
                this.divPMReviewed.Attributes["class"] = "greenBox";
                this.divTsonClientServer.Attributes["class"] = "greenBox";
                this.divTsOnLocalServer.Attributes["class"] = "greenBox";
                this.divPMAudit.Attributes["class"] = "greenBox";
                this.divPublshPS.Attributes["class"] = "greenBox";
                this.divWaitClientVerified.Attributes["class"] = "greenBox";
                this.divComp.Attributes["class"] = "greenBox";
            }
        }

        public TicketsEntity WorkFlowTicketEntity { get; set; }
    }
}