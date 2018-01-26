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
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.FileModel;
using System.Text;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class AddFeedBack : BaseAscx
    {
        FileApplication fileApp = new FileApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            #region validate
            if (FeedBacksEntityInfo == null) return;
            #endregion

            Initial();

        }


        #region Initial

        private void Initial()
        {
            SetUploadValue();//set upload value
            hd_Project.Value = ProjectID.ToString();
            TicketID = FeedBacksEntityInfo.TicketID;
            if (IsReply)
            {
                trOthers.Visible = false;
                trOriDesc.Visible = true;
                trOriFile.Visible = true;
                trOriDate.Visible = true;
                lblDate.InnerText = FeedBacksEntityInfo.CreatedOn.ToString("MM/dd/yyyy");
                txtOriginalContent.Attributes["readonly"] = "readonly";
                this.txtTitle.Value = FeedBacksEntityInfo.Title.Trim().NoHTML();
                this.txtOriginalContent.Value = FeedBacksEntityInfo.Description;
                this.lblFiles.InnerHtml = ShowImageList(FeedBacksEntityInfo.FeedBackID);

                hfPublic.Value = FeedBacksEntityInfo.IsPublic.ToString();
                if (FeedBacksEntityInfo.WaitClientFeedback == 1) ClientReplyFeedbackID = FeedBacksEntityInfo.FeedBackID;
                if (FeedBacksEntityInfo.WaitPMFeedback == 1) PMReplyFeedbackID = FeedBacksEntityInfo.FeedBackID;
            }
            else
            {
                if (UserInfo.Role == RolesEnum.CLIENT)
                {
                    trOthers.Visible = false;
                    hfPublic.Value = "true";
                }
                else if (UserInfo.Role == RolesEnum.Supervisor)
                {
                    trOthers.Visible = false;
                    this.ckIsPublic.Visible = false;
                    this.chkIsWaitClientFeedback.Visible = false;
                    this.chkIsWaitPMFeedback.Visible = false;
                }
                else
                {
                    trOthers.Visible = true;
                    if (UserInfo.Role == RolesEnum.QA || UserInfo.Role == RolesEnum.DEV
                        || UserInfo.Role == RolesEnum.Leader || UserInfo.Role == RolesEnum.Contactor)
                    {
                        this.ckIsPublic.Visible = false;
                        this.chkIsWaitClientFeedback.Visible = false;
                        this.chkIsWaitPMFeedback.Visible = true;
                    }
                    else
                    {
                        this.ckIsPublic.Visible = true;
                        this.ckIsPublic.Checked = FeedBacksEntityInfo.IsPublic;
                        this.chkIsWaitClientFeedback.Visible = true;
                        this.chkIsWaitPMFeedback.Visible = false;
                    }
                }
            }
        }

        #endregion

        #region public attribute

        public FeedBacksEntity FeedBacksEntityInfo { get; set; }
        protected int TicketID;
        public int ProjectID { get; set; }
        public bool IsReply { get; set; }
        protected int ClientReplyFeedbackID;
        protected int PMReplyFeedbackID;

        #endregion

        #region set upload value

        public void SetUploadValue()
        {
            UploadFile1.ProjectName = this.hd_Project.ClientID; // ddlProject.ClientID;
            this.UploadFile1.PrimaryKey = this.ID;
        }

        #endregion

        public string ShowImageList(int feedbackId)
        {
            if (feedbackId < 1) return "";
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = fileApp.GetFileListBySourceId(feedbackId, FileSourceType.FeedBack);
            foreach (FilesEntity filesEntity in list)
            {
                sb.AppendFormat("<span><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"iframeDownloadFile\");\">{3}</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> "
                    , Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle);
            }
            return sb.ToString();
        }
    }
}