using SunNet.PMNew.App;
using SunNet.PMNew.Entity.ProposalTrackerModel;
using SunNet.PMNew.Web.Do;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SunNet.PMNew.PM2014.Do
{
    /// <summary>
    /// DoGetddlProposal 的摘要说明
    /// </summary>
    public class DoGetddlProposal : DoBase, IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            HttpRequest request = context.Request;
            int projectID = QS(request.QueryString["projectID"], 0);
            if (projectID != 0)
            {
                ProposalTrackerApplication wrApp=new ProposalTrackerApplication();

                List<ProposalTrackerEntity> proposalTracker = wrApp.GetProposalTrackerByPid(projectID);

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("[");
                foreach (ProposalTrackerEntity Proposal in proposalTracker)
                {
                    stringBuilder.Append("{");
                    stringBuilder.AppendFormat("\"name\":\"{0}\",\"value\":\"{1}\"", Proposal.Title,
                        Proposal.ProposalTrackerID);
                    stringBuilder.Append("},");
                }
                response.Write(stringBuilder.ToString().TrimEnd(',') + "]");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}