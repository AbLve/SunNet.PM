using System;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.OA.Seals
{
    public partial class AddSealFile : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((Pop)this.Master).Width = 540;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (QS("ID", 0) > 0)
            {
                if (fileUpload.PostedFile.ContentLength == 0)
                {
                    ShowFailMessageToClient("Please specify a file to upload.");
                    return;
                }
                string fileName = fileUpload.PostedFile.FileName;
                string tmpFileName = string.Format("{0}{2}{1}",
                    DateTime.Now.ToString("MMddyyHHmmss"),
                    fileName.Substring(fileName.LastIndexOf(".")),
                    1);
                fileUpload.PostedFile.SaveAs(Config.SealFilePath + tmpFileName);
                SealFileEntity fileEntity = new SealFileEntity();
                fileEntity.Title = txtTitle.Text;
                fileEntity.Name = fileName;
                fileEntity.Path = Config.SealFilePath + tmpFileName;
                fileEntity.SealRequestsID = QS("ID", 0);
                fileEntity.UserID = UserInfo.UserID;
                fileEntity.WorkflowHistoryID = 2;
                fileEntity.IsDeleted = false;
                fileEntity.CreateOn = DateTime.Now;
                if (new App.SealsApplication().SealFilesInsert(fileEntity) > 0)
                    Redirect(EmptyPopPageUrl, false, true);
                else
                    Redirect("File upload error.", "AddSealFile.aspx?ID" + QS("ID"));
            }
            else
                ShowFailMessageToClient("unauthorized access.");
        }
    }
}