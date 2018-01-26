using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Framework.Core.BrokenMessage;
namespace SunNet.PMNew.Web.UserControls
{
    public partial class LeftMenu : BaseAscx
    {
        public int ParentID
        {
            get
            {
                if (ViewState["ParentID"] != null)
                {
                    return (int)ViewState["ParentID"];
                }
                return 1;
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }
        public int RoleID
        {
            get
            {
                //return (int)ViewState["RoleID"];
                return UserInfo.RoleID;
            }
            set
            {
                ViewState["RoleID"] = value;
            }
        }
        public int CurrentIndex
        {
            get
            {
                if (!(null == ViewState["CurrentIndex"]))
                    return (int)ViewState["CurrentIndex"];
                return 0;
            }
            set
            {
                ViewState["CurrentIndex"] = value;
            }
        }
        public int CurrentSecondIndex
        {
            get
            {
                if (!(null == ViewState["CurrentSecondIndex"]))
                    return (int)ViewState["CurrentSecondIndex"];
                return 0;
            }
            set
            {
                ViewState["CurrentSecondIndex"] = value;
            }
        }

        UserApplication userApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            userApp = new UserApplication();
            InitControl(BaseWebsitePage.UserInfo.RoleID);

        }
        private void InitControl(int role)
        {
            List<ModulesEntity> listCurrent = userApp.GetRoleModules(role);
            ltlLeft.Text = GetTree(listCurrent, ParentID);
        }
        #region Template
        private string tempFirstChild = @"<li id='leftMenu{ID}'  class='{ClassName}' ><a  onclick='return {Click}'  href='{Href}'>{Text}</a></li>";
        private string tempFirstParent = @"<li class='{ClassName}'  onclick='return {Click}'  id='leftMenu{ID}'  ><a  href='{Href}'>{Text}</a></li><div  id='leftMenu{ID}_second' >{Children}</div>";
        private string tempSecondChild = @"<div class='leftSub'><a  id='leftMenu{ID}'  onclick='return {Click}'  class='{ClassName}' href='{Href}'>{Text}</a></div>";

        private string classFirstSelected = "currentleft";
        private string classFirstUnSelectedChild = "";
        private string classFirstUnSelectedParent = "sub";
        private string classSecondSelected = "thiscate";
        private string classSecondUnSelected = "";


        #endregion
        // Create Modules tree
        private string GetTree(List<ModulesEntity> list, int parentID)
        {
            StringBuilder strTree = new StringBuilder();

            ModulesEntity parent = list.Find(m => m.ID == parentID);

            if (null == parent)
            {
                return string.Empty;
            }
            var children = list.FindAll(m => m.ParentID == parent.ID);
            StringBuilder strChildren = new StringBuilder();
            string className = GetClassName(parent);
            if (children.Count<ModulesEntity>() > 0)
            {
                List<ModulesEntity> listChildren = children.ToList<ModulesEntity>();
                foreach (ModulesEntity model in listChildren)
                {
                    strChildren.Append(GetTree(list, model.ID));
                }
            }
            else
            {
                string temp = GetTemplate(parent);
                string child = temp.Replace("{ID}", parent.ID.ToString())
                                        .Replace("{Href}", parent.ModulePath)
                                        .Replace("{Text}", parent.ModuleTitle)
                                        .Replace("{ClassName}", className)
                                        .Replace("{Click}", parent.ClickFunctioin)
                                        .Replace("{Children}", strChildren.ToString());
                return child;
            }
            string _this = string.Empty;
            if (parent.ID != ParentID)
            {
                _this = tempFirstParent.Replace("{ID}", parent.ID.ToString())
                    .Replace("{ID}", parent.ID.ToString())
                    .Replace("{Click}", parent.ClickFunctioin)
                    .Replace("{Href}", string.Format("{0}{1}", parent.ModulePath, parent.DefaultPage))
                    .Replace("{Text}", parent.ModuleTitle)
                    .Replace("{Children}", strChildren.ToString())
                    .Replace("{ClassName}", className);
            }
            else
            {
                _this = strChildren.ToString();
            }
            strTree.Append(_this);
            return strTree.ToString();
        }

        private string GetTemplate(ModulesEntity parent)
        {
            string temp = string.Empty;
            if (parent.IsModule)
            {
                temp = tempFirstParent;
            }
            else
            {
                if (parent.ParentID == ParentID)
                {
                    temp = tempFirstChild;
                    if (!parent.ShowInMenu)
                    {
                        temp = string.Empty;
                    }
                }
                else
                {
                    temp = tempSecondChild;
                    if (!parent.ShowInMenu)
                    {
                        temp = string.Empty;
                    }
                }
            }
            return temp;
        }

        private string GetClassName(ModulesEntity parent)
        {
            string className = string.Empty;
            if (parent.ParentID == ParentID)
            {
                if (parent.IsPage)
                {
                    if (parent.ID == CurrentSecondIndex)
                        className = classFirstSelected;
                    else
                        className = classFirstUnSelectedChild;
                }
                else if (parent.IsModule)
                {
                    if (parent.ID == CurrentIndex)
                        className = classFirstSelected;
                    else
                        className = classFirstUnSelectedParent;
                }
            }
            else
            {
                if (parent.ID == CurrentSecondIndex)
                    className = classSecondSelected;
                else
                    className = classSecondUnSelected;
            }
            return className;
        }

    }
}