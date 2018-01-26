using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.PM2014.Codes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SunNet.PMNew.PM2014.UserControls.Ticket
{
    public partial class Progress : BaseAscx
    {
        public List<ClientProgressState> orderedDisplayStates = new List<ClientProgressState>();

        public ClientProgressState CurrentState { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ltrlProcess.Text = GetProgressHTML();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayStates">有序的状态序列</param>
        /// <param name="currentState">当前的状态</param>
        /// <returns></returns>
        protected string GetProgressHTML()
        {
            if (orderedDisplayStates.Count != 0)
            {
                int currentPosition = orderedDisplayStates.FindIndex((r) => r == CurrentState);
                int statesCount = orderedDisplayStates.Count;
                string templatHTml = "</li><li class=\"processtext {1}\" >{2}</li><li class=\"roundicon {0}\">";
                const string roundCompleteCSS = "completed";
                const string textCompleteCSS = "completedtext";
                const string roundOngoingCSS = "ongoing";
                const string textOngoingCSS = "ongoingtext";
                const string roundUnfinishCSS = "nofinish";
                const string textNofinishCSS = "nofinishtext";
                StringBuilder stringBuilder = new StringBuilder();
                if (CurrentState == ClientProgressState.None)
                {
                    stringBuilder.AppendFormat("<li class=\"roundicon {0}\"></li>", roundUnfinishCSS);
                    for (int i = 0; i < statesCount; i++)
                    {
                        stringBuilder.Append(string.Format(templatHTml, roundUnfinishCSS, textNofinishCSS,
                   GetEnumName(orderedDisplayStates[i])));
                    }
                    return stringBuilder.ToString();
                }
                stringBuilder.AppendFormat("<li class=\"roundicon {0}\"></li>", roundCompleteCSS);
                int j = 0;
                for (; j < currentPosition; j++)
                {
                    ClientProgressState ticketsState = orderedDisplayStates[j];
                    stringBuilder.Append(string.Format(templatHTml, roundCompleteCSS, textCompleteCSS,
                         GetEnumName(ticketsState)));
                }

                if (j < statesCount)
                {

                    switch (CurrentState)
                    {
                        case ClientProgressState.Developing:
                        case ClientProgressState.Testing:
                        case ClientProgressState.Client_Confirm:
                            {
                                stringBuilder.Append(string.Format(templatHTml, roundOngoingCSS, textOngoingCSS,
                       GetEnumName(orderedDisplayStates[j])));
                            }
                            break;
                        default:
                            {
                                stringBuilder.Append(string.Format(templatHTml, roundCompleteCSS, textCompleteCSS,
                       GetEnumName(orderedDisplayStates[j])));
                            }
                            break;
                    }


                    j++;
                    for (; j < statesCount; j++)
                    {
                        stringBuilder.Append(string.Format(templatHTml, roundUnfinishCSS, textNofinishCSS,
                         GetEnumName(orderedDisplayStates[j])));
                    }
                }

                return stringBuilder.ToString();
            }
            else
            {
                return "<h1 style=\"color:red;\">parameters exception.</h1>";
            }
        }
    }
}