using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils.Helpers;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class ListTicketCancel : BaseWebsitePage
    {
        #region declare
        int page = 1;
        int recordCount;
        TicketsSearchConditionDTO dto;
        ProjectApplication proApp = new ProjectApplication();
        TicketsRelationApplication trApp = new TicketsRelationApplication();
        TicketsApplication ticketAPP = new TicketsApplication();
        List<ProjectDetailDTO> listPorject = new List<ProjectDetailDTO>();
        UserApplication userApp = new UserApplication();
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            listPorject = proApp.GetUserProjects(UserInfo);
            if (!IsPostBack)
            {
                TicketSatusBind();
                ProjectTitleBind();
                int projectID = QS("pid", 0);

                if (projectID != 0)
                {
                    ddlProject.SelectedValue = projectID.ToString();
                    TicketsDataBind(projectID.ToString());
                }
                else
                {
                    TicketsDataBind(string.Empty);
                }
            }
        }

        #region initial data bind

        private void TicketsDataBind(string projectID)
        {
            List<TicketsEntity> list = null;

            GetTicketsListByConditionRequest request = new GetTicketsListByConditionRequest();

            string keyWord = this.txtKeyWord.Text.Trim();

            dto = new TicketsSearchConditionDTO();

            dto.OrderExpression = hidOrderBy.Value;

            dto.OrderDirection = hidOrderDirection.Value;

            dto.KeyWord = ReturnTicketId(keyWord).NoHTML();
            dto.IsInternal = false;
            if (ddlStatus.SelectedIndex <= 0)
            {
                dto.Status = "0";
            }
            else
            {
                dto.Status = this.ddlStatus.SelectedValue;
            }
            if (UserInfo.Role == RolesEnum.ADMIN || UserInfo.Role == RolesEnum.PM)
            {
                dto.Company = "0";
            }
            else
            {
                dto.Company = UserInfo.CompanyID.ToString();
            }

            if (!string.IsNullOrEmpty(projectID))
            {
                dto.Project = projectID;
            }
            else
            {
                if (this.ddlProject.SelectedIndex <= 0)
                {
                    dto.Project = "0";
                }
                else
                {
                    dto.Project = this.ddlProject.SelectedValue;
                }
            }

            TicketsType ticketType = TicketsType.None;

            ticketType = (TicketsType)int.Parse(ddlTicketType.SelectedValue);

            int pageCount = ticketAPP.GetCancelCount(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company), int.Parse(dto.Status), ticketType, dto.KeyWord);

            if (pageCount > 0)
            {
                list = ticketAPP.GetCancelList(UserInfo.ID, int.Parse(dto.Project), int.Parse(dto.Company), int.Parse(dto.Status), ticketType
                    , dto.KeyWord, anpUsers.CurrentPageIndex, anpUsers.PageSize, dto.OrderExpression, dto.OrderDirection);

                this.trNoTickets.Visible = false;
            }
            else
            {
                this.trNoTickets.Visible = true;
            }

            anpUsers.RecordCount = pageCount;
            this.rptTicketsList.DataSource = list;
            this.rptTicketsList.DataBind();
        }

        private void TicketSatusBind()
        {
            var dictionary = new Dictionary<int, string>();

            int[] array = { (int)TicketsState.Cancelled, (int)TicketsState.Estimation_Fail };

            foreach (int value in Enum.GetValues(typeof(TicketsState)))
            {
                if (array.Contains(value))
                {
                    dictionary.Add(value, Enum.GetName(typeof(TicketsState), value).Replace("_", " "));
                }
            }

            ddlStatus.DataTextField = "Value";
            ddlStatus.DataValueField = "Key";
            ddlStatus.DataSource = dictionary;
            ddlStatus.DataBind();
            this.ddlStatus.Items.Insert(0, new ListItem("Please select...", ""));
        }

        private void ProjectTitleBind()
        {
            this.ddlProject.DataTextField = "Title";
            this.ddlProject.DataValueField = "ProjectID";

            this.ddlProject.DataSource = listPorject;
            this.ddlProject.DataBind();
            if (listPorject.Count != 1)
                this.ddlProject.Items.Insert(0, new ListItem("Please select...", ""));
        }

        public string GetAllowPid()
        {
            string pidList = "";

            foreach (ProjectsEntity item in listPorject)
            {
                pidList += item.ProjectID + ",";
            }

            return pidList.TrimEnd(',');
        }

        #endregion

        protected void SearchImgBtn_Click(object sender, ImageClickEventArgs e)
        {
            TicketsDataBind(string.Empty);
        }

        protected void anpUsers_PageChanged(object sender, EventArgs e)
        {
            page = anpUsers.CurrentPageIndex;
            TicketsDataBind(string.Empty);
        }

        #region common method

        public string ShowRelatedByTid(string tid)
        {
            return trApp.GetAllRelationStringById(Convert.ToInt32(tid), false).TrimEnd(',');
        }
        protected string GetUserNameByCreateID(string cid)
        {
            string userName = this.GetClientUserName(userApp.GetUser(Convert.ToInt32(cid)));
            return userName.Length == 0 ? "" : userName;
        }

        #endregion

        protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
