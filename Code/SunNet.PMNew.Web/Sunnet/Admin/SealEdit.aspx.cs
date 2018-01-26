using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class SealEdit : BaseWebsitePage
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
                else //insert 
                {

                    ListItem liOwner = ddlOwner.Items.FindByValue(Config.SealOwner.ToString());
                    if (liOwner == null)
                    {
                        UsersEntity usersEntity = userApp.GetUser(Config.SealOwner);
                        ddlOwner.Items.Insert(0, new ListItem() { Value = usersEntity.UserID.ToString(), Text = usersEntity.FirstName });
                    }
                    else
                        liOwner.Selected = true;

                    ListItem liApprover = ddlApprover.Items.FindByValue(Config.SealApprover.ToString());
                    if (liApprover == null)
                    {
                        UsersEntity usersEntity = userApp.GetUser(Config.SealApprover);
                        ddlApprover.Items.Insert(0, new ListItem() { Value = usersEntity.UserID.ToString(), Text = usersEntity.FirstName });
                    }
                    else
                        liApprover.Selected = true;
                }
            }
        }


        private void BindDropDownData()
        {
            SearchUsersRequest requestUser = new SearchUsersRequest(
            SearchUsersType.All, false, " FirstName ", " ASC ");
            requestUser.IsSunnet = true;
            requestUser.IsActive = true;
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
    }
}
