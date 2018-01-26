using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using System.Text.RegularExpressions;
using SunNet.PMNew.Entity.CompanyModel;
using SunNet.PMNew.Framework.Utils.Helpers;
using SunNet.PMNew.Entity.WorkRequestModel;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class WorkRequestFiles : BaseWebsitePage
    {

        FileApplication fileApp = new FileApplication();

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
        WorkRequestApplication wrApp = new WorkRequestApplication();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitControl();
            }
        }

        private void InitControl()
        {
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.WorkRequest, true, hidOrderBy.Value, hidOrderDirection.Value);
            request.CurrentPage = anpWorkRequestFiles.CurrentPageIndex;
            request.PageCount = anpWorkRequestFiles.PageSize;
            request.Keyword = txtTag.Text.Trim().NoHTML();
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                request.UserID = UserInfo.UserID;
            }
            List<FileDetailDto> list = fileApp.GetFiles(request);
            rpt.DataSource = list;
            rpt.DataBind();
            anpWorkRequestFiles.RecordCount = request.RecordCount;
        }



        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpWorkRequestFiles.CurrentPageIndex = 1;
            InitControl();
        }


        protected void anpWorkRequestFiles_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }
    }
}
