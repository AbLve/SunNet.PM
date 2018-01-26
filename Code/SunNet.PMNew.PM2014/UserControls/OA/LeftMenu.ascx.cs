using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.SealModel;
using SunNet.PMNew.PM2014.Codes;

namespace SunNet.PMNew.PM2014.UserControls.OA
{
    public partial class LeftMenu : BaseAscx
    {
        SealsApplication app = new SealsApplication();

        public int waitingForRespnseCount { get; set; }
        public override int ModuleID
        {
            get
            {
                return 11;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetMenuCount();
            }
        }

        private void SetMenuCount()
        {
            List<RequestStatus> status = new List<RequestStatus>();
            status.Add(RequestStatus.Draft);
            status.Add(RequestStatus.Denied);
            status.Add(RequestStatus.Submitted);
            status.Add(RequestStatus.PendingApproval);
            status.Add(RequestStatus.Approved);
            status.Add(RequestStatus.PendingProcess);
            status.Add(RequestStatus.Processed);

            waitingForRespnseCount = app.GetSealRequestsWaitingCount(UserInfo.UserID, status); ;

        }
    }
}