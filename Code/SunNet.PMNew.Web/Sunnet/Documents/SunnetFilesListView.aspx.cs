using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.FileModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Documents
{
    public partial class SunnetFilesListView : BaseWebsitePage
    {
        FileApplication fileApp;
        protected void Page_Load(object sender, EventArgs e)
        {
            fileApp = new FileApplication();
            if (!IsPostBack)
            {
                InitDirectories();
                InitControl();
            }
            if (!(Session["CreatedDirectory"] == null))
            {
                InitDirectories();
                hidCurrentDirectory.Value = Session["CreatedDirectory"].ToString();
                txtID.Text = "";
                txtKeyword.Text = "";
                Session["CreatedDirectory"] = null;
            }
        }
        private void InitDirectories()
        {
            List<DirectoryEntity> listFirst = fileApp.GetDirectories(1, false);
            rptFirstDire.DataSource = listFirst;
            rptFirstDire.DataBind();
        }
        protected void rptFirstDire_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DirectoryEntity de = (DirectoryEntity)e.Item.DataItem;
                Repeater rptSecondDire = (Repeater)e.Item.FindControl("rptSecondDire");
                List<DirectoryEntity> listFirst = fileApp.GetDirectories(de.ID, false);
                if (listFirst != null && listFirst.Count > 0)
                {
                    rptSecondDire.DataSource = listFirst;
                    rptSecondDire.DataBind();
                }
                else
                {
                    rptSecondDire.Visible = false;
                }
            }
        }
        private void InitControl()
        {
            SearchFilesRequest request = new SearchFilesRequest(SearchFileType.SunnetFile, true, "ModifiedOn", "DESC");
            request.CurrentPage = anpFiles.CurrentPageIndex;
            request.PageCount = anpFiles.PageSize;
            request.CompanyID = UserInfo.CompanyID;
            string[] dires = hidCurrentDirectory.Value.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (dires.Length == 2)
            {
                if (dires[1] == "0")
                {
                    request.ParentID = int.Parse(dires[0]);
                    request.IncludeChildDirectory = true;
                }
                else
                {
                    request.ParentID = int.Parse(dires[1]);
                    request.IncludeChildDirectory = false;
                }
            }
            else
            {
                request.ParentID = 1;
                request.IncludeChildDirectory = true;
                hidCurrentDirectory.Value = "1-0";
            }
            if (request.ParentID == 1)
            {
                request.ParentID = 0;
            }
            int direobjID = 0;
            if (int.TryParse(txtID.Text, out direobjID))
            {
                request.SearchID = direobjID;
            }
            request.Keyword = txtKeyword.Text.NoHTML();
            List<DirectoryEntity> list = fileApp.GetObjects(request);
            if (list != null && list.Count > 0)
            {
                rptFiles.Visible = true;
                rptFiles.DataSource = list;
                rptFiles.DataBind();
                trNoTickets.Visible = false;
                anpFiles.RecordCount = request.RecordCount;
            }
            else
            {
                rptFiles.Visible = false;
                trNoTickets.Visible = true;
                anpFiles.RecordCount = 0;
            }

        }
        protected void iBtnSearch_Click(object sender, ImageClickEventArgs e)
        {
            anpFiles.CurrentPageIndex = 1;
        }

        protected void anpFiles_PageChanged(object sender, EventArgs e)
        {
            InitControl();
        }


    }
}
