using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.FileModel;
using System.Text;

namespace SunNet.PMNew.Web
{
    public partial class AddTicket : BaseAscx
    {
        #region declare

        /// <summary>
        /// 设置默认都是打开的
        /// </summary>
        public bool hideBaseInfo
        {
            set
            {
                this.hdhideDev.Value = value.ToString();
            }
        }
        TicketsApplication ticketAPP = new TicketsApplication();
        FileApplication fileApp = new FileApplication();
        FeedBackMessagesApplication fbmApp = new FeedBackMessagesApplication();
        ProjectApplication proApp = new ProjectApplication();
        ProjectApplication projectApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetUploadValue();

            EnbaleWhenNoAccess();

            IsInternal();

            if (!IsPostBack)
            {
                #region set default value - Priority
                this.radioP2.Checked = true;
                hideBaseInfo = false;//for hide ticket base info
                #endregion
                ProjectTitleBind();
                DateJsBind();
                ShowAssginUsers(TicketsEntityInfo);
                if (TicketsEntityInfo != null)
                {
                    #region hide when in detail page

                    if (TicketsEntityInfo.Status > TicketsState.Submitted &&
                        TicketsEntityInfo.Status != TicketsState.Cancelled &&
                        QS("is0hsisdse") != "54156")
                    {
                        this.lilhideDev.Text = "<a style='cursor:pointer;' onclick='hideDiv(true);return false'><span id='VisibleText' style='font-size: 14px; color: green;'>--</span></a>";
                        hideBaseInfo = false;
                    }

                    #endregion
                    BindDataModel(TicketsEntityInfo);
                    IsEnableAttr();
                    ReadingFeedBackMessage(UserInfo.UserID, TicketsEntityInfo.TicketID);
                }
            }
        }

        private void ShowAssginUsers(TicketsEntity TicketsEntityInfo)
        {
            if (IsSunnet == "true" && TicketsEntityInfo == null)
            {
                this.trAssignUser.Visible = true;
            }
        }
        private void DateJsBind()
        {
            devDateJs.Attributes.Add("onclick",
                string.Format(@"javascript:popUpCalendar(document.getElementById('{0}'),document.getElementById('{1}'), 'mm/dd/yyyy', 0, 0); ",
                                     txtDeliveryDate.ClientID,
                                     txtDeliveryDate.ClientID));
            startDateJs.Attributes.Add("onclick",
                string.Format(@"javascript:popUpCalendar(document.getElementById('{0}'),document.getElementById('{1}'), 'mm/dd/yyyy', 0, 0); ",
                                     txtStartDate.ClientID,
                                     txtStartDate.ClientID));
        }

        private void ReadingFeedBackMessage(int userId, int ticketId)
        {
            fbmApp.ReadingFeedBackMessage(userId, ticketId);
        }

        private void IsInternal()
        {
            if (IsSunnet == "true")
            {
                this.trStartDate.Visible = true;
            }
        }

        #region enable controle when no access

        private void EnbaleWhenNoAccess()
        {
            if (IsSunnet == "true")
            {

            }
            else if (!(UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.CLIENT || UserInfo.Role == RolesEnum.ADMIN))
            {
                this.ckbEN.Attributes["disabled"] = "disabled";
            }
        }

        #endregion

        #region set upload value

        private void SetUploadValue()
        {
            UploadFile1.ProjectName = ddlProject.ClientID;

            this.UploadFile1.PrimaryKey = this.ID;
        }

        #endregion

        #region enable all control,when IsEnbale is true

        private void IsEnableAttr()
        {
            if (IsEnable)
            {
                this.txtDesc.Attributes["readonly"] = "readonly";
                this.txtTitle.Attributes["disabled"] = "true";
                this.txtUrl.Attributes["disabled"] = "true";
                this.ddlTicketType.Attributes["disabled"] = "true";
                this.radioP1.Attributes["disabled"] = "true";
                this.radioP2.Attributes["disabled"] = "true";
                this.radioP3.Attributes["disabled"] = "true";
                this.radioP4.Attributes["disabled"] = "true";
                this.ddlProject.Enabled = false;
                this.UploadFile1.EnableUpload = true;
                this.UploadFile1.Visible = false;
                this.ckbEN.Attributes["disabled"] = "true";
                this.txtStartDate.Enabled = false;
                this.txtDeliveryDate.Enabled = false;
                this.devDateJs.Visible = false;
                this.startDateJs.Visible = false;
            }
        }

        #endregion

        #region data bind

        public TicketsEntity TicketsEntityInfo { get; set; }

        private void BindDataModel(TicketsEntity info)
        {
            this.TicketID = info.TicketID;
            this.ProjectId = info.ProjectID.ToString();
            this.Url = info.URL;
            this.Descr = Server.HtmlDecode(info.FullDescription);
            this.Title = Server.HtmlDecode(info.Title);
            this.BugType = info.TicketType.GetHashCode();
            this.CheckEn = info.IsEstimates;
            this.Priority = info.Priority.GetHashCode();
            this.startDate = info.StartDate <= UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() ? "" : info.StartDate.ToString("MM/dd/yyyy");
            this.DeliveryDate = info.DeliveryDate <= UtilFactory.Helpers.CommonHelper.GetDefaultMinDate() ? "" : info.DeliveryDate.ToString("MM/dd/yyyy");
            this.lilImageList = fileList(info.TicketID, info.Status);
            if ((UserInfo.Role == RolesEnum.Leader || UserInfo.Role == RolesEnum.PM))
            {
                selectStatus.Visible = true;
                if (info.Status != TicketsState.Draft && info.Status != TicketsState.Submitted && info.Status != TicketsState.Wait_PM_Feedback)
                {
                    selectStatus.Items.FindByValue(((int)info.Status).ToString()).Selected = true;
                }
                btnChangeInternalStatus.Visible = true;
            }
            else
            {
                selectStatus.Visible = false;
                btnChangeInternalStatus.Visible = false;
            }
        }

        private string fileList(int ticketId, TicketsState status)
        {
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = fileApp.GetFileListBySourceId(ticketId, FileSourceType.Ticket);
            if (status == TicketsState.Draft)
            {
                foreach (FilesEntity filesEntity in list)
                {
                    sb.AppendFormat("<li  class=\"file\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\">{3}{4} (Size:{2})</a><a onclick='deleteImgWhenStatusDraft({1})'><img src=\"/Scripts/Upload/images/error_fuck.png\" width='12' height='12' border=\"0\" /></a></li>"
                       , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType);
                }
            }
            else
            {
                foreach (FilesEntity filesEntity in list)
                {
                    sb.AppendFormat("<li  class=\"file\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\">{3}{4} (Size:{2})</a></li>"
                       , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType);
                }
            }

            return string.Format("<ul id='demo-list'>{0}</ul>", sb);
        }

        private void ProjectTitleBind()
        {
            List<ProjectDetailDTO> list = proApp.GetUserProjects(UserInfo);
            this.ddlProject.DataTextField = "Title";
            this.ddlProject.DataValueField = "ProjectID";
            if (list.Count == 1)
            {
                BindTicketUsers(list[0].ProjectID);
            }
            ddlProject.DataSource = list;
            ddlProject.DataBind();
            if (list.Count != 1)
                ddlProject.Items.Insert(0, new ListItem("Please select...", ""));
        }

        #endregion

        #region public attribute

        public string IsSunnet
        {
            get { return this.hdIsSunnet.Value; }
            set { this.hdIsSunnet.Value = value; }
        }

        public bool ProjectEnabled
        {
            set
            {
                this.ddlProject.Enabled = value;
            }
        }

        public bool TicketTypeEnabled
        {
            set
            {
                this.ddlTicketType.Attributes["disabled"] = value.ToString().ToLower();
            }
        }

        public bool IsEnable { get; set; }

        public int TicketID { get; set; }

        public string ProjectId
        {
            set { this.ddlProject.SelectedValue = value; }
            get { return this.ddlProject.SelectedValue; }
        }

        public int BugType
        {
            set { this.ddlTicketType.Value = value.ToString(); }
            get { return Convert.ToInt32(this.ddlTicketType.Value); }
        }

        public string Msg
        {
            set { this.MsgInfo.Text = value; }
        }

        public int Priority
        {
            set
            {
                SetRadioValue(value);
            }
            get
            {
                return ReturnRadioValueBySel();
            }
        }

        private int ReturnRadioValueBySel()
        {
            int value = 0;
            if (this.radioP1.Checked)
            {
                value = 1;
            }
            else if (this.radioP2.Checked)
            {
                value = 2;
            }
            else if (this.radioP3.Checked)
            {
                value = 3;

            }
            else if (this.radioP4.Checked)
            {
                value = 4;
            }
            else
            {
                value = 0;
            }
            return value;
        }

        private void SetRadioValue(int value)
        {
            switch (value)
            {
                case 1:
                    this.radioP1.Checked = true;
                    break;
                case 2:
                    this.radioP2.Checked = true;
                    break;
                case 3:
                    this.radioP3.Checked = true;
                    break;
                case 4:
                    this.radioP4.Checked = true;
                    break;
                default:
                    this.radioP1.Checked = true;
                    break;
            }
        }

        public bool CheckEn
        {
            set { this.ckbEN.Checked = value; }
            get { return this.ckbEN.Checked; }
        }

        public string Title
        {
            set { this.txtTitle.Value = value; }
            get { return this.txtTitle.Value; }
        }

        public string Url
        {
            set { this.txtUrl.Value = value; }
            get { return this.txtUrl.Value; }
        }

        public string Descr
        {
            set { this.txtDesc.Value = value; }
            get { return this.txtDesc.Value; }
        }

        public string startDate
        {
            set { this.txtStartDate.Text = value; }
            get { return this.txtStartDate.Text; }
        }

        public string DeliveryDate
        {
            set { this.txtDeliveryDate.Text = value; }
            get { return this.txtDeliveryDate.Text; }
        }

        public string lilImageList
        {
            get { return this.lilImgList.Text; }
            set { this.lilImgList.Text = value; }
        }

        public string File
        {
            get { return this.UploadFile1.FileList; }
        }
        public string DeleteFile
        {
            get { return this.UploadFile1.DeleteFileList; }
        }
        #endregion

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlProject.SelectedIndex > 0)
            {
                int projectID = 0;
                int.TryParse(this.ddlProject.SelectedValue, out projectID);
                BindTicketUsers(projectID);
            }
        }

        protected void BindTicketUsers(int projectID)
        {
            List<ProjectUsersEntity> listProjectUser = projectApp.GetProjectSunnetUserList(projectID);
            StringBuilder sb = new StringBuilder();
            foreach (ProjectUsersEntity tmpU in listProjectUser)
            {
                UsersEntity user = userApp.GetUser(tmpU.UserID);
                if (user != null && user.ID != UserInfo.ID && user.Role != RolesEnum.Supervisor)
                {
                    if (user.Status.Trim() != "INACTIVE")
                    {
                        sb.AppendFormat("<option id={0}-{3}>{1}, {2}</option>", user.UserID, user.LastName, user.FirstName, user.RoleID);
                    }
                }
            }
            this.lilUserList.Text = sb.ToString();
        }
    }
}