using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.UserControls
{
    public partial class FileUploader : BaseAscx
    {
        public int TicketID { get; set; }

        /// <summary>
        /// 如果是View则不显示删除按钮
        /// 如果是Edit则要显示删除按钮
        /// </summary>
        public string UploadType { get { return uploadType; } set { uploadType = value; } }
        private string uploadType = "View";

        public string TabIndex { get; set; }

        public int FileUploadCount { get { return fileUploadCount; } set { fileUploadCount = value; } }
        private int fileUploadCount = 0;

        private string templateHTML = string.Empty;
        private const string viewTemplateHTML = "<li ><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"_blank\">{3}{4}</a></li>";
        private const string editTemplateHTML = "<li  class=\"file\"><a href='{0}/do/DoDownloadFileHandler.ashx?FileID={1}&size={2}' target=\"_blank\">"
            + "{3}{4}</a><a onclick='deleteImgWhenStatusDraft({1})'><img alt='Delete' title='Delete' src=\"/images/icons/delete.png\" style=\"margin-left: 7px; vertical-align: middle; cursor:pointer;\"  width='12' height='12' border=\"0\" /></a></li>";

        protected void Page_Load(object sender, EventArgs e)
        {
            switch (UploadType)
            {
                case "View":
                    {
                        templateHTML = viewTemplateHTML;
                        this.Controls.Add(new Literal() { Text = GetFilesHTML() });
                    }
                    break;
                case "Edit":
                    {
                        templateHTML = viewTemplateHTML;
                        this.Controls.Add(new Literal() { Text = GetFilesHTML() });
                        AddFileUploader();
                    }
                    break;
                case "Add":
                    {
                        AddFileUploader();
                    }
                    break;
                default:
                    {
                        this.Controls.Add(new Literal() { Text = "Invalid type." });
                    } break;
            }

        }

        private void AddFileUploader()
        {
            short tabIndex = 0;
            short.TryParse(TabIndex, out tabIndex);
            for (int i = 0; i < fileUploadCount; i++)
            {
                FileUpload fileUpload = new FileUpload();
                fileUpload.ID = "fileUpload" + i;
                fileUpload.Attributes.Add("class", "inlineBtn1 inputw6");
                fileUpload.Attributes.Add("accept", GetAccept());
                if (tabIndex != 0)
                {
                    fileUpload.TabIndex = tabIndex;

                }
                this.Controls.Add(fileUpload);
            }
        }

        private string GetAccept()
        {
            return "application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet,"
                + "application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document,"
                + "application/pdf,application/x-zip-compressed,application/x-rar-compressed,image/jpeg,image/png,video/mpeg,image/bmp,image/gif."
                + "application/zip,application/pdf";
        }

        protected string GetFilesHTML()
        {
            List<FilesEntity> list = new List<FilesEntity>();
            string Host = System.Configuration.ConfigurationManager.AppSettings["DomainHost"];
            StringBuilder sb = new StringBuilder();
            list = new FileApplication().GetFileListBySourceId(TicketID, FileSourceType.Ticket);
            if (null != list && list.Count > 0)
            {
                string templateHTML = string.Empty;
                if (UploadType == "View")
                {
                    templateHTML = viewTemplateHTML;
                }
                else if (UploadType == "Edit")
                {
                    templateHTML = editTemplateHTML;
                }
                foreach (FilesEntity filesEntity in list)
                {
                    sb.AppendFormat(templateHTML, Host, filesEntity.FileID, filesEntity.FileSize, filesEntity.FileTitle, filesEntity.ContentType);
                }
                return string.Format("<ul class=\"fileList\">{0}</ul>", sb);
            }
            else
            {
                return "";
            }
        }
    }
}