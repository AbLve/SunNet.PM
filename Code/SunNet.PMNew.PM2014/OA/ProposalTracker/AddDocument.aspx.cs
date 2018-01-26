using StructureMap;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SunNet.PMNew.PM2014.OA.ProposalTracker
{
    public partial class AddDocument : BasePage
    {
        FileApplication fileApp = new FileApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 540;
            if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
            if (QS("ID", 0) == 0)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!fileUpload.HasFile)
            {
                ShowFailMessageToClient("Please upload file");
                return;
            }
            ProposalTrackerEntity proposaltrackerEntity = new App.ProposalTrackerApplication().Get(QS("ID", 0));
            if (proposaltrackerEntity == null)
            {
                ShowFailMessageToClient("unauthorized access.");
                return;
            }

            FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("ProposalTracker", proposaltrackerEntity.ProjectID, fileUpload.PostedFile); ;

            model.ProposalTrackerId = proposaltrackerEntity.ProposalTrackerID;
            model.ContentType = fileUpload.PostedFile.ContentType;
            model.FileID = 0;
            model.FileSize = fileUpload.PostedFile.ContentLength;
            model.FileTitle = Path.GetFileName(fileUpload.PostedFile.FileName);
            model.IsDelete = false;
            model.IsPublic = UserInfo.Role == RolesEnum.CLIENT;
            model.SourceType = (int)FileSourceType.WorkRequest;
            model.ThumbPath = txtFileTitle.Text;
            model.IsDelete = false;
            model.Tags = txtTags.Text.Trim();
            bool result = true;
            result = fileApp.AddFile(model) > 0;

            if (!result)
            {
                ShowFailMessageToClient(fileApp.BrokenRuleMessages);
            }
            else
            {
                Redirect(EmptyPopPageUrl, false, true);
            }
        }
    }
}