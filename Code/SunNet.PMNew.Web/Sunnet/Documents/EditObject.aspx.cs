using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Framework.Utils;
using System.IO;
using StructureMap;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class EditObject : BaseWebsitePage
    {
        FileApplication fileApp = new FileApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["id"])
                    || string.IsNullOrEmpty(Request.QueryString["type"])
                    || (QS("id", 0) == 0))
                {
                    ShowArgumentErrorMessageToClient();
                    return;
                }
                int id = int.Parse(Request.QueryString["id"]);
                string type = Request.QueryString["type"];

                if (type == DirectoryObjectType.Directory.ToString())
                {
                    rbtnNewDirectory.Checked = true;
                    rbtnNewFile.Checked = false;

                    DirectoryEntity model = fileApp.GetDirectory(int.Parse(Request.Params["id"]));
                    DirectoryEntity parent = fileApp.GetDirectory(model.ParentID);
                    List<DirectoryEntity> list = fileApp.GetDirectories(parent.ParentID);
                    list.BindDropdown<DirectoryEntity>(ddlCurrent, "Title", "ID", "Please Select", "0");
                    ddlCurrent.SelectedValue = model.ParentID.ToString();

                    txtTitle.Text = model.Title;
                    txtDesc.Text = model.Description;
                }
                else if (type == DirectoryObjectType.File.ToString())
                {

                    int direid = 0;
                    if (!string.IsNullOrEmpty(Request.Params["direid"])
                        && int.TryParse(Request.Params["direid"], out direid))
                    {
                        DirectoryObjectsEntity objects = fileApp.GetObjects(direid);
                        if (!ISReadOnlyRole || UserInfo.ID == objects.CreatedBy)
                        {
                            DirectoryEntity parent = fileApp.GetDirectory(objects.DirectoryID);
                            ddlCurrent.Items.Add(new ListItem(parent.Title, parent.ID.ToString(), true));
                            FilesEntity file = fileApp.Get(objects.ObjectID);
                            txtDesc.Text = file.ThumbPath;
                        }
                        else
                        {
                            ShowArgumentErrorMessageToClient();
                            return;
                        }
                    }
                    rbtnNewDirectory.Checked = false;
                    rbtnNewFile.Checked = true;
                }
            }
        }
        private DirectoryEntity GetDirectory()
        {
            DirectoryEntity model = fileApp.GetDirectory(QS("id", 0));
            model.Description = txtDesc.Text.NoHTML();
            model.Logo = "Directory.png";
            model.Title = txtTitle.Text.NoHTML();
            model.ParentID = int.Parse(ddlCurrent.SelectedValue);

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
            int id = int.Parse(Request.Params["id"]);
            FilesEntity model = fileApp.Get(id);
            if (fileCompany.HasFile)
            {
                model.FilePath = UtilFactory.Helpers.FileHelper.SaveUploadFiles("Company", UserInfo.CompanyID, fileCompany.PostedFile);
                model.ContentType = Path.GetExtension(fileCompany.FileName);
                model.FileTitle = Path.GetFileName(fileCompany.FileName);
                model.FileTitle = model.FileTitle.Substring(0, model.FileTitle.LastIndexOf("."));
                model.FileSize = fileCompany.PostedFile.ContentLength;
            }
            model.ThumbPath = txtDesc.Text;
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
                if (fileApp.UpdateFile(model))
                {
                    ShowSuccessMessageToClient(true, true);
                }
                else
                {
                    ShowFailMessageToClient(fileApp.BrokenRuleMessages, false);
                }

            }
        }
    }
}
