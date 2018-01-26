using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using StructureMap;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;
using System.IO;
namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class NewDirectory : BaseWebsitePage
    {
        FileApplication fileApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            fileApp = new FileApplication();
            if (!IsPostBack)
            {
                if (QS("id", 0) == 0)
                {
                    ShowArgumentErrorMessageToClient();
                }
                else
                {
                    DirectoryEntity model = fileApp.GetDirectory(int.Parse(Request.Params["id"]));
                    List<DirectoryEntity> list = fileApp.GetDirectories(model.ParentID);
                    list.BindDropdown<DirectoryEntity>(ddlCurrent, "Title", "ID", "Please Select", "0");
                    ddlCurrent.SelectedValue = model.ID.ToString();
                }
                SetControlsStatus();
            }
        }
        private void SetControlsStatus()
        {
            if (!ISCreateDirectoryRoles)
            {
                rbtnNewFile.Checked = true;
                rbtnNewFile.Enabled = false;
                rbtnNewDirectory.Checked = false;
                rbtnNewDirectory.Enabled = false;
            }
            else
            {
                if (!string.IsNullOrEmpty(Request.Params["newdirectory"]))
                {
                    rbtnNewFile.Checked = false;
                    rbtnNewFile.Enabled = false;
                    rbtnNewDirectory.Checked = true;
                    rbtnNewDirectory.Enabled = false;
                }
                else if (!string.IsNullOrEmpty(Request.Params["both"]))
                {
                    rbtnNewFile.Checked = true;
                    rbtnNewFile.Enabled = true;
                    rbtnNewDirectory.Checked = false;
                    rbtnNewDirectory.Enabled = true;
                }
                else
                {
                    rbtnNewFile.Checked = true;
                    rbtnNewFile.Enabled = false;
                    rbtnNewDirectory.Checked = false;
                    rbtnNewDirectory.Enabled = false;
                }
            }
        }
        private DirectoryEntity GetDirectory()
        {
            DirectoryEntity model = FileFactory.CreateDirectoryEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.Description = txtDesc.Text.NoHTML();
            model.Logo = "Directory.png";
            model.Title = txtTitle.Text.NoHTML();
            model.ParentID = int.Parse(ddlCurrent.SelectedValue);
            model.Type = DirectoryObjectType.Directory.ToString();
            return model;
        }
        private DirectoryObjectsEntity GetObject(int fileID)
        {
            DirectoryObjectsEntity model = FileFactory.CreateDirectoryObject(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());

            model.DirectoryID = int.Parse(ddlCurrent.SelectedValue);
            model.Logo = "File.png";
            model.ObjectID = fileID;
            model.Type = DirectoryObjectType.File.ToString();

            return model;
        }
        private FilesEntity GetFile()
        {
            int companyID = UserInfo.CompanyID;
            FilesEntity model = FileFactory.CreateFileEntity(UserInfo.ID, ObjectFactory.GetInstance<ISystemDateTime>());
            model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Company", companyID, fileCompany.PostedFile);
            model.CompanyID = companyID;
            model.ContentType = Path.GetExtension(fileCompany.FileName);
            model.FileID = 0;
            model.FileSize = fileCompany.PostedFile.ContentLength;
            model.FileTitle = Path.GetFileName(fileCompany.FileName);
            model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
            model.IsDelete = false;
            model.IsPublic = false;
            model.SourceType = (int)FileSourceType.Company;
            model.ThumbPath = txtDesc.Text;
            model.IsDelete = false;
            return model;
        }
        private bool CheckInput(out string msg)
        {
            if (rbtnNewDirectory.Checked)
            {
                if (string.IsNullOrEmpty(txtTitle.Text))
                {
                    msg = "Directory title can not be null";
                    return false;
                }
            }
            else if (rbtnNewFile.Checked)
            {
                if (!fileCompany.HasFile)
                {
                    msg = "You must select a file";
                    return false;
                }
            }
            msg = string.Empty;
            return true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            if (!CheckInput(out msg))
            {
                ShowMessageToClient(msg, 2, false, false);
                return;
            }
            if (rbtnNewDirectory.Checked)
            {
                DirectoryEntity model = GetDirectory();
                int id = fileApp.UpdateDirectory(model);
                if (id > 0)
                {
                    Session["CreatedDirectory"] = string.Format("{0}-{1}", ddlCurrent.SelectedValue, id.ToString());
                    this.ShowSuccessMessageToClient();
                }
                else
                {
                    this.ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                }
            }
            else if (rbtnNewFile.Checked)
            {
                FilesEntity model = GetFile();
                FileApplication fileApp = new FileApplication();
                int result = fileApp.AddFile(model);
                if (result <= 0)
                {
                    ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                    return;
                }
                else
                {
                    DirectoryObjectsEntity objecttoadd = GetObject(result);
                    int id = fileApp.PushObjectToDirectory(objecttoadd);
                    if (id > 0)
                    {
                        ShowSuccessMessageToClient(true, true);
                    }
                    else
                    {
                        ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                        return;
                    }
                }
            }
        }
    }
}
