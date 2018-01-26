using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.Web.Codes;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Entity.UserModel;
using SunNet.PMNew.Entity.ProjectModel;
using SunNet.PMNew.App;

namespace SunNet.PMNew.Web.UserControls
{
    public partial class ClientTicketBaseInfo : BaseAscx
    {
        public TicketsEntity TicketsEntityInfo { get; set; }
        ProjectApplication proApp = new ProjectApplication();
        protected FeedBackMessageHandler fbmHandler;
        UserApplication userApp = new UserApplication();

        protected void Page_Load(object sender, EventArgs e)
        {
            fbmHandler = new FeedBackMessageHandler(UserInfo);
            BindTicketBaseInfo();
            WorkFlow();
        }

        #region allow show statuss

        int[] UnderDevelopingStatus = {  (int)TicketsState.Testing_On_Client,   (int)TicketsState.Tested_Success_On_Client,
                                         (int)TicketsState.Tested_Fail_On_Client,  (int)TicketsState.Tested_Fail_On_Local,
                                         (int)TicketsState.Testing_On_Local,(int)TicketsState.Tested_Success_On_Local, 
                                         (int)TicketsState.Developing,    (int)TicketsState.PM_Deny,
                                         (int)TicketsState.PM_Reviewed   
                                      };

        int[] UnderEstimationStatus = { (int)TicketsState.PM_Verify_Estimation, (int)TicketsState.Waiting_Sales_Confirm,
                                        (int)TicketsState.Waiting_For_Estimation,(int)TicketsState.Estimation_Approved};

        #endregion

        #region common show

        private string ShowProjectName(int tid)
        {
            SearchProjectsRequest request = new SearchProjectsRequest(SearchProjectsType.SingleInstance, false, " Title ", " ASC ");

            request.ProjectID = tid;

            SearchProjectsResponse reponse = proApp.SearchProjects(request);

            return null != reponse ? reponse.ResultList[0].Title : "";
        }

        private void BindTicketBaseInfo()
        {
            if (null != TicketsEntityInfo)
            {
                if (TicketsEntityInfo.Status < TicketsState.Developing)
                {
                    this.lilEN.Text = TicketsEntityInfo.IsEstimates == true
                        ? "<span  style='Color:red;'>YES</span>" : "<span style='Color:red;'>NO</span>";
                }
                else
                {
                    this.lilEN.Text = TicketsEntityInfo.IsEstimates == true
                               ? "YES" : "NO";
                }
                this.lilIsInternal.Text = TicketsEntityInfo.IsInternal == true ? "IsInternal," : "";
                this.lilProjectName.Text = ShowProjectName(TicketsEntityInfo.ProjectID);
                this.lilTicketCode.Text = TicketsEntityInfo.TicketCode;
                this.lilTicketPriority.Text = Enum.GetName(typeof(PriorityState), (int)TicketsEntityInfo.Priority);
                this.lilTicketStatus.Text = GetClientStatusNameBySatisfyStatus((int)TicketsEntityInfo.Status, TicketsEntityInfo.TicketID).Replace("<span style='color:red;'>", "").Replace("</span>", "");
                this.lilCreateBy.Text = BaseWebsitePage.GetClientUserName(TicketsEntityInfo.CreatedBy);
                this.lilCreateTime.Text = ShowFormatTime(TicketsEntityInfo.CreatedOn);
                if (!TicketsEntityInfo.IsEstimates)
                {
                    this.trEstLast.Visible = false;
                }
                else
                {
                    this.lilFinalHours.Text = TicketsEntityInfo.Status < TicketsState.Estimation_Approved ? "0" : TicketsEntityInfo.FinalTime.ToString();
                }
            }
        }

        public string GetClientStatusNameBySatisfyStatus(int status, int TicketID)
        {
            return fbmHandler.GetClientStatusNameBySatisfyStatus(status, TicketID);
        }

        private string ShowFormatTime(DateTime dt)
        {
            string dateStirng = "";

            if (dt > Convert.ToDateTime("1753-01-01"))
            {
                dateStirng = dt.ToString("MM/dd/yyyy");
            }

            return dateStirng;
        }

        private void WorkFlow()
        {
            if (null != TicketsEntityInfo)
            {
                if (TicketsEntityInfo.Status > TicketsState.Submitted &&
                    TicketsEntityInfo.Status < TicketsState.Developing)
                {
                    if (TicketsEntityInfo.IsEstimates)
                    {
                        if (TicketsEntityInfo.Status == TicketsState.PM_Reviewed)
                        {
                            this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                        }
                        else if (TicketsEntityInfo.Status > TicketsState.PM_Reviewed &&
                                 TicketsEntityInfo.Status < TicketsState.Estimation_Fail)
                        {
                            this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                            this.tdEstimation.Attributes["class"] = "orangeBoxClient";
                            //this.lilEsStatus.Text = "<span style='color:black; font-size: 15px; '>(Waiting)</span>";
                        }
                        else if (TicketsEntityInfo.Status == TicketsState.Estimation_Fail)
                        {
                            this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                            this.tdEstimation.Attributes["class"] = "orangeBoxClient";
                            // this.lilEsStatus.Text = "<span style='color:black;'>Fail</span>";
                        }
                        else if (TicketsEntityInfo.Status == TicketsState.Estimation_Approved)
                        {
                            this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                            this.tdEstimation.Attributes["class"] = "greenBoxClient";
                            this.tdDeveloping.Attributes["class"] = "orangeBoxClient";

                            //this.lilEsStatus.Text = "<span style='color:black; font-size: 15px; '>(Pass)</span>";
                            //this.lilDevStatus.Text = "<span style='color:black; font-size: 15px; '>(Waiting)</span>";
                        }
                    }
                    else
                    {
                        if (TicketsEntityInfo.Status == TicketsState.PM_Reviewed)
                        {
                            this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                            this.tdEstimation.Attributes["class"] = "greenBoxClient";
                            this.tdDeveloping.Attributes["class"] = "orangeBoxClient";
                            //this.lilDevStatus.Text = "<span style='color:black; font-size: 15px;'>(Developing)</span>";
                        }
                    }

                }
                else if (TicketsEntityInfo.Status == TicketsState.Developing)
                {
                    this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                    this.tdEstimation.Attributes["class"] = "greenBoxClient";
                    this.tdDeveloping.Attributes["class"] = "orangeBoxClient";

                    // this.lilEsStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                    // this.lilDevStatus.Text = "<span style='color:black;font-size: 15px; '>(Developing)</span>";
                }
                else if (TicketsEntityInfo.Status > TicketsState.Developing &&
                         TicketsEntityInfo.Status < TicketsState.Ready_For_Review)
                {
                    this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                    this.tdEstimation.Attributes["class"] = "greenBoxClient";
                    this.tdDeveloping.Attributes["class"] = "greenBoxClient";
                    //this.lilEsStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                    // this.lilDevStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";

                    if (TicketsEntityInfo.Status == TicketsState.Testing_On_Local)
                    {
                        this.tdTesting.Attributes["class"] = "orangeBoxClient";
                        //  this.lilTestStatus.Text = "<span style='color:black;font-size: 15px;'>(Waiting)</span>";
                    }
                    else if (TicketsEntityInfo.Status > TicketsState.Testing_On_Local &&
                             TicketsEntityInfo.Status < TicketsState.Ready_For_Review)
                    {
                        this.tdTesting.Attributes["class"] = "orangeBoxClient";
                        // this.lilTestStatus.Text = "<span style='color:black;font-size: 15px;'>(Testing)</span>";
                    }
                }
                else if (TicketsEntityInfo.Status == TicketsState.Ready_For_Review)
                {
                    this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                    this.tdEstimation.Attributes["class"] = "greenBoxClient";
                    this.tdDeveloping.Attributes["class"] = "greenBoxClient";
                    this.tdTesting.Attributes["class"] = "greenBoxClient";
                    this.tdReadyfor_review.Attributes["class"] = "orangeBoxClient";

                    // this.lilClientStatus.Text = "<span style='color:black;font-size: 15px; '>(Waiting)</span>";
                    // this.lilTestStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                    // this.lilEsStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                    // this.lilDevStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                }
                else if (TicketsEntityInfo.Status == TicketsState.Not_Approved)
                {
                    this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                    this.tdEstimation.Attributes["class"] = "greenBoxClient";
                    this.tdDeveloping.Attributes["class"] = "greenBoxClient";
                    this.tdTesting.Attributes["class"] = "greenBoxClient";
                    this.tdReadyfor_review.Attributes["class"] = "orangeBoxClient";

                    //this.lilClientStatus.Text = "<span style='color:black;font-size: 15px;'>(Fail)</span>";
                    //this.lilTestStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                    //this.lilEsStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                    //this.lilDevStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                }
                else if (TicketsEntityInfo.Status == TicketsState.Completed)
                {
                    this.tdWaitPmReview.Attributes["class"] = "greenBoxClient";
                    this.tdEstimation.Attributes["class"] = "greenBoxClient";
                    this.tdDeveloping.Attributes["class"] = "greenBoxClient";
                    this.tdTesting.Attributes["class"] = "greenBoxClient";
                    this.tdReadyfor_review.Attributes["class"] = "greenBoxClient";
                    this.tdCompeleted.Attributes["class"] = "greenBoxClient";

                    //this.lilClientStatus.Text = "<span style='color:black;font-size: 15px;'>(Pass)</span>";
                    //this.lilTestStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                    //this.lilEsStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                    //this.lilDevStatus.Text = "<span style='color:black;font-size: 15px; '>(Pass)</span>";
                }
            }
        }

        #endregion
    }
}