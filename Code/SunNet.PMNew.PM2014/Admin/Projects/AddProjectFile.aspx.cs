using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.Admin.Projects
{
    public partial class AddProjectFile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 540;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            int id = QS("id", 0);

            if (id == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            else
            {
                ProjectsEntity projectEntity = new ProjectApplication().Get(id);
                if (projectEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return;
                }

                if (fileUpload.PostedFile.ContentLength == 0)
                {
                    ShowFailMessageToClient("Please specify a file to upload.");
                    return;
                }

                FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
                model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Project", id, fileUpload.PostedFile); ;

                model.CompanyID = projectEntity.CompanyID;
                model.ContentType = fileUpload.PostedFile.ContentType;
                model.FileID = 0;
                model.FileSize = fileUpload.PostedFile.ContentLength;
                if (string.IsNullOrEmpty(txtFileTitle.Text))
                {
                    model.FileTitle = Path.GetFileName(fileUpload.FileName);
                    model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
                }
                else
                {
                    model.FileTitle = txtFileTitle.Text;
                }
                model.IsDelete = false;
                model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
                model.SourceType = (int)FileSourceType.Project;
                model.ProjectId = id;
                model.ThumbPath = Path.GetFileName(fileUpload.PostedFile.FileName);
                model.IsDelete = false;

                FileApplication fileApp = new FileApplication();
                int result = fileApp.AddFile(model);

                OperateDocManagements.OperateDocManagementSoapClient client = new OperateDocManagements.OperateDocManagementSoapClient();
                List<FilesEntity> clientFiles = new List<FilesEntity>();
                clientFiles.Add(model);
                client.AddDocManagement(Newtonsoft.Json.JsonConvert.SerializeObject(clientFiles));

                if (result <= 0)
                {
                    Redirect("File upload error.", "AddProjectFile.aspx?ID" + QS("ID"));
                }
                else
                {
                    Redirect(EmptyPopPageUrl, false, true);
                }
            }

        }
    }
}