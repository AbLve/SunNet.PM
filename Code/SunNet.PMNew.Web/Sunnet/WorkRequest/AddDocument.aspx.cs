using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using StructureMap;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.UserModel;
using System.Text;
using SunNet.PMNew.Entity.FileModel;
using System.IO;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class AddDocument : BaseWebsitePage
    {
        
        FileApplication fileApp = new FileApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = QS("id", 0);
                if (id>0)
                {
                    FilesEntity entity = fileApp.Get(id);
                    BindDataModel(entity);
                }
            }
        }

        protected void BindDataModel(FilesEntity entity)
        {
            txtFileTitle.Text = entity.FileTitle;
            txtTags.Text = entity.Tags;
            lblFile.Visible = true;
            lblFile.Text = string.Format("<a href='/Do/DoDownloadFileHandler.ashx?FileID={0}&size={1}' target='_blank'>{2}</a>",entity.FileID,entity.FileSize,entity.FileTitle);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = QS("id", 0);
            int workRequestId = QS("wid", 0);
            if (fileProject.HasFile || lblFile.Visible==true)
            {
                FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("WorkRequest", workRequestId, fileProject.PostedFile); ;

                model.WorkRequestId = workRequestId;
                model.ContentType = fileProject.PostedFile.ContentType;
                model.FileID = 0;
                model.FileSize = fileProject.PostedFile.ContentLength;
                if (string.IsNullOrEmpty(txtFileTitle.Text))
                {
                    model.FileTitle = Path.GetFileName(fileProject.FileName);
                    model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
                }
                else
                {
                    model.FileTitle = txtFileTitle.Text;
                }
                model.IsDelete = false;
                model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                model.SourceType = (int)FileSourceType.WorkRequest;
                model.ThumbPath = Path.GetFileName(fileProject.PostedFile.FileName);
                model.IsDelete = false;
                model.Tags = txtTags.Text.Trim();
                bool result = true;
                if (id > 0)
                {
                    model.FileID = id;
                    result = fileApp.UpdateFile(model);
                }
                else
                {
                    result = fileApp.AddFile(model) > 0 ? true : false;
                }
                if (!result)
                {
                    this.ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                }
                else
                {
                    this.ShowSuccessMessageToClient();
                }
            }
            else
            {
                this.ShowFailMessageToClient("Please select a file");
            }
        }
    }
}
