using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using System.IO;
using System.Text.RegularExpressions;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core;
using System.Drawing;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils.Helpers;
namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class AddTicketList : BaseWebsitePage
    {
        #region declare

        TicketsApplication ticketAPP = new TicketsApplication();
        TicketsEntity entity = null;
        List<TicketsEntity> list = null;
        int result = 0;
        ProjectApplication projectApp = new ProjectApplication();
        bool HasFileMsG = true;
        FileApplication fileApp = new FileApplication();
        List<string> stringErrorMsg = new List<string>();
        string tempPath = "";
        string FolderName = "";
        FilesEntity fileEntity = null;

        #region declare attribute

        string pId = string.Empty;
        string title = string.Empty;
        string Descr = string.Empty;
        string url = string.Empty;
        int tType = 0;
        bool ckbEn = false;
        string imageList = string.Empty;
        int pty = 0;
        string bType = string.Empty;

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.AddTicket1.Msg = "NO 1";
                this.AddTicket2.Msg = "NO 2";
                this.AddTicket3.Msg = "NO 3";
            }
        }

        private string AnalysisString(string image, int type)
        {
            string NewString = "";
            string[] listImgCounts = image.Split('#');
            Regex regex = new Regex(@"^[a-zA-z0-9\s]+[-,+]*[a-zA-z0-9\s]*\.+[a-zA-z]+$");

            foreach (string item in listImgCounts)
            {
                string[] ImgeDetail = item.Split('|');
                foreach (string detail in ImgeDetail)
                {
                    if (type == 1)
                    {
                        if (regex.Match(detail).Success && detail.Length > 2)
                        {
                            NewString += detail + ",";
                        }
                    }
                    else
                    {
                        if (detail.Contains("KB") && detail.Length > 2)
                        {
                            NewString += detail + ",";
                        }
                    }
                }
            }

            return NewString;
        }

        private void AddFile(int tid, int pId, int i)
        {
            HttpContext context = HttpContext.Current;

            string image = "";
            string DeleteImg = "";

            if (i == 0)
            {
                image = this.AddTicket1.File;
                DeleteImg = this.AddTicket1.DeleteFile;
            }
            else if (i == 1)
            {
                image = this.AddTicket2.File;
                DeleteImg = this.AddTicket2.DeleteFile;
            }
            else
            {
                image = this.AddTicket3.File;
                DeleteImg = this.AddTicket3.DeleteFile;
            }
            string imageList = AnalysisString(image, 1);
            string imageSizeList = AnalysisString(image, 2);
            string deleteImglist = AnalysisString(DeleteImg, 1);

            ProjectsEntity pentity = projectApp.Get(Convert.ToInt32(pId));



            if (null != pentity)
            {
                FolderName = pentity.ProjectID.ToString();
            }

            string sNewFileName = "";

            tempPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];

            string[] listStringName = imageList.Split(',');
            string[] listStringSize = imageSizeList.Split(',');
            string[] listdelImage = deleteImglist.Split(',');

            foreach (string Name in listStringName)
            {
                fileEntity = new FilesEntity();
                string sExtension = Path.GetExtension(Name).Replace(".", "").Trim();
                foreach (string Size in listStringSize)
                {
                    if (listdelImage.Contains(Name)) continue;
                    sNewFileName = FolderName + Name;
                    fileEntity.ContentType = "." + sExtension.ToLower();
                    fileEntity.CreatedBy = UserInfo.UserID;
                    fileEntity.FilePath = tempPath.Substring(2) + FolderName + @"/" + sNewFileName;
                    fileEntity.FileSize = Convert.ToDecimal(Size.ToLower().Replace("kb", ""));
                    fileEntity.FileTitle = Name.Substring(0, Name.LastIndexOf('.'));
                    fileEntity.IsPublic = true;
                    fileEntity.TicketId = tid;//ticketID
                    fileEntity.ProjectId = pId;
                    fileEntity.FeedbackId = 0;
                    fileEntity.SourceType = (int)FileSourceType.Ticket;
                    fileEntity.ThumbPath = context.Server.MapPath(tempPath) + FolderName + sNewFileName; ;//
                    fileEntity.CreatedOn = DateTime.Now.Date;
                    fileEntity.CompanyID = IdentityContext.CompanyID;
                    int responseResult = fileApp.AddFile(fileEntity);
                    if (responseResult <= 0)
                    {
                        HasFileMsG = false;
                        stringErrorMsg.Add(fileEntity.FileTitle);
                    }
                    break;
                }
            }
        }

        private bool BaseValidate(string pid, string desc, string title, int bType, int Priority)
        {
            bool IsPass = true;

            if (pid.Length == 0)
            {
                IsPass = false;
                ShowMessageToClient("Please select a project.", 0, false, false);
            }
            else if (desc.Length == 0)
            {
                IsPass = false;
                ShowMessageToClient("Please Input description.", 0, false, false);
            }
            else if (title.Length == 0)
            {
                ShowMessageToClient("Please enter the title.", 0, false, false);
                IsPass = false;
            }
            else if (bType < 0)
            {
                ShowMessageToClient("Please select ticket type.", 0, false, false);
                IsPass = false;
            }
            else if (Priority <= 0)
            {
                ShowMessageToClient("Please select priority.", 0, false, false);
                IsPass = false;
            }

            return IsPass;
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.opener=null;window.close();</script>");
        }

        //submit no 3
        protected void btnSub_Click(object sender, EventArgs e)
        {

            #region set value

            pId = AddTicket3.ProjectId;
            Descr = AddTicket3.Descr.NoHTML();
            title = AddTicket3.Title.NoHTML();
            tType = AddTicket3.BugType;
            url = AddTicket3.Url;
            ckbEn = AddTicket3.CheckEn;
            pty = AddTicket3.Priority;

            #endregion

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.btnSub.Enabled = false;
                this.btnSave.Enabled = false;

                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID;
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Draft;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);

                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 2);
                    this.AddTicket3.Msg = "No 3, Ticket Add Success!";
                    //ShowMessageToClient("No 3, Ticket Add Success!", 0, false, false);
                    this.btnSave.CssClass += " button_enabled";
                    this.btnSub.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket3.Msg = "No 3, Ticket Add Fail!";
                    //ShowMessageToClient("No 3, Ticket Add Fail!", 0, false, false);
                    this.btnSub.Enabled = true;
                    this.btnSave.Enabled = true;
                }
            }
        }

        //draft no 3
        protected void btnSave_Click(object sender, EventArgs e)
        {

            #region set value

            pId = AddTicket3.ProjectId;
            Descr = AddTicket3.Descr.NoHTML();
            title = AddTicket3.Title.NoHTML();
            tType = AddTicket3.BugType;
            url = AddTicket3.Url;
            ckbEn = AddTicket3.CheckEn;
            pty = AddTicket3.Priority;

            #endregion

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.btnSub.Enabled = false;
                this.btnSave.Enabled = false;
                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID;
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Draft;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);
                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 2);
                    this.AddTicket3.Msg = "No 3, Ticket Add Success!";
                    //ShowMessageToClient("No 3, Ticket Add Success!", 0, false, false);
                    this.btnSave.CssClass += " button_enabled";
                    this.btnSub.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket3.Msg = "No 3, Ticket Add Success!";
                    //ShowMessageToClient("No 3, Ticket Add Fail!", 0, false, false);
                    this.btnSub.Enabled = true;
                    this.btnSave.Enabled = true;
                }
            }
        }

        //draft no 1
        protected void Button1_Click(object sender, EventArgs e)
        {

            #region setValue

            pId = AddTicket1.ProjectId;
            Descr = AddTicket1.Descr.NoHTML();
            title = AddTicket1.Title.NoHTML();
            tType = AddTicket1.BugType;
            url = AddTicket1.Url;
            ckbEn = AddTicket1.CheckEn;
            pty = AddTicket1.Priority;

            #endregion

            #region

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.Button1.Enabled = false;
                this.Button2.Enabled = false;

                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID; //
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Draft;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);
                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 0);
                    this.AddTicket1.Msg = "No 1, Ticket Add Success!";
                    //ShowMessageToClient("No 1, Ticket Add Success!", 0, false, false);
                    this.Button1.CssClass += " button_enabled";
                    this.Button2.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket1.Msg = "No 1, Ticket Add Fail!";
                    //ShowMessageToClient("No 1, Ticket Add Fail!", 0, false, false);
                    this.Button1.Enabled = true;
                    this.Button2.Enabled = true;
                }
            }

            #endregion

        }

        //submit no 1
        protected void Button2_Click(object sender, EventArgs e)
        {
            #region setValue

            pId = AddTicket1.ProjectId;
            Descr = AddTicket1.Descr.NoHTML();
            title = AddTicket1.Title.NoHTML();
            tType = AddTicket1.BugType;
            url = AddTicket1.Url;
            ckbEn = AddTicket1.CheckEn;
            pty = AddTicket1.Priority;


            #endregion

            #region

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.Button1.Enabled = false;
                this.Button2.Enabled = false;
                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID; //
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Submitted;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);

                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 0);
                    this.AddTicket1.Msg = "No 1, Ticket Add Success!";
                    //ShowMessageToClient("No 1, Ticket Add Success!", 0, false, false);
                    this.Button1.CssClass += " button_enabled";
                    this.Button2.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket1.Msg = "No 1, Ticket Add Fail!";
                    //ShowMessageToClient("No 1, Ticket Add Fail!", 0, false, false);
                    this.Button1.Enabled = true;
                    this.Button2.Enabled = true;
                }
            }

            #endregion

        }
        //draft no 2
        protected void Button3_Click(object sender, EventArgs e)
        {

            #region set value

            pId = AddTicket2.ProjectId;
            Descr = AddTicket2.Descr.NoHTML();
            title = AddTicket2.Title.NoHTML();
            tType = AddTicket2.BugType;
            url = AddTicket2.Url;
            ckbEn = AddTicket2.CheckEn;
            pty = AddTicket2.Priority;

            #endregion

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.Button3.Enabled = false;
                this.Button4.Enabled = false;
                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID; //
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Draft;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);

                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 1);
                    this.AddTicket2.Msg = "No 2, Ticket Add Success!";
                    // ShowMessageToClient("No 2, Ticket Add Success!", 0, false, false);
                    this.Button3.CssClass += " button_enabled";
                    this.Button4.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket2.Msg = "No 2, Ticket Add Fail!";
                    // ShowMessageToClient("No 2, Ticket Add Fail!", 0, false, false);
                    this.Button3.Enabled = true;
                    this.Button4.Enabled = true;
                }
            }
        }
        //submit no 2
        protected void Button4_Click(object sender, EventArgs e)
        {

            #region set value

            pId = AddTicket2.ProjectId;
            Descr = AddTicket2.Descr.NoHTML();
            title = AddTicket2.Title.NoHTML();
            tType = AddTicket2.BugType;
            url = AddTicket2.Url;
            ckbEn = AddTicket2.CheckEn;
            pty = AddTicket2.Priority;

            #endregion

            if (BaseValidate(pId, Descr, title, tType, pty))
            {
                this.Button3.Enabled = false;
                this.Button4.Enabled = false;
                entity = new TicketsEntity();
                entity.ProjectID = Convert.ToInt32(pId);
                entity.CompanyID = UserInfo.CompanyID; //
                entity.Priority = (PriorityState)Convert.ToInt32(pty);
                entity.TicketType = (TicketsType)Convert.ToInt32(tType);
                entity.Title = title;
                entity.URL = url;
                entity.FullDescription = Descr;
                entity.CreatedBy = UserInfo.UserID;
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.Status = TicketsState.Submitted;
                entity.IsEstimates = ckbEn;
                entity.StartDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.DeliveryDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.TicketCode = Enum.GetName(typeof(TicketsType), Convert.ToInt32(tType)).Substring(0, 1);
                entity.IsInternal = false;
                entity.ModifiedBy = 0;
                entity.PublishDate = UtilFactory.Helpers.CommonHelper.GetDefaultMinDate();
                entity.ConvertDelete = CovertDeleteState.Normal;
                entity.Source = UserInfo.Role;
                result = ticketAPP.AddTickets(entity);

                if (result > 0)
                {
                    AddFile(result, entity.ProjectID, 1);
                    this.AddTicket2.Msg = "No 2, Ticket Add Success!";
                    //ShowMessageToClient("No 2, Ticket Add Success!", 0, false, false);
                    this.Button3.CssClass += " button_enabled";
                    this.Button4.CssClass += " button_enabled";
                    DoAddPmAndSendEmail(entity);
                }
                else
                {
                    this.AddTicket2.Msg = "No 2, Ticket Add Fail!";
                    //ShowMessageToClient("No 2, Ticket Add Fail!", 0, false, false);
                    this.Button3.Enabled = true;
                    this.Button4.Enabled = true;
                }
            }
        }

        public void DoAddPmAndSendEmail(TicketsEntity ticketEntity)
        {
            #region add pm
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

            #endregion

            #region send email

            if (!ticketEntity.IsInternal && ticketEntity.Status != TicketsState.Draft)
            {
                TicketStatusManagerApplication ex = new TicketStatusManagerApplication();
                ex.SendEmailToPMWhenTicketAdd(result, ticketEntity.TicketType);
            }

            #endregion
        }

    }
}
