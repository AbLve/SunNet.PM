using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Providers;
using System.IO;
using System.Text;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    public partial class SealRequestsEdit : BaseWebsitePage
    {
         SealsApplication app = new SealsApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (QS("id", 0) > 0) //edit
                {

                    SealRequestsEntity sealRequestEntity = app.GetSealRequests(QS("Id", 0));
                    List<int> userList = app.GetUsersId(QS("ID", 0));
                    if (sealRequestEntity == null || !userList.Contains(UserInfo.UserID))
                    {
                        ShowFailMessageToClient("unauthorized access.");
                        return;
                    }
                    else
                    {
                        BindSealRequestData(sealRequestEntity);
                    }
                    hdID.Value = QS("ID", 0).ToString();
                }
                else //add
                {
                    if (UserInfo.Office == "CN" && UserInfo.Role != RolesEnum.PM) return;

                    BindDropDownData();

                    trUploadFiles.Visible = true;
                    trStatus.Visible = false;
                    btnSave.Visible = true;
                }
            }
        }

        private string BuilderSeals(int sealRequestId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border='0' ><tbody>");

            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            List<SealUnionRequestsEntity> listUnioSeals = app.GetSealUnionRequestsList(sealRequestId);

            foreach (SealsEntity entity in list)
            {
                SealUnionRequestsEntity unionEntity = listUnioSeals.Find(r => r.SealID == entity.ID);
                sb.AppendFormat("<tr><td><span disabled=\"disabled\"><input type=\"checkbox\" disabled=\"disabled\" {0}><label>{1}</label></span>",
                    unionEntity == null ? "" : "checked=\"checked\"", entity.SealName);
                if (unionEntity != null && unionEntity.SealedDate > MinDate)
                    sb.AppendFormat("<span><span class=\"sealinfo\">Sealed By:</span> {0} <span class=\"sealinfo\">Sealed Date:</span> {1} </span>",
                        new App.UserApplication().GetUser(unionEntity.SealedBy).FirstName, unionEntity.SealedDate.ToString("MM/dd/yyyy"));
                sb.Append("</td></tr>");
            }
            sb.Append("</tbody></table>");
            return sb.ToString();
        }

        private void BindSealRequestData(SealRequestsEntity entity)
        {
            hdID.Value = entity.ID.ToString();
            txtTitle.Text = entity.Title;
            txtStatus.Text = entity.Status.ToString();
            txtDescription.Text = entity.Description.Replace("<br>", "\r\n");

            List<SealUnionRequestsEntity> list = app.GetSealUnionRequestsList(entity.ID);

            if ((int)entity.Status < (int)RequestStatus.Approved)
            {
                BindDropDownData();
                foreach (SealUnionRequestsEntity tmpEntity in list)
                {
                    ListItem li = chklistSeal.Items.FindByValue(tmpEntity.SealID.ToString());
                    if (li != null)
                        li.Selected = true;
                }
            }
            else
            {
                ltSelas.Text = BuilderSeals(entity.ID);
            }

            if (entity.RequestedBy == UserInfo.UserID && entity.Status < RequestStatus.Approved) //修改为在Approve前都可以删除
                lblFiles.InnerHtml = BuilderFiles(entity.ID, true);
            else
                lblFiles.InnerHtml = BuilderFiles(entity.ID, false);

            if (entity.Status == RequestStatus.Open && UserInfo.UserID == entity.RequestedBy)
                DisableControl(true, string.Empty);
            else
                DisableControl(false, entity.Description);

            switch (entity.Status)
            {
                case RequestStatus.Open:
                    if (entity.RequestedBy == UserInfo.UserID)
                    {
                        trUploadFiles.Visible = true;
                        btnCancel.Visible = true;
                        btnSave.Visible = true;
                        btnSubmit.Visible = true;
                    }
                    break;
                case RequestStatus.Submit:
                    foreach (SealUnionRequestsEntity unionEntity in list)
                    {
                        if (entity.RequestedBy == UserInfo.UserID)
                        {
                            btnSave.Visible = true;
                            trUploadFiles.Visible = true;
                        }
                        if (unionEntity.ApprovedBy == UserInfo.UserID)
                        {
                            btnDenied.Visible = true;
                            btnApproved.Visible = true;
                            break;
                        }
                    }
                    break;
                case RequestStatus.Approved:
                    foreach (SealUnionRequestsEntity unionEntity in list)
                    {
                        if (unionEntity.SealedBy == UserInfo.UserID)
                        {
                            btnSeal.Visible = true;
                            if (unionEntity.IsSealed)
                            {
                                btnSeal.Visible = false;
                            }
                            break;
                        }
                    }
                    break;
                case RequestStatus.Sealed:
                    if (entity.RequestedBy == UserInfo.UserID)
                        btnCompleted.Visible = true;
                    break;
                case RequestStatus.Complete:
                case RequestStatus.Denied:
                case RequestStatus.Cancel:
                    break;
            }
            rptNotes.DataSource = app.GetSealNotesList(entity.ID);
            rptNotes.DataBind();
        }

        private void DisableControl(bool disable, string description)
        {
            txtTitle.Enabled = disable;
            if (!disable)
            {
                txtDescription.Visible = false;
                lblDescription.InnerHtml = description;
                foreach (ListItem li in chklistSeal.Items)
                    li.Enabled = false;
            }
            else
            {
                txtDescription.Enabled = true;
            }
        }

        private string BuilderFiles(int sealRequestId, bool showDeleted)
        {
            string tmpFiles = string.Empty;
            List<SealFileEntity> list = app.GetSealFilesList(sealRequestId);
            bool One = true;
            foreach (SealFileEntity entity in list.FindAll(r => r.Type == 1))
            {
                if (One)
                    One = false;
                else
                    tmpFiles += "&nbsp;&nbsp;&nbsp;&nbsp;";

                tmpFiles += string.Format("<div id='div{0}' style='margin-bottom:-5px;'><a href='/do/DownloadSealFile.ashx?id={0}' target='iframeDownloadFile'>{1}</a>", entity.ID, entity.Name);
                if (showDeleted)
                    tmpFiles += string.Format("&nbsp;&nbsp;<a href='#' onclick='deleteFile(\"{0}\");'><img src='/Images/ico_delete.gif' align='absmiddle'/></a>", entity.ID);
                tmpFiles += "</div>";
            }
            rptFiles.DataSource = list.FindAll(r => r.Type == 2);
            rptFiles.DataBind();
            return tmpFiles;
        }

        private void BindDropDownData()
        {
            List<SealsEntity> list = app.GetList().FindAll(r => r.Status == Status.Active);
            chklistSeal.Items.Clear();
            foreach (SealsEntity entity in list)
            {
                chklistSeal.Items.Add(new ListItem() { Value = entity.ID.ToString(), Text = entity.SealName });
                hdChklistKeys.Value += entity.ID.ToString() + ",";
            }
            hdChklistKeys.Value.TrimEnd(',');
        }

        private void InsertFile(HttpPostedFile file, int sealRequestId, int type, int index)
        {
            string fileName = file.FileName;
            string tmpFileName = string.Format("{0}{2}{1}", DateTime.Now.ToString("MMddyyHHmmss"), fileName.Substring(fileName.LastIndexOf(".")), index);
            file.SaveAs(Config.SealFilePath + tmpFileName);
            SealFileEntity fileEntity = new SealFileEntity();
            fileEntity.Title = string.Empty;
            fileEntity.Name = fileName;
            fileEntity.Path = Config.SealFilePath + tmpFileName;
            fileEntity.SealRequestsID = sealRequestId;
            fileEntity.UserID = UserInfo.UserID;
            fileEntity.Type = type;
            fileEntity.IsDeleted = false;
            fileEntity.CreateOn = DateTime.Now;
            app.SealFilesInsert(fileEntity);
        }

        private SealRequestsEntity CheckData()
        {
            int id;
            if (int.TryParse(hdID.Value, out id))
            {
                SealRequestsEntity sealRequestsEntity = app.GetSealRequests(id);
                if (sealRequestsEntity == null)
                {
                    ShowFailMessageToClient("unauthorized access.");
                    return null;
                }
                return sealRequestsEntity;
            }
            else
            {
                ShowFailMessageToClient("unauthorized access.");
                return null;
            }
        }

        protected string ShowDeleteButton(int id)
        {
            SealFileEntity sealFileEntity = app.GetSealFiles(id);
            SealRequestsEntity sealRequestsEntity = app.GetSealRequests(sealFileEntity.SealRequestsID);
            if (sealFileEntity.UserID != UserInfo.UserID || sealRequestsEntity.Status >= RequestStatus.Approved)
            {
                return "&nbsp;";
            }
            else
            {
                SealsEntity sealsEntity = app.Get(sealFileEntity.SealRequestsID);
                return string.Format("<a href=\"#\" onclick=\"deleteFile('{0}',true)\"><img border=\"0\" title=\"Delete\" src=\"/icons/17.gif\"></a>"
                   , id);
            }
        }

    }
}