using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.App;
using System.Text;
namespace SunNet.PMNew.Web.UserControls
{
    public partial class ChangeStatus : BaseAscx
    {
        #region declare
        Dictionary<int, string> dictionary = new Dictionary<int, string>();
        int status = 0;
        bool IsEst = true;
        bool IsInternal = true;
        decimal FinalEsTime = 0;
        int EsUserID = 0;
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApplication = new UserApplication();
        RolesEnum ticketCreatedUserRole;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ChangeStatusTicketsEntityInfo == null)//validate
            {
                return;
            }

            #region set value

            status = Convert.ToInt32(ChangeStatusTicketsEntityInfo.Status);
            IsEst = ChangeStatusTicketsEntityInfo.IsEstimates;
            IsInternal = ChangeStatusTicketsEntityInfo.IsInternal;
            FinalEsTime = ChangeStatusTicketsEntityInfo.FinalTime;
            EsUserID = ChangeStatusTicketsEntityInfo.EsUserID;
            ticketCreatedUserRole = userApplication.GetUser(ChangeStatusTicketsEntityInfo.CreatedBy).Role;
            #endregion

            if (UserInfo.Role != RolesEnum.Sales && status != (int)TicketsState.Completed && IsTicketUser())
            {
                ShowStatusByUserRoleAndUserType();
            }
        }

        private bool IsTicketUser()
        {
            TicketsApplication ticketsApplication = new TicketsApplication();
            return ticketsApplication.IsTicketUser(ChangeStatusTicketsEntityInfo.ID,
                UserInfo.ID);
        }

        private string GetButtonStringList(int[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int item in array)
            {
                if (item == (int)TicketsState.Ready_For_Review || item == (int)TicketsState.Cancelled)
                {
                    sb.Append(string.Format("<input  type='button' class='tickettopBtn'  value='{0}'  onclick='updateStatusConfirm({1},{2});return false;' />",
                        Enum.GetName(typeof(TicketsState), item).Replace('_', ' '), item, "true"));
                }
                else
                {
                    sb.Append(string.Format("<input  type='button' class='tickettopBtn'  value='{0}'  onclick='updateStatusConfirm({1},{2});return false;' />",
                        Enum.GetName(typeof(TicketsState), item).Replace('_', ' '), item, "false"));
                }

            }
            return sb.ToString();
        }

        private void ShowStatusByUserRoleAndUserType()
        {
            if (UserInfo.Role == RolesEnum.DEV || UserInfo.Role == RolesEnum.Leader
                || UserInfo.Role == RolesEnum.Contactor)
            {
                if (status == (int)TicketsState.PM_Deny)
                {
                    this.lilBtnList.Text = GetButtonStringList(SunnetDevStatus);
                }
                else
                {
                    if (IsEst)
                    {
                        if (((status == (int)TicketsState.PM_Reviewed && FinalEsTime > 0) ||
                              status == (int)TicketsState.Estimation_Approved ||
                             (status >= (int)TicketsState.Developing && status < (int)TicketsState.Tested_Success_On_Client)) &&
                              status != (int)TicketsState.Testing_On_Local && status != (int)TicketsState.Testing_On_Client)
                        {
                            this.lilBtnList.Text = GetButtonStringList(SunnetDevSecondStatus);
                        }
                        else if (status == (int)TicketsState.PM_Reviewed && UserInfo.UserID == ChangeStatusTicketsEntityInfo.CreatedBy
                            && UserInfo.Role == RolesEnum.Leader)
                        {
                            if (HasDevOrQaUnderTicket(ChangeStatusTicketsEntityInfo))
                            {
                                this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepWithEs);
                            }
                        }
                        else if (status == (int)TicketsState.Waiting_For_Estimation && EsUserID == UserInfo.UserID)
                        {
                            if (ChangeStatusTicketsEntityInfo.InitialTime > 0)
                            {
                                this.lilBtnList.Text = GetButtonStringList(SunnetWaitEsStatus);
                            }

                        }
                    }
                    else
                    {
                        if ((status == (int)TicketsState.PM_Reviewed ||
                            (status >= (int)TicketsState.Developing && status < (int)TicketsState.Tested_Success_On_Client)) &&
                             status != (int)TicketsState.Testing_On_Local && status != (int)TicketsState.Testing_On_Client)
                        {
                            this.lilBtnList.Text = GetButtonStringList(SunnetDevSecondStatus);
                        }
                    }
                }

            }
            else if (UserInfo.Role == RolesEnum.QA)
            {
                if (status == (int)TicketsState.Testing_On_Client || status == (int)TicketsState.Testing_On_Local || status == (int)TicketsState.Waiting_For_Estimation)
                {

                    if (status == (int)TicketsState.Testing_On_Local)
                    {
                        this.lilBtnList.Text = GetButtonStringList(SunnetQaStatus);
                    }
                    else
                    {
                        if (status == (int)TicketsState.Waiting_For_Estimation && EsUserID == UserInfo.UserID)
                        {
                            if (ChangeStatusTicketsEntityInfo.InitialTime > 0)
                            {
                                this.lilBtnList.Text = GetButtonStringList(SunnetWaitEsStatus);
                            }
                        }
                        else
                        {
                            this.lilBtnList.Text = GetButtonStringList(SunnetQaSecondStatus);
                        }
                    }
                }
            }
            else if (UserInfo.Role == RolesEnum.PM || UserInfo.Role == RolesEnum.ADMIN)
            {
                if (status == (int)TicketsState.Submitted || status == (int)TicketsState.PM_Reviewed)
                {
                    if (IsInternal)
                    {
                        if (IsEst)
                        {
                            if (status == (int)TicketsState.PM_Reviewed)
                            {
                                if (ChangeStatusTicketsEntityInfo.EsUserID > 0)
                                {
                                    this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepWithEs);
                                }

                            }
                            else
                            {
                                if (HasDevOrQaUnderTicket(ChangeStatusTicketsEntityInfo))
                                {
                                    this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStep);
                                }
                                else
                                {
                                    this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepNotAssignUser);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!(status == (int)TicketsState.PM_Reviewed && FinalEsTime > 0))
                        {
                            if (IsEst)
                            {
                                if (status == (int)TicketsState.PM_Reviewed)
                                {
                                    if (ChangeStatusTicketsEntityInfo.EsUserID > 0)
                                    {
                                        this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepWithEs);
                                    }
                                }
                                else
                                {
                                    if (HasDevOrQaUnderTicket(ChangeStatusTicketsEntityInfo))
                                    {
                                        this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStep);
                                    }
                                    else
                                    {
                                        this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepNotAssignUser);
                                    }

                                }
                            }
                            else
                            {
                                if (HasDevOrQaUnderTicket(ChangeStatusTicketsEntityInfo))
                                {
                                    if (ChangeStatusTicketsEntityInfo.Status < TicketsState.PM_Reviewed)
                                    {
                                        this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepWithNotEstAndInternal);
                                    }
                                }
                                else
                                {
                                    this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusFirstStepNotAssignUser);
                                }
                            }
                        }
                    }
                }
                else if (status == (int)TicketsState.Waiting_For_Estimation && EsUserID == UserInfo.UserID)
                {
                    if (ChangeStatusTicketsEntityInfo.InitialTime > 0)
                    {
                        this.lilBtnList.Text = GetButtonStringList(SunnetWaitEsStatus);
                    }
                }
                else if (status == (int)TicketsState.PM_Verify_Estimation)
                {
                    if (ChangeStatusTicketsEntityInfo.FinalTime > 0 &&
                        ticketApp.tickeExistSalerUser(ChangeStatusTicketsEntityInfo.TicketID))
                    {

                        this.lilBtnList.Text = GetButtonStringList(SunnetWaitSalerConfirm);
                    }
                }
                else if (status >= (int)TicketsState.Testing_On_Local && status < (int)TicketsState.Ready_For_Review &&
                       status != (int)TicketsState.Tested_Fail_On_Client && status != (int)TicketsState.Tested_Fail_On_Local &&
                       status != (int)TicketsState.PM_Deny)
                {
                    if (IsInternal)
                    {
                        if (ticketCreatedUserRole != RolesEnum.Supervisor)
                        {
                            this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusThirdStepWithInternal);
                        }
                        else
                        {
                            this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusThirdStep);
                        }
                    }
                    else
                    {
                        this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusThirdStep);
                    }
                }
                else if (status == (int)TicketsState.Not_Approved)
                {
                    this.lilBtnList.Text = GetButtonStringList(SunnetPMAndAdminStatusWithClientNotApprove);
                }
            }
            else if (UserInfo.Role == RolesEnum.Supervisor)
            {
                if (status == (int)TicketsState.Ready_For_Review && ChangeStatusTicketsEntityInfo.CreatedBy == UserInfo.ID)
                {
                    this.lilBtnList.Text = GetButtonStringList(SunnetSupervisorStatus);
                }
            }
        }

        private Dictionary<int, string> GetDicByUserRole(int[] allowShowStatus, bool IsContain)
        {
            foreach (int value in Enum.GetValues(typeof(TicketsState)))
            {
                if (IsContain)
                {
                    if (allowShowStatus.Contains(value))
                    {
                        dictionary.Add(value, Enum.GetName(typeof(TicketsState), value).Replace('_', ' '));
                    }
                }
                else
                {
                    if (!allowShowStatus.Contains(value))
                    {
                        dictionary.Add(value, Enum.GetName(typeof(TicketsState), value).Replace('_', ' '));
                    }
                }

            }
            return dictionary;
        }

        public bool HasDevOrQaUnderTicket(TicketsEntity entity)
        {
            if (null == entity || entity.TicketID <= 0) return false;
            List<TicketUsersEntity> listUser = ticketApp.GetListUsersByTicketId(entity.TicketID);
            if (listUser == null || listUser.Count <= 0) return false;
            if (listUser.FindAll(x => x.Type == TicketUsersType.Dev).Count > 0 && listUser.FindAll(x => x.Type == TicketUsersType.QA).Count > 0)
                return true;
            return false;
        }

        #region // status
        int[] pmAllowStatus = {   (int)TicketsState.Submitted , (int)TicketsState.PM_Reviewed ,
                                  (int)TicketsState.Not_Approved , (int)TicketsState.Testing_On_Local , 
                                  (int)TicketsState.Testing_On_Client , (int)TicketsState.Tested_Success_On_Local,
                                  (int)TicketsState.Tested_Success_On_Client   
                               };

        #region est
        int[] SunnetPMAndAdminStatusFirstStepNotAssignUser = { (int)TicketsState.Cancelled };
        int[] SunnetPMAndAdminStatusFirstStep = { (int)TicketsState.Cancelled, (int)TicketsState.PM_Reviewed };
        int[] SunnetPMAndAdminStatusFirstStepWithEs = { (int)TicketsState.Waiting_For_Estimation };
        int[] SunnetPMAndAdminStatusFirstStepWithNotEst = { (int)TicketsState.Cancelled, (int)TicketsState.PM_Reviewed };
        int[] SunnetPMAndAdminStatusFirstStepWithNotEstAndInternal = { (int)TicketsState.Cancelled, (int)TicketsState.PM_Reviewed };
        int[] SunnetPMAndAdminStatusSecondStep = { (int)TicketsState.Waiting_Sales_Confirm, 
                                                   (int)TicketsState.Waiting_For_Estimation };

        #endregion

        int[] SunnetWaitSalerConfirm = { (int)TicketsState.Waiting_Sales_Confirm };
        int[] SunnetPMAndAdminStatus = { (int)TicketsState.Draft, (int)TicketsState.Submitted };
        int[] SunnetPMAndAdminStatusThirdStep = { (int)TicketsState.PM_Deny, (int)TicketsState.Ready_For_Review };
        int[] SunnetPMAndAdminStatusThirdStepWithInternal = { (int)TicketsState.PM_Deny, (int)TicketsState.Completed };
        int[] SunnetPMAndAdminStatusWithClientNotApprove = { (int)TicketsState.PM_Reviewed };
        int[] SunnetSupervisorStatus = { (int)TicketsState.Completed, (int)TicketsState.Not_Approved };

        #region commen status
        int[] SunnetWaitEsStatus = { (int)TicketsState.PM_Verify_Estimation };
        #endregion

        #region dev status
        int[] SunnetDevStatus = { (int)TicketsState.Developing, (int)TicketsState.Testing_On_Local };
        int[] SunnetDevSecondStatus = { (int)TicketsState.Developing, (int)TicketsState.Testing_On_Local, (int)TicketsState.Testing_On_Client };
        #endregion

        #region qa status
        int[] SunnetQaStatus = { (int)TicketsState.Tested_Fail_On_Local, (int)TicketsState.Tested_Success_On_Local };
        int[] SunnetQaSecondStatus = { (int)TicketsState.Tested_Fail_On_Client, (int)TicketsState.Tested_Success_On_Client };
        #endregion

        #endregion

        #region public attribute
        public TicketsEntity ChangeStatusTicketsEntityInfo { get; set; }
        #endregion

    }
}