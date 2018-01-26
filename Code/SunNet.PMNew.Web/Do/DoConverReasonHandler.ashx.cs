using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoConverReasonHandler : IHttpHandler
    {
        #region declare
        TicketsApplication ticketAPP = new TicketsApplication();
        ProjectApplication projectApp = new ProjectApplication();
        TicketsRelationApplication trApp = new TicketsRelationApplication();
        UserApplication userApp = new UserApplication();
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                #region get value

                String action = context.Request["action"];
                String desc = (context.Request["desc"]).NoHTML();
                bool ckIsSave = "no" == context.Request["ckIsSave"] ? false : true;
                int tid = Convert.ToInt32(context.Request["tid"]);

                #endregion

                #region declare

                int result = 0;

                TicketsEntity entity = new TicketsEntity();

                TicketsEntity Originalentity = ticketAPP.GetTickets(tid);

                bool Update = true;

                #endregion

                if (action == "cRequest")
                {
                    if (ckIsSave)
                    {
                        #region add

                        entity.ProjectID = Originalentity.ProjectID;
                        entity.CompanyID = Originalentity.CompanyID;
                        entity.Priority = Originalentity.Priority;
                        entity.TicketType = TicketsType.Request;
                        entity.Title = Originalentity.Title;
                        entity.URL = Originalentity.URL;
                        entity.FullDescription = Originalentity.TicketCode + ":" + Originalentity.FullDescription +
                                             string.Format(Environment.NewLine +
                                             "==================================" +
                                             Environment.NewLine +
                                             "Convert Reason:{0}", desc.TrimStart());
                        entity.CreatedBy = Originalentity.CreatedBy;
                        entity.CreatedOn = DateTime.Now;
                        entity.ModifiedOn = DateTime.Now;
                        entity.Status = TicketsState.Submitted;
                        entity.IsEstimates = Originalentity.IsEstimates;
                        entity.TicketCode = "R";
                        entity.IsInternal = false;
                        entity.ModifiedBy = 0;
                        entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.ConvertDelete = CovertDeleteState.Normal;
                        entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.Source = userApp.GetUser(IdentityContext.UserID).Role;
                        result = ticketAPP.AddTickets(entity);

                        if (result > 0)
                        {
                            TicketUsersEntity ticketUserEntity = new TicketUsersEntity();
                            ticketUserEntity.Type = TicketUsersType.PM;
                            ticketUserEntity.TicketID = result;
                            ProjectsEntity projectEntity = projectApp.Get(entity.ProjectID);
                            if (projectEntity != null)
                            {
                                ticketUserEntity.UserID = projectEntity.PMID;
                                ticketAPP.AddTicketUser(ticketUserEntity);
                            }
                            else
                            {
                                WebLogAgent.Write(string.Format("Add Pm To Ticket User Error:Project :{0},Ticket:{1},CreateDate:{2}",
                                       entity.ProjectID, result, DateTime.Now));

                            }
                            ticketUserEntity.Type = TicketUsersType.Create;
                            ticketUserEntity.TicketID = result;
                            ticketUserEntity.UserID = Originalentity.CreatedBy;
                            ticketAPP.AddTicketUser(ticketUserEntity);
                        }
                        #endregion

                        #region add relation

                        TicketsRelationDTO dtoEntity = new TicketsRelationDTO();

                        AddTicketsRelationRequest request = new AddTicketsRelationRequest();

                        dtoEntity.RTID = Convert.ToInt32(result);

                        dtoEntity.TID = Convert.ToInt32(tid);

                        dtoEntity.CreatedBy = Originalentity.CreatedBy;

                        request.dto = dtoEntity;

                        trApp.AddTR(request);

                        #endregion

                        #region history
                        TicketHistorysEntity historEntity = new TicketHistorysEntity();
                        historEntity.ModifiedBy = IdentityContext.UserID;
                        historEntity.ModifiedOn = DateTime.Now.Date;
                        historEntity.TicketID = Originalentity.TicketID;
                        historEntity.Description = entity.FullDescription = Originalentity.TicketCode + ":" + Originalentity.FullDescription +
                                           string.Format(Environment.NewLine +
                                           "==================================" +
                                           Environment.NewLine +
                                           "Convert Reason:{0}" +
                                           Environment.NewLine +
                                           "Convert By:{1}"
                                           , desc.TrimStart()
                                           , userApp.GetLastNameFirstName(IdentityContext.UserID)
                                           );
                        ticketAPP.AddTicketHistory(historEntity);
                        #endregion

                        #region update

                        Originalentity.ConvertDelete = CovertDeleteState.ConvertToHistory;
                        Originalentity.ModifiedOn = DateTime.Now;
                        Originalentity.ModifiedBy = IdentityContext.UserID;

                        Update = ticketAPP.UpdateTickets(Originalentity);

                        #endregion

                    }
                    else
                    {
                        #region add

                        entity.ProjectID = Originalentity.ProjectID;
                        entity.CompanyID = Originalentity.CompanyID;
                        entity.Priority = Originalentity.Priority;
                        entity.TicketType = TicketsType.Request;
                        entity.Title = Originalentity.Title;
                        entity.URL = Originalentity.URL;
                        entity.FullDescription = Originalentity.TicketCode + ":" + Originalentity.FullDescription +
                                           string.Format(Environment.NewLine +
                                           "==================================" +
                                           Environment.NewLine +
                                           "Convert Reason:{0}", desc.TrimStart());
                        entity.CreatedBy = Originalentity.CreatedBy;
                        entity.CreatedOn = DateTime.Now;
                        entity.ModifiedOn = DateTime.Now;
                        entity.Status = TicketsState.Submitted;
                        entity.IsEstimates = Originalentity.IsEstimates;
                        entity.TicketCode = "R";
                        entity.IsInternal = false;
                        entity.ModifiedBy = 0;
                        entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.ConvertDelete = CovertDeleteState.Normal;
                        entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                        entity.Source = userApp.GetUser(IdentityContext.UserID).Role;
                        result = ticketAPP.AddTickets(entity);

                        if (result > 0)
                        {
                            TicketUsersEntity ticketUserEntity = new TicketUsersEntity();
                            ticketUserEntity.Type = TicketUsersType.PM;
                            ticketUserEntity.TicketID = result;
                            ProjectsEntity projectEntity = projectApp.Get(entity.ProjectID);
                            if (projectEntity != null)
                            {
                                ticketUserEntity.UserID = projectEntity.PMID;
                                ticketAPP.AddTicketUser(ticketUserEntity);
                            }
                            else
                            {
                                WebLogAgent.Write(string.Format("Add Pm To Ticket User Error:Project :{0},Ticket:{1},CreateDate:{2}",
                                    entity.ProjectID, result, DateTime.Now));
                            }
                            ticketUserEntity.Type = TicketUsersType.Create;
                            ticketUserEntity.TicketID = result;
                            ticketUserEntity.UserID = Originalentity.CreatedBy;
                            ticketAPP.AddTicketUser(ticketUserEntity);
                        }

                        #endregion

                        #region history
                        TicketHistorysEntity historEntity = new TicketHistorysEntity();
                        historEntity.ModifiedBy = IdentityContext.UserID;
                        historEntity.ModifiedOn = DateTime.Now.Date;
                        historEntity.TicketID = Originalentity.TicketID;
                        historEntity.Description = entity.FullDescription = Originalentity.TicketCode + ":" + Originalentity.FullDescription +
                                           string.Format(Environment.NewLine +
                                           "==================================" +
                                           Environment.NewLine +
                                           "Convert Reason:{0}" +
                                           Environment.NewLine +
                                           "Convert By:{1}"
                                           , desc.TrimStart()
                                           , userApp.GetLastNameFirstName(IdentityContext.UserID)
                                           );
                        ticketAPP.AddTicketHistory(historEntity);
                        #endregion

                        #region add relation

                        TicketsRelationDTO dtoEntity = new TicketsRelationDTO();

                        AddTicketsRelationRequest request = new AddTicketsRelationRequest();

                        dtoEntity.RTID = Convert.ToInt32(result);

                        dtoEntity.TID = Convert.ToInt32(tid);

                        dtoEntity.CreatedBy = Originalentity.CreatedBy;

                        request.dto = dtoEntity;

                        trApp.AddTR(request);

                        #endregion

                        #region update

                        Originalentity.Status = TicketsState.Cancelled;
                        Originalentity.ConvertDelete = CovertDeleteState.ForeverDelete;
                        Originalentity.ModifiedOn = DateTime.Now;

                        Originalentity.ModifiedBy = IdentityContext.UserID;
                        Update = ticketAPP.UpdateTickets(Originalentity);

                        #endregion
                    }
                }
                if (Update)
                {
                    context.Response.Write("Update Status Success!");
                }
                else
                {
                    context.Response.Write("Update Status Fail!");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoConverReasonHandler.ashx Messages:\r\n{0}", ex));
                return;
            }


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
