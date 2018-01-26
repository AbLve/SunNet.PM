using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ProjectModel;
using System.IO;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>

    public class DoEditTicketHandler : IHttpHandler
    {
        #region declare

        TicketsApplication TicketApp = new TicketsApplication();

        FileApplication fileApp = new FileApplication();

        ProjectApplication projectApp = new ProjectApplication();

        string originalTitle = string.Empty;

        string originalDesc = string.Empty;

        string originalState = string.Empty;

        bool HasFileMsG = true;

        List<string> stringErrorMsg = new List<string>();

        string tempPath = "";

        string FolderName = "";

        FilesDTO fileDto = null;

        bool IsSend = false;

        #endregion

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                #region get value

                int tid = Convert.ToInt32(context.Request["tid"]);
                int pId = Convert.ToInt32(context.Request["pId"]);
                String tType = context.Request["tType"];
                String ckbEn = context.Request["ckbEn"];
                String pty = context.Request["pty"];
                String title = context.Request["title"].NoHTML();
                String url =context.Server.UrlEncode(context.Request["url"]);
                String descr = context.Request["descr"].NoHTML();
                String imageList = context.Request["imageList"];
                String imageSizeList = context.Request["imageSizeList"];
                String StartDate = context.Request["StartDate"];
                String DeliveryDate = context.Request["DeliveryDate"];
                String satus = context.Request["satus"];
                String IsSunnet = context.Request["isSunnet"];

                #endregion

                bool update = true;

                TicketsEntity ticketEntity = TicketApp.GetTickets(tid);//get original model

                #region set original value
                {

                    originalDesc = ticketEntity.FullDescription;

                    originalState = ticketEntity.Status.ToString();

                    originalTitle = ticketEntity.Title;
                }
                #endregion

                #region set satus value

                switch (satus)
                {
                    case "save":
                        ticketEntity.Status = ticketEntity.Status;
                        break;
                    case "cancle":
                        ticketEntity.Status = TicketsState.Cancelled;
                        break;
                    case "submit":
                        IsSend = true;
                        ticketEntity.Status = TicketsState.Submitted;
                        break;
                }

                #endregion

                #region ticket

                ticketEntity.ProjectID = pId;
                ticketEntity.URL = url;
                ticketEntity.FullDescription = descr;
                ticketEntity.Title = title.NoHTML();
                ticketEntity.TicketType = tType == "0" ? TicketsType.Bug : TicketsType.Request;
                ticketEntity.IsEstimates = ckbEn == "checked" ? true : false;
                ticketEntity.Priority = (PriorityState)Convert.ToInt32(pty);
                ticketEntity.ModifiedBy = IdentityContext.UserID;

                if (IsSunnet == "true")
                {
                    ticketEntity.StartDate = !string.IsNullOrEmpty(StartDate.ToString()) ? DateTime.Parse(StartDate).Date : UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    ticketEntity.DeliveryDate = !string.IsNullOrEmpty(DeliveryDate.ToString()) ? DateTime.Parse(DeliveryDate).Date : UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                }
                else
                {
                    ticketEntity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                    ticketEntity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                }

                #region record,when title or descr or status changed

                if (originalTitle != title || originalDesc != descr)
                {
                    ticketEntity.ModifiedOn = DateTime.Now;
                }

                if ((TicketsState)Enum.Parse(typeof(TicketsState), originalState) != ticketEntity.Status)
                {
                    ticketEntity.PublishDate = DateTime.Now;
                }

                #endregion

                #region record history, when descr changed

                if (originalDesc != descr && ticketEntity.Status != TicketsState.Draft)
                {
                    TicketHistorysEntity history = new TicketHistorysEntity();
                    history.TicketID = ticketEntity.TicketID;
                    history.ModifiedBy = IdentityContext.UserID;
                    history.ModifiedOn = DateTime.Now;
                    history.Description = originalDesc;

                    TicketApp.AddTicketHistory(history);
                }

                #endregion

                update = TicketApp.UpdateTickets(ticketEntity);

                #region send email
                if (IsSend)
                {
                    TicketStatusManagerApplication EX = new TicketStatusManagerApplication();
                    EX.SendEmailToQaAndDevWhenStatusChanged(ticketEntity);
                }
                #endregion

                #endregion

                #region file

                FilesEntity fileEntity = new FilesEntity();

                FolderName = ticketEntity.ProjectID.ToString();

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
                        fileEntity.CreatedBy = IdentityContext.UserID; ;
                        fileEntity.FilePath = tempPath.Substring(2) + FolderName + @"/" + sNewFileName;
                        fileEntity.FileSize = Convert.ToDecimal(Size.ToLower().Replace("kb", ""));
                        fileEntity.FileTitle = Name.Substring(0, Name.LastIndexOf('.'));
                        fileEntity.IsPublic = true;
                        fileEntity.TicketId = tid;//ticketID
                        fileEntity.ProjectId = pId;//ticketID
                        fileEntity.SourceType = (int)FileSourceType.Ticket;
                        fileEntity.ThumbPath = context.Server.MapPath(tempPath) + FolderName + sNewFileName; ;//
                        fileEntity.CreatedOn = DateTime.Now;
                        int responseFile = fileApp.AddFile(fileEntity);
                        if (responseFile <= 0)
                        {
                            HasFileMsG = false;
                            stringErrorMsg.Add(fileEntity.FileTitle);
                        }
                        break;
                    }
                }
                #endregion

                #region response
                if (update)
                {
                    if (HasFileMsG)
                    {
                        context.Response.Write("Update Ticket Success!");
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
                    context.Response.Write("Update Fail!");
                }
                #endregion

            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoEditTicketHandler.ashx Messages:\r\n{0}", ex));
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
