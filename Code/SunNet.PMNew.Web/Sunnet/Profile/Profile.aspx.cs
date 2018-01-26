using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;

namespace SunNet.PMNew.Web.Sunnet.Profile
{
    public partial class Profile : BaseWebsitePage
    {
        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (UserInfo.Role == RolesEnum.Supervisor) 
                { 
                
                }
                userApp = new UserApplication();
                UsersEntity model = UserInfo;
                Profile1.IsAdd = false;
                Profile1.IsSunnet = !(model.Role == RolesEnum.CLIENT);
                Profile1.UserToEdit = model;
            }
            catch
            {
                this.ShowArgumentErrorMessageToClient();
            }
        }
    }
}
