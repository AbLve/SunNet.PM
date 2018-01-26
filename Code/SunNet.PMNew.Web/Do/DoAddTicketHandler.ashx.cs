using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;


namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoAddTicketHandler : IHttpHandler
    {
        #region declare

        TicketsApplication ticketAPP = new TicketsApplication();
        UserApplication userApp = new UserApplication();
        FileApplication fileApp = new FileApplication();
        ProjectApplication projectApp = new ProjectApplication();
        bool HasFileMsG = true;
        List<string> stringErrorMsg = new List<string>();
        string tempPath = "";
        string FolderName = "";
        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                if (IdentityContext.UserID <= 0)
                    return;
                int result = 0;
                #region getValue

                int pId = Convert.ToInt32(context.Request["pId"]);
                string tType = context.Request["tType"];
                string ckbEn = context.Request["ckbEn"];
                string pty = context.Request["pty"];
                string title = context.Request["title"];
                string url = context.Request["url"];
                string description = context.Request["descr"];
                string imageList = context.Request["imageList"];
                string imageSizeList = context.Request["imageSizeList"];
                string StartDate = context.Request["StartDate"];
                string DeliveryDate = context.Request["DeliveryDate"];
                string satus = context.Request["satus"];
                string IsSunnet = context.Request["isSunnet"];
                string userlist = context.Request["userlist"];

                #endregion

                UsersEntity entity = userApp.GetUser(IdentityContext.UserID);
                ProjectsEntity projectsEntity = projectApp.Get(pId);

                #region add ticket
                TicketsEntity ticketEntity = new TicketsEntity();
                pty = string.IsNullOrEmpty(pty) ? "1" : pty;
                ticketEntity.ProjectID = pId;
                ticketEntity.CompanyID = projectsEntity.CompanyID;
                ticketEntity.Priority = (PriorityState)Convert.ToInt32(pty);
                ticketEntity.TicketType = (TicketsType)Convert.ToInt32(tType);
                ticketEntity.Title = title.NoHTML();
                ticketEntity.URL = context.Server.UrlEncode(url);
                ticketEntity.FullDescription = description.NoHTML();
                ticketEntity.CreatedBy = IdentityContext.UserID;
                ticketEntity.CreatedOn = DateTime.Now;
                ticketEntity.ModifiedOn = DateTime.Now;

                ticketEntity.IsEstimates = ckbEn == "checked" ? true : false;
                ticketEntity.TicketCode = new TicketsApplication().ConvertTicketTypeToTicketCode(ticketEntity.TicketType);
                ticketEntity.IsInternal = IsSunnet == "true" ? true : false;
                ticketEntity.ModifiedBy = 0;
                ticketEntity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                ticketEntity.ConvertDelete = CovertDeleteState.Normal;
                ticketEntity.Source = entity.Role;
                if (IsSunnet == "true")
                {
                    ticketEntity.StartDate = !string.IsNullOrEmpty(StartDate.ToString()) ? DateTime.Parse(StartDate).Date : UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    ticketEntity.DeliveryDate = !string.IsNullOrEmpty(DeliveryDate.ToString()) ? DateTime.Parse(DeliveryDate).Date : UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    if (entity.Role == RolesEnum.Supervisor)
                    {
                        ticketEntity.Status = TicketsState.Submitted;
                    }
                    else
                    {
                        ticketEntity.Status = TicketsState.PM_Reviewed;
                    }
                }
                else
                {
                    ticketEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    ticketEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    ticketEntity.Status = satus == "0" ? TicketsState.Draft : TicketsState.Submitted;
                }

                result = ticketAPP.AddTickets(ticketEntity);

                if (result > 0)
                {
                    TicketUsersEntity ticketUserEntity = new TicketUsersEntity();
                    //add pm user
                    ticketUserEntity.Type = TicketUsersType.PM;
                    ticketUserEntity.TicketID = result;
                    ProjectsEntity projectEntity = projectApp.Get(ticketEntity.ProjectID);
                    if (projectEntity != null)
                    {
                        ticketUserEntity.UserID = projectEntity.PMID;
                        ticketAPP.AddTicketUser(ticketUserEntity);
                    }
                    else
                    {
                        WebLogAgent.Write(string.Format("Add Pm To Ticket User Error:Project :{0},Ticket:{1},CreateDate:{2}",
                            ticketEntity.ProjectID, ticketEntity.TicketID, DateTime.Now));
                    }
                    //add create user
                    ticketUserEntity.Type = TicketUsersType.Create;
                    ticketUserEntity.TicketID = result;
                    ticketUserEntity.UserID = ticketEntity.CreatedBy;
                    ticketAPP.AddTicketUser(ticketUserEntity);
                }
                #endregion

                #region send email
                TicketStatusManagerApplication ex = new TicketStatusManagerApplication();
                if (!ticketEntity.IsInternal)
                {
                    ex.SendEmailToPMWhenTicketAdd(result, ticketEntity.TicketType);
                }

                #endregion

                #region add file

                FilesEntity fileEntity = new FilesEntity();

                if (null != projectsEntity)
                {
                    FolderName = projectsEntity.ProjectID.ToString();
                }

                string sNewFileName = "";

                tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];

                string[] listStringName = imageList.Split(',');

                string[] listStringSize = imageSizeList.Split(',');


                foreach (string Name in listStringName)
                {
                    if (Name.Length == 0) break;
                    string sExtension = Path.GetExtension(Name).Replace(".", "").Trim();
                    foreach (string Size in listStringSize)
                    {
                        sNewFileName = FolderName + Name;
                        fileEntity.ContentType = "." + sExtension.ToLower();
                        fileEntity.CreatedBy = entity.UserID;
                        fileEntity.FilePath = tempPath.Substring(2) + FolderName + @"/" + sNewFileName;
                        fileEntity.FileSize = Convert.ToDecimal(Size.ToLower().Replace("kb", ""));
                        fileEntity.FileTitle = Name.Substring(0, Name.LastIndexOf('.'));
                        fileEntity.IsPublic = !ticketEntity.IsInternal;
                        fileEntity.ProjectId = pId;
                        fileEntity.TicketId = result;
                        fileEntity.CreatedOn = DateTime.Now.Date;
                        fileEntity.FeedbackId = 0;
                        fileEntity.SourceType = (int)FileSourceType.Ticket;
                        fileEntity.ThumbPath = context.Server.MapPath(tempPath) + FolderName + sNewFileName; ;//
                        fileEntity.CompanyID = IdentityContext.CompanyID;
                        int response = fileApp.AddFile(fileEntity);
                        if (response <= 0)
                        {
                            HasFileMsG = false;
                            stringErrorMsg.Add(fileEntity.FileTitle);
                        }
                        break;
                    }
                }

                #endregion

                #region response msg

                if (result > 0)
                {
                    if (HasFileMsG)
                    {
                        context.Response.Write("The ticket has been added.");
                    }
                    else
                    {
                        string error = "";
                        foreach (string item in stringErrorMsg)
                        {
                            error += item + "File Upload Failed!";
                        }
                        context.Response.Write(error);
                    }

                }
                else
                {
                    context.Response.Write("Add Fail!");
                }

                #endregion

                #region assign user and send email
                TicketUsersEntity tuEntity = new TicketUsersEntity(); ;
                string[] userWithRoleList = userlist.TrimEnd(',').Split(',');
                int assignResult = 0;
                if (userWithRoleList.Length > 0)
                {
                    foreach (string item in userWithRoleList)
                    {
                        if (item.Length > 0)
                        {
                            string[] userWithRole = item.Split('-');
                            if (userWithRole.Length > 0)
                            {
                                tuEntity.TicketID = result;
                                tuEntity.UserID = Convert.ToInt32(userWithRole[0]);
                                tuEntity.Type = GetUserTypeByRoleID(userWithRole[1]);  //Convert.ToInt32(userWithRole[0]);
                                assignResult = ticketAPP.AddTicketUser(tuEntity);
                                if (assignResult > 0)
                                {
                                    ex.SendEmailToAssignedUser(tuEntity);
                                }
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                context.Response.Write("Input special symbol is not allowed,please check title and description!");
                WebLogAgent.Write(string.Format("Error Ashx:DoAddTicketHandler.ashx Messages:\r\n{0}", ex));
                return;
            }
        }

        /// <summary>
        /// Adapter from RoleEnum to TicketUserType
        /// </summary>
        /// <param name="role">current user role</param>
        /// <returns></returns>
        private TicketUsersType GetUserTypeByRoleID(string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                int roleID = Convert.ToInt32(role);
                if ((int)RolesEnum.QA == roleID)
                {
                    return TicketUsersType.QA;
                }
                else if ((int)RolesEnum.DEV == roleID || (int)RolesEnum.Contactor == roleID)
                {
                    return TicketUsersType.Dev;
                }
                else
                {
                    return TicketUsersType.Other;
                }
            }
            return TicketUsersType.Other;
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
