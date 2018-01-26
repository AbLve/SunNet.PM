using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Core.BrokenMessage;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.Admin
{
    public partial class SealDetail : BasePage
    {
        SealsApplication app = new SealsApplication();
        UserApplication userApp = new UserApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropDownData();

                if (QS("ID", 0) > 0) //edit
                {
                    SealsEntity sealsEntity = app.Get(QS("ID", 0));
                    if (sealsEntity == null)
                    {

                        btnSave.Visible = false;
                    }
                    else
                    {
                        BindSealsData(sealsEntity);
                    }
                }
            }
        }

        private void BindSealsData(SealsEntity sealsEntity)
        {
            txtSealName.Text = sealsEntity.SealName;
            txtDescription.Text = sealsEntity.Description;
            ListItem liOwner = ddlOwner.Items.FindByValue(sealsEntity.Owner.ToString());
            if (liOwner == null)
            {
                UsersEntity usersEntity = userApp.GetUser(sealsEntity.Owner);
                ddlOwner.Items.Insert(0, new ListItem() { Value = usersEntity.UserID.ToString(), Text = usersEntity.FirstName });
            }
            else
                liOwner.Selected = true;

            ListItem liApprover = ddlApprover.Items.FindByValue(sealsEntity.Approver.ToString());
            if (liApprover == null)
            {
                UsersEntity usersEntity = userApp.GetUser(sealsEntity.Approver);
                ddlApprover.Items.Insert(0, new ListItem() { Value = usersEntity.UserID.ToString(), Text = usersEntity.FirstName });
            }
            else
                liApprover.Selected = true;

            hdID.Value = sealsEntity.ID.ToString();
            ddlStatus.SelectedValue = ((int)sealsEntity.Status).ToString();
        }
        private void BindDropDownData()
        {
            SearchUsersRequest requestUser = new SearchUsersRequest(
            SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            SearchUserResponse responseuser = userApp.SearchUsers(requestUser);
            ddlOwner.DataSource = responseuser.ResultList.FindAll(r => r.Office == "CN");
            ddlOwner.DataBind();

            ddlApprover.DataSource = responseuser.ResultList.FindAll(r => r.Role == RolesEnum.ADMIN);
            ddlApprover.DataBind();
            ListItem liApprover = ddlApprover.Items.FindByValue(Config.SealApprover.ToString());
            if (liApprover == null)
            {
                UsersEntity usersEntity = userApp.GetUser(Config.SealApprover);
                ddlApprover.Items.Insert(0, new ListItem() { Value = usersEntity.UserID.ToString(), Text = usersEntity.FirstName });
            }
            
        }


        protected SealsEntity GetEntity()
        {
            SealsEntity sealsEntity = new SealsEntity();
            int id = QS("ID", 0);
            if (id > 0)
            {
                sealsEntity.ID = id;
            }
            sealsEntity.SealName = txtSealName.Text.Trim().NoHTML();
            sealsEntity.Owner = int.Parse(ddlOwner.SelectedValue);
            sealsEntity.Approver = int.Parse(ddlApprover.SelectedValue);
            sealsEntity.Description = txtDescription.Text.Trim().NoHTML();
            sealsEntity.Status = (Status)Enum.Parse(typeof(Status), ddlStatus.SelectedValue);
            //  sealsEntity.CreatedOn = DateTime.Now;
            return sealsEntity;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SealsEntity sealsEntity = GetEntity();
            if (app.CheckSealName(sealsEntity.ID, sealsEntity.SealName))
            {
                List<BrokenRuleMessage> list = new List<BrokenRuleMessage>();
                BrokenRuleMessage message = new BrokenRuleMessage("nameExist", "Seal Name already exists.");
                list.Add(message);
                this.ShowFailMessageToClient(list);
            }
            else
            {
                if (sealsEntity.ID > 0) //edit
                {
                    if (app.Update(sealsEntity))
                    {
                        Redirect(Request.RawUrl, false, true);
                    }
                    else
                    {
                        this.ShowFailMessageToClient(app.BrokenRuleMessages);
                    }
                }
                else
                {
                    List<BrokenRuleMessage> list = new List<BrokenRuleMessage>();
                    BrokenRuleMessage message = new BrokenRuleMessage("SealNotExist", "Seal not exists.");
                    list.Add(message);
                    this.ShowFailMessageToClient(list);
                }
            }

        }
    }
}