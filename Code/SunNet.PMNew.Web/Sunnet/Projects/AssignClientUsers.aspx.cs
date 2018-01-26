using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;

namespace SunNet.PMNew.Web.Sunnet.Projects
{
    public partial class AssignClientUsers : BaseWebsitePage
    {
        List<UsersEntity> userList;
        protected void Page_Load(object sender, EventArgs e)
        {
            userList = new ProjectApplication().GetPojectClientUsers(QS("projectId", 0), QS("companyId", 0));
            if (!IsPostBack)
            {
                rptAssignUser.DataSource = new UserApplication().GetActiveUserList().FindAll(r => r.CompanyID == QS("companyId", 0));
                rptAssignUser.DataBind();
            }
        }

        protected string ShowCheckbox(object userId)
        {
            UsersEntity entity = userList.Find(r => r.UserID == (int)userId);
            if (entity == null)
                return string.Format("<input type='checkbox' name='chkUser' value='{0}' />", userId);
            return string.Format("<input type='checkbox' name='chkUser' checked='checked' value='{0}' />", userId);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            string userIds = QF("chkUser");


            if (userIds.Trim() == "" && (userList == null || userList.Count == 0)) //直接关闭
            {
                ShowMessageToClient("Operation successful.", 0, true, true);
                return;
            }

            if (userIds.Trim() == "") //全部删除
            {
                string ids = string.Join(",", userList.Select(r => r.UserID.ToString()).ToArray());
                new ProjectApplication().DeleteProjectClientUsers(QS("projectId", 0), ids);
            }
            else if (userList == null || userList.Count == 0) //新添加
            {
                foreach (string s in userIds.Split(','))
                {
                    new ProjectApplication().AssignUserToProject(new ProjectUsersEntity() { UserID = int.Parse(s), ISClient = true, ProjectID = QS("projectId", 0) });
                }
            }
            else
            {
                List<string> list = userIds.Split(',').ToList();

                List<string> listOld = userList.Select(r => r.UserID.ToString()).ToList();

                string[] deletedId = listOld.Except(list).ToArray();
                if (deletedId.Length > 0)
                {
                    new ProjectApplication().DeleteProjectClientUsers(QS("projectId", 0), string.Join(",", deletedId));
                }

                List<string> listNew = list.Except(listOld).ToList();
                foreach (string s in listNew)
                {
                    new ProjectApplication().AssignUserToProject(new ProjectUsersEntity() { UserID = int.Parse(s), ISClient = true, ProjectID = QS("projectId", 0) });
                }
            }

            ShowMessageToClient("Operation successful.", 0, true, true);

            userList = new ProjectApplication().GetPojectClientUsers(QS("projectId", 0), QS("companyId", 0));
            rptAssignUser.DataSource = new UserApplication().GetActiveUserList().FindAll(r => r.CompanyID == QS("companyId", 0));
            rptAssignUser.DataBind();
        }
    }
}
