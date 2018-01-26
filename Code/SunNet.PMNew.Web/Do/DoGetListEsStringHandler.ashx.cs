using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using System.Text;
using SunNet.PMNew.Framework.Core;
namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoGetListEsStringHandler : IHttpHandler
    {
        #region declare
        TicketsApplication ticketApp = new TicketsApplication();
        UserApplication userApp = new UserApplication();
        #endregion
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            int tid = int.Parse(context.Request.Params["tid"]);

            List<TicketEsTime> list = ticketApp.GetListEs(tid);
            List<TicketEsTime> listEsUser = list.FindAll(x => x.IsPM == false);
            List<TicketEsTime> listPm = list.FindAll(x => x.IsPM == true);

            #region
            StringBuilder sb = new StringBuilder();
            sb.Append("<table id= 'esTable' width= '90%' border='1' align='center' cellpadding='5' cellspacing='0'  class='tabStyle'>");
            sb.Append("<tr class='owlistTitle'>");
            sb.Append("<th style='width: 40px;'>Week</th>");
            sb.Append("<th style='width: 40px;'>QA</th>");
            sb.Append("<th style='width: 40px;'>Dev</th>");
            sb.Append("<th style='width: 60px;'>Grap Time</th>");
            sb.Append("<th style='width: 60px;'>Doc Time</th>");
            sb.Append("<th style='width: 80px;'>Training Time</th>");
            sb.Append("<th style='width: 60px;'>Total</th>");
            sb.Append("<th style='width: 110px;'>Remark</th>");
            sb.Append("<th style='width: 60px;'>Create By</th>");
            sb.Append("</tr>");
            sb.Append("{row}");
            sb.Append("</table>");
            #endregion

            StringBuilder FillData = new StringBuilder();
            int rows = 1;
            int totalCount = 0;
            if (listPm.Count <= 0)
            {
                if (listEsUser.Count <= 0)
                {
                    sb.Replace("{row}", NotDataRow());
                }
                else
                {
                    totalCount = listEsUser.Count;
                    TicketEsTime model = new TicketEsTime();
                    model.TotalTimes = listEsUser.Sum(x => x.TotalTimes);
                    model.DevAdjust = listEsUser.Sum(x => x.DevAdjust);
                    model.DocTime = listEsUser.Sum(x => x.DocTime);
                    model.GrapTime = listEsUser.Sum(x => x.GrapTime);
                    model.QaAdjust = listEsUser.Sum(x => x.QaAdjust);
                    model.TrainingTime = listEsUser.Sum(x => x.TrainingTime);
                    model.EsByUserId = 0;
                    model.Week = "Total:";
                    listEsUser.Add(model);
                    foreach (TicketEsTime item in listEsUser)
                    {
                        rows++;
                        FillData.Append(DataRowTemplete().Replace("{EsID}", item.EsID.ToString())
                                                         .Replace("{Week}", item.Week)
                                                         .Replace("{QaAdjust}", item.QaAdjust.ToString())
                                                         .Replace("{DevAdjust}", item.DevAdjust.ToString())
                                                         .Replace("{GrapTime}", item.GrapTime.ToString())
                                                         .Replace("{DocTime}", item.DocTime.ToString())
                                                         .Replace("{TrainingTime}", item.TrainingTime.ToString())
                                                         .Replace("{TotalTimes}", item.TotalTimes.ToString())
                                                         .Replace("{Remark}", item.Remark)
                                                         .Replace("{CreateUser}", GetUserNameByCreateID(item.EsByUserId))
                                                         .Replace("{class}", rows % 2 == 0 ? "bg1" : "bg2")
                                                          );
                    }
                    sb.Replace("{row}", FillData.ToString());
                }

            }
            else
            {
                totalCount = listPm.Count;
                TicketEsTime model = new TicketEsTime();
                model.TotalTimes = listPm.Sum(x => x.TotalTimes);
                model.DevAdjust = listPm.Sum(x => x.DevAdjust);
                model.DocTime = listPm.Sum(x => x.DocTime);
                model.GrapTime = listPm.Sum(x => x.GrapTime);
                model.QaAdjust = listPm.Sum(x => x.QaAdjust);
                model.TrainingTime = listPm.Sum(x => x.TrainingTime);
                model.EsByUserId = 0;
                model.Week = "Total:";
                listPm.Add(model);
                foreach (TicketEsTime item in listPm)
                {
                    rows++;
                    FillData.Append(DataRowTemplete().Replace("{EsID}", item.EsID.ToString())
                                                     .Replace("{Week}", item.Week)
                                                     .Replace("{QaAdjust}", item.QaAdjust.ToString())
                                                     .Replace("{DevAdjust}", item.DevAdjust.ToString())
                                                     .Replace("{GrapTime}", item.GrapTime.ToString())
                                                     .Replace("{DocTime}", item.DocTime.ToString())
                                                     .Replace("{TrainingTime}", item.TrainingTime.ToString())
                                                     .Replace("{TotalTimes}", item.TotalTimes.ToString())
                                                     .Replace("{Remark}", item.Remark)
                                                     .Replace("{CreateUser}", GetUserNameByCreateID(item.EsByUserId))
                                                     .Replace("{class}", rows % 2 == 0 ? "bg1" : "bg2")
                                                      );
                }
                sb.Replace("{row}", FillData.ToString());
            }
            context.Response.Write(string.Format("{0}{1}", totalCount.ToString().PadLeft(2, '0'), sb));
        }

        #region commen method

        protected string GetUserNameByCreateID(int cid)
        {
            string userName = userApp.GetLastNameFirstName(cid);
            return userName.Length == 0 ? "" : userName;
        }

        protected string DataRowTemplete()
        {

            StringBuilder sbDataRow = new StringBuilder();
            sbDataRow.Append("<tr onclick='ClickRow(this);return false;'  ondblclick='UpdateValue(this); return false;'  class='{class}' id='{EsID}'>");
            sbDataRow.Append("<td>{Week}</td>");
            sbDataRow.Append("<td>{QaAdjust}</td>");
            sbDataRow.Append("<td>{DevAdjust}</td>");
            sbDataRow.Append("<td>{GrapTime}</td>");
            sbDataRow.Append("<td>{DocTime}</td>");
            sbDataRow.Append("<td>{TrainingTime}</td>");
            sbDataRow.Append("<td>{TotalTimes}</td>");
            sbDataRow.Append("<td>{Remark}</td>");
            sbDataRow.Append("<td>{CreateUser}</td>");
            sbDataRow.Append("</tr>");
            return sbDataRow.ToString(); ;
        }

        protected string NotDataRow()
        {
            StringBuilder NoDataRow = new StringBuilder();
            NoDataRow.Append("<tr runat='server' class='noTicket' id='trNoTickets' visible='trNoTickets'>");
            NoDataRow.Append("<th colspan='9'>");
            NoDataRow.Append("<span style='color: Red;'>&nbsp; No records</span>");
            NoDataRow.Append("</th>");
            NoDataRow.Append("</tr>");
            return NoDataRow.ToString(); ;
        }

        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
