using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.WorkRequestModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.Sunnet.WorkRequest
{
    public partial class AddNote : BaseWebsitePage
    {

        WorkRequestApplication app = new WorkRequestApplication();
        UserApplication userApp = new UserApplication();

        public UsersEntity UserInfo
        {
            get
            {
                IEncrypt encrypt = UtilFactory.GetEncryptProvider(EncryptType.DES);
                string userID = encrypt.Decrypt(UtilFactory.Helpers.CookieHelper.Get(encrypt.Encrypt("LoginUserID")));
                if (string.IsNullOrEmpty(userID))
                {
                    return null;
                }
                UtilFactory.Helpers.CookieHelper.Resume(encrypt.Encrypt("LoginUserID"), 30);

                int id = int.Parse(userID);
                UsersEntity model = userApp.GetUser(id);
                return model;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id = QS("id", 0);
                if (id > 0)
                {
                }
            }

        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            int workRequestId = QS("wid", 0);
            WorkRequestNoteEntity model = new WorkRequestNoteEntity();
            model.WorkRequestID = workRequestId;
            model.Title = txtTitle.Text.Trim();
            model.Description = txtDescription.Text.Trim();
            model.ModifyOn = DateTime.Now;
            model.ModifyBy = UserInfo.UserID;
            if (app.AddNote(model)>0)
            {
                this.ShowSuccessMessageToClient();
            }
            else
            {
                this.ShowFailMessageToClient(app.BrokenRuleMessages, false);
            }
        }
    }
}
