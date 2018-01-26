using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoUpdateTicketStatus : IHttpHandler
    {
        #region declare
        TicketsApplication ticketAPP = new TicketsApplication();
        TicketStatusManagerApplication ticketStatusMgr = new TicketStatusManagerApplication();
        bool notSendEmail = false;
        bool ChangeToEs = false;
        string[] NostatusArray = { "isBug", "notBug", "toEs", "toNotEs" };
        string[] statusArray = { "pReview", "approve", "deny", "estApp", "estDeny" };
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                String statusValue = context.Request["statusValue"];

                int tid = Convert.ToInt32(context.Request["tid"]);

                TicketsEntity ticketEntity = new TicketsEntity();

                ticketEntity = ticketAPP.GetTickets(tid);

                TicketsState originalStatus = ticketEntity.Status;
                bool Update = true;
                bool isCompleteMsgInfo = false;
                if (statusValue == "pReview")
                {
                    //
                    if (HasDevOrQaUnderTicket(ticketEntity))
                    {
                        ticketEntity.Status = TicketsState.PM_Reviewed;
                    }
                    else
                    {
                        context.Response.Write("696");
                        //context.Response.Write("Please assign user before you change status.");
                        return;
                    }

                }
                else if (statusValue == "pmReviewMaintenanceValidate")
                {
                    ProjectApplication projectApplication = new ProjectApplication();
                    ProjectsEntity projectsEntity = projectApplication.Get(ticketEntity.ProjectID);

                    if ((projectsEntity.MainPlanOption == UserMaintenancePlanOption.NO ||
                        projectsEntity.MainPlanOption == UserMaintenancePlanOption.NEEDAPPROVAL
                        || projectsEntity.MainPlanOption == UserMaintenancePlanOption.ALLOWME) &&
                        ticketEntity.IsEstimates == false)
                    {
                        context.Response.Write("-1");
                    }
                    else
                    {
                        context.Response.Write("1");
                    }
                    return;
                }
                else if (statusValue == "approve")
                {
                    isCompleteMsgInfo = true;
                    ticketEntity.Status = TicketsState.Completed;
                }
                else if (statusValue == "deny")
                {
                    ticketEntity.Status = TicketsState.Not_Approved;
                }
                else if (statusValue == "estApp")
                {
                    ticketEntity.Status = TicketsState.Estimation_Approved;
                }
                else if (statusValue == "estDeny")
                {
                    ticketEntity.Status = TicketsState.Estimation_Fail;
                }
                else if (statusValue == "notBug")
                {
                    ticketEntity.ConvertDelete = CovertDeleteState.NotABug;
                }
                else if (statusValue == "isBug")
                {
                    ticketEntity.ConvertDelete = CovertDeleteState.Normal;
                    notSendEmail = true;
                }
                else if (statusValue == "toEs")
                {
                    ticketEntity.IsEstimates = true;
                    ChangeToEs = false;
                }
                else if (statusValue == "toNotEs")
                {
                    ticketEntity.IsEstimates = false;
                }
                bool IsPass = true;

                #region //validate
                if (statusArray.Contains(statusValue))
                {
                    if (!BaseValidate(originalStatus, ticketEntity.Status))
                    {
                        IsPass = false;
                    }

                }
                else
                {
                    if (!NostatusArray.Contains(statusValue))
                    {
                        if (BaseValidate(originalStatus, (TicketsState)Enum.Parse(typeof(TicketsState), statusValue)))
                        {
                            ticketEntity.Status = (TicketsState)Enum.Parse(typeof(TicketsState), statusValue);
                        }
                        else
                        {
                            IsPass = false;
                        }
                    }
                }
                if (!IsPass)
                {
                    context.Response.Write("same");
                    return;
                }
                #endregion

                ticketEntity.ModifiedOn = DateTime.Now;
                ticketEntity.ModifiedBy = IdentityContext.UserID;
                ticketEntity.PublishDate = DateTime.Now.Date;
                Update = ticketAPP.UpdateTickets(ticketEntity);

                #region send email
                if (!notSendEmail)
                {
                    if (ChangeToEs)
                    {
                        ticketStatusMgr.SendEmailToSalerWithStatusToEs(ticketEntity);
                    }
                    else
                    {
                        if (statusValue == "notBug")
                        {
                            ticketStatusMgr.SendEmailToAllUserUnderProjectWithNotABug(ticketEntity);
                        }
                        else if (ticketEntity.Status == TicketsState.Ready_For_Review)
                        {
                            ticketStatusMgr.SendEmailtoClientForVerify(ticketEntity);
                        }
                        else if (ticketEntity.Status == TicketsState.Not_Approved)
                        {
                            ticketStatusMgr.SendEmailWithClientNotApp(ticketEntity);
                        }
                        else if (ticketEntity.Status == TicketsState.Waiting_For_Estimation)
                        {
                            ticketStatusMgr.SendEmailToAssignedUserTs(ticketEntity);
                        }
                        else if (ticketEntity.Status == TicketsState.Waiting_Sales_Confirm)
                        {
                            ticketStatusMgr.SendEmailToSalerWithStatusToEs(ticketEntity);
                        }
                        else if (ticketEntity.Status == TicketsState.Tested_Fail_On_Local ||
                                ticketEntity.Status == TicketsState.Testing_On_Local ||
                                ticketEntity.Status == TicketsState.Tested_Success_On_Local ||
                                ticketEntity.Status == TicketsState.Tested_Fail_On_Client ||
                                ticketEntity.Status == TicketsState.Testing_On_Client ||
                                ticketEntity.Status == TicketsState.Tested_Success_On_Client ||
                                ticketEntity.Status == TicketsState.Cancelled)
                        {
                            ticketStatusMgr.SendEmailToUserWithSpecStatus(ticketEntity);
                            if (ticketEntity.Status == TicketsState.Tested_Success_On_Client)
                            {
                                ticketStatusMgr.SendEmailToUserWithTestRSuccToPm(ticketEntity);
                            }
                        }
                        else
                        {
                            if (ticketEntity.Status != TicketsState.Developing)
                            {
                                ticketStatusMgr.SendEmailToQaAndDevWhenStatusChanged(ticketEntity);
                            }
                        }
                    }

                }
                #endregion

                if (Update)
                {
                    if (statusValue == "notBug")
                    {
                        context.Response.Write("Question sent, please wait for PM to verify.");
                    }
                    else
                    {
                        if (isCompleteMsgInfo)
                        {
                            context.Response.Write("The ticket has been approved.");
                        }
                        else
                        {
                            context.Response.Write("The ticket’s status has been updated.");
                        }
                    }
                }
                else
                {
                    context.Response.Write("Update ticket’s status fail.");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!" + ex.Message);
                WebLogAgent.Write(string.Format("Error Ashx:DoUpdateTicketStatus.ashx Messages:\r\n{0}", ex));
                return;
            }

        }
        public bool HasDevOrQaUnderTicket(TicketsEntity entity)
        {
            if (null == entity || entity.TicketID <= 0) return false;
            List<TicketUsersEntity> listUser = ticketAPP.GetListUsersByTicketId(entity.TicketID);
            if (listUser == null || listUser.Count <= 0) return false;
            if (listUser.FindAll(x => (x.Type == TicketUsersType.Dev || x.Type == TicketUsersType.QA)).Count > 0)
                return true;
            return false;
        }

        private bool BaseValidate(TicketsState ts, TicketsState status)
        {
            if (ts == status)
            {
                return false;
            }
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
