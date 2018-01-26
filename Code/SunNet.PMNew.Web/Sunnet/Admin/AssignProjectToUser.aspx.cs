using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Web.Codes;

namespace SunNet.PMNew.Web.Sunnet.Admin
{
    public partial class AssignProjectToUser : System.Web.UI.Page
    {
        protected bool isClient = false;
        protected UsersEntity UserToEdit;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserApplication userApp = new UserApplication();
            int id = QS("uid"); ;
            if (id == 0)
            {
                return;
            }
            UserToEdit = userApp.GetUser(id);
            isClient = (UserToEdit.Role == RolesEnum.CLIENT);
        }

        public int QS(string id)
        {
            int result;
            if (int.TryParse(Request.QueryString[id], out result))
            {
                return result;
            }
            else
            {
                return 0;
            }

        }
    }
}
