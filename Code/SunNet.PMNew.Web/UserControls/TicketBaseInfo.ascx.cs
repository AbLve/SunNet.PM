using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Framework.Utils;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class TicketBaseInfo : BaseAscx
    {
        #region declare

        TicketsApplication ticketApp = new TicketsApplication();
        ProjectApplication proApp = new ProjectApplication();
        UserApplication userApp = new UserApplication();
        public TicketsEntity TicketsEntityInfo { get; set; }

        #endregion

        #region FeedbackMessage
        FeedBackMessageHandler fbmHandler;
        protected string FeedBackMessage(object ticketId)
        {
            return fbmHandler.FeedBackMessage(ticketId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            if (null != TicketsEntityInfo)
            {
                fbmHandler = new FeedBackMessageHandler(UserInfo);
                this.ChangeStatus1.ChangeStatusTicketsEntityInfo = TicketsEntityInfo;
                BindTicketBaseInfo();

                

            }
            else
            {
                return;
            }

        }

        private void BindTicketBaseInfo()
        {
            if (null != TicketsEntityInfo)
            {
                if (this.TicketsEntityInfo.IsEstimates)
                {
                    this.rdoEs.Checked = true;
                }
                else
                {
                    this.rdoNotEs.Checked = true;
                }
                if (TicketsEntityInfo.ConvertDelete == CovertDeleteState.NotABug)
                {
                    this.lilQuestion.Text = "<span  style='Color:red;'>Not A Bug</span>";
                    if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.Leader)
                    {
                        this.lilIsABug.Text = "<input  type='button' class='btnthree' id='btnIsABug' value='Is Bug' onclick=\"updateStatusConfirm('isBug',false);return false;\"/>";
                    }

                }
                bool hasPermission = ticketApp.ValidateIsExistUserUnderProject(TicketsEntityInfo.TicketID, UserInfo.UserID);
                if (!TicketsEntityInfo.IsInternal &&
                    (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN) &&
                    TicketsEntityInfo.Status < TicketsState.PM_Reviewed &&
                    TicketsEntityInfo.Status != TicketsState.Cancelled && hasPermission)
                {
                    this.rdoEs.Enabled = true;
                    this.rdoNotEs.Enabled = true;
                }
                else
                {
                    this.rdoEs.Enabled = false;
                    this.rdoNotEs.Enabled = false;
                }

                this.lilIsInternal.Text = TicketsEntityInfo.IsInternal == true ? "IsInternal," : "";
                this.lilProjectName.Text = ShowProjectName(TicketsEntityInfo.ProjectID);
                this.lilTicketCode.Text = TicketsEntityInfo.TicketCode;
                this.lilTicketPriority.Text = Enum.GetName(typeof(PriorityState), (int)TicketsEntityInfo.Priority);
                this.lilTicketStatus.Text = ChangeStatus(TicketsEntityInfo.Status, TicketsEntityInfo.TicketID);
                this.lilCreateBy.Text = BaseWebsitePage.GetClientUserName(TicketsEntityInfo.CreatedBy);
                this.lilCreateTime.Text = ShowFormatTime(TicketsEntityInfo.CreatedOn);
                this.lilModify.Text = BaseWebsitePage.GetClientUserName(TicketsEntityInfo.ModifiedBy);
                this.lilModifyTime.Text = this.lilModify.Text.Length == 0 ? "" : ShowFormatTime(TicketsEntityInfo.ModifiedOn);
                this.lilDueDate.Text = ShowFormatTime(TicketsEntityInfo.DeliveryDate);
                this.lilScdDate.Text = ShowFormatTime(TicketsEntityInfo.StartDate);
                this.lilInitialTime.Text = TicketsEntityInfo.InitialTime.ToString();
                this.lilEsUserName.Text = string.Format("({0})", BaseWebsitePage.GetClientUserName(TicketsEntityInfo.EsUserID));

                #region Hidden when value is null or access is not satisfy condition

                if (BaseWebsitePage.GetClientUserName(TicketsEntityInfo.ModifiedBy).Length == 0)
                {
                    this.updateBy.Visible = false;
                }
                if (TicketsEntityInfo.IsEstimates)//Estimation Needed
                {
                    trEst.Visible = true;
                    if ((int)TicketsState.Cancelled < (int)TicketsEntityInfo.Status &&
                        (int)TicketsEntityInfo.Status < (int)TicketsState.PM_Verify_Estimation &&
                        TicketsEntityInfo.FinalTime <= 0 && TicketsEntityInfo.EsUserID <= 0)
                    {
                        if (UserInfo.Role == RolesEnum.ADMIN ||
                            UserInfo.Role == RolesEnum.PM ||
                            UserInfo.Role == RolesEnum.Leader)
                        {
                            btnAssignUser.Visible = true;
                        }
                    }

                    if (TicketsEntityInfo.Status > TicketsState.Waiting_For_Estimation ||
                        (TicketsEntityInfo.FinalTime > 0 && TicketsEntityInfo.Status == TicketsState.PM_Reviewed))
                    {
                        if (TicketsEntityInfo.FinalTime > 0)
                        {
                            this.lilLookFinalEs.Text = "<a  onclick=\"LookEsDetail('iushau02u340final');return false;\" style=\"cursor:pointer;\"/><span style='color:blue;'>View Estimation</span></a>";
                        }
                        else if (TicketsEntityInfo.InitialTime > 0 && TicketsEntityInfo.FinalTime <= 0)
                        {
                            this.lilLookInitialEs.Text = "<a  onclick=\"LookEsDetail('fwef654wr432erf12grdge');return false;\" style=\"cursor:pointer;\"/><span style='color:blue;'>View Initial Estimation</span></a>";
                        }

                    }
                }
                else
                {
                    trEst.Visible = false;
                    this.trEstLast.Visible = false;
                }

                this.lilFinalHours.Text = TicketsEntityInfo.FinalTime.ToString(); //final hours

                #region show value who be assigned


                if (TicketsEntityInfo.EsUserID == UserInfo.UserID &&
                    TicketsEntityInfo.Status == TicketsState.Waiting_For_Estimation)
                {
                    this.btnEsTime.Visible = true;
                }

                if (TicketsEntityInfo.DeliveryDate <= UtilFactory.Helpers.CommonHelper.GetDefaultMinDate())
                {
                    this.lilDueDate.Text = "<span style='Color:red;'>waiting for confirm <span>";
                }
                if (TicketsEntityInfo.StartDate <= UtilFactory.Helpers.CommonHelper.GetDefaultMinDate())
                {
                    this.lilScdDate.Text = "<span style='Color:red;'>waiting for confirm <span>";
                }
                if (TicketsEntityInfo.InitialTime <= 0)
                {
                    this.lilInitialTime.Text = "<span style='Color:red;'>waiting<span>";
                }
                if (TicketsEntityInfo.FinalTime <= 0)
                {
                    this.lilFinalHours.Text = "<span style='Color:red;'>waiting<span>";
                }
                //allow user 
                List<int> listAllowUser = new List<int>();
                listAllowUser.Add((int)RolesEnum.PM);
                listAllowUser.Add((int)RolesEnum.ADMIN);
                listAllowUser.Add((int)RolesEnum.Leader);

                if (listAllowUser.Contains(UserInfo.RoleID))
                {
                    if (TicketsEntityInfo.IsEstimates)
                    {
                        if (TicketsEntityInfo.Status == TicketsState.PM_Verify_Estimation)
                        {
                            this.btnEsPmTime.Visible = true;
                        }
                    }

                    if (TicketsEntityInfo.Status >= TicketsState.PM_Reviewed &&
                        TicketsEntityInfo.Status < TicketsState.Developing &&
                        TicketsEntityInfo.Status != TicketsState.Cancelled)
                    {
                        if (!(TicketsEntityInfo.Status == TicketsState.PM_Reviewed && TicketsEntityInfo.InitialTime > 0))
                        {
                            this.BtnUpdateSc.Visible = true;
                        }
                    }
                }
                #endregion

                #endregion
            }
        }

        #region commen method

        private string ShowProjectName(int tid)
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.SingleInstance, false, " Title ", " ASC ");

            request.ProjectID = tid;

            SearchProjectsResponse reponse = proApp.SearchProjects(request);

            return null != reponse ? reponse.ResultList[0].Title : "";
        }

        protected string ChangeStatus(object status, int ticketId)
        {
            return fbmHandler.GetSunnetStatusNameByStatus(status, ticketId);
        }

        private string ShowFormatTime(DateTime dt)
        {
            string dateStirng = "";

            if (dt > UtilFactory.Helpers.CommonHelper.GetDefaultMinDate())
            {
                dateStirng = dt.ToString("MM/dd/yyyy");
            }

            return dateStirng;
        }

        #endregion
    }
}