using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;

namespace SunNet.PMNew.PM2014.Demo
{
    public partial class ExtendedDropDownList : System.Web.UI.Page
    {
        UserApplication userApp = new UserApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var list = new List<ListItem>()
                {
                    new ListItem("Lading") { Selected = false },
                    new ListItem("A", "1") { Selected = false },
                    new ListItem("B", "2") { Selected = false },
                    new ListItem("C", "3") { Selected = false },
                    new ListItem("D", "4") { Selected = false },
                    new ListItem("E", "5") { Selected = false },
                    new ListItem("Rome") { Selected = false },
                    new ListItem("I", "1") { Selected = false },
                    new ListItem("II", "2") { Selected = false },
                    new ListItem("III", "3") { Selected = false },
                    new ListItem("IV", "4") { Selected = false },
                    new ListItem("V", "5") { Selected = false }
                };
                ExtendedDropdownList1.OptionGroupValues = "Lading,Rome";
                ExtendedDropdownList1.DataSource = list;
                ExtendedDropdownList1.DataBind();

                SearchUsersRequest request = new SearchUsersRequest(
                SearchUsersType.List, false, " FirstName ", " ASC ");
                //request.IsActive = true;
                //request.IsSunnet = true;

                SearchUserResponse response = userApp.SearchUsers(request);
                //ddlUsers.DataTextField = "FirstAndLastName";
                //ddlUsers.DataValueField = "UserID";
                ddlUsers.DataSource = response.ResultList;
                Func<UsersEntity, string, bool> userByRole = delegate(UsersEntity user, string status)
                {
                    return user.Role.ToString() == status;
                };
                ddlUsers.DataBind(userByRole);
                ddlUsers.SelectedValue = "120";

                //ddlUsers2.DataTextField = "FirstAndLastName";
                //ddlUsers2.DataValueField = "UserID";
                ddlUsers2.DataSource = response.ResultList;
                Func<UsersEntity, string, bool> userByStatus = delegate(UsersEntity user, string status)
                {
                    return user.Status == status;
                };
                ddlUsers2.DataBind(userByStatus);
                ddlUsers2.SelectedValue = "120";
                ddlUsers2.DefaultSelectText = "User is required";

                //ddlUsers3.DataTextField = "FirstAndLastName";
                //ddlUsers3.DataValueField = "UserID";
                ddlUsers3.DataSource = response.ResultList.FindAll(x => x.Status == "ACTIVE");
                ddlUsers3.DataBind(userByStatus);
                ddlUsers3.SelectedValue = "120";

                Button1_Click(null, null);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Literal1.Text = ExtendedDropdownList1.SelectedValue;
            Literal2.Text = ddlUsers.SelectedValue;
            Literal3.Text = ddlUsers2.SelectedValue;
            Literal4.Text = ddlUsers3.SelectedValue;
        }
    }
}