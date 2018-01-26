using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.CompanyModel;
using System.IO;

namespace SunNet.PMNew.Web.Sunnet.Clients
{
    public partial class TicketsDetail : BaseWebsitePage
    {
        #region declare
        TicketsApplication TicketApp = new TicketsApplication();
        int status = 0;
        bool isShowAddFeedback = true;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            #region initial

            this.AddTicket1.IsSunnet = "false";

            #endregion

            if (!IsPostBack)
            {
                int tid = QS("tid", 0);

                if (tid <= 0)
                {
                    this.ShowArgumentErrorMessageToClient();
                    return;
                }
                else
                {
                    TicketsEntity ticketEntity = TicketApp.GetTickets(tid);
                    HideControlByStatus(ticketEntity);//hidden control by ticket status

                    if (ticketEntity != null)
                    {
                        if (!CheckSecurity(ticketEntity))
                        {
                            this.ShowArgumentErrorMessageToClient();
                            return;
                        }
                        //by Lee
                        //if (ticketEntity.Status == TicketsState.Submitted)
                        //{
                        //    this.AddTicket1.IsEnable = false;
                        //    this.divBtnSave.Visible = true;
                        //}
                        //else
                        {
                            this.AddTicket1.IsEnable = true;
                        }
                        this.lilTicketTitle.Text = " : " + ticketEntity.Title;
                        this.FeedBacksList1.IsSunnet = false;
                        this.FeedBacksList1.TicketsEntityInfo = ticketEntity;
                        AddTicket1.TicketsEntityInfo = ticketEntity;
                        ClientTicketBaseInfo1.TicketsEntityInfo = ticketEntity;
                    }
                    else
                    {
                        this.ShowArgumentErrorMessageToClient();
                        return;
                    }

                }
            }
            InitCompanyInfo();
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            //对需要加载页上的所有其他控件的任务使用该事件。
            FeedBacksList1.Visible = isShowAddFeedback;
            base.OnLoadComplete(e);
        }

        private void HideControlByStatus(TicketsEntity entity)
        {
            status = (int)entity.Status;
            if (status == (int)TicketsState.Cancelled ||
                status == (int)TicketsState.Completed)
            {
                isShowAddFeedback = false;
                divAddRelation.Visible = false;
            }
            if (status == (int)TicketsState.Ready_For_Review)
            {
                divTopMenu.Visible = true;
            }
            if (this.divTopMenu.Visible == false)
            {
                this.divTopMenuFill.Visible = true;
            }
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                if (UserInfo.UserID != entity.CreatedBy)
                {
                    isShowAddFeedback = false;
                }
            }

        }

        private bool CheckSecurity(TicketsEntity info)
        {
            if (UserInfo.Role == RolesEnum.CLIENT)
            {
                if (UserInfo.CompanyID != info.CompanyID)
                    return false;
            }
            else if (UserInfo.Role != RolesEnum.ADMIN && UserInfo.Role != RolesEnum.PM && UserInfo.Role != RolesEnum.Sales)
                return false;
            return true;
        }

        private void InitCompanyInfo()
        {
            CompanyApplication comApp = new CompanyApplication();
            CompanysEntity company = comApp.GetCompany(UserInfo.CompanyID);
            if (UserInfo.CompanyID == 1)  //Sunnet 公司
            {
                ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                    , BuilderLogo(company.Logo));
            }
            else
            {
                CompanysEntity sunntCompanyEntity = comApp.GetCompany(1);//获取Sunnet公司

                if (company.Logo.IndexOf("logomain.jpg") >= 0) //没有上传Logo ，则显示 Sunnet 公司Logo
                {
                    ltLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
                   , BuilderLogo(sunntCompanyEntity.Logo));
                }
                else
                {
                    ltLogo.Text = BuilderLogo(company.Logo);

                    ltSunnetLogo.Text = string.Format("<a href=\"http://www.sunnet.us\" target=\"_blank\">{0}</a>"
          , BuilderLogo(sunntCompanyEntity.Logo));
                }
            }
        }

        private string BuilderLogo(string image)
        {
            string filename = Server.MapPath(image);
            if (File.Exists(filename))
            {
                return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", image);
            }
            else return string.Format("<img style=\"height:39px;width:126px;border-width:0px;\" src=\"{0}\"/>", "/images/logomain.jpg");
        }
    }
}
