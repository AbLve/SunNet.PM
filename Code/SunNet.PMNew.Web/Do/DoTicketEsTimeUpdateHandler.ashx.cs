using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SunNet.PMNew.App;
using SunNet.PMNew.Entity.TicketModel;
using SunNet.PMNew.Framework.Core;
using SunNet.PMNew.Framework.Utils.Providers;
using SunNet.PMNew.Framework.Utils;
using Newtonsoft.Json;

namespace SunNet.PMNew.Web.Do
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class DoTicketEsTimeUpdateHandler : DoBase, IHttpHandler
    {
        TicketsApplication TicketApp = new TicketsApplication();

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            try
            {
                if (IdentityContext.UserID <= 0)
                    return;

                #region get value

                string JsonString = context.Request["JsonString"];
                bool FirstUpdate = Convert.ToBoolean(context.Request["FirstUpdate"]);
                bool uType = bool.Parse(context.Request["type"]);
                int EsId = int.Parse(context.Request["EsId"]);
                int tid = int.Parse(context.Request["tid"]);

                #endregion

                IList<TicketEsTime> list = JsonConvert.DeserializeObject<IList<TicketEsTime>>(JsonString); //新加的估时数据
                int result = 0;
                bool HasError = false;
                bool UpdateSuc = false;
                if (EsId > 0)                                                           //是更新吗？
                {
                    TicketEsTime model = TicketApp.GetEsTimeModel(EsId);
                    model.DevAdjust = list[0].DevAdjust;
                    model.DocTime = list[0].DocTime;
                    model.GrapTime = list[0].GrapTime;
                    model.QaAdjust = list[0].QaAdjust;
                    model.Remark = list[0].Remark;
                    model.TotalTimes = list[0].TotalTimes;
                    model.TrainingTime = list[0].TrainingTime;
                    model.Week = list[0].Week;
                    UpdateSuc = TicketApp.UpdateEsTime(model);
                }
                else                                                  //添加的情况
                {
                    foreach (TicketEsTime item in list)
                    {
                        item.CreatedTime = DateTime.Now.Date;
                        item.EsByUserId = IdentityContext.UserID;
                        item.IsPM = uType;
                        item.Week = item.Week;
                        result = TicketApp.AddEsTime(item);
                        if (result < 0)
                        {
                            HasError = true;
                        }
                    }
                }
                if (uType)                                    //用于判断是否是PM估时，如果是PM              
                {
                    List<TicketEsTime> listPmEsTime = TicketApp.GetListEs(tid).FindAll(x => x.IsPM == true); //如果是PM就把所计算所有PM估时的总和 然后做为final time更新到tickets表。
                    decimal pmTime = listPmEsTime.Count > 0 ? listPmEsTime.Sum(x => x.TotalTimes) : 0;
                    TicketApp.UpdateEs(pmTime, tid, UserID, true);                                            //这里更新的是Es
                }
                else                                     //否则如果是Leader的情况会把第一次估时，也就是initial time写入到Ticket表中
                {
                    List<TicketEsTime> listLeaderEsTime = TicketApp.GetListEs(tid).FindAll(x => x.IsPM == false);
                    decimal InitialTime = listLeaderEsTime.Count > 0 ? listLeaderEsTime.Sum(x => x.TotalTimes) : 0;
                    TicketApp.UpdateEs(InitialTime, tid, UserID, false);
                }

                if ((result > 0 && !HasError) || UpdateSuc)
                {
                    if (UpdateSuc)
                    {
                        context.Response.Write("Update Es Time Success!");
                    }
                    else
                    {
                        context.Response.Write("Add Es Time Success! ");
                    }
                }
                else
                {
                    if (UpdateSuc)
                    {
                        context.Response.Write("Update Es Time Fail!");
                    }
                    else
                    {
                        context.Response.Write("Add Es Time Fail!");
                    }
                }

            }
            catch (Exception ex)
            {
                context.Response.Write("para error!");
                WebLogAgent.Write(string.Format("Error Ashx:DoTicketEsTimeUpdateHandler.ashx Messages:\r\n{0}", ex));
                return;
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
