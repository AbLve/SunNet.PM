using System;
using System.IO;
using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ShareModel;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.SunnetTicket.Knowledge
{
    public partial class New : BasePage
    {
        private readonly ShareApplication _shareApp = new ShareApplication();
        private readonly FileApplication _fileApp = new FileApplication();
        private TicketsApplication _ticketApp = new TicketsApplication();
        protected TicketsEntity Ticket { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Pop)this.Master;
            if (master != null) master.Width = 580;
            if (!IsPostBack)
            {
                txtType.Attributes.Add("data-hidden", "#" + hidType.ClientID);
                int ticket = QS("ticket", 0);
                Ticket = _ticketApp.GetTickets(ticket);
                if (ticket <= 0)
                {
                    Response.Redirect(EmptyPopPageUrl);
                }
                ltlTicket.Text = ticket.ToString();
            }
        }

        private ShareEntity GetEntity()
        {
            var entity = new ShareEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            entity.Note = txtDescription.Text;
            int type = 0;
            int.TryParse(hidType.Value, out type);
            entity.Type = type;
            entity.TicketID = QS("ticket", 0);
            entity.Title = string.Empty;
            entity.TypeEntity.Title = txtType.Text;
            return entity;
        }
        private int InsertFile(string file, int shareID)
        {
            var realFileName = file.Replace(string.Format("{0}_", UserInfo.ID), "");

            string tempPath = Config.UploadPath;
            string tmpFile = Server.MapPath(Config.UploadPath + "/" + file);
            if (!File.Exists(tmpFile))
                return 0;

            string folderName = UserInfo.CompanyID.ToString();
            string savePath = Path.Combine(Server.MapPath(tempPath), folderName);
            string sExtension = Path.GetExtension(file);
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            string sNewFileName = string.Format("{0}_{1}{2}", shareID, DateTime.Now.ToString("yyMMddssmm"), sExtension); ;


            FilesEntity fileEntity = new FilesEntity();
            fileEntity.ContentType = sExtension.ToLower();
            fileEntity.CreatedBy = UserInfo.UserID;
            fileEntity.FilePath = string.Format("{0}{1}/{2}", tempPath.Substring(1), folderName, sNewFileName);
            fileEntity.FileSize = Convert.ToDecimal(new FileInfo(tmpFile).Length);
            fileEntity.FileTitle = realFileName.Substring(0, realFileName.LastIndexOf('.'));
            fileEntity.IsPublic = true;
            fileEntity.FeedbackId = 0;
            fileEntity.TicketId = 0;
            fileEntity.ProjectId = 0;
            fileEntity.SourceID = shareID;
            fileEntity.SourceType = (int)FileSourceType.KnowledgeShare;
            fileEntity.ThumbPath = "";
            fileEntity.CreatedOn = DateTime.Now.Date;
            fileEntity.CompanyID = UserInfo.CompanyID;

            if (FileHelper.Move(file, Server.MapPath(fileEntity.FilePath)))
            {
                return _fileApp.AddFile(fileEntity);
            }
            return 0;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var addShareEntity = GetEntity();
            if (_shareApp.Save(addShareEntity))
            {
                if (!string.IsNullOrEmpty(hidUploadFile.Value))
                {
                    InsertFile(hidUploadFile.Value, addShareEntity.ID);
                }
                Redirect(EmptyPopPageUrl, false, true);
            }
        }
    }
}